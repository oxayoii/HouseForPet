using DataBaseContext.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace HouseForPet.DataBaseContext.Models.Pets
{
    public class User : BaseEntity {
        public User(string login, string passwordHash)
        {
            CreatAt = DateTime.UtcNow;
            Login = login;
            PasswordHash = passwordHash;
        }

        [Required(ErrorMessage = "Login is required.")]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Login must be between 3 and 10 characters.")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string PasswordHash { get; set; }
        public Guid RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public ICollection<UserPermission> UserPermission { get; set; }
        public ICollection<UserFavorite> UserFavorites { get; set; }

    }
}
