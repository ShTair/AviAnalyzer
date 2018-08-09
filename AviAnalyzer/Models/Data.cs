using System.IO;

namespace AviAnalyzer.Models
{
    abstract class Data
    {
        private Stream _stream;

        protected int _offset;

        protected int _length;

        public string FourCC { get; protected set; }

        public Data(Stream stream, int offset, int length)
        {
            _stream = stream;
            _offset = offset;
            _length = length;
        }
    }
}
