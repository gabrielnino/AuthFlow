using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthFlow.Application.DTOs
{
    public class InvalidOperationResultException_REVIEWED : Exception
    {
        public InvalidOperationResultException_REVIEWED(string message)
            : base(message)
        {
        }
    }
}
