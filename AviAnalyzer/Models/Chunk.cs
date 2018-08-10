using System.IO;
using System.Threading.Tasks;

namespace AviAnalyzer.Models
{
    class Chunk
    {
        protected Stream _stream;

        protected int _offset;

        protected int _length;

        public string FourCC { get; }

        public virtual int DataLength => _length;

        public Chunk(Stream stream, int offset, int length, string fourCC)
        {
            _stream = stream;
            _offset = offset;
            _length = length;
            FourCC = fourCC;
        }

        public async Task<int> ReadDataAsync(byte[] buffer, int offset)
        {
            _stream.Seek(_offset + 8, SeekOrigin.Begin);
            return await _stream.ReadAsync(buffer, offset, DataLength);
        }

        public override string ToString()
        {
            return $"{FourCC}: {_offset:X8} -> {_offset + _length + 8:X8}({_length + 8:X8}): {_offset + 8:X8}({_length:X8})";
        }
    }
}
