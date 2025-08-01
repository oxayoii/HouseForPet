﻿
using DataBaseContext.Dto.RequestModel;
using HouseForPet.DataBaseContext.Models.Pets;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Service.Middleware.CustomException;

namespace HouseForPet.Service
{
    public static class PetsExtensions
    {
        public static IQueryable<Pet> Filter (this IQueryable<Pet> query, ResponsePetsSearch response) 
        {
            if (!string.IsNullOrEmpty(response.SortGender))
            {
                if (Enum.TryParse<Sex>(response.SortGender, true, out var gender))
                {
                    query = query.Where(x => x.Gender == gender);
                }
            }
            return query;
        }
        public static IQueryable<Pet> Sort(this IQueryable<Pet> query, ResponsePetsSearch response)
        {
            Expression<Func<Pet, object>> selectorKey = response.SortItem?.ToLower()
            switch
            {
                "age" => pets => pets.Age,
               _ => pets => pets.Id
            };
            query = response.SortOrder == "desc"
                ? query.OrderByDescending(selectorKey) : query.OrderBy(selectorKey);
            return query;
        }
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
