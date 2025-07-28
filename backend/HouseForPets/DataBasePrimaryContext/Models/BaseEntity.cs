using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseContext.Models
{
    public class BaseEntity
    {
        [Key]
        public int Id{ get; set; }

        public DateTime CreatAt { get; set; }
        public DateTime ModifyDate { get; set; }

        public BaseEntity()
        {
            CreatAt = DateTime.UtcNow;
        }
        public void Update()
        {
            ModifyDate = DateTime.UtcNow;
        }
    }
}
