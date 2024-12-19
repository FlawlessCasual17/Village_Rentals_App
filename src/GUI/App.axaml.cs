using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;
using Microsoft.Extensions.DependencyInjection;
namespace GUI;

// ReSharper disable once PartialTypeWithSinglePart
public partial class App : Application {
    static readonly IServiceCollection SERVICES = new ServiceCollection();

    public override void Initialize()
        => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted() {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.MainWindow = new MainWindow();

        base.OnFrameworkInitializationCompleted();
    }

    public override void RegisterServices()
        => SERVICES.AddSingleton<CategoryListViewModel>();
}