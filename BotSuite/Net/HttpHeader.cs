using System;

namespace BotSuite.Net
{
	public class HttpHeader : IEquatable<HttpHeader>, IComparable<HttpHeader>
	{
		public String Key { get; private set; }
		public String Value { get; private set; }

		public HttpHeader(String key, String value)
		{
			this.Key = key;
			this.Value = value;
		}

		public Boolean Equals(HttpHeader other)
		{
			return this.Key.Equals(other.Key);
		}

		public Int32 CompareTo(HttpHeader other)
		{
			return this.Key.CompareTo(other.Key);
		}

		public override string ToString()
		{
			return "HttpHeader->{" + this.Key + ": " + this.Value + "}";
		}
	}
}
