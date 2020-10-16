using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using FluentAssertions;
using PosiTicks.Server.Domain;
using System.Threading.Tasks;
using PosiTicks.Shared;
using System;

namespace PosiTicks.AcceptanceTests
{
    // As a teacher, the Class Period is how I manager a group of students
    [TestClass]
    public class ClassPeriodSpecs
    {
        [TestClass]
        public class Rule_MustBeUniquelyNamed
        {
            [TestMethod]
            public async Task CanCreateAnotherClassPeriod_WithAUniqueName()
            {
                var svc = new ClassPeriodService();
                await svc.CreateAsync("My Favorite One");

                var expected = new List<ClassPeriod>
                {
                    new ClassPeriod {Name = "My Favorite One"},
                    new ClassPeriod {Name = "My Least Favorite One"}
                };

                await svc.CreateAsync("My Least Favorite One");
                var actual = await svc.GetAllAsync();
                actual.Should().BeEquivalentTo(expected);
            }

            [DataTestMethod]
            [DataRow("My Favorite One", DisplayName = "exact duplicate")]
            [DataRow("MY FAVORITE ONE", DisplayName = "duplicate, all upper")]
            [DataRow("my favorite one", DisplayName = "duplicate, all lower")]
            public async Task CannotCreateAnotherClassPeriod_WithTheSameName(string name)
            {
                var svc = new ClassPeriodService();
                await svc.CreateAsync("My Favorite One");

                var expected = new List<ClassPeriod>
                {
                    new ClassPeriod {Name = "My Favorite One"}
                };

                Func<Task<ClassPeriod>> mut = async () => await svc.CreateAsync(name);
                await mut.Should().ThrowAsync<Exception>();

                var actual = await svc.GetAllAsync();
                actual.Should().BeEquivalentTo(expected);
            }
        }
    }
}
