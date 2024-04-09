using DataAccess_Layer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Interface
{
    public  interface IUserRepo
    {
        Task<User> CheckNurse();
        Task<User> CheckReceptionist();
        Task<User> CheckDoctor();
        Task<int> CountNurse();
        Task<int> CountReceptionist();
        Task<int> CountDoctor();
        Task<User> GetUserById(int userId);
        Task<bool> DeleteUser(User user);
    }
}
