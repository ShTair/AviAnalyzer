using AviAnalyzer.Models;
using System.IO;
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
            }
        }
    }
}
