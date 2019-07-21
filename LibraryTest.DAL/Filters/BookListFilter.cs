using LibraryTest.CodeData.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.DAL.Filters
{
    public class BookListFilter
    {
        public int Page { get; set; }
        public IList<Guid> GenreIds { get; set; }
        public string Name { get; set; }
        public BookCategories? Category { get; set; }
        public BookStatuses? Status { get; set; }
        public int? Year { get; set; }
        public int StartIndex { get; set; }
        public int Limit { get; set; }
    }
}
