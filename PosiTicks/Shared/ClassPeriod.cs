using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace PosiTicks.Shared
{
    public class ClassPeriod
    {
        private static readonly Regex REGEX = new Regex(@"\s+");

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IList<Student> Students { get; set; } = new List<Student>();

        public void AddStudent(string name)
            => Students.Add(new Student { Name = RemoveExcessWhitespace(name) });

        private static string RemoveExcessWhitespace(string value)
            => REGEX.Replace(value.Trim(), @" ");
    }
}
