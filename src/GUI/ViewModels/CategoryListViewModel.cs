using System.Reactive;
using Libraries.Data;
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
            var service = new DatabaseService();
            var categoryList = new CategoryList(service);
            var viewCategories = categoryList.viewCategories();
            Categories = new ObservableCollection<CategoryListModel>(viewCategories);
        } catch (Exception ex) {
            throw new Exception("Failed to load categories", ex);
        }

        return Task.CompletedTask;
    }
}
