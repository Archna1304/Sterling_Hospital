using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Service_Layer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service_Layer.Service
{
    public class NurseService : INurseService
    {
        private readonly INurseRepo _nurseRepo;

        public NurseService(INurseRepo nurseRepo)
        {
            _nurseRepo = nurseRepo;
        }

        public async Task<List<AppointmentDetails>> GetAllDuties()
        {
            return await _nurseRepo.GetAllDuties();
        }

    }
}
