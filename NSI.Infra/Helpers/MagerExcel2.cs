using ClosedXML.Excel;
using System.Reflection;

namespace NSI.Infra.Helpers
{
	public class MagerExcel2 : IDisposable
	{
		public class CellResult
		{
			public int Row { get; set; }
			public int Col { get; set; }
		}

		public IXLWorksheet ws;
		public int rowStart;
		public int colStart;

		public MagerExcel2(IXLWorksheet _ws)
		{
			ws = _ws;
			rowStart = 0;
			colStart = 0;
		}

		// Constructor dengan parameter columnWidth
		public MagerExcel2(IXLWorksheet _ws, double columnWidth = 8.43)
		{
			ws = _ws;
			rowStart = 1; // ClosedXML menggunakan indeks 1 untuk baris pertama
			colStart = 1; // ClosedXML menggunakan indeks 1 untuk kolom pertama

			ws.Columns().Width = columnWidth; // Mengatur lebar seluruh kolom
		}

		// Constructor dengan parameter columnWidth dan rowHeight
		public MagerExcel2(IXLWorksheet _ws, double columnWidth = 8.43, double rowHeight = 15)
		{
			ws = _ws;
			rowStart = 1; // ClosedXML menggunakan indeks 1 untuk baris pertama
			colStart = 1; // ClosedXML menggunakan indeks 1 untuk kolom pertama

			ws.Columns().Width = columnWidth; // Mengatur lebar seluruh kolom
			ws.Rows().Height = rowHeight; // Mengatur tinggi seluruh baris
		}

		public enum BorderType
		{
			NoBorderAll,
			NoBorderAround,
			BorderAroundThin,
			BorderAroundThick,
			BorderAllThin,
			BorderAllThick,
			BorderAroundDotted,
			BorderAroundDashed,
			BorderAllDotted,
			BorderAllDashed,
			BorderBottomThin,
			BorderBottomThick,
			BorderBottomDotted,
			BorderBottomDashed,
			BorderBottomDouble,
			BorderAllDouble,
			BorderAroundDouble
		}

		public enum BgColorType
		{
			Odd,
			Even,
			All
		}

		public void DrawAdjustedValue(object? propertyValue, int row, int col)
		{
			if (propertyValue is string)
			{
				ws.Cell(row, col).Value = (string)propertyValue;
			}
			else if (propertyValue is double)
			{
				ws.Cell(row, col).Value = (double)propertyValue;
			}
			else if (propertyValue is int)
			{
				ws.Cell(row, col).Value = (int)propertyValue;
			}
			else if (propertyValue is DateTime)
			{
				ws.Cell(row, col).Value = (DateTime)propertyValue;
			}
			else
			{
				// Jika tipe tidak diketahui, Anda bisa langsung menetapkan objek apapun
				ws.Cell(row, col).Value = propertyValue?.ToString();
			}
		}

		//-------------------------------------------------------------- ROW / COLUMN SETTING -----------------------------------------------------------------------

		public void SetColWidth(int col, double width)
		{
			ws.Column(col).Width = width;
		}

		public void SetColWidthByRange(int col, int colTo, double width)
		{
			while (col <= colTo)
			{
				ws.Column(col).Width = width;
				col++;
			}
		}

		public void SetRowHeight(int row, double height)
		{
			ws.Row(row).Height = height;
		}

		public void SetRowHeightByRange(int row, int rowTo, double height)
		{
			while (row <= rowTo)
			{
				ws.Row(row).Height = height;
				row++;
			}
		}

		//-------------------------------------------------------------- BOLD ONLY ----------------------------------------------------------------------------------

		public CellResult Bold(int row, int col)
		{
			try
			{
				// Menggunakan ClosedXML untuk mengatur teks menjadi tebal (bold) pada cell tertentu
				ws.Cell(row, col).Style.Font.Bold = true;
				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[Bold] row({row}) column({col})");
			}
		}

