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

        public async Task<ClassPeriod> CreateAsync(string name)
        {
            if (classPeriods.Any(p => p.Name.Equals(name, System.StringComparison.OrdinalIgnoreCase)))
                throw new Exception("duplicate");

            var classPeriod = new ClassPeriod { Name = name };
            classPeriods.Add(classPeriod);
            return await Task.FromResult(classPeriod);
        }
    }
}