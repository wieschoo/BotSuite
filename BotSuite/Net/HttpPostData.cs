// -----------------------------------------------------------------------
//  <copyright file="HttpPostData.cs" company="Binary Overdrive">
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
	using System.Web;

	/// <summary>
	///     container-class for Http POST data key-value-pairs
	/// </summary>
	public class HttpPostData : IEquatable<HttpPostData>, IComparable<HttpPostData>
	{
		/// <summary>
		///     the key/name of the POST data
		/// </summary>
		private string _key;

		/// <summary>
		///     Gets or sets the key/name of the POST data
		/// </summary>
		public string Key
		{
			get
			{
				return this._key;
			}

			set
			{
				this._key = value;
			}
		}

		/// <summary>
		///     the value of the POST data
		/// </summary>
		private string _value;

		/// <summary>
		///     Gets or sets the value of the POST data
		/// </summary>
		public string Value
		{
			get
			{
				return this._value;
			}

			set
			{
				this._value = value;
			}
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpPostData" /> class
		/// </summary>
		/// <param name="key">
		///     key/name of the POST data
		/// </param>
		/// <param name="value">
		///     value of the POST data
		/// </param>
		public HttpPostData(string key, string value)
		{
			this._key = key;
			this._value = value;
		}

		/// <summary>
		///     creates a string-representation of the POST data
		/// </summary>
		/// <returns>a string representation of the POST data key-value-pair</returns>
		public override string ToString()
		{
			return string.Concat(HttpUtility.UrlEncode(this._key), '=', HttpUtility.UrlEncode(this._value));
		}

		/// <summary>
		///     checks if both instances of HTTP post data are equal
		/// </summary>
		/// <param name="other">
		///     the HttpPostData instance to compare equality with
		/// </param>
		/// <returns>
		///     true if both HttpPostData instances are equal, else false
		/// </returns>
		public bool Equals(HttpPostData other)
		{
			return this._key.Equals(other._key);
		}

		/// <summary>
		///     compares two HttpPostData instances, like for sorting in a list
		/// </summary>
		/// <param name="other">
		///     the HttpPostData instance to compare with
		/// </param>
		/// <returns>
		///     -1 if instance A is smaller, 0 if equal and 1 if instance A is greater then/to instance B
		/// </returns>
		public int CompareTo(HttpPostData other)
		{
			return string.Compare(this._key, other._key, StringComparison.Ordinal);
		}
	}
}