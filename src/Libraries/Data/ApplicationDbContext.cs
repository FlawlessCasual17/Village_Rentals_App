using Microsoft.EntityFrameworkCore;
using Libraries.Data;

namespace Libraries.Data;

public class ApplicationDbContext : DbContext {
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options
    ) : base(options) { }

    public DbSet<CategoryList> Categories { get; set; }
    public DbSet<CustomerInfo> Customers { get; set; }
    public DbSet<RentalEquipment> RentalEquipments { get; set; }
    public DbSet<RentalInfo> RentalInfos { get; set; }

    protected override void OnModelCreating(
        ModelBuilder modelBuilder
    ) {
        // Configure your entity relationships and constraints here
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CategoryList>(entity =>
        {
            entity.ToTable("category_list");
            entity.HasKey(e => e.CategoryID);
            entity.Property(e => e.CategoryID).HasColumnName("category_id");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();
        });

        modelBuilder.Entity<CustomerInfo>(entity =>
        {
            entity.ToTable("customer_information");
            entity.HasKey(e => e.CustomerID);
            entity.Property(e => e.CustomerID).HasColumnName("customer_id");
            entity.Property(e => e.LastName).HasColumnName("last_name").IsRequired();
            entity.Property(e => e.FirstName).HasColumnName("first_name").IsRequired();
            entity.Property(e => e.ContactPhone).HasColumnName("contact_phone");
            entity.Property(e => e.Email).HasColumnName("email").IsRequired();
        });

        modelBuilder.Entity<RentalEquipment>(entity =>
        {
            entity.ToTable("rental_equipment");
            entity.HasKey(e => e.EquipmentID);
            entity.Property(e => e.EquipmentID).HasColumnName("equipment_id");
            entity.Property(e => e.CategoryID).HasColumnName("category_id");
            entity.Property(e => e.DailyRate).HasColumnName("daily_rate");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Name).HasColumnName("name").IsRequired();

            entity.HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryID);
        });

        modelBuilder.Entity<RentalInfo>(entity =>
        {
            entity.ToTable("rental_information");
            entity.HasKey(e => e.RentalID);
            entity.Property(e => e.RentalID).HasColumnName("rental_id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.CustomerID).HasColumnName("customer_id");
            entity.Property(e => e.EquipmentID).HasColumnName("equipment_id");
            entity.Property(e => e.RentalDate).HasColumnName("rental_date");
            entity.Property(e => e.ReturnDate).HasColumnName("return_date");
            entity.Property(e => e.Cost).HasColumnName("cost");

            entity.HasOne(r => r.Customer)
                .WithMany()
                .HasForeignKey(r => r.CustomerID);

            entity.HasOne(r => r.Equipment)
                .WithMany()
                .HasForeignKey(r => r.EquipmentID);
        });
    }
}
