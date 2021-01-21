using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.DAL
{  
    public class MockDbService : IDbService
    {
        private static IEnumerable<Students> _students;

        static MockDbService() {
            _students = new List<Students>
            {
                new Students{IdStudent=1, FirstName="Jan", LastName="Kowalski"},
                new Students{IdStudent=2, FirstName="Anna", LastName="Malewski"},
                new Students{IdStudent=3, FirstName="Andrzej", LastName="Andrzejewicz"}
            };
        }
        public IEnumerable<Students> getStudents()
        {
            return _students;
        }
    }
}
