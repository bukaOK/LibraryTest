using LibraryTest.BLL.Services;
using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.DAL.Entities;
using LibraryTest.DAL.Repositories.Interfaces;
using LibraryTest.Models.Genres;
using LibraryTest.Tests.Helpers;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.GenreServiceTests
{
    public abstract class GenreServiceTest
    {
        protected IGenreService genreService;
        protected Guid NonExistingId = Guid.NewGuid();
        protected Guid ExistingId = Guid.NewGuid();

        public GenreServiceTest()
        {
            var logger = MocksHelper.GetLogger<GenreService>();
            var mapper = MocksHelper.GetMapper();

            var repoMock = new Mock<IGenreRepository>();
            repoMock.Setup(repo => repo.GetAsync(null)).ReturnsAsync((Genre)null);
            repoMock.Setup(repo => repo.GetAsync(NonExistingId)).ReturnsAsync((Genre)null);
            repoMock.Setup(repo => repo.GetAsync(ExistingId)).ReturnsAsync(new Genre
            {
                Id = ExistingId,
                Name = "Mine"
            });

            genreService = new GenreService(logger, mapper, repoMock.Object);
        }
    }
}
