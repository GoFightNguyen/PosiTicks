using System.Collections.Generic;
using System.Threading.Tasks;
using PosiTicks.Shared;
using System.Linq;
using System;

namespace PosiTicks.Server.Domain
{
    public class ClassPeriodService
    {
        private readonly List<ClassPeriod> classPeriods = new List<ClassPeriod>();

        public async Task<IEnumerable<ClassPeriod>> GetAllAsync()
        {
            return await Task.FromResult(classPeriods);
        }

        public async Task<ClassPeriod> GetAsync(int id)
        {
            var match = classPeriods.Single(cp => cp.Id == id);
            return await Task.FromResult(match);
        }

        public async Task<ClassPeriod> CreateAsync(string name)
        {
            if (classPeriods.Any(cp => cp.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateClassPeriodException(name);

            var classPeriod = new ClassPeriod { Id = GetNextId(), Name = name };
            classPeriods.Add(classPeriod);
            return await Task.FromResult(classPeriod);
        }

        public async Task UpdateAsync(ClassPeriod classPeriod)
        {
            await Task.Delay(1);
            var match = classPeriods.Single(cp => cp.Id == classPeriod.Id);
            foreach(var student in classPeriod.Students)
            {
                if (!match.Students.Any(s => s.Name == student.Name))
                    match.AddStudent(student.Name);

                var matchStudent = match.Students.Single(s => s.Name == student.Name);
                matchStudent.Tickets = student.Tickets;
            }
        }

        private int GetNextId() => classPeriods.Any() ? classPeriods.Max(cp => cp.Id) + 1 : 1;
    }
}