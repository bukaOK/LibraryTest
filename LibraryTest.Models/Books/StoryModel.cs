using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.Models.Books
{
    public class StoryModel
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
