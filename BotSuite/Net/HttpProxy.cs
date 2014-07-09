// -----------------------------------------------------------------------
//  <copyright file="HttpProxy.cs" company="Wieschoo &amp; Binary Overdrive">
//      Copyright (c) Wieschoo &amp; Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite.Net
{
	using System;
	using System.Net;

	/// <summary>
	///     proxy class for storing proxy informations for HttpClient
	/// </summary>
	public class HttpProxy
	{
		/// <summary>
		///     the internally used WebProxy instance
		/// </summary>
		private readonly WebProxy internalProxy = new WebProxy();

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpProxy" /> class
		/// </summary>
		/// <param name="address">
		///     url of proxy
		/// </param>
		/// <param name="username">
		///     username for proxy, if applicable
		/// </param>
		/// <param name="password">
		///     password for proxy, if applicable
		/// </param>
		public HttpProxy(string address, string username = null, string password = null)
			: this(new Uri(address), username, password)
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpProxy" /> class
		/// </summary>
		/// <param name="address">
		///     uri of proxy
		/// </param>
		/// <param name="username">
		///     username for proxy, if applicable
		/// </param>
		/// <param name="password">
		///     password for proxy, if applicable
		/// </param>
		public HttpProxy(Uri address, string username = null, string password = null)
		{
			NetworkCredential credential;

			if (username != null)
			{
				if (password == null)
				{
					password = string.Empty;
				}

				credential = new NetworkCredential(username, password);
			}
			else
			{
				credential = CredentialCache.DefaultNetworkCredentials;
			}

			this.internalProxy.Address = address;
			this.internalProxy.Credentials = credential;
		}

		/// <summary>
		///     returns the internally manageg WebProxy object for usage in HttpWebRequest
		/// </summary>
		/// <returns>the internally used instance of the WebProxy class</returns>
		internal WebProxy GetWebProxy()
		{
			return this.internalProxy;
		}
	}
}