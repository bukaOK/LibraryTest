using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryTest.DAL.Entities
{
    public class StoryBook : BookCategoryEntity
    {
        public List<Story> Stories { get; set; }
    }
}
