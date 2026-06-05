using DocumentFormat.OpenXml.Spreadsheet;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security;
using NSI.Infra.Helpers;

namespace NSI.Api.Services.Security
{
    public class UserSetupRepositories 
    {
        private readonly IUserSetupRepository _repo;

        public UserSetupRepositories(IUserSetupRepository repo)
        {
            _repo = repo;
        }


        public async Task<IEnumerable<UserSetupDto>> GetAll()
        {
            var data = await _repo.GetAll();
            return data.Select(x => new UserSetupDto
            {
                UserID = x.UserID,
                UserName = x.UserName,
                UserGroupName = x.UserGroupName,
                ClusterName = x.ClusterName,
                AdminStatus = x.AdminStatus,
                LockedStatus = x.LockedStatus,
                PasswordExpired = x.PasswordExpired,
                RegisterDate = x.RegisterDate,
                RegisterUser = x.RegisterUser,
                LastUpdate = x.LastUpdate,
                LastUser = x.LastUser
            });
        }

        public async Task<UserSetupInfo> GetData(string userId)
        {
            var data = await _repo.GetData(userId);
            if (data == null) return null;

            return new UserSetupInfo
            {
                UserID = data.UserID,
                UserName = data.UserName,
                Password = CryptHelper.PasswordDecrypt(data.Password, data.UserID.ToUpper()),
                UserGroupCode = data.UserGroupCode,
                UserGroupName = data.UserGroupName,
                ClusterCode = data.ClusterCode,
                ClusterName = data.ClusterName,
                StatusAdmin = data.StatusAdmin,
                Locked = data.Locked,
                PasswordExpired = data.PasswordExpired

            };
        }

        public async Task Create(UserSetup model, string userLogin)
        {
           
            var entity = new UserSetup
            {
                UserID = model.UserID,
                UserName = model.UserName,
                Password = CryptHelper.PasswordEncrypt(model.Password,model.UserID.ToUpper()),  
                ConfirmPassword = CryptHelper.PasswordEncrypt(model.ConfirmPassword, model.UserID.ToUpper()),
                UserGroup = model.UserGroup,
                StatusAdmin = model.StatusAdmin,
                Locked = model.Locked,
                PasswordExpired = model.PasswordExpired

            };

            await _repo.Create(entity, userLogin);
        }

        public async Task Update(UserSetup model, string userLogin)
        {
            var entity = new UserSetup
            {
                UserID = model.UserID,
                UserName = model.UserName,
                Password = CryptHelper.PasswordEncrypt(model.Password, model.UserID.ToUpper()),
                ConfirmPassword = CryptHelper.PasswordEncrypt(model.ConfirmPassword, model.UserID.ToUpper()),
                UserGroup = model.UserGroup,
                StatusAdmin = model.StatusAdmin,
                Locked = model.Locked,
                PasswordExpired = model.PasswordExpired
            };

            await _repo.Update(entity, userLogin);
        }

        public async Task Remove(string userId, string userLogin, string ipAddress)
        {
            await _repo.Remove(userId, userLogin, ipAddress);
        }



    }
}
