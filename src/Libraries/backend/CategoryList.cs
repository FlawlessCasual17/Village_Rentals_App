using Libraries.Data;
using Microsoft.EntityFrameworkCore;
namespace Libraries.backend;

using Response = List<Data.CategoryList>;

public class CategoryList(DatabaseService service) {
    // ReSharper disable once MemberCanBePrivate.Global
    public async Task<Response> fetch() {
        try {
            await service.intialize();
            var context = service.getDbContext();
            var result = await context.Categories.ToListAsync();
            return result;
        } catch (Exception ex) {
            throw new Exception("Error: The Category List data fetch failed!", ex);
        }
    }

    // ReSharper disable once InconsistentNaming
    public CategoryListModel getCategory(int categoryID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var category = result.First(c => c.CategoryID == categoryID);
            return category ?? throw new Exception("Category not found");
        } catch (Exception ex) {
            const string msg =
                "Couldn't find a category record with the associated category id";
            throw new Exception(msg, ex);
        }
    }

    public Response viewCategories() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: Failed to fetch the category list information!");
            throw new Exception(
                "Error: Failed to fetch the category list information!", ex);
        }
    }
}
