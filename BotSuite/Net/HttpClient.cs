/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/

using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace BotSuite.Net
{
	/// <summary>
	/// HttpClient class for making bots for Browsergames
	/// </summary>
	public class HttpClient
	{
		CookieContainer _Cookies = new CookieContainer();
		String _UserAgent = String.Empty;

		private Boolean _AutoReferer = true;
		/// <summary>
		/// true, if the HttpClient should adjust the Referer at every change of the URL
		/// </summary>
		public Boolean AutoReferer
		{
			get
			{
				return this._AutoReferer;
			}
			set
			{
				this._AutoReferer = value;
			}
		}

		private String _Referer = null;
		/// <summary>
		/// the current Referer
		/// </summary>
		public String Referer
		{
			get
			{
				return this._Referer;
			}
			set
			{
				this._Referer = value;
			}
		}

		private Boolean _UseProxy = false;
		/// <summary>
		/// true, if the HttpClient should use the HttpClient.Proxy for requests
		/// </summary>
		public Boolean UseProxy
		{
			get { return this._UseProxy; }
			set { this._UseProxy = value; }
		}

		private HttpProxy _Proxy = null;
		/// <summary>
		/// proxy object with settings for proxy usage for requests (when HttpClient.UseProxy is set to true)
		/// </summary>
		public HttpProxy Proxy
		{
			get { return this._Proxy; }
			set { this._Proxy = value; }
		}

		/// <summary>
		/// useUnsafeHeaderParsing
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
					throw new Exception("unable to set useUnsafeHeaderParsing in configuration");
			}
		}

		/// <summary>
		/// constructor for the HttpClient class
		/// </summary>
		/// <param name="useragent">a useragent string</param>
		/// <param name="proxy">a proxy settings container (aka. HttpProxy)</param>
		/// <param name="useProxy">defines if proxy should be used or not for requests</param>
		/// <param name="initialReferer">the Referer the first request will be sent from, i.e. www.google.com</param>
		/// <param name="useUnsafeHeaderParsing">useUnsafeHeaderParsing</param>
		public HttpClient(String useragent, HttpProxy proxy = null, Boolean useProxy = false, String initialReferer = null, Boolean useUnsafeHeaderParsing = false)
		{
			this._UserAgent = useragent;
			this._Referer = initialReferer;
			this._Proxy = proxy;
			this._UseProxy = useProxy;

			this.UseUnsafeHeaderParsing = useUnsafeHeaderParsing;
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
		/// <param name="url">the URL to send the post request to</param>
		/// <param name="postdata">the POST data</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns>the HTML sourcecode</returns>
		public String POST(String url, HttpPostDataCollection postdata, String referer = null, bool expect100continue = true)
		{
			return this.POST(url, postdata.ToString(), referer, expect100continue);
		}

		/// <summary>
		/// sends a HTTP POST request to a given URL with given POST data and a optional referer...
		/// (better use overload with HttpPostDataCollection parameter, it's easier to use and more flexible)
		/// </summary>
		/// <example>
		/// <code>
		/// var html = hc.POST("http://www.codebot.de",
		///		System.Web.HttpUtility.UrlEncode("sender") + "="
		///		+ System.Web.HttpUtility.UrlEncode("wieschoo") + "&"
		///		+ System.Web.HttpUtility.UrlEncode("message") + "="
		///		+ System.Web.HttpUtility.UrlEncode("hallo welt"));
		/// </code>
		/// </example>
		/// <param name="url">the URL to send the post request to</param>
		/// <param name="postdata">the POST data</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns>the HTML sourcecode</returns>
		public String POST(String url, String postdata, String referer = null, bool expect100continue = true)
		{
			if(referer != null)
				this._Referer = referer;

			url = CorrectUrl(url);

			String src = null;
			try
			{
				HttpWebRequest req = PrepareRequest(url, expect100continue, "POST");
				req.ContentType = "application/x-www-form-urlencoded";
				byte[] data = Encoding.Default.GetBytes(postdata);
				req.ContentLength = data.Length;
				using(System.IO.Stream s = req.GetRequestStream())
				{
					s.Write(data, 0, data.Length);
				}
				src = GetResponse(req);
			}
			catch(Exception ex)
			{
				throw new RequestException("POST request to " + url + " failed.", ex);
			}
			return src;
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
		/// <param name="url">the URL to send the request to</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns></returns>
		public String GET(String url, String referer = null, bool expect100continue = true)
		{
			if(referer != null)
				this._Referer = referer;

			url = CorrectUrl(url);

			String src = null;
			try
			{
				HttpWebRequest req = PrepareRequest(url, expect100continue, "GET");
				src = GetResponse(req);
			}
			catch(Exception ex)
			{
				throw new RequestException("GET request to " + url + " failed.", ex);
			}
			return src;
		}

		/// <summary>
		/// tries to convert a sourcecode-string (i.e. from a POST or GET request) into an image (works only if the response was an image)
		/// </summary>
		/// <example>
		/// <code>
		/// var hc = new HttpClient("some-user-agent", "www.google.de");
		/// var sourcecode = hc.GET("http://www.codebot.de");
		/// var img = HttpClient.SourcecodeToImage(sourcecode);
		/// </code>
		/// </example>
		/// <param name="sourcecode">the sourcecode-string that comes from a POTS or GET request</param>
		/// <returns>null, if unable to convert to image, else an System.Drawing.Image object</returns>
		public static Image SourcecodeToImage(String sourcecode)
		{
			Image retImg = null;
			try
			{
				using(MemoryStream ms = new MemoryStream())
				{
					StreamWriter sw = new StreamWriter(ms, Encoding.Default);
					sw.Write(sourcecode);
					ms.Seek(0, SeekOrigin.Begin);
					retImg = Image.FromStream(ms);
					sw.Dispose();
				}
			}
			catch
			{
				return null;
			}
			return retImg;
		}

		/// <summary>
		/// corrects some standard-failures made when someone writes and URL, like forgetting the protocol at the beginning
		/// </summary>
		/// <param name="url">the URL that probably needs some correction</param>
		/// <returns>the, if it was necessary, corrected URL</returns>
		private static string CorrectUrl(String url)
		{
			//DerpyHooves 2013-05-02: URL URI-Format failover
			if(!url.StartsWith("http://") && !url.StartsWith("https://"))
				url = "http://" + url;
			//... Derpy Hooves
			return url;
		}

		/// <summary>
		/// Creates a HttpWebRequest and prepares it for usage (e.g. sets method and other parameters)
		/// </summary>
		/// <param name="url">The url to request will be used for</param>
		/// <param name="expect100continue">Expect100Continue of the ServicePoint of the request</param>
		/// <param name="method">the method to use, e.g. POST or GET</param>
		/// <returns>a fully prepared HttpWebRequest</returns>
		private HttpWebRequest PrepareRequest(String url, bool expect100continue, string method)
		{
			HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);

			req.CookieContainer = _Cookies;
			req.Method = method;
			req.UserAgent = _UserAgent;
			req.ServicePoint.Expect100Continue = expect100continue; //DerpyHooves 2013-06-15
			if(this._Referer != null)
				req.Referer = this._Referer;
			//DerpyHooves 2013-04-16: added proxy ...
			if(this._UseProxy && (this._Proxy != null))
			{
				req.Proxy = this._Proxy.GetWebProxy();
				req.Credentials = req.Proxy.Credentials;
			}
			//... Derpy Hooves 2013-04-16
			return req;
		}

		/// <summary>
		/// Receives the response for a HttpWebRequest
		/// </summary>
		/// <param name="req">the HttpWebRequest the return is wanted for</param>
		/// <returns>the soourcecode for the request</returns>
		private string GetResponse(HttpWebRequest req)
		{
			String src = null;
			using(HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
			{
				using(System.IO.Stream s = resp.GetResponseStream())
				{
					Encoding enc = (!String.IsNullOrEmpty(resp.CharacterSet)) ? Encoding.GetEncoding(resp.CharacterSet) : Encoding.Default;
					using(System.IO.StreamReader sr = new System.IO.StreamReader(s, enc))
					{
						src = sr.ReadToEnd();
						if(this._AutoReferer)
							this._Referer = resp.ResponseUri.AbsolutePath;
					}
				}
			}
			return src;
		}
	}
}