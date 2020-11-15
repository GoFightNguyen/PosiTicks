using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace PosiTicks.Shared
{
    [DebuggerDisplay("{Name}")]
    public class Student
    {
        [Required]
        public string Name { get; set; }

        [Range(0, int.MaxValue)]
        public int Tickets { get; set; }

        internal void AddTickets(int tickets)
        {
            if (tickets < 1)
                return;
            
            Tickets += tickets;
        }
    }
}
