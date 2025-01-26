using LibraryApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApi.Infra.Mapping
{
    public class BookMap : IEntityTypeConfiguration<BookModels>
    {
        public void Configure(EntityTypeBuilder<BookModels> builder)
        {
            builder.ToTable("Book");
            builder.HasKey(p => p.BookId);
            builder.Property(p => p.BookId).IsRequired().HasColumnName("txtBookId");
            builder.Property(p => p.Title).IsRequired().HasColumnName("txtBookTitle").HasMaxLength(250);
            builder.Property(p => p.Publisher).IsRequired().HasColumnName("txtBookPublisher").HasMaxLength(250);
            builder.Property(p => p.Author).IsRequired().HasColumnName("txtBookAuthor").HasMaxLength(250);
            builder.Property(p => p.PublishedYear).IsRequired().HasColumnName("intPublishedYear");
            builder.Property(p => p.CreatedDate).IsRequired().HasColumnType("datetime").HasColumnName("dtmCreatedDate");
            builder.Property(p => p.UpdatedDate).HasColumnType("datetime").HasColumnName("dtmUpdatedDate");
            builder.Property(p => p.CoverId).HasColumnName("txtCoverId");
        }
    }
}