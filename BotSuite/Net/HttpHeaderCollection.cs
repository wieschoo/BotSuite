﻿//-----------------------------------------------------------------------
// <copyright file="HttpHeaderCollection.cs" company="Wieschoo &amp; enWare">
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
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	/// represents a collection of multiple HttpHeader instances
	/// </summary>
	public class HttpHeaderCollection : List<HttpHeader>
	{
		/// <summary>
		/// looks for an instance of the HttpHeader class in this collection by its key
		/// </summary>
		/// <param name="key">the key to search for</param>
		/// <returns>an instance of the HttpHeader class which has the given key</returns>
		public HttpHeader this[String key]
		{
			get
			{
				HttpHeader ret = this.GetHeaderByKey(key);
				if(ret == null)
				{
					throw new IndexOutOfRangeException("Der key " + key + " ist nicht in der Auflistung vorhanden");
				}

				return ret;
			}
		}

		/// <summary>
		/// looks for an instance of the HttpHeader class in this collection by its key
		/// </summary>
		/// <param name="key">the key to search for</param>
		/// <returns>an instance of the HttpHeader class which has the given key</returns>
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

		/// <summary>
		/// checks if this collection contains an instance of the HttpHeader class with a given key
		/// </summary>
		/// <param name="key">the key to look for</param>
		/// <returns>true, if this collection contains an instance of the HttpHeader class with the given key, else false</returns>
		public Boolean Contains(String key)
		{
			return this.GetHeaderByKey(key) != null;
		}

		/// <summary>
		/// creates a string representation of this instance of the HttpHeaderCollection class
		/// </summary>
		/// <returns>a string representation of this instance of the HttpHeaderCollection class</returns>
		public override String ToString()
		{
			String ret = String.Empty;

			foreach(HttpHeader item in this)
			{
				ret += item.ToString();
				if(!item.Equals(this.Last()))
				{
					ret += Environment.NewLine;
				}
			}

			return ret;
		}
	}
}
