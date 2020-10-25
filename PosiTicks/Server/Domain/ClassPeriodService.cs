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

        public async Task UpdateAsync(ClassPeriod classPeriod)
        {
            await Task.Delay(1);
            var match = classPeriods.Single(cp => cp.Id == classPeriod.Id);
            match = classPeriod;
        }

        private int GetNextId() => classPeriods.Any() ? classPeriods.Max(cp => cp.Id) + 1 : 1;
    }
}