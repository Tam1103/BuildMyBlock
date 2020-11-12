using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechWebsite.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public virtual DbSet<Account> Accounts { get; set; }

        public virtual DbSet<Role> Roles { get; set; }

        public virtual DbSet<RoleAccount> RoleAccounts { get; set; }

        public virtual DbSet<Category> Categories { get; set; }

        public virtual DbSet<SlideShow> SlideShows { get; set; }

        public virtual DbSet<Product> Products  { get; set; }

        public virtual DbSet<Photo> Photos  { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            //account
            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.Email)
                 .HasMaxLength(250)
                 .IsUnicode(false);

                entity.Property(e => e.FullName)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.Password)
                  .HasMaxLength(250)
                  .IsUnicode(false);

                entity.Property(e => e.Username)
                  .HasMaxLength(250)
                  .IsUnicode(false);
            });

            //role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);

            });

            //roleAccount
            modelBuilder.Entity<RoleAccount>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.AccountId });

                entity.HasOne(d => d.Account)
                .WithMany(p => p.RoleAccounts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleAccount_Account");


                entity.HasOne(d => d.Role)
                .WithMany(p => p.RoleAccounts)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleAccount_Role");
            });

            //category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);
                
                entity.HasOne(d => d.Parent)
                .WithMany(p => p.InverseParents)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Category_Category");
            });

            //slideshow
            modelBuilder.Entity<SlideShow>(entity =>
            {
                entity.Property(e => e.Name)
                .HasMaxLength(250)
                .IsUnicode(false);

            });

            //product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");
            });

            //photo
            modelBuilder.Entity<Photo>(entity =>
            {
                entity.HasOne(d => d.Product)
                .WithMany(p => p.Photos)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Photo_Product");
            });
        }
    }
}
