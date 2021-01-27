using Cw3.Models;
using System;
using System.Collections.Generic;

#nullable disable

namespace APBDcw3.Models
{
    public partial class Enrollment
    {
        public Enrollment()
        {
            Students = new HashSet<Students>();
        }

        public int IdEnrollment { get; set; }
        public int Semester { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }

        public virtual Study IdStudyNavigation { get; set; }
        public virtual ICollection<Students> Students { get; set; }
    }
}
