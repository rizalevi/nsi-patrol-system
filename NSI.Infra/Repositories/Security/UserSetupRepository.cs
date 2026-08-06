using Dapper;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;
using NSI.Domain.Models.Security ;
using NSI.Infra.Datas;
using NSI.Infra.Helpers;
using System.Data;

namespace NSI.Infra.Repositories.Security
{
    public class UserSetupRepository : IUserSetupRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public UserSetupRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task<List<UserSetupDto>> GetAll()
        {
                string sp = "sp_Web_SS_User_List";
                var result = await _dbExecutor.QueryListAsync<UserSetupDto>(sp);
                return result.ToList();
        }

        public async Task<UserSetupInfo> GetData(string userId)
        {

                string sp = "sp_Web_SS_User_GetData";

                var model = await _dbExecutor.QueryFirstOrDefaultAsync<UserSetupInfo>(
                    sp,
                    new { UserID = userId }
                );

                if (model != null && !string.IsNullOrEmpty(model.Password))
                {
                    model.Password = CryptHelper.PasswordDecrypt(
                        model.Password,
                        userId.ToUpper()
                    );
                }

            return model;
        }

        public async Task Create(UserSetup model, string userLogin)
        {
            var oldPassword = model.Password;
            model.Password = CryptHelper.PasswordEncrypt(model.Password, model.UserID.ToUpper());
            model.ConfirmPassword = CryptHelper.PasswordEncrypt(model.ConfirmPassword, model.UserID.ToUpper());

            //old methode encrypt on desktop
            var crypto = new DesktopVersionEncryptHelper("TOS");
                     
            model.PasswordOld = crypto.EncryptData(oldPassword);

            string sql = @"sp_Web_SS_User_Insert";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.UserID,
                model.UserName,
                model.Password,
                model.ConfirmPassword,
                model.UserGroup,
                model.Email,
                model.StatusEmail,
                model.StatusAdmin,
                model.Locked,
                model.PasswordExpired,
                UserLogin = userLogin,
                model.PasswordOld
            });
        }
        
        public async Task Update(UserSetup model, string userLogin)
        {
            var oldPassword = model.Password;

            model.Password = CryptHelper.PasswordEncrypt(model.Password, model.UserID.ToUpper());
            model.ConfirmPassword = CryptHelper.PasswordEncrypt(model.ConfirmPassword, model.UserID.ToUpper());


            //old methode encrypt on desktop
            var crypto = new DesktopVersionEncryptHelper("TOS");


            model.PasswordOld = crypto.EncryptData(oldPassword); ;

            string sql = @"sp_Web_SS_User_Update";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.UserID,
                model.UserName,
                model.Password,
                model.ConfirmPassword,
                model.UserGroup,
                model.Email,
                model.StatusEmail,
                model.StatusAdmin,
                model.Locked,
                model.PasswordExpired,
                UserLogin = userLogin,
                model.PasswordOld
            });
        }

        public async Task Remove(string userId, string userLogin,string ipAddress)
        {
            string sql = "sp_Web_SS_User_Delete";
            await _dbExecutor.ExecuteAsync(sql, new {UserID = userId , UserLogin = userLogin , IPAddress = ipAddress });
        }

        public async Task<List<UserSettingCourseDto>> GetUserSettingCourse(string userId)
        {
            string sp = "sp_Web_SS_User_SettingCourse_List";
            return (await _dbExecutor.QueryListAsync<UserSettingCourseDto>(sp, new { UserID = userId })).ToList();
        }

        public async Task SaveUserSettingCourse(List<UserSettingCourse> model, string userLogin)
        {
            var param = new DynamicParameters();
            // 🔥 TVP
            param.Add("@DetailUserCourse",
                DataTableHelper.ToUserCourseTable(model).AsTableValuedParameter("dbo.UserSettingPerCourse"));
            param.Add("@UserLogin", userLogin);

            await _dbExecutor.ExecuteAsync(
                "sp_Web_SS_User_SettingCourse_SubmitBatch",
                param,
                commandType: CommandType.StoredProcedure,
                commandTimeout: 60
            );
        }

        public async Task<ProfileDto> GetProfile(string userId)
        {
            string sp = "sp_Web_SS_User_GetProfile";

            var model = await _dbExecutor.QueryFirstOrDefaultAsync<ProfileDto>(
                sp,
                new { UserID = userId }
            );

            if (model != null && !string.IsNullOrEmpty(model.Password))
            {
                model.Password = CryptHelper.PasswordDecrypt(
                    model.Password,
                    userId.ToUpper()
                );
            }

            return model;
        }

        
        public async Task SaveProfile(UserSetup model, string userLogin)
        {
               
            string sql = @"sp_Web_SS_User_SaveProfile";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.UserID,
                model.UserName,
                model.Email,
                model.PictureName,
                model.PicturePath,
                UserLogin = userLogin
            });
        }

    }
}
