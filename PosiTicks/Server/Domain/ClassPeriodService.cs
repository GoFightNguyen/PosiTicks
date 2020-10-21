using System.Collections.Generic;
using System.Threading.Tasks;
using PosiTicks.Shared;
using System.Linq;

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
            var classPeriod = new ClassPeriod { Id = GetNextId(), Name = name };
            classPeriods.Add(classPeriod);
            return await Task.FromResult(classPeriod);
        }

        public async Task AddStudent(int classPeriodId, Student student)
        {
            var classPeriod = await GetAsync(classPeriodId);
            classPeriod.AddStudent(student.Name);
        }

        private int GetNextId() => classPeriods.Any() ? classPeriods.Max(cp => cp.Id) + 1 : 1;
    }
}