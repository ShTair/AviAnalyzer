using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AviAnalyzer.Models
{
    class List : Data
    {
        public string ListFourCC { get; }

        public List<Data> Datas { get; }

        public List(Stream stream, int offset, int length, string listFourCC) : base(stream, offset, length) { }

        public static async Task<List> Parse(Stream stream)
        {
            var fourCC = await stream.ReadFourCCAsync(0);
            var length = await stream.ReadInt32Async(4);

            var list = new List(stream, 0, length, fourCC);
            await list.ParseChildren(stream);
            return list;
        }

        private async Task ParseChildren(Stream stream)
        {
            FourCC = await stream.ReadFourCCAsync(_offset + 8);

            int current = 4;
            while (current < _length)
            {
                var fourCC = await stream.ReadFourCCAsync(_offset + current);
                var length = await stream.ReadInt32Async(_offset + current + 4);

                if (fourCC == "RIFF" || fourCC == "LIST")
                {
                    var list = new List(stream, _offset + current, length, fourCC);
                    Datas.Add(list);
                    await list.ParseChildren(stream);
                }
                else
                {
                    var chunk = new Chunk(stream, _offset + current, length, fourCC);
                    Datas.Add(chunk);
                }

                current += 8 + length;
            }
        }
    }
}
