using LibraryTest.BLL.Services;
using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Repositories;
using LibraryTest.DAL.Repositories.Interfaces;
using LibraryTest.Models.Books;
using LibraryTest.Tests.Helpers;
using Manlike.BLL.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.BookServiceTests
{
    public abstract class BookServiceTest
    {
        protected IBookService bookService;

        protected Guid NonExistingId = Guid.NewGuid();
        protected Guid ExistingId = Guid.NewGuid();

        protected Guid NonExistingClientId = Guid.NewGuid();
        protected Guid ExistingClientId = Guid.NewGuid();

        protected Guid NonExistingGenreId = Guid.NewGuid();
        protected Guid ExistingGenreId = Guid.NewGuid();

        public BookServiceTest()
        {
            var logger = MocksHelper.GetLogger<BookService>();
            var mapper = MocksHelper.GetMapper();

            var repoMock = new Mock<IBookRepository>();
            var dbTransactionMock = new Mock<IDbContextTransaction>();
            repoMock.Setup(repo => repo.GetAsync(null)).ReturnsAsync((Book)null);
            repoMock.Setup(repo => repo.GetAsync(NonExistingId)).ReturnsAsync((Book)null);
            repoMock.Setup(repo => repo.GetAsync(ExistingId)).ReturnsAsync(new Book
            {
                Id = ExistingId,
                Name = "Mine"
            });
            repoMock.Setup(repo => repo.BeginTransactionAsync()).ReturnsAsync(dbTransactionMock.Object);

            var clientRepoMock = new Mock<IClientRepository>();
            clientRepoMock.Setup(repo => repo.IsExistAsync(NonExistingClientId)).ReturnsAsync(false);
            clientRepoMock.Setup(repo => repo.IsExistAsync(ExistingClientId)).ReturnsAsync(true);
            clientRepoMock.Setup(repo => repo.GetAsync(ExistingClientId)).ReturnsAsync(new Client
            {
                Name = "Client",
                Phone = "12412"
            });
            clientRepoMock.Setup(repo => repo.IsExistAsync(NonExistingClientId)).ReturnsAsync(null);

            var classicBookRepoMock = new Mock<IClassicBookRepository>();
            var storyBookRepoMock = new Mock<IStoryBookRepository>();
            var bookMoveServiceMock = new Mock<IBookMoveService>();
            bookMoveServiceMock.Setup(repo => repo.AddAsync(It.IsAny<BookMoveModel>())).ReturnsAsync(DataServiceResult.Success);

            var genreRepoMock = new Mock<IGenreRepository>();
            genreRepoMock.Setup(repo => repo.IsExistAsync(ExistingGenreId)).ReturnsAsync(true);
            genreRepoMock.Setup(repo => repo.IsExistAsync(NonExistingGenreId)).ReturnsAsync(false);

            var configMock = new Mock<IConfiguration>();
            configMock.Setup(conf => conf["filter:rowsPerPage"]).Returns("10");

            bookService = new BookService(logger, mapper, repoMock.Object, clientRepoMock.Object, 
                classicBookRepoMock.Object, storyBookRepoMock.Object, bookMoveServiceMock.Object,
                genreRepoMock.Object, configMock.Object);
        }
    }
}
