namespace AviAnalyzer.Models;

class Chunk(Stream stream, int offset, int length, string fourCC)
{
    protected Stream _stream = stream;

    protected int _offset = offset;

    protected int _length = length;

    public string FourCC { get; } = fourCC;

    public virtual int DataLength => _length;

    public async Task<int> ReadDataAsync(byte[] buffer, int offset)
    {
        _stream.Seek(_offset + 8, SeekOrigin.Begin);
        return await _stream.ReadAsync(buffer.AsMemory(offset, DataLength));
    }

    public override string ToString()
    {
        return $"{FourCC}: {_offset:X8} -> {_offset + _length + 8:X8}({_length + 8:X8}): {_offset + 8:X8}({_length:X8})";
    }
}
