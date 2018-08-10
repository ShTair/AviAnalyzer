using AviAnalyzer.Models;
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
            var list = await List.ParseAsync(target);
        }
    }
}
