using LibraryTest.Models.Books;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.BookServiceTests
{
    public class SetClientTest : BookServiceTest
    {
        [Fact]
        public async Task SetClient_NonExistingId_NonExistingClientId()
        {
            var model = new BookMoveModel
            {
                BookId = NonExistingClientId,
                ClientId = NonExistingClientId,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var result = await bookService.SetClientAsync(model);
            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task SetClient_NonExistingId_ExistingClientId()
        {
            var model = new BookMoveModel
            {
                BookId = NonExistingClientId,
                ClientId = NonExistingClientId,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var result = await bookService.SetClientAsync(model);
            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task SetClient_ExistingId_NonExistingClientId()
        {
            var model = new BookMoveModel
            {
                BookId = NonExistingClientId,
                ClientId = NonExistingClientId,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var result = await bookService.SetClientAsync(model);
            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task SetClient_ExistingId_ExistingClientId()
        {
            var model = new BookMoveModel
            {
                BookId = ExistingId,
                ClientId = ExistingClientId,
                EndDate = DateTime.UtcNow.AddDays(1)
            };
            var result = await bookService.SetClientAsync(model);
            
            Assert.True(result.Succeeded);
        }
    }
}
