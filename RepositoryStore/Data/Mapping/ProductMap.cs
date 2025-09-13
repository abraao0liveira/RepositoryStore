using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepositoryStore.Models;

namespace RepositoryStore.Data.Mapping
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("product");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.Title)
                .IsRequired(true)
                .HasMaxLength(160)
                .HasColumnType("nvarchar");
        }
    }
}
