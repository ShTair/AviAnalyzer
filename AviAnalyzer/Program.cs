using AviAnalyzer.Models;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AviAnalyzer
{
    class Program
    {
        private static byte[] _buffer = new byte[4];

        static void Main(string[] args)
        {
            Run(args[0]).Wait();
        }

        private static async Task Run(string target)
        {
            using (var stream = File.Open(target, FileMode.Open))
            {
                var list = await List.Parse(stream);
                //await ReadChunk(stream, 0, "");
            }
        }

        private static async Task<int> ReadChunk(Stream stream, int offset, string indent)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            await stream.ReadAsync(_buffer, 0, 4);
            var id = Encoding.ASCII.GetString(_buffer);

            await stream.ReadAsync(_buffer, 0, 4);
            var length = BitConverter.ToInt32(_buffer, 0);

            if (id.Equals("RIFF", StringComparison.CurrentCultureIgnoreCase) || id.Equals("LIST", StringComparison.CurrentCultureIgnoreCase))
            {
                Console.Write($"{indent}{id}");
                return 8 + await ReadCapsule(stream, offset + 8, length, indent);
            }
            else
            {
                Console.Write($"{indent}{id}");
                Console.CursorLeft = 16;
                Console.WriteLine($"({Pad8(offset)} {Pad8(offset + 8)} -> {Pad8(offset + 8 + length)})({Pad8(length)})");
                return 8 + length;
            }
        }

        private static async Task<int> ReadCapsule(Stream stream, int offset, int length, string indent)
        {
            stream.Seek(offset, SeekOrigin.Begin);
            await stream.ReadAsync(_buffer, 0, 4);
            var id = Encoding.ASCII.GetString(_buffer);

            Console.Write($"({id})");
            Console.CursorLeft = 16;
            Console.WriteLine($"({Pad8(offset - 8)} {Pad8(offset + 4)} -> {Pad8(offset + length)})({Pad8(length - 4)})");

            var current = 4;
            while (current < length)
            {
                current += await ReadChunk(stream, offset + current, indent + " ");
            }

            return length;
        }

        private static string Pad8(int num)
        {
            return num.ToString("X2").PadLeft(8);
        }
    }
}
