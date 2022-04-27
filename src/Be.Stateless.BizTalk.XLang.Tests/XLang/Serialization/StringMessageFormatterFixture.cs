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
using Be.Stateless.IO.Extensions;
using FluentAssertions;
using Xunit;

namespace Be.Stateless.BizTalk.XLang.Serialization
{
	public class StringMessageFormatterFixture
	{
		[Fact]
		public void DeserializeContent()
		{
			using (var stream = new MemoryStream(StringMessageFormatter.DefaultEncoding.GetBytes(_content)))
			{
				new StringMessageFormatter().Deserialize(stream)
					.Should().BeOfType<StringMessage>()
					.And.Subject.As<StringMessage>().Content.Should().Be(_content);
			}
		}

		[Fact]
		public void SerializationRoundTrips()
		{
			using (var stream = new MemoryStream())
			{
				var message = new StringMessage(_content);

				var sut = new StringMessageFormatter();

				sut.Serialize(stream, message);
				sut.Deserialize(stream.Rewind())
					.Should().BeOfType<StringMessage>()
					.And.BeEquivalentTo(message, options => options.Including(m => m.Content));
			}
		}

		[Fact]
		public void SerializeContent()
		{
			using (var stream = new MemoryStream())
			{
				var message = new StringMessage(_content);
				new StringMessageFormatter().Serialize(stream, message);
				stream.Rewind().ReadToEnd().Should().Be(_content);
			}
		}

		private static readonly string _content = typeof(StringMessageFormatterFixture).FullName;
	}
}
