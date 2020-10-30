using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using PosiTicks.Server.Domain;
using System.Threading.Tasks;
using PosiTicks.Shared;
using System.Linq;
using System;

namespace PosiTicks.AcceptanceTests
{
    // As a teacher, the Class Period is how I manage a group of students

    [TestClass]
    public class CreatingAClassPeriod : Steps
    {
        [TestInitialize]
        public void Setup()
        {
            _sut = new ClassPeriodService();
        }

        [TestMethod]
        public async Task CreateFirstClassPeriod()
        {
            await WhenICreateClassPeriod("Creation: 1");
            await ThenIHaveTheFollowingClassPeriods(new List<ClassPeriod>
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
        public async Task GetSpecifiedClassPeriod()
        {
            var second = new ClassPeriod { Id = 2, Name = "Creation: 2" };

            await GivenIPreviouslyCreatedClassPeriod("Creation: 1");
            await GivenIPreviouslyCreatedClassPeriod("Creation: 2");
            var actual = await WhenIAskForTheSecondOne();
            ThenIGet(actual, second);
        }
    }

    [TestClass]
    public class Rule_ClassPeriodNameIsUnique : Steps
    {
        [TestInitialize]
        public async Task Background()
        {
            _sut = new ClassPeriodService();
            await GivenIPreviouslyCreatedClassPeriod("Creation: Justice League of America");
        }

        [TestMethod]
        public async Task CreateAClassPeriodWithADifferentName()
        {
            await WhenICreateClassPeriod("Creation: Lord Havok and The Extremists");
            await ThenIHaveTheFollowingClassPeriods(new List<ClassPeriod>
                {
                    new ClassPeriod { Id = 1,  Name = "Creation: Justice League of America"},
                    new ClassPeriod { Id = 2,  Name = "Creation: Lord Havok and The Extremists"}
                });
        }

        [DataTestMethod]
        [DataRow("Creation: Justice League of America", DisplayName = "exact match")]
        [DataRow("CREATION: JUSTICE LEAGUE OF AMERICA", DisplayName = "all upper")]
        [DataRow("creation: justice league of america", DisplayName = "all lower")]
        [DataRow("Creation: justice League Of AmerIca", DisplayName = "mixed")]
        public async Task CannotCreateAClassPeriodWithTheSameName(string name)
        {
            Func<Task> mut = async () => await WhenICreateClassPeriod(name);
            await mut.Should().ThrowAsync<DuplicateClassPeriodException>();
            await ThenIHaveTheFollowingClassPeriods(new List<ClassPeriod>
                {
                    new ClassPeriod { Id = 1,  Name = "Creation: Justice League of America"}
                });
        }
    }

    [TestClass]
    public class AddingStudents : Steps
    {
        [TestInitialize]
        public void Setup()
        {
            _sut = new ClassPeriodService();
        }

        [TestMethod]
        public async Task AddStudent()
        {
            var orig = await GivenIPreviouslyCreatedClassPeriod("Add Students: Justice League of America");

            var request = new ClassPeriod
            {
                Id = orig.Id,
                Name = orig.Name
            };

            await WhenIAddStudents(request, "Batman");
            ThenTheseStudentsAreInTheClassPeriod(orig, "Batman");
        }

        [TestMethod]
        public async Task AddStudentsToClassAlreadyContainingStudents()
        {
            var orig = await GivenAClassPeriodWithStudents("Add Students: Justice League of America", "Batman", "Killer Frost");

            var request = new ClassPeriod
            {
                Id = orig.Id,
                Name = orig.Name,
                Students = new List<Student>
                {
                    new Student { Name = "Batman"},
                    new Student { Name = "Killer Frost"}
                }
            };

            await WhenIAddStudents(request, "Black Canary", "Vixen", "The Atom", "The Ray", "Lobo");
            ThenTheseStudentsAreInTheClassPeriod(orig,
                "Batman",
                "Killer Frost",
                "Black Canary",
                "Vixen",
                "The Atom",
                "The Ray",
                "Lobo"
            );
        }
    }

    public abstract class Steps
    {
        protected ClassPeriodService _sut;

        /**Given**/
        protected async Task<ClassPeriod> GivenIPreviouslyCreatedClassPeriod(string name)
            => await _sut.CreateAsync(name);

        protected async Task<ClassPeriod> GivenAClassPeriodWithStudents(string name, params string[] students)
        {
            var cp = await _sut.CreateAsync(name);
            foreach (var student in students)
                cp.AddStudent(student);

            await _sut.UpdateAsync(cp);
            return cp;
        }


        /**When**/
        protected async Task WhenIAddStudents(ClassPeriod request, params string[] students)
        {
            foreach (var student in students)
                request.AddStudent(student);

            await _sut.UpdateAsync(request);
        }

        protected async Task<ClassPeriod> WhenIAskForTheSecondOne()
            => await _sut.GetAsync(2);

        protected async Task<ClassPeriod> WhenICreateClassPeriod(string name)
            => await _sut.CreateAsync(name);


        /**Then**/
        protected void ThenIGet(ClassPeriod actual, ClassPeriod expected)
            => actual.Should().BeEquivalentTo(expected);

        protected async Task ThenIHaveTheFollowingClassPeriods(IEnumerable<ClassPeriod> expected)
        {
            var actual = await _sut.GetAllAsync();
            actual.Should().BeEquivalentTo(expected);
        }

        protected static void ThenTheseStudentsAreInTheClassPeriod(ClassPeriod classPeriod, params string[] students)
        {
            var expected = students.Select(s => new Student { Name = s });
            classPeriod.Students.Should().BeEquivalentTo(expected);
        }
    }
}
