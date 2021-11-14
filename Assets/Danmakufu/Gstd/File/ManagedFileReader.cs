namespace Gstd
{
    namespace File
    {
        abstract class ManagedFileReader// : FileReader
        {
            private FileType type;
            //private File file;
            //private ArchiveFileEntry entry;
            //private ByteBuffer buffer;
            private int offset;

            //public ManagedFileReader(ref_count_ptr<File> file, ref_count_ptr<ArchiveFileEntry> entry);
            //public ~ManagedFileReader();

            public abstract bool Open();
            public abstract void Close();
            public abstract int GetFileSize();
            //public abstract DWORD Read(LPVOID buf,DWORD size);
            //public abstract bool SetFilePointerBegin();
            //public abstract bool SetFilePointerEnd();
            //public abstract bool Seek(long offset);
            //public abstract long GetFilePointer();

            public abstract bool IsArchived();
            public abstract bool IsCompressed();
        }
    }
}