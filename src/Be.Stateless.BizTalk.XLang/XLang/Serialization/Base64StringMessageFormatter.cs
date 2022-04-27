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

namespace Be.Stateless.BizTalk.XLang.Serialization
{
	internal class Base64StringMessageFormatter : StringMessageFormatter
	{
		#region Base Class Member Overrides

		protected override byte[] GetBytes(string content)
		{
			return Convert.FromBase64String(content);
		}

		protected override StringMessage SetBytes(byte[] content)
		{
			return new Base64StringMessage(Convert.ToBase64String(content));
		}

		#endregion
	}
}
