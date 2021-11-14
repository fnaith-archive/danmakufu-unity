// https://docs.microsoft.com/zh-tw/dotnet/api/system.io.binaryreader.readbytes?view=net-5.0

namespace Gstd
{
    namespace File
    {
        public interface IReader
        {
            int Read(byte[] buf, int size);
        }
        sealed class Reader // TODO
        {
            /*public template <typename T> DWORD Read(T& data)
            {
                return Read(&data, sizeof(T));
            }*/
            //public bool ReadBoolean(){bool res; Read(res);return res;}
            //public char ReadCharacter(){char res; Read(res);return res;}
            //public short ReadShort(){short res; Read(res);return res;}
            //public int ReadInteger(){int num; Read(num);return num;}
            //public _int64 ReadInteger64(){_int64 num; Read(num);return num;}
            //public float ReadFloat(){float num; Read(num);return num;}
            //public double ReadDouble(){double num; Read(num);return num;}

            public static string ReadString(IReader reader, int size)
            {
                byte[] buffer = new byte[size];
                reader.Read(buffer, size);
                return System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
        }
    }
}