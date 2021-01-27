using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBDcw3.DTOs.Requests;
using APBDcw3.DTOs.Responses;
using APBDcw3.Models;
using APBDcw3.Services;
using Cw3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace APBDcw3.Controllers
{
    
    [ApiController]
    [Route("api/enrollments")]
    public class EnrollmentsController : ControllerBase
    {
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s16061;Integrated Security=True;MultipleActiveResultSets=true";
         IStudentDbService _service;
        s16061Context _dbcont;

        public EnrollmentsController(IStudentDbService service, s16061Context dbcont)
        {
            _dbcont = dbcont;
            _service = service;
        }

        [HttpPost]
       // [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {

            var enrollment = _service.StudentEnrollment(
                request.IndexNumber, request.FirstName, request.LastName, request.Birthdate, request.Studies
                );

            return CreatedAtAction(nameof(GetEnrollment),
                new { IdEnrollment = enrollment.IdEnrollment },
                new EnrollStudentResponse
                {
                    IdEnrollment = enrollment.IdEnrollment,
                    IdStudy = enrollment.IdStudy,
                    Semester = enrollment.Semester,
                    StartDate = enrollment.StartDate
                });
            
        }
        public IActionResult GetEnrollment(int idEnrollment)
        {
            var enrollment = _service.GetEnrollment(idEnrollment);
            if (enrollment != null)
                return Ok(new EnrollStudentResponse
                {
                    IdEnrollment = enrollment.IdEnrollment,
                    IdStudy = enrollment.IdStudy,
                    Semester = enrollment.Semester,
                    StartDate = enrollment.StartDate
                });
            else
                return NotFound("Nie znaleziono");
        }


         
        [HttpPost("promotions")]
        //[Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromotionStudentRequest request)
        {

            var st = _service.GetStudyName(request.Studies);
            if(st == null)
            {
                return NotFound("Nie znaleziono");
            }
            var enrollment = _service.GetEnrollment(st.IdStudy, request.Semester);
            var setEnrollment = _service.Promote(st.IdStudy, request.Semester);
            return CreatedAtAction(nameof(GetEnrollment),
                new { idEnrollment = setEnrollment.IdEnrollment },
                new EnrollStudentResponse
                {
                    IdEnrollment = setEnrollment.IdEnrollment,
                    IdStudy = setEnrollment.IdStudy,
                    Semester = setEnrollment.Semester,
                    StartDate = setEnrollment.StartDate
                });

        }
    }

}


