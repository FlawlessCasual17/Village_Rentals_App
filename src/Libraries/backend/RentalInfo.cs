using Libraries.Data;
using Microsoft.EntityFrameworkCore;
namespace Libraries.backend;

using Response = List<RentalInfoModel>;

// ReSharper disable once UnusedType.Global
public class RentalInfo(DatabaseService service) {
    // private fields
    readonly DateTime date = DateTime.Now;
    decimal DailyRate { get; set; }
    decimal TotalCost { get; set; }
    // ReSharper disable all
    // public fields
    public int? RentalID { get; set; }
    public int EquipmentID { get; set; }
    public int CustomerID { get; set; }
    public DateTime ReturnDate { get; set; }
    public DateTime RentalDate { get; set; }
    public decimal Cost { get; set; }
    // ReSharper restore all

    // ReSharper disable once MemberCanBePrivate.Global
    public async Task<Response> fetch() {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            var result = await dbContext.RentalInfos.ToListAsync();
            return result;
        } catch (Exception ex) {
            Console.WriteLine("Error: The Rental Info data fetch failed!");
            throw new Exception("Error: The Rental Info data fetch failed!", ex);
        }
    }

    static async Task<decimal> getDailyRate(int equipmentId) {
        const string msg = "Error: The data fetching returned a null value!";

        var newService = new DatabaseService();
        var equipment = new RentalEquipment(newService);
        var result = await equipment.fetch();
        if (result == null) throw new Exception(msg);

        // First is the equivalent of Where.First() in LINQ.
        // Search through the models and retrieve the value of "daily_rate".
        var first = result.First(e => e.EquipmentID == equipmentId);
        var newDailyRate = first.DailyRate;

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
    public bool processReturn(int rentalID) {
        try {
            var result = Task.Run(() => fetch()).GetAwaiter().GetResult();

            var proof = result.Any(r => r.RentalID == rentalID);

            if (!proof) return false;

            Console.WriteLine("Found a rental info record with the associated rental id.");

            return proof;
        } catch (Exception ex) {
            throw new Exception(
                "Couldn't find a rental info record with the associated rental id.", ex);
        }
    }

    // ReSharper disable InconsistentNaming
    public Data.RentalInfo getRentalInfo(int rentalID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();

            var first = result.First(r => r.RentalID == rentalID);

            Console.WriteLine("Found a rental info record with the associated rental id.");

            return first;
        } catch (Exception ex) {
            throw new Exception(
                "Couldn't find a rental info record with the associated rental id", ex);
        }
    }

    public async Task<Response> createRental(int equipmentID, int customerID) {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            var recordModel = new RentalInfoModel {
                RentalID = null!,
                Date = date,
                CustomerID = customerID,
                EquipmentID = equipmentID,
                RentalDate = RentalDate,
                ReturnDate = ReturnDate,
                Cost = TotalCost
            };

            await dbContext.RentalInfos.AddAsync(recordModel);
            await dbContext.SaveChangesAsync();

            var result = await dbContext.RentalInfos.ToListAsync();

            // ReSharper disable once InvertIf
            if (result.Count != 0) {
                var first = result.First(r =>
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
            throw new Exception("Error: The creation of a new rental record has failed!", ex);
        }
    }

    public async Task<Response> updateRental(int rentalID, int equipmentID, int customerID) {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            await dbContext.RentalInfos
                .Where(r =>
                    r.RentalID == RentalID &&
                    r.EquipmentID == equipmentID &&
                    r.CustomerID == CustomerID
                ).ExecuteUpdateAsync(set => set
                    .SetProperty(c => c.RentalID, rentalID)
                    .SetProperty(c => c.EquipmentID, equipmentID)
                    .SetProperty(c => c.CustomerID, customerID)
                    .SetProperty(c => c.RentalDate, RentalDate)
                    .SetProperty(c => c.ReturnDate, ReturnDate)
                    .SetProperty(c => c.Cost, TotalCost)
                );
            await dbContext.SaveChangesAsync();

            var result = await dbContext.RentalInfos.ToListAsync();

            // ReSharper disable once InvertIf
            if (result.Count != 0) {
                var first = result.First(r => r.RentalID == rentalID);
                RentalID = first.RentalID;
                RentalDate = first.RentalDate;
                ReturnDate = first.ReturnDate;
                Cost = first.Cost;
                CustomerID = first.CustomerID;
                EquipmentID = first.EquipmentID;
            }

            return result;
        } catch (Exception ex) {
            throw new Exception(
                "Error: Failed to update the rental record with the associated rental ID.", ex);
        }
    }

    public Response viewAllRentalInfo() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            return result;
        } catch (Exception ex) {
            throw new Exception("Error: Failed to fetch the rental information!", ex);
        }
    }

    public bool processRental(int equipmentID, int customerID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();

            var proof = result.Any(r =>
                r.EquipmentID == equipmentID &&
                r.CustomerID == customerID);

            if (!proof) return false;

            Console.WriteLine("""
                Found an existing record associated with
                the specified equipment ID, and customer ID.
            """);

            return proof;
        } catch (Exception ex) {
            throw new Exception("""
                Couldn't find a record associated with
                the specified equipment ID, and customer ID.
            """, ex);
        }
    }
}
