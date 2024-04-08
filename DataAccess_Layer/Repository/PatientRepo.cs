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
    public class PatientRepo : IPatientRepo
    {
        private readonly AppDbContext _context;

        public PatientRepo(AppDbContext context)
        {
            _context = context;
        }

        // Get current appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetCurrentAppointments(int patientId)
        {
            return await _context.AppointmentDetails
                .Where(a => a.PatientId == patientId && a.ScheduleStartTime >= DateTime.Now)
                .OrderByDescending(a => a.ScheduleStartTime)
                .ToListAsync();
        }

        // Get previous appointments for a specific patient ordered by latest first
        public async Task<List<AppointmentDetails>> GetPreviousAppointments(int patientId)
        {
            return await _context.AppointmentDetails
                .Where(a => a.PatientId == patientId && a.ScheduleStartTime < DateTime.Now)
                .OrderByDescending(a => a.ScheduleStartTime)
                .ToListAsync();
        }

    }
}
