using Avalonia;
using Village_Rentals_App.bones;

namespace Village_Rentals_App;

class Program {
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) 
        => buildAvaloniaApp().StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder buildAvaloniaApp()
        => AppBuilder.Configure<bones.App>().UsePlatformDetect().WithInterFont().LogToTrace();
}
