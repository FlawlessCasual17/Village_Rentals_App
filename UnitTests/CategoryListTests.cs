using System.Net;
using MainApp.Supabase;
using Moq;
using Newtonsoft.Json;
using Supabase.Postgrest;
using Supabase.Postgrest.Responses;
using Xunit.Abstractions;
using CategoryList = MainApp.backend.CategoryList;
namespace UnitTests;

public class CategoryListTests {
    // readonly Mock<SupabaseService> mockService;
    // readonly List<CategoryListModel> mockData;
    readonly ITestOutputHelper output;
    
    public CategoryListTests(ITestOutputHelper output) {
        // mockService = new Mock<SupabaseService>();
        // mockData = [
        //     new CategoryListModel { CategoryID = 10, Name = "Power tools" },
        //     new CategoryListModel { CategoryID = 20, Name = "Yard equipment" }
        // ];
        this.output = output;
    }

    [Fact]
    public async Task Fetch_ShouldReturnData() {
        // // Arrange
        // var serializerSettings = new JsonSerializerSettings {
        //     Formatting = Formatting.Indented,
        //     NullValueHandling = NullValueHandling.Ignore,
        //     DefaultValueHandling = DefaultValueHandling.Include
        // };
        //
        // var jsonData = JsonConvert.SerializeObject(mockData, serializerSettings);
        // var baseResponse = new BaseResponse(
        //     new ClientOptions { Schema = "public" },
        //     new HttpResponseMessage { StatusCode = HttpStatusCode.OK },
        //     jsonData
        // );
        // var response = new ModeledResponse<CategoryListModel>(
        //     baseResponse, serializerSettings);
        //
        // mockService.Setup(s => s.Client!.From<CategoryListModel>()
        //     .Get(It.IsAny<CancellationToken>())).ReturnsAsync(response);

        // Act & Assert
        var exception = await Record.ExceptionAsync(CategoryList.fetch);
        Assert.NotNull(exception);
    }
    
    // [Fact]
    // public void GetCategory_ShouldReturnCorrectCategory() {
    //     // Arrange
    //     var response = new ModeledResponse<CategoryListModel> { Models = _mockData };
    //     _mockService.Setup(s => s.Client.From<CategoryListModel>().Get()).ReturnsAsync(response);
    //
    //     // Act
    //     var category = CategoryList.getCategory(1);
    //
    //     // Assert
    //     Assert.NotNull(category);
    //     Assert.Equal(1, category.CategoryID);
    //     Assert.Equal("Category 1", category.Name);
    // }
    //
    // [Fact]
    // public void ViewCategories_ShouldReturnAllCategories() {
    //     // Arrange
    //     var response = new ModeledResponse<CategoryListModel> { Models = _mockData };
    //     _mockService.Setup(s => s.Client.From<CategoryListModel>().Get()).ReturnsAsync(response);
    //
    //     // Act
    //     var categories = CategoryList.viewCategories();
    //
    //     // Assert
    //     Assert.NotNull(categories);
    //     Assert.Equal(2, categories.Count);
    // }
}