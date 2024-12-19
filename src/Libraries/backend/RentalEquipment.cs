using Libraries.Data;
using Microsoft.EntityFrameworkCore;
namespace Libraries.backend;

using Response = List<RentalEquipmentModel>;

// ReSharper disable once ClassNeverInstantiated.Global
public class RentalEquipment(DatabaseService service) {
    // public fields
    // ReSharper disable all
    public int EquipmentID { get; set; }
    public int CategoryID { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal DailyRate { get; set; }
    // ReSharper restore all

    public async Task<Response> fetch() {
        try {
            await service.intialize();
            var dbContext = service.getDbContext();

            var result = await dbContext.RentalEquipments.ToListAsync();
            return result;
        } catch (Exception ex) {
            throw new Exception("Error: The Rental Equipment data fetch failed!", ex);
        }
    }

    // ReSharper disable once InconsistentNaming
    public RentalEquipmentModel getEquipment(int equipmentID) {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();

            var first = result.First(r => r.EquipmentID == equipmentID);

            Console.WriteLine("Found an equipment record with the associated equipment id.");

            // ReSharper disable once InvertIf
            if (result.Count != 0) {
                EquipmentID = first.EquipmentID;
                CategoryID = first.CategoryID;
                DailyRate = first.DailyRate;
                Description = first.Description;
                Name = first.Name;
            }

            return first;
        } catch (Exception ex) {
            throw new Exception(
                "Couldn't find an equipment record with the associated equipment id.", ex);
        }
    }

    // NOTE: Not needed right now.
    // // ReSharper disable once AsyncVoidMethod
    // public async Task setEquipment() {
    //     try {
    //         await SERVICE.intializeService();
    //         var client = SERVICE.Client;
    //
    //         var recordModel = new RentalEquipmentModel() {
    //             CategoryID = CategoryID,
    //             DailyRate = DailyRate,
    //             Description = Description,
    //             Name = Name
    //         };
    //
    //         var result = await client!.From<RentalEquipmentModel>()
    //             .Where(r => r.EquipmentID == EquipmentID)
    //             .Insert(recordModel);
    //         var models = result.Models;
    //
    //         // ReSharper disable once InvertIf
    //         if (models.Count!= 0) {
    //             var first = models.First(r => r.EquipmentID == EquipmentID);
    //             CategoryID = first.CategoryID;
    //             DailyRate = first.DailyRate;
    //             Description = first.Description;
    //             Name = first.Name;
    //         }
    //
    //     } catch (Exception ex) { // ReSharper disable once AsyncVoidMethod
    //         Console.WriteLine("Error: The record update operation has failed!");
    //         throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
    //     }
    // }

    public Response viewEquipmentInventory() {
        try {
            var result = Task.Run(fetch).GetAwaiter().GetResult();
            return result;
        } catch (Exception ex) {
            throw new Exception("Error: Couldn't fetch the equipment inventory.", ex);
        }
    }

    // NOTE: Not needed right now.
    // public async Task updateEquipmentDetails(
    //     // ReSharper disable once InconsistentNaming
    //     int equipmentID,
    //     string? name = null,
    //     string? description = null,
    //     decimal? dailyRate = null,
    //     int? categoryId = null
    // ) {
    //     try {
    //         await SERVICE.intializeService();
    //         var client = SERVICE.Client;
    //
    //         var query = client!.From<RentalEquipmentModel>()
    //             .Where(r => r.EquipmentID == equipmentID);
    //
    //         if (name != null)
    //             query = query.Set(r => r.Name!, name);
    //         if (description != null)
    //             query = query.Set(r => r.Description!, description);
    //         if (dailyRate.HasValue)
    //             query = query.Set(r => r.DailyRate, dailyRate.Value);
    //         if (categoryId.HasValue)
    //             query = query.Set(r => r.CategoryID, categoryId.Value);
    //
    //         var result = await query.Update();
    //         var models = result.Models;
    //
    //         Console.WriteLine(
    //             $"Equipment with ID \"{equipmentID}\" " +
    //             "has been updated successfully.");
    //
    //         // ReSharper disable once InvertIf
    //         if (models.Count != 0) {
    //             var first = models.First(r => r.EquipmentID == equipmentID);
    //             EquipmentID = first.EquipmentID;
    //             CategoryID = first.CategoryID;
    //             DailyRate = first.DailyRate;
    //             Description = first.Description;
    //             Name = first.Name;
    //         }
    //     } catch (Exception ex) {
    //         Console.WriteLine("Error updating equipment details.");
    //         throw new SupabaseException(ex.Message, ex.HResult, $"{ex.StackTrace}");
    //     }
    // }
}
