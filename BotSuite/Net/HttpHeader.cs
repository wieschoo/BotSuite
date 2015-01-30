// -----------------------------------------------------------------------
//  <copyright file="HttpHeader.cs" company="Binary Overdrive">
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

	/// <summary>
	///     represents a HTTP header field
	/// </summary>
	public class HttpHeader : IEquatable<HttpHeader>, IComparable<HttpHeader>
	{
		/// <summary>
		///     Gets the name/key of the header field
		/// </summary>
		public string Key { get; private set; }

		/// <summary>
		///     Gets the value of the header field
		/// </summary>
		public string Value { get; private set; }

		/// <summary>
		///     Initializes a new instance of the <see cref="HttpHeader" /> class
		/// </summary>
		/// <param name="key">
		///     the name/key of the header field
		/// </param>
		/// <param name="value">
		///     the value of the header field
		/// </param>
		public HttpHeader(string key, string value)
		{
			this.Key = key;
			this.Value = value;
		}

		/// <summary>
		///     compares 2 instances of the HttpHeader class for equality
		/// </summary>
		/// <param name="other">
		///     the instance to compare to
		/// </param>
		/// <returns>
		///     true, if both are equal, else false
		/// </returns>
		public bool Equals(HttpHeader other)
		{
			return this.Key.Equals(other.Key);
		}

		/// <summary>
		///     compares the order of 2 instances of the HttpClass
		/// </summary>
		/// <param name="other">
		///     the instance to compare to
		/// </param>
		/// <returns>
		///     an integer determing the order of both elements
		/// </returns>
		public int CompareTo(HttpHeader other)
		{
			return string.Compare(this.Key, other.Key, StringComparison.Ordinal);
		}

		/// <summary>
		///     creates a string representation of the current instance of this class
		/// </summary>
		/// <returns>a string representation of the current instance of this class</returns>
		public override string ToString()
		{
			return "HttpHeader->{" + this.Key + ": " + this.Value + "}";
		}
	}
}