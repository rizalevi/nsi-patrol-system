using System.Security.Cryptography;
using System.Text;
using NSI.Domain.Interfaces;

namespace NSI.Infra.Services
{
    public class HashService : IHashService
    {
        public string Hash(string input)
        {
            using var sha = SHA256.Create();
            return Convert.ToHexString(
                sha.ComputeHash(Encoding.UTF8.GetBytes(input))
            );
        }
    }
}
