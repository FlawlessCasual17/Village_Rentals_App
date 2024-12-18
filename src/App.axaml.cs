﻿using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

namespace MainApp;

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