using src.Supabase;
using Supabase.Postgrest.Responses;
namespace src.backend;

using Response = ModeledResponse<Supabase.CustomerInfo>;

// ReSharper disable once UnusedType.Global
public class CustomerInfo {
    // private fields
    static readonly SupabaseService SERVICE = new SupabaseService();
    // public fields
    // ReSharper disable all
    public int? CustomerID { get; set; }
    public string? LastName { get; set; }
    public string? FirstName { get; set; }
    public string? ContactPhone { get; set; }
    public string? Email { get; set; }
    // ReSharper restore all

    // ReSharper disable once MemberCanBePrivate.Global
    public static async Task<Response> fetch() {
        try {
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            var result = await client!.From<Supabase.CustomerInfo>().Get();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The Customer Info data fetch failed!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    // ReSharper disable once InconsistentNaming
    static bool verifyCustomer(int customerID) 
        => Task.Run(fetch).GetAwaiter().GetResult()
            .Models.Any(c => c.CustomerID == customerID);

    // ReSharper disable once InconsistentNaming
    public static Supabase.CustomerInfo validateCustomer(int customerID) {
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

    public static List<Supabase.CustomerInfo> viewCustomers() {
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
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            var recordModel = new Supabase.CustomerInfo {
                CustomerID = null,
                LastName = LastName,
                FirstName = FirstName,
                ContactPhone = ContactPhone,
                Email = Email
            };

            var result = await client!.From<Supabase.CustomerInfo>().Insert(recordModel);
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
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            var recordModel = new Supabase.CustomerInfo {
                CustomerID = customerID ?? CustomerID,
                LastName = LastName,
                FirstName = FirstName,
                ContactPhone = ContactPhone,
                Email = Email
            };

            var result = await client!.From<Supabase.CustomerInfo>()
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
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            await client!.From<Supabase.CustomerInfo>()
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