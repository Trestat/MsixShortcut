namespace MsixShortcut.Tests
{
    public sealed class TestReadingAppxManifest
    {
        /*
        [Test]
        public void GetShortcutOptionsFromAppxManifest_ShouldReturnValidOptions()
        {
            string xml =
"""
<?xml version="1.0" encoding="utf-8" standalone="yes"?>
<Package IgnorableNamespaces="uap mp uap3 uap5 build" xmlns="http://schemas.microsoft.com/appx/manifest/foundation/windows10" xmlns:mp="http://schemas.microsoft.com/appx/2014/phone/manifest" xmlns:uap="http://schemas.microsoft.com/appx/manifest/uap/windows10" xmlns:uap3="http://schemas.microsoft.com/appx/manifest/uap/windows10/3" xmlns:uap5="http://schemas.microsoft.com/appx/manifest/uap/windows10/5" xmlns:rescap="http://schemas.microsoft.com/appx/manifest/foundation/windows10/restrictedcapabilities" xmlns:build="http://schemas.microsoft.com/developer/appx/2015/build">
  <Identity Name="43891JeniusApps.Ambie" Publisher="CN=0B44CA61-7898-42FE-825C-ADA43BC01A93" Version="4.1.1.0" ProcessorArchitecture="x64" />
  <mp:PhoneIdentity PhoneProductId="ce2960c3-d414-440e-9746-69ca2501e58a" PhonePublisherId="5b2bef43-e87f-4b76-95c9-9a6af1d72a6b" />
  <Properties>
    <DisplayName>ms-resource:AppDisplayName</DisplayName>
    <PublisherDisplayName>Jenius Apps</PublisherDisplayName>
    <Logo>Assets\StoreLogo.png</Logo>
  </Properties>
  <Dependencies>
    <TargetDeviceFamily Name="Windows.Universal" MinVersion="10.0.18362.0" MaxVersionTested="10.0.22000.0" />
    <PackageDependency Name="Microsoft.UI.Xaml.2.8" MinVersion="8.2306.22001.0" Publisher="CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US" />
    <PackageDependency Name="Microsoft.NET.Native.Framework.2.2" MinVersion="2.2.29512.0" Publisher="CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US" />
    <PackageDependency Name="Microsoft.NET.Native.Runtime.2.2" MinVersion="2.2.28604.0" Publisher="CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US" />
    <PackageDependency Name="Microsoft.VCLibs.140.00" MinVersion="14.0.30704.0" Publisher="CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US" />
    <PackageDependency Name="Microsoft.Services.Store.Engagement" MinVersion="10.0.19011.0" Publisher="CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US" />
    <PackageDependency Name="Microsoft.VCLibs.140.00.UWPDesktop" MinVersion="14.0.30704.0" Publisher="CN=Microsoft Corporation, O=Microsoft Corporation, L=Redmond, S=Washington, C=US" />
  </Dependencies>
  <Resources>
    <Resource Language="EN-US" />
    <Resource uap:Scale="200" />
  </Resources>
  <Applications>
    <Application Id="App" Executable="AmbientSounds.Uwp.exe" EntryPoint="AmbientSounds.App">
      <uap:VisualElements DisplayName="ms-resource:AppDisplayName" Square150x150Logo="Assets\Square150x150Logo.png" Square44x44Logo="Assets\Square44x44Logo.png" Description="AmbientSounds" BackgroundColor="transparent">
        <uap:DefaultTile Wide310x150Logo="Assets\Wide310x150Logo.png" Square71x71Logo="Assets\SmallTile.png" Square310x310Logo="Assets\LargeTile.png" ShortName="Ambie">
          <uap:ShowNameOnTiles>
            <uap:ShowOn Tile="square150x150Logo" />
            <uap:ShowOn Tile="wide310x150Logo" />
            <uap:ShowOn Tile="square310x310Logo" />
          </uap:ShowNameOnTiles>
        </uap:DefaultTile>
        <uap:SplashScreen Image="Assets\SplashScreen.png" a:Optional="true" BackgroundColor="transparent" xmlns:a="http://schemas.microsoft.com/appx/manifest/uap/windows10/5" />
      </uap:VisualElements>
      <Extensions>
        <uap:Extension Category="windows.appService">
          <uap:AppService Name="com.jeniusapps.ambie" />
        </uap:Extension>
        <uap:Extension Category="windows.protocol" EntryPoint="AmbientSounds.App">
          <uap:Protocol Name="ambie">
            <uap:DisplayName>ms-resource:AppDisplayName</uap:DisplayName>
          </uap:Protocol>
        </uap:Extension>
        <Extension Category="windows.backgroundTasks" EntryPoint="AmbientSounds.Tasks.StartupTask">
          <BackgroundTasks>
            <Task Type="systemEvent" />
          </BackgroundTasks>
        </Extension>
      </Extensions>
    </Application>
  </Applications>
  <Capabilities>
    <Capability Name="internetClient" />
    <uap3:Capability Name="backgroundMediaPlayback" />
    <uap:Capability Name="userAccountInformation" />
    <rescap:Capability Name="confirmAppClose" />
  </Capabilities>
  <Extensions>
    <Extension Category="windows.activatableClass.inProcessServer">
      <InProcessServer>
        <Path>Microsoft.Graphics.Canvas.dll</Path>
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasVirtualBitmap" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasRenderTarget" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Svg.CanvasSvgDocument" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasBitmap" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.DpiCompensationEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.BorderEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.GammaTransferEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.HueToRgbEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.PixelShaderEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.OpacityEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.HueRotationEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.PosterizeEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.HighlightsAndShadowsEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.WhiteLevelAdjustmentEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.SpotDiffuseEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.TableTransfer3DEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ColorSourceEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ShadowEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ArithmeticCompositeEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.EdgeDetectionEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.StraightenEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.TileEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.EffectTransferTable3D" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.PointSpecularEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.CropEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.AtlasEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.CompositeEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.DirectionalBlurEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.AlphaMaskEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ChromaKeyEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.HdrToneMapEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.SharpenEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.VignetteEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ConvolveMatrixEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.DistantSpecularEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.SepiaEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ScaleEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.InvertEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.MorphologyEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ColorManagementProfile" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.DistantDiffuseEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.CrossFadeEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.LuminanceToAlphaEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.GaussianBlurEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.BlendEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ColorManagementEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.PointDiffuseEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.TurbulenceEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.PremultiplyEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.GrayscaleEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.TemperatureAndTintEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.SpotSpecularEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.TableTransferEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.OpacityMetadataEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.Transform3DEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.Transform2DEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.EmbossEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.RgbToHueEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.DiscreteTransferEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.LinearTransferEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.UnPremultiplyEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.BrightnessEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ContrastEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.TintEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ColorMatrixEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.SaturationEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.DisplacementMapEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Effects.ExposureEffect" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasSwapChain" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasCommandList" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Printing.CanvasPrintDocument" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.CanvasCreateResourcesEventArgs" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Composition.CanvasComposition" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasVirtualImageSource" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedControl" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasImageSource" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasVirtualControl" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedDrawEventArgs" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasAnimatedUpdateEventArgs" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.UI.Xaml.CanvasSwapChainPanel" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasDevice" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Geometry.CanvasGeometry" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Geometry.CanvasGradientMesh" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Geometry.CanvasStrokeStyle" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Geometry.CanvasCachedGeometry" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Geometry.CanvasPathBuilder" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasFontSet" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasTypography" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasNumberSubstitution" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasTextLayout" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasTextFormat" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasTextRenderingParameters" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Text.CanvasTextAnalyzer" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasImage" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Brushes.CanvasRadialGradientBrush" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Brushes.CanvasImageBrush" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Brushes.CanvasLinearGradientBrush" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.Brushes.CanvasSolidColorBrush" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Graphics.Canvas.CanvasSpriteBatch" ThreadingModel="both" />
      </InProcessServer>
    </Extension>
    <Extension Category="windows.activatableClass.inProcessServer">
      <InProcessServer>
        <Path>AmbientSounds.Uwp.dll</Path>
        <ActivatableClass ActivatableClassId="AmbientSounds.Tasks.StartupTask" ThreadingModel="both" />
      </InProcessServer>
    </Extension>
    <Extension Category="windows.activatableClass.inProcessServer">
      <InProcessServer>
        <Path>Microsoft.Web.WebView2.Core.dll</Path>
        <ActivatableClass ActivatableClassId="Microsoft.Web.WebView2.Core.CoreWebView2Environment" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Web.WebView2.Core.CoreWebView2Controller" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Web.WebView2.Core.CoreWebView2CompositionController" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Web.WebView2.Core.CoreWebView2ControllerWindowReference" ThreadingModel="both" />
        <ActivatableClass ActivatableClassId="Microsoft.Web.WebView2.Core.CoreWebView2EnvironmentOptions" ThreadingModel="both" />
      </InProcessServer>
    </Extension>
  </Extensions>
  <build:Metadata>
    <build:Item Name="TargetFrameworkMoniker" Value=".NETCore,Version=v5.0" />
    <build:Item Name="VisualStudio" Version="17.0" />
    <build:Item Name="OperatingSystem" Version="10.0.20348.1 (WinBuild.160101.0800)" />
    <build:Item Name="Microsoft.Build.AppxPackage.dll" Version="17.8.37259.26410" />
    <build:Item Name="ProjectGUID" Value="{B0BA021E-FA43-4921-A25A-0004A43F7C40}" />
    <build:Item Name="ilc.exe" Version="2.2.31116.00 built by: PROJECTNREL" />
    <build:Item Name="Microsoft.Windows.UI.Xaml.Build.Tasks.dll" Version="0.0.0.0" />
    <build:Item Name="OptimizingToolset" Value="ilc.exe" />
    <build:Item Name="UseDotNetNativeSharedAssemblyFrameworkPackage" Value="false" />
    <build:Item Name="DisableStackTraceMetadata" Value="false" />
    <build:Item Name="DisableExceptionMessages" Value="false" />
    <build:Item Name="ShortcutGenericAnalysis" Value="false" />
    <build:Item Name="GeneratePGD" Value="false" />
    <build:Item Name="ConsumePGD" Value="false" />
    <build:Item Name="SingleThreadNUTC" Value="false" />
    <build:Item Name="Use64BitCompiler" Value="false" />
    <build:Item Name="OptimizeForSize" Value="false" />
    <build:Item Name="AlignMethodsAtMinimalBoundaries" Value="false" />
    <build:Item Name="LargeAddressAware" Value="false" />
    <build:Item Name="NoLinkerSymbols" Value="false" />
    <build:Item Name="OutOfProcPDB" Value="false" />
    <build:Item Name="MakePri.exe" Version="10.0.22000.832 (WinBuild.160101.0800)" />
  </build:Metadata>
</Package>
""";
            var options = ShortcutHelpers.GetShortcutOptionsFromAppxManifest(@"c:\path\to\installation", xml);

            var expected = new PackageShortcutOptions(
                PackageInstallationPath: @"c:\path\to\installation",
                PackageFamilyName: "",
                DisplayName: "Ambie",
                )

            Assert.That(options, Is.EqualTo(new PackageShortcutOptions())
        }
        */
    }
}