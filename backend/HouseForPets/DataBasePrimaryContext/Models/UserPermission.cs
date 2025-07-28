using HouseForPet.DataBaseContext.Models.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Models
{
    public class UserPermission : BaseEntity
    {
        public UserPermission(int userId, int permissionId)
        {
            UserId = userId;
            PermissionId = permissionId;
        }
        public User Users { get; set; }
        public int UserId { get; set; }
        public Permission Permission { get; set; }
        public int PermissionId { get; set; }
    }
}
