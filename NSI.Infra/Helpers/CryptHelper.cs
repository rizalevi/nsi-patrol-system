using Jose;
using System.Security.Cryptography;
using System.Text;

namespace NSI.Infra.Helpers
{
	public static class CryptHelper
	{
		private const string AES_KEY = "1234your-32-byte-long-secret-key";
		private const string AES_IV = "1your-16-byte-iv";
		private const string PASS_ENC_KEY = "Shokubai2026";
		private const string PASS_SALT = "Tosinformationsystemindonesia2026";
		private const int Iterations = 10000; // Jumlah iterasi yang disarankan untuk keamanan lebih tinggi

		//-- Aes Encryptor

		private static string GetEnvAesKey()
		{
			string key = AES_KEY; //Environment.GetEnvironmentVariable("AES_KEY");
			if (string.IsNullOrEmpty(key) || key.Length != 32) // Pastikan key memiliki panjang 32 byte
			{
				throw new InvalidOperationException("AES_KEY must be 32 bytes long.");
			}
			return key;
		}

		private static string GetEnvAesIv()
		{
			string iv = AES_IV; //Environment.GetEnvironmentVariable("AES_IV");
			if (string.IsNullOrEmpty(iv) || iv.Length != 16) // Pastikan IV memiliki panjang 16 byte
			{
				throw new InvalidOperationException("AES_IV must be 16 bytes long.");
			}
			return iv;
		}

		public static string AesEncrypt(string plainText)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Encoding.UTF8.GetBytes(GetEnvAesKey());
				aesAlg.IV = Encoding.UTF8.GetBytes(GetEnvAesIv());

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msEncrypt = new())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
					return Convert.ToBase64String(msEncrypt.ToArray());
				}
			}
		}

		public static string AesDecrypt(string cipherText)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Encoding.UTF8.GetBytes(GetEnvAesKey());
				aesAlg.IV = Encoding.UTF8.GetBytes(GetEnvAesIv());

				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
				using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText)))
				using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				using (StreamReader srDecrypt = new StreamReader(csDecrypt))
				{
					return srDecrypt.ReadToEnd();
				}
			}
		}

		public static void AesEncryptFile(string inputFilePath, string outputFilePath)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Convert.FromBase64String(GetEnvAesKey());  // AES Key (32 bytes for AES-256)
				aesAlg.IV = Convert.FromBase64String(GetEnvAesIv());    // AES IV (16 bytes)

				using (FileStream fsOutput = new FileStream(outputFilePath, FileMode.Create))
				using (CryptoStream cs = new CryptoStream(fsOutput, aesAlg.CreateEncryptor(), CryptoStreamMode.Write))
				using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
				{
					fsInput.CopyTo(cs);  // Enkripsi file input ke file output
				}
			}
		}

		public static void AesDecryptFile(string inputFilePath, string outputFilePath)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Convert.FromBase64String(GetEnvAesKey());  // AES Key (32 bytes for AES-256)
				aesAlg.IV = Convert.FromBase64String(GetEnvAesIv());    // AES IV (16 bytes)

				using (FileStream fsOutput = new FileStream(outputFilePath, FileMode.Create))
				using (CryptoStream cs = new CryptoStream(fsOutput, aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
				using (FileStream fsInput = new FileStream(inputFilePath, FileMode.Open))
				{
					fsInput.CopyTo(cs);  // Dekripsi file input ke file output
				}
			}
		}

		public static byte[] AesEncryptBlob(byte[] blobData)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Convert.FromBase64String(GetEnvAesKey());  // AES Key (32 bytes for AES-256)
				aesAlg.IV = Convert.FromBase64String(GetEnvAesIv());    // AES IV (16 bytes)

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						csEncrypt.Write(blobData, 0, blobData.Length);
						csEncrypt.FlushFinalBlock();
					}
					return msEncrypt.ToArray();
				}
			}
		}

		public static byte[] AesDecryptBlob(byte[] encryptedBlobData)
		{
			using (Aes aesAlg = Aes.Create())
			{
				aesAlg.Key = Convert.FromBase64String(GetEnvAesKey());  // AES Key (32 bytes for AES-256)
				aesAlg.IV = Convert.FromBase64String(GetEnvAesIv());    // AES IV (16 bytes)

				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msDecrypt = new MemoryStream(encryptedBlobData))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (MemoryStream msResult = new MemoryStream())
						{
							csDecrypt.CopyTo(msResult);
							return msResult.ToArray();
						}
					}
				}
			}
		}

		//-- Common Encryptor

		public static string HashString(string text)
		{
			byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));
			StringBuilder builder = new();
			foreach (byte b in bytes)
			{
				builder.Append(b.ToString("x2"));
			}
			return builder.ToString();
		}

		//-- Base64 Encryptor

		public static string Base64Encode(string plainText)
		{
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			return Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64Decode(string base64EncodedData)
		{
			var base64Bytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(base64Bytes);
		}

		//-- Jwt Encryptor

		public static string JwtEncrypt(string originalJwt, string encryptionKey)
		{
			var parts = originalJwt.Split('.');
			if (parts.Length != 3) throw new ArgumentException("Invalid JWT format.");

			var header = parts[0];
			var payload = parts[1];

			byte[] key = Encoding.UTF8.GetBytes(encryptionKey);

			var encryptedJwt = JWT.Encode(payload, key, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512);

			return $"{header}.{encryptedJwt}";
		}

		public static string JwtDecrypt(string encryptedJwt, string encryptionKey)
		{
			var parts = encryptedJwt.Split('.');
			if (parts.Length != 2) throw new ArgumentException("Invalid encrypted JWT format.");

			var header = parts[0];
			var encryptedPayload = parts[1];

			byte[] key = Encoding.UTF8.GetBytes(encryptionKey);

			var decryptedPayload = JWT.Decode(encryptedPayload, key, JweAlgorithm.A256KW, JweEncryption.A256CBC_HS512);

			return $"{header}.{decryptedPayload}";
		}

		//-- Password Encryptor

		
		public static string PasswordEncrypt(string clearText, string EncryptionKey)
		{
			EncryptionKey += PASS_ENC_KEY;
			byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, Encoding.UTF8.GetBytes(PASS_SALT));
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
					{
						cs.Write(clearBytes, 0, clearBytes.Length);
						cs.Close();
					}
					clearText = Convert.ToBase64String(ms.ToArray());
				}
			}
			return clearText;
		}

		public static string PasswordDecrypt(string cipherText, string EncryptionKey)
		{
			if (cipherText.Length < 5)
			{
				return cipherText;
			}
			EncryptionKey += PASS_ENC_KEY;
			byte[] cipherBytes = Convert.FromBase64String(cipherText);
			using (Aes encryptor = Aes.Create())
			{
				Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, Encoding.UTF8.GetBytes(PASS_SALT));
				encryptor.Key = pdb.GetBytes(32);
				encryptor.IV = pdb.GetBytes(16);
				using (MemoryStream ms = new MemoryStream())
				{
					using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
					{
						cs.Write(cipherBytes, 0, cipherBytes.Length);
						cs.Close();
					}
					cipherText = Encoding.Unicode.GetString(ms.ToArray());
				}
			}
			return cipherText;
		}

		//eof
	}
}