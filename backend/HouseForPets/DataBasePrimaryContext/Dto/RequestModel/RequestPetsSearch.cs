using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.RequestModel
{
    public class ResponsePetsSearch()
    {
        public string? SortItem { get; set; } //age
        public string? SortOrder { get; set; }
        public string? SortGender { get; set; }
    }
}
