// -----------------------------------------------------------------------
//  <copyright file="HttpProxy.cs" company="Binary Overdrive">
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
	using System.Net;

	/// <summary>
	///     proxy class for storing proxy informations for HttpClient
	/// </summary>
	public class HttpProxy
	{
		/// <summary>
		///     the internally used WebProxy instance
		/// </summary>
		private readonly WebProxy _internalProxy = new WebProxy();

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

			if(username != null)
			{
				if(password == null)
				{
					password = string.Empty;
				}

				credential = new NetworkCredential(username, password);
			}
			else
			{
				credential = CredentialCache.DefaultNetworkCredentials;
			}

			this._internalProxy.Address = address;
			this._internalProxy.Credentials = credential;
		}

		/// <summary>
		///     returns the internally manageg WebProxy object for usage in HttpWebRequest
		/// </summary>
		/// <returns>the internally used instance of the WebProxy class</returns>
		internal WebProxy GetWebProxy()
		{
			return this._internalProxy;
		}
	}
}