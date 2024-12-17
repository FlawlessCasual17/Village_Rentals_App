using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Village_Rentals_App.bones;

namespace Village_Rentals_App.frontend;

public partial class App : Application {
    public override void Initialize() =>
        AvaloniaXamlLoader.Load(this);


    [SuppressMessage("ReSharper", "EnforceIfStatementBraces")]
    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) 
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }
}