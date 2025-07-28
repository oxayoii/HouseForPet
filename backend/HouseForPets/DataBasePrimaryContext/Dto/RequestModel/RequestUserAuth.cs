using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.RequestModel
{
    public class RequestUserAuth
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string CaptchaToken { get; set; }
        public string CaptchaInput { get; set; }
    }
}
