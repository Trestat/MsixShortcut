namespace MsixShortcut;

/// <summary>
/// Writes a simple shell link.
/// </summary>
public sealed class ShellLinkWriter
{
    private BinaryWriter Writer { get; }

    public ShellLinkWriter(Stream stream)
    {
        if (stream is null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        Writer = new BinaryWriter(stream);
    }

    /// <summary>
    /// Writes the shell link header block to the stream.
    /// </summary>
    public void Header()
    {
        byte[] header = [
            0x4C, 0x00, 0x00, 0x00, 0x01, 0x14, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0xC0, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x46, 0x81, 0x00, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        ];

        Writer.Write(header);
    }

    /// <summary>
    /// Writes the ItemIDList to the shell link.
    /// </summary>
    /// <param name="innerWriter"></param>
    public void LinkTargetItemId(Action<ItemIdWriter> innerWriter)
    {
        long linkTargetIdListLengthPos = Writer.BaseStream.Position;

        Writer.Write((ushort)0);    // size of LinkTargetIdList

        innerWriter.Invoke(new ItemIdWriter(Writer.BaseStream));

        long endPos = Writer.BaseStream.Position;

        Writer.BaseStream.Position = linkTargetIdListLengthPos;
        Writer.Write((ushort)(endPos - linkTargetIdListLengthPos));

        Writer.BaseStream.Position = endPos;
    }

    /// <summary>
    /// Writes the shell link footer block to the stream.
    /// </summary>
    public void Footer()
    {
        Writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });
    }
}
