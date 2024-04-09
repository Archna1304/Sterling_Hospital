using DataAccess_Layer.Interface;
using Service_Layer.DTO;
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

        #region Get Doctor Appointments
        public async Task<List<dynamic>> GetDoctorAppointments(string specialization, int doctorId)
        {
            return await _doctorRepo.GetDoctorAppointments(specialization, doctorId);
        }
        #endregion

        #region Reschedule Appointment
            public async Task<ResponseDTO> RescheduleAppointment(ChangeAppointmentDTO changeAppointmentDTO)
            {
                try
                {
                    bool result = await _doctorRepo.RescheduleAppointment(changeAppointmentDTO.AppointmentId, changeAppointmentDTO.NewAppointmentStartTime, changeAppointmentDTO.NewConsultingDoctor);
                    if (result)
                    {
                        return new ResponseDTO { Status = 200, Message = "Appointment rescheduled successfully." };
                    }
                    else
                    {
                        return new ResponseDTO { Status = 400, Message = "Failed to reschedule appointment. Appointment not found." };
                    }
                }
                catch (Exception ex)
                {
                    return new ResponseDTO { Status = 500, Message = "An error occurred while rescheduling appointment.", Error = ex.Message };
                }
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
