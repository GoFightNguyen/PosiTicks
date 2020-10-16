using System.ComponentModel.DataAnnotations;

namespace PosiTicks.Shared
{
    public class ClassPeriod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
