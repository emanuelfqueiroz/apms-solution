using AffiliatePMS.Domain.AffiliateCustomers;
using AffiliatePMS.Domain.Affiliates;
using AffiliatePMS.Domain.Common;
using AffiliatePMS.Domain.Finance;
using AffiliatePMS.Domain.Sales;
using Microsoft.EntityFrameworkCore;

namespace AffiliatePMS.Infra;

public partial class APMSDbContext : DbContext
{
    public APMSDbContext()
    {
    }

    public APMSDbContext(DbContextOptions<APMSDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Affiliate> Affiliates { get; set; }

    public virtual DbSet<AffiliateAddress> AffiliateAddresses { get; set; }

    public virtual DbSet<AffiliateBankAccount> AffiliateBankAccounts { get; set; }

    public virtual DbSet<AffiliateCustomer> AffiliateCustomers { get; set; }

    public virtual DbSet<AffiliateCustomerTag> AffiliateCustomerTags { get; set; }

    public virtual DbSet<AffiliateDetail> AffiliateDetails { get; set; }

    public virtual DbSet<AffiliateSocialMedia> AffiliateSocialMedia { get; set; }

    public virtual DbSet<AppUser> AppUsers { get; set; }

    public virtual DbSet<OrderHeader> OrderHeaders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Affiliate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Affiliate_ID");

            entity.ToTable("Affiliate");
            entity.HasIndex(e => e.PublicName, "IDX_AffiliateDetail_PublicName");
            entity.Property(e => e.Id).ValueGeneratedOnAdd();
            entity.Property(e => e.PublicName).HasMaxLength(255);
            entity.Ignore(e => e.UserCreatedId);
        });

        modelBuilder.Entity<AffiliateAddress>(entity =>
        {
            entity.HasKey(e => e.AffiliateId).HasName("PK_AffiliateAddress_AffiliateId");

            entity.ToTable("AffiliateAddress");

            entity.Property(e => e.AffiliateId).ValueGeneratedNever();
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.City).HasMaxLength(255);
            entity.Property(e => e.Country).HasMaxLength(255);
            entity.Property(e => e.State).HasMaxLength(255);
            entity.Property(e => e.ZipCode).HasMaxLength(50);

            entity.HasOne(d => d.Affiliate).WithOne(p => p.AffiliateAddress)
                .HasForeignKey<AffiliateAddress>(d => d.AffiliateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AffiliateAddress_AffiliateId");
        });

        modelBuilder.Entity<AffiliateBankAccount>(entity =>
        {
            entity.HasKey(e => e.AffiliateId).HasName("PK_AffiliateBankAccount_AffiliateId");

            entity.ToTable("AffiliateBankAccount");

            entity.Property(e => e.AffiliateId).ValueGeneratedNever();
            entity.Property(e => e.BankAccountHolder).HasMaxLength(255);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankAccountType).HasMaxLength(50);
            entity.Property(e => e.BankBranch).HasMaxLength(255);
            entity.Property(e => e.BankName).HasMaxLength(255);

            entity.HasOne(d => d.Affiliate).WithOne(p => p.AffiliateBankAccount)
                .HasForeignKey<AffiliateBankAccount>(d => d.AffiliateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AffiliateBankAccount_AffiliateId");
        });

        modelBuilder.Entity<AffiliateCustomer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AffiliateCustomer_Id");

            entity.ToTable("AffiliateCustomer");

            entity.Property(e => e.AvgTicket).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Gender).HasMaxLength(50);
            entity.Property(e => e.TotalPurchase).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Affiliate).WithMany(p => p.AffiliateCustomers)
                .HasForeignKey(d => d.AffiliateId)
                .HasConstraintName("FK_AffiliateCustomer_AffiliateId");
        });

        modelBuilder.Entity<AffiliateCustomerTag>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("AffiliateCustomerTag");

            entity.Property(e => e.Tag).HasMaxLength(255);
            entity.Property(e => e.Weigth).HasDefaultValue((short)1);

            entity.HasOne(d => d.Affiliate).WithMany()
                .HasForeignKey(d => d.AffiliateId)
                .HasConstraintName("FK_AffiliateCustomerTag_AffiliateId");
        });

        modelBuilder.Entity<AffiliateDetail>(entity =>
        {
            entity.HasKey(e => e.AffiliateId).HasName("PK_AffiliateDetail_AffiliateId");

            entity.ToTable("AffiliateDetail");

            entity.HasIndex(e => e.Email, "IDX_AffiliateDetail_Email").IsUnique();

            entity.Property(e => e.AffiliateId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(255);
            entity.Property(e => e.Phone1).HasMaxLength(50);
            entity.Property(e => e.Phone2).HasMaxLength(50);

            entity.HasOne(d => d.Affiliate).WithOne(p => p.AffiliateDetail)
                .HasForeignKey<AffiliateDetail>(d => d.AffiliateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AffiliateDetail_AffiliateId");
        });

        modelBuilder.Entity<AffiliateSocialMedia>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AffiliateSocialMedia_Id");

            entity.Property(e => e.Type).HasMaxLength(30);
            entity.Property(e => e.Url).HasMaxLength(255);

            entity.HasOne(d => d.Affiliate).WithMany(p => p.AffiliateSocialMedia)
                .HasForeignKey(d => d.AffiliateId)
                .HasConstraintName("FK_AffiliateSocialMedia_AffiliteId");
        });

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AppUser_Id");

            entity.ToTable("AppUser");

            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EncodedPassword)
                .HasMaxLength(128)
                .IsUnicode(false);
            entity.Property(e => e.FullName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasDefaultValue((short)1);
        });

        modelBuilder.Entity<OrderHeader>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Sales_OrderHeader_Id");

            entity.ToTable("OrderHeader", "Sales");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TotalAmountItems).HasColumnType("money");

            entity.HasOne(d => d.AffiliateCustomer).WithMany(p => p.OrderHeaders)
                .HasForeignKey(d => d.AffiliateCustomerId)
                .HasConstraintName("FK__OrderHead__Affil__4D94879B");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Sales_OrderItem_Id");

            entity.ToTable("OrderItem", "Sales");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.TotalAmount).HasColumnType("money");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_Sales_OrderItem_OrderId");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Sales_OrderItem_ProductId");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Sales_Product_Id");

            entity.ToTable("Product", "Sales");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
