using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
// todo this recursive
namespace BotSuite
{
    /// <summary>
    /// easy handle some object groups (add, read, store in file, load from file)
    /// </summary>
    /// <typeparam name="T">type of the objects</typeparam>
    [Serializable]
    public class Settings<T>
    {
        Dictionary<String, T> settings;
        /// <summary>
        /// reads an entry
        /// </summary>
        /// <param name="Idx">entry key</param>
        /// <returns></returns>
        T Read(String Idx)
        {
            if (settings.ContainsKey(Idx))
                return (T)(object)settings[Idx];
            else return (T)(object) null;
        }
        /// <summary>
        /// add an entry into settings
        /// </summary>
        /// <param name="Idx">entry key</param>
        /// <param name="value">entry data</param>
        void Write(String Idx,T value)
        {
            settings.Add(Idx, value);
        }
        /// <summary>
        /// save settings as binary file
        /// </summary>
        /// <param name="file">filename</param>
        public void Store(string file)
        {
            IFormatter binFmt = new BinaryFormatter();
            Stream s = File.Open(file, FileMode.Create);
            binFmt.Serialize(s, this);
            s.Close();
        }
        /// <summary>
        /// reads settings from binary file
        /// </summary>
        /// <param name="file">filename</param>
        /// <returns>settings</returns>
        public static Settings<T> Load(string file)
        {
            Settings<T> result;
            try
            {
                IFormatter binFmt = new BinaryFormatter();
                Stream s = File.Open(file, FileMode.Open);
                result = (Settings<T>)binFmt.Deserialize(s);
                s.Close();
            }
            catch (Exception e)
            {
                throw new Exception("settings : Unable to load file " + file + " : " + e);
            }
            return result;
        } 
    }
}
