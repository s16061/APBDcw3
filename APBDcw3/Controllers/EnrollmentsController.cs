using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using APBDcw3.DTOs.Requests;
using APBDcw3.DTOs.Responses;
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

        public EnrollmentsController(IStudentDbService service)
        {
            _service = service;
        }

        [HttpPost]
        [Authorize(Roles = "employee")]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            var response = _service.EnrollStudent(request);
            if (response == null)
            {
                return BadRequest();
            }

                

                return Ok(response);
            
        }


        [HttpPost("promotions")]
        [Authorize(Roles = "employee")]
        public IActionResult PromoteStudent(PromotionStudentRequest request)
        {
            var response = _service.PromoteStudent(request);
            if (response == null)
            {
                return BadRequest();
            }



            return Ok(response);

        }
    }
    }

    
