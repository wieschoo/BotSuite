using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace BotSuite
{
    public class Encryption
    {
        internal static T DecryptAndDeserialize<T>(string filename, string encryptionKey)
        {
            T local;
            ICryptoTransform transform = new DESCryptoServiceProvider().CreateDecryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(encryptionKey));
            using (FileStream stream = File.Open(filename, FileMode.Open))
            {
                using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read))
                {
                    local = (T)new System.Xml.Serialization.XmlSerializer(typeof(T)).Deserialize(stream2);
                }
            }
            return local;
        }

        internal static void EncryptAndSerialize<T>(string filename, T obj, string encryptionKey)
        {
            ICryptoTransform transform = new DESCryptoServiceProvider().CreateEncryptor(Encoding.ASCII.GetBytes("64bitPas"), Encoding.ASCII.GetBytes(encryptionKey));
            using (FileStream stream = File.Open(filename, FileMode.Create))
            {
                using (CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Write))
                {
                    new System.Xml.Serialization.XmlSerializer(typeof(T)).Serialize((Stream)stream2, obj);
                }
            }
        }
    }
}
