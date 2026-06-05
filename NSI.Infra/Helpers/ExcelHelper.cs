using ClosedXML.Excel;

namespace NM9Infra.Helpers.Global
{
	public static class ExcelHelper
	{

		/*usage:
			MergeSameCells("inputFile.xlsx", "outputFile.xlsx", "A1:A500");
		*/
		//public static void MergeSameCells(string fileFromPath, string fileToPath, string range)
		//{
		//	// Memuat workbook dari path sumber
		//	using (var workbook = new XLWorkbook(fileFromPath))
		//	{
		//		var worksheet = workbook.Worksheet(1);  // Mengakses sheet pertama (bisa diubah sesuai kebutuhan)

		//		// Mengakses range yang diberikan
		//		var cellRange = worksheet.Range(range);
		//		string currentValue = string.Empty;
		//		int startRow = 1;

		//		// Loop untuk memeriksa dan menggabungkan sel yang memiliki nilai yang sama
		//		for (int row = 1; row <= cellRange.RowCount(); row++)
		//		{
		//			var cellValue = cellRange.Cell(row, 1).Value.ToString();

		//			// Jika nilai berubah, gabungkan sel sebelumnya yang memiliki nilai yang sama
		//			if (cellValue != currentValue && row > startRow)
		//			{
		//				var mergeRange = worksheet.Range(startRow, 1, row - 1, 1);  // Rentang yang akan digabung
		//				mergeRange.Merge();
		//			}

		//			// Perbarui nilai dan baris awal untuk perbandingan berikutnya
		//			currentValue = cellValue;
		//			startRow = row;
		//		}

		//		// Setelah loop, gabungkan sel yang tersisa yang memiliki nilai yang sama
		//		if (startRow < cellRange.RowCount())
		//		{
		//			var mergeRange = worksheet.Range(startRow, 1, cellRange.RowCount(), 1);
		//			mergeRange.Merge();
		//		}

		//		// Simpan workbook yang telah dimodifikasi ke file tujuan
		//		workbook.SaveAs(fileToPath);
		//	}

		//	Console.WriteLine($"File berhasil disimpan ke: {fileToPath}");
		//}

		//public static void MergeSameCells(string fileFromPath, string fileToPath, string range)
		//{
		//	// Memuat workbook dari path sumber
		//	using (var workbook = new XLWorkbook(fileFromPath))
		//	{
		//		var worksheet = workbook.Worksheet(1);  // Mengakses sheet pertama (bisa diubah sesuai kebutuhan)

		//		// Mengakses range yang diberikan
		//		var cellRange = worksheet.Range(range);
		//		string currentValue = string.Empty;
		//		int startRow = 1;

		//		// Loop untuk memeriksa dan menggabungkan sel yang memiliki nilai yang sama
		//		for (int row = 1; row <= cellRange.RowCount(); row++)
		//		{
		//			var cellValue = cellRange.Cell(row, 1).Value.ToString();

		//			// Jika nilai berubah atau pada baris terakhir, gabungkan sel sebelumnya yang memiliki nilai yang sama
		//			if (cellValue != currentValue && row > startRow)
		//			{
		//				var mergeRange = worksheet.Range(startRow, 1, row - 1, 1);  // Rentang yang akan digabung
		//				mergeRange.Merge();
		//			}

		//			// Perbarui nilai dan baris awal untuk perbandingan berikutnya
		//			currentValue = cellValue;
		//			startRow = row;
		//		}

		//		// Setelah loop, gabungkan sel yang tersisa yang memiliki nilai yang sama
		//		if (startRow < cellRange.RowCount())
		//		{
		//			var mergeRange = worksheet.Range(startRow, 1, cellRange.RowCount(), 1);
		//			mergeRange.Merge();
		//		}

		//		// Simpan workbook yang telah dimodifikasi ke file tujuan
		//		workbook.SaveAs(fileToPath);
		//	}

		//	Console.WriteLine($"File berhasil disimpan ke: {fileToPath}");
		//}

