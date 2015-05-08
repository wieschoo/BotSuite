// -----------------------------------------------------------------------
//  <copyright file="HttpProxyHacker.cs" company="Wieschoo &amp; Binary Overdrive">
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
	using System.Net.Configuration;
	using System.Reflection;

	/// <summary>
	/// class used to change some settings so that a proxy can be used
	/// </summary>
	internal class HttpProxyHacker
	{
		/// <summary>
		/// en-/disabled allow unsafe header parsing
		/// </summary>
		/// <param name="enable">
		/// determines whether to en- or disable this option
		/// </param>
		/// <returns>
		/// whether the operation succeeded or not
		/// </returns>
		public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
			if (assembly == null)
			{
				return false;
			}

			Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");

			if (settingsSectionType == null)
			{
				return false;
			}

			object objectInstance = settingsSectionType.InvokeMember(
				"Section",
				BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic,
				null,
				null,
				new object[] { });

			if (objectInstance == null)
			{
				return false;
			}

			FieldInfo fieldUseUnsafeHeaderParsing = settingsSectionType.GetField(
				"useUnsafeHeaderParsing",
				BindingFlags.NonPublic | BindingFlags.Instance);

			if (fieldUseUnsafeHeaderParsing == null)
			{
				return false;
			}

			fieldUseUnsafeHeaderParsing.SetValue(objectInstance, enable);

			return true;
		}

		/// <summary>
		/// checks if unsafe header parsing is active
		/// </summary>
		/// <returns>true, if unsafe header parsing is enabled, else false</returns>
		public static bool IsUseUnsafeHeaderParsingActivated()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
			if (assembly == null)
			{
				return false;
			}

			Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");

			if (settingsSectionType == null)
			{
				return false;
			}

			object objectInstance = settingsSectionType.InvokeMember(
				"Section",
				BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic,
				null,
				null,
				new object[] { });

			if (objectInstance == null)
			{
				return false;
			}

			FieldInfo fieldUseUnsafeHeaderParsing = settingsSectionType.GetField(
				"useUnsafeHeaderParsing",
				BindingFlags.NonPublic | BindingFlags.Instance);

			return fieldUseUnsafeHeaderParsing != null && (bool)fieldUseUnsafeHeaderParsing.GetValue(objectInstance);
		}
	}
}