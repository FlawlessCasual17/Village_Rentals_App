using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUI.ViewModels;

namespace GUI.Layouts;

public partial class CategoryListView : UserControl {
    public CategoryListView() {
        InitializeComponent();
        DataContext = new CategoryListViewModel();
    }
}