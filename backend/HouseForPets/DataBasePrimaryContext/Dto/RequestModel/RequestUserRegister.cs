using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.RequestModel
{
    public class RequestUserRegister
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }
    }
}
