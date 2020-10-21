using Microsoft.VisualStudio.TestTools.UnitTesting;
using PosiTicks.Shared;
using System.Linq;
using FluentAssertions;

namespace UnitTests
{
    [TestClass]
    public class ClassPeriodTests
    {
        [DataTestMethod]
        [DataRow("Jamal", "Jamal")]
        [DataRow(" Jamal ", "Jamal")]
        [DataRow(" Jamal\t", "Jamal")]
        [DataRow(" Jamal\n", "Jamal")]
        [DataRow("\tJamal\t", "Jamal")]
        [DataRow(" \n Jamal \t", "Jamal")]
        [DataRow("Jamal Anderson", "Jamal Anderson")]
        [DataRow(" Jamal Anderson ", "Jamal Anderson")]
        [DataRow("Jamal    Anderson", "Jamal Anderson")]
        [DataRow("\nJamal    Anderson\t", "Jamal Anderson")]
        public void AddStudent_TrimsWhitespace(string original, string expected)
        {
            var cp = new ClassPeriod();
            cp.AddStudent(original);
            cp.Students.First().Name.Should().Be(expected);
        }
    }
}
