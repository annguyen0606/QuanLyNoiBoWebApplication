using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuanLyNoiBoWebAPI.Model
{
    public class RegisterResponseModel
    {
        public int err_code { get; set; }
        public string err_message { get; set; }
        public string data { get; set; }
    }
}
