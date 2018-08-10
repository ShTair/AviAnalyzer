using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AviAnalyzer.Models
{
    class List : Chunk
    {
        public string ListFourCC { get; }

        public List<Chunk> Chunks { get; }

        public override int DataLength => _length - 4;

        public List(Stream stream, int offset, int length, string fourCC, string listFourCC) : base(stream, offset, length, fourCC)
        {
            ListFourCC = listFourCC;
            Chunks = new List<Chunk>();
        }

        public static async Task<List> ParseAsync(string fileName)
        {
            using (var stream = File.Open(fileName, FileMode.Open))
            {
                return await ParseAsync(stream);
            }
        }

        public static async Task<List> ParseAsync(Stream stream)
        {
            var listFourCC = await stream.ReadFourCCAsync(0);
            var length = await stream.ReadInt32Async(4);
            var fourCC = await stream.ReadFourCCAsync(8);

            var list = new List(stream, 0, length, fourCC, listFourCC);
            Console.WriteLine(list);
            await list.ParseChildren(stream, " ");
            return list;
        }

        private async Task ParseChildren(Stream stream, string indent)
        {
            int current = 4;
            while (current < _length)
            {
                var fourCC = await stream.ReadFourCCAsync(_offset + 8 + current);
                var length = await stream.ReadInt32Async(_offset + 8 + current + 4);

                if (fourCC == "RIFF" || fourCC == "LIST")
                {
                    var realFourCC = await stream.ReadFourCCAsync(_offset + 8 + current + 8);
                    var list = new List(stream, _offset + 8 + current, length, realFourCC, fourCC);
                    Console.WriteLine(indent + list);

                    await list.ParseChildren(stream, indent + " ");
                    Chunks.Add(list);
                }
                else
                {
                    var chunk = new Chunk(stream, _offset + 8 + current, length, fourCC);
                    Console.WriteLine(indent + chunk);
                    Chunks.Add(chunk);
                }

                current += 8 + length;
            }
        }

        public override string ToString()
        {
            return $"{FourCC}({ListFourCC}): {_offset:X8} -> {_offset + _length + 8:X8}({_length + 8:X8}): {_offset + 12:X8}({_length - 4:X8})";
        }
    }
}
