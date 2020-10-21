using System.ComponentModel.DataAnnotations;

namespace PosiTicks.Shared
{
    public class Student
    {
        [Required]
        public string Name { get; set; }
    }
}
