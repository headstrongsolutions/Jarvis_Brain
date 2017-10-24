using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jarvis_Brain.Code
{
    public enum StateEnum
    {
        Received, 
        Sent, 
        Error
    }

    public enum ErrorType
    {
        ConnectionFailure,
        ConnectionInterupted,
        InconsistentDataShape,
        Unknown
    }
}
