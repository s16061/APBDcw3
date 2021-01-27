using APBDcw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Models
{
    public partial class Students
    {
        public Students()
        {
            RefreshToken = new HashSet<RefreshToken>();
        }
        //public int IdStudent { get; set; }
        public string IndexNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        //public string Studies { get; set; }
        public int IdEnrollment { get; set; }
        public string Password { get; set; }
        //public int Semester { get; set; }
        public virtual Enrollment IdEnrollmentNavigation { get; set; }
        public virtual ICollection<RefreshToken> RefreshToken { get; set; }
    }
}
