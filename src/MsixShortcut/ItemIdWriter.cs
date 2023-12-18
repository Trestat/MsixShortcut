namespace MsixShortcut;

/// <summary>
/// Writes a shell link ItemID.
/// </summary>
public sealed class ItemIdWriter
{
    /// <summary>
    /// A byte array representing the ItemID for "Applications".
    /// </summary>
    public static IReadOnlyCollection<byte> ApplicationsItemId => _applicationsItemId;
    private static readonly byte[] _applicationsItemId = [
        0x1F, 0x80, 0x9B, 0xD4, 0x34, 0x42, 0x45, 0x02, 0xF3, 0x4D, 0xB7, 0x80, 0x38, 0x93, 0x94, 0x34, 0x56, 0xE1
    ];

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
    public void Item(IReadOnlyCollection<byte> data)
    {
        if (data is null) throw new ArgumentNullException(nameof(data));

        if (data.Count > ushort.MaxValue - 2) throw new ArgumentException("Data is too big.");

        Writer.Write((ushort)(data.Count + 2));

        foreach (byte b in data)
        {
            Writer.Write(b);
        }
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
