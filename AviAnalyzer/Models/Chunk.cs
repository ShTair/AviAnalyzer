using System.IO;

namespace AviAnalyzer.Models
{
    class Chunk
    {
        protected Stream _stream;

        protected int _offset;

        protected int _length;

        public string FourCC { get; }

        public Chunk(Stream stream, int offset, int length, string fourCC)
        {
            _stream = stream;
            _offset = offset;
            _length = length;
            FourCC = fourCC;
        }

        public override string ToString()
        {
            return $"{FourCC}: {_offset:X2} -> {_offset + _length + 8:X2}({_length + 8:X2}): {_offset + 8:X2}({_length:X2})";
        }
    }
}