		public CellResult Bold(int row, int col, int rowTo, int colTo)
		{
			try
			{
				// Menggunakan ClosedXML untuk mengatur teks menjadi tebal (bold) pada rentang cell
				ws.Range(row, col, rowTo, colTo).Style.Font.Bold = true;
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[Bold] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- MERGE ONLY ----------------------------------------------------------------------------------

		public CellResult Merge(int row, int col, int rowTo, int colTo)
		{
			try
			{
				// Menggunakan ClosedXML untuk melakukan merge sel
				ws.Range(row, col, rowTo, colTo).Merge();
				ws.Range(row, col, rowTo, colTo).Style.Alignment.WrapText = true;
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[Merge] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- ALIGNMENT ----------------------------------------------------------------------------------
		
		public CellResult SetAlign(int row, int col, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				// Menggunakan ClosedXML untuk mengatur vertikal dan horizontal alignment pada sel
				ws.Cell(row, col).Style.Alignment.Vertical = vAlign;
				ws.Cell(row, col).Style.Alignment.Horizontal = hAlign;
				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[SetAlign] row({row}) column({col})");
			}
		}

		public CellResult SetAlign(int row, int col, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, int rowTo, int colTo)
		{
			try
			{
				// Menggunakan ClosedXML untuk mengatur vertikal dan horizontal alignment pada rentang sel
				ws.Range(row, col, rowTo, colTo).Style.Alignment.Vertical = vAlign;
				ws.Range(row, col, rowTo, colTo).Style.Alignment.Horizontal = hAlign;
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[SetAlign] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- TRANSPOSE / TABLE RIGHT -------------------------------------------------------------------

		public CellResult DrawTableRightMerge(int row, int col, List<object> dataList)
		{
			if (dataList.Count > 0)
			{
				try
				{
					rowStart = row;
					int rowLast = 0;

					foreach (object data in dataList)
					{
						Type dataType = data.GetType();
						PropertyInfo[] properties = dataType.GetProperties();

						foreach (PropertyInfo property in properties)
						{
							object? propertyValue = property.GetValue(data);
							//ws.Cell(row, col).Value = propertyValue;
							DrawAdjustedValue(propertyValue, row, col);

							ws.Cell(row, col).Style.Alignment.WrapText = true; // Wrap text
							row++;
						}
						col++;
						rowLast = row > rowLast ? row : rowLast;
						row = rowStart;
					}
					return new CellResult { Row = rowLast, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawTableRightMerge] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawTableRightMerge] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawTableRightMerge(int row, int col, List<object> dataList, int rowMerge, int colMerge)
		{
			if (dataList.Count > 0)
			{
				try
				{
					rowMerge = rowMerge < 1 ? 1 : rowMerge;
					colMerge = colMerge < 1 ? 1 : colMerge;
					rowStart = row;
					colStart = col;
					int rowLast = 0;

					foreach (object data in dataList)
					{
						Type dataType = data.GetType();
						PropertyInfo[] properties = dataType.GetProperties();

						foreach (PropertyInfo property in properties)
						{
							object? propertyValue = property.GetValue(data);
							//ws.Cell(row, col).Value = propertyValue;
							DrawAdjustedValue(propertyValue, row, col);
							
							// Merge sel pada row dan col yang diberikan
							ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge();
							ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true; // Wrap text
							row += rowMerge;
						}
						col += colMerge;
						rowLast = row > rowLast ? row : rowLast;
						row = rowStart;
					}
					return new CellResult { Row = rowLast, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawTableRightMerge] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawTableRightMerge] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawTableRightMerge(int row, int col, List<object> dataList, int rowMerge, int colMerge, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				CellResult result = DrawTableRightMerge(row, col, dataList, rowMerge, colMerge);
				return SetAlign(row, col, vAlign, hAlign, result.Row, result.Col);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTableRightMerge] row({row}) column({col})");
			}
		}

		public CellResult DrawTableRightMerge(int row, int col, List<object> dataList, int rowMerge, int colMerge, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, BorderType borderStyle)
		{
			try
			{
				CellResult result = DrawTableRightMerge(row, col, dataList, rowMerge, colMerge);
				result = SetAlign(row, col, vAlign, hAlign, result.Row, result.Col);
				return DrawBorderByType(row, col, result.Row - 1, result.Col - 1, borderStyle);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTableRightMerge] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- HEADER RIGHT -------------------------------------------------------------------------------

		public CellResult DrawHeaderRight(int row, int col, object obj)
		{
			if (obj != null)
			{
				try
				{
					Type type = obj.GetType();
					PropertyInfo[] properties = type.GetProperties();

					foreach (PropertyInfo property in properties)
					{
						ws.Cell(row, col).Value = property.Name;
						col++;
					}
					return new CellResult { Row = row, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawHeaderRight] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawHeaderRight] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawHeaderRight(int row, int col, object obj, int rowMerge, int colMerge)
		{
			if (obj != null)
			{
				try
				{
					rowStart = row;
					colStart = col;
					rowMerge = rowMerge < 1 ? 1 : rowMerge;
					colMerge = colMerge < 1 ? 1 : colMerge;

					Type type = obj.GetType();
					PropertyInfo[] properties = type.GetProperties();

					foreach (PropertyInfo property in properties)
					{
						ws.Cell(row, col).Value = property.Name;
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge();
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true;
						col += colMerge;
					}
					return new CellResult { Row = row, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawHeaderRight] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawHeaderRight] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawHeaderRight(int row, int col, object obj, int rowMerge, int colMerge, BorderType borderType)
		{
			try
			{
				CellResult result = DrawHeaderRight(row, col, obj, rowMerge, colMerge);
				return DrawBorderByType(rowStart, colStart, result.Row + rowMerge - 1, result.Col - 1, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawHeaderRight] row({row}) column({col})");
			}
		}

		public CellResult DrawHeaderRight(int row, int col, object obj, int rowMerge, int colMerge, BorderType borderType, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				CellResult result = DrawHeaderRight(row, col, obj, rowMerge, colMerge, borderType);
				return SetAlign(rowStart, colStart, vAlign, hAlign, result.Row + rowMerge, result.Col - 1);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawHeaderRight] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- HEADER DOWN -------------------------------------------------------------------------------

		public CellResult DrawHeaderDown(int row, int col, object obj)
		{
			if (obj != null)
			{
				try
				{
					Type type = obj.GetType();
					PropertyInfo[] properties = type.GetProperties();

					foreach (PropertyInfo property in properties)
					{
						ws.Cell(row, col).Value = property.Name;
						row++;  // Move to next row for the next property
					}
					return new CellResult { Row = row, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawHeaderDown] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawHeaderDown] object/list is NULL. row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- LIST DOWN -------------------------------------------------------------------------------

		public CellResult DrawListDown(int row, int col, List<object> dataList)
		{
			if (dataList.Count > 0)
			{
				try
				{
					foreach (var obj in dataList)
					{
						//ws.Cell(row, col).Value = obj?.ToString() ?? string.Empty;
						DrawAdjustedValue(obj, row, col);
						row++; // Move to the next row for the next data
					}
					return new CellResult { Row = row, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawListDown] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawListDown] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawListDown(int row, int col, List<object> dataList, int rowMerge, int colMerge)
		{
			if (dataList.Count > 0)
			{
				try
				{
					rowStart = row;
					colStart = col;
					rowMerge = rowMerge < 1 ? 1 : rowMerge;
					colMerge = colMerge < 1 ? 1 : colMerge;

					foreach (var obj in dataList)
					{
						//ws.Cell(row, col).Value = obj?.ToString() ?? string.Empty;
						DrawAdjustedValue(obj, row, col);
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge();
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true; // Wrap Text
						row += rowMerge;
						col = colStart;
					}
					return new CellResult { Row = row, Col = col + colMerge };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawListDown] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawListDown] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawListDown(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType)
		{
			try
			{
				CellResult result = DrawListDown(row, col, dataList, rowMerge, colMerge);
				return DrawBorderByType(rowStart, colStart, result.Row - 1, result.Col - 1, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawListDown] row({row}) column({col})");
			}
		}

		public CellResult DrawListDown(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				CellResult result = DrawListDown(row, col, dataList, rowMerge, colMerge, borderType);
				return SetAlign(rowStart, colStart, vAlign, hAlign, result.Row, result.Col);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawListDown] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- LIST RIGHT -------------------------------------------------------------------------------
		
		public CellResult DrawListRight(int row, int col, List<object> dataList)
		{
			if (dataList.Count > 0)
			{
				try
				{
					foreach (var obj in dataList)
					{
						//ws.Cell(row, col).Value = obj?.ToString() ?? string.Empty;
						DrawAdjustedValue(obj, row, col);
						col++;  // Pindah ke kolom berikutnya
					}
					return new CellResult { Row = row, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawListRight] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawListRight] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawListRight(int row, int col, List<object> dataList, int rowMerge, int colMerge)
		{
			if (dataList.Count > 0)
			{
				try
				{
					rowStart = row;
					colStart = col;
					rowMerge = rowMerge < 1 ? 1 : rowMerge;
					colMerge = colMerge < 1 ? 1 : colMerge;

					foreach (var obj in dataList)
					{
						//ws.Cell(row, col).Value = obj?.ToString() ?? string.Empty;
						DrawAdjustedValue(obj, row, col);
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge(); // Merge sel
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true; // Membungkus teks
						col += colMerge;  // Pindah ke kolom berikutnya
					}
					return new CellResult { Row = rowStart, Col = col + colMerge };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawListRight] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawListRight] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawListRight(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType)
		{
			try
			{
				CellResult result = DrawListRight(row, col, dataList, rowMerge, colMerge);
				if (rowMerge > 1)
				{
					result.Row += 1;
				}
				result = DrawBorderByType(row, col, row + rowMerge - 1, result.Col - colMerge - 1, borderType);
				return new CellResult { Row = result.Row, Col = result.Col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawListRight] row({row}) column({col})");
			}
		}

		public CellResult DrawListRight(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				CellResult result = DrawListRight(row, col, dataList, rowMerge, colMerge, borderType);
				return SetAlign(rowStart, colStart, vAlign, hAlign, result.Row, result.Col);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawListRight] row({row}) column({col})");
			}
		}

		//-------------------------------------------------------------- HEADER RIGHT FOR TABLE ----------------------------------------------------------------------

		private CellResult DrawHeaderRightForTable(int row, int col, object obj, int rowMerge, int colMerge, BorderType borderType)
		{
			if (obj != null)
			{
				try
				{
					rowStart = row;
					colStart = col;
					int rowMergeStart = rowMerge;

					Type type = obj.GetType();
					PropertyInfo[] properties = type.GetProperties();

					foreach (PropertyInfo property in properties)
					{
						ws.Cell(row, col).Value = property.Name;
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge();
						ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true;
						rowMerge = rowMergeStart;
						col += colMerge;
					}
					DrawBorderByType(rowStart, colStart, row + rowMerge, col - 1, borderType);
					return new CellResult { Row = rowStart + rowMerge, Col = colStart };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawHeaderRightForTable] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawHeaderRightForTable] object/list is NULL. row({row}) column({col})");
			}
		}

		private CellResult DrawHeaderRightForTable(int row, int col, object obj, int rowMerge, int colMerge, BorderType borderType, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				CellResult result = DrawHeaderRightForTable(row, col, obj, rowMerge, colMerge, borderType);
				SetAlign(rowStart, colStart, vAlign, hAlign, result.Row, result.Col);
				return new CellResult { Row = rowStart + rowMerge, Col = colStart };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawHeaderRightForTable] row({row}) column({col})");
			}
		}

		//----------------------------------------------------------------------- TEXT ------------------------------------------------------------------------------

		public CellResult DrawTextStyle(int row, int col, string text, int size, bool isBold, XLColor color)
		{
			try
			{
				ws.Cell(row, col).Value = text;
				ws.Cell(row, col).Style.Font.FontSize = size;  // Mengatur ukuran font
				ws.Cell(row, col).Style.Font.Bold = isBold;    // Mengatur ketebalan font
				ws.Cell(row, col).Style.Font.SetFontColor(color);

				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTextStyle] row({row}) column({col})");
			}
		}

		public CellResult DrawTextStyle(int row, int col, string text, int size, bool isBold, XLColor color, int rowTo, int colTo, BorderType borderType, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				// Menggambar teks dengan gaya font yang sudah ditentukan
				DrawTextStyle(row, col, text, size, isBold, color);  // Panggil metode DrawTextStyle sebelumnya

				// Menyatu (merge) sel sesuai dengan rentang yang diberikan
				var range = ws.Range(ws.Cell(row, col), ws.Cell(rowTo, colTo));
				range.Merge();  // Menyatukan sel
				range.Style.Alignment.WrapText = true;  // Menyetting wrap text

				// Menggambar border
				DrawBorderByType(row, col, rowTo, colTo, borderType);

				// Mengatur alignement untuk teks
				SetAlign(row, col, vAlign, hAlign, rowTo, colTo);

				// Mengembalikan posisi sel yang sudah diperbarui
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTextStyle] row({rowTo}) column({colTo})");
			}
		}

		public CellResult DrawText(int row, int col, string text)
		{
			try
			{
				// Menetapkan nilai teks ke sel yang sesuai di worksheet
				ws.Cell(row, col).Value = text;

				// Mengembalikan hasil dengan posisi baris dan kolom
				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				// Menangani error dengan melemparkan exception baru
				throw new Exception($"{e.Message} \n[DrawText] row({row}) column({col})");
			}
		}

		public CellResult DrawText(int row, int col, string text, int rowTo, int colTo)
		{
			try
			{
				// Menetapkan nilai teks ke sel yang sesuai di worksheet
				ws.Cell(row, col).Value = text;

				// Menggabungkan sel dari row, col ke rowTo, colTo
				ws.Range(row, col, rowTo, colTo).Merge();

				// Mengaktifkan fitur wrap text pada sel yang digabungkan
				ws.Range(row, col, rowTo, colTo).Style.Alignment.WrapText = true;

				// Mengembalikan hasil dengan posisi baris dan kolom tujuan
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				// Menangani error dengan melemparkan exception baru
				throw new Exception($"{e.Message} \n[DrawText] row({row}) column({col})");
			}
		}

		public CellResult DrawText(int row, int col, string text, int rowTo, int colTo, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				DrawText(row, col, text, rowTo, colTo);
				SetAlign(row, col, vAlign, hAlign, rowTo, colTo);
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawText] row({rowTo}) column({colTo})");
			}
		}

		// Konversi untuk DrawText dengan pengaturan border, alignment, dan tanpa Bold
		public CellResult DrawText(int row, int col, string text, int rowTo, int colTo, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, BorderType borderType)
		{
			try
			{
				// Memanggil metode DrawText untuk menetapkan teks ke sel dan menggabungkan sel
				DrawText(row, col, text, rowTo, colTo);

				// Mengatur alignment vertikal dan horizontal
				var range = ws.Range(row, col, rowTo, colTo);
				range.Style.Alignment.Vertical = vAlign;
				range.Style.Alignment.Horizontal = hAlign;

				// Menambahkan border sesuai tipe
				DrawBorderByType(row, col, rowTo, colTo, borderType);

				// Mengembalikan hasil dengan posisi sel yang digabungkan
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				// Menangani error dengan melemparkan exception baru
				throw new Exception($"{e.Message} \n[DrawText] row({rowTo}) column({colTo})");
			}
		}

		// Konversi untuk DrawText dengan pengaturan border, alignment, dan Bold
		public CellResult DrawText(int row, int col, string text, int rowTo, int colTo, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, BorderType borderType, bool isBold)
		{
			try
			{
				// Memanggil metode DrawText untuk menetapkan teks ke sel dan menggabungkan sel
				DrawText(row, col, text, rowTo, colTo, vAlign, hAlign);

				// Menetapkan teks menjadi Bold
				ws.Cell(row, col).Style.Font.Bold = isBold;

				// Menambahkan border sesuai tipe
				return DrawBorderByType(row, col, rowTo, colTo, borderType);
			}
			catch (Exception e)
			{
				// Menangani error dengan melemparkan exception baru
				throw new Exception($"{e.Message} \n[DrawText] row({rowTo}) column({colTo})");
			}
		}

		//wakwaw
		//----------------------------------------------------------------------- DRAW OBJECT ------------------------------------------------------------------------------

		public CellResult DrawObjectStyle(int row, int col, object val, int size, bool isBold, XLColor color)
		{
			try
			{
				//ws.Cell(row, col).Value = val.ToString();
				DrawAdjustedValue(val, row, col);
				ws.Cell(row, col).Style.Font.FontSize = size;
				ws.Cell(row, col).Style.Font.Bold = isBold;
				ws.Cell(row, col).Style.Font.SetFontColor(color);

				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObjectStyle] row({row}) column({col})");
			}
		}

		public CellResult DrawObjectStyle(int row, int col, object val, int size, bool isBold, XLColor color, int rowTo, int colTo, BorderType borderType,
			XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				DrawObjectStyle(row, col, val, size, isBold, color);
				ws.Range(row, col, rowTo, colTo).Merge();  // Menggabungkan cell
				ws.Range(row, col, rowTo, colTo).Style.Alignment.WrapText = true; // Mengatur wrapping text

				DrawBorderByType(row, col, rowTo, colTo, borderType);
				SetAlign(row, col, vAlign, hAlign, rowTo, colTo); // Menetapkan alignment

				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObjectStyle] row({rowTo}) column({colTo})");
			}
		}

		public CellResult DrawObject(int row, int col, object val)
		{
			try
			{
				//ws.Cell(row, col).Value = val.ToString();
				DrawAdjustedValue(val, row, col);
				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObject] row({row}) column({col})");
			}
		}

		public CellResult DrawObject(int row, int col, object val, int rowTo, int colTo)
		{
			try
			{
				// Menetapkan nilai ke cell
				//ws.Cell(row, col).Value = val;
				DrawAdjustedValue(val, row, col);
				// Menggabungkan cell dari (row, col) ke (rowTo, colTo)
				ws.Range(row, col, rowTo, colTo).Merge();

				// Mengatur agar teks di cell dibungkus (wrap text)
				ws.Range(row, col, rowTo, colTo).Style.Alignment.WrapText = true;

				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObject] row({row}) column({col})");
			}
		}


		public CellResult DrawObject(int row, int col, object val, int rowTo, int colTo, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				DrawObject(row, col, val, rowTo, colTo);
				SetAlign(row, col, vAlign, hAlign, rowTo, colTo);
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObject] row({rowTo}) column({colTo})");
			}
		}

		public CellResult DrawObject(int row, int col, object val, int rowTo, int colTo, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, BorderType borderType)
		{
			try
			{
				DrawObject(row, col, val, rowTo, colTo, vAlign, hAlign);
				return DrawBorderByType(row, col, rowTo, colTo, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObject] row({rowTo}) column({colTo})");
			}
		}

		public CellResult DrawObject(int row, int col, object val, int rowTo, int colTo, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, BorderType borderType, bool isBold)
		{
			try
			{
				// Menetapkan nilai ke cell dan menggabungkan sel
				DrawObject(row, col, val, rowTo, colTo);

				// Mengatur align vertikal dan horizontal untuk rentang sel
				var range = ws.Range(row, col, rowTo, colTo);
				range.Style.Alignment.Vertical = vAlign;
				range.Style.Alignment.Horizontal = hAlign;

				// Menetapkan font bold jika isBold = true
				ws.Cell(row, col).Style.Font.Bold = isBold;

				// Menambahkan border sesuai tipe
				return DrawBorderByType(row, col, rowTo, colTo, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawObject] row({rowTo}) column({colTo})");
			}
		}

		//----------------------------------------------------------------------- TABLE DOWN ------------------------------------------------------------------------

		public CellResult DrawTableDown(int row, int col, List<object> dataList)
		{
			if (dataList.Count > 0)
			{
				try
				{
					colStart = col;

					foreach (object data in dataList)
					{
						Type dataType = data.GetType();
						PropertyInfo[] properties = dataType.GetProperties();

						foreach (PropertyInfo property in properties)
						{
							object? propertyValue = property.GetValue(data);
							//ws.Cell(row, col).Value = propertyValue;
							DrawAdjustedValue(propertyValue, row, col);
							col++;
						}
						row++;
						col = colStart;
					}
					return new CellResult { Row = row, Col = col };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawTableDown] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawTableDown] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawTableDown(int row, int col, List<object> dataList, bool isWithHeader)
		{
			try
			{
				if (isWithHeader)
				{
					DrawHeaderRight(row, col, dataList[0]);
					row++;
				}
				return DrawTableDown(row, col, dataList);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTableDown] row({row}) column({col})");
			}
		}

		public CellResult DrawTableDownMerge(int row, int col, List<object> dataList, int rowMerge, int colMerge)
		{
			if (dataList.Count > 0)
			{
				try
				{
					rowStart = row;
					colStart = col;
					rowMerge = rowMerge < 1 ? 1 : rowMerge;
					colMerge = colMerge < 1 ? 1 : colMerge;
					int colLast = 0;

					foreach (object data in dataList)
					{
						Type dataType = data.GetType();
						PropertyInfo[] properties = dataType.GetProperties();

						foreach (PropertyInfo property in properties)
						{
							object? propertyValue = property.GetValue(data);
							//ws.Cell(row, col).Value = propertyValue;
							DrawAdjustedValue(propertyValue, row, col);
							ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge();
							ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true;
							col += colMerge;
						}
						row += rowMerge;
						colLast = col > colLast ? col : colLast;
						col = colStart;
					}
					return new CellResult { Row = row, Col = colLast };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawTableDownMerge] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawTableDownMerge] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawTableDownMerge(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType)
		{
			try
			{
				CellResult result = DrawTableDownMerge(row, col, dataList, rowMerge, colMerge);
				return DrawBorderByType(rowStart, colStart, result.Row - 1, result.Col - 1, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTableDownMerge] row({row}) column({col})");
			}
		}

		public CellResult DrawTableDownMerge(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType, bool isWithHeader, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign)
		{
			try
			{
				CellResult result;
				if (isWithHeader)
				{
					result = DrawHeaderRightForTable(row, col, dataList[0], rowMerge, colMerge, borderType);
					result = DrawTableDownMerge(result.Row, result.Col, dataList, rowMerge, colMerge);
				}
				else
				{
					result = DrawTableDownMerge(row, col, dataList, rowMerge, colMerge);
				}
				SetAlign(row, col, vAlign, hAlign, result.Row, result.Col);
				return DrawBorderByType(rowStart, colStart, result.Row - 1, result.Col - 1, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawTableDownMerge] row({row}) column({col})");
			}
		}

		public CellResult DrawTableDownMergeColor(int row, int col, List<object> dataList, int rowMerge, int colMerge, XLColor color, BgColorType bgColorType)
		{
			if (dataList.Count > 0)
			{
				try
				{
					rowStart = row;
					colStart = col;
					rowMerge = rowMerge < 1 ? 1 : rowMerge;
					colMerge = colMerge < 1 ? 1 : colMerge;
					int colLast = 0;
					int i = 0;

					foreach (object data in dataList)
					{
						Type dataType = data.GetType();
						PropertyInfo[] properties = dataType.GetProperties();

						foreach (PropertyInfo property in properties)
						{
							object propertyValue = property.GetValue(data);
							//ws.Cell(row, col).Value = propertyValue;
							DrawAdjustedValue(propertyValue, row, col);
							ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Merge();
							ws.Range(row, col, row + rowMerge - 1, col + colMerge - 1).Style.Alignment.WrapText = true;
							col += colMerge;
						}
						row += rowMerge;
						colLast = col > colLast ? col : colLast;
						col = colStart;

						if (bgColorType.Equals(BgColorType.Odd))
						{
							if (i == 0)
							{
								SetBackgroundColor(rowStart, colStart, color, row - 1, colLast - 1);
							}
							else if (i % 2 != 0)
							{
								SetBackgroundColor(row, col, color, row, colLast - 1);
							}
						}

						if (bgColorType.Equals(BgColorType.Even) && (i % 2 == 0) && (i < dataList.Count - 1))
						{
							SetBackgroundColor(row, col, color, row, colLast - 1);
						}

						i++;
					}

					if (bgColorType == BgColorType.All)
					{
						SetBackgroundColor(rowStart, colStart, color, row - 1, colLast - 1);
					}

					return new CellResult { Row = row, Col = colLast };
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawTableDownMergeColor] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawTableDownMergeColor] object/list is NULL. row({row}) column({col})");
			}
		}

		public CellResult DrawTableDownMergeColor(int row, int col, List<object> dataList, int rowMerge, int colMerge, BorderType borderType, XLAlignmentVerticalValues vAlign, XLAlignmentHorizontalValues hAlign, XLColor color, BgColorType bgColorType)
		{
			if (dataList.Count > 0)
			{
				try
				{
					CellResult result;
					result = DrawTableDownMergeColor(row, col, dataList, rowMerge, colMerge, color, bgColorType);

					SetAlign(row, col, vAlign, hAlign, result.Row, result.Col);
					return DrawBorderByType(rowStart, colStart, result.Row - 1, result.Col - 1, borderType);
				}
				catch (Exception e)
				{
					throw new Exception($"{e.Message} \n[DrawTableDownMergeColor] row({row}) column({col})");
				}
			}
			else
			{
				throw new Exception($"[DrawTableDownMergeColor] object/list is NULL. row({row}) column({col})");
			}
		}

		//----------------------------------------------------------------------- BACKGROUND COLOR ------------------------------------------------------------------------

		// Set background color for a single cell
		public CellResult SetBackgroundColor(int row, int col, XLColor color)
		{
			try
			{
				ws.Cell(row, col).Style.Fill.PatternType = XLFillPatternValues.Solid;
				ws.Cell(row, col).Style.Fill.BackgroundColor = color;
				return new CellResult { Row = row, Col = col };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[SetBackgroundColor] row({row}) column({col})");
			}
		}

		// Set background color for a range of cells
		public CellResult SetBackgroundColor(int row, int col, XLColor color, int rowTo, int colTo)
		{
			try
			{
				ws.Range(row, col, rowTo, colTo).Style.Fill.PatternType = XLFillPatternValues.Solid;
				ws.Range(row, col, rowTo, colTo).Style.Fill.BackgroundColor = color;
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[SetBackgroundColor] row({row}) column({col})");
			}
		}


		//--------------------------------------------------------------------------- MATH ---------------------------------------------------------------------------

		// Sum for incrementing values in a range
		public double SumLineInc(int row, int col, int rowTo, int colTo)
		{
			double sum = 0;
			foreach (var cell in ws.Range(row, col, rowTo, colTo).Cells())
			{
				double cellVal = 0;
				if (double.TryParse(cell.Value.ToString(), out cellVal))
				{
					sum += cellVal;
				}
			}
			return sum;
		}

		// Sum for decrementing values in a range
		public double SumLineDec(int row, int col, int rowTo, int colTo)
		{
			double sum = 0;
			foreach (var cell in ws.Range(row, col, rowTo, colTo).Cells())
			{
				double cellVal = 0;
				if (double.TryParse(cell.Value.ToString(), out cellVal))
				{
					sum -= cellVal;
				}
			}
			return sum;
		}

		//--------------------------------------------------------------------------- GET VALUE ---------------------------------------------------------------------------
		
		// Get value as an object
		public object GetValue(int row, int col)
		{
			return ws.Cell(row, col).Value;
		}

		// Get value as a string
		public string GetValueString(int row, int col)
		{
			return ws.Cell(row, col).GetValue<string>();
		}

		// Get value as an int
		public int GetValueInt(int row, int col)
		{
			return ws.Cell(row, col).GetValue<int>();
		}

		// Get value as Int16
		public Int16 GetValueInt16(int row, int col)
		{
			return ws.Cell(row, col).GetValue<Int16>();
		}

		// Get value as Int32
		public Int32 GetValueInt32(int row, int col)
		{
			return ws.Cell(row, col).GetValue<Int32>();
		}

		// Get value as Int64
		public Int64 GetValueInt64(int row, int col)
		{
			return ws.Cell(row, col).GetValue<Int64>();
		}

		// Get value as long
		public long GetValueLong(int row, int col)
		{
			return ws.Cell(row, col).GetValue<long>();
		}

		// Get value as decimal
		public decimal GetValueDecimal(int row, int col)
		{
			return ws.Cell(row, col).GetValue<decimal>();
		}

		// Get value as float
		public float GetValueFloat(int row, int col)
		{
			return ws.Cell(row, col).GetValue<float>();
		}

		// Get value as double
		public double GetValueDouble(int row, int col)
		{
			return ws.Cell(row, col).GetValue<double>();
		}

		// Get value as boolean
		public bool GetValueBool(int row, int col)
		{
			return ws.Cell(row, col).GetValue<bool>();
		}

		// Get value as byte
		public byte GetValueByte(int row, int col)
		{
			return ws.Cell(row, col).GetValue<byte>();
		}

		//----------------------------------------------------------------------- BORDER TYPE ------------------------------------------------------------------------

		public CellResult DrawBorder(int rowStart, int colStart, int rowTo, int colTo, BorderType borderType)
		{
			try
			{
				return DrawBorderByType(rowStart, colStart, rowTo, colTo, borderType);
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawBorder] row({rowStart}) column({colStart})");
			}
		}

		public CellResult DrawBorderByType(int rowStart, int colStart, int rowTo, int colTo, BorderType borderType)
		{
			try
			{
				var range = ws.Range(rowStart, colStart, rowTo, colTo);

				switch (borderType)
				{
					case BorderType.NoBorderAll:
						{
							range.Style.Border.TopBorder = XLBorderStyleValues.None;
							range.Style.Border.RightBorder = XLBorderStyleValues.None;
							range.Style.Border.BottomBorder = XLBorderStyleValues.None;
							range.Style.Border.LeftBorder = XLBorderStyleValues.None;
						}
						break;
					case BorderType.NoBorderAround:
						{
							range.Style.Border.OutsideBorder = XLBorderStyleValues.None;
						}
						break;
					case BorderType.BorderAroundThin:
						{
							range.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
						}
						break;
					case BorderType.BorderAroundThick:
						{
							range.Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
						}
						break;
					case BorderType.BorderAllThin:
						{
							range.Style.Border.TopBorder = XLBorderStyleValues.Thin;
							range.Style.Border.RightBorder = XLBorderStyleValues.Thin;
							range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
							range.Style.Border.LeftBorder = XLBorderStyleValues.Thin;
						}
						break;
					case BorderType.BorderAllThick:
						{
							range.Style.Border.TopBorder = XLBorderStyleValues.Thick;
							range.Style.Border.RightBorder = XLBorderStyleValues.Thick;
							range.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
							range.Style.Border.LeftBorder = XLBorderStyleValues.Thick;
						}
						break;
					case BorderType.BorderAroundDotted:
						{
							range.Style.Border.OutsideBorder = XLBorderStyleValues.Dotted;
						}
						break;
					case BorderType.BorderAroundDashed:
						{
							range.Style.Border.OutsideBorder = XLBorderStyleValues.Dashed;
						}
						break;
					case BorderType.BorderAllDotted:
						{
							range.Style.Border.TopBorder = XLBorderStyleValues.Dotted;
							range.Style.Border.RightBorder = XLBorderStyleValues.Dotted;
							range.Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
							range.Style.Border.LeftBorder = XLBorderStyleValues.Dotted;
						}
						break;
					case BorderType.BorderAllDashed:
						{
							range.Style.Border.TopBorder = XLBorderStyleValues.Dashed;
							range.Style.Border.RightBorder = XLBorderStyleValues.Dashed;
							range.Style.Border.BottomBorder = XLBorderStyleValues.Dashed;
							range.Style.Border.LeftBorder = XLBorderStyleValues.Dashed;
						}
						break;
					case BorderType.BorderBottomThin:
						{
							range.Style.Border.BottomBorder = XLBorderStyleValues.Thin;
						}
						break;
					case BorderType.BorderBottomThick:
						{
							range.Style.Border.BottomBorder = XLBorderStyleValues.Thick;
						}
						break;
					case BorderType.BorderBottomDotted:
						{
							range.Style.Border.BottomBorder = XLBorderStyleValues.Dotted;
						}
						break;
					case BorderType.BorderBottomDashed:
						{
							range.Style.Border.BottomBorder = XLBorderStyleValues.Dashed;
						}
						break;
					case BorderType.BorderBottomDouble:
						{
							range.Style.Border.BottomBorder = XLBorderStyleValues.Double;
						}
						break;
					case BorderType.BorderAroundDouble:
						{
							range.Style.Border.OutsideBorder = XLBorderStyleValues.Double;
						}
						break;
					case BorderType.BorderAllDouble:
						{
							range.Style.Border.TopBorder = XLBorderStyleValues.Double;
							range.Style.Border.RightBorder = XLBorderStyleValues.Double;
							range.Style.Border.BottomBorder = XLBorderStyleValues.Double;
							range.Style.Border.LeftBorder = XLBorderStyleValues.Double;
						}
						break;
				}
				return new CellResult { Row = rowTo, Col = colTo };
			}
			catch (Exception e)
			{
				throw new Exception($"{e.Message} \n[DrawBorderByType] row({rowStart}) column({colStart})");
			}
		}

		// The Dispose method implementation from IDisposable interface
		public void Dispose()
		{
			// Dispose of resources here
			// If you are holding onto any resources that need to be disposed, do it here
			if (ws != null)
			{
				ws = null; // Nulling out or performing cleanup
			}

			// Indicate that Dispose has been called.
			GC.SuppressFinalize(this);
		}

		// Optional: Destructor to ensure that resources are disposed in case Dispose is not called
		~MagerExcel2()
		{
			Dispose();
		}

		//eof
	}
}
