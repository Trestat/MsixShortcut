namespace MsixShortcut;

/// <summary>
/// Writes a shell link ItemID.
/// </summary>
public sealed class ItemIdWriter
{
    private BinaryWriter Writer { get; }

    public ItemIdWriter(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Writer = new BinaryWriter(stream);
    }

    /// <summary>
    /// Writes an ItemID with the given data.
    /// </summary>
    /// <param name="data"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public void Item(byte[] data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));

        if (data.Length > ushort.MaxValue - 2) throw new ArgumentException("Data is too big.");

        Writer.Write((ushort)(data.Length + 2));
        Writer.Write(data);
    }

    /// <summary>
    /// Writes an ItemID that describes a target to a packaged application.
    /// </summary>
    /// <param name="innerWriter"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public void Item(Action<MsixTargetWriter> innerWriter)
    {
        if (innerWriter is null) throw new ArgumentNullException(nameof(innerWriter));

        long lengthPos = Writer.BaseStream.Position;

        Writer.Write((ushort)0);    // size of LinkId entry
        innerWriter.Invoke(new MsixTargetWriter(Writer.BaseStream));

        long endPos = Writer.BaseStream.Position;

        Writer.BaseStream.Position = lengthPos;
        Writer.Write((ushort)(endPos - lengthPos));

        Writer.BaseStream.Position = endPos;
    }
}
