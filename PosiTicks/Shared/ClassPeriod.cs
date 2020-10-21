using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace PosiTicks.Shared
{
    public class ClassPeriod
    {
        private static readonly Regex REGEX = new Regex(@"\s+");

        private IList<Student> _students = new List<Student>();

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public IEnumerable<Student> Students
            => _students.AsEnumerable();

        public void AddStudent(string name)
            => _students.Add(new Student { Name = RemoveExcessWhitespace(name) });

        private static string RemoveExcessWhitespace(string value)
            => REGEX.Replace(value.Trim(), @" ");
    }
}
