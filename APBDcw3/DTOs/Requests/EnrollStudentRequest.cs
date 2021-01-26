using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.DTOs.Requests
{
    public class EnrollStudentRequest
    {
        [Required(ErrorMessage = "Musisz podać indeks")]
        public string IndexNumber { get; set; }
        [Required(ErrorMessage = "Musisz podać imię")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Musisz podać Nazwisko")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Musisz podać date urodzenia")]
        public DateTime Birthdate { get; set; }
        [Required(ErrorMessage = "Musisz podać kierunek")]
        public string Studies { get; set; }
    }
}
