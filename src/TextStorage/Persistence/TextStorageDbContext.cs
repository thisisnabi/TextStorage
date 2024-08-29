using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using TextStorage.Models;

namespace TextStorage.Persistence
{
    public class TextStorageDbContext(DbContextOptions<TextStorageDbContext> dbContextOptions) 
        : DbContext(dbContextOptions)
    {


        public DbSet<Paste> Pastes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Paste>().HasKey(x => x.Id);
            modelBuilder.Entity<Paste>().HasIndex(x => x.ShortenCode)
                                        .IsUnique();

            modelBuilder.Entity<Paste>().Property(x => x.Content)
                                        .IsRequired()
                                        .IsUnicode(true)
                                        .HasMaxLength(4000);

            modelBuilder.Entity<Paste>().Property(x => x.ExpireOn)
                                         .IsRequired();
        }

    }
}
