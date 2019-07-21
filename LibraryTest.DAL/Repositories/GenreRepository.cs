using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Infrastructure;
using LibraryTest.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public override async Task<IList<Genre>> GetAllAsync()
        {
            return await Table.AsNoTracking().OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<bool> IsExistAsync(Guid genreId)
        {
            return await Table.AnyAsync(x => x.Id == genreId);
        }
    }
}
