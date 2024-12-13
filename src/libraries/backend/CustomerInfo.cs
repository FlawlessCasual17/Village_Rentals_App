using libraries.Supabase;
using Supabase.Postgrest.Responses;
namespace libraries.backend;

using Response = ModeledResponse<CustomerInfoModel>;

// ReSharper disable once UnusedType.Global
public class CustomerInfo {
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
            var service = new SupabaseService();

            await service.intialize();
            var client = service.getClient();

            var result = await client.From<CustomerInfoModel>().Get();

            Console.WriteLine(result.Model);

            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The Customer Info data fetch failed!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    // ReSharper disable once InconsistentNaming
    bool verifyCustomer(int customerID)
        => Task.Run(fetch).GetAwaiter().GetResult()
            .Models.Any(c => c.CustomerID == customerID);

    // ReSharper disable once InconsistentNaming
    public CustomerInfoModel validateCustomer(int customerID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var first = models.First(c => c.CustomerID == customerID);

            Console.WriteLine(
                "Found a customer record associated with the associated customer id.");

            return first;
        } catch (Exception ex) {
            Console.WriteLine(
                $"Couldn't find a customer record with the associated customer id");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    public List<CustomerInfoModel> viewCustomers() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;
            return models;
        } catch (Exception ex) {
            Console.WriteLine("Error: Couldn't fetch the customer list.");
            throw new Exception(ex.Message);
        }
    }

    public async Task<Response> createCustomer() {
        try {
            var service = new SupabaseService();

            await service.intialize();
            var client = service.getClient();

            var recordModel = new CustomerInfoModel() {
                CustomerID = null,
                LastName = LastName,
                FirstName = FirstName,
                ContactPhone = ContactPhone,
                Email = Email
            };

            var result = await client!.From<CustomerInfoModel>().Insert(recordModel);
            var models = result.Models;
            var proof = verifyCustomer((int)CustomerID!);

            // ReSharper disable once InvertIf
            if (proof) {
                var first = models.First(c =>
                    c.ContactPhone == ContactPhone &&
                    c.Email == Email);
                CustomerID = first.CustomerID;
                LastName = first.LastName;
                FirstName = first.FirstName;
            }

            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: Failed to create a new customer record.");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    // ReSharper disable once InconsistentNaming
    public async Task<Response> updateCustomerInfo(int? customerID = null) {
        try {
            var service = new SupabaseService();

            await service.intialize();
            var client = service.getClient();

            var recordModel = new CustomerInfoModel {
                CustomerID = customerID ?? CustomerID,
                LastName = LastName,
                FirstName = FirstName,
                ContactPhone = ContactPhone,
                Email = Email
            };

            var result = await client!.From<CustomerInfoModel>()
                .Where(c => c.CustomerID == CustomerID)
                .Update(recordModel);
            var models = result.Models;
            var proof = verifyCustomer((int)CustomerID!);

            // ReSharper disable once InvertIf
            if (proof) {
                var first = models.First(c =>
                    c.ContactPhone == ContactPhone &&
                    c.Email == Email);
                CustomerID = first.CustomerID;
                LastName = first.LastName;
                FirstName = first.FirstName;
            }

            return result;
        } catch (Exception ex) {
            Console.WriteLine(
                "Error: Failed to update a customer record based on the customer ID.");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    // ReSharper disable once InconsistentNaming
    public async Task deleteCustomerInfo(int customerID) {
        try {
            var service = new SupabaseService();

            await service.intialize();
            var client = service.getClient();

            await client!.From<CustomerInfoModel>()
                .Where(c => c.CustomerID == customerID)
                .Delete();

            var proof = verifyCustomer(customerID);
            if (!proof)
                Console.WriteLine("Customer record deleted successfully.");
        } catch (Exception ex) {
            Console.WriteLine(
                "Error: Failed to delete a customer record based on the customer ID.");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }
}
