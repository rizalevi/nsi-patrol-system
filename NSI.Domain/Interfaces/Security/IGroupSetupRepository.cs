using NSI.Domain.DTOs.Security;
using NSI.Domain.Models.Security;

namespace NSI.Domain.Interfaces.Security
{
    public interface IGroupSetupRepository
    {

        Task<List<GroupSetupDDLDto>> GetGroupDDL();
        Task<List<GroupSetupDto>> GetAll();
        Task<GroupSetup?> GetData(string groupId);
        Task Save(GroupSetup model, string userLogin);
        Task Remove(string groupId, string userLogin, string ipAddress);
        //Task<Dictionary<string, object>> Capture(string addressCode);
               

    }
}
