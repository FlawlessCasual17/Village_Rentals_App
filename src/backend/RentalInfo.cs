using src.Supabase;
using Supabase.Postgrest.Responses;
namespace src.backend;

using Response = ModeledResponse<RentalInfoModel>;

// ReSharper disable once UnusedType.Global
public class RentalInfo {
    // private fields
    static readonly SupabaseService SERVICE = new SupabaseService();
    readonly DateTime date = DateTime.Now;
    decimal DailyRate { get; set; }
    decimal TotalCost { get; set; }
    // ReSharper disable InconsistentNaming
    // public fields
    public int? RentalID { get; set; }
    public int EquipmentID { get; set; }
    public int CustomerID { get; set; }
    public DateTime ReturnDate { get; set; }
    public DateTime RentalDate { get; set; }
    public decimal Cost { get; set; }
    // ReSharper restore InconsistentNaming

    // ReSharper disable once MemberCanBePrivate.Global
    public static async Task<Response> fetch() {
        try {
            await SERVICE.intializeService();
            var client = SERVICE.Client;
    
            var result = await client!.From<RentalInfoModel>().Get();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The Rental Info data fetch failed!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    static async Task<decimal> getDailyRate(int equipmentId) {
        const string msg = "Error: The data fetching returned a null value!";

        var result = await RentalEquipment.fetch();
        if (result == null) throw new Exception(msg);
        var models = result.Models;

        // First is the equivalent of Where.First() in LINQ.
        // Search through the models and retrieve the value of "daily_rate".
        var linqResult = models.First(e => e.EquipmentID == equipmentId);
        var newDailyRate = linqResult.DailyRate;

        return newDailyRate; // Return the new daily rate.
    }

    // ReSharper disable once InconsistentNaming
    public decimal calcCost(int equipmentID) {
        const string msg = "The Return date cannot be earlier than the Rental date";
        var rentalDays = (int)(RentalDate - ReturnDate).TotalDays;
        if (rentalDays > 0) throw new ArgumentException(msg);

        var result = Task.Run(() => getDailyRate(equipmentID)).GetAwaiter();
        var newDailyRate = result.GetResult();
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

        var totalCost = Math.Round(baseCost, 2);
        TotalCost = totalCost;
        return totalCost;
    }

    // ReSharper disable once InconsistentNaming
    public static bool processReturn(int rentalID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var proof = models.Any(r => r.RentalID == rentalID);
            
            if (!proof) return false;

            Console.WriteLine("Found a rental info record with the associated rental id.");
            
            return proof;
        } catch (Exception ex) {
            Console.WriteLine(
                "Couldn't find a rental info record with the associated rental id.");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    // ReSharper disable InconsistentNaming
    public static RentalInfoModel getRentalInfo(int rentalID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var first = models.First(r => r.RentalID == rentalID);

            Console.WriteLine("Found a rental info record with the associated rental id.");

            return first;
        } catch (Exception ex) {
            Console.WriteLine(
                $"Couldn't find a rental info record with the associated rental id");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    public async Task<Response> createRental(int equipmentID, int customerID) {
        try {
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            var recordModel = new RentalInfoModel {
                RentalID = null!,
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
                var first = models.First(r => 
                    r.CustomerID == customerID &&
                    r.EquipmentID == equipmentID);
                RentalID = first.RentalID;
                RentalDate = first.RentalDate;
                ReturnDate = first.ReturnDate;
                Cost = first.Cost;
                CustomerID = first.CustomerID;
                EquipmentID = first.EquipmentID;
            }
            
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The creation of a new rental record has failed!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    public async Task<Response> updateRental(int rentalID, int equipmentID, int customerID) {
        try {
            await SERVICE.intializeService();
            var client = SERVICE.Client;

            var recordModel = new RentalInfoModel {
                RentalID = rentalID,
                CustomerID = customerID,
                EquipmentID = equipmentID,
                RentalDate = RentalDate,
                ReturnDate = ReturnDate,
                Cost = TotalCost
            };

            var result = await client!.From<RentalInfoModel>()
                .Where(r => r.RentalID == rentalID)
                .Update(recordModel);
            var models = result.Models;

            // ReSharper disable once InvertIf
            if (models.Count != 0) {
                var first = models.First(r => r.RentalID == rentalID);
                RentalID = first.RentalID;
                RentalDate = first.RentalDate;
                ReturnDate = first.ReturnDate;
                Cost = first.Cost;
                CustomerID = first.CustomerID;
                EquipmentID = first.EquipmentID;
            }
            
            return result;
        } catch (Exception ex) {
            Console.WriteLine(
                "Error: Failed to update the rental record with the associated rental ID.");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    public static List<RentalInfoModel> viewAllRentalInfo() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;
            return models;
        } catch (Exception ex) {
            Console.WriteLine("Error: Failed to fetch the rental information!");
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }

    public static bool processRental(int equipmentID, int customerID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            var models = result.Models;

            var proof = models.Any(r => 
                r.EquipmentID == equipmentID && 
                r.CustomerID == customerID);
            
            if (!proof) return false;

            Console.WriteLine("""
                Found an existing record associated with 
                the specified equipment ID, and customer ID.
            """);
            
            return proof;
        } catch (Exception ex) {
            Console.WriteLine("""
                Couldn't find a record associated with 
                the specified equipment ID, and customer ID.
            """);
            throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
        }
    }
}
