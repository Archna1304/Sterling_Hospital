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

        // Login with email or phone number
        public async Task<User> LoginWithEmailOrPhoneNumber(string emailOrPhoneNumber, string password)
        {
            User user = null;

            if (IsEmail(emailOrPhoneNumber))
            {
                // Login with email
                user = await _context.User.FirstOrDefaultAsync(u => u.Email == emailOrPhoneNumber && u.Password == password);
            }
            else
            {
                // Login with phone number
                user = await _context.User.FirstOrDefaultAsync(u => u.PhoneNumber == emailOrPhoneNumber && u.Password == password);
            }

            return user;
        }

        // Helper method to check if input is in email format
        private bool IsEmail(string input)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(input);
                return addr.Address == input;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
