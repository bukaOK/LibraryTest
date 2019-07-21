using LibraryTest.Models.Books;
using Manlike.BLL.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest.BLL.Services.Interfaces
{
    public interface IBookService : IDataService
    {
        Task<BookModel> GetForEditAsync(Guid id);
        Task<BookListModel> GetCatalogAsync(BookFilterModel filterModel);
        Task<DataServiceResult> AddAsync(BookModel model);
        Task<DataServiceResult> UpdateAsync(BookModel model);
        Task<DataServiceResult> SetClientAsync(BookMoveModel moveModel);
        Task<DataServiceResult> RemoveAsync(Guid bookId);
        Task<DataServiceResult> RemoveClientAsync(Guid bookId);
    }
}
