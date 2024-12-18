using src.Supabase;
using Supabase.Postgrest.Responses;
namespace libraries.backend;

using Response = ModeledResponse<CategoryListModel>;

public class CategoryList {
    static readonly SupabaseService SERVICE = new SupabaseService();

    // ReSharper disable once MemberCanBePrivate.Global
    public static async Task<Response> fetch() {
        try {
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            var result = await client!.From<CategoryListModel>().Get();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The Category List data fetch failed!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    // ReSharper disable once InconsistentNaming
    public static CategoryListModel getCategory(int categoryID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var first = models.First(c => c.CategoryID == categoryID);

            Console.WriteLine(
                "Found a category info record with the associated category id.");

            return first;
        } catch (Exception ex) {
            Console.WriteLine(
                $"Couldn't find a category record with the associated category id");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    public static List<CategoryListModel> viewCategories() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;
            return models;
        } catch (Exception ex) {
            Console.WriteLine("Error: Failed to fetch the category list information!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }
}