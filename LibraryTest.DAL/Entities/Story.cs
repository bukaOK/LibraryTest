using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryTest.DAL.Entities
{
    public class Story : GuidEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("StoryBook")]
        public Guid StoryBookId { get; set; }
        public StoryBook StoryBook { get; set; }
    }
}
