namespace MsixShortcut;

public sealed class LinkIdObjectReader
{
    private byte[] Data { get; }
    private BinaryReader Reader { get; }

    public LinkIdObjectReader(byte[] data)
    {
        Data = data ?? throw new ArgumentNullException(nameof(data));
        Reader = new BinaryReader(new MemoryStream(Data));
    }

    public DataEntryHeader ReadDataEntryHeader()
    {
        return new DataEntryHeader(
            Length: Reader.ReadUInt32(),
            Key: Reader.ReadUInt32(),
            Unknown1: Reader.ReadByte(),
            Kind: (DataEntryKind)Reader.ReadUInt32());
    }

    public Unknown0B_32DataEntryValue ReadUnknown0B_32DataEntryValue() => new(Value: Reader.ReadUInt32());

    public U32DataEntryValue ReadU32DataEntryValue() => new(Value: Reader.ReadUInt32());

    public U64DataEntryValue ReadU64DataEntryValue() => new(Value: Reader.ReadUInt64());

    public TextDataEntryValue ReadTextDataEntryValue()
    {
        uint length = Reader.ReadUInt32();

        return new TextDataEntryValue(
            CharCount: length,
            Data: Reader.ReadBytes((int)length * 2));
    }

    public GuidDataEntryValue ReadGuidDataEntryValue() => new(Data: Reader.ReadBytes(16));

    public DataEntry ReadDataEntry()
    {
        long startPos = Reader.BaseStream.Position;

        var header = ReadDataEntryHeader();

        IDataEntryValue value = header.Kind switch
        {
            DataEntryKind.Unknown0B_32 => ReadUnknown0B_32DataEntryValue(),
            DataEntryKind.U32 => ReadU32DataEntryValue(),
            DataEntryKind.U64 => ReadU64DataEntryValue(),
            DataEntryKind.Text => ReadTextDataEntryValue(),
            DataEntryKind.Guid => ReadGuidDataEntryValue(),
            _ => throw new InvalidOperationException($"Unknown DataEntryKind '{header.Kind}'.")
        };

        var entry = new DataEntry(
            Header: header,
            Value: value);

        Reader.BaseStream.Position = startPos + header.Length;

        return entry;
    }

    public SectionHeader ReadSectionHeader()
    {
        return new SectionHeader(
            Leader: Reader.ReadUInt32(),
            Length: Reader.ReadUInt32(),
            MagicSps1: Reader.ReadBytes(4),
            Guid: new Guid(Reader.ReadBytes(16)));
    }

    public Section ReadSection()
    {
        long startPos = Reader.BaseStream.Position;

        var header = ReadSectionHeader();

        IEnumerable<DataEntry> enumerateDataEntries()
        {
            for (int dataLength = (int)header.Length - DataEntryConstants.SECTION_HEADER_LENGTH;
                dataLength > DataEntryConstants.DATAENTRY_HEADER_LENGTH;)
            {
                var dataEntry = ReadDataEntry();
                dataEntry.Header.SetSectionForDisplay(header);
                dataLength -= (int)dataEntry.Header.Length;
                yield return dataEntry;
            }
        }

        var section = new Section(
            Header: header,
            Entries: enumerateDataEntries().ToArray());

        Reader.BaseStream.Position = startPos + header.Length;

        return section;
    }

    public MainHeader ReadMainBodyHeader()
    {
        return new MainHeader(
            Leader: Reader.ReadUInt16(),
            Length: Reader.ReadUInt16(),
            MagicApps: Reader.ReadBytes(4),
            LengthB: Reader.ReadUInt16(),
            Unknown1: Reader.ReadUInt16(),
            Unknown2: Reader.ReadUInt32());
    }

    public MainBody ReadMainBody()
    {
        long startPos = Reader.BaseStream.Position;

        var header = ReadMainBodyHeader();

        IEnumerable<Section> enumerateSections()
        {
            for (int dataLength = header.LengthB - 4;
                dataLength > DataEntryConstants.SECTION_HEADER_LENGTH;)
            {
                var section = ReadSection();
                dataLength -= (int)section.Header.Length;
                yield return section;
            }
        }

        var main = new MainBody(
            Header: header,
            Sections: enumerateSections().ToArray());

        Reader.BaseStream.Position = startPos += header.Length + 4;

        return main;
    }
}
