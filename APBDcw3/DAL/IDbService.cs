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
    }
}
