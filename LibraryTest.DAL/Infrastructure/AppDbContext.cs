using LibraryTest.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.DAL.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<ClassicBook> ClassicBooks { get; set; }
        public DbSet<StoryBook> StoryBooks { get; set; }
        public DbSet<Story> Stories { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<BookMove> BookMoves { get; set; }
    }
}
