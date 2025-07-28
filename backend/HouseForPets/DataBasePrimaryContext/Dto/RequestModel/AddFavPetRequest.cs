using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Dto.RequestModel
{
    public class AddFavPetRequest
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public int PetId { get; set; }
    }
}
