using System.Text.RegularExpressions;

namespace NSI.Infra.Helpers
{
	public static class CleanHelper
	{
		public static string GetCleanFileName(string fileName)
		{
			if (string.IsNullOrEmpty(fileName))
			{
				return "default_filename"; // Jika nama file kosong, berikan nama default
			}

			// Menghapus spasi ekstra di awal dan akhir
			fileName = fileName.Trim();

			// Ganti spasi dengan underscore
			fileName = fileName.Replace(" ", "_");

			// Hapus karakter yang tidak diizinkan dalam nama file (Windows dan beberapa sistem file lainnya)
			fileName = Regex.Replace(fileName, @"[\\\/:*?""<>|]", "");

			// Hapus titik berturut-turut dan pastikan tidak ada titik di awal atau akhir
			fileName = Regex.Replace(fileName, @"^[\.]+|[\.]+$", ""); // Menghapus titik di awal/akhir
			fileName = Regex.Replace(fileName, @"\.\.+", "."); // Ganti dua atau lebih titik berturut-turut menjadi satu titik

			// Jika nama file menjadi kosong setelah pembersihan, beri nama default
			if (string.IsNullOrEmpty(fileName))
			{
				fileName = "default_filename";
			}

			return fileName;
		}

		//eof
	}
}
