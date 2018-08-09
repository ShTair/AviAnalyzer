using System.IO;

namespace AviAnalyzer.Models
{
    class Chunk : Data
    {
        public Chunk(Stream stream, int offset, int length, string fourCC) : base(stream, offset, length)
        {
            FourCC = fourCC;
        }
    }
}
