Certainly! I'll provide you with a general approach to migrate from Supabase to using Npgsql, Microsoft.EntityFrameworkCore, and Npgsql.EntityFrameworkCore.PostgreSQL. This will involve several steps and changes across multiple files. I'll outline the main changes for each file:

1. First, you'll need to add these NuGet packages to your project:
   - Microsoft.EntityFrameworkCore
   - Npgsql.EntityFrameworkCore.PostgreSQL

2. Create a new DbContext class:

**File: c:\Users\krist\Village_Rentals_App\src\Libraries\Data\ApplicationDbContext.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using Libraries.Supabase;

namespace Libraries.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<CategoryList> Categories { get; set; }
        public DbSet<CustomerInfo> Customers { get; set; }
        public DbSet<RentalEquipment> RentalEquipments { get; set; }
        public DbSet<RentalInfo> RentalInfos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here
        }
    }
}
```

3. Update the SupabaseService class to use EF Core:

**File: c:\Users\krist\Village_Rentals_App\src\Libraries\Data\DatabaseService.cs**
```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Libraries.Data
{
    public class DatabaseService
    {
        private readonly IConfiguration _configuration;

        public DatabaseService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ApplicationDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
```

Now, let's update each of the mentioned files:

**File: c:\Users\krist\Village_Rentals_App\src\Libraries\backend\CategoryList.cs**
```csharp
using Libraries.Data;
using Libraries.Supabase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraries.backend
{
    public class CategoryList
    {
        private readonly DatabaseService _databaseService;

        public CategoryList(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<Supabase.CategoryList>> fetch()
        {
            try
            {
                using var context = _databaseService.CreateDbContext();
                return await context.Categories.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: The Category List data fetch failed!");
                throw new Exception(ex.Message, ex);
            }
        }

        public Supabase.CategoryList getCategory(int categoryID)
        {
            try
            {
                using var context = _databaseService.CreateDbContext();
                var category = context.Categories.FirstOrDefault(c => c.CategoryID == categoryID);
                if (category == null)
                {
                    throw new Exception("Category not found");
                }
                return category;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Couldn't find a category record with the associated category id");
                throw new Exception(ex.Message, ex);
            }
        }

        public List<Supabase.CategoryList> viewCategories()
        {
            try
            {
                using var context = _databaseService.CreateDbContext();
                return context.Categories.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: Failed to fetch the category list information!");
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
```

Similar changes need to be made to the other files (@CustomerInfo.cs, @RentalInfo.cs, and @RentalEquipment.cs). The main changes involve:

1. Replacing Supabase client with the ApplicationDbContext.
2. Using Entity Framework Core methods like ToListAsync(), FirstOrDefaultAsync(), etc.
3. Wrapping database operations in using statements to ensure proper disposal of the DbContext.
4. Updating the exception handling to use standard exceptions instead of SupabaseException.

Here's an example of how you might update the CustomerInfo class:

**File: c:\Users\krist\Village_Rentals_App\src\Libraries\backend\CustomerInfo.cs**
```csharp
using Libraries.Data;
using Libraries.Supabase;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraries.backend
{
    public class CustomerInfo
    {
        private readonly DatabaseService _databaseService;

        public CustomerInfo(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public int? CustomerID { get; set; }
        public string? LastName { get; set; }
        public string? FirstName { get; set; }
        public string? ContactPhone { get; set; }
        public string? Email { get; set; }

        public async Task<List<Supabase.CustomerInfo>> fetch()
        {
            try
            {
                using var context = _databaseService.CreateDbContext();
                return await context.Customers.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: The Customer Info data fetch failed!");
                throw new Exception(ex.Message, ex);
            }
        }

        // Implement other methods (validateCustomer, viewCustomers, createCustomer, updateCustomerInfo, deleteCustomerInfo) similarly
    }
}
```

You'll need to make similar changes to the RentalInfo and RentalEquipment classes.

Remember to update your dependency injection setup to include the new DatabaseService and ApplicationDbContext. You might need to add something like this in your startup configuration:

```csharp
services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
services.AddScoped<DatabaseService>();
```

This migration is a significant change and will require careful testing and possibly further adjustments based on your specific requirements and data access patterns. Also, ensure that your connection string is properly configured in your application settings.
