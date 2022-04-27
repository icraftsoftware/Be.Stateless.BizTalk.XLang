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
using System.Runtime.Serialization;
using System.Text;

namespace Be.Stateless.BizTalk.XLang.Serialization
{
	internal class StringMessageFormatter : IFormatter
	{
		#region IFormatter Members

		public SerializationBinder Binder
		{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}

		public StreamingContext Context
		{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}

		public ISurrogateSelector SurrogateSelector
		{
			get => throw new NotSupportedException();
			set => throw new NotSupportedException();
		}

		public object Deserialize(Stream serializationStream)
		{
			using (var memoryStream = new MemoryStream())
			{
				serializationStream.CopyTo(memoryStream);
				return SetBytes(memoryStream.ToArray());
			}
		}

		public void Serialize(Stream serializationStream, object graph)
		{
			if (serializationStream == null) throw new ArgumentNullException(nameof(serializationStream));
			if (graph == null) throw new ArgumentNullException(nameof(graph));
			var message = (StringMessage) graph;
			var bytes = GetBytes(message.Content);
			serializationStream.Write(bytes, 0, bytes.Length);
		}

		#endregion

		protected virtual byte[] GetBytes(string content)
		{
			return DefaultEncoding.GetBytes(content);
		}

		protected virtual StringMessage SetBytes(byte[] content)
		{
			return new StringMessage(DefaultEncoding.GetString(content));
		}

		internal static readonly Encoding DefaultEncoding = Encoding.UTF8;
	}
}
