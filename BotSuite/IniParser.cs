// -----------------------------------------------------------------------
//  <copyright file="IniParser.cs" company="HoovesWare">
//      Copyright (c) HoovesWare
//  </copyright>
//  <project>BotSuite.Net</project>
//  <purpose>framework for creating bots</purpose>
//  <homepage>http://botsuite.net/</homepage>
//  <license>http://botsuite.net/license/index/</license>
// -----------------------------------------------------------------------

namespace BotSuite
{
	using System.IO;
	using System.Reflection;
	using System.Text;

	/// <summary>
	///     class to handle ini files
	/// </summary>
	public class IniParser
	{
		/// <summary>
		///     path to ini file
		/// </summary>
		private readonly string path;

		/// <summary>
		///     Initializes a new instance of the <see cref="IniParser" /> class.
		/// </summary>
		/// <param name="iniPath">
		///     file to open, either a relative or a absolute path (relative pathes start with "\"
		///     and for can only be pathes the same level as executable or below in file tree) --- UNCs not supported at the
		///     moment!
		/// </param>
		public IniParser(string iniPath)
		{
			string absolutePath = iniPath;
			bool pathIsRelative = iniPath.StartsWith("\\");
			if (pathIsRelative)
			{
				string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				if (currentDir != null)
				{
					absolutePath = Path.Combine(currentDir, iniPath.Trim('\\'));
				}
			}

			this.path = absolutePath;
		}

		/// <summary>
		///     writes an information into the ini file
		/// </summary>
		/// <param name="section">
		///     section in ini file
		/// </param>
		/// <param name="key">
		///     name of variable
		/// </param>
		/// <param name="value">
		///     value of variable
		/// </param>
		public void Write(string section, string key, string value)
		{
			NativeMethods.WritePrivateProfileString(section, key, value, this.path);
		}

		/// <summary>
		///     writes an information into the ini file
		/// </summary>
		/// <param name="key">
		///     name of variable
		/// </param>
		/// <param name="value">
		///     value of variable
		/// </param>
		public void Write(string key, string value)
		{
			NativeMethods.WritePrivateProfileString(Assembly.GetExecutingAssembly().GetName().Name, key, value, this.path);
		}

		/// <summary>
		///     writes an information into the ini file
		/// </summary>
		/// <param name="section">
		///     section in ini file
		/// </param>
		/// <param name="key">
		///     name of variable
		/// </param>
		/// <returns>
		///     value of variable
		/// </returns>
		public string Read(string section, string key)
		{
			StringBuilder temp = new StringBuilder(255);
			NativeMethods.GetPrivateProfileString(section, key, string.Empty, temp, 255, this.path);
			return temp.ToString();
		}

		/// <summary>
		///     reads an information from the ini file
		/// </summary>
		/// <param name="key">
		///     name of variable
		/// </param>
		/// <returns>
		///     value of variable
		/// </returns>
		public string Read(string key)
		{
			StringBuilder temp = new StringBuilder(255);
			NativeMethods.GetPrivateProfileString(
				Assembly.GetExecutingAssembly().GetName().Name,
				key,
				string.Empty,
				temp,
				255,
				this.path);
			return temp.ToString();
		}
	}
}