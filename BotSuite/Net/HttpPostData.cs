//-----------------------------------------------------------------------
// <copyright file="HttpPostData.cs" company="Wieschoo &amp; enWare">
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

	/// <summary>
	/// container-class for Http POST data key-value-pairs
	/// </summary>
	public class HttpPostData : IEquatable<HttpPostData>, IComparable<HttpPostData>
	{
		/// <summary>
		/// the key/name of the POST data
		/// </summary>
		private String _Key;

		/// <summary>
		/// Gets or sets the key/name of the POST data
		/// </summary>
		public String Key
		{
			get
			{
				return this._Key;
			}

			set
			{
				this._Key = value;
			}
		}

		/// <summary>
		/// the value of the POST data
		/// </summary>
		private String _Value;

		/// <summary>
		/// Gets or sets the value of the POST data
		/// </summary>
		public String Value
		{
			get
			{
				return this._Value;
			}

			set
			{
				this._Value = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpPostData"/> class
		/// </summary>
		/// <param name="key">key/name of the POST data</param>
		/// <param name="value">value of the POST data</param>
		public HttpPostData(String key, String value)
		{
			this._Key = key;
			this._Value = value;
		}

		/// <summary>
		/// creates a string-representation of the POST data
		/// </summary>
		/// <returns>a string representation of the POST data key-value-pair</returns>
		public override String ToString()
		{
			return String.Concat(
				System.Web.HttpUtility.UrlEncode(this._Key),
				'=',
				System.Web.HttpUtility.UrlEncode(this._Value));
		}

		/// <summary>
		/// checks if both instances of HTTP post data are equal
		/// </summary>
		/// <param name="other">the HttpPostData instance to compare equality with</param>
		/// <returns>true if both HttpPostData instances are equal, else false</returns>
		public Boolean Equals(HttpPostData other)
		{
			return this._Key.Equals(other._Key);
		}

		/// <summary>
		/// compares two HttpPostData instances, like for sorting in a list
		/// </summary>
		/// <param name="other">the HttpPostData instance to compare with</param>
		/// <returns>-1 if instance A is smaller, 0 if equal and 1 if instance A is greater then/to instance B</returns>
		public Int32 CompareTo(HttpPostData other)
		{
			return this._Key.CompareTo(other._Key);
		}
	}
}
