using DataAccess_Layer.Models;

namespace DataAccess_Layer.Interface
{
    public interface INurseRepo
    {
        Task<List<dynamic>> GetNurseDuties(int nurseId);
    }
}
