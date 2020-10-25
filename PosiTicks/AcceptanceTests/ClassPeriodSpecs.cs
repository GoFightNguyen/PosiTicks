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

        private async Task<ClassPeriod> GivenIPreviouslyCreatedClassPeriod(string name) 
            => await _sut.CreateAsync(name);

        private async Task<ClassPeriod> WhenICreateClassPeriod(string name)
            => await _sut.CreateAsync(name);

        private async Task<ClassPeriod> WhenIAskForTheSecondOne()
            => await _sut.GetAsync(2);

        private async Task ThenIHaveOneClassPeriod(IEnumerable<ClassPeriod> expected)
            => await ThenIHaveTheFollowingClassPeriods(expected);

        private async Task ThenIHaveTheFollowingClassPeriods(IEnumerable<ClassPeriod> expected)
        {
            var actual = await _sut.GetAllAsync();
            actual.Should().BeEquivalentTo(expected);
        }

        private void ThenIGet(ClassPeriod actual, ClassPeriod expected)
            => actual.Should().BeEquivalentTo(expected);
    }
}
