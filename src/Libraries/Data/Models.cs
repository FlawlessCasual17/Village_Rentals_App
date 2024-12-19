namespace Libraries.Data;

// ReSharper disable InconsistentNaming
[Table("category_list")]
public class CategoryList {
    [Key]
    [Column("category_id")]
    public int CategoryID { get; set; }

    [Required]
    [Column("name")]
    public string? Name { get; set; }
}

[Table("customer_information")]
public class CustomerInfo {
    [Key]
    [Column("customer_id")]
    public int? CustomerID { get; set; }

    [Required]
    [Column("last_name")]
    public string? LastName { get; set; }

    [Required]
    [Column("first_name")]
    public string? FirstName { get; set; }

    [Column("contact_phone")]
    public string? ContactPhone { get; set; }

    [Required]
    [Column("email")]
    public string? Email { get; set; }
}

[Table("rental_equipment")]
public class RentalEquipment {
    [Key]
    [Column("equipment_id")]
    public int EquipmentID { get; set; }

    [Column("category_id")]
    public int CategoryID { get; set; }

    [Column("daily_rate")]
    public decimal DailyRate { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Required]
    [Column("name")]
    public string? Name { get; set; }

    [ForeignKey("CategoryID")]
    public CategoryList? Category { get; set; }
}

[Table("rental_information")]
public class RentalInfo {
    [Key]
    [Column("rental_id")]
    public int? RentalID { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("customer_id")]
    public int CustomerID { get; set; }

    [Column("equipment_id")]
    public int EquipmentID { get; set; }

    [Column("rental_date")]
    public DateTime RentalDate { get; set; }

    [Column("return_date")]
    public DateTime ReturnDate { get; set; }

    [Column("cost")]
    public decimal Cost { get; set; }

    [ForeignKey("CustomerID")]
    public CustomerInfo? Customer { get; set; }

    [ForeignKey("EquipmentID")]
    public RentalEquipment? Equipment { get; set; }
}
