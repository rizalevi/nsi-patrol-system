using Dapper;
using NSI.Domain.DTOs.Security;
using NSI.Domain.Interfaces.Security;
using NSI.Infra.Datas;
using System.Data;
using NSI.Infra.Helpers;
using NSI.Domain.Models.Security;

namespace NSI.Infra.Repositories.Security
{
    public class ChangePasswordRepository : IChangePasswordRepository
    {
        private readonly DbExecutor _dbExecutor;
       
        public ChangePasswordRepository(DbExecutor dbExecutor)
        {
            _dbExecutor = dbExecutor;
        }


        public async Task Save(ChangePassword model)
        {

            var newpasswordOri = model.NewPassword;

            model.OldPassword = CryptHelper.PasswordEncrypt(model.OldPassword, model.UserID.ToUpper());
            model.NewPassword = CryptHelper.PasswordEncrypt(model.NewPassword, model.UserID.ToUpper());
            model.ConfirmPassword = CryptHelper.PasswordEncrypt(model.ConfirmPassword, model.UserID.ToUpper());

            //old methode encrypt on desktop
            var crypto = new DesktopVersionEncryptHelper("TOS");

            string encrypted = crypto.EncryptData(newpasswordOri);
            
            var PasswordTripleDes = encrypted;

            

            string sql = @"sp_Web_SS_ChangePassword_Save";
            await _dbExecutor.ExecuteAsync(sql, new
            {
                model.UserID,
                model.OldPassword,
                model.NewPassword,
                model.ConfirmPassword,
                model.PasswordExpired,
                PasswordTripleDes
            });
        }
        

       
    }
}
