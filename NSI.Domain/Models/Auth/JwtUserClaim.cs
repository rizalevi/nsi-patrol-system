using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSI.Domain.Models.Auth
{
    internal class JwtUserClaim
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
