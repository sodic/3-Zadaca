using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadatak_1
{
    class ToDoAccesDeniedException : Exception
    {

        public ToDoAccesDeniedException(string message) : base(message) { }

        public ToDoAccesDeniedException(string message, Exception innerException) : base(message, innerException) { }

    }
}
