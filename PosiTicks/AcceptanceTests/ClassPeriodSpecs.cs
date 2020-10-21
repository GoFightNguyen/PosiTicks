using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using PosiTicks.Server.Domain;
using System.Threading.Tasks;
using PosiTicks.Shared;

namespace PosiTicks.AcceptanceTests
{
    // As a teacher, the Class Period is how I manage a group of students
    [TestClass]
    public class ClassPeriodSpecs
    {
        private ClassPeriodService _sut;

        [TestInitialize]
        public void Setup()
        {
            _sut = new ClassPeriodService();
        }

        [TestMethod]
        public async Task CreateFirstClassPeriod()
        {
            await WhenICreateClassPeriod("Creation: 1");
            await ThenIHaveOneClassPeriod(new List<ClassPeriod>
            {
                new ClassPeriod { Id = 1,  Name = "Creation: 1"}
            });
        }

        [TestMethod]
        public async Task ReturnsTheCreatedClassPeriod()
        {
            var expected = new ClassPeriod { Id = 1, Name = "Creation: 1" };

            var actual = await WhenICreateClassPeriod("Creation: 1");

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task CreateAnotherClassPeriod()
        {
            await GivenIPreviouslyCreatedClassPeriod("Creation: 1");
            await WhenICreateClassPeriod("Creation: 2");
            await ThenIHaveTheFollowingClassPeriods(new List<ClassPeriod>
            {
                new ClassPeriod { Id = 1,  Name = "Creation: 1"},
                new ClassPeriod { Id = 2,  Name = "Creation: 2"}
            });
        }

        [TestMethod]
        public async Task GetClassPeriod()
        {
            var second = new ClassPeriod { Id = 2, Name = "Creation: 2" };

            await GivenIPreviouslyCreatedClassPeriod("Creation: 1");
            await GivenIPreviouslyCreatedClassPeriod("Creation: 2");
            var actual = await WhenIAskForTheSecondOne();
            ThenIGet(actual, second);
        }

        [TestMethod]
        public async Task AddStudents()
        {
            var student1 = new Student { Name = "Jamal Anderson" };
            var student2 = new Student { Name = "Tim Tebow" };
            await GivenIPreviouslyCreatedClassPeriod("Add Students: 1");
            await WhenIAddStudents(student1, student2);
            await ThenTheClassPeriodHasTwoStudents(new List<Student>
            {
                new Student{ Name = "Jamal Anderson"},
                new Student{ Name = "Tim Tebow"}
            });
        }

        [TestMethod]
        public async Task AddStudentToAnExistingClassPeriodWithStudents()
        {
            var student2 = new Student { Name = "Tim Tebow" };
            await GivenIPreviouslyCreatedClassPeriodWithStudent("Add Students: 1", "Jamal Anderson");
            await WhenIAddStudents(student2);
            await ThenTheClassPeriodHasTwoStudents(new List<Student>
            {
                new Student{ Name = "Jamal Anderson"},
                new Student{ Name = "Tim Tebow"}
            });
        }

        private async Task GivenIPreviouslyCreatedClassPeriod(string name)
                    => await _sut.CreateAsync(name);
        
        private async Task GivenIPreviouslyCreatedClassPeriodWithStudent(string classPeriodName, string student)
        {
            var cp = await _sut.CreateAsync(classPeriodName);
            await _sut.AddStudent(cp.Id, new Student{ Name = student });
        }

        private async Task<ClassPeriod> WhenICreateClassPeriod(string name)
            => await _sut.CreateAsync(name);

        private async Task<ClassPeriod> WhenIAskForTheSecondOne()
            => await _sut.GetAsync(2);

        private async Task WhenIAddStudents(params Student[] students)
        {
            foreach (var student in students)
                await _sut.AddStudent(1, student);
        }

        private async Task ThenIHaveOneClassPeriod(IEnumerable<ClassPeriod> expected)
            => await ThenIHaveTheFollowingClassPeriods(expected);

        private async Task ThenIHaveTheFollowingClassPeriods(IEnumerable<ClassPeriod> expected)
        {
            var actual = await _sut.GetAllAsync();
            actual.Should().BeEquivalentTo(expected);
        }

        private void ThenIGet(ClassPeriod actual, ClassPeriod expected)
            => actual.Should().BeEquivalentTo(expected);

        private async Task ThenTheClassPeriodHasTwoStudents(IEnumerable<Student> expected)
        {
            var cp = await _sut.GetAsync(1);
            cp.Students.Should().BeEquivalentTo(expected);
        }
    }
}
