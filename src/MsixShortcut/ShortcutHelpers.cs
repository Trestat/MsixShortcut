using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MsixShortcut;

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

    /*
    /// <summary>
    /// Creates a new shell link file at the given path from the given package installation. If the shortcut file already exists, it will be overwritten.
    /// </summary>
    /// <param name="packageInstallationPath">
    /// <para>
    /// The package's installation path, like <c>C:\Program Files\WindowsApps\43891JeniusApps.Ambie_3.9.26.0_x64__jaj7tphbgjeh8</c>. 
    /// </para>
    /// <para>
    /// To get the installation path for a package, acquire a <a href="https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.package">Windows.ApplicationModel.Package</a> object and get the
    /// <a href="https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.package.installedlocation">InstalledPath</a> property.
    /// </para>
    /// </param>
    /// <param name="pathToShortcut">The path to the shortcut file to be created, ending with an .LNK extension.</param>
    /// /// <param name="appIdentifier">
    /// The Id of the app that you want to launch. If the AppxManifest contains more than one Application declaration, this parameter is required.
    /// <para>
    /// Corresponds to the Id attribute on the <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-application">Application</a>
    /// element in the AppxManifest.
    /// </para>
    /// </param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="MsixShortcutException"></exception>
    public static void CreateShortcut(string packageInstallationPath, string pathToShortcut, string? appIdentifier = null)
    {
        if (string.IsNullOrWhiteSpace(packageInstallationPath))
        {
            throw new ArgumentException($"'{nameof(packageInstallationPath)}' cannot be null or whitespace.", nameof(packageInstallationPath));
        }

        if (string.IsNullOrWhiteSpace(pathToShortcut))
        {
            throw new ArgumentException($"'{nameof(pathToShortcut)}' cannot be null or whitespace.", nameof(pathToShortcut));
        }

        using var fs = new FileStream(pathToShortcut, FileMode.Create, FileAccess.ReadWrite);

        CreateShortcut(packageInstallationPath, fs, appIdentifier);

        fs.Flush();
    }
    */

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
            itemIdWriter.Item(
            [
                0x1F, 0x80, 0x9B, 0xD4, 0x34, 0x42, 0x45, 0x02, 0xF3, 0x4D, 0xB7, 0x80, 0x38, 0x93, 0x94, 0x34, 0x56, 0xE1
            ]);

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
                            // All square tile assets must have a non-null `extra` value for it to be recognized as an icon for the desktop shortcut.
                            // This is not relevant if the icon is merely pinned to the taskbar or to the Start menu as a tile.

                            // (Win10 19045) Regardless of the asset provided, at least one must be provided for the shortcut to have an icon.
                            // Additionally, it does not matter which asset key is provided, as Explorer will always use the appropriate
                            // small, medium, or large tile depending on factors such as UI scaling and the display mode in Explorer
                            // (i.e. "Extra large icons", "Medium icons", "Details" views, etc.)
                            // Also, it is not strictly required that the asset key point to a specific file. It is enough to, for example,
                            // set Square44x44Logo to the folder name "Images" or "Assets", whichever contains the images. Empty strings
                            // do not work, and the folder or file name must exist. Setting it to "appxmanifest" also seems to work.

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
                                dataEntryWriter.Text((uint)AssetKey.Square310x310Logo, options.Square150x150LogoPath, extra: 0);
                            }

                            if (options.Wide310x150LogoPath is not null)
                            {
                                dataEntryWriter.Text((uint)AssetKey.Wide310x150Logo, options.Wide310x150LogoPath, extra: 0);
                            }
                        });
                },
                trailerWriter =>
                {
                    // Nothing
                });
            });
        }));

        writer.Footer();
    }

    /*
    /// <summary>
    /// Creates a new shell link by reading the AppxManifest.xml from the given package installation path and writes it to the output stream.
    /// </summary>
    /// <param name="packageInstallationPath">
    /// <para>
    /// The package's installation path, like <c>C:\Program Files\WindowsApps\43891JeniusApps.Ambie_3.9.26.0_x64__jaj7tphbgjeh8</c>. 
    /// </para>
    /// <para>
    /// To get the installation path for a package, acquire a <a href="https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.package">Windows.ApplicationModel.Package</a> object and get the
    /// <a href="https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.package.installedlocation">InstalledPath</a> property.
    /// </para>
    /// </param>
    /// <param name="outputStream">The stream to which to write the new shell link file. The stream is left open.</param>
    /// <param name="appIdentifier">
    /// The Id of the app that you want to launch. If the AppxManifest contains more than one Application declaration, this parameter is required.
    /// <para>
    /// Corresponds to the Id attribute on the <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-application">Application</a>
    /// element in the AppxManifest.
    /// </para>
    /// </param>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="MsixShortcutException"></exception>
    public static void CreateShortcut(string packageInstallationPath, Stream outputStream, string? appIdentifier = null)
    {
        if (string.IsNullOrWhiteSpace(packageInstallationPath))
        {
            throw new ArgumentException($"'{nameof(packageInstallationPath)}' cannot be null or whitespace.", nameof(packageInstallationPath));
        }

        if (outputStream is null)
        {
            throw new ArgumentNullException(nameof(outputStream));
        }

        if (!outputStream.CanWrite)
        {
            throw new ArgumentException("The output stream must be writable.", nameof(outputStream));
        }

        string xml;

        using (var manifestFileStream = new FileStream(Path.Combine(packageInstallationPath, "AppxManifest.xml"), FileMode.Open, FileAccess.Read, FileShare.Read))
        using (var reader = new StreamReader(manifestFileStream))
        {
            xml = reader.ReadToEnd();
        }

        var options = GetShortcutOptionsFromAppxManifest(packageInstallationPath, xml, appIdentifier);

        CreateShortcut(options, outputStream);
    }

    /// <summary>
    /// Creates a <see cref="PackageShortcutOptions"/> from the given AppxManifest.
    /// </summary>
    /// <param name="packageInstallationPath">
    /// <para>
    /// The package's installation path, like <c>C:\Program Files\WindowsApps\43891JeniusApps.Ambie_3.9.26.0_x64__jaj7tphbgjeh8</c>. 
    /// </para>
    /// <para>
    /// To get the installation path for a package, acquire a <a href="https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.package">Windows.ApplicationModel.Package</a> object and get the
    /// <a href="https://learn.microsoft.com/en-us/uwp/api/windows.applicationmodel.package.installedlocation">InstalledPath</a> property.
    /// </para>
    /// </param>
    /// <param name="xml">The XML contents of the AppxManifest.xml file.</param>
    /// <param name="appIdentifier">
    /// The Id of the app that you want to launch. If the AppxManifest contains more than one Application declaration, this parameter is required.
    /// <para>
    /// Corresponds to the Id attribute on the <a href="https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-application">Application</a>
    /// element in the AppxManifest.
    /// </para>
    /// </param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="MsixShortcutException">Thrown when the AppxManifest does not contain a required element or attribute.</exception>
    public static PackageShortcutOptions GetShortcutOptionsFromAppxManifest(string packageInstallationPath, string xml, string? appIdentifier = null)
    {
        if (string.IsNullOrWhiteSpace(packageInstallationPath))
        {
            throw new ArgumentException($"'{nameof(packageInstallationPath)}' cannot be null or whitespace.", nameof(packageInstallationPath));
        }

        if (string.IsNullOrWhiteSpace(xml))
        {
            throw new ArgumentException($"'{nameof(xml)}' cannot be null or whitespace.", nameof(xml));
        }

        var manifest = new XmlDocument();

        manifest.LoadXml(xml);

        const string ns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";
        const string nsUap = "http://schemas.microsoft.com/appx/manifest/uap/windows10";
        const string nsUap10 = "http://schemas.microsoft.com/appx/manifest/uap/windows10/10";

        var packageNode =
            manifest
            .FindFirstElementOrThrow("Package", ns);

        var identityNode =
            packageNode
            .FindFirstElementOrThrow("Identity", ns);

        var applicationNodes =
            packageNode
            .FindFirstElementOrThrow("Applications", ns)
            .FindElements("Application", ns);

        XmlElement applicationNode;

        if (appIdentifier is null)
        {
            if (applicationNodes.Count() > 1)
            {
                throw new ArgumentException($"There is more than one 'Application' node in the AppxManifest. You must specify an app ID to clarify which one to target.", nameof(appIdentifier));
            }

            applicationNode = applicationNodes
                .SingleOrDefault()
                ?? throw new MsixShortcutException("Could not find an 'Application' node in the AppxManifest.");
        }
        else
        {
            applicationNode = applicationNodes
                .FirstOrDefault(n => n.Attributes
                    .Cast<XmlAttribute>()
                    .Any(a =>
                        string.Equals(a.LocalName, "Id", StringComparison.OrdinalIgnoreCase)
                        && string.Equals(a.NamespaceURI, string.Empty, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(a.Value, appIdentifier, StringComparison.OrdinalIgnoreCase)))
                ?? throw new MsixShortcutException($"Could not find any 'Application' node matching Id '{appIdentifier}'.");
        }

        XmlElement visualElementsNode = applicationNode
            .FindFirstElementOrThrow("VisualElements", nsUap);

        XmlElement? defaultTileNode = visualElementsNode
            .FindFirstElementOrDefault("DefaultTile", nsUap);

        ActivationBehavior activationBehavior = CalculateActivationBehavior(
            startPage: applicationNode.GetAttributeValueOrDefault("StartPage", string.Empty),
            entryPoint: applicationNode.GetAttributeValueOrDefault("EntryPoint", string.Empty),
            runtimeBehavior: applicationNode.GetAttributeValueOrDefault("RuntimeBehavior", nsUap10));

        PackageShortcutOptions options = new(
            PackageInstallationPath: packageInstallationPath,
            PackageFamilyName: CalculatePackageFamilyName(
                identityName: identityNode.GetAttributeValueOrThrow("Name", string.Empty),
                identityPublisher: identityNode.GetAttributeValueOrThrow("Publisher", string.Empty)),
            DisplayName: visualElementsNode.GetAttributeValueOrThrow("DisplayName", string.Empty),
            ActivationBehavior: activationBehavior,
            AppIdentifier: applicationNode.GetAttributeValueOrThrow("Id", string.Empty),
            Square44x44LogoPath: visualElementsNode.GetAttributeValueOrThrow("Square44x44Logo", string.Empty),
            Square71x71LogoPath: defaultTileNode?.GetAttributeValueOrDefault("Square71x71Logo", string.Empty),
            Square150x150LogoPath: visualElementsNode.GetAttributeValueOrThrow("Square150x150Logo", string.Empty),
            Wide310x150LogoPath: defaultTileNode?.GetAttributeValueOrDefault("Wide310x150Logo", string.Empty));

        return options;
    }

    private static ActivationBehavior CalculateActivationBehavior(string? startPage, string? entryPoint, string? runtimeBehavior)
    {
        // https://learn.microsoft.com/en-us/uwp/schemas/appxpackage/uapmanifestschema/element-application#remarks

        // StartPage is set for UWP JavaScript apps.
        if (startPage is not null)
        {
            return ActivationBehavior.UWP;
        }

        if (string.Equals(runtimeBehavior, "packagedClassicApp", StringComparison.OrdinalIgnoreCase)
            || string.Equals(runtimeBehavior, "win32App", StringComparison.OrdinalIgnoreCase))
        {
            return ActivationBehavior.Win32;
        }

        if (string.Equals(runtimeBehavior, "windowsApp", StringComparison.OrdinalIgnoreCase))
        {
            return ActivationBehavior.UWP;
        }

        if (string.Equals(entryPoint, "windows.fullTrustApplication", StringComparison.OrdinalIgnoreCase)
            || string.Equals(entryPoint, "windows.partialTrustApplication", StringComparison.OrdinalIgnoreCase))
        {
            return ActivationBehavior.Win32;
        }

        return ActivationBehavior.UWP;
    }

    private static string CalculatePackageFamilyName(string identityName, string identityPublisher)
    {
        const string CrockfordBase32Alphabet = "0123456789abcdefghjkmnpqrstvwxyz";

        using var sha = SHA256.Create();

        byte[] hash = sha.ComputeHash(Encoding.Unicode.GetBytes(identityPublisher));

        string binaryString = string.Concat(
            hash
            .Take(8)
            .Select(c =>
                Convert.ToString(c, toBase: 2)
                .PadLeft(8, '0')))
            + '0'; // 65-bits = 13 * 5

        string encodedPublisherId = string.Concat(
            Enumerable.Range(0, binaryString.Length / 5)
            .Select(i => CrockfordBase32Alphabet
                .Substring(
                    startIndex: Convert.ToInt32(
                        value: binaryString.Substring(i * 5, 5),
                        fromBase: 2),
                    length: 1)));

        return $"{identityName}_{encodedPublisherId}";
    }
    */
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
    string? Wide310x150LogoPath = null);
