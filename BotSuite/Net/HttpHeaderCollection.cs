// -----------------------------------------------------------------------
//  <copyright file="HttpHeaderCollection.cs" company="Binary Overdrive">
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
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     Represents a collection of multiple <see cref="HttpHeader"/> instances.
	/// </summary>
	public class HttpHeaderCollection : List<HttpHeader>
	{
		/// <summary>
		///     Looks for an instance of the <see cref="HttpHeader"/> class in this collection by its key.
		/// </summary>
		/// <param name="key">
		///     The key to search for.
		/// </param>
		/// <returns>
		///     An instance of the <see cref="HttpHeader"/> class which has the given key.
		/// </returns>
		public HttpHeader this[string key]
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
		///     Looks for an instance of the <see cref="HttpHeader"/> class in this collection by its key.
		/// </summary>
		/// <param name="key">
		///     The key to search for.
		/// </param>
		/// <returns>
		///     An instance of the <see cref="HttpHeader"/> class which has the given key.
		/// </returns>
		public HttpHeader GetHeaderByKey(string key)
		{
			return this.FirstOrDefault(tmpHeader => tmpHeader.Key == key);
		}

		/// <summary>
		///     Checks if this collection contains an instance of the <see cref="HttpHeader"/> class with a given key.
		/// </summary>
		/// <param name="key">
		///     The key to look for.
		/// </param>
		/// <returns>
		///     True, if this collection contains an instance of the <see cref="HttpHeader"/> class with the given key, else false.
		/// </returns>
		public bool Contains(string key)
		{
			return this.GetHeaderByKey(key) != null;
		}

		/// <summary>
		///     Creates a string representation of this instance of the <see cref="HttpHeaderCollection"/> class.
		/// </summary>
		/// <returns>A string representation of this instance of the <see cref="HttpHeaderCollection"/> class.</returns>
		public override string ToString()
		{
			string ret = string.Empty;

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