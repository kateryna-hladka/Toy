using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Toy.Models;

public partial class ToyContext : DbContext
{
    public ToyContext()
    {
    }

    public ToyContext(DbContextOptions<ToyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Basket> Baskets { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CountryProducer> CountryProducers { get; set; }

    public virtual DbSet<Discount> Discounts { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Packaging> Packagings { get; set; }

    public virtual DbSet<PhoneMe> PhoneMes { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<PhotoBrand> PhotoBrands { get; set; }

    public virtual DbSet<PhotoProduct> PhotoProducts { get; set; }

    public virtual DbSet<PhotoProductOnOrder> PhotoProductOnOrders { get; set; }

    public virtual DbSet<PhotoReview> PhotoReviews { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductDiscount> ProductDiscounts { get; set; }

    public virtual DbSet<ProductOnOrder> ProductOnOrders { get; set; }

    public virtual DbSet<PurchaseHistory> PurchaseHistories { get; set; }

    public virtual DbSet<PurchaseHistoryProduct> PurchaseHistoryProducts { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> User { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KATERYNA;Initial Catalog=Toy;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Basket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Basket__3213E83F3C6DEFC0");

            entity.ToTable("Basket");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasDefaultValue((short)1)
                .HasColumnName("amount");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Basket__product___76969D2E");

            entity.HasOne(d => d.User).WithMany(p => p.Baskets)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Basket__user_id__778AC167");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brand__3213E83FE6054E4B");

            entity.ToTable("Brand");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Category__3213E83F0D2C2E43");

            entity.ToTable("Category");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CountryProducer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Country___3213E83F74FA6766");

            entity.ToTable("Country_Producer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Discount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Discount__3213E83F381D3CA2");

            entity.ToTable("Discount");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DateTimeEnd)
                .HasColumnType("datetime")
                .HasColumnName("date_time_end");
            entity.Property(e => e.DateTimeStart)
                .HasColumnType("datetime")
                .HasColumnName("date_time_start");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.Value)
                .HasColumnType("money")
                .HasColumnName("value");

            entity.HasOne(d => d.Unit).WithMany(p => p.Discounts)
                .HasForeignKey(d => d.UnitId)
                .HasConstraintName("FK__Discount__unit_i__6D0D32F4");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3213E83F79CE17D9");

            entity.ToTable("Material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Packaging>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Packagin__3213E83F6AF0CE2F");

            entity.ToTable("Packaging");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Hight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("hight");
            entity.Property(e => e.Length)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("length");
            entity.Property(e => e.Name)
                .HasMaxLength(55)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.UnitId).HasColumnName("unit_id");
            entity.Property(e => e.Width)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("width");

            entity.HasOne(d => d.Unit).WithMany(p => p.Packagings)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Packaging__unit___5070F446");
        });

        modelBuilder.Entity<PhoneMe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Phone_Me__3213E83F0BBFF5E0");

            entity.ToTable("Phone_Me");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Time).HasColumnName("time");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo__3213E83FEC759EFE");

            entity.ToTable("Photo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsMain)
                .HasDefaultValue(false)
                .HasColumnName("is_Main");
            entity.Property(e => e.PhotoUrl)
                .IsUnicode(false)
                .HasColumnName("photo_url");
        });

        modelBuilder.Entity<PhotoBrand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo_Br__3213E83F7351D051");

            entity.ToTable("Photo_Brand");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.PhotoId).HasColumnName("photo_id");

            entity.HasOne(d => d.Brand).WithMany(p => p.PhotoBrands)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Bra__brand__31B762FC");

            entity.HasOne(d => d.Photo).WithMany(p => p.PhotoBrands)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Bra__photo__367C1819");
        });

        modelBuilder.Entity<PhotoProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo_Pr__3213E83F9D86A2EC");

            entity.ToTable("Photo_Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PhotoId).HasColumnName("photo_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Photo).WithMany(p => p.PhotoProducts)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Pro__photo__2A164134");

            entity.HasOne(d => d.Product).WithMany(p => p.PhotoProducts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Pro__produ__29221CFB");
        });

        modelBuilder.Entity<PhotoProductOnOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo_Pr__3213E83F18BB2931");

            entity.ToTable("Photo_Product_On_Order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PhotoId).HasColumnName("photo_id");
            entity.Property(e => e.ProductOnOrderId).HasColumnName("product_on_order_id");

            entity.HasOne(d => d.Photo).WithMany(p => p.PhotoProductOnOrders)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Pro__photo__37703C52");

            entity.HasOne(d => d.ProductOnOrder).WithMany(p => p.PhotoProductOnOrders)
                .HasForeignKey(d => d.ProductOnOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Pro__produ__3587F3E0");
        });

        modelBuilder.Entity<PhotoReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Photo_Re__3213E83F745F52A3");

            entity.ToTable("Photo_Review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PhotoId).HasColumnName("photo_id");
            entity.Property(e => e.ReviewId).HasColumnName("review_id");

            entity.HasOne(d => d.Photo).WithMany(p => p.PhotoReviews)
                .HasForeignKey(d => d.PhotoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Rev__photo__3864608B");

            entity.HasOne(d => d.Review).WithMany(p => p.PhotoReviews)
                .HasForeignKey(d => d.ReviewId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Photo_Rev__revie__2DE6D218");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product__3213E83F29E5D73F");

            entity.ToTable("Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AgeFrom).HasColumnName("age_from");
            entity.Property(e => e.AgeTo).HasColumnName("age_to");
            entity.Property(e => e.Amount)
                .HasDefaultValue((short)1)
                .HasColumnName("amount");
            entity.Property(e => e.BrandId).HasColumnName("brand_id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CountryProducerId).HasColumnName("country_producer_id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.MaterialId).HasColumnName("material_id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PackagingId).HasColumnName("packaging_id");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.Sex)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("sex");
            entity.Property(e => e.Size)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("size");
            entity.Property(e => e.SizeUnitId).HasColumnName("size_unit_id");
            entity.Property(e => e.Weight)
                .HasColumnType("decimal(5, 2)")
                .HasColumnName("weight");
            entity.Property(e => e.WeightUnitId).HasColumnName("weight_unit_id");
            entity.Property(e => e.PriceUnitId).HasColumnName("price_unit_id")
                .HasDefaultValue(6);

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__brand_i__5EBF139D");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__categor__5DCAEF64");

            entity.HasOne(d => d.CountryProducer).WithMany(p => p.Products)
                .HasForeignKey(d => d.CountryProducerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__country__60A75C0F");

            entity.HasOne(d => d.Material).WithMany(p => p.Products)
                .HasForeignKey(d => d.MaterialId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__materia__619B8048");

            entity.HasOne(d => d.Packaging).WithMany(p => p.Products)
                .HasForeignKey(d => d.PackagingId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product__packagi__628FA481");

            entity.HasOne(d => d.SizeUnit).WithMany(p => p.ProductSizeUnits)
                .HasForeignKey(d => d.SizeUnitId)
                .HasConstraintName("FK__Product__size_un__656C112C");

            entity.HasOne(d => d.WeightUnit).WithMany(p => p.ProductWeightUnits)
                .HasForeignKey(d => d.WeightUnitId)
                .HasConstraintName("FK__Product__weight___6477ECF3");

            entity.HasOne(d => d.PriceUnit).WithMany(p => p.ProductPriceUnits)
                .HasForeignKey(d => d.PriceUnitId)
                .HasConstraintName("FK__Product__price_u__4E53A1AA");
        });

        modelBuilder.Entity<ProductDiscount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product___3213E83F57FDE2A5");

            entity.ToTable("Product_Discount");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.DiscountId).HasColumnName("discount_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Product_D__categ__70DDC3D8");

            entity.HasOne(d => d.Discount).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.DiscountId)
                .HasConstraintName("FK__Product_D__disco__71D1E811");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductDiscounts)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK__Product_D__produ__6FE99F9F");
        });

        modelBuilder.Entity<ProductOnOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Product___3213E83F26B4D262");

            entity.ToTable("Product_On_Order");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("description");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.UserName)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("user_name");

            entity.HasOne(d => d.User).WithMany(p => p.ProductOnOrders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("FK__Product_O__user___4BAC3F29");
        });

        modelBuilder.Entity<PurchaseHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3213E83F828CB065");

            entity.ToTable("Purchase_History");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Amount)
                .HasDefaultValue((short)1)
                .HasColumnName("amount");
            entity.Property(e => e.Date).HasColumnName("date");
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("payment_status");
            entity.Property(e => e.Price)
                .HasColumnType("money")
                .HasColumnName("price");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseHistories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Purchase___produ__68487DD7");

            entity.HasOne(d => d.User).WithMany(p => p.PurchaseHistories)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__Purchase___user___693CA210");
        });

        modelBuilder.Entity<PurchaseHistoryProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3213E83FC92FBBB7");

            entity.ToTable("Purchase_History_Product");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PurchaseHistoryId).HasColumnName("purchase_history_id");
            entity.Property(e => e.PurchaceId).HasColumnName("purchase_id");
            entity.HasOne(d => d.PurchaseHistory).WithMany(p => p.PurchaseHistoryProducts)
                .HasForeignKey(d => d.PurchaseHistoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Purchase___purch__40058253");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Review__3213E83F024CA29D");

            entity.ToTable("Review");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Advantages)
                .IsUnicode(false)
                .HasColumnName("advantages");
            entity.Property(e => e.Comment)
                .IsUnicode(false)
                .HasColumnName("comment");
            entity.Property(e => e.Disadvantages)
                .IsUnicode(false)
                .HasColumnName("disadvantages");
            entity.Property(e => e.Mark).HasColumnName("mark");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Review__product___7B5B524B");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Review__user_id__151B244E");
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Unit__3213E83F185D2DF3");

            entity.ToTable("Unit");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3213E83F1949C92D");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E616480355481").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__User__B43B145F916BF3D6").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(8)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("phone");
            entity.Property(e => e.Surname)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasColumnName("surname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
