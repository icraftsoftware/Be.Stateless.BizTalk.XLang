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

using System.IO;
using Be.Stateless.BizTalk.XLang.Serialization;
using Be.Stateless.IO.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.XLang
{
	public class StringMessageFixture
	{
		[Fact]
		public void CanDeserialize()
		{
			using (var stream = new MemoryStream(StringMessageFormatter.DefaultEncoding.GetBytes(_content)))
			{
				new StringMessageFormatter().Deserialize(stream)
					.Should().BeOfType<StringMessage>()
					.And.Subject.As<StringMessage>().Content.Should().Be(_content);
			}
		}

		[Fact]
		public void CanImplicitlyConvertFromString()
		{
			((StringMessage) _content).Should().BeOfType<StringMessage>();
		}

		[Fact]
		public void CanSerialize()
		{
			using (var stream = new MemoryStream())
			{
				var sut = new StringMessage(_content);
				new StringMessageFormatter().Serialize(stream, sut);
				stream.Rewind().ReadToEnd().Should().Be(_content);
			}
		}

		[Fact]
		public void ToStringReturnsContent()
		{
			new StringMessage(_content).ToString().Should().Be(_content);
		}

		private static readonly string _content = typeof(StringMessageFixture).FullName;
	}
}

namespace BizTalk.Factory.XLang
{
#pragma warning disable CS0618
	public class StringMessageFixture
	{
		[Fact]
		public void CanBeAssignedToStringMessage()
		{
			((StringMessage) _content).Should().BeAssignableTo<Be.Stateless.BizTalk.XLang.StringMessage>();
		}

		[Fact]
		public void CanDeserializeAsBaseClass()
		{
			using (var stream = new MemoryStream(StringMessageFormatter.DefaultEncoding.GetBytes(_content)))
			{
				new StringMessageFormatter().Deserialize(stream)
					.Should().BeOfType<Be.Stateless.BizTalk.XLang.StringMessage>();
			}
		}

		[Fact]
		public void CanImplicitlyConvertFromString()
		{
			((StringMessage) _content).Should().BeOfType<StringMessage>();
		}

		[Fact]
		public void CanSerializeViaBaseClass()
		{
			using (var stream = new MemoryStream())
			{
				var sut = new StringMessage(_content);
				new StringMessageFormatter().Serialize(stream, sut);
				stream.Rewind().ReadToEnd().Should().Be(_content);
			}
		}

		[Fact]
		public void ToStringReturnsContent()
		{
			new StringMessage(_content).ToString().Should().Be(_content);
		}

		private static readonly string _content = typeof(StringMessageFixture).FullName;
	}
#pragma warning restore CS0618
}
