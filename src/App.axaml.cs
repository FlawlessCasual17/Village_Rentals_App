using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
namespace src;

using AppLifetimeControl = IClassicDesktopStyleApplicationLifetime; 

public partial class App : Application {
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted() {
        var appLifetime = ApplicationLifetime;
        if (appLifetime is AppLifetimeControl desktop) 
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}