
using NSI.Domain.DTOs.Logger;
using NSI.Domain.Models.Logger;

namespace NSI.Domain.Interfaces.Logger
{
    public interface IErrorLogRepository
    {
        Task LogAsync(ErrorLogModel log);
        Task<IEnumerable<ErrorDto>> GetAll();
        Task<IEnumerable<ErrorDto>> Search(string keyword);
    }
}
