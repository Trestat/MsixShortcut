namespace Trestat.MsixShortcut.Tests;

public sealed class TestMsixTargetWriter
{
    [Test]
    public void MsixTargetWriter_MsixTargetReader_ShouldRoundTrip()
    {
        var ms = new MemoryStream();

        new MsixTargetWriter(ms)
            .Apps(bw =>
            {
                bw.Section(
                    leader: 0x0,
                    KnownSectionGuids.Package,
                    dew =>
                    {
                        dew.U32((uint)PackageKey.ActivationBehavior, (uint)ActivationBehavior.UWP);
                        dew.Text((uint)PackageKey.PackageFamilyName, "43891JeniusApps.Ambie_jaj7tphbgjeh8");
                        dew.Text((uint)PackageKey.PackageFamilyNameAppTarget, "43891JeniusApps.Ambie_jaj7tphbgjeh8!App");
                    });
                bw.Section(
                    leader: 0x0,
                    KnownSectionGuids.Assets,
                    dew =>
                    {
                        dew.Text((uint)AssetKey.DisplayName, "Ambie");
                    });
                bw.Section(
                    leader: 0x0,
                    KnownSectionGuids.Taskbar,
                    dew =>
                    {
                        dew.Text((uint)TaskbarKey.DisplayName, "Ambie", extra: 0);
                    });
            },
            tw =>
            {
                // no trailer
            });

        var reader = new MsixTargetItemIdReader(ms.ToArray());

        var mainBody = reader.ReadMainBody();

        Assert.Multiple(() =>
        {
            Assert.That(mainBody.Flatten(), Is.EquivalentTo(new SectionKeyValue[]
            {
                new(KnownSectionGuids.Package, (uint)PackageKey.PackageFamilyName, DataEntryKind.Text, new TextDataEntryValue(@"43891JeniusApps.Ambie_jaj7tphbgjeh8"), null),
                new(KnownSectionGuids.Package, (uint)PackageKey.PackageFamilyNameAppTarget, DataEntryKind.Text, new TextDataEntryValue(@"43891JeniusApps.Ambie_jaj7tphbgjeh8!App"), null),
                new(KnownSectionGuids.Package, (uint)PackageKey.ActivationBehavior, DataEntryKind.U32, new U32DataEntryValue((uint)ActivationBehavior.UWP), null),
                new(KnownSectionGuids.Assets, (uint)AssetKey.DisplayName, DataEntryKind.Text, new TextDataEntryValue(@"Ambie"), null),
                new(KnownSectionGuids.Taskbar, (uint)TaskbarKey.DisplayName, DataEntryKind.Text, new TextDataEntryValue(@"Ambie"), 0),
            }));
        });
    }
}
