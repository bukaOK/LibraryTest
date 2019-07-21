using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.Models.Clients
{
    public class ClientListModel
    {
        public int PagesCount { get; set; }
        public IList<ClientModel> Clients { get; set; }
    }
}
