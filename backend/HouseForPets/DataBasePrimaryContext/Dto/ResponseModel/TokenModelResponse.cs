using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.ResponseModel
{
    public class TokenModelRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
