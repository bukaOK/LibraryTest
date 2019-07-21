using LibraryTest.CodeData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.Models.Books
{
    public class BookFilterModel
    {
        public int Page { get; set; }
        public IList<Guid> Genres { get; set; }
        public string Name { get; set; }
        public BookCategories? Category { get; set; }
        public BookStatuses? Status { get; set; }
        public int? Year { get; set; }
    }
}
