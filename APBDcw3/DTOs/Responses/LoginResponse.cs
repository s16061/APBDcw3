using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APBDcw3.DTOs.Responses
{
    public class LoginResponse
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
    }
} 
