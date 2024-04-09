using DataAccess_Layer.Interface;
using Service_Layer.Interface;

namespace Service_Layer.Service
{
    public class NurseService : INurseService
    {
        #region Prop
        private readonly INurseRepo _nurseRepo;
        #endregion

        #region Constructor
        public NurseService(INurseRepo nurseRepo)
        {
            _nurseRepo = nurseRepo;
        }
        #endregion

        //Methods 

        #region Get Duties
        public async Task<List<dynamic>> GetNurseDuties(int nurseId)
        {
            return await _nurseRepo.GetNurseDuties(nurseId);
        }
        #endregion


    }
}
