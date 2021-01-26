using APBDcw3.Models;
using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.DAL
{
    public interface IDbService
    {
        public IEnumerable<Students> getStudents();
        public Students GetStudent(string index);

        public int CreateRefreshToken(RefreshToken refreshToken);
        public int DeleteRefreshToken(string refreshToken);
        public Students GetUserWithRefreshToken(string refreshToken);

    }
}
