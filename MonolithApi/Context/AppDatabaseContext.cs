using Microsoft.EntityFrameworkCore;
using MonolithApi.Data;
using MonolithApi.Models;

namespace MonolithApi.Context
{
    public class AppDatabaseContext : DbContext
    {
        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().
                HasOne(p => p.ProductType).WithMany(pt => pt.Products);

            modelBuilder.Entity<Product>().
                HasOne(p => p.Shop).WithMany(s => s.Products);

            modelBuilder.Entity<Product>().
                HasMany(p => p.ProductReductions).WithOne(pr => pr.Product);

            modelBuilder.Entity<Product>().
                HasIndex(p => new
                {
                    p.ProductTypeId,
                    p.ShopId,
                    p.Name
                }).IsUnique();

            modelBuilder.Entity<Product>().
                Property(a => a.CreatedAt).
                HasDefaultValueSql("now()");

            modelBuilder.Entity<Product>().
                Property(a => a.UpdatedAt).
                HasDefaultValueSql("now()");

            //---------------------

            modelBuilder.Entity<Address>().
                HasIndex(a => new 
                {
                    a.Street,
                    a.StreetNumber,
                    a.Country,
                    a.City,
                    a.PostalCode
                }).IsUnique();

            modelBuilder.Entity<Address>().
                Property(a => a.CreatedAt).HasDefaultValueSql("now()");

            modelBuilder.Entity<Address>().
                Property(a => a.UpdatedAt).HasDefaultValueSql("now()");

            //-----------------

            modelBuilder.Entity<ProductType>().
                Property(a => a.CreatedAt).HasDefaultValueSql("now()");

            modelBuilder.Entity<ProductType>().
                Property(a => a.UpdatedAt).HasDefaultValueSql("now()");

            //-------------------

            modelBuilder.Entity<Reduction>().
                HasIndex(r => new
                {
                    r.Title,
                    r.ShopId
                }).IsUnique();

            modelBuilder.Entity<Reduction>().
                HasMany(r => r.ProductReductions).WithOne(pr => pr.Reduction);

            modelBuilder.Entity<Reduction>().
                Property(a => a.CreatedAt).HasDefaultValueSql("now()");

            modelBuilder.Entity<Shop>().
                Property(a => a.UpdatedAt).HasDefaultValueSql("now()");

            //-----------------------

            modelBuilder.Entity<ProductReduction>().
                HasIndex(pr => new
                {
                    pr.ProductId,
                    pr.ReductionId
                }).IsUnique();

            modelBuilder.Entity<ProductReduction>().
               Property(pr => pr.CreatedAt).HasDefaultValueSql("now()");

            modelBuilder.Entity<ProductReduction>().
               Property(pr => pr.UpdatedAt).HasDefaultValueSql("now()");

            //------------------

            modelBuilder.Entity<Shop>().
                HasIndex(s=> s.ShopName).IsUnique();

            modelBuilder.Entity<Shop>().
                HasMany(s => s.Reductions).WithOne(r => r.Shop);

            modelBuilder.Entity<Shop>().
                Property(a => a.CreatedAt).HasDefaultValueSql("now()");

            modelBuilder.Entity<Shop>().
                Property(a => a.UpdatedAt).HasDefaultValueSql("now()");

            //---------
           
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductReduction> ProductReductions { get; set; }
        public DbSet<Reduction> Reductions { get; set; }

    }
}
