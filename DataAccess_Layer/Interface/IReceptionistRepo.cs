using DataAccess_Layer.Models;

namespace DataAccess_Layer.Interface
{
    public interface IReceptionistRepo
    {
        Task<bool> CheckDoctorAvailability(Specialization specialization, DateTime appointmentTime);

        Task<bool> ScheduleAppointment(AppointmentDetails appointmentDetails);

        Task<bool> ChangeAppointment(int appointmentId, DateTime newAppointmentTime, string newConsultingDoctor);

        Task<List<AppointmentDetails>> GetPatientAppointments(int patientId, DateTime? appointmentDate = null);


        Task<bool> CreatePatientProfile(User user);

        Task<int> GetNextPatientNumber();


        Task<bool> CheckPatientById(int patientId);


        Task<bool> CheckPatient(string firstName, string email, DateTime dateOfBirth);
        Task<User> GetUserById(int userId);


    }
}
