using System;
using System.Diagnostics.Contracts;
using System.Net;

namespace BotSuite.Net
{
	/// <summary>
	/// proxy class for storing proxy informations for HttpClient
	/// </summary>
	public class HttpProxy
	{
		private WebProxy _InternalProxy = new WebProxy();
		private NetworkCredential _Credential = null;
		private Uri _Address = null;

		/// <summary>
		/// constructor for proxy class
		/// </summary>
		/// <param name="address">url of proxy</param>
		/// <param name="username">username for proxy, if applicable</param>
		/// <param name="password">password for proxy, if applicable</param>
		public HttpProxy(String address, String username = null, String password = null) : this(new Uri(address), username, password) { }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="address"></param>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public HttpProxy(Uri address, String username = null, String password = null)
		{
			Contract.Requires(address != null);

			this._Address = address;
			if(username != null)
			{
				if(password == null)
					password = String.Empty;

				this._Credential = new NetworkCredential(username, password);
			}
			else
				this._Credential = CredentialCache.DefaultNetworkCredentials;

			this._InternalProxy.Address = this._Address;
			this._InternalProxy.Credentials = this._Credential;
		}

		/// <summary>
		/// returns the internally manageg WebProxy object for usage in HttpWebRequest
		/// </summary>
		/// <returns></returns>
		internal WebProxy GetWebProxy()
		{
			return this._InternalProxy;
		}
	}
}
