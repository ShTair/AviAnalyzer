using AviAnalyzer.Models;
using CsvHelper;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AviAnalyzer
{
    class Program
    {
        private static byte[] _buffer = new byte[4];
        private static Regex _regex = new Regex("[0-9.-]+", RegexOptions.Compiled);

        static void Main(string[] args)
        {
            Run(args[0]).Wait();
        }

        private static async Task Run(string target)
        {
            target = Path.GetFullPath(target);
            var list = await List.ParseAsync(target);

            byte[] buffer = new byte[0];
            var txChuncs = ((List)list.Chunks[2]).Chunks.Where(t => t.FourCC == "03tx");

            var name = Path.ChangeExtension(target, ".csv");
            using (var stream = new StreamWriter(name, false, Encoding.Default))
            using (var csv = new CsvWriter(stream))
            {
                foreach (var tx in txChuncs)
                {
                    if (buffer.Length < tx.DataLength)
                    {
                        buffer = new byte[tx.DataLength];
                    }

                    var count = await tx.ReadDataAsync(buffer, 0);
                    var str = Encoding.UTF8.GetString(buffer, 0, count);

                    Console.WriteLine(str);

                    csv.WriteField(str.Substring(0, 6));
                    csv.WriteField(str.Substring(7, 19));
                    csv.WriteField(str.Substring(29, 5).Trim());
                    csv.WriteField(str.Substring(37, 5).Trim());
                    csv.WriteField(str.Substring(45, 5).Trim());
                    csv.WriteField(str.Substring(51, 5));
                    csv.WriteField(str.Substring(58, 4));

                    var m = _regex.Matches(str.Substring(67));

                    csv.WriteField(m[0].Value);
                    csv.WriteField(m[1].Value);
                    csv.WriteField(m[2].Value);
                    csv.WriteField(m[3].Value);
                    csv.WriteField(m[4].Value);
                    csv.WriteField(m[5].Value);
                    csv.WriteField(m[6].Value);

                    await csv.NextRecordAsync();
                }
            }
        }
    }
}
