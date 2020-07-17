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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml;
using Be.Stateless.BizTalk.Xml;
using Be.Stateless.Linq.Extensions;
using Be.Stateless.Xml;
using BizTalk.Factory.XLang.Extensions;
using Microsoft.XLANGs.BaseTypes;

namespace BizTalk.Factory.XLang
{
	/// <summary>
	/// A collection of <see cref="XLANGMessage"/> messages that facilitates calling of <see cref="TransformHelper"/> with
	/// several messages from within an orchestration XLang expression shape.
	/// </summary>
	[Serializable]
	[SuppressMessage("ReSharper", "CommentTypo")]
	public sealed class MessageCollection : LinkedList<XLANGMessage>, IDisposable
	{
		#region Operators

		[SuppressMessage("Usage", "CA2225:Operator overloads have named alternates")]
		public static implicit operator XmlReader(MessageCollection collection)
		{
			var xmlReaderSettings = new XmlReaderSettings { CloseInput = true, XmlResolver = null };
			return collection is null
				? EmptyXmlReader.Create()
				: collection.Count == 1
					? XmlReader.Create(collection.First.Value.AsStream(), xmlReaderSettings)
					: CompositeXmlReader.Create(collection.Select(m => m.AsStream()).ToArray(), xmlReaderSettings);
		}

		#endregion

		public MessageCollection() { }

		// NOTICE ctor with params XLANGMessage[], though callable in a XLang expression shape of a BTS orchestration,
		// does not behave as expected and entails weird NullReferenceException upon Dispose(); hence the other 9 ctors
		// public MessageCollection(params XLANGMessage[] messages) : base(messages) { }

		public MessageCollection(XLANGMessage message1)
			: base(new[] { message1 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
		}

		public MessageCollection(XLANGMessage message1, XLANGMessage message2)
			: base(new[] { message1, message2 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
		}

		public MessageCollection(XLANGMessage message1, XLANGMessage message2, XLANGMessage message3)
			: base(new[] { message1, message2, message3 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
		}

		public MessageCollection(XLANGMessage message1, XLANGMessage message2, XLANGMessage message3, XLANGMessage message4)
			: base(new[] { message1, message2, message3, message4 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
			if (message4 == null) throw new ArgumentNullException(nameof(message4));
		}

		public MessageCollection(XLANGMessage message1, XLANGMessage message2, XLANGMessage message3, XLANGMessage message4, XLANGMessage message5)
			: base(new[] { message1, message2, message3, message4, message5 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
			if (message4 == null) throw new ArgumentNullException(nameof(message4));
			if (message5 == null) throw new ArgumentNullException(nameof(message5));
		}

		public MessageCollection(
			XLANGMessage message1,
			XLANGMessage message2,
			XLANGMessage message3,
			XLANGMessage message4,
			XLANGMessage message5,
			XLANGMessage message6)
			: base(new[] { message1, message2, message3, message4, message5, message6 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
			if (message4 == null) throw new ArgumentNullException(nameof(message4));
			if (message5 == null) throw new ArgumentNullException(nameof(message5));
			if (message6 == null) throw new ArgumentNullException(nameof(message6));
		}

		public MessageCollection(
			XLANGMessage message1,
			XLANGMessage message2,
			XLANGMessage message3,
			XLANGMessage message4,
			XLANGMessage message5,
			XLANGMessage message6,
			XLANGMessage message7)
			: base(new[] { message1, message2, message3, message4, message5, message6, message7 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
			if (message4 == null) throw new ArgumentNullException(nameof(message4));
			if (message5 == null) throw new ArgumentNullException(nameof(message5));
			if (message6 == null) throw new ArgumentNullException(nameof(message6));
			if (message7 == null) throw new ArgumentNullException(nameof(message7));
		}

		public MessageCollection(
			XLANGMessage message1,
			XLANGMessage message2,
			XLANGMessage message3,
			XLANGMessage message4,
			XLANGMessage message5,
			XLANGMessage message6,
			XLANGMessage message7,
			XLANGMessage message8)
			: base(new[] { message1, message2, message3, message4, message5, message6, message7, message8 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
			if (message4 == null) throw new ArgumentNullException(nameof(message4));
			if (message5 == null) throw new ArgumentNullException(nameof(message5));
			if (message6 == null) throw new ArgumentNullException(nameof(message6));
			if (message7 == null) throw new ArgumentNullException(nameof(message7));
			if (message8 == null) throw new ArgumentNullException(nameof(message8));
		}

		public MessageCollection(
			XLANGMessage message1,
			XLANGMessage message2,
			XLANGMessage message3,
			XLANGMessage message4,
			XLANGMessage message5,
			XLANGMessage message6,
			XLANGMessage message7,
			XLANGMessage message8,
			XLANGMessage message9)
			: base(new[] { message1, message2, message3, message4, message5, message6, message7, message8, message9 })
		{
			if (message1 == null) throw new ArgumentNullException(nameof(message1));
			if (message2 == null) throw new ArgumentNullException(nameof(message2));
			if (message3 == null) throw new ArgumentNullException(nameof(message3));
			if (message4 == null) throw new ArgumentNullException(nameof(message4));
			if (message5 == null) throw new ArgumentNullException(nameof(message5));
			if (message6 == null) throw new ArgumentNullException(nameof(message6));
			if (message7 == null) throw new ArgumentNullException(nameof(message7));
			if (message8 == null) throw new ArgumentNullException(nameof(message8));
			if (message9 == null) throw new ArgumentNullException(nameof(message9));
		}

		private MessageCollection(SerializationInfo info, StreamingContext context) : base(info, context) { }

		#region IDisposable Members

		[SuppressMessage("Design", "CA1063:Implement IDisposable Correctly")]
		void IDisposable.Dispose()
		{
			this.Where(m => m != null).ForEach(m => m.Dispose());
		}

		#endregion

		public void Add(XLANGMessage message)
		{
			AddLast(message);
		}
	}
}
