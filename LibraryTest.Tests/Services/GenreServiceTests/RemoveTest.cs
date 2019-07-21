using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryTest.Tests.Services.GenreServiceTests
{
    public class RemoveTest : GenreServiceTest
    {
        [Fact]
        public async Task Remove_NonExistingId()
        {
            var result = await genreService.RemoveAsync(NonExistingId);
            Assert.False(result.Succeeded);
        }
        [Fact]
        public async Task Remove_ExistingId()
        {
            var result = await genreService.RemoveAsync(ExistingId);
            Assert.True(result.Succeeded);
        }
    }
}
