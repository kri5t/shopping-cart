using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shopping.Database.Models;

namespace Shopping.Database
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        { }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=shopping.db");
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Item>(ConfigureItem);
            modelBuilder.Entity<ShoppingCart>(ConfigureShoppingCart);
        }

        private void ConfigureItem(EntityTypeBuilder<Item> entity)
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(p => p.Uid)
                .IsRequired();
            
            entity.Property(p => p.CreatedDate)
                .IsRequired();
            
            entity.Property(p => p.Quantity)
                .IsRequired();
            
            entity.Property(p => p.Description)
                .IsRequired();
            
            entity.HasOne(e => e.ShoppingCart)
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.ShoppingCartId)
                .HasConstraintName("FK_Item_ShoppingCartId")
                .IsRequired();
        }

        private void ConfigureShoppingCart(EntityTypeBuilder<ShoppingCart> entity)
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(p => p.Uid)
                .IsRequired();
            
            entity.Property(p => p.CreatedDate)
                .IsRequired();
        }
    }
}