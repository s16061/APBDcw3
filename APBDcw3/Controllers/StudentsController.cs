using APBDcw3.DAL;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s16061;Integrated Security=True";


        public StudentsController(IDbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        public IActionResult GetStudent()
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM STUDENT", con))
                {
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    var list = new List<Students>();
                    while (dr.Read())
                    {
                        var st = new Students();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        list.Add(st);


                    }
                    return Ok(list);

                }
            }
        }

        [HttpGet("enrollment/{id}")]
        public IActionResult GetStudentEnrollment(int id)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("checkSemester", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);
                    con.Open();
                    var dr = cmd.ExecuteReader();
                    var list = new List<Students>();
                    while (dr.Read())
                    {
                        var st = new Students();
                        st.IndexNumber = dr["IndexNumber"].ToString();
                        st.FirstName = dr["FirstName"].ToString();
                        st.LastName = dr["LastName"].ToString();
                        st.Semester = dr["Semester"].ToString();
                        list.Add(st);


                    }
                    return Ok(list);

                }
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetStudent(int id)
        {
            if (id == 1)
                return Ok("Kowalski");
            else if (id == 2)
                return Ok("Malewski");
            return NotFound("Nie znaleziono użytkownika");
        }

        [HttpPost]
        public IActionResult CreateStudent(Students student)
        {
            student.IndexNumber = $"s{new Random().Next(1, 2000)}";
            return Ok(student);
        }
        [HttpPut("{id}")]
        public IActionResult UpdateStudent(int id, Students student)
        {
            if (id < 3)
            {
                student.IdStudent = id;
                return Ok(student);
            }
            else
                return NotFound("Student o tym id nie został znaleziony");
            
        }
        [HttpDelete("{id}")]
        public IActionResult DeleteStudent(int id)
        {
            {
                if (id > 2)
                    return NotFound("Nie znaleziono użytkownika");
                else
                    return Ok("Usuwanie ukończone");
            }
        }
    }
}
