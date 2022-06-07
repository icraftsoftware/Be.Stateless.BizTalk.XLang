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
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using Microsoft.BizTalk.XLANGs.BTXEngine;
using Microsoft.XLANGs.Core;

namespace Be.Stateless.BizTalk.XLang
{
	/// <summary>
	/// In the context of BizTalk Server composite orchestrations, allows a sub-orchestration to notify its caller that it has
	/// failed by throwing a <see cref="SubOrchestrationFailedException"/> that identifies it by its <see cref="Name"/>.
	/// </summary>
	[Serializable]
	public class SubOrchestrationFailedException : Exception
	{
		/// <summary>
		/// XLang helper to throw a <see cref="SubOrchestrationFailedException"/>.
		/// </summary>
		/// <remarks>
		/// If the direct caller of this method is an orchestration, i.e. a <see cref="BTXService"/>-derived type, the thrown
		/// <see cref="SubOrchestrationFailedException"/>'s <see cref="Name"/> will be initialized to this orchestration's
		/// namespace, which by convention is its name. If the direct caller is not an orchestration, then the <see cref="Name"/>
		/// will be initialized to the <see cref="Service.RootService"/>'s <see cref="Type"/> namespace.
		/// </remarks>
		public static void Throw()
		{
			var declaringType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
			var name = declaringType != null && typeof(BTXService).IsAssignableFrom(declaringType)
				? declaringType.Namespace
				: Service.RootService.GetType().Namespace;
			var message = $"Orchestration '{name}' failed.";
			throw new SubOrchestrationFailedException(name, message);
		}

		/// <summary>
		/// XLang helper to throw a <see cref="SubOrchestrationFailedException"/> with an <paramref name="inner"/> <see
		/// cref="Exception"/>.
		/// </summary>
		/// <param name="inner">
		/// The inner exception.
		/// </param>
		/// <remarks>
		/// If the direct caller of this method is an orchestration, i.e. a <see cref="BTXService"/>-derived type, the thrown
		/// <see cref="SubOrchestrationFailedException"/>'s <see cref="Name"/> will be initialized to this orchestration's
		/// namespace, which by convention is its name. If the direct caller is not an orchestration, then the <see cref="Name"/>
		/// will be initialized to the <see cref="Service.RootService"/>'s <see cref="Type"/> namespace.
		/// </remarks>
		public static void Throw(Exception inner)
		{
			var declaringType = new StackTrace().GetFrame(1).GetMethod().DeclaringType;
			var name = declaringType != null && typeof(BTXService).IsAssignableFrom(declaringType)
				? declaringType.Namespace
				: Service.RootService.GetType().Namespace;
			var message = $"Orchestration '{name}' failed.";
			throw new SubOrchestrationFailedException(name, message, inner);
		}

		public SubOrchestrationFailedException(string name, string message) : base(message)
		{
			Name = name;
		}

		public SubOrchestrationFailedException(string name, string message, Exception inner) : base(message, inner)
		{
			Name = name;
		}

		protected SubOrchestrationFailedException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			Name = info.GetString("Name");
		}

		#region Base Class Member Overrides

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null) throw new ArgumentNullException(nameof(info));
			info.AddValue("Name", Name);
			base.GetObjectData(info, context);
		}

		public override string Message => $"Orchestration '{Name}' failed. {base.Message}";

		#endregion

		/// <summary>
		/// The name of the sub-orchestration that has failed and thrown this exception.
		/// </summary>
		[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
		public string Name { get; set; }
	}
}

namespace BizTalk.Factory.XLang
{
	[Obsolete("Use class in Be.Stateless.BizTalk.XLang instead.")]
	[Serializable]
	public class SubOrchestrationFailedException : Be.Stateless.BizTalk.XLang.SubOrchestrationFailedException
	{
		public SubOrchestrationFailedException(string name, string message) : base(name, message) { }

		public SubOrchestrationFailedException(string name, string message, Exception inner) : base(name, message, inner) { }

		protected SubOrchestrationFailedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
	}
}
