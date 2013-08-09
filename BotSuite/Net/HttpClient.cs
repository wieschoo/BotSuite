﻿//-----------------------------------------------------------------------
// <copyright file="HttpClient.cs" company="Wieschoo &amp; enWare">
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
	using System.Drawing;
	using System.IO;
	using System.Net;
	using System.Text;

	/// <summary>
	/// HttpClient class for making bots for Browsergames
	/// </summary>
	public class HttpClient
	{
		/// <summary>
		/// cache for last response
		/// </summary>
		private byte[] _Cache = null;

		/// <summary>
		/// Gets all bytes of the last response
		/// </summary>
		public byte[] Cache
		{
			get
			{
				return this._Cache;
			}
		}

		/// <summary>
		/// Contains all cookies for this instance of the HttpClient class
		/// </summary>
		private CookieContainer _Cookies = new CookieContainer();

		/// <summary>
		/// Gets all cookies for this instance of the HttpClient class
		/// </summary>
		public CookieContainer Cookies
		{
			get { return this._Cookies; }
		}

		/// <summary>
		/// the user agent string which is used for requests
		/// </summary>
		private String _UserAgent = String.Empty;

		/// <summary>
		/// a collection of all headers of the last response
		/// </summary>
		private HttpHeaderCollection _Headers = new HttpHeaderCollection();

		/// <summary>
		/// Gets a collection of all headers of the last response
		/// </summary>
		public HttpHeaderCollection Headers
		{
			get { return this._Headers; }
		}

		/// <summary>
		/// true, if the HttpClient should adjust the referer at every change of the URL
		/// </summary>
		private Boolean _AutoReferer = true;

		/// <summary>
		/// Gets or sets a value indicating whether the HttpClient should adjust the referer at every change of the URL
		/// </summary>
		public Boolean AutoReferer
		{
			get { return this._AutoReferer; }
			set { this._AutoReferer = value; }
		}

		/// <summary>
		/// the current referer
		/// </summary>
		private String _Referer = null;

		/// <summary>
		/// Gets or sets the current referer
		/// </summary>
		public String Referer
		{
			get { return this._Referer; }
			set { this._Referer = value; }
		}

		/// <summary>
		/// true, if the HttpClient should use the HttpClient.Proxy for requests
		/// </summary>
		private Boolean _UseProxy = false;

		/// <summary>
		/// Gets or sets a value indicating whether the HttpClient should use the HttpClient.Proxy for requests
		/// </summary>
		public Boolean UseProxy
		{
			get { return this._UseProxy; }
			set { this._UseProxy = value; }
		}

		/// <summary>
		/// proxy object with settings for proxy usage for requests (when HttpClient.UseProxy is set to true)
		/// </summary>
		private HttpProxy _Proxy = null;

		/// <summary>
		/// Gets or sets a proxy object with settings for proxy usage for requests (when HttpClient.UseProxy is set to true)
		/// </summary>
		public HttpProxy Proxy
		{
			get { return this._Proxy; }
			set { this._Proxy = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether unsafe header parsing should be used
		/// </summary>
		public Boolean UseUnsafeHeaderParsing
		{
			get
			{
				return HttpProxyHacker.IsUseUnsafeHeaderParsingActivated();
			}

			set
			{
				if(!HttpProxyHacker.ToggleAllowUnsafeHeaderParsing(value))
				{
					throw new Exception("unable to set useUnsafeHeaderParsing in configuration");
				}
			}
		}

		/// <summary>
		/// true, if auto-redirects should be allowed, else false (false might cause more work for the developer since more requests have to be made manually)
		/// </summary>
		private Boolean _AllowAutoRedirect = true;

		/// <summary>
		///  Gets or sets a value indicating whether auto-redirects should be allowed (false might cause more work for the developer since more requests have to be made manually)
		/// </summary>
		public Boolean AllowAutoRedirect
		{
			get { return this._AllowAutoRedirect; }
			set { this._AllowAutoRedirect = value; }
		}

		/// <summary>
		/// true, if BotSuites internal redirect method should be used (will only work if .AllowAutoRedirect is false), else false
		/// </summary>
		private Boolean _AllowBotSuiteAutoRedirect = false;

		/// <summary>
		///  Gets or sets a value indicating whether BotSuites internal redirect method should be used (will only work if .AllowAutoRedirect is false), else false
		/// </summary>
		public Boolean AllowBotSuiteAutoRedirect
		{
			get { return this._AllowBotSuiteAutoRedirect; }
			set { this._AllowBotSuiteAutoRedirect = value; }
		}

		/// <summary>
		/// defines the maximum of consecutive requests for AllowAutoRedirect and AllowBotSuiteAutoRedirect
		/// </summary>
		private Int32 _MaximumRedirectCount = 100;

		/// <summary>
		/// Gets or sets the maximum of consecutive requests for AllowAutoRedirect and AllowBotSuiteAutoRedirect
		/// </summary>
		public Int32 MaximumRedirectCount
		{
			get { return this._MaximumRedirectCount; }
			set { this._MaximumRedirectCount = value; }
		}

		/// <summary>
		/// sets Expect100Continue of the HttpWebRequest.ServiceProvider upon request
		/// </summary>
		private Boolean _Expect100Continue = false;

		/// <summary>
		///  Gets or sets a value indicating whether Expect100Continue of the HttpWebRequest.ServiceProvider is used upon request
		/// </summary>
		public Boolean Expect100Continue
		{
			get { return this._Expect100Continue; }
			set { this._Expect100Continue = value; }
		}

		/// <summary>
		/// returns the encoding of the last web-response
		/// </summary>
		private Encoding _LastResponseEncoding = null;

		/// <summary>
		/// Gets the encoding of the last web-response
		/// </summary>
		public Encoding LastResponseEncoding
		{
			get { return this._LastResponseEncoding; }
		}

		/// <summary>
		/// Determines whether or not the HttpClient should ignore SSL/TLS certificate validation failures
		/// </summary>
		private Boolean _IgnoreCertificateValidationFailures = false;

		/// <summary>
		/// Gets or sets a value indicating whether or not the HttpClient should ignore SSL/TLS certificate validation failures
		/// </summary>
		public Boolean IgnoreCertificateValidationFailures
		{
			get { return this._IgnoreCertificateValidationFailures; }
			set { this._IgnoreCertificateValidationFailures = value; }
		}

		/// <summary>
		/// The decompression method
		/// </summary>
		private DecompressionMethods _DecompressionMethod = DecompressionMethods.None;

		/// <summary>
		/// Gets or sets the decompression method.
		/// </summary>
		public DecompressionMethods DecompressionMethod
		{
			get { return this._DecompressionMethod; }
			set { this._DecompressionMethod = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpClient"/> class
		/// </summary>
		/// <param name="useragent">a useragent string</param>
		/// <param name="initialReferer">the Referer the first request will be sent from, i.e. www.google.com</param>
		public HttpClient(String useragent, String initialReferer = null)
		{
			this._UserAgent = useragent;
			this._Referer = initialReferer;
		}

		/// <summary>
		/// sends a HTTP POST request to a given URL with given POST data and a optional referer
		/// </summary>
		/// <example>
		/// <code>
		/// var hpdc = new HttpPostDataCollection();
		/// hpdc.Add(new HttpPostData("sender", "wieschoo");
		/// hpdc.Add(new HttpPostData("message", "hallo welt");
		/// var hc = new HttpClient("some-user-agent", "www.google.de");
		/// var html = hc.POST("http://www.codebot.de", hpdc);
		/// </code>
		/// </example>
		/// <typeparam name="TRespType"></typeparam>
		/// <param name="url">the URL to send the post request to</param>
		/// <param name="postdata">the POST data</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns>the response as TRespType</returns>
		public TRespType POST<TRespType>(String url, HttpPostDataCollection postdata, String referer = null) where TRespType : class
		{
			return this.POST<TRespType>(url, postdata.ToString(), referer);
		}

		/// <summary>
		/// sends a HTTP POST request to a given URL with given POST data and a optional referer...
		/// (better use overload with HttpPostDataCollection parameter, it's easier to use and more flexible)
		/// </summary>
		/// <example>
		/// <code><![CDATA[
		/// var html = hc.POST("http://www.codebot.de",
		///		System.Web.HttpUtility.UrlEncode("sender") + "="
		///		+ System.Web.HttpUtility.UrlEncode("wieschoo") + "&"
		///		+ System.Web.HttpUtility.UrlEncode("message") + "="
		///		+ System.Web.HttpUtility.UrlEncode("hallo welt"));
		/// ]]></code>
		/// </example>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="url">the URL to send the post request to</param>
		/// <param name="postdata">the POST data</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns>the response as TRespType</returns>
		public TRespType POST<TRespType>(String url, String postdata, String referer = null) where TRespType : class
		{
			if(this._AllowBotSuiteAutoRedirect && !this._AllowAutoRedirect)
			{
				TRespType response = null;
				Int32 requestCount = 0;
				while(true)
				{
					if(requestCount > this._MaximumRedirectCount)
					{
						throw new HttpRequestException("too many automatic redirects");
					}

					if(requestCount <= 0)
					{
						response = this._POST<TRespType>(url, postdata, referer);
					}
					else if(this._Headers.Contains(HttpConstants.HeaderNames.Location))
					{
						response = this._GET<TRespType>(this.Headers[HttpConstants.HeaderNames.Location].Value);
					}
					else
					{
						break;
					}

					requestCount++;
				}

				return response;
			}
			else
			{
				return this._POST<TRespType>(url, postdata, referer);
			}
		}

		/// <summary>
		/// internal POST method
		/// </summary>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="url">url to send post request to</param>
		/// <param name="postdata">data of the post request</param>
		/// <param name="referer">referer for the post request</param>
		/// <returns>response as TRespType of site</returns>
		private TRespType _POST<TRespType>(String url, String postdata, String referer = null) where TRespType : class
		{
			this._Headers.Clear(); // DerpyHooves 2013-06-21

			if(referer != null)
			{
				this._Referer = referer;
			}

			url = CorrectUrl(url);

			TRespType response = null;
			try
			{
				HttpWebRequest req = this.PrepareRequest(url, "POST");
				req.ContentType = "application/x-www-form-urlencoded";
				byte[] data = Encoding.Default.GetBytes(postdata);
				req.ContentLength = data.Length;
				using(System.IO.Stream s = req.GetRequestStream())
				{
					s.Write(data, 0, data.Length);
				}

				response = this.GetResponse<TRespType>(req);
			}
			catch(Exception ex)
			{
				throw new HttpRequestException("POST request to " + url + " failed.", ex);
			}

			return response;
		}

		/// <summary>
		/// sends a HTTP GET request to a given URL with a optional referer
		/// </summary>
		/// <example>
		/// <code>
		/// var hc = new HttpClient("some-user-agent", "www.google.de");
		/// var html = hc.GET("http://www.codebot.de");
		/// </code>
		/// </example>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="url">the URL to send the request to</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns>returns the response as TRespType of the requested website</returns>
		public TRespType GET<TRespType>(String url, String referer = null) where TRespType : class
		{
			if(this._AllowBotSuiteAutoRedirect && !this._AllowAutoRedirect)
			{
				TRespType response = null;
				Int32 requestCount = 0;
				while(true)
				{
					if(requestCount > this._MaximumRedirectCount)
					{
						throw new HttpRequestException("too many automatic redirects");
					}

					if(requestCount <= 0)
					{
						response = this._GET<TRespType>(url, referer);
					}
					else if(this._Headers.Contains(HttpConstants.HeaderNames.Location))
					{
						response = this._GET<TRespType>(this.Headers[HttpConstants.HeaderNames.Location].Value);
					}
					else
					{
						break;
					}

					requestCount++;
				}

				return response;
			}
			else
			{
				return this._GET<TRespType>(url, referer);
			}
		}
		
		/// <summary>
		/// internal GET method
		/// </summary>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="url">url to send request to</param>
		/// <param name="referer">referer for the request</param>
		/// <returns>response as TRespType of the requested website</returns>
		private TRespType _GET<TRespType>(String url, String referer = null) where TRespType : class
		{
			this._Headers.Clear();

			if(referer != null)
			{
				this._Referer = referer;
			}

			url = CorrectUrl(url);

			TRespType response = null;
			try
			{
				HttpWebRequest req = this.PrepareRequest(url, "GET");
				response = this.GetResponse<TRespType>(req);
			}
			catch(Exception ex)
			{
				throw new HttpRequestException("GET request to " + url + " failed.", ex);
			}

			return response;
		}

		/// <summary>
		/// sends a HTTP HEAD request to a given URL with a optional referer
		/// </summary>
		/// <example>
		/// <code>
		/// var hc = new HttpClient("some-user-agent", "www.google.de");
		/// var html = hc.HEAD("http://www.codebot.de");
		/// </code>
		/// </example>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="url">the URL to send the request to</param>
		/// <param name="referer">the referer to send the request from</param>
		public void HEAD<TRespType>(String url, String referer = null) where TRespType : class
		{
			if(this._AllowBotSuiteAutoRedirect && !this._AllowAutoRedirect)
			{
				Int32 requestCount = 0;
				while(true)
				{
					if(requestCount > this._MaximumRedirectCount)
					{
						throw new HttpRequestException("too many automatic redirects");
					}

					if(requestCount <= 0)
					{
						this._HEAD<TRespType>(url, referer);
					}
					else if(this._Headers.Contains(HttpConstants.HeaderNames.Location))
					{
						this._HEAD<TRespType>(this.Headers[HttpConstants.HeaderNames.Location].Value);
					}
					else
					{
						break;
					}

					requestCount++;
				}
			}
			else
			{
				this._HEAD<TRespType>(url, referer);
			}
		}

		/// <summary>
		/// internal HEAD request method
		/// </summary>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="url">url for the head request</param>
		/// <param name="referer">referer for the head request</param>
		private void _HEAD<TRespType>(String url, String referer = null) where TRespType : class
		{
			this._Headers.Clear(); // DerpyHooves 2013-06-21

			if(referer != null)
			{
				this._Referer = referer;
			}

			url = CorrectUrl(url);

			try
			{
				HttpWebRequest req = this.PrepareRequest(url, "HEAD");
				this.GetResponse<TRespType>(req);
			}
			catch(Exception ex)
			{
				throw new HttpRequestException("HEAD request to " + url + " failed.", ex);
			}
		}

		/// <summary>
		/// corrects some standard-failures made when someone writes and URL, like forgetting the protocol at the beginning
		/// </summary>
		/// <param name="url">the URL that probably needs some correction</param>
		/// <returns>the, if it was necessary, corrected URL</returns>
		private static String CorrectUrl(String url)
		{
			// DerpyHooves 2013-05-02: URL URI-Format failover
			if(!url.StartsWith("http://") && !url.StartsWith("https://"))
			{
				url = "http://" + url;
			}

			// ... Derpy Hooves
			return url;
		}

		/// <summary>
		/// Creates a HttpWebRequest and prepares it for usage (e.g. sets method and other parameters)
		/// </summary>
		/// <param name="url">The url to request will be used for</param>
		/// <param name="method">the method to use, e.g. POST or GET</param>
		/// <returns>a fully prepared HttpWebRequest</returns>
		private HttpWebRequest PrepareRequest(String url, string method)
		{
			this._Cache = null;

			HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

			ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errors) => { return this._IgnoreCertificateValidationFailures; };

			req.CookieContainer = this._Cookies;
			req.Method = method;
			req.UserAgent = this._UserAgent;
			req.AutomaticDecompression = this._DecompressionMethod;
			req.ServicePoint.Expect100Continue = this._Expect100Continue;
			req.AllowAutoRedirect = this._AllowAutoRedirect;
			req.Credentials = CredentialCache.DefaultCredentials;
			if(this._Referer != null)
			{
				req.Referer = this._Referer;
			}

			if(this._UseProxy && (this._Proxy != null))
			{
				req.Proxy = this._Proxy.GetWebProxy();
				req.Credentials = req.Proxy.Credentials;
			}

			return req;
		}

		/// <summary>
		/// Receives the response for a HttpWebRequest
		/// </summary>
		/// <typeparam name="TRespType">the return type for the response</typeparam>
		/// <param name="req">the HttpWebRequest the return is wanted for</param>
		/// <returns>the response as TRespType for the request</returns>
		private TRespType GetResponse<TRespType>(HttpWebRequest req) where TRespType : class
		{
			TRespType response = null;
			using(HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				foreach(String headerKey in resp.Headers.AllKeys)
				{
					this.Headers.Add(new HttpHeader(headerKey, resp.Headers[headerKey]));
				}

				foreach(Cookie c in resp.Cookies)
				{
					this.RepairCookie(resp.ResponseUri, c);
				}

				using(System.IO.Stream s = resp.GetResponseStream())
				{
					this._LastResponseEncoding = (!String.IsNullOrEmpty(resp.CharacterSet)) ? Encoding.GetEncoding(resp.CharacterSet) : Encoding.Default;

					using(BinaryReader br = new BinaryReader(s, this._LastResponseEncoding))
					{
						System.Collections.Generic.List<byte> bytes = new System.Collections.Generic.List<byte>();
						byte[] buffer = new byte[1];
						int bytesRead = 0;

						while((bytesRead = br.Read(buffer, 0, buffer.Length)) > 0)
						{
							bytes.AddRange(buffer);
						}

						this._Cache = bytes.ToArray();
					}

					if(this._Cache != null)
					{
						if(typeof(TRespType) == typeof(String))
							response = (LoadStringFromCache() as TRespType);
						else if(typeof(TRespType) == typeof(Image))
							response = (LoadImageFromCache() as TRespType);

						if(this._AutoReferer)
						{
							this._Referer = resp.ResponseUri.AbsolutePath;
						}
					}
				}
			}

			return response;
		}

		/// <summary>
		/// Converts the cache of this HttpClient into a string
		/// </summary>
		/// <returns>the string from the cache</returns>
		private String LoadStringFromCache()
		{
			String retStr = null;

			if(this._Cache != null)
				retStr = (this._LastResponseEncoding ?? Encoding.Default).GetString(this._Cache);

			return retStr;
		}

		/// <summary>
		/// Converts the cache of this HttpClient into an image
		/// </summary>
		/// <returns>the image from the cache</returns>
		private Image LoadImageFromCache()
		{
			Image retImg = null;

			if(this._Cache != null)
			{
				try
				{
					using(MemoryStream ms = new MemoryStream())
					{
						BinaryWriter bw = new BinaryWriter(ms, this._LastResponseEncoding ?? Encoding.Default);
						bw.Write(this._Cache);
						ms.Seek(0, SeekOrigin.Begin);
						retImg = Image.FromStream(ms);
						bw.Dispose();
					}
				}
				catch
				{
					return null;
				}
			}

			return retImg;
		}

		/// <summary>
		/// repairs a cookie (path and uri)
		/// </summary>
		/// <param name="u">the uri the cookie is for</param>
		/// <param name="c">the cookie to repair</param>
		private void RepairCookie(Uri u, Cookie c)
		{
			string path = c.Path;
			if(!path.EndsWith("/"))
			{
				if(!path.Contains("/"))
				{
					path = "/" + path;
				}

				path = c.Path.Remove(c.Path.LastIndexOf('/') + 1);
			}

			Cookie nc = new Cookie(c.Name, c.Value, path, c.Domain);
			this._Cookies.Add(u, nc);
		}
	}
}