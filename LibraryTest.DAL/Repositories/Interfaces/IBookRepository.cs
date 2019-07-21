using LibraryTest.CodeData.Enums;
using LibraryTest.DAL.Core;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.DAL.Repositories.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book> GetByIsbnAsync(string isbn);
        Task<IList<Book>> GetByClientAsync(Guid clientId);
        Task<BookCategories?> GetBookCategoryAsync(Guid bookId);
        Task<(int booksCount, IList<Book> books)> FilterAsync(BookListFilter filter);
    }
}
