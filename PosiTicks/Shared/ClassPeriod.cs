using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace PosiTicks.Shared
{
    public class ClassPeriod
    {
        private static readonly Regex REGEX = new Regex(@"\s+");

        [Key]
        public int Id { get; set; }

        // TODO: remove excess whitespace
        [Required]
        public string Name { get; set; }

        public IList<Student> Students { get; set; } = new List<Student>();

        public void AddStudent(string name)
        {
            var cleanedUpName = RemoveExcessWhitespace(name);
            if (Students.Any(s => s.Name.Equals(cleanedUpName, StringComparison.OrdinalIgnoreCase)))
                throw new DuplicateStudentException(cleanedUpName);

            Students.Add(new Student { Name = cleanedUpName });
        }

        private static string RemoveExcessWhitespace(string value)
            => REGEX.Replace(value.Trim(), @" ");

        public void GiveTicketsTo(Student student, int tickets)
        {
            student.AddTickets(tickets);
        }
    }
}