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
        [TestMethod]
        public async Task CanCreateAClassPeriod()
        {
            var expected = new List<ClassPeriod>
            {
                new ClassPeriod { Id = 1,  Name = "Creation: 1"}
            };

            var _sut = new ClassPeriodService();
            await _sut.CreateAsync("Creation: 1");

            var actual = await _sut.GetAllAsync();
            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task ReturnsTheCreatedClassPeriod()
        {
            var expected = new ClassPeriod { Id = 1, Name = "Creation: 1" };

            var _sut = new ClassPeriodService();
            var actual = await _sut.CreateAsync("Creation: 1");

            actual.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public async Task CanCreateAnotherClassPeriod()
        {
            var expected = new List<ClassPeriod>
            {
                new ClassPeriod { Id = 1,  Name = "Creation: 1"},
                new ClassPeriod { Id = 2,  Name = "Creation: 2"}
            };

            var _sut = new ClassPeriodService();
            await _sut.CreateAsync("Creation: 1");
            await _sut.CreateAsync("Creation: 2");

            var actual = await _sut.GetAllAsync();
            actual.Should().BeEquivalentTo(expected);
        }
    }
}
