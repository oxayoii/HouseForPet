using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.ResponseModel
{
    public class ResponseCaptcha
    {
        public string Question { get; set; }
        public string Token { get; set; }
    }
}
