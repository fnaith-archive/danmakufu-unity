// https://docs.microsoft.com/zh-tw/dotnet/api/system.io.binaryreader.readbytes?view=net-5.0

namespace Gstd
{
    namespace File
    {
        public interface IWriter
        {
            int Write(byte[] buf, int size);
        }
        sealed class Writer
        {
			/*template <typename T> DWORD Write(T& data)
			{
				return Write(&data, sizeof(T));
			}
			void WriteBoolean(bool b){Write(b);}
			void WriteCharacter(char ch){Write(ch);}
			void WriteShort(short num){Write(num);}
			void WriteInteger(int num){Write(num);}
			void WriteInteger64(_int64 num){Write(num);}
			void WriteFloat(float num){Write(num);}
			void WriteDouble(double num){Write(num);}*/
        }
    }
}