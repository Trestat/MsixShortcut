using System.Xml;

namespace Trestat.MsixShortcut;

public static class ShortcutHelpers
{
    /// <summary>
    /// Creates a new shell link file at the given path. If the shortcut file already exists, it will be overwritten.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="pathToShortcut">The path to the shortcut file to be created, ending with an .LNK extension.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void CreateShortcut(PackageShortcutOptions options, string pathToShortcut)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrWhiteSpace(pathToShortcut))
        {
            throw new ArgumentException($"'{nameof(pathToShortcut)}' cannot be null or whitespace.", nameof(pathToShortcut));
        }

        using var fs = new FileStream(pathToShortcut, FileMode.Create, FileAccess.ReadWrite);

        CreateShortcut(options, fs);

        fs.Flush();
    }

    /// <summary>
    /// Creates a new shell link file using the given options and writes it to the output stream.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="outputStream">The stream to which to write the new shell link file. The stream is left open.</param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static void CreateShortcut(PackageShortcutOptions options, Stream outputStream)
    {
        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        if (outputStream is null)
        {
            throw new ArgumentNullException(nameof(outputStream));
        }

        if (!outputStream.CanWrite)
        {
            throw new ArgumentException("The output stream must be writable.", nameof(outputStream));
        }

        var writer = new ShellLinkWriter(outputStream);

        writer.Header();
        
        writer.LinkTargetItemId((itemIdWriter =>
        {
            // "Applications"
            itemIdWriter.Item(ItemIdWriter.ApplicationsItemId);

            itemIdWriter.Item(mtWriter =>
            {
                mtWriter.Apps(msWriter =>
                {
                    msWriter.Section(
                        leader: 0x00000000,
                        guid: KnownSectionGuids.Package,
                        dataEntryWriter =>
                        {
                            dataEntryWriter.U32((uint)PackageKey.ActivationBehavior, (uint)options.ActivationBehavior); // Required for launch
                            dataEntryWriter.Text((uint)PackageKey.PackageFamilyNameAppTarget, $"{options.PackageFamilyName}!{options.AppIdentifier}"); // Required for launch
                            dataEntryWriter.Text((uint)PackageKey.PackageInstallPath, options.PackageInstallationPath, extra: 0); // Required for icon to display correctly
                        });

                    msWriter.Section(
                        leader: 0x00000000,
                        guid: KnownSectionGuids.Assets,
                        dataEntryWriter =>
                        {
                            /*
                             * All square tile assets must have a non-null `extra` value for it to be recognized as an icon for the desktop shortcut.
                             * This is not relevant if the icon is merely pinned to the taskbar or to the Start menu as a tile.
                             *
                             * (Win10 19045) Regardless of the asset provided, at least one must be provided for the shortcut to have an icon.
                             * Additionally, it does not matter which asset key is provided, as Explorer will always use the appropriate
                             * small, medium, or large tile depending on factors such as UI scaling and the display mode in Explorer
                             * (i.e. "Extra large icons", "Medium icons", "Details" views, etc.)
                             * Also, it is not strictly required that the asset key point to a specific file. It is enough to, for example,
                             * set Square44x44Logo to the folder name "Images" or "Assets", whichever contains the images. Empty strings
                             * do not work, and the folder or file name must exist. Setting it to "appxmanifest" also seems to work.
                             */

                            if (options.Square44x44LogoPath is not null)
                            {
                                dataEntryWriter.Text((uint)AssetKey.Square44x44Logo, options.Square44x44LogoPath, extra: 0);
                            }

                            if (options.Square71x71LogoPath is not null)
                            {
                                dataEntryWriter.Text((uint)AssetKey.Square71x71Logo, options.Square71x71LogoPath, extra: 0);
                            }

                            if (options.Square150x150LogoPath is not null)
                            {
                                dataEntryWriter.Text((uint)AssetKey.Square150x150Logo, options.Square150x150LogoPath, extra: 0);
                            }

                            if (options.Square310x310LogoPath is not null)
                            {
                                dataEntryWriter.Text((uint)AssetKey.Square310x310Logo, options.Square310x310LogoPath, extra: 0);
                            }

                            if (options.Wide310x150LogoPath is not null)
                            {
                                dataEntryWriter.Text((uint)AssetKey.Wide310x150Logo, options.Wide310x150LogoPath, extra: 0);
                            }
                        });

                    msWriter.Section(
                        leader: 0x00000000,
                        guid: KnownSectionGuids.Taskbar,
                        dataEntryWriter =>
                        {
                            dataEntryWriter.Text((uint)TaskbarKey.DisplayName, options.DisplayName);
                        });
                },
                trailerWriter =>
                {
                    /*
                     * Nothing.
                     * 
                     * To emulate the trailer that Win10 19045 creates when dragging a packaged app off of the Start menu, we can write this:
                     * 
                     * trailerWriter.Section(
                     *	leader: 0xBEEF0027,
                     *	guid: KnownSectionGuids.ExperienceHost,
                     *	dataEntryWriter =>
                     *	{
                     *		dataEntryWriter.Text((uint)ExperienceHostKey.Unknown64, "Microsoft.Windows.StartMenuExperienceHost_cw5n1h2txyewy");
                     *	});
                     */
                });
            });
        }));

        writer.Footer();
    }
}

internal static class XmlExtensions
{
    public static IEnumerable<XmlElement> FindElements(this XmlNode node, string nodeLocalName, string namespaceUri) =>
        node
        .Cast<XmlElement>()
        .Where(n =>
            string.Equals(n.LocalName, nodeLocalName, StringComparison.OrdinalIgnoreCase)
            && string.Equals(n.NamespaceURI, namespaceUri, StringComparison.OrdinalIgnoreCase));

    public static XmlElement? FindFirstElementOrDefault(this XmlNode node, string nodeLocalName, string namespaceUri) =>
        node
        .FindElements(nodeLocalName, namespaceUri)
        .FirstOrDefault();

    public static XmlElement FindFirstElementOrThrow(this XmlNode node, string nodeLocalName, string namespaceUri) =>
        node
        .FindFirstElementOrDefault(nodeLocalName, namespaceUri)
        ?? throw new MsixShortcutException($"Could not find an '{nodeLocalName}' node in the AppxManifest.");

    public static XmlAttribute? FindAttributeOrDefault(this XmlElement node, string attributeLocalName, string? namespaceUri = null) =>
        node.Attributes
        .Cast<XmlAttribute>()
        .FirstOrDefault(a =>
            string.Equals(a.LocalName, attributeLocalName, StringComparison.OrdinalIgnoreCase)
            && (string.IsNullOrEmpty(namespaceUri)
                ? (string.IsNullOrEmpty(a.NamespaceURI) || string.Equals(a.OwnerElement!.NamespaceURI, a.NamespaceURI, StringComparison.OrdinalIgnoreCase))
                : string.Equals(a.NamespaceURI, namespaceUri, StringComparison.OrdinalIgnoreCase)));

    public static string? GetAttributeValueOrDefault(this XmlElement node, string attributeLocalName, string? namespaceUri = null) =>
        node
        .FindAttributeOrDefault(attributeLocalName, namespaceUri)
        ?.Value;

    public static string GetAttributeValueOrThrow(this XmlElement node, string attributeLocalName, string? namespaceUri = null) =>
        (node
        .FindAttributeOrDefault(attributeLocalName, namespaceUri)
        ?? throw new MsixShortcutException($"Could not find attribute '{attributeLocalName}' on node '{node.LocalName}' in the AppxManifest."))
        .Value;
}

/// <summary>
/// Describes the package and icon assets for the purpose of creating a shell link.
/// </summary>
/// <remarks>
/// <para>
/// At least one Logo resource is required for your shortcut to render an icon. From testing on Win10 build 19045 and Win11 build 22621, it does not matter which Logo you specify. It also does not matter if
/// the specified file is an image, as long as the path exists. It is also possible to specify a directory instead of a file.
/// </para>
/// <para>
/// When displaying your icon, Windows will render it as a square tile by reading directly from the assets declared in the AppxManifest, and it will apply scaling, padding, and background color
/// almost as if it were a tile in the Start menu. If the manifest doesn't specify a background color, the tile backplate will use the user's accent color. It is not possible to specify a null,
/// fully transparent, or different background color to work around this.
/// </para>
/// </remarks>
/// <param name="PackageInstallationPath">The package's installation path for the current version, like <c>C:\Program Files\WindowsApps\43891JeniusApps.Ambie_3.9.26.0_x64__jaj7tphbgjeh8</c>.</param>
/// <param name="PackageFamilyName">The package family name, like <c>43891JeniusApps.Ambie_jaj7tphbgjeh8</c>.</param>
/// <param name="DisplayName">The name of the app, like <c>Ambie</c>.</param>
/// <param name="ActivationBehavior">Specify whether this is a UWP app or a packaged Win32 app. Specifying the wrong value will make your shortcut unlaunchable.</param>
/// <param name="AppIdentifier">
/// The Id of the app that you want to launch. Corresponds to the Id attribute on the <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-application">Application</a>
/// element in the AppxManifest.
/// </param>
/// <param name="Square44x44LogoPath">An optional relative path to the Square44x44Logo resource as specified in the AppxManifest. At least one Logo resource is required for your shortcut to render an icon.</param>
/// <param name="Square71x71LogoPath">An optional relative path to the Square71x71Logo resource as specified in the AppxManifest. At least one Logo resource is required for your shortcut to render an icon.</param>
/// <param name="Square150x150LogoPath">An optional relative path to the Square150x150Logo resource as specified in the AppxManifest. At least one Logo resource is required for your shortcut to render an icon.</param>
/// <param name="Square310x310LogoPath">An optional relative path to the Square310x310Logo resource as specified in the AppxManifest. At least one Logo resource is required for your shortcut to render an icon.</param>
/// <param name="Wide310x150LogoPath">An optional relative path to the Wide310x150Logo resource as specified in the AppxManifest. At least one Logo resource is required for your shortcut to render an icon.</param>
public sealed record PackageShortcutOptions(
    string PackageInstallationPath,
    string PackageFamilyName,
    string DisplayName,
    ActivationBehavior ActivationBehavior,
    string AppIdentifier,
    string? Square44x44LogoPath = null,
    string? Square71x71LogoPath = null,
    string? Square150x150LogoPath = null,
    string? Square310x310LogoPath = null,
    string? Wide310x150LogoPath = null);
