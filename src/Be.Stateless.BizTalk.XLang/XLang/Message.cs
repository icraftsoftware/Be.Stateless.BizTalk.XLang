#region Copyright & License

// Copyright © 2012 - 2022 François Chabot
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
using System.Xml;
using Microsoft.BizTalk.DefaultPipelines;
using Microsoft.BizTalk.XLANGs.BTXEngine;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.Core;
using Microsoft.XLANGs.Pipeline;

namespace Be.Stateless.BizTalk.XLang
{
	[Serializable]
	public sealed class Message : BTXMessage
	{
		public static XLANGMessage Create(Context context, Stream content)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			var message = new Message(context, content);
			return message.GetMessageWrapperForUserCode();
		}

		public static XLANGMessage Create(Context context, XmlDocument content)
		{
			if (context == null) throw new ArgumentNullException(nameof(context));
			var message = new Message(context, content);
			return message.GetMessageWrapperForUserCode();
		}

		/// <summary>
		/// Promotes the <see cref="BTS.MessageType"/> of an untyped &#8212;typically <see cref="XmlDocument"/>&#8212; <see
		/// cref="XLANGMessage"/> <paramref name="message"/> from within an orchestration.
		/// </summary>
		/// <param name="message">
		/// The untyped <see cref="XLANGMessage"/> message whose <see cref="BTS.MessageType"/> needs to be promoted.
		/// </param>
		/// <remarks>
		/// <para>
		/// Messages sent to the message box using a direct send port are generally subscribed to by their destination
		/// orchestrations or ports using a filter based on their message type. An untyped <paramref name="message"/> sent to the
		/// message box through a direct send port, however, has by default no promoted properties, hence no <see
		/// cref="BTS.MessageType"/>, and will generally end up with a <i>routing failure</i> error.
		/// </para>
		/// <para>
		/// The only property really needing to be promoted in this case is the <see cref="BTS.MessageType"/>; but within an
		/// orchestration <see cref="BTS.MessageType"/> is <b>readonly</b>. To work around this issue entails invoking the <see
		/// cref="XMLReceive"/> pipeline within the orchestration, as it promotes a series of properties including in particular
		/// the <see cref="BTS.MessageType"/>.
		/// </para>
		/// <para>
		/// Invoking a pipeline within an orchestration however requires an atomic scope, which is not necessary in our case and
		/// be avoided by calling this helper method from within a message assignment or construction shape.
		/// </para>
		/// <para>
		/// There is one last "<i>gotcha</i>". <see cref="XmlDocument"/> messages sent to message box using a direct send port
		/// will still drop their context and consequently their promoted <see cref="BTS.MessageType"/> property. A correlation
		/// set including this very same <see cref="BTS.MessageType"/> property therefore still need to be defined and
		/// initialized from within the orchestration.
		/// </para>
		/// </remarks>
		/// <seealso href="http://ronaldlokers.blogspot.com/2012/04/promoting-messagetype-property-on.html">Promoting MessageType property on untyped messages</seealso>
		public static void PromoteMessageType(XLANGMessage message)
		{
			var outputMessages = XLANGPipelineManager.ExecuteReceivePipeline(typeof(XMLReceive), message);
			outputMessages.MoveNext();
			outputMessages.GetCurrent(message);
		}

		private Message(Context context, object content) : base("BizTalkFactoryXLangMessage", context)
		{
			context.RefMessage(this);
			AddPart(string.Empty, "Body");
			this[0].LoadFrom(content);
		}
	}
}

namespace BizTalk.Factory.XLang
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[Obsolete("Use class in Be.Stateless.BizTalk.XLang instead.")]
	public static class Message
	{
		public static XLANGMessage Create(Context context, Stream content)
		{
			return Be.Stateless.BizTalk.XLang.Message.Create(context, content);
		}

		public static XLANGMessage Create(Context context, XmlDocument content)
		{
			return Be.Stateless.BizTalk.XLang.Message.Create(context, content);
		}
	}

	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[Obsolete("Use Be.Stateless.BizTalk.XLang.Message.PromoteMessageType instead.")]
	public static class MessageHelper
	{
		public static void PromoteMessageType(XLANGMessage message)
		{
			Be.Stateless.BizTalk.XLang.Message.PromoteMessageType(message);
		}
	}
}
