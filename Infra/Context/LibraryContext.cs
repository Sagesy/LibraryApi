using System.Reflection;
using LibraryApi.Domain.Library;
using LibraryApi.Domain.Models;
using LibraryApi.Infra.Mapping;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Infra.Context
{
    public class LibraryContext : DbContext
    {
        public LibraryContext()
        {
        }

        public LibraryContext(DbContextOptions<LibraryContext> options) : base(options) {}

        public DbSet<BookModels> Books { get; set; }
        public DbSet<AttachmentModels> Attachments { get; set; }
        public DbSet<OrderModels> Orders { get; set; }

        public DbSet<BookOrderModels> BookOrders { get; set; }


        public DbSet<LogEntry> Logs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost;Database=LibraryDB;User Id=sa;Password=Password_123#;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            builder.ApplyConfiguration(new BookMap());
            builder.ApplyConfiguration(new AttachmentMap());
            builder.ApplyConfiguration(new OrderMap());
            builder.ApplyConfiguration(new BookOrderMap());
            builder.ApplyConfiguration(new LogEntryMap());
        }
    }
}