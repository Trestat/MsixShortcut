namespace MsixShortcut.Tests;

public sealed class TestCreateShortcut
{
    [Test]
    public void CreateShortcut_WithPackageShortcutOptions_ShouldWriteToStream()
    {
        var ms = new MemoryStream();

        var options = new PackageShortcutOptions(
            PackageInstallationPath: @"c:\path\to\installation",
            PackageFamilyName: "43891JeniusApps.Ambie_jaj7tphbgjeh8",
            DisplayName: "Ambie",
            ActivationBehavior: ActivationBehavior.UWP,
            AppIdentifier: "App",
            Square44x44LogoPath: @"Assets\Square44x44Logo.png",
            Square71x71LogoPath: @"Assets\SmallTile.png",
            Square150x150LogoPath: @"Assets\Square150x150Logo.png",
            Square310x310LogoPath: @"Assets\LargeTile.png",
            Wide310x150LogoPath: @"Assets\Wide310x150Logo.png");

        ShortcutHelpers.CreateShortcut(options, ms);

        var shortcut = ShellLink.Shortcut.FromByteArray(ms.ToArray());

        var itemApplications = shortcut.LinkTargetIDList.ItemIDList[0];
        var itemMsixTarget = shortcut.LinkTargetIDList.ItemIDList[1];

        var reader = new MsixTargetItemIdReader(itemMsixTarget.Data);

        var mainBody = reader.ReadMainBody();

        Assert.Multiple(() =>
        {
            Assert.That(itemApplications.Data, Is.EqualTo(ItemIdWriter.ApplicationsItemId));

            Assert.That(mainBody.Flatten(), Is.EquivalentTo(new SectionKeyValue[]
            {
                new(KnownSectionGuids.Package, (uint)PackageKey.PackageInstallPath, DataEntryKind.Text, new TextDataEntryValue(@"c:\path\to\installation"), 0),
                new(KnownSectionGuids.Package, (uint)PackageKey.PackageFamilyNameAppTarget, DataEntryKind.Text, new TextDataEntryValue(@"43891JeniusApps.Ambie_jaj7tphbgjeh8!App"), null),
                new(KnownSectionGuids.Package, (uint)PackageKey.ActivationBehavior, DataEntryKind.U32, new U32DataEntryValue((uint)ActivationBehavior.UWP), null),
                new(KnownSectionGuids.Assets, (uint)AssetKey.Square44x44Logo, DataEntryKind.Text, new TextDataEntryValue(@"Assets\Square44x44Logo.png"), 0),
                new(KnownSectionGuids.Assets, (uint)AssetKey.Square71x71Logo, DataEntryKind.Text, new TextDataEntryValue(@"Assets\SmallTile.png"), 0),
                new(KnownSectionGuids.Assets, (uint)AssetKey.Square150x150Logo, DataEntryKind.Text, new TextDataEntryValue(@"Assets\Square150x150Logo.png"), 0),
                new(KnownSectionGuids.Assets, (uint)AssetKey.Square310x310Logo, DataEntryKind.Text, new TextDataEntryValue(@"Assets\LargeTile.png"), 0),
                new(KnownSectionGuids.Assets, (uint)AssetKey.Wide310x150Logo, DataEntryKind.Text, new TextDataEntryValue(@"Assets\Wide310x150Logo.png"), 0),
                new(KnownSectionGuids.Taskbar, (uint)TaskbarKey.DisplayName, DataEntryKind.Text, new TextDataEntryValue(@"Ambie"), null),
            }));
        });
    }
}
