using APBDcw3.DAL;
using APBDcw3.DTOs.Requests;
using APBDcw3.DTOs.Responses;
using APBDcw3.Models;
using APBDcw3.Services;
using Cw3.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Cw3.Controllers
{
    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly IDbService _dbService;
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s16061;Integrated Security=True";

        public IConfiguration Configuration { get; set; }

        public StudentsController(IConfiguration configuration, IDbService dbService)
        {
            Configuration = configuration;
            _dbService = dbService;
        }
         


        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            var student = _dbService.GetStudent(request.Login);

            if (student == null)
                return NotFound("Username or password dosen't exists or is incorrect");
              

            var claims = new[] {
                                new Claim(ClaimTypes.NameIdentifier, student.IndexNumber),
                                new Claim(ClaimTypes.Name, student.FirstName),
                                new Claim(ClaimTypes.Name, student.LastName),
                                new Claim(ClaimTypes.Role, "student"),
            };
            static string Create(string value, string salt)
            {
                var valueBytes = KeyDerivation.Pbkdf2(
                    password: value,
                    salt: Encoding.UTF8.GetBytes(salt),
                    prf: KeyDerivationPrf.HMACSHA512,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                    );
                return Convert.ToBase64String(valueBytes);
            }

            static string CreateSalt()
            {
                byte[] randomBytes = new byte[128 / 8];
                using (var generator = RandomNumberGenerator.Create())
                {
                    generator.GetBytes(randomBytes);
                    return Convert.ToBase64String(randomBytes);
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            
            var token = new JwtSecurityToken(
                issuer: "s16061",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );

            var response = new LoginResponse
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid().ToString()
            };

            _dbService.CreateRefreshToken(
                new RefreshToken { Id = response.refreshToken, IndexNumber = student.IndexNumber });
            return Ok(response);
        }
        [HttpPost("refresh-token/{token}")]
        public IActionResult RefreshToken(string refToken)
        {
            var st = _dbService.GetUserWithRefreshToken(refToken);

           var claims = new[] {
                                new Claim(ClaimTypes.NameIdentifier, st.IndexNumber),
                                new Claim(ClaimTypes.Name, st.FirstName),
                                new Claim(ClaimTypes.Name, st.LastName),
                                new Claim(ClaimTypes.Role, "student"),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
                issuer: "s16061",
                audience: "Students",
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
                );

            var response = new LoginResponse
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = Guid.NewGuid().ToString()
            };

            _dbService.CreateRefreshToken(
                new RefreshToken { Id = response.refreshToken, IndexNumber = st.IndexNumber });
            return Ok(response);
        }

        [HttpGet]
        [Authorize]
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
                        st.Semester = int.Parse(dr["Semester"].ToString());
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
