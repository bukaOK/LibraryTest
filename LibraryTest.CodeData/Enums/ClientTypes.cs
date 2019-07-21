using System;
using System.Collections.Generic;
using System.Text;

namespace LibraryTest.CodeData.Enums
{
    public enum ClientTypes
    {
        All = 0,
        /// <summary>
        /// Взяли книги
        /// </summary>
        Taked = 1,
        /// <summary>
        /// Взяли и просрочили
        /// </summary>
        Expired = 2
    }
}
