using System.ComponentModel.DataAnnotations;

namespace PosiTicks.Shared
{
    public class ClassPeriod
    {
        [Required]
        public string Name { get; set; }
    }
}
