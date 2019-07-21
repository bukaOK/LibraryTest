using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.DAL.Entities
{
    public class Client : GuidEntity
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public DateTime RegisterDate { get; set; }
    }
}
