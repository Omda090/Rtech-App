using Rtech_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rtech_App.Interface
{
   public interface IAuthRepository
    {
        //
        Task<User> Register(User user, string Password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
         
    }
}
