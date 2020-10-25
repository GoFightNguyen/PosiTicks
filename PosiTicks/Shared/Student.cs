using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PosiTicks.Shared
{
    [DebuggerDisplay("{Name}")]
    public class Student
    {
        [Required]
        public string Name { get; set; }
    }
}
