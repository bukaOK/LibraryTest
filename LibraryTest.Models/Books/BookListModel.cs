using LibraryTest.Models.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.Models.Books
{
    public class BookListModel
    {
        public int PagesCount { get; set; }
        public IList<CatalogBookModel> Books { get; set; }
    }
}
