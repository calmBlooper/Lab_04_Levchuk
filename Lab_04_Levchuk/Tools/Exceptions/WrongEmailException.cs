using System;

namespace Lab_04_Levchuk.Tools.Exceptions
{
    class WrongEmailException : Exception
    {
        public WrongEmailException() { }

        public WrongEmailException(string message)
            : base(message) { }

        public WrongEmailException(string message, Exception inner)
            : base(message, inner) { }
    }
}