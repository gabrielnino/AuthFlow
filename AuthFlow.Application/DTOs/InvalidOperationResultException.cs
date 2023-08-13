using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Application.DTOs
{
    public class InvalidOperationResultException : Exception
    {
        public InvalidOperationResultException(string message)
            : base(message)
        {
        }
    }
}
