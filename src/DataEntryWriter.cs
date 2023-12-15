using System.Text;

namespace MsixShortcut;

public sealed class DataEntryWriter
{
    private BinaryWriter Writer { get; }

    public DataEntryWriter(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Writer = new BinaryWriter(stream);
    }

    public void U32(uint key, uint value)
    {
        Writer.Write(DataEntryConstants.DATAENTRY_HEADER_LENGTH + (uint)sizeof(uint));
        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.U32);
        Writer.Write(value);
    }

    public void U64(uint key, ulong value)
    {
        Writer.Write(DataEntryConstants.DATAENTRY_HEADER_LENGTH + (uint)sizeof(ulong));
        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.U64);
        Writer.Write(value);
    }

    public void Text(uint key, string value, ushort? extra = default)
    {
        byte[] stringAsUtf16PlusNull = Encoding.Unicode.GetBytes(value + '\0');

        Writer.Write(
            (uint)
            (DataEntryConstants.DATAENTRY_HEADER_LENGTH
            + sizeof(uint)
            + (uint)stringAsUtf16PlusNull.Length
            + (extra.HasValue ? sizeof(ushort) : 0)));

        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.Text);
        Writer.Write((uint)(stringAsUtf16PlusNull.Length / 2));
        Writer.Write(stringAsUtf16PlusNull);

        if (extra.HasValue) Writer.Write(extra.Value);
    }

    public void Guid(uint key, Guid value)
    {
        Writer.Write(DataEntryConstants.DATAENTRY_HEADER_LENGTH + (uint)16);
        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.Guid);
        Writer.Write(value.ToByteArray());
    }
}
