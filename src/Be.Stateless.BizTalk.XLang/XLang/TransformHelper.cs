#region Copyright & License

// Copyright © 2012 - 2020 François Chabot
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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;
using Be.Stateless.BizTalk.Runtime.Caching;
using Be.Stateless.Xml.Extensions;
using Be.Stateless.Xml.Xsl;
using log4net;
using Microsoft.BizTalk.Streaming;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.Core;
using XsltArgumentList = Be.Stateless.Xml.Xsl.XsltArgumentList;

namespace BizTalk.Factory.XLang
{
	/// <summary>
	/// Helper class that allows to easily use an <see cref="XslCompiledTransform"/> from within an orchestration XLang
	/// expression shape.
	/// </summary>
	/// <remarks>
	/// The implementation is directly inspired by <see
	/// href="http://blogs.msdn.com/b/paolos/archive/2010/01/29/how-to-boost-message-transformations-using-the-xslcompiledtransform-class.aspx">How
	/// To Boost Message Transformations Using the XslCompiledTransform class</see> and <see
	/// href="http://blogs.msdn.com/b/paolos/archive/2010/04/08/how-to-boost-message-transformations-using-the-xslcompiledtransform-class-extended.aspx">How
	/// To Boost Message Transformations Using the XslCompiledTransform class Extended</see>.
	/// </remarks>
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	public static class TransformHelper
	{
		/// <summary>
		/// Applies the XSL transformation specified by <paramref name="map"/> to the specified <paramref name="message"/>.
		/// </summary>
		/// <remarks>
		/// This method assumes only the first part of a multi-part <paramref name="message"/> message has to be transformed and
		/// creates an output message with a single part named "Main".
		/// </remarks>
		/// <param name="message">
		/// The <see cref="XLANGMessage"/> to be transformed.
		/// </param>
		/// <param name="map">
		/// The type of the BizTalk map class containing the transform to apply.
		/// </param>
		/// <returns>
		/// The transformed message with the result in the first part (at index 0).
		/// </returns>
		public static XLANGMessage Transform(XLANGMessage message, Type map)
		{
			if (message == null) throw new ArgumentNullException(nameof(message));
			return Transform(new MessageCollection { message }, map);
		}

		/// <summary>
		/// Applies the XSL transformation specified by <paramref name="map"/> to the specified <paramref name="message"/>.
		/// </summary>
		/// <remarks>
		/// This method assumes only the first part of a multi-part <paramref name="message"/> message has to be transformed and
		/// creates an output message with a single part named "Main".
		/// </remarks>
		/// <param name="message">
		/// The <see cref="XLANGMessage"/> to be transformed.
		/// </param>
		/// <param name="map">
		/// The type of the BizTalk map class containing the transform to apply.
		/// </param>
		/// <param name="arguments">
		/// The arguments to pass to the XSL transformation.
		/// </param>
		/// <returns>
		/// The transformed message with the result in the first part (at index 0).
		/// </returns>
		public static XLANGMessage Transform(XLANGMessage message, Type map, params XsltArgument[] arguments)
		{
			if (message == null) throw new ArgumentNullException(nameof(message));
			return Transform(new MessageCollection { message }, map, arguments);
		}

		/// <summary>
		/// Applies the XSL transformation specified by <paramref name="map"/> to the specified <paramref name="messages"/>.
		/// </summary>
		/// <remarks>
		/// This method assumes only the first part of any multi-part <paramref name="messages"/> message has to be transformed
		/// and creates an output message with a single part named "Main".
		/// </remarks>
		/// <param name="messages">
		/// The <see cref="MessageCollection"/> to be transformed.
		/// </param>
		/// <param name="map">
		/// The type of the BizTalk map class containing the transform to apply.
		/// </param>
		/// <returns>
		/// The transformed message with the result in the first part (at index 0).
		/// </returns>
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public static XLANGMessage Transform(MessageCollection messages, Type map)
		{
			return Transform(messages, map, new XsltArgumentList());
		}

		/// <summary>
		/// Applies the XSL transformation specified by <paramref name="map"/> to the specified <paramref name="messages"/>.
		/// </summary>
		/// <param name="messages">
		/// The <see cref="MessageCollection"/> to be transformed.
		/// </param>
		/// <param name="map">
		/// The type of the BizTalk map class containing the transform to apply.
		/// </param>
		/// <param name="arguments">
		/// The arguments to pass to the XSL transformation.
		/// </param>
		/// <returns>
		/// The transformed message with the result in the first part (at index 0).
		/// </returns>
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Public API.")]
		public static XLANGMessage Transform(MessageCollection messages, Type map, params XsltArgument[] arguments)
		{
			return Transform(messages, map, new XsltArgumentList(arguments));
		}

		private static XLANGMessage Transform(MessageCollection messages, Type map, System.Xml.Xsl.XsltArgumentList arguments)
		{
			if (messages == null) throw new ArgumentNullException(nameof(messages));
			if (messages.Count == 0) throw new ArgumentException("MessageCollection is empty.", nameof(messages));
			if (map == null) throw new ArgumentNullException(nameof(map));
			using (messages)
			{
				var resultContent = Transform((XmlReader) messages, map, arguments);
				var resultMessage = Message.Create(Service.RootService.XlangStore.OwningContext, resultContent);
				return resultMessage;
			}
		}

		[SuppressMessage("ReSharper", "AssignNullToNotNullAttribute")]
		private static Stream Transform(XmlReader reader, Type map, System.Xml.Xsl.XsltArgumentList arguments)
		{
			if (_logger.IsDebugEnabled) _logger.DebugFormat("About to execute transform '{0}'.", map.AssemblyQualifiedName);
			var transformDescriptor = XsltCache.Instance[map];
			using (reader)
			{
				var outputStream = new VirtualStream(DEFAULT_BUFFER_SIZE, DEFAULT_THRESHOLD_SIZE);
				var writerSettings = transformDescriptor.XslCompiledTransform.OutputSettings.Override(
					s => {
						s.CloseOutput = false;
						s.Encoding = Encoding.UTF8;
					});
				using (var writer = XmlWriter.Create(outputStream, writerSettings))
				{
					if (_logger.IsDebugEnabled) _logger.DebugFormat("Executing transform '{0}'.", map.AssemblyQualifiedName);
					var xsltArguments = transformDescriptor.Arguments.Union(arguments);
					transformDescriptor.XslCompiledTransform.Transform(reader, xsltArguments, writer);
				}
				outputStream.Seek(0, SeekOrigin.Begin);
				return outputStream;
			}
		}

		private const int DEFAULT_BUFFER_SIZE = 10 * 1024; //10 KB
		private const int DEFAULT_THRESHOLD_SIZE = 1024 * 1024; //1 MB
		private static readonly ILog _logger = LogManager.GetLogger(typeof(TransformHelper));
	}
}
