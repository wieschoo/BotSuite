using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wieschoo.BotSuite.Net
{
	[Serializable]
	public class RequestException : Exception
	{
		public RequestException()
		{
		}
		public RequestException(string message) : base(message)
		{
		}
		public RequestException(string message, Exception inner) : base(message, inner)
		{
		}
		protected RequestException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
