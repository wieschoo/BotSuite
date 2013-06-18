/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/

using System;

namespace BotSuite.Net
{
	/// <summary>
	/// container-class for Http POST data key-value-pairs
	/// </summary>
	public class HttpPostData : IEquatable<HttpPostData>, IComparable<HttpPostData>
	{
		private String _Key;
		/// <summary>
		/// the key/name of the POST data
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

		private String _Value;
		/// <summary>
		/// the value of the POST data
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
		/// constructor for Http POST data key-value-pair
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
