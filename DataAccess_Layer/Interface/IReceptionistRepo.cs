using DataAccess_Layer.Models;

namespace DataAccess_Layer.Interface
{
    public interface IReceptionistRepo
    {
        Task<bool> CheckDoctorAvailability(Specialization specialization, DateTime appointmentTime);

        Task<bool> ScheduleAppointment(AppointmentDetails appointmentDetails);


        Task<List<dynamic>> GetPatientAppointments(int patientId);


        Task<bool> CreatePatientProfile(User user);

        Task<int> GetNextPatientNumber();


        Task<bool> CheckPatientById(int patientId);


        Task<bool> CheckPatient(string firstName, string email, DateTime dateOfBirth);
        


    }
}
