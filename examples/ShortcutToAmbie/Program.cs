/// Creates a launchable desktop shortcut to Ambie (https://apps.microsoft.com/detail/9P07XNM5CHP0?hl=en-us&gl=US).
/// This shortcut does not have an icon.

using Trestat.MsixShortcut;

string shortcutPath = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
    "Ambie.lnk");

using var fs = new FileStream(shortcutPath, FileMode.Create);

var writer = new ShellLinkWriter(fs);

writer.Header();

writer.LinkTargetItemId(iw =>
{
    iw.Item(ItemIdWriter.ApplicationsItemId);

    iw.Item(tw =>
    {
        tw.Apps(bw =>
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
        tw => { });
    });
});

writer.Footer();
