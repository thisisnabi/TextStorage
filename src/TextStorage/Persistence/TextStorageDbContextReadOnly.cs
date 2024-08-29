using Microsoft.EntityFrameworkCore;
using System.Reflection.PortableExecutable;
using TextStorage.Models;

namespace TextStorage.Persistence
{
    public class TextStorageDbContextReadOnly(DbContextOptions<TextStorageDbContextReadOnly> dbContextOptions) 
        : DbContext(dbContextOptions)
    {


        public DbSet<Paste> Pastes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
