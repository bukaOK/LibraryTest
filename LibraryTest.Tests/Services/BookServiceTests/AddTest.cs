using LibraryTest.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.BookServiceTests
{
    public class AddTest : BookServiceTest
    {
        [Fact]
        public async Task Add_ExistingGenreId()
        {
            // Несуществующий GenreId не протестирован
            var model = new BookModel
            {
                Id = null,
                Name = "Mine",
                ISBN = "123124",
                GenreId = ExistingGenreId,
                PagesCount = 120,
                Year = 2010,
                Category = CodeData.Enums.BookCategories.Classic,
                VolCount = 12,
                Author = "asdawd"
            };
            var result = await bookService.AddAsync(model);

            Assert.True(result.Succeeded);
            Assert.IsType<Guid>(result.ResultData);
        }
        [Fact]
        public async Task Add_NonExistingGenreId()
        {
            // Несуществующий GenreId не протестирован
            var model = new BookModel
            {
                Id = null,
                Name = "Mine",
                ISBN = "123124",
                GenreId = NonExistingGenreId,
                PagesCount = 120,
                Category = CodeData.Enums.BookCategories.Classic,
                VolCount = 12,
                Year = 2010,
                Author = "adwasd"
            };
            var result = await bookService.AddAsync(model);

            Assert.False(result.Succeeded);
        }
    }
}
