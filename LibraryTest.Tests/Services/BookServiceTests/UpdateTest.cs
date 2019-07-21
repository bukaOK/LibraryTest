using LibraryTest.BLL.Services.Interfaces;
using LibraryTest.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.BookServiceTests
{
    public class UpdateTest : BookServiceTest
    {

        [Fact]
        public async Task Update_IdNull()
        {
            var model = new BookModel
            {
                Id = null,
                Name = "Mine"
            };
            var result = await bookService.UpdateAsync(model);

            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task Update_IdNotNull_NonExistingId()
        {
            var model = new BookModel
            {
                Id = NonExistingId,
                Name = "Mine"
            };
            var result = await bookService.UpdateAsync(model);

            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task Update_IdNotNull_ExistingId()
        {
            var model = new BookModel
            {
                Id = ExistingId,
                Name = "Mine"
            };
            var result = await bookService.UpdateAsync(model);

            Assert.True(result.Succeeded);
        }
    }
}
