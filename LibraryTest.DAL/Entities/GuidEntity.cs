using System;
using System.ComponentModel.DataAnnotations;

namespace LibraryTest.DAL.Entities
{
    public abstract class GuidEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
