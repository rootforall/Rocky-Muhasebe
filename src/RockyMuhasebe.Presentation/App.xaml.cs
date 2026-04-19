using System.Windows;

namespace RockyMuhasebe.Presentation;

/// <summary>
/// Rocky Muhasebe Ana Uygulama Giriş Noktası
/// </summary>
public partial class App : global::System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Türkçe kültür ayarı
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
        System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("tr-TR");
    }
}

