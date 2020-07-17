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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using FluentAssertions;
using Xunit;

namespace BizTalk.Factory.XLang
{
	public class SubOrchestrationFailedExceptionFixture
	{
		[Fact]
		public void Message()
		{
			Action act = () => throw new SubOrchestrationFailedException("process", "Can't proceed.");
			act.Should().Throw<SubOrchestrationFailedException>().WithMessage("Orchestration 'process' failed. Can't proceed.");
		}

		[Fact]
		public void SerializationRoundTrip()
		{
			var stream = new MemoryStream();
			var formatter = new BinaryFormatter();

			Action act = () => throw new SubOrchestrationFailedException("process", "Can't proceed.");
			var originalException = act.Should().Throw<SubOrchestrationFailedException>().Which;
			formatter.Serialize(stream, originalException);
			stream.Position = 0;
			var deserializedException = (SubOrchestrationFailedException) formatter.Deserialize(stream);

			deserializedException.Should().NotBeSameAs(originalException);
			deserializedException.Name.Should().Be(originalException.Name);
			deserializedException.ToString().Should().Be(originalException.ToString());
		}

		[Fact]
		public void ToStringSerialization()
		{
			Action act = () => throw new SubOrchestrationFailedException("process", "Can't proceed.");
			var exception = act.Should().Throw<SubOrchestrationFailedException>().Which;
			exception.ToString().Should().StartWith($"{typeof(SubOrchestrationFailedException).FullName}: Orchestration 'process' failed. Can't proceed.");
		}
	}
}
