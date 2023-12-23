namespace Trestat.MsixShortcut.Tests;

public static class TestExtensions
{
    public static IEnumerable<SectionKeyValue> Flatten(this MainBody mainBody) =>
        mainBody.Sections
            .SelectMany(s => s.Entries, (s, e) => (s, e))
            .Select(pair => new SectionKeyValue(
                Section: pair.s.Header.Guid,
                Key: pair.e.Header.Key,
                Kind: pair.e.Header.Kind,
                Value: pair.e.Value,
                Extra: pair.e.Extra));
}
