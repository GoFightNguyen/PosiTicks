using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using PosiTicks.Server.Domain;
using System.Threading.Tasks;
using PosiTicks.Shared;
using System;

namespace PosiTicks.AcceptanceTests
{
    // As a teacher, the Class Period is how I manage a group of students
    [TestClass]
    public class ClassPeriodSpecs
    {
        [TestClass]
        public class Rule_MustBeUniquelyNamed
        {
            private ClassPeriodService _sut;
            
            [TestInitialize]
            public async Task Setup()
            {
                _sut = new ClassPeriodService();
                await _sut.CreateAsync("My Favorite One");
            }

            [TestMethod]
            public async Task CanCreateAnotherClassPeriod_WithAUniqueName()
            {
                var expected = new List<ClassPeriod>
                {
                    new ClassPeriod {Name = "My Favorite One"},
                    new ClassPeriod {Name = "My Least Favorite One"}
                };

                await _sut.CreateAsync("My Least Favorite One");
                var actual = await _sut.GetAllAsync();
                actual.Should().BeEquivalentTo(expected);
            }

            [DataTestMethod]
            [DataRow("My Favorite One", DisplayName = "exact duplicate")]
            [DataRow("MY FAVORITE ONE", DisplayName = "duplicate, all upper")]
            [DataRow("my favorite one", DisplayName = "duplicate, all lower")]
            public async Task CannotCreateAnotherClassPeriod_WithTheSameName(string name)
            {
                var expected = new List<ClassPeriod>
                {
                    new ClassPeriod {Name = "My Favorite One"}
                };

                Func<Task<ClassPeriod>> mut = async () => await _sut.CreateAsync(name);
                await mut.Should().ThrowAsync<Exception>();

                var actual = await _sut.GetAllAsync();
                actual.Should().BeEquivalentTo(expected);
            }
        }
    }
}
