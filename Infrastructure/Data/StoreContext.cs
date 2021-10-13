using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType> ProductTypes { get; set; }
        public DbSet<ProductBrand> ProductBrands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DeliveryMethod> DeliveryMethods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                foreach (IMutableEntityType type in modelBuilder.Model.GetEntityTypes())
                {
                    IEnumerable<PropertyInfo> properties = type.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));
                    IEnumerable<PropertyInfo> dateProperties = type.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset));

                    foreach (PropertyInfo pi in properties)
                        modelBuilder.Entity(type.Name).Property(pi.Name).HasConversion<double>();

                    foreach (PropertyInfo pi in dateProperties)
                        modelBuilder.Entity(type.Name).Property(pi.Name).HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
    }
}