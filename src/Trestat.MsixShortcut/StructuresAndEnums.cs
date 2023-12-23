namespace Trestat.MsixShortcut;

public static class KnownSectionGuids
{
    /// <summary>
    /// A section containing package details. See <see cref="PackageKey"/>.
    /// </summary>
    public static readonly Guid Package = new("{9F4C2855-9F79-4B39-A8D0-E1D42DE1D5F3}");

    /// <summary>
    /// A section containing visual assets. See <see cref="AssetKey"/>.
    /// </summary>
    public static readonly Guid Assets = new("{86D40B4D-9069-443C-819A-2A54090DCCEC}");

    /// <summary>
    /// A section containing additional display name information. See <see cref="TaskbarKey"/>.
    /// </summary>
    public static readonly Guid Taskbar = new("{B725F130-47EF-101A-A5F1-02608C9EEBAC}");

    /// <summary>
    /// A section containing information about the ExperienceHost.
    /// See <see cref="ExperienceHostKey"/>.
    /// This section may be present on shell links created by Windows when the user drags and drops a packaged app icon from the Start menu to the desktop.
    /// It is only present in the Trailer of <see cref="MsixTargetWriter.Apps(Action{MsixSectionWriter}, Action{MsixSectionWriter})"/>.
    /// </summary>
    public static readonly Guid ExperienceHost = new("{FFAE9DB7-1C8D-43FF-818C-84403AA3732D}");

    /// <summary>
    /// A section containing some unknown information. See <see cref="Unknown1Key"/>.
    /// </summary>
    public static readonly Guid Unknown1 = new("{446D16B1-8DAD-4870-A748-402EA43D788C}");

    /// <summary>
    /// A section containing some unknown information. See <see cref="Unknown2Key"/>.
    /// </summary>
    public static readonly Guid Unknown2 = new("{0DED77B3-C614-456C-AE5B-285B38D7B01B}");
}

/// <summary>
/// Keys for use with <see cref="KnownSectionGuids.Package"/>.
/// </summary>
public enum PackageKey : uint
{
    /// <summary>
    /// A string with the package name and app target, like <c>43891JeniusApps.Ambie_jaj7tphbgjeh8!App</c>.
    /// </summary>
    PackageFamilyNameAppTarget = 0x05,

    /// <summary>
    /// Determines how the package needs to be activated when the user double-clicks the shell link. Corresponds to <see cref="MsixShortcut.ActivationBehavior"/>.
    /// </summary>
    ActivationBehavior = 0x0E,

    /// <summary>
    /// A string with the full file system path to the installed package at the time the shortcut was created, like <c>C:\Program Files\WindowsApps\43891JeniusApps.Ambie_3.9.26.0_x64__jaj7tphbgjeh8</c>.
    /// </summary>
    PackageInstallPath = 0x0F,

    /// <summary>
    /// A string describing the package family name, like <c>43891JeniusApps.Ambie_jaj7tphbgjeh8</c>.
    /// </summary>
    PackageFamilyName = 0x11,

    /// <summary>
    /// A string describing the full name of the package including version and processor architecture at the time the shortcut was created, like <c>43891JeniusApps.Ambie_3.9.26.0_x64__jaj7tphbgjeh8</c>.
    /// </summary>
    PackageInstallFolderName = 0x15,

    /// <summary>
    /// Unknown. Always appears to be 1 in all files inspected.
    /// </summary>
    Unknown19 = 0x19,
    
    /// <summary>
    /// Unknown. Some GUID value. This may be used to uniquely identify the shell link.
    /// </summary>
    Unknown20 = 0x20,

    /// <summary>
    /// Unknown. Always appears to be 0x0000FFFF. Not present in Win10 build 19045 or earlier.
    /// </summary>
    Unknown27 = 0x27 // (discovered on Windows build 22621)
}

/// <summary>
/// Determines how the package needs to be activated when the user double-clicks the shell link. See <see cref="PackageKey.ActivationBehavior"/>.
/// </summary>
public enum ActivationBehavior : uint
{
    /// <summary>
    /// The app has a UWP lifetime.
    /// </summary>
    UWP = 1,

    /// <summary>
    /// The app has a classic Win32 lifetime.
    /// </summary>
    Win32 = 2
}

/// <summary>
/// Keys for use with <see cref="KnownSectionGuids.Assets"/>.
/// </summary>
public enum AssetKey : uint
{
    /// <summary>
    /// Relative path to the Square44x44 time assets as it appears in the AppxManifest, like <c>Assets\Square44x44Logo.png</c>.
    /// <para>
    /// Corresponds to <c>uap:VisualElements.Square44x44Logo</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-visualelements"/>.
    /// </para>
    /// </summary>
    Square44x44Logo = 0x02,

    /// <summary>
    /// An ABGR color value representing the tile color as it appears in the AppxManifest.
    /// For links created by the shell, if the manifest file does not indicate a background color, this value will be set to the current user's accent color from their appearance settings.
    /// <para>
    /// Corresponds to <c>uap:VisualElements.BackgroundColor</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-visualelements"/>.
    /// </para>
    /// </summary>
    BackgroundColor = 0x04,

    /// <summary>
    /// Unknown. Always appears to be 0xFFFFFFFF in all files inspected.
    /// </summary>
    Unknown05 = 0x05,

    /// <summary>
    /// The display name of the package as it appears in the AppxManifest.
    /// <para>
    /// Corresponds to <c>uap:VisualElements.DisplayName</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-visualelements"/>.
    /// </para>
    /// </summary>
    DisplayName = 0x0B,

    /// <summary>
    /// Relative path to the Square150x150 tile assets as it appears in the AppxManifest, like <c>Assets\Square150x150Logo.png</c>.
    /// <para>
    /// Corresponds to <c>uap:VisualElements.Square150x150Logo</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-visualelements"/>.
    /// </para>
    /// </summary>
    Square150x150Logo = 0x0C,

    /// <summary>
    /// Relative path to the Wide310x150 tile assets as it appears in the AppxManifest, like <c>Assets\Wide310x150Logo.png</c>.
    /// <para>
    /// Corresponds to <c>uap:DefaultTile.Wide310x150Logo</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-defaulttile"/>.
    /// </para>
    /// </summary>
    Wide310x150Logo = 0x0D,

    /// <summary>
    /// Relative path to the BadgeLogo assets as it appears in the AppxManifest, like <c>Assets\BadgeLogo.png</c>.
    /// <para>
    /// Corresponds to <c>uap:LockScreen.BadgeLogo</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-lockscreen"/>.
    /// </para>
    /// </summary>
    BadgeLogo = 0x0F,

    /// <summary>
    /// Relative path to the Square310x310 tile assets as it appears in the AppxManifest, like <c>Assets\Square310x310Logo.png</c>.
    /// <para>
    /// Corresponds to <c>uap:DefaultTile.Square310x310</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-defaulttile"/>.
    /// </para>
    /// </summary>
    Square310x310Logo = 0x13,

    /// <summary>
    /// Relative path to the Square71x71 tile assets as it appears in the AppxManifest, like <c>Assets\Square71x71Logo.png</c>.
    /// <para>
    /// Corresponds to <c>uap:DefaultTile.Square71x71Logo</c>; see <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-uap-defaulttile"/>.
    /// </para>
    /// </summary>
    Square71x71Logo = 0x14,
}

/// <summary>
/// Keys for use with section <see cref="KnownSectionGuids.Unknown1"/>.
/// </summary>
public enum Unknown1Key : uint
{
    Unknown13 = 0x13,
    Unknown64 = 0x64
}

/// <summary>
/// Keys for use with section <see cref="KnownSectionGuids.Taskbar"/>.
/// </summary>
public enum TaskbarKey : uint
{
    /// <summary>
    /// The app name used on the taskbar pin. This value is applied when the user right-clicks the shell link and selects "Pin to Taskbar". This value should be identical to <see cref="AssetKey.DisplayName"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The name is shown on the taskbar pin in two places: when the application is closed and the user hovers the mouse over the pin to reveal a tooltip (the "Tooltip Name"), and when the user right-clicks the pin to reveal
    /// the jump list and the name is shown at the top of the list (the "Jump List Name").
    /// </para>
    /// <para>
    /// The absence of this key or setting it to a value that is not <see cref="AssetKey.DisplayName"/> can have unusual behavior as tested on Win10 build 19045 and Win11 build 22621.
    /// </para>
    /// <para>
    /// When the key is absent, the Jump List Name will be the same as defined on the DisplayName property on the
    /// <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-application">Application</a> element of the AppxManifest.
    /// However, the Tooltip Name will be undefined and usually uses the same tooltip text as another pinned app on the taskbar when the user hovers the mouse over it. For example, hovering the mouse over the Windows Terminal
    /// pin and then back over the pin in question will reveal a Tooltip Name of "Windows Terminal".
    /// </para>
    /// <para>
    /// When the key is present but is a random string, the behavior is the same as if it were absent.
    /// </para>
    /// <para>
    /// When the key is present and set to the name of another packaged app, both the Jump List Name and the Tooltip Name will use the name of the other app. For example, if you were to create a shortcut to
    /// <a href="https://apps.microsoft.com/detail/9P07XNM5CHP0">Ambie</a> and you also had <a href="https://apps.microsoft.com/detail/9PF4KZ2VN4W9">TranslucentTB</a> installed, the pin would adopt the name "TranslucentTB".
    /// </para>
    /// </remarks>
    DisplayName = 0x0A
}

