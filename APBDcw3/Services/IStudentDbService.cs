using APBDcw3.DTOs.Requests;
using APBDcw3.DTOs.Responses;
using APBDcw3.Models;
using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.Services
{
    public interface IStudentDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        public Enrollment StudentEnrollment(string indexNumber, string firstName, string lastName, DateTime birthDate, string name);
        public Enrollment GetEnrollment(int idStudy, int semester);
        public Enrollment GetEnrollment(int idStudy);
        public Enrollment Promote(int idStudy, int semester);
        public Study GetStudyName(string name);
        //PromotionStudentResponse PromoteStudent(PromotionStudentRequest request);

        Students GetStudent(string index);


    }
}
