using AviAnalyzer.Models;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;

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
            var list = await List.ParseAsync(target);

            byte[] buffer = new byte[0];
            var txChuncs = ((List)list.Chunks[2]).Chunks.Where(t => t.FourCC == "03tx");
            foreach (var tx in txChuncs)
            {
                if (buffer.Length < tx.DataLength)
                {
                    buffer = new byte[tx.DataLength];
                }

                var count = await tx.ReadDataAsync(buffer, 0);
                var str = Encoding.UTF8.GetString(buffer, 0, count);
                Console.WriteLine(str);
            }
        }
    }
}
