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
using System.IO;
using System.Linq;
using Be.Stateless.BizTalk.Message;
using Be.Stateless.Linq.Extensions;
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
		/// Retrieves the context of an <see cref="XLANGMessage"/> message.
		/// </summary>
		/// <remarks>
		/// Iterate over all segments in the current orchestration in search of the specified message object and return a
		/// collection of context properties.
		/// </remarks>
		/// <param name="message">
		/// The message whose context has to be retrieved.
		/// </param>
		/// <returns>
		/// The context of the <see cref="XLANGMessage"/> <paramref name="message"/>.
		/// </returns>
		/// <seealso href="https://maximelabelle.wordpress.com/2011/01/07/retrieving-the-context-of-a-biztalk-message-from-an-orchestration/">Retrieving the Context of a BizTalk Message from an Orchestration</seealso>
		/// <seealso href="https://tsabar.wordpress.com/2009/12/02/enumerating-context-properties/">Enumerating context properties</seealso>
		public static XmlQNameTable GetContext(this XLANGMessage message)
		{
			return message is XMessage xm
				? new(xm.GetContentProperties())
				: new();
		}

		public static string ToXml(this XmlQNameTable context)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			return MessageContextSerializer.Serialize(
				serializeProperty => context.GetEnumerator().Cast<DictionaryEntry>().Select(
					de => {
						var qn = (XmlQName) de.Key;
						return serializeProperty(qn.Name, qn.Namespace, de.Value, false);
					}));
		}
	}
}
