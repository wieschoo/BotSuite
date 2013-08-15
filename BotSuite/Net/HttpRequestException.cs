//-----------------------------------------------------------------------
// <copyright file="HttpRequestException.cs" company="Wieschoo &amp; enWare">
//     Copyright (c) Wieschoo &amp; enWare.
// </copyright>
// <project>BotSuite.Net</project>
// <purpose>framework for creating bots</purpose>
// <homepage>http://botsuite.net/</homepage>
// <license>http://botsuite.net/license/index/</license>
//-----------------------------------------------------------------------

namespace BotSuite.Net
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	/// <summary>
	/// Exception class for wrapping exceptions thrown by one of the request methods (POST/GET)
	/// </summary>
	[Serializable]
	public class HttpRequestException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestException"/> class
		/// </summary>
		public HttpRequestException()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestException"/> class
		/// </summary>
		/// <param name="message">the message of the exception</param>
		public HttpRequestException(string message) : base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestException"/> class
		/// </summary>
		/// <param name="message">the message of the exception</param>
		/// <param name="inner">the inner exception</param>
		public HttpRequestException(string message, Exception inner) : base(message, inner)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpRequestException"/> class
		/// </summary>
		/// <param name="info">an instance of the SerializationInfo class</param>
		/// <param name="context">an instance of the StreamingContext class</param>
		protected HttpRequestException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context)
		{
		}
	}
}
