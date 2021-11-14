using System;

namespace Gstd
{
    namespace File
    {
        sealed class ByteBuffer : IReader, IWriter, System.IDisposable
        {
            private int reserve;
            private int size;
            private int offset;
            private byte[] data;
            
            private int _GetReservedSize()
            {
                return reserve;
            }
            private void _Resize(int size)
            {
                byte[] oldData = data;
                int oldSize = this.size;
                
                data = new byte[size];

                int sizeCopy = System.Math.Min(size, oldSize);
                Array.Copy(oldData, data, sizeCopy);
                reserve = size;
                this.size = size;

                oldData = null;
            }
            public ByteBuffer()
            {
                data = null;
                Clear();
            }
            public ByteBuffer(ByteBuffer buffer)
            {
                data = null;
                Clear();
                Copy(buffer);
            }
            public void Dispose()
            {
                Clear();
            }
            public void Copy(ByteBuffer src)
            {
                if (data != null && src.reserve != reserve)
                {
                    data = new byte[src.reserve];
                }

                offset = src.offset;
                reserve = src.reserve;
                size = src.size;

                Array.Copy(src.data, data, reserve);
            }
            public void Clear()
            {
                if (data != null)
                {
                    data = null;
                }

                data = new byte[0];
                offset = 0;
                reserve = 0;
                size = 0;
            }
            public void Seek(int pos)
            {
                offset = pos;
                if (offset < 0)
                {
                    offset = 0;
                }
                else if (offset > size)
                {
                    offset = size;
                }
            }
            public void SetSize(int size)
            {
                _Resize(size);
            }
            public int GetSize()
            {
                return size;
            }
            public int GetOffset()
            {
                return offset;
            }
            public int Write(byte[] buf, int size)
            {
                if (offset + size > reserve)
                {
                    int sizeNew = (int)((offset + size) * 2);
                    _Resize(sizeNew);
                    this.size = 0; // ���ƂōČv�Z
                }

                Array.Copy(buf, 0, data, offset, size);
                offset += size;
                this.size = System.Math.Max(this.size, offset);
                return size;
            }
            public int Read(byte[] buf, int size)
            {
                Array.Copy(data, offset, buf, 0, size);
                offset += size;
                return size;
            }
            public byte GetPointer(int offset = 0)
            {
                if (offset > size)
                {
                    throw new Exception("ByteBuffer:");//�C���f�b�N�X�G���[");
                }
                return data[offset];
            }
        }
    }
}