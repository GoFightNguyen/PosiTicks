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
        [DataRow("Black", "Black")]
        [DataRow(" Black ", "Black")]
        [DataRow(" Black\t", "Black")]
        [DataRow(" Black\n", "Black")]
        [DataRow("\tBlack\t", "Black")]
        [DataRow(" \n Black \t", "Black")]
        [DataRow("Black Canary", "Black Canary")]
        [DataRow(" Black Canary ", "Black Canary")]
        [DataRow("Black    Canary", "Black Canary")]
        [DataRow("\nBlack    Canary\t", "Black Canary")]
        public void AddStudent_TrimsWhitespace(string original, string expected)
        {
            var cp = new ClassPeriod();
            cp.AddStudent(original);
            cp.Students.First().Name.Should().Be(expected);
        }

        [DataTestMethod]
        [DataRow("Black Canary", DisplayName = "exact")]
        [DataRow("black canary", DisplayName = "all lower")]
        [DataRow("BLACK CANARY", DisplayName = "all upper")]
        [DataRow("Black     Canary   ", DisplayName = "extra whitespace")]
        [DataRow("blACK canARY", DisplayName = "mixed casing")]
        public void AddStudent_DuplicateName_ThrowsException(string name)
        {
            var cp = new ClassPeriod();
            cp.AddStudent("Black Canary");
            Assert.ThrowsException<DuplicateStudentException>(() => cp.AddStudent(name));
            cp.Students.Count.Should().Be(1);
        }

        [TestMethod]
        public void AddStudent_UniqueName_Succeeds()
        {
            var cp = new ClassPeriod();
            cp.AddStudent("Black Canary");
            cp.AddStudent("Killer Frost");
            cp.Students.Select(s => s.Name).Should().BeEquivalentTo("Black Canary", "Killer Frost");
        }

        [DataTestMethod]
        [DataRow(0)]
        [DataRow(-1)]
        public void GiveTicketsToStudent_IgnoresNonPositiveNumbers(int tickets)
        {
            var cp = new ClassPeriod();
            cp.AddStudent("Black Canary");
            cp.GiveTicketsTo(cp.Students.Last(), tickets);
            cp.Students.Single().Tickets.Should().Be(0);
        }

        [DataTestMethod]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(15)]
        [DataRow(100)]
        public void GiveTicketsToStudent_StartValue0_TotalTicketsEqualToHowManyGiven(int tickets)
        {
            var cp = new ClassPeriod();
            cp.Students.Add(new Student { Name = "Killer Frost" });
            cp.GiveTicketsTo(cp.Students.Last(), tickets);
            cp.Students.Last().Tickets.Should().Be(tickets);
        }

        [TestMethod]
        public void GiveTicketsToStudent_StartValue3_AddsTickets()
        {
            var cp = new ClassPeriod();
            cp.Students.Add(new Student { Name = "Killer Frost", Tickets = 3 });
            cp.GiveTicketsTo(cp.Students.Last(), 4);
            cp.Students.Single().Tickets.Should().Be(7);
        }

        [TestMethod]
        public void GiveTicketsToStudent_OnlyAffectsSpecifiedStudent()
        {
            var cp = new ClassPeriod();
            cp.Students.Add(new Student { Name = "Black Canary", Tickets = 2 });
            cp.Students.Add(new Student { Name = "Killer Frost", Tickets = 3 });

            cp.GiveTicketsTo(cp.Students.Last(), 4);

            cp.Students.First().Tickets.Should().Be(2, "this student should not have received any tickets");
            cp.Students.Last().Tickets.Should().Be(7);
        }
    }
}
