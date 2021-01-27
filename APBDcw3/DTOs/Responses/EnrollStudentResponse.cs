using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.DTOs.Responses
{
    public class EnrollStudentResponse
    {
        [Required]
        public int IdEnrollment { get; set; }
        [Required]
        public int Semester { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public int IdStudy { get; set; }
    }
}
