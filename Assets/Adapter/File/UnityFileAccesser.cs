using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using Port.File;

namespace Adapter.File
{
    public class UnityFileAccesser : FileAccesser
    {
#if UNITY_EDITOR
        static readonly string PERSISTENT_DATA_PATH = Regex.Replace(Application.dataPath, $"Assets$", "DevPersistentDataPath");
#else
        static readonly string PERSISTENT_DATA_PATH = Application.persistentDataPath;
#endif
        public static string GetPath(string fileName)
        {
            StringBuilder stringBuilder = new StringBuilder(PERSISTENT_DATA_PATH.Length + 1 + fileName.Length);
            stringBuilder.Append(PERSISTENT_DATA_PATH);
            stringBuilder.Append(Path.DirectorySeparatorChar);
            stringBuilder.Append(fileName);
            return stringBuilder.ToString();
        }
        public UnityFileAccesser() : base()
        {
        }
        public string Read(string fileName)
        {
            string filePath = GetPath(fileName);
            byte[] buffer = System.IO.File.ReadAllBytes(filePath);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }
        public void Write(string fileName, string content)
        {
            string filePath = GetPath(fileName);
            byte[] buffer = Encoding.UTF8.GetBytes(content);
            System.IO.File.WriteAllBytes(filePath, buffer);
        }
    }
}
