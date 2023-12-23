namespace Trestat.MsixShortcut;

/// <summary>
/// Writes a section containing some key-value entries.
/// </summary>
public sealed class MsixSectionWriter
{
    private BinaryWriter Writer { get; }

    public MsixSectionWriter(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Writer = new BinaryWriter(stream);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="leader">
    /// A 32-bit leader value. For sections in the main body, all reverse-engineered shell links have values of 0x00000000.
    /// A <see cref="KnownSectionGuids.ExperienceHost"/> section in the trailer has a leader value of 0xBEEF0027 on Win10 19045 and Win11 22621.
    /// </param>
    /// <param name="guid">A GUID that identifies the section. May be one of <see cref="KnownSectionGuids"/>.</param>
    /// <param name="innerWriter">An inner writer used to write the key-value pairs.</param>
    /// <exception cref="ArgumentNullException"></exception>
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
