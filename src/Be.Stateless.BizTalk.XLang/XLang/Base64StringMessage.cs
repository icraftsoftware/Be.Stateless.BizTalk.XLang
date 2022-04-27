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
using Be.Stateless.BizTalk.XLang.Serialization;
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.XLang
{
	/// <summary>
	/// Message type to use when one needs to send a base 64 encoded text message from an orchestration.
	/// </summary>
	[CustomFormatter(typeof(Base64StringMessageFormatter))]
	[Serializable]
	public class Base64StringMessage : StringMessage
	{
		#region Operators

		public static implicit operator Base64StringMessage(string base64Content)
		{
			return new(base64Content);
		}

		#endregion

		public Base64StringMessage(string base64Content) : base(base64Content) { }

		#region Base Class Member Overrides

		public override string ToString()
		{
			return StringMessageFormatter.DefaultEncoding.GetString(Convert.FromBase64String(Content));
		}

		#endregion
	}
}

namespace BizTalk.Factory.XLang
{
	/// <summary>
	/// Message type to use when one needs to send a base 64 encoded text message from an orchestration.
	/// </summary>
	[Obsolete("Use class in Be.Stateless.BizTalk.XLang instead.")]
	[CustomFormatter(typeof(Base64StringMessageFormatter))]
	[Serializable]
	public class Base64StringMessage : Be.Stateless.BizTalk.XLang.Base64StringMessage
	{
		#region Operators

		public static implicit operator Base64StringMessage(string base64Content)
		{
			return new(base64Content);
		}

		#endregion

		public Base64StringMessage(string base64Content) : base(base64Content) { }
	}
}
