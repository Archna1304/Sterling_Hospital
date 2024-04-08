using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Layer.Repository
{
    public class UserRepo : IUserRepo
    {
        #region prop
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public UserRepo(AppDbContext context)
        {
            _context = context;

        }
        #endregion


        //Method

        #region Check Doctor, Nurse, Receptionist
        public async Task<User> CheckDoctor()
        {
            var check = await _context.User.FirstOrDefaultAsync(u => u.Role == Role.Doctor);
            return check;
        }

        public async Task<User> CheckNurse()
        {
            var check = await _context.User.FirstOrDefaultAsync(u => u.Role == Role.Nurse);
            return check;
        }

        public async Task<User> CheckReceptionist()
        {
            var check = await _context.User.FirstOrDefaultAsync(u => u.Role == Role.Receptionist);
            return check;
        }
        #endregion


        #region Count Doctor, Nurse, Receptionist
        public async Task<int> CountDoctor()
        {
            int count = await _context.User.CountAsync(u => u.Role == Role.Doctor);
            return count;
        }

        public async Task<int> CountNurse()
        {
            int count = await _context.User.CountAsync(u => u.Role == Role.Nurse);
            return count;
        }

        public async Task<int> CountReceptionist()
        {
            int count = await _context.User.CountAsync(u => u.Role == Role.Receptionist);
            return count;
        }
        #endregion
    }
}
