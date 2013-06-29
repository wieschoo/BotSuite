/* **************************************************************
 * Name:      BotSuite.NET
 * Purpose:   Framework for creating bots
 * Homepage:  http://www.wieschoo.com
 * Copyright: (c) 2013 wieschoo & enWare
 * License:   http://www.wieschoo.com/botsuite/license/
 * *************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BotSuite
{
    /// <summary>
    /// class to handle ini files
    /// </summary>
    public class IniParser
    {
        /// <summary>
        /// path to ini file
        /// </summary>
        public string Path;
        /// <summary>
        /// handles ini files
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // load "config.ini" from the application directory
        /// IniParser Config = new IniParser("\config.ini");
        /// // write something
        /// Config.Write("SomeVariable", "ValueToWrite");
        /// // read something
        /// string Get = Config.Read("SomeVariable");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="IniPath">file to open, either a relative or a absolute path (relative pathes start with "\"
		/// and for can only be pathes the same level as executable or below in file tree) --- UNCs not supported at the moment!</param>
        public IniParser(string IniPath)
        {
			var absolutePath = IniPath;
			var pathIsRelative = IniPath.StartsWith("\\");
			if(pathIsRelative)
			{
				var currentDir = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
				absolutePath = System.IO.Path.Combine(currentDir, IniPath.Trim('\\'));
			}
			Path = absolutePath;
        }

        /// <summary>
        /// writes an information into the ini file
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // load "config.ini" from the application directory
        /// IniParser Config = new IniParser("config");
        /// Config.Write("Section", "Variable", "Value");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Section">section in ini file</param>
        /// <param name="Key">name of variable</param>
        /// <param name="Value">value of variable</param>
        public void Write(string Section, string Key, string Value)
        {
            NativeMethods.WritePrivateProfileString(Section, Key, Value, this.Path);
        }

        /// <summary>
        /// writes an information into the ini file
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // load "config.ini" from the application directory
        /// IniParser Config = new IniParser("config");
        /// Config.Write("Variable", "Value");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Key">name of variable</param>
        /// <param name="Value">value of variable</param>
        public void Write(string Key, string Value)
        {
            NativeMethods.WritePrivateProfileString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, Key, Value, this.Path);
        }

        /// <summary>
        /// writes an information into the ini file
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // load "config.ini" from the application directory
        /// IniParser Config = new IniParser("config");
        /// string Get = Config.Read("Section", "Variable");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Section">section in ini file</param>
        /// <param name="Key">name of variable</param>
        /// <returns>value of variable</returns>
        public string Read(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = NativeMethods.GetPrivateProfileString(Section, Key, "", temp, 255, this.Path);
            return temp.ToString();
        }

        /// <summary>
        /// reads an information from the ini file
        /// </summary>
        /// <example>
        /// <code>
        /// <![CDATA[
        /// // load "config.ini" from the application directory
        /// IniParser Config = new IniParser("config");
        /// string Get = Config.Read("Variable");
        /// ]]>
        /// </code>
        /// </example>
        /// <param name="Key">name of variable</param>
        /// <returns>value of variable</returns>
        public string Read(string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            int i = NativeMethods.GetPrivateProfileString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name, Key, "", temp, 255, this.Path);
            return temp.ToString();
        }
    }
}
