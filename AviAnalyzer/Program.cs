using AviAnalyzer.Models;
using Microsoft.Extensions.Configuration;
using System.Buffers;
using System.Text;


var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();


var list = await List.ParseAsync(configuration["Target"]);

var txChunks = ((List)list.Chunks[2]).Chunks.Where(t => t.FourCC == "03tx");
foreach (var tx in txChunks)
{
    var buffer = ArrayPool<byte>.Shared.Rent(tx.DataLength);

    var count = await tx.ReadDataAsync(buffer, 0);
    var str = Encoding.UTF8.GetString(buffer, 0, count);






    Console.WriteLine(　$"{buffer.GetHashCode():X} {str}");

    ArrayPool<byte>.Shared.Return(buffer);
}
