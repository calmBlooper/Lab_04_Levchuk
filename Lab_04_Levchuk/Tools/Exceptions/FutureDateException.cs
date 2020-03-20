using System;

namespace Lab_04_Levchuk.Tools.Exceptions
{
    class FutureDateException : Exception
    {
        public FutureDateException() { }

        public FutureDateException(string message)
            : base(message) { }

        public FutureDateException(string message, Exception inner)
            : base(message, inner) { }
    }
}