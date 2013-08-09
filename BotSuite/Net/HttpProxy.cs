//-----------------------------------------------------------------------
// <copyright file="HttpProxy.cs" company="Wieschoo &amp; enWare">
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
	using System.Diagnostics.Contracts;
	using System.Net;

	/// <summary>
	/// proxy class for storing proxy informations for HttpClient
	/// </summary>
	public class HttpProxy
	{
		/// <summary>
		/// the internally used WebProxy instance
		/// </summary>
		private WebProxy _InternalProxy = new WebProxy();

		/// <summary>
		/// the network credentials to use with this proxy
		/// </summary>
		private NetworkCredential _Credential = null;

		/// <summary>
		/// the address of the proxy
		/// </summary>
		private Uri _Address = null;

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpProxy"/> class
		/// </summary>
		/// <param name="address">url of proxy</param>
		/// <param name="username">username for proxy, if applicable</param>
		/// <param name="password">password for proxy, if applicable</param>
		public HttpProxy(String address, String username = null, String password = null) : this(new Uri(address), username, password)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpProxy"/> class
		/// </summary>
		/// <param name="address">uri of proxy</param>
		/// <param name="username">username for proxy, if applicable</param>
		/// <param name="password">password for proxy, if applicable</param>
		public HttpProxy(Uri address, String username = null, String password = null)
		{
			Contract.Requires(address != null);

			this._Address = address;
			if(username != null)
			{
				if(password == null)
				{
					password = String.Empty;
				}

				this._Credential = new NetworkCredential(username, password);
			}
			else
			{
				this._Credential = CredentialCache.DefaultNetworkCredentials;
			}

			this._InternalProxy.Address = this._Address;
			this._InternalProxy.Credentials = this._Credential;
		}

		/// <summary>
		/// returns the internally manageg WebProxy object for usage in HttpWebRequest
		/// </summary>
		/// <returns>the internally used instance of the WebProxy class</returns>
		internal WebProxy GetWebProxy()
		{
			return this._InternalProxy;
		}
	}
}
