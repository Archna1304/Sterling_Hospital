using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Service_Layer.Interface;

namespace Service_Layer.Service
{
    public class DoctorService : IDoctorService
    {
        #region Prop
        private readonly IDoctorRepo _doctorRepo;
        #endregion

        #region constructor
        public DoctorService(IDoctorRepo doctorRepo)
        {
            _doctorRepo = doctorRepo;
        }
        #endregion

        //Methods

        #region Get All Appoinements
        public async Task<List<AppointmentDetails>> GetAllAppointments()
        {
            return await _doctorRepo.GetAllAppointments();
        }
        #endregion

        #region Reschedule Appointment
        public async Task<bool> RescheduleAppointment(int appointmentId, DateTime newStartTime)
        {
            return await _doctorRepo.RescheduleAppointment(appointmentId, newStartTime);
        }
        #endregion

        #region Cancel Appointments
        public async Task<bool> CancelAppointment(int appointmentId)
        {
            return await _doctorRepo.CancelAppointment(appointmentId);
        }

        #endregion

        #region AsisgnDuty to Nurse
        public async Task<bool> AssignDutyToNurse(int appointmentId, int nurseId)
        {
            return await _doctorRepo.AssignDutyToNurse(appointmentId, nurseId);
        }
        #endregion
    }
}
