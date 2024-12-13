using Avalonia.Markup.Xaml.Styling;

namespace Village_Rentals_App.TailwindInspired;

using Avalonia.Controls;
// using Avalonia.Controls.Primitives;
// using Avalonia.Styling;

public static class TailwindClasses {
    public static readonly StyleInclude Text2Xl = new StyleInclude(
        new Uri("avares://Avalonia.Themes.Default/Accents/Base.xaml")
    ) {
        Source = new Uri("avares://TailwindInspiredApp/Styles/Text2xl.xaml")
    };

    public static readonly StyleInclude FontBold = new StyleInclude(
        new Uri("avares://Avalonia.Themes.Default/Accents/Base.xaml")
    ) {
        Source = new Uri("avares://TailwindInspiredApp/Styles/FontBold.xaml")
    };

    // ... other Tailwind CSS classes (e.g., bg-blue-500, text-white, etc.)

    public static void ApplyClasses(this Control control, params StyleInclude[] classes)
        => Array.ForEach(classes, cls => control.Styles.Add(cls) );
}
