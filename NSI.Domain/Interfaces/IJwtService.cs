using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(string userId);
    }
}
