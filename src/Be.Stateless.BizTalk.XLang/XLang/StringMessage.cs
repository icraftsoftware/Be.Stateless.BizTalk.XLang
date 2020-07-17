#region Copyright & License

// Copyright © 2012 - 2021 François Chabot
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
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Microsoft.XLANGs.BaseTypes;

namespace BizTalk.Factory.XLang
{
	/// <summary>
	/// Message type to use when one needs to send a text message from an orchestration.
	/// </summary>
	[SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
	[CustomFormatter(typeof(StringContentFormatter))]
	[Serializable]
	public class StringMessage
	{
		#region Nested Type: StringContentFormatter

		public class StringContentFormatter : IFormatter
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
				var reader = new StreamReader(serializationStream, true);
				var content = reader.ReadToEnd();
				return new StringMessage(content);
			}

			public void Serialize(Stream serializationStream, object graph)
			{
				if (serializationStream == null) throw new ArgumentNullException(nameof(serializationStream));
				if (graph == null) throw new ArgumentNullException(nameof(graph));
				var content = (StringMessage) graph;
				var bytes = content.GetBytes();
				serializationStream.Write(bytes, 0, bytes.Length);
			}

			#endregion
		}

		#endregion

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
		protected string Content { get; }

		protected virtual byte[] GetBytes()
		{
			return Encoding.UTF8.GetBytes(Content);
		}
	}
}
