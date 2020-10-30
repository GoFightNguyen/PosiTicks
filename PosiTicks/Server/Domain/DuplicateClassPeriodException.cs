using System;

namespace PosiTicks.Server.Domain
{
    public class DuplicateClassPeriodException : Exception
    {
        public DuplicateClassPeriodException(string name)
            : base($"There is already a Class Period named {name}") { }
    }
}