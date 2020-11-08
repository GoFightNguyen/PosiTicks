using System;

namespace PosiTicks.Shared
{
    public class DuplicateStudentException : Exception
    {
        public DuplicateStudentException(string name)
            : base($"There is already a Student named {name}") { }
    }
}