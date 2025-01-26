using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Infra.Mapping
{
    public class OrderMap : IEntityTypeConfiguration<OrderModels>
    {
        public void Configure(EntityTypeBuilder<OrderModels> builder)
        {
            builder.ToTable("Order");
            builder.HasKey(p => p.OrderId);

            builder.Property(p => p.OrderId).IsRequired().HasColumnName("txtOrderId");
            builder.Property(p => p.OrderName).IsRequired().HasColumnName("txtOrderName").HasMaxLength(250);
            builder.Property(p => p.Duration).IsRequired().HasColumnName("intDuration");
            builder.Property(p => p.DueDate).IsRequired().HasColumnType("date").HasColumnName("dtmDueDate");
            builder.Property(p => p.CreatedDate).IsRequired().HasColumnType("datetime").HasColumnName("dtmCreatedDate");
            builder.Property(p => p.UpdatedDate).HasColumnType("datetime").HasColumnName("dtmUpdatedDate");
        }
    }
}