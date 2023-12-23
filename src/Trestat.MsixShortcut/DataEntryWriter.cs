using System.Text;

namespace Trestat.MsixShortcut;

/// <summary>
/// Writes a key-value pair.
/// </summary>
public sealed class DataEntryWriter
{
    private BinaryWriter Writer { get; }

    public DataEntryWriter(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Writer = new BinaryWriter(stream);
    }

    /// <summary>
    /// Writes a 32-bit value and advances the position in the stream.
    /// </summary>
    /// <param name="key">Some key that is appropriate for the current section. See <see cref="KnownSectionGuids"/>.</param>
    /// <param name="value"></param>
    public void U32(uint key, uint value)
    {
        Writer.Write(DataEntryConstants.DATAENTRY_HEADER_LENGTH + (uint)sizeof(uint));
        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.U32);
        Writer.Write(value);
    }

    /// <summary>
    /// Writes a 64-bit value and advances the position in the stream.
    /// </summary>
    /// <param name="key">Some key that is appropriate for the current section. See <see cref="KnownSectionGuids"/>.</param>
    /// <param name="value"></param>
    public void U64(uint key, ulong value)
    {
        Writer.Write(DataEntryConstants.DATAENTRY_HEADER_LENGTH + (uint)sizeof(ulong));
        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.U64);
        Writer.Write(value);
    }

    /// <summary>
    /// Writes a string value and advances the position in the stream.
    /// </summary>
    /// <param name="key">Some key that is appropriate for the current section. See <see cref="KnownSectionGuids"/>.</param>
    /// <param name="value"></param>
    /// <param name="extra">An optional 16-bit value that follows the end of the string.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Text(uint key, string value, ushort? extra = default)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

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

    /// <summary>
    /// Writes a GUID value and advances the position in the stream.
    /// </summary>
    /// <param name="key">Some key that is appropriate for the current section. See <see cref="KnownSectionGuids"/>.</param>
    /// <param name="value"></param>
    public void Guid(uint key, Guid value)
    {
        Writer.Write(DataEntryConstants.DATAENTRY_HEADER_LENGTH + (uint)16);
        Writer.Write(key);
        Writer.Write((byte)0);
        Writer.Write((uint)DataEntryKind.Guid);
        Writer.Write(value.ToByteArray());
    }
}
