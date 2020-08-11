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
using System.Xml;
using Microsoft.BizTalk.XLANGs.BTXEngine;
using Microsoft.XLANGs.BaseTypes;
using Microsoft.XLANGs.Core;

namespace BizTalk.Factory.XLang
{
	[Serializable]
	[SuppressMessage("Design", "CA1010:Collections should implement generic interface")]
	[SuppressMessage("Design", "CA1724:Type names should not match namespaces")]
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

		private Message(Context context, object content) : base("BizTalkFactoryXLangMessage", context)
		{
			context.RefMessage(this);
			AddPart(string.Empty, "Body");
			this[0].LoadFrom(content);
		}
	}
}
