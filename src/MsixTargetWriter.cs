namespace MsixShortcut;

public sealed class MsixTargetWriter
{
    private BinaryWriter Writer { get; }

    public MsixTargetWriter(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Writer = new BinaryWriter(stream);
    }

    public void Apps(
        Action<MsixSectionWriter> bodyWriter,
        Action<MsixSectionWriter> trailerWriter)
    {
        if (bodyWriter is null) throw new ArgumentNullException(nameof(bodyWriter));
        if (trailerWriter is null) throw new ArgumentNullException(nameof(trailerWriter));

        long originPos = Writer.BaseStream.Position;

        Writer.Write((ushort)0);        // zeros

        long appsLengthPos = Writer.BaseStream.Position;

        Writer.Write((ushort)0);        // length
        Writer.Write((uint)0x53505041); // magic 'APPS'

        long appsDataLengthPos = Writer.BaseStream.Position;

        Writer.Write((ushort)0);        // data length
        Writer.Write((ushort)0x0008);   // unknown

        long appsDataStartPos = Writer.BaseStream.Position;

        Writer.Write((uint)0x00000003); // unknown

        bodyWriter.Invoke(new MsixSectionWriter(Writer.BaseStream));

        long appsDataEndPos = Writer.BaseStream.Position;

        Writer.BaseStream.Position = appsDataLengthPos;
        ushort appsDataLength = (ushort)(appsDataEndPos - appsDataStartPos);
        Writer.Write(appsDataLength);

        Writer.BaseStream.Position = appsLengthPos;
        Writer.Write((ushort)(appsDataEndPos - originPos));

        Writer.BaseStream.Position = appsDataEndPos;

        AppsTrailer(trailerWriter, appsDataLength);
    }

    private void AppsTrailer(Action<MsixSectionWriter> trailerWriter, ushort appsDataLength)
    {
        Writer.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });  // 10-byte null separator

        long originPos = Writer.BaseStream.Position;

        Writer.Write((uint)0);                      // Trailer length

        trailerWriter.Invoke(new MsixSectionWriter(Writer.BaseStream));

        Writer.Write(new byte[] { 0, 0, 0, 0, 0, 0, 0, 0 });

        Writer.Write((ushort)(appsDataLength + 6)); // This doesn't seem to cleanly align to anything, but
                                                    // it always seems to be the same as the Apps length field + 6 bytes.

        long endPos = Writer.BaseStream.Position;

        Writer.BaseStream.Position = originPos;
        Writer.Write((uint)(endPos - originPos));   // Trailer length

        Writer.BaseStream.Position = endPos;
    }
}
