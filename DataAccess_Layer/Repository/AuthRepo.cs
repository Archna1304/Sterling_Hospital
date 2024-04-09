using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Layer.Repository
{
    public class AuthRepo : IAuthRepo
    {
        #region prop
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public AuthRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion

        //Method

        #region Register Method
        public async Task<bool> Register(User user)
        {
            try
            {
                _context.User.Add(user);
                var result = await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region Login Method

        //Login with email
        public async Task<User> LoginWithEmail (string email, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            return user;
        }

        //Login with Phone Number
        public async Task<User> LoginWithPhoneNumber (string phoneNumber, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber && u.Password == password);
            return user;
        }
        #endregion

    }
}
