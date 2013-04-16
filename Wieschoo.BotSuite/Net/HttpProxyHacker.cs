using System;
using System.Net.Configuration;
using System.Reflection;

namespace BotSuite.Net
{
	internal class HttpProxyHacker
	{
		public static Boolean ToggleAllowUnsafeHeaderParsing(Boolean enable)
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
			if(assembly != null)
			{
				Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
				if(settingsSectionType != null)
				{
					Object anInstance = settingsSectionType.InvokeMember(
						"Section",
						BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic,
						null,
						null,
						new Object[] { });
					if(anInstance != null)
					{
						FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
						if(aUseUnsafeHeaderParsing != null)
						{
							aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
							return true;
						}
					}
				}
			}
			return false;
		}

		public static Boolean IsUseUnsafeHeaderParsingActivated()
		{
			Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
			if(assembly != null)
			{
				Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
				if(settingsSectionType != null)
				{
					Object anInstance = settingsSectionType.InvokeMember(
						"Section",
						BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic,
						null,
						null,
						new Object[] { });
					if(anInstance != null)
					{
						FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
						if(aUseUnsafeHeaderParsing != null)
						{
							return (Boolean)aUseUnsafeHeaderParsing.GetValue(anInstance);
						}
					}
				}
			}
			return false;
		}
	}
}
