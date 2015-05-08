// -----------------------------------------------------------------------
//  <copyright file="HttpHeaderCollection.cs" company="Wieschoo &amp; Binary Overdrive">
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
		/// <param name="key">
		/// the key to search for
		/// </param>
		/// <returns>
		/// an instance of the HttpHeader class which has the given key
		/// </returns>
		public HttpHeader this[string key]
		{
			get
			{
				HttpHeader ret = this.GetHeaderByKey(key);
				if (ret == null)
				{
					throw new IndexOutOfRangeException("Der key " + key + " ist nicht in der Auflistung vorhanden");
				}

				return ret;
			}
		}

		/// <summary>
		/// looks for an instance of the HttpHeader class in this collection by its key
		/// </summary>
		/// <param name="key">
		/// the key to search for
		/// </param>
		/// <returns>
		/// an instance of the HttpHeader class which has the given key
		/// </returns>
		public HttpHeader GetHeaderByKey(string key)
		{
			return this.FirstOrDefault(tmpHeader => tmpHeader.Key == key);
		}

		/// <summary>
		/// checks if this collection contains an instance of the HttpHeader class with a given key
		/// </summary>
		/// <param name="key">
		/// the key to look for
		/// </param>
		/// <returns>
		/// true, if this collection contains an instance of the HttpHeader class with the given key, else false
		/// </returns>
		public bool Contains(string key)
		{
			return this.GetHeaderByKey(key) != null;
		}

		/// <summary>
		/// creates a string representation of this instance of the HttpHeaderCollection class
		/// </summary>
		/// <returns>a string representation of this instance of the HttpHeaderCollection class</returns>
		public override string ToString()
		{
			string ret = string.Empty;

			foreach (HttpHeader item in this)
			{
				ret += item.ToString();
				if (!item.Equals(this.Last()))
				{
					ret += Environment.NewLine;
				}
			}

			return ret;
		}
	}
}