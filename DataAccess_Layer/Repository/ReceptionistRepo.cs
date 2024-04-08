using DataAccess_Layer.Interface;
using DataAccess_Layer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess_Layer.Repository
{
    public class ReceptionistRepo : IReceptionistRepo
    {
        #region prop
        private readonly AppDbContext _context;
        #endregion

        #region Constructor
        public ReceptionistRepo(AppDbContext context)
        {
            _context = context;

        }
        #endregion


        //Method

        #region Change Appoinment
        public async Task<bool> ChangeAppointment(int appointmentId, DateTime newAppointmentTime, string newConsultingDoctor)
        {
            try
            {
                var appointment = await _context.AppointmentDetails.FindAsync(appointmentId);
                if (appointment != null)
                {
                    appointment.ScheduleStartTime = newAppointmentTime;
                    appointment.ConsultingDoctor = newConsultingDoctor;
                    _context.Entry(appointment).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Check Doctors Availabilty

        public async Task<bool> CheckDoctorAvailability(Specialization specialization, DateTime appointmentTime)
        {
            try
            {
                // Find the doctor specialization based on the provided specialization
                var doctorSpecialization = await _context.DoctorSpecialization.FirstOrDefaultAsync(ds => ds.Specialization == specialization);

                if (doctorSpecialization == null)
                {
                    // Doctor with the specified specialization not found
                    return false;
                }

                int doctorId = doctorSpecialization.UserId;

                // Check if the doctor has any overlapping appointments at the specified time
                var appointments = await _context.AppointmentDetails
                    .Where(a => a.ConsultingDoctor == doctorId.ToString() && a.ScheduleEndTime >= appointmentTime && a.ScheduleEndTime >= appointmentTime)
                    .ToListAsync();

                // If there are no appointments, the doctor is available
                return !appointments.Any();
            }
            catch (Exception)
            {
                // Handle exceptions
                return false;
            }
        }


        #endregion

        #region Check Patient
        public async Task<bool> CheckPatient(string firstname, string email, DateTime dateofbirth)
        {
            bool check = await _context.User.AnyAsync(u => u.Email == email && u.DateOfBirth == dateofbirth && u.FirstName == firstname && u.Role == Role.Patient);
            return check;

        }

        #endregion

        #region Check Patient by Id
        public async Task<bool> CheckPatientById(int patientId)
        {
            bool check = await _context.User.AnyAsync(u => u.UserId == patientId && u.Role == Role.Patient);
            return check;
        }

        #endregion

        #region Create Patient Profile
        public async Task<bool> CreatePatientProfile(User user)
        {
            try
            {
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {

                return false;
            }
        }

        #endregion

        #region Get Patient Number
        public async Task<int> GetNextPatientNumber()
        {
            try
            {
                // Retrieve the maximum patient number from the database
                int maxPatientNumber = await _context.User
                    .Where(u => u.Role == Role.Patient)
                    .MaxAsync(u => u.UserId);

                // Increment the maximum patient number to get the next available patient number
                int nextPatientNumber = maxPatientNumber + 1;

                return nextPatientNumber;
            }
            catch (Exception)
            {
                // Handle exceptions
                return 1; // Default to 1 if there are no existing patient numbers
            }
        }

        #endregion

        #region Get PAtient Appointment
        public async Task<List<AppointmentDetails>> GetPatientAppointments(int patientId, DateTime? appointmentDate = null)
        {
            try
            {
                IQueryable<AppointmentDetails> query = _context.AppointmentDetails.Where(a => a.PatientId == patientId);

                if (appointmentDate.HasValue)
                {
                    query = query.Where(a => a.ScheduleStartTime.Date == appointmentDate.Value.Date);
                }

                var appointments = await query.ToListAsync();
                return appointments;
            }
            catch (Exception)
            {
                // Handle exceptions
                return null;
            }
        }
        #endregion

        #region Schedule Appoinement
        public async Task<bool> ScheduleAppointment(AppointmentDetails appointmentDetails)
        {
            try
            {
                // Fetch the user object based on the userId (patientId)
                var user = await GetUserById(appointmentDetails.PatientId);
                if (user == null)
                {
                    // User not found
                    return false;
                }

                // Add the appointment to the database context
                _context.AppointmentDetails.Add(appointmentDetails);

                // Save changes asynchronously
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                // Handle exceptions
                return false;
            }

        }
        #endregion

        #region Get User by Id

        public async Task<User> GetUserById(int userId)
        {
            try
            {
                // Retrieve the user from the database based on the userId (patientId)
                var user = await _context.User.FindAsync(userId);
                return user;
            }
            catch (Exception)
            {
                // Handle exceptions
                return null;
            }
        }

        #endregion

    }


}
