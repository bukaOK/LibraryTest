using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.BookServiceTests
{
    public class RemoveTest : BookServiceTest
    {
        [Fact]
        public async Task Remove_NonExistingId()
        {
            var result = await bookService.RemoveAsync(NonExistingId);
            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task Remove_ExistingId()
        {
            var result = await bookService.RemoveAsync(ExistingId);
            Assert.True(result.Succeeded);
        }
    }
}
