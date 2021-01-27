using Cw3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.Models
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string IndexNumber { get; set; }
        public virtual Students IndexNumberNavigation { get; set; }


    }
}
