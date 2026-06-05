using NSI.Domain.DTOs.Security;
using NSI.Domain.Models.Security;

namespace NSI.Domain.Interfaces.Security
{
    public interface IChangePasswordRepository
    {
        Task Save(ChangePassword model);

    }
}
