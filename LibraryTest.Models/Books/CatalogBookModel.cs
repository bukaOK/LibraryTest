using LibraryTest.Models.Clients;
using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.Models.Books
{
    public class CatalogBookModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int? Year { get; set; }
        public ClientModel Client { get; set; }
    }
}
