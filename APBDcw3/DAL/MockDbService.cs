using APBDcw3.Models;
using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.DAL
{
    public class MockDbService : IDbService
    {
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s16061;Integrated Security=True;MultipleActiveResultSets=true";
        private static IEnumerable<Students> _students;

        static MockDbService()
        {/*
            _students = new List<Students>
            {
                new Students{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
                new Students{IdStudent=2, FirstName="Anna", LastName="Malewski"},
                new Students{IdStudent=3, FirstName="Andrzej", LastName="Andrzejewicz"}
            };
            */
        }
        

        public int DeleteRefreshToken(string refreshToken)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "DELETE FROM RefreshTokenAuth WHERE Id=@refreshToken";
                    cmd.Parameters.AddWithValue("refreshToken", refreshToken);
                    return cmd.ExecuteNonQuery();
                };

            }
        }

        public int CreateRefreshToken(RefreshToken refreshToken)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "INSERT INTO RefreshTokenAuth VALUES(@id, @indexNumber)";
                    cmd.Parameters.AddWithValue("id", refreshToken.Id);
                    cmd.Parameters.AddWithValue("indexNumber", refreshToken.IndexNumber);
                    return cmd.ExecuteNonQuery();
                };


            }
        }

        public Students GetStudent(string index)
        {

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT IndexNumber, FirstName, LastName FROM Student WHERE IndexNumber=@indexnumber";
                    cmd.Parameters.AddWithValue("@indexnumber", index);
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        var student = new Students();
                        {
                            student.IndexNumber = dr["IndexNumber"].ToString();
                            student.FirstName = dr["FirstName"].ToString();
                            student.LastName = dr["LastName"].ToString();
                        };
                        return student;

                    }

                }
                return null;
            }

        }


        public IEnumerable<Students> getStudents()
        {
            return _students;
        }

        public Students GetUserWithRefreshToken(string refreshToken)
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT * FROM RefreshTokenAuth WHERE Id = @refreshToken";
                    cmd.Parameters.AddWithValue("refreshToken", refreshToken);
                    var dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        var token = new RefreshToken
                        {
                            Id = dr["Id"].ToString(),
                            IndexNumber = dr["IndexNumber"].ToString()
                        };
                        return GetStudent(token.IndexNumber);
                    }
                    return null;
                }
            }
        }
    }
}
        


