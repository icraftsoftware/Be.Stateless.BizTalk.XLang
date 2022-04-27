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
using Be.Stateless.IO.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.XLang.Serialization
{
	public class Base64StringMessageFormatterFixture
	{
		[Fact]
		public void DeserializeConvertsContentToBase64()
		{
			using (var stream = new MemoryStream(StringMessageFormatter.DefaultEncoding.GetBytes(_content)))
			{
				new Base64StringMessageFormatter().Deserialize(stream)
					.Should().BeOfType<Base64StringMessage>()
					.And.Subject.As<Base64StringMessage>().Content.Should().Be(_base64Content);
			}
		}

		[Fact]
		public void SerializationRoundTrips()
		{
			using (var stream = new MemoryStream())
			{
				var sut = new Base64StringMessageFormatter();

				var message = new Base64StringMessage(_base64Content);
				sut.Serialize(stream, message);
				var deserializedObject = sut.Deserialize(stream.Rewind());

				deserializedObject.Should().BeOfType<Base64StringMessage>();
				deserializedObject.Should().BeEquivalentTo(message, options => options.Including(m => m.Content));
				deserializedObject.As<Base64StringMessage>().Content.Should().Be(_base64Content);
			}
		}

		[Fact]
		public void SerializeConvertsContentFromBase64()
		{
			using (var stream = new MemoryStream())
			{
				var sut = new Base64StringMessage(_base64Content);
				new Base64StringMessageFormatter().Serialize(stream, sut);
				stream.Rewind().ReadToEnd().Should().Be(_content);
			}
		}

		private static readonly string _content = typeof(Base64StringMessageFormatterFixture).FullName;
		private static readonly string _base64Content = Convert.ToBase64String(Encoding.UTF8.GetBytes(_content));
	}
}
