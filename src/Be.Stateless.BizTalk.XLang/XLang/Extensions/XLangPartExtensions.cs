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
using Microsoft.XLANGs.BaseTypes;

namespace Be.Stateless.BizTalk.XLang.Extensions
{
	public static class XLangPartExtensions
	{
		public static Stream AsStream(this XLANGPart messagePart)
		{
			if (messagePart == null) throw new ArgumentNullException(nameof(messagePart));
			return (Stream) messagePart.RetrieveAs(typeof(Stream));
		}
	}
}

namespace BizTalk.Factory.XLang.Extensions
{
	[SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Public API.")]
	[SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Public API.")]
	[Obsolete("Use class in Be.Stateless.BizTalk.XLang.Extensions instead.")]
	public static class XLangPartExtensions
	{
		public static Stream AsStream(this XLANGPart messagePart)
		{
			return Be.Stateless.BizTalk.XLang.Extensions.XLangPartExtensions.AsStream(messagePart);
		}
	}
}
