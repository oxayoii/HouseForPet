using HouseForPet.DataBaseContext.Models.Pets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;

namespace Service.Extensions
{
    public static class ConvertToGenderPets
    {
        public static Sex ConvertToGender(string gender)
        {
            return gender switch
            {
                "М" => Sex.M,
                "Ж" => Sex.W,
                _ => throw new BadRequestException("Неизвестное значение пола")
            };
        }
    }
}
