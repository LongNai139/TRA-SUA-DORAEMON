using DIHA.Models.Product;
using DIHA.Models.Contacts;
using DIHA.Models;
using DIHA.Models.Sell;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using DIHA.Areas.Category.Models.Data;
using DIHA.Areas.Product.Models.Data;
using DIHA.Areas.Invoice.Models.Data;
using DIHA.Areas.Custom.Models.Data;
using DIHA.Areas.Staff.Models.Data;

namespace DIHA.Models
{
    // App.Models.AppDbContext
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }

            modelBuilder.Entity<CategoryProduct>(entity => {
                entity.HasIndex(c => c.Slug)
                      .IsUnique();
            });

            modelBuilder.Entity<PostCategory>(entity => {
                entity.HasKey(c => new { c.PostID, c.CategoryID });
            });

            modelBuilder.Entity<Post>(entity => {
                entity.HasIndex(p => p.Slug)
                      .IsUnique();
            });

            modelBuilder.Entity<CategoryProduct>(entity => {
                entity.HasIndex(c => c.Slug)
                      .IsUnique();
            });

            modelBuilder.Entity<ProductCategoryProduct>(entity => {
                entity.HasKey(c => new { c.ProductID, c.CategoryID });
            });

            modelBuilder.Entity<ProductModel>(entity => {
                entity.HasIndex(p => p.Slug)
                      .IsUnique();
            });
        }
        public DbSet<Contact> Contacts { get; set; }


        public DbSet<CategoryProduct> Categories { get; set; }
        public DbSet<DanhMuc> DanhMucs { get; set; }
        public DbSet<SanPham> SanPhams { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostCategory> PostCategories { get; set; }



        public DbSet<CategoryProduct> CategoryProducts { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<Custom> Customs { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public DbSet<HoaDon> HoaDons { get; set; }

        public DbSet<ProductCategoryProduct> ProductCategoryProducts { get; set; }

        public DbSet<ProductPhoto> ProductPhotos { get; set; }

    }
}