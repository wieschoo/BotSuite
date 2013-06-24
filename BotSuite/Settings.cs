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
    [Serializable]
    public class Settings<T>
    {
        Dictionary<String, T> settings;
        
        T Read<T>(String Idx)
        {
            if (settings.ContainsKey(Idx))
                return (T)(object)settings[Idx];
            else return (T)(object) null;
        }

        public void Store(string file)
        {
            IFormatter binFmt = new BinaryFormatter();
            Stream s = File.Open(file, FileMode.Create);
            binFmt.Serialize(s, this);
            s.Close();
        }

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
