/// Creates a launchable desktop shortcut to Ambie (https://apps.microsoft.com/detail/9P07XNM5CHP0?hl=en-us&gl=US).
/// This shortcut uses an .ico file for its icon, sourced from https://github.com/jenius-apps/ambie.

using ShellLink.Structures;
using ShortcutWithIcon;
using Trestat.MsixShortcut;

string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

string shortcutPath = Path.Combine(desktopPath, "Ambie.lnk");

// For this demonstration, we'll copy the .ico file next to the shortcut.
string iconPath = Path.Combine(desktopPath, "ambie-icon.ico");

using (var icoFs = new FileStream(iconPath, FileMode.Create))
{
    icoFs.Write(Resources.ambie_icon);
}

using var fs = new FileStream(shortcutPath, FileMode.Create);

var shortcut = new ShellLink.Shortcut
{
    LinkTargetIDList = new()
};

shortcut.LinkTargetIDList.ItemIDList.Add(new ItemID([.. ItemIdWriter.ApplicationsItemId]));

var ms = new MemoryStream();

new MsixTargetWriter(ms)
    .Apps(bw =>
    {
        bw.Section(0x0, KnownSectionGuids.Package, dew =>
        {
            dew.U32((uint)PackageKey.ActivationBehavior, (uint)ActivationBehavior.UWP);
            dew.Text((uint)PackageKey.PackageFamilyName, "43891JeniusApps.Ambie_jaj7tphbgjeh8");
            dew.Text((uint)PackageKey.PackageFamilyNameAppTarget, "43891JeniusApps.Ambie_jaj7tphbgjeh8!App");
        });
        bw.Section(0x0, KnownSectionGuids.Assets, dew =>
        {
            dew.Text((uint)AssetKey.DisplayName, "Ambie");
        });
        bw.Section(0x0, KnownSectionGuids.Taskbar, dew =>
        {
            dew.Text((uint)TaskbarKey.DisplayName, "Ambie", extra: 0);
        });
    },
    tw => { });

shortcut.LinkTargetIDList.ItemIDList.Add(new ItemID(ms.ToArray()));

shortcut.LinkFlags |= ShellLink.Flags.LinkFlags.IsUnicode
    | ShellLink.Flags.LinkFlags.EnableTargetMetadata
    | ShellLink.Flags.LinkFlags.HasIconLocation
    | ShellLink.Flags.LinkFlags.HasExpIcon;

shortcut.IconIndex = 0;

const string iconLocation = @"%userprofile%\desktop\ambie-icon.ico";

shortcut.StringData = new()
{
    IsUnicode = true,
    IconLocation = iconLocation
};

shortcut.ExtraData = new()
{
    IconEnvironmentDataBlock = new(Target: iconLocation)
};

fs.Write(shortcut.GetBytes());
