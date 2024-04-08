using DataAccess_Layer.Models;

namespace Service_Layer.Interface
{
    public interface INurseService
    {
        Task<List<AppointmentDetails>> GetAllDuties();
    }
}
