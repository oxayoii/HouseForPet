using Microsoft.AspNetCore.Authorization;
using HouseForPet.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBaseContext.Enum;
using DataBaseContext.Models;

namespace Service.Services.Auth
{
    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement(PermissionEnum[] permissions)
        {
            Permissions = permissions;
        }
        public PermissionEnum[] Permissions { get; }
    }
}
