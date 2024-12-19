using System.Reactive;
using Libraries.Supabase;
using ReactiveUI;
using CategoryList = Libraries.backend.CategoryList;
namespace GUI.ViewModels;

public class CategoryListViewModel : ViewModelBase {
    ObservableCollection<CategoryListModel> categories;

    public ReactiveCommand<Unit, Unit> RefreshCommand { get; init; }

    public ObservableCollection<CategoryListModel> Categories {
        get => categories;
        set => this.RaiseAndSetIfChanged(ref categories, value);
    }

    public CategoryListViewModel() {
        categories = [];

        RefreshCommand = ReactiveCommand
            .CreateFromTask(async () => await loadCategories());

        loadCategories();
    }

    Task loadCategories() {
        try {
            var categoryList = new CategoryList();
            var viewCategories = categoryList.viewCategories();
            Categories = new ObservableCollection<CategoryListModel>(viewCategories);
        } catch (SupabaseException ex) {
            // Handle error appropriately
            Console.WriteLine($"Failed to load categories: {ex.Message}");
        }

        return Task.CompletedTask;
    }
}
