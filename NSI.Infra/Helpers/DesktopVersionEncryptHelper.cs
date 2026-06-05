using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace NSI.Infra.Helpers
{
    public class DesktopVersionEncryptHelper
    {
        private readonly TripleDESCryptoServiceProvider _tripleDes;

        public DesktopVersionEncryptHelper(string key)
        {
            _tripleDes = new TripleDESCryptoServiceProvider();

            _tripleDes.Key = TruncateHash(key, _tripleDes.KeySize / 8);
            _tripleDes.IV = TruncateHash("", _tripleDes.BlockSize / 8);
        }

        private byte[] TruncateHash(string key, int length)
        {
            using (SHA1 sha1 = SHA1.Create())
            {
                byte[] keyBytes = Encoding.Unicode.GetBytes(key);
                byte[] hash = sha1.ComputeHash(keyBytes);

                Array.Resize(ref hash, length);

                return hash;
            }
        }

        public string EncryptData(string plaintext)
        {
            byte[] plaintextBytes = Encoding.Unicode.GetBytes(plaintext);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream encStream = new CryptoStream(
                    ms,
                    _tripleDes.CreateEncryptor(),
                    CryptoStreamMode.Write))
                {
                    encStream.Write(plaintextBytes, 0, plaintextBytes.Length);
                    encStream.FlushFinalBlock();
                }

                return Convert.ToBase64String(ms.ToArray());
            }
        }

        public string DecryptData(string encryptedText)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedText);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream decStream = new CryptoStream(
                    ms,
                    _tripleDes.CreateDecryptor(),
                    CryptoStreamMode.Write))
                {
                    decStream.Write(encryptedBytes, 0, encryptedBytes.Length);
                    decStream.FlushFinalBlock();
                }

                return Encoding.Unicode.GetString(ms.ToArray());
            }
        }
    }
}
