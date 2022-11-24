using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMASTER.POS.Business.Exceptions
{
    /// <summary>
    /// Exception to use when the total price is grater than the received cash 
    /// </summary>
    public class NotEnoughtCashException : Exception
    {
        public NotEnoughtCashException()
        {

        }

        public NotEnoughtCashException(string message): base(message)
        {

        }

        public NotEnoughtCashException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
