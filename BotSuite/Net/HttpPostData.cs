// -----------------------------------------------------------------------
//  <copyright file="HttpPostData.cs" company="Wieschoo &amp; Binary Overdrive">
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
	using System.Web;

	/// <summary>
	/// container-class for Http POST data key-value-pairs
	/// </summary>
	public class HttpPostData : IEquatable<HttpPostData>, IComparable<HttpPostData>
	{
		/// <summary>
		/// the key/name of the POST data
		/// </summary>
		private string key;

		/// <summary>
		/// Gets or sets the key/name of the POST data
		/// </summary>
		public string Key
		{
			get
			{
				return this.key;
			}

			set
			{
				this.key = value;
			}
		}

		/// <summary>
		/// the value of the POST data
		/// </summary>
		private string value;

		/// <summary>
		/// Gets or sets the value of the POST data
		/// </summary>
		public string Value
		{
			get
			{
				return this.value;
			}

			set
			{
				this.value = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpPostData" /> class
		/// </summary>
		/// <param name="key">
		/// key/name of the POST data
		/// </param>
		/// <param name="value">
		/// value of the POST data
		/// </param>
		public HttpPostData(string key, string value)
		{
			this.key = key;
			this.value = value;
		}

		/// <summary>
		/// creates a string-representation of the POST data
		/// </summary>
		/// <returns>a string representation of the POST data key-value-pair</returns>
		public override string ToString()
		{
			return string.Concat(HttpUtility.UrlEncode(this.key), '=', HttpUtility.UrlEncode(this.value));
		}

		/// <summary>
		/// checks if both instances of HTTP post data are equal
		/// </summary>
		/// <param name="other">
		/// the HttpPostData instance to compare equality with
		/// </param>
		/// <returns>
		/// true if both HttpPostData instances are equal, else false
		/// </returns>
		public bool Equals(HttpPostData other)
		{
			return this.key.Equals(other.key);
		}

		/// <summary>
		/// compares two HttpPostData instances, like for sorting in a list
		/// </summary>
		/// <param name="other">
		/// the HttpPostData instance to compare with
		/// </param>
		/// <returns>
		/// -1 if instance A is smaller, 0 if equal and 1 if instance A is greater then/to instance B
		/// </returns>
		public int CompareTo(HttpPostData other)
		{
			return string.Compare(this.key, other.key, StringComparison.Ordinal);
		}
	}
}