using HouseForPet.DataBaseContext.Models.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Models
{
    public class UserFavorite : BaseEntity
    {
        public UserFavorite(int userId, int petId)
        {
            UserId = userId;
            PetId = petId;
        }

        public int UserId { get; set; }
        public User User { get; set; }
        public int PetId { get; set; }
        public Pet Pet { get; set; }
    }
}
