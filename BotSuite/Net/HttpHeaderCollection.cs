using System;
using System.Collections.Generic;
using System.Linq;

namespace BotSuite.Net
{
	public class HttpHeaderCollection : List<HttpHeader>
	{
		public HttpHeader this[String key]
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

		public override String ToString()
		{
			String ret = String.Empty;

			foreach(HttpHeader item in this)
			{
				ret += item.ToString();
				if(!item.Equals(this.Last()))
 					ret += Environment.NewLine;
			}

			return ret;
		}
	}
}
