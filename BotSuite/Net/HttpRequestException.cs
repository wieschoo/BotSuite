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
	/// <summary>
	/// Exception class for wrapping exceptions thrown by one of the request methods (POST/GET)
	/// </summary>
	[Serializable]
	public class HttpRequestException : Exception
	{
		public HttpRequestException()
		{
		}
		public HttpRequestException(string message) : base(message)
		{
		}
		public HttpRequestException(string message, Exception inner) : base(message, inner)
		{
		}
		protected HttpRequestException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
