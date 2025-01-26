using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Infra.Mapping
{
    public class BookOrderMap : IEntityTypeConfiguration<BookOrderModels>
    {
        public void Configure(EntityTypeBuilder<BookOrderModels> builder)
        {
            builder.ToTable("BookOrder");
            builder.HasKey(bc => new { bc.BookId, bc.OrderId });
            builder.HasOne(p => p.Book).WithMany(p => p.BookOrders).HasForeignKey(p => p.BookId);
            builder.HasOne(p => p.Order).WithMany(p => p.BookOrders).HasForeignKey(p => p.OrderId);

            builder.Property(p => p.BookOrderId).IsRequired().HasColumnName("txtBookOrderId");
            builder.Property(p => p.OrderId).IsRequired().HasColumnName("txtOrderId");
            builder.Property(p => p.BookId).IsRequired().HasColumnName("txtBookId");
            builder.Property(p => p.Status).IsRequired().HasColumnName("txtStatus").HasMaxLength(100);
        }
    }
}