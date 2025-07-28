using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Service.interfaces.AuthInterfaces;

namespace Service.Auth
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Generate(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(password);
        }
        public bool Verify(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);
        }
    }
}