/// <summary>
/// Keys for use with section <see cref="KnownSectionGuids.Unknown2"/>.
/// </summary>
public enum Unknown2Key : uint
{
    /// <summary>
    /// Unknown. Always appears to be 0x00000000.
    /// </summary>
    Unknown07 = 0x07
}

/// <summary>
/// Keys for use with section <see cref="KnownSectionGuids.ExperienceHost"/>.
/// </summary>
public enum ExperienceHostKey : uint
{
    Unknown64 = 0x64
}

public enum DataEntryKind : uint
{
    /// <summary>
    /// Unknown. Appears to be a 32-bit wide value that is at most 16-bits wide. Not present on Win10 build 19045 or earlier.
    /// </summary>
    Unknown0B_32 = 0x0B, // Appears to be 16-bit unsigned value, 32-bits wide (discovered on Windows build 22621)

    /// <summary>
    /// A 32-bit unsigned number.
    /// </summary>
    U32 = 0x13,

    /// <summary>
    /// A 64-bit unsigned number.
    /// </summary>
    U64 = 0x15,

    /// <summary>
    /// A null-terminated string.
    /// </summary>
    Text = 0x1F,

    /// <summary>
    /// A GUID.
    /// </summary>
    Guid = 0x48
}

/// <summary>Describes a <see cref="DataEntry"/>.</summary>
/// <param name="Length">The length of the header and the payload, minus 4.</param>
/// <param name="Key">Some key, as in a key=value relationship. The meaning of the key varies depending on the section in which this entry is contained.</param>
/// <param name="Unknown1"></param>
/// <param name="Kind">The data type, which refers to some type of <see cref="IDataEntryValue"/>.</param>
public sealed record DataEntryHeader(
    uint Length,
    uint Key,
    byte Unknown1,
    DataEntryKind Kind)
{
    private SectionHeader? _section;

    public void SetSectionForDisplay(SectionHeader section) => _section = section;

    public string? KnownKeyName
    {
        get
        {
            if (_section is null) return null;
            if (_section.Guid == KnownSectionGuids.Package) return Enum.GetName(typeof(PackageKey), Key);
            else if (_section.Guid == KnownSectionGuids.Assets) return Enum.GetName(typeof(AssetKey), Key);
            else if (_section.Guid == KnownSectionGuids.Unknown1) return Enum.GetName(typeof(Unknown1Key), Key);
            else if (_section.Guid == KnownSectionGuids.Taskbar) return Enum.GetName(typeof(TaskbarKey), Key);
            else if (_section.Guid == KnownSectionGuids.Unknown2) return Enum.GetName(typeof(Unknown2Key), Key);
            else if (_section.Guid == KnownSectionGuids.ExperienceHost) return Enum.GetName(typeof(ExperienceHostKey), Key);
            else return string.Empty;
        }
    }
}

public interface IDataEntryValue;

public sealed record Unknown0B_32DataEntryValue(uint Value) : IDataEntryValue;

public sealed record U32DataEntryValue(uint Value) : IDataEntryValue;

public sealed record U64DataEntryValue(ulong Value) : IDataEntryValue;

public sealed record TextDataEntryValue(uint CharCount, byte[] Data) : IDataEntryValue
{
    public TextDataEntryValue(string s)
        : this(
            CharCount: (uint)s.Length + 1,
            Data: System.Text.Encoding.Unicode.GetBytes(s + '\0'))
    { }

    public string StringValue =>
        System.Text.Encoding.Unicode.GetString(
            bytes: Data,
            index: 0,
            count: CharCount == 0 ? 0 : ((int)CharCount - 1) * 2); // Data has a null terminator char at the end; ignore it

