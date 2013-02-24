/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BotSuite.Net
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
