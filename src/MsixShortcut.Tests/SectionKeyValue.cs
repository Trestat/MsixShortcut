namespace MsixShortcut.Tests;

public sealed record SectionKeyValue(Guid Section, uint Key, DataEntryKind Kind, IDataEntryValue Value);
