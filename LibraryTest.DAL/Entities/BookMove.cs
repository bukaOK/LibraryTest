using LibraryTest.CodeData.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LibraryTest.DAL.Entities
{
    /// <summary>
    /// Движения книг (изменения статуса)
    /// </summary>
    public class BookMove : GuidEntity
    {
        [ForeignKey("Book")]
        public Guid BookId { get; set; }
        public Book Book { get; set; }
        /// <summary>
        /// Дата изменения статуса
        /// </summary>
        public DateTime Date { get; set; }
        /// <summary>
        /// Дата окончания действия статуса(напр. до какого числа нужно вернуть книгу)
        /// </summary>
        public DateTime? EndDate { get; set; }
        public BookStatuses NewStatus { get; set; }
    }
}
