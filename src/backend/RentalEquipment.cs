using MainApp.Supabase;
using Supabase.Postgrest.Responses;

namespace MainApp.backend;

using RentalEquipmentModel = Supabase.RentalEquipment;
using Response = ModeledResponse<Supabase.RentalEquipment>;

public class RentalEquipment {
    #pragma warning disable CA1822
    public async Task<Response> fetch() {
        #pragma warning restore CA1822
        try {
            var service = new SupabaseService();
            service.intializeService();

            var client = service.Client;

            var result = await client!.From<RentalEquipmentModel>().Get();
            return result;
        }  catch (Exception ex) {
            Console.WriteLine("Error: The data fetch failed!");
            throw new Exception(ex.Message);
        }
    }
    
    // TODO: Add getEquipment(int equipmentID)
    // TODO: Add updateEquipmentStatus()
    // TODO: Add setEquipment()
    // TODO: Add viewEquipmentInventory()
}
