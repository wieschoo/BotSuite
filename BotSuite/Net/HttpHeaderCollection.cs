using System;
using System.Collections.Generic;

namespace BotSuite.Net
{
	public class HttpHeaderCollection : List<HttpHeader>
	{
		public HttpHeader this[string key]
		{
			get
			{
				HttpHeader ret = this.GetHeaderByKey(key);
				if(ret == null)
					throw new IndexOutOfRangeException("Der key " + key + " ist nicht in der Auflistung vorhanden");
				return ret;
			}
		}

		public HttpHeader GetHeaderByKey(string key)
		{
			HttpHeader httpHeader = null;

			foreach(HttpHeader tmpHeader in this)
			{
				if(tmpHeader.Key == key)
				{
					httpHeader = tmpHeader;
					break;
				}
			}

			return httpHeader;
		}

		public Boolean Contains(string key)
		{
			return (this.GetHeaderByKey(key) != null);
		}
	}
}
