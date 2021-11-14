// https://stackoverflow.com/questions/21691677/how-to-retrieve-integer-value-from-binary-file-in-c-sharp

namespace Gstd
{
    namespace File
    {
        abstract class FileReader : IReader
        {
            private string pathOriginal = "";
            private void _SetOriginalPath(string path)
            {
                pathOriginal = path;
            }
            public abstract int Read(byte[] buf, int size);
            public abstract bool Open();
            public abstract void Close();
            public abstract int GetFileSize();
            public abstract bool SetFilePointerBegin();
            public abstract bool SetFilePointerEnd();
            public abstract bool Seek(long offset);
            public abstract long GetFilePointer();
            public bool IsArchived()
            {
                return false;
            }
            public bool IsCompressed()
            {
                return false;
            }
            public string GetOriginalPath()
            {
                return pathOriginal;
            }
            public string ReadAllString()
            {
                SetFilePointerBegin();
                return Reader.ReadString(this, GetFileSize());
            }
        }
    }
}