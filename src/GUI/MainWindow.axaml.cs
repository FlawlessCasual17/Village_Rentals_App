using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace GUI;

public partial class MainWindow : Window {
    public MainWindow() {
        InitializeComponent();
        // Add more code here later on...
        AvaloniaXamlLoader.Load(this);

#if DEBUG
    this.AttachDevTools();
#endif
    }
}