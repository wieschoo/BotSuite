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

		/// <summary>
		/// constructor for the HttpClient class
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
		/// <param name="url">the URL to send the post request to</param>
		/// <param name="postdata">the POST data</param>
		/// <param name="referer">the referer to send the request from</param>
		/// <returns>the HTML sourcecode</returns>
		public String POST(String url, HttpPostDataCollection postdata, String referer = null)
		{
			if(referer != null)
				this._Referer = referer;

			String src = null;
			try
			{
				HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
				req.CookieContainer = _Cookies;
				req.Method = "POST";
				req.UserAgent = _UserAgent;
				req.ContentType = "application/x-www-form-urlencoded";
				if(this._Referer != null)
					req.Referer = this._Referer;
				byte[] data = Encoding.Default.GetBytes(postdata.ToString());
				req.ContentLength = data.Length;
				using(System.IO.Stream s = req.GetRequestStream())
				{
					s.Write(data, 0, data.Length);
				}
				using(HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
				{
					using(System.IO.Stream s = resp.GetResponseStream())
					{
						var enc = (!String.IsNullOrEmpty(resp.CharacterSet)) ? Encoding.GetEncoding(resp.CharacterSet) : Encoding.Default;
						using(System.IO.StreamReader sr = new System.IO.StreamReader(s, enc))
						{
							src = sr.ReadToEnd();
							if(this._AutoReferer)
								this._Referer = resp.ResponseUri.AbsolutePath;
						}
					}
				}
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
		public String GET(String url, String referer = null)
		{
			if(referer != null)
				this._Referer = referer;

			String src = null;
			try
			{
				HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
				req.CookieContainer = _Cookies;
				req.Method = "GET";
				req.UserAgent = _UserAgent;
				if(this._Referer != null)
					req.Referer = this._Referer;
				using(HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
				{
					using(System.IO.Stream s = resp.GetResponseStream())
					{
						var enc = (!String.IsNullOrEmpty(resp.CharacterSet)) ? Encoding.GetEncoding(resp.CharacterSet) : Encoding.Default;
						using(System.IO.StreamReader sr = new System.IO.StreamReader(s, enc))
						{
							src = sr.ReadToEnd();
							if(this._AutoReferer)
								this._Referer = resp.ResponseUri.AbsoluteUri;
						}
					}
				}
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
	}
}
