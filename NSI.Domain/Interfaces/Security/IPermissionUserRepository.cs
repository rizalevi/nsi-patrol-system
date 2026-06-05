using NSI.Domain.DTOs.Security;
namespace NSI.Domain.Interfaces.Security
{
    public interface IPermissionUserRepository
    {
        Task<PermissionDto?> GetPermission(PermissionParamDto model);
    }
}
