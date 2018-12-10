using System;

namespace TimeGap.Exceptions
{
    public class InvalidDuodecimDateException : Exception
    {
        public InvalidDuodecimDateException(string message) : base(message)
        {
        }
    }
}
