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
using System.IO;
using System.Text;
using Be.Stateless.BizTalk.XLang.Serialization;
using Be.Stateless.IO.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.XLang
{
	public class Base64StringMessageFixture
	{
		[Fact]
		public void CanBeAssignedToStringMessage()
		{
			((Base64StringMessage) _base64Content).Should().BeAssignableTo<StringMessage>();
		}

		[Fact]
		public void CanDeserialize()
		{
			using (var stream = new MemoryStream(StringMessageFormatter.DefaultEncoding.GetBytes(_content)))
			{
				new Base64StringMessageFormatter().Deserialize(stream)
					.Should().BeOfType<Base64StringMessage>()
					.And.Subject.As<Base64StringMessage>().Content.Should().Be(_base64Content);
			}
		}

		[Fact]
		public void CanImplicitlyConvertFromString()
		{
			((Base64StringMessage) _base64Content).Should().BeOfType<Base64StringMessage>();
		}

		[Fact]
		public void CanSerialize()
		{
			using (var stream = new MemoryStream())
			{
				var sut = new Base64StringMessage(_base64Content);
				new Base64StringMessageFormatter().Serialize(stream, sut);
				stream.Rewind().ReadToEnd().Should().Be(_content);
			}
		}

		[Fact]
		public void ToStringReturnsDecodedContent()
		{
			new Base64StringMessage(_base64Content).ToString().Should().Be(_content);
		}

		private static readonly string _content = typeof(Base64StringMessageFixture).FullName;
		private static readonly string _base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(_content));
	}
}

namespace BizTalk.Factory.XLang
{
	public class Base64StringMessageFixture
	{
		[Fact]
		public void CanBeAssignedToBase64StringMessage()
		{
			((Base64StringMessage) _base64Content).Should().BeAssignableTo<Be.Stateless.BizTalk.XLang.Base64StringMessage>();
		}

		[Fact]
		public void CanBeAssignedToStringMessage()
		{
			((Base64StringMessage) _base64Content).Should().BeAssignableTo<Be.Stateless.BizTalk.XLang.StringMessage>();
		}

		[Fact]
		public void CanDeserializeAsBaseClass()
		{
			using (var stream = new MemoryStream(StringMessageFormatter.DefaultEncoding.GetBytes(_content)))
			{
				new Base64StringMessageFormatter().Deserialize(stream)
					.Should().BeOfType<Be.Stateless.BizTalk.XLang.Base64StringMessage>();
			}
		}

		[Fact]
		public void CanImplicitlyConvertFromString()
		{
			((Base64StringMessage) _base64Content).Should().BeOfType<Base64StringMessage>();
		}

		[Fact]
		public void CanSerializeViaBaseClass()
		{
			using (var stream = new MemoryStream())
			{
				var sut = new Base64StringMessage(_base64Content);
				new Base64StringMessageFormatter().Serialize(stream, sut);
				stream.Rewind().ReadToEnd().Should().Be(_content);
			}
		}

		[Fact]
		public void ToStringReturnsDecodedContent()
		{
			new Base64StringMessage(_base64Content).ToString().Should().Be(_content);
		}

		private static readonly string _content = typeof(Base64StringMessageFixture).FullName;
		private static readonly string _base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(_content));
	}
}
