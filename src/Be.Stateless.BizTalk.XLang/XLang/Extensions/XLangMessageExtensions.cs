#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Be.Stateless.Extensions;
using Be.Stateless.Linq.Extensions;
using Be.Stateless.Reflection;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.Core;

namespace BizTalk.Factory.XLang.Extensions
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	public static class XLangMessageExtensions
	{
		public static Stream AsStream(this XLANGMessage message)
		{
			if (message == null) throw new ArgumentNullException(nameof(message));
			return message[0].AsStream();
		}

		/// <summary>
		/// </summary>
		/// <remarks>
		/// Iterate over all segments in the current orchestration in search of the specified message object and return a
		/// collection of context properties.
		/// </remarks>
		/// <param name="message"></param>
		/// <returns></returns>
		/// <seealso href="https://maximelabelle.wordpress.com/2011/01/07/retrieving-the-context-of-a-biztalk-message-from-an-orchestration/">Retrieving the Context of a BizTalk Message from an Orchestration</seealso>
		/// <seealso href="https://tsabar.wordpress.com/2009/12/02/enumerating-context-properties/">Enumerating context properties</seealso>
		public static XmlQNameTable GetContext(this XLANGMessage message)
		{
			Hashtable contextProperties = null;
			try
			{
				var xm = message as XMessage;
				if (xm == null)
				{
					if (message is MessageWrapperForUserCode mw) xm = (XMessage) Reflector.InvokeMethod(mw, "Unwrap");
				}

				if (xm != null) contextProperties = xm.GetContextProperties();
			}
			catch (Exception exception) when (!exception.IsFatal()) { }

			return new XmlQNameTable(contextProperties ?? new Hashtable());
		}

		public static string ToXml(this XmlQNameTable context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			// cache xmlns while constructing xml info set...
			var nsCache = new XmlDictionary();
			var xmlDocument = new XElement(
				"context",
				context.GetEnumerator().Cast<DictionaryEntry>().Select(
					de => {
						var qn = (XmlQName) de.Key;
						// give each property element a name of 'p' and store its actual name inside the 'n' attribute, which avoids
						// the cost of the name.IsValidQName() check for each of them as the name could be an xpath expression in the
						// case of a distinguished property
						return qn.Name.IndexOf("password", StringComparison.OrdinalIgnoreCase) > -1
							? null
							: new XElement(
								(XNamespace) nsCache.Add(qn.Namespace).Value + "p",
								new XAttribute("n", qn.Name),
								de.Value);
					}));

			// ... and declare/alias all of them at the root element level to minimize xml string size
			for (var i = 0; nsCache.TryLookup(i, out var xds); i++)
			{
				xmlDocument.Add(new XAttribute(XNamespace.Xmlns + "s" + xds.Key.ToString(CultureInfo.InvariantCulture), xds.Value));
			}

			return xmlDocument.ToString(SaveOptions.DisableFormatting);
		}
	}
}
