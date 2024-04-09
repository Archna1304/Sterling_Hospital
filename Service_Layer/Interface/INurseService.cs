namespace Service_Layer.Interface
{
    public interface INurseService
    {
        Task<List<dynamic>> GetNurseDuties(int nurseId);
    }
}
