using NSI.Domain.DTOs.Security;
using NSI.Domain.Models.Security;

namespace NSI.Domain.Interfaces.Security
{
    public interface IUserSetupRepository
    {
        Task<List<UserSetupDto>> GetAll();
        Task<UserSetupInfo?> GetData(string userId);
        Task Create(UserSetup model, string userLogin);
        Task Update(UserSetup payload , string userLogin);
        Task Remove(string userId, string userLogin, string ipAddress);
        //Task<Dictionary<string, object>> Capture(string addressCode);


        //user course setting
        Task<List<UserSettingCourseDto>> GetUserSettingCourse(string userId);
        Task SaveUserSettingCourse(List<UserSettingCourse> model, string userLogin);

        Task<ProfileDto?> GetProfile(string userId);
        Task SaveProfile(UserSetup payload, string userLogin);
    }
}
