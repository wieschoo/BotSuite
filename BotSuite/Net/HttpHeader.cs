//-----------------------------------------------------------------------
// <copyright file="HttpHeader.cs" company="Wieschoo &amp; enWare">
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
	/// represents a HTTP header field
	/// </summary>
	public class HttpHeader : IEquatable<HttpHeader>, IComparable<HttpHeader>
	{
		/// <summary>
		/// Gets the name/key of the header field
		/// </summary>
		public String Key { get; private set; }

		/// <summary>
		/// Gets the value of the header field
		/// </summary>
		public String Value { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="HttpHeader"/> class
		/// </summary>
		/// <param name="key">the name/key of the header field</param>
		/// <param name="value">the value of the header field</param>
		public HttpHeader(String key, String value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		/// compares 2 instances of the HttpHeader class for equality
		/// </summary>
		/// <param name="other">the instance to compare to</param>
		/// <returns>true, if both are equal, else false</returns>
		public Boolean Equals(HttpHeader other)
		{
			return this.Key.Equals(other.Key);
		}

		/// <summary>
		/// compares the order of 2 instances of the HttpClass
		/// </summary>
		/// <param name="other">the instance to compare to</param>
		/// <returns>an integer determing the order of both elements</returns>
		public Int32 CompareTo(HttpHeader other)
		{
			return this.Key.CompareTo(other.Key);
		}

		/// <summary>
		/// creates a string representation of the current instance of this class
		/// </summary>
		/// <returns>a string representation of the current instance of this class</returns>
		public override String ToString()
		{
			return "HttpHeader->{" + this.Key + ": " + this.Value + "}";
		}
	}
}
