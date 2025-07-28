using DataBaseContext.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HouseForPet.DataBaseContext.Models.Pets
{
    public class Pet : BaseEntity
    {
        public Pet(string imageKey, string name, int age, Sex gender, string description)
        {
            CreatAt = DateTime.UtcNow;
            ImageKey = imageKey;
            Name = name;
            Age = age;
            Gender = gender;
            Description = description;
        }
        [Required(ErrorMessage = "Image is required")]
        public string ImageKey { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [StringLength(10, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 10 characters.")]
        [RegularExpression(@"^[a-zA-Z\s]+$", ErrorMessage = "Name can only contain letters and spaces.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [Range(0, 20, ErrorMessage = "Age must be between 0 and 20.")]
        public int Age { get; set; }
        [EnumDataType(typeof(Sex), ErrorMessage = "Gender cannot take this value")]
        public Sex Gender { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 200 characters.")]
        [RegularExpression(@"^[a-zA-Z\s.,!?'""-]+$", ErrorMessage = "Description can only contain letters, numbers, spaces, and certain punctuation.")]
        public string Description { get; set; }
        public ICollection<UserFavorite> UserFavoritePet { get; set; }

    }
    public enum Sex
    {
        M,
        W
    }
}
