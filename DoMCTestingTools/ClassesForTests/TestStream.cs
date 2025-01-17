namespace DoMCTestingTools.ClassesForTests
{
    public class TestStream : Stream
    {
        private byte[] data = new byte[0];
        private long CurrentReadPosition;
        private long CurrentWritePosition;
        public override bool CanRead => true;

        public override bool CanSeek => true;

        public override bool CanWrite => true;

        public override long Length => data.LongCount();

        public override long Position { get => CurrentReadPosition; set => CurrentReadPosition = value; }

        public override void Flush()
        {

        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            long readCount;
            if (CurrentReadPosition + count > Length)
                readCount = Length - CurrentReadPosition;
            else
                readCount = count;
            Array.Copy(data, CurrentReadPosition, buffer, offset, readCount);
            CurrentReadPosition += readCount;
            return (int)readCount;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            lock (this)
            {
                switch (origin)
                {
                    case SeekOrigin.Begin:
                        CurrentReadPosition = offset;
                        return CurrentReadPosition;
                    case SeekOrigin.Current:
                        CurrentReadPosition += offset;
                        CurrentWritePosition += offset;
                        return CurrentReadPosition;
                    case SeekOrigin.End:
                        CurrentWritePosition = Length - offset;
                        return CurrentWritePosition;
                    default:
                        return -1;
                }
            }
        }

        public override void SetLength(long value)
        {
            Array.Resize(ref data, (int)value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (count == 0) return;
            var newSize = CurrentWritePosition + count;
            if (newSize > Length)
                SetLength(newSize);
            Array.Copy(buffer, offset, data, CurrentWritePosition, count);
            CurrentWritePosition += count;
        }
    }
}
