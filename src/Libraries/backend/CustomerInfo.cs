using Libraries.Data;
using Microsoft.EntityFrameworkCore;
namespace Libraries.backend;

using Response = List<CustomerInfoModel>;

// ReSharper disable once UnusedType.Global
public class CustomerInfo(DatabaseService service) {
    // private fields
    // public fields
    // ReSharper disable all
    public int? CustomerID { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? ContactPhone { get; set; }
    public string? Email { get; set; }
    // ReSharper restore all

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task<Response> fetch() {
        try {
            await service.intialize();
            var context = service.getDbContext();
            var result = await context.Customers.ToListAsync();
            return result;
        } catch (Exception ex) {
            throw new Exception("Error: The Customer Info data fetch failed!", ex);
        }
    }

    // ReSharper disable once InconsistentNaming
    bool verifyCustomer(int customerID)
        => Task.Run(fetch).GetAwaiter().GetResult()
            .Any(c => c.CustomerID == customerID);

    // ReSharper disable once InconsistentNaming
    public CustomerInfoModel validateCustomer(int customerID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();

            var first = result.First(c => c.CustomerID == customerID);

            Console.WriteLine(
                "Found a customer record associated with the associated customer id.");

            return first;
        } catch (Exception ex) {
            throw new Exception(
                "Couldn't find a customer record with the associated customer id",
                ex);
        }
    }

    public Response viewCustomers() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: Couldn't fetch the customer list.");
            throw new Exception(ex.Message);
        }
    }

    public async Task<Response> createCustomer() {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            var recordModel = new CustomerInfoModel() {
                CustomerID = null,
                LastName = LastName,
                FirstName = FirstName,
                ContactPhone = ContactPhone,
                Email = Email
            };

            await dbContext.Customers.AddAsync(recordModel);
            await dbContext.SaveChangesAsync();

            var result = await dbContext.Customers.ToListAsync();
            // ReSharper disable once InconsistentNaming
            var customerID = result.First(c =>
                c.ContactPhone == ContactPhone &&
                c.Email == Email).CustomerID;
            var proof = verifyCustomer((int)customerID!);

            // ReSharper disable once InvertIf
            if (proof) {
                var first = result.First(c =>
                    c.ContactPhone == ContactPhone &&
                    c.Email == Email);
                CustomerID = first.CustomerID;
                LastName = first.LastName;
                FirstName = first.FirstName;
            }

            return result;
        } catch (Exception ex) {
            throw new Exception("Failed to create a new customer record.", ex);
        }
    }

    // ReSharper disable once InconsistentNaming
    public async Task<Response> updateCustomerInfo(int? customerID = null) {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            await dbContext.Customers
                .Where(c => c.CustomerID == customerID)
                .ExecuteUpdateAsync(set => set
                    .SetProperty(c => c.CustomerID, customerID ?? CustomerID)
                    .SetProperty(c => c.LastName, LastName)
                    .SetProperty(c => c.FirstName, FirstName)
                    .SetProperty(c => c.ContactPhone, ContactPhone)
                    .SetProperty(c => c.Email, Email)
            );
            await dbContext.SaveChangesAsync();

            var result = await dbContext.Customers.ToListAsync();
            var proof = verifyCustomer((int)customerID!);

            // ReSharper disable once InvertIf
            if (proof) {
                var first = result.First(c =>
                    c.ContactPhone == ContactPhone &&
                    c.Email == Email);
                CustomerID = first.CustomerID;
                LastName = first.LastName;
                FirstName = first.FirstName;
            }

            return result;
        } catch (Exception ex) {
            throw new Exception(
                "Failed to update a customer record based on the customer ID.", ex);
        }
    }

    // ReSharper disable once InconsistentNaming
    public async Task deleteCustomerInfo(int customerID) {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            await dbContext.Customers
                .Where(c => c.CustomerID == customerID)
                .ExecuteDeleteAsync();
            await dbContext.SaveChangesAsync();

            var proof = verifyCustomer(customerID);
            if (!proof)
                Console.WriteLine("Customer record deleted successfully.");
        } catch (Exception ex) {
            throw new Exception(
                "Error: Failed to delete a customer record based on the customer ID.", ex);
        }
    }
}
