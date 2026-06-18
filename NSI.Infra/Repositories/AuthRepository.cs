using Dapper;
using NSI.Infra.Datas;
using System.Data;
using NSI.Domain.DTOs;
using NSI.Domain.Interfaces;
using NSI.Domain.Models.Auth;
using NSI.Domain.DTOs.Email;

namespace NSI.Infra.Repositories
{
    internal class AuthRepository : IAuthRepository 
    {
        private readonly IDbConnectionFactory _connectionFactory;
        private readonly IHashService _hashService;

        public AuthRepository(
       IDbConnectionFactory connectionFactory,
       IHashService hashService)
        {
            _connectionFactory = connectionFactory;
            _hashService = hashService;
        }

      

        public async Task<UserDto?> LoginAsync(string userId, string password)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<UserDto>(
                "sp_SS_User_CheckUser",
                new
                {
                    UserId = userId,
                    Password = password
                },
                commandType: CommandType.StoredProcedure
            );
        }

        //if (dbUserData.Count > 0)
        //{
        //    result.ApplicationID = param.ApplicationID;
        //    result.UserID = dbUserData[0].UserID;
        //    result.Password = dbUserData[0].Password;
        //    result.UserName = dbUserData[0].FullName;
        //    result.Email = dbUserData[0].Email;
        //    result.Role = dbUserData[0].Role;
        //}

        public async Task<AuthModel?> GetByUserIdAsync(string userId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<AuthModel>(
                "sp_WEB_Auth_GetDataUser",
                new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure
            );

            //using var db = _connectionFactory.CreateConnection();
            //return await db.QueryFirstOrDefaultAsync<AuthModel>(
            //    "SELECT RTRIM(User_Id) as UserId,RTRIM(User_Name) UserName FROM User_Setup WHERE User_Id = @userId",
            //    new { userId }
            //);
        }


        //sp_WEB_GetEncryptedPassword
        //public async Task<UserDto?> LoginAsync(string userId, string password)
        //{
        //    using var conn = _connectionFactory.CreateConnection();

        //    return await conn.QueryFirstOrDefaultAsync<UserDto>(
        //        "sp_SS_User_CheckUser",
        //        new
        //        {
        //            UserId = userId,
        //            Password = password
        //        },
        //        commandType: CommandType.StoredProcedure
        //    );
        //}

        public async Task<string?> GetEncryptedPasswordAsync(string userId)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<string>(
                "sp_WEB_GetEncryptedPassword",
                new
                {
                    UserId = userId
                },
                commandType: CommandType.StoredProcedure
            );
        }

        //public async Task<string?> GetEncryptedPasswordAsync(string userId)
        //{
        //    const string sql = @"
        //    SELECT Password
        //    FROM User_Setup
        //    WHERE LOWER(User_ID) = @UserId
        //    ";

        //    using var cn = _connectionFactory.CreateConnection();
        //    return await cn.ExecuteScalarAsync<string?>(sql, new { UserId = userId });
        //}
        //public async Task<AuthModel?> GetUserDataFromDBByIDAndPassword(Login param)
        //{
        //    AuthModel result = new();

        //    param.Password = CryptHelper.PasswordEncrypt(param.Password, param.UserId);
        //    //List<SSUserSetupModel> dbUserData = (await _userData.GetUserByIdAndPassword(param)).ToList();

        //    using var db = _connectionFactory.CreateConnection();
        //    var dbUserData = await db.QueryFirstOrDefaultAsync<AuthModel>(
        //        "sp_SS_User_CheckUser",
        //        new
        //        {
        //            UserId = param.UserId,
        //            Password = param.Password
        //        },
        //        commandType: CommandType.StoredProcedure
        //    );

        //    if (dbUserData.UserId !="")
        //    {
        //        result.UserName = dbUserData.UserName;
        //        result.GroupUser = dbUserData.GroupUser;
        //        result.Locked = dbUserData.Locked;
        //        result.GroupUser = dbUserData.GroupUser;

        //    }

        //    return result;
        //}

        public async Task<bool> ChangePasswordAsync(
          string userId,
          string oldPassword,
          string newPassword)
        {
            using var db = _connectionFactory.CreateConnection();

            var oldHash = _hashService.Hash(oldPassword);
            var newHash = _hashService.Hash(newPassword);

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@OldHash", oldHash);
            parameters.Add("@NewHash", newHash);
            parameters.Add("@IsSuccess", dbType: DbType.Boolean, direction: ParameterDirection.Output);

            await db.ExecuteAsync(
                "sp_User_ChangePassword",
                parameters,
                commandType: CommandType.StoredProcedure);

            return parameters.Get<bool>("@IsSuccess");
        }

        public async Task<DateTime?> CheckExpiredPassword(string userId)
        {
            const string sql = @"
            SELECT Password_Expired
            FROM User_Setup
            WHERE LOWER(User_ID) = @UserId
            ";

            using var cn = _connectionFactory.CreateConnection();
            return await cn.ExecuteScalarAsync<DateTime?>(sql, new { UserId = userId });
        }


        
        public async Task<EmailLoginInfoDto?> GetByTokenAsync(Guid token, string type)
        {
            using var conn = _connectionFactory.CreateConnection();

            return await conn.QueryFirstOrDefaultAsync<EmailLoginInfoDto>(
                "sp_WEB_TaskListRemaining_Link",
                new
                {
                    RedirectKey = token,
                    Type = type
                },
                commandType: CommandType.StoredProcedure
            );

            //using var conn = _connectionFactory.CreateConnection();

            //return await conn.QueryFirstOrDefaultAsync<EmailLoginInfoDto>(
            //    @"
            //        SELECT
            //            EmailToken,
            //            UserID,
            //            RedirectAction,
            //            Payload,
            //            IsValid,
            //            ISNULL(RedirectCount,0) AS RedirectCount,
            //            RedirectDate
            //        FROM SendEmail_Url
            //        WHERE EmailToken = @Token
            //        ",
            //    new
            //    {
            //        Token = token
            //    });
        }

        public async Task<bool?> MarkEmailLinkVisitedAsync(Guid token)
        {
            using var conn = _connectionFactory.CreateConnection();

            //var affectedRows =
            //    await conn.ExecuteAsync(
            //    @"
            //        UPDATE SendEmail_Url
            //        SET
            //            RedirectCount =
            //                ISNULL(RedirectCount,0) + 1,
            //            RedirectDate = GETDATE()
            //        WHERE EmailToken = @Token
            //        ",
            //    new
            //    {
            //        Token = token
            //    });

            var affectedRows = await conn.ExecuteAsync(
               "sp_WEB_SendEmail_UpdateCount",
               new
               {
                   RedirectKey = token
               },
               commandType: CommandType.StoredProcedure);

            return affectedRows > 0;
        }

       
    }

}
