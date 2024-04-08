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
    public class DoctorRepo : IDoctorRepo
    {
        #region Prop
        private readonly AppDbContext _context;
        #endregion

        #region construtor
        public DoctorRepo (AppDbContext context)
        {
            _context = context;
        }
        #endregion

        //Methods

        #region Get Appointments
        public async Task<List<AppointmentDetails>> GetAllAppointments()
        {
            return await _context.AppointmentDetails
         .Include(a => a.Patient)
         .Where(a => a.ConsultingDoctor == Specialization && a.DoctorId == doctorId)
         .OrderByDescending(a => a.ScheduleStartTime)
         .ToListAsync();
        }
        #endregion

        #region Check Availability

        public async Task<bool> CheckAvailability(int doctorId, DateTime appointmentTime)
        {
            return !(await _context.AppointmentDetails
                .AnyAsync(a => a.DoctorId == doctorId && a.ScheduleStartTime <= appointmentTime && a.ScheduleEndTime >= appointmentTime));
        }

        #endregion

        #region Reschedule Appointment
        public async Task<bool> RescheduleAppointment(int appointmentId, DateTime newStartTime)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.ScheduleStartTime = newStartTime;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region Cancel Appointment
        public async Task<bool> CancelAppointment(int appointmentId)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            if (appointment != null)
            {
                _context.AppointmentDetails.Remove(appointment);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion

        #region Assigne Nurse Duty
        public async Task<bool> AssignDutyToNurse(int appointmentId, int nurseId)
        {
            var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
            if (appointment != null)
            {
                appointment.NurseId = nurseId;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
    }
}