		// Metode untuk melakukan merge pada sel yang memiliki nilai yang sama
		public static void MergeSameCells(string fileFromPath, string fileToPath, string range)
		{
			// Memuat workbook dari path sumber
			using (var workbook = new XLWorkbook(fileFromPath))
			{
				var worksheet = workbook.Worksheet(1);  // Mengakses sheet pertama (bisa diubah sesuai kebutuhan)

				// Mengakses range yang diberikan
				var cellRange = worksheet.Range(range);

				string currentValue = string.Empty;
				int startRow = 1;

				// Memastikan bahwa kita hanya melakukan penggabungan berdasarkan jumlah baris data yang sebenarnya
				int lastRow = worksheet.LastRowUsed().RowNumber();  // Mendapatkan baris terakhir yang terpakai

				// Loop untuk memeriksa dan menggabungkan sel yang memiliki nilai yang sama
				for (int row = 1; row <= lastRow; row++)
				{
					var cellValue = cellRange.Cell(row, 1).Value.ToString();

					// Jika nilai berubah atau pada baris terakhir, gabungkan sel sebelumnya yang memiliki nilai yang sama
					if (cellValue != currentValue && row > startRow)
					{
						var mergeRange = worksheet.Range(startRow, 1, row - 1, 1);  // Rentang yang akan digabung
						mergeRange.Merge();
					}

					// Perbarui nilai dan baris awal untuk perbandingan berikutnya
					currentValue = cellValue;
					startRow = row;
				}

				// Setelah loop, gabungkan sel yang tersisa yang memiliki nilai yang sama
				if (startRow < lastRow)
				{
					var mergeRange = worksheet.Range(startRow, 1, lastRow, 1);
					mergeRange.Merge();
				}

				// Simpan workbook yang telah dimodifikasi ke file tujuan
				workbook.SaveAs(fileToPath);
			}

			Console.WriteLine($"File berhasil disimpan ke: {fileToPath}");
		}

		public static void MergeSameCellsV2(string fileFromPath, string fileToPath, string rangeStartEnd = "A9:A") //rangestart diitung dari kolom headernya sampai kolom end nya dimana
		{
			using (var workbook = new XLWorkbook(fileFromPath))
			{
				var worksheet = workbook.Worksheet(1);  // Mengakses sheet pertama, bisa disesuaikan

				// Menghitung LastRow berdasarkan data yang ada di worksheet
				int lastRow = worksheet.LastRowUsed().RowNumber();

				// Mendapatkan data dari range yang dimulai dari parameter rangeStart
				var range = worksheet.Range(rangeStartEnd + lastRow);  // Asumsi kolom A
				var values = range.Cells().Select(c => c.Value.ToString()).ToArray();

				int i = rangeStartEnd[1] - '0';  // Menentukan baris mulai dari parameter rangeStart (misal A1 menjadi 1)
				while (i < values.Length)
				{
					// Mencari rentang yang memiliki nilai yang sama
					if (values[i - 1] == values[i])
					{
						int k = 0;

						// Tentukan berapa banyak baris yang harus digabungkan
						while (i + k < values.Length && values[i + k] == values[i - 1])
						{
							k++;
						}

						// Gabungkan sel di rentang yang relevan
						for (int col = 1; col <= 12; col++)  // Bisa disesuaikan dengan jumlah kolom yang ingin digabungkan
						{
							worksheet.Range(worksheet.Cell(i, col), worksheet.Cell(i + k - 1, col)).Merge();
						}

						// Sesuaikan perataan vertikal untuk semua baris yang digabungkan
						worksheet.Range(worksheet.Cell(i, 1), worksheet.Cell(i + k - 1, 12)).Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

						// Increment i untuk melanjutkan ke baris berikutnya setelah penggabungan
						i += k;
					}
					else
					{
						i++;
					}
				}

				// Simpan workbook yang telah dimodifikasi ke file tujuan
				workbook.SaveAs(fileToPath);
			}

			Console.WriteLine("Proses selesai. File berhasil disimpan.");
		}

		//eof
	}
}
