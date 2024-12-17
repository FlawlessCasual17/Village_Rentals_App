using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
namespace Village_Rentals_App.Supabase;

// Category List
[Table("category_list")]
public class CategoryList : BaseModel {
    // ReSharper disable ExplicitCallerInfoArgument
    [PrimaryKey("category_id")]
    public int CategoryId { get; init; }
    [Column("name")]
    public string Name { get; set; }
}

// Customer Information
[Table("customer_information")]
public class CustomerInfo : BaseModel {
    // ReSharper disable ExplicitCallerInfoArgument
    [PrimaryKey("customer_id")]
    public int CustomerId { get; init; }
    [Column("last_name")]
    public string LastName { get; set; }
    [Column("first_name")]
    public string FirstName { get; set; }
    [Column("contact_phone")]
    public string? ContactPhone { get; set; }
    [Column("email")]
    public string Email { get; set; }
}

// Rental Equipment
[Table("rental_equipment")]
public class RentalEquipment : BaseModel {
    // ReSharper disable ExplicitCallerInfoArgument
    [PrimaryKey("equipment_id")]
    public int EquipmentId { get; init; }
    [Column("category_id")]
    public int CategoryId { get; set; }
    [Column("daily_rate")]
    public decimal DailyRate { get; set; }
    [Column("description")]
    public string? Description { get; set; }
    [Column("name")]
    public string Name { get; set; }
    // Navigation property
    public CategoryList Category { get; set; }
}

// Rental Information
public class RentalInfo : BaseModel {
    // ReSharper disable InconsistentNaming
    // ReSharper disable ExplicitCallerInfoArgument
    [PrimaryKey("rental_id")]
    public int RentalID { get; init; }
    [Column("date")]
    public DateTime Date { get; set; }
    [Column("customer_id")]
    public int CustomerID { get; set; }
    [Column("equipment_id")]
    public int EquipmentID { get; set; }
    [Column("rental_date")]
    public DateTime RentalDate { get; set; }
    [Column("return_date")]
    public DateTime? ReturnDate { get; set; }
    [Column("cost")]
    public decimal Cost { get; set; }
    // Navigation properties
    public CustomerInfo Customer { get; set; }
    public RentalEquipment Equipment { get; set; }
}

// // For Insert/Update operations you might want these DTOs:
// public class RentalEquipmentUpsertDto {
//     public int CategoryId { get; set; }
//     public decimal DailyRate { get; set; }
//     public string? Description { get; set; }
//     public string Name { get; set; }
// }
//
// public class CustomerInfoUpsertDto {
//     public string? ContactPhone { get; set; }
//     public string Email { get; set; }
//     public string FirstName { get; set; }
//     public string LastName { get; set; }
// }
//
// public class RentalInfoUpsertDto {
//     public decimal Cost { get; set; }
//     public int CustomerId { get; set; }
//     public DateTime Date { get; set; }
//     public int EquipmentId { get; set; }
//     public DateTime RentalDate { get; set; }
//     public DateTime? ReturnDate { get; set; }
// }