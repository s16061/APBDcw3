using APBDcw3.DTOs.Requests;
using APBDcw3.DTOs.Responses;
using APBDcw3.Models;
using Cw3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.Services
{
    public class SqlServerStudentDbService : IStudentDbService
    {
        private const string ConnectionString = "Data Source=db-mssql;Initial Catalog=s16061;Integrated Security=True;MultipleActiveResultSets=true";
        private readonly s16061Context _dbcont;
        public SqlServerStudentDbService(s16061Context dbcont)
            {
            _dbcont = dbcont;
            }

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
           
            var response = new EnrollStudentResponse();
           

            return (response);
        }
        public Enrollment StudentEnrollment(
                        string indexNumber, string firstName, string lastName, DateTime birthDate, string name)

        {
            var st = GetStudyName(name);
            var enrollment = GetEnrollment(st.IdStudy, 1);
            if(enrollment == null)
            {
                enrollment = new Enrollment
                {
                    IdEnrollment = _dbcont.Enrollments.Max(e => e.IdEnrollment) + 1,
                    IdStudy = st.IdStudy,
                    Semester = 1,
                    StartDate = DateTime.Now
                };
                _dbcont.Attach(enrollment);
                _dbcont.Add(enrollment);
            }
            return enrollment;
        }
            
        public Enrollment GetEnrollment(int idStudy, int semester)
        {
            return _dbcont.Enrollments
                    .Where(e => e.IdStudy == idStudy)
                    .Where(e => e.Semester == semester)
                    .FirstOrDefault();

        }

        public Enrollment GetEnrollment(int idStudy)
        {
            return _dbcont.Enrollments
                    .Where(e => e.IdStudy == idStudy)
                    .FirstOrDefault();

        }


        public Study GetStudyName(string name)
        {
            return _dbcont.Studies.Where(study => string.Equals(study.Name, name)).FirstOrDefault();
        }


        /*
        var st = new Students();
        st.IndexNumber = request.IndexNumber;
        st.FirstName = request.FirstName;
        st.LastName = request.LastName;
        st.Birthdate = request.Birthdate;
        st.Studies = request.Studies;

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            using (SqlCommand cmd = new SqlCommand())
            {

                con.Open();

                SqlTransaction tran = con.BeginTransaction();
                cmd.Connection = con;
                cmd.Transaction = tran;

                cmd.CommandText = "SELECT IdStudy from Studies where name=@name";
                cmd.Parameters.AddWithValue("@name", request.Studies);
                var dr = cmd.ExecuteReader();
                if (!dr.Read())
                {
                    dr.Close();
                    tran.Rollback();

                }

                int IdStudy = (int)dr["IdStudy"];
                dr.Close();

                cmd.CommandText = "SELECT IdEnrollment FROM Enrollment WHERE (IdStudy=@idstudy AND Semester=1)";
                cmd.Parameters.AddWithValue("@idstudy", IdStudy);
                dr = cmd.ExecuteReader();
                int IdEnrollment = 0;
                if (dr.Read())
                {
                    IdEnrollment = (int)dr["IdEnrollment"];
                }
                else
                {
                    dr.Close();

                    cmd.CommandText = "SELECT MAX(IdEnrollment) AS MaxIdEnrollment FROM Enrollment WHERE Semester=1";
                    dr = cmd.ExecuteReader();


                    if (dr.Read())
                    {
                        IdEnrollment = (int)dr["MaxIdEnrollment"];


                        cmd.CommandText = "INSERT INTO Enrollment (IdEnrollment, Semester, IdStudy, StartDate) VALUES (@MaxIdEnrollment, @Semester, @IdStudy, @Sdate)";
                        DateTime today = DateTime.Today;
                        cmd.Parameters.AddWithValue("@MaxIdEnrollment", IdEnrollment);
                        cmd.Parameters.AddWithValue("@Semester", 1);
                        cmd.Parameters.AddWithValue("@IdStudy", IdStudy);
                        cmd.Parameters.AddWithValue("@Sdate", today);
                    }

                }
                dr.Close();
                cmd.Parameters.Clear();

                cmd.CommandText = "INSERT into Student(IndexNumber, FirstName, LastName, BirthDate, IdEnrollment) VALUES (@Index, @Fname, @Lname, @Birthdate, @Ienrollment)";
                cmd.Parameters.AddWithValue("@Index", request.IndexNumber);
                cmd.Parameters.AddWithValue("@Fname", request.FirstName);
                cmd.Parameters.AddWithValue("@Lname", request.LastName);
                cmd.Parameters.AddWithValue("@Birthdate", request.Birthdate);
                cmd.Parameters.AddWithValue("@Ienrollment", IdEnrollment);
                cmd.ExecuteNonQuery();


                tran.Commit();

                var response = new EnrollStudentResponse();
                response.IndexNumber = st.IndexNumber;
                response.FirstName = st.FirstName;
                response.LastName = st.LastName;
                response.Studies = st.Studies;

                return (response);

            }

        }

    }
    */

        public Students GetStudent(string index)
        {
            var st = new Students();
            st.IndexNumber = index;

            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = "SELECT IndexNumber FROM Student WHERE IndexNumber=@indexnumber";
                    cmd.Parameters.AddWithValue("@indexnumber", index);
                    var dr = cmd.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        return null;
                        
                    }
                    st.IndexNumber= (string)dr["IndexNumber"];

                    dr.Close();
                }
                    }
            return st;

        }

        public Enrollment Promote(int idStudy, int semester)
        {
            var getEnrollment = GetEnrollment(idStudy, semester);
            if (getEnrollment == null)
                return null;

            var setEnrollment = GetEnrollment(idStudy, semester+1);
            if (setEnrollment == null)
            {
                setEnrollment = new Enrollment
                {
                    IdEnrollment = _dbcont.Enrollments.Max(e => e.IdEnrollment) + 1,
                    IdStudy = getEnrollment.IdStudy,
                    Semester = getEnrollment.Semester + 1
                };
                _dbcont.Attach(setEnrollment);
                _dbcont.Add(setEnrollment);
            }
            _dbcont.Students
                .Where(s => s.IdEnrollment == getEnrollment.IdEnrollment)
                .ToList()
                .ForEach(s =>
                {
                    s.IdEnrollment = setEnrollment.IdEnrollment;
                    _dbcont.Attach(s);
                    _dbcont.Entry(s).State = EntityState.Modified;
                });
            _dbcont.SaveChanges();
            return setEnrollment;
        }
        /*
public PromotionStudentResponse PromoteStudent(PromotionStudentRequest request)
{
   using (SqlConnection con = new SqlConnection(ConnectionString))
   {
       using (SqlCommand cmd = new SqlCommand())
       {
           var st = new Students();
           st.Studies = request.Studies;
           st.Semester = request.Semester;

           con.Open();

           cmd.Connection = con;
           cmd.CommandText = "SELECT IdEnrollment, Semester, E.IdStudy " +
   "FROM Enrollment E " +
   "JOIN Studies S " +
   "ON S.IdStudy = E.IdStudy " +
   "WHERE Semester = @semester AND S.Name = @name";

           cmd.Parameters.AddWithValue("@name", request.Studies);
           cmd.Parameters.AddWithValue("@semester", request.Semester);

           var dr = cmd.ExecuteReader();
           if (!dr.Read())
           {
               dr.Close();
           }
           dr.Close();
           cmd.CommandText = "promoteStudents";
           cmd.CommandType = CommandType.StoredProcedure;
           cmd.Parameters.AddWithValue("@name", request.Studies);
           cmd.Parameters.AddWithValue("@semester", request.Semester);

           var response = new PromotionStudentResponse();
           response.Studies = st.Studies;
           response.Semester = st.Semester + 1;

           return (response);



       }
   }
}
*/
    }
}
