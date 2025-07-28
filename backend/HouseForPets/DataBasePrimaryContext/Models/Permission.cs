using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Models
{
    public class Permission : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<UserPermission> UserPermission { get; set; }
    }
}
