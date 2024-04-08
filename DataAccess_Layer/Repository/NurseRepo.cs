using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess_Layer.Repository
{
    public class NurseRepo : INurseRepo
    {
        #region prop
        private readonly AppDbContext _context;
        #endregion

        #region constuctor
        public NurseRepo(AppDbContext context)
        {
            _context = context;
        }
        #endregion


        //Methods

        #region Get Duties
        public async Task<List<AppointmentDetails>> GetAllDuties()
        {
            return await _context.AppointmentDetails.Include(a => a.Patient).ToListAsync();
        }
        #endregion
    }
}
