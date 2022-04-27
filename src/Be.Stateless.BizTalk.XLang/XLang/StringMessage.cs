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
using System.Xml.Serialization;
using Be.Stateless.BizTalk.XLang.Serialization;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.XLang
{
	/// <summary>
	/// Message type to use when one needs to send a text message from an orchestration.
	/// </summary>
	[CustomFormatter(typeof(StringMessageFormatter))]
	[Serializable]
	public class StringMessage
	{
		#region Operators

		public static implicit operator StringMessage(string content)
		{
			return new(content);
		}

		#endregion

		public StringMessage(string content)
		{
			Content = content ?? throw new ArgumentNullException(nameof(content));
		}

		#region Base Class Member Overrides

		public override string ToString()
		{
			return Content;
		}

		#endregion

		[XmlIgnore]
		[field: XmlIgnore]
		public string Content { get; }
	}
}

namespace BizTalk.Factory.XLang
{
	/// <summary>
	/// Message type to use when one needs to send a text message from an orchestration.
	/// </summary>
	[Obsolete("Use class in Be.Stateless.BizTalk.XLang instead.")]
	[CustomFormatter(typeof(StringMessageFormatter))]
	[Serializable]
	public class StringMessage : Be.Stateless.BizTalk.XLang.StringMessage
	{
		#region Operators

		public static implicit operator StringMessage(string content)
		{
			return new(content);
		}

		#endregion

		public StringMessage(string content) : base(content) { }
	}
}
