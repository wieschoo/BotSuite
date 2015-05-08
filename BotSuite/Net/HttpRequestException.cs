// -----------------------------------------------------------------------
//  <copyright file="HttpRequestException.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Net
{
	using System;
	using System.Runtime.Serialization;

	/// <summary>
	///     Exception class for wrapping exceptions thrown by one of the request methods (POST/GET)
	/// </summary>
	[Serializable]
	public class HttpRequestException : Exception
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="HttpRequestException" /> class
		/// </summary>
		public HttpRequestException()
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpRequestException" /> class
		/// </summary>
		/// <param name="message">
		///     the message of the exception
		/// </param>
		public HttpRequestException(string message)
			: base(message)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpRequestException" /> class
		/// </summary>
		/// <param name="message">
		///     the message of the exception
		/// </param>
		/// <param name="inner">
		///     the inner exception
		/// </param>
		public HttpRequestException(string message, Exception inner)
			: base(message, inner)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpRequestException" /> class
		/// </summary>
		/// <param name="info">
		///     an instance of the SerializationInfo class
		/// </param>
		/// <param name="context">
		///     an instance of the StreamingContext class
		/// </param>
		protected HttpRequestException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}