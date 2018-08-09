using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AviAnalyzer.Models
{
    static class Extensions
    {
        private static byte[] _buffer = new byte[4];

        public static async Task<string> ReadFourCCAsync(this Stream stream, int offset)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            await stream.ReadAsync(_buffer, 0, 4);
            return Encoding.ASCII.GetString(_buffer);
        }

        public static async Task<int> ReadInt32Async(this Stream stream, int offset)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            await stream.ReadAsync(_buffer, 0, 4);
            return BitConverter.ToInt32(_buffer, 0);
        }
    }
}
