namespace MsixShortcut;

public sealed class MsixSectionWriter
{
    private BinaryWriter Writer { get; }

    public MsixSectionWriter(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Writer = new BinaryWriter(stream);
    }

    public void Section(uint leader, Guid guid, Action<DataEntryWriter> innerWriter)
    {
        if (innerWriter is null) throw new ArgumentNullException(nameof(innerWriter));

        long sectionStartPos = Writer.BaseStream.Position;

        Writer.Write(leader);               // leader

        long sectionLengthPos = Writer.BaseStream.Position;

        Writer.Write((uint)0);              // section length
        Writer.Write((uint)0x53505331);     // magic "SPS1"
        Writer.Write(guid.ToByteArray());   // guid

        innerWriter.Invoke(new DataEntryWriter(Writer.BaseStream));
        long dataEndPos = Writer.BaseStream.Position;

        Writer.BaseStream.Position = sectionLengthPos;
        Writer.Write((uint)(dataEndPos - sectionStartPos));

        Writer.BaseStream.Position = dataEndPos;
    }
}
