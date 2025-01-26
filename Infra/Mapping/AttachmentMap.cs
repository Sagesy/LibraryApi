using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Infra.Mapping
{
    public class AttachmentMap : IEntityTypeConfiguration<AttachmentModels>
    {
        public void Configure(EntityTypeBuilder<AttachmentModels> builder)
        {
            builder.ToTable("Attachment");
            builder.HasKey(p => p.AttachmentId);
            builder.Property(p => p.AttachmentId).IsRequired().HasColumnName("txtAttachmentId");
            builder.Property(p => p.TransactionType).IsRequired().HasColumnName("txtTransactionType").HasMaxLength(100);
            builder.Property(p => p.Url).IsRequired().HasColumnName("txtAttachmentUrl").HasMaxLength(1000);
            builder.Property(p => p.ContentType).IsRequired().HasColumnName("txtContentType").HasMaxLength(100);
            builder.Property(p => p.NameFile).IsRequired().HasColumnName("txtNameFile").HasMaxLength(500);
            builder.Property(p => p.OriginalNameFile).IsRequired().HasColumnName("txtOriginalNameFile").HasMaxLength(500);
            builder.Property(p => p.CreatedDate).IsRequired().HasColumnType("date").HasColumnName("dtmCreatedDate");
            builder.Property(p => p.UpdatedDate).HasColumnType("date").HasColumnName("dtmUpdatedDate");
        }
    }
}