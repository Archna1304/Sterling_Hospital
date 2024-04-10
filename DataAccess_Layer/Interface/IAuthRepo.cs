using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interface
{
    public interface IAuthRepo
    {
        Task<bool> Register(User user);
        Task<User> LoginWithEmailOrPhoneNumber(string emailOrPhoneNumber, string password);
    }
}