    public bool Equals(TextDataEntryValue? other) =>
        other is not null
        && CharCount == other.CharCount
        && Data.SequenceEqual(other.Data);

    public override int GetHashCode() => CharCount.GetHashCode();
}

public sealed record GuidDataEntryValue(byte[] Data) : IDataEntryValue
{
    public Guid GuidValue => new(Data);

    public override string ToString() => GuidValue.ToString();
}

public sealed record DataEntry(DataEntryHeader Header, IDataEntryValue Value, ushort? Extra);

/// <summary>Describes a <see cref="Section"/>.</summary>
/// <param name="Leader">The start of the section. Zero in the main payload, but is 0xBEEF0027 on the Trailer.</param>
/// <param name="Length">Total number of bytes in the section, including this header.</param>
/// <param name="MagicSps1">Should be a 4-byte ASCII string representing "SPS1".</param>
/// <param name="Guid">A GUID representing the section type.</param>
public sealed record SectionHeader(
    uint Leader,
    uint Length,
    byte[] MagicSps1,
    Guid Guid)
{
    public string KnownGuidName
    {
        get
        {
            if (Guid == KnownSectionGuids.Package) return nameof(KnownSectionGuids.Package);
            else if (Guid == KnownSectionGuids.Assets) return nameof(KnownSectionGuids.Assets);
            else if (Guid == KnownSectionGuids.Unknown1) return nameof(KnownSectionGuids.Unknown1);
            else if (Guid == KnownSectionGuids.Taskbar) return nameof(KnownSectionGuids.Taskbar);
            else if (Guid == KnownSectionGuids.Unknown2) return nameof(KnownSectionGuids.Unknown2);
            else if (Guid == KnownSectionGuids.ExperienceHost) return nameof(KnownSectionGuids.ExperienceHost);
            else return string.Empty;
        }
    }
}

/// <summary>
/// Contains an array of key-value pairs.
/// </summary>
/// <param name="Header">The header describing this section.</param>
/// <param name="Entries">The collection of key-value pairs.</param>
public sealed record Section(SectionHeader Header, DataEntry[] Entries);

/// <summary>Describes a <see cref="MainBody"/>.</summary>
/// <param name="Leader">Seems to always be 0x0000.</param>
/// <param name="Length">Total number of bytes in the header and the main payload, minus 4; or, the number of bytes immediately following the Length field through the end of the main payload.</param>
/// <param name="MagicApps">Should be a 4-byte ASCII string representing "APPS".</param>
/// <param name="LengthB">Number of bytes in main payload, plus 4.</param>
/// <param name="Unknown1">Seems to always be 0x0008 in all files inspected.</param>
/// <param name="Unknown2">Seems to always be 0x00000003 in all files inspected.</param>
public sealed record MainHeader(
    ushort Leader,
    ushort Length,
    byte[] MagicApps,
    ushort LengthB,
    ushort Unknown1,
    uint Unknown2);

/// <summary>
/// Contains an array of sections.
/// </summary>
/// <param name="Header">The header describing this body.</param>
/// <param name="Sections">The collection of sections.</param>
public sealed record MainBody(
    MainHeader Header,
    Section[] Sections);

/// <summary>A trailer containing some additional metadata.</summary>
/// <param name="Length">Total length of the trailer.</param>
/// <param name="Section">The single section. In reverse-engineered shell links on Win10 19045 and Win11 22621, this is a section of type <see cref="KnownSectionGuids.ExperienceHost"/>.</param>
/// <param name="LengthB">Total length from the start of the Header to the end of the main payload, plus +2.</param>
public sealed record Trailer(
    uint Length,
    Section Section,
    ushort LengthB);

/// <summary>
/// Represents the entirety of the ItemID that describes the packaged application target.
/// </summary>
/// <param name="MainBody"></param>
/// <param name="Trailer"></param>
public sealed record MsixTargetItemId(
    MainBody MainBody,
    Trailer Trailer);

/// <summary>
/// An assortment of constants used when reading and writing ItemID data describing the shell link target.
/// </summary>
public static class DataEntryConstants
{
    public const int GUID_LENGTH = 16;
    public const int SECTION_HEADER_LENGTH = 4 + 4 + 4 + GUID_LENGTH;
    public const int DATAENTRY_HEADER_LENGTH = 4 + 4 + 1 + 4;
}
