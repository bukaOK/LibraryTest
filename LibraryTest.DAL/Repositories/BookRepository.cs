using LibraryTest.CodeData.Enums;
using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Filters;
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
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(AppDbContext dbContext) : base(dbContext)
        {
        }
        public async Task<Book> GetByIsbnAsync(string isbn)
        {
            return await Table.FirstOrDefaultAsync(x => x.ISBN == isbn);
        }
        public async Task<IList<Book>> GetByClientAsync(Guid clientId)
        {
            return await Table.AsNoTracking().Where(x => x.ClientId == clientId).ToListAsync();
        }
        public async Task<(int booksCount, IList<Book> books)> FilterAsync(BookListFilter filter)
        {
            var query = Table.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Name))
                // Без GIN
                query = query.Where(x => EF.Functions.ToTsVector("russian", x.Name + " " + x.Author).Matches(EF.Functions.ToTsQuery("russian", filter.Name)));

            if (filter.Year != null)
                query = query.Where(x => x.Year == filter.Year);

            if (filter.Status != null)
            {
                switch (filter.Status)
                {
                    case BookStatuses.Taked:
                        query = query.Where(x => x.ClientId != null);
                        break;
                    case BookStatuses.InPlace:
                        query = query.Where(x => x.ClientId == null);
                        break;
                }
            }

            if (filter.GenreIds != null && filter.GenreIds.Count > 0)
                query = query.Where(x => filter.GenreIds.Contains(x.GenreId));

            if(filter.Category != null)
            {
                switch (filter.Category)
                {
                    case BookCategories.Story:
                        query = DbContext.StoryBooks.AsNoTracking().Join(query, x => x.BookId, y => y.Id, (x, y) => y);
                        break;
                    case BookCategories.Classic:
                        query = DbContext.ClassicBooks.AsNoTracking().Join(query, x => x.BookId, y => y.Id, (x, y) => y);
                        break;
                }
            }
            var booksCount = await query.CountAsync();
            var result = await query.Include(x => x.Client).Skip(filter.StartIndex).Take(filter.Limit).ToListAsync();
            return (booksCount, result);
        }

        public async Task<BookCategories?> GetBookCategoryAsync(Guid bookId)
        {
            var isStory = await DbContext.StoryBooks.AnyAsync(x => x.BookId == bookId);
            if (isStory)
                return BookCategories.Story;
            var isClassic = await DbContext.ClassicBooks.AnyAsync(x => x.BookId == bookId);
            if (isClassic)
                return BookCategories.Classic;
            return null;
        }
    }
}
