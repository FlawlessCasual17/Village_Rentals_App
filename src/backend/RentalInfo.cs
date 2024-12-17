using System.Linq;
using Supabase.Postgrest.Responses;
using Village_Rentals_App.Supabase;
namespace Village_Rentals_App.backend;

using RentalInfoModel = Supabase.RentalInfo;
using Response = ModeledResponse<Supabase.RentalInfo>;

public class RentalInfo {
    // private fields
    readonly static SupabaseService SERVICE = new SupabaseService();
    readonly DateTime date = DateTime.Now;
    decimal DailyRate { get; set; }
    decimal TotalCost { get; set; }
    // ReSharper disable all
    int EquipmentID { get; set; }
    int CustomerID { get; set; }
    // public fields
    public int RentalID { get; set; }
    public DateTime ReturnDate { get; set; }
    public DateTime RentalDate { get; set; }
    // ReSharper restore all

    // ReSharper disable once MemberCanBePrivate.Global
    #pragma warning disable CA1822
    public async Task<Response> fetch() {
        #pragma warning restore CA1822
        try {
            var service = new SupabaseService();
            service.intializeService();
    
            var client = service.Client;
    
            var result = await client!.From<RentalInfoModel>().Get();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The data fetch failed!");
            throw new Exception(ex.Message);
        }
    }

    async static Task<decimal> getDailyRate(int equipmentId) {
        const string msg = "Error: The data fetching returned a null value!";
        var data = new RentalEquipment();

        var result = await data.fetch();
        if (result == null) throw new Exception(msg);
        var models = result.Models;

        // First is the equivalent of Where.First() in LINQ.
        // Search through the models and retrieve the value of "daily_rate".
        var linqResult = models.First(e => e.EquipmentId == equipmentId);
        var newDailyRate = linqResult.DailyRate;

        return newDailyRate; // Return the new daily rate.
    }

    // ReSharper disable once InconsistentNaming
    public void calcCost(int equipmentID) {
        const string msg = "The Return date cannot be earlier than the Rental date";
        var rentalDays = (int)(RentalDate - ReturnDate).TotalDays;
        if (rentalDays > 0) throw new ArgumentException(msg);

        var task = Task.Run(() => getDailyRate(equipmentID));
        var newDailyRate = task.GetAwaiter().GetResult();
        DailyRate = newDailyRate;

        var baseCost = rentalDays * DailyRate;

        // ReSharper disable UnreachableSwitchCaseDueToIntegerAnalysis
        switch (rentalDays) {
            case > 30:
                baseCost *= 0.8m;
                break;
            case > 7:
                baseCost *= 0.9m;
                break;
        }

        TotalCost = Math.Round(baseCost, 2);
    }

    // TODO: Implement this method here so it checks
    //      the records for an entry containing a specified
    //      rental id.
    // ReSharper disable once InconsistentNaming
    public bool processReturn(int rentalID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var proof = models.Any(r => r.RentalID == rentalID);

            Console.WriteLine("Found a record associated with the specified rental id.");
            
            return proof;
        } catch (Exception ex) {
            // Log the exception
            var msg = $"Couldn't find a record associated " +
                        $"with the specified rental id.\n{ex.Message}";
            Console.WriteLine(msg);
            return false;
        }
    }

    // ReSharper disable InconsistentNaming
    public async Task<Response> createRental(int equipmentID, int customerID) {
        try {
            SERVICE.intializeService();
            var client = SERVICE.Client;

            var recordModel = new RentalInfoModel {
                Date = date,
                CustomerID = customerID,
                EquipmentID = equipmentID,
                RentalDate = RentalDate,
                ReturnDate = ReturnDate,
                Cost = TotalCost
            };

            var result = await client!.From<RentalInfoModel>().Insert(recordModel);
            var models = result.Models;
            
            // ReSharper disable once InvertIf
            if (models.Count != 0) {
                var first = models.First(r => r.Date == date);
                RentalID = first.RentalID;
                EquipmentID = first.EquipmentID;
                CustomerID = first.CustomerID;
            }
            
            return result;
        } catch (Exception ex) {
            const string msg = "Error: The creation of a new record failed!";
            Console.WriteLine(msg);
            throw new Exception(ex.Message);
        }
    }
    
    // TODO: Redo this method using equipmentID, and customerID
    //  as the new parameters.
    public bool processRental(int equipmentID, int customerID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var proof = models.Any(r => 
                r.EquipmentID == equipmentID && r.CustomerID == customerID);

            Console.WriteLine("Found a record associated with the specified rental id.");
            
            return proof;
        } catch (Exception ex) {
            // Log the exception
            var msg = $"Couldn't find a record associated " +
                        $"with the specified rental id.\n{ex.Message}";
            Console.WriteLine(msg);
            return false;
        }
    }
}
