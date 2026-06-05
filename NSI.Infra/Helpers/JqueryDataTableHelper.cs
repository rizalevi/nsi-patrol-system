using static NM9Infra.Models.Global.JQueryDataTableModel;

namespace NSI.Infra.Helpers
{
	public static class JqueryDataTableHelper
	{
		public static (string, string) DefaultOrderColumnAndDirection(DataTablesRequest param, string defaultOrderColumn, string defaultOrderDir)
		{
			// Setup Default Order & Direction (kalo dari payload nemu _orderColumn & _orderDir -> pake dari payload else pake default)
			string _orderColumn = string.Empty;
			string _orderDir = string.Empty;

			if (param.Order != null && param.Order.Count > 0 && param.Order[0].Column >= 0)
			{
				int columnIndex = param.Order[0].Column;
				if (param.Columns != null && columnIndex < param.Columns.Count)
				{
					_orderColumn = param.Columns[columnIndex].Data?.ToUpper() ?? "";
					_orderDir = param.Order[0].Dir?.ToUpper() ?? "";
				}
			}
			else
			{
				_orderColumn = defaultOrderColumn;
				_orderDir = defaultOrderDir;
			}

			return (_orderColumn, _orderDir);
		}

		//eof
	}
}