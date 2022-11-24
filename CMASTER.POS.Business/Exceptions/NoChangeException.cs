using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMASTER.POS.Business.Exceptions
{
    /// <summary>
    /// Exception to use when the total price is equal to the received cash. 
    /// </summary>
    public class NoChangeException : Exception
    {
        public NoChangeException()
        {

        }

        public NoChangeException(string message) : base(message)
        {

        }

        public NoChangeException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
