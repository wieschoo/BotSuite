// -----------------------------------------------------------------------
//  <copyright file="UserAgent.cs" company="Binary Overdrive">
//      Copyright (c) Binary Overdrive.
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>Framework for creating automation applications.</purpose>
//  <homepage>https://bitbucket.org/KarillEndusa/botsuite.net</homepage>
//  <license>https://bitbucket.org/KarillEndusa/botsuite.net/wiki/license</license>
// -----------------------------------------------------------------------

namespace BotSuite.Net
{
	/// <summary>
	/// Represents a browsers useragent
	/// </summary>
	public class UserAgent
	{
		private const string UA_CHROME33 = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML like Gecko) Chrome/33.0.1750.5 Safari/537.36";
		private const string UA_FF28 = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:28.0) Gecko/20100101 Firefox/28.0";
		private const string UA_IE11 = "Mozilla/5.0 (Windows NT 6.1; WOW64; Trident/7.0; rv:11.0) like Gecko";

		private static readonly UserAgent _chrome33 = new UserAgent(UA_CHROME33, "Chrome 33/Windows 7");
		private static readonly UserAgent _firefox28 = new UserAgent(UA_FF28, "Firefox 28/Windows 7 x64");
		private static readonly UserAgent _internetExplorer11 = new UserAgent(UA_IE11, "Internet Explorer 11/Windows 7 x64");

		/// <summary>
		/// Gets the useragent for the Chrome browser in version 33
		/// </summary>
		public static UserAgent Chrome33
		{
			get
			{
				return _chrome33;
			}
		}

		/// <summary>
		/// Gets the useragent for the Firefox browser in version 28
		/// </summary>
		public static UserAgent Firefox28
		{
			get
			{
				return _firefox28;
			}
		}

		/// <summary>
		/// Gets the useragent for the Internet Explorer browser in version 11
		/// </summary>
		public static UserAgent InternetExplorer11
		{
			get
			{
				return _internetExplorer11;
			}
		}

		/// <summary>
		/// Gets the actual useragent string
		/// </summary>
		public string UserAgentString { get; private set; }

		/// <summary>
		/// Gets the name of this instance of the <see cref="UserAgent"/> class
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="UserAgent"/> class
		/// </summary>
		/// <param name="userAgentString">The useragent string</param>
		/// <param name="name">The name</param>
		public UserAgent(string userAgentString, string name)
		{
			this.UserAgentString = userAgentString;
			this.Name = name;
		}

		/// <summary>
		/// Return a string representation of this instance of this <see cref="UserAgent"/> class
		/// </summary>
		/// <returns>A string representation of this instance of this <see cref="UserAgent"/> class</returns>
		public override string ToString()
		{
			return string.Format("{{{0} | {1}}}", this.Name, this.UserAgentString);
		}
	}
}