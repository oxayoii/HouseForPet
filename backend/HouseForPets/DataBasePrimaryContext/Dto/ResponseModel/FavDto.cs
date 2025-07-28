using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.ResponseModel
{
    public class FavDto
    {
        public int Id { get; set; }
        public int PetId { get; set; }
        public string ImageUrl { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Description { get; set; }
    }
}
