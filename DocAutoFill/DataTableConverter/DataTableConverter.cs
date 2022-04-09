using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace DocAutoFill.DataTableConvert
{
    public class AutoFillDataRow
    {
        public string TableName { get; set; }
        public string[] Columns { get; set; }
        public string[] Row { get; set; }
    }

    public class DataTableConverter
    {
        public DataTable CreateDataTable(AutoFillDataRow[] rows)
        {
            DataTable table = new DataTable();
            table.TableName = rows[0].TableName;

            for (int i = 0; i < rows[0].Columns.Length; i++)
            {
                table.Columns.Add();
                table.Columns[i].ColumnName = rows[0].Columns[i];
            }

            foreach (var row in rows)
            {
                DataRow dataRow = table.NewRow();
                for (int i = 0; i < row.Columns.Length; i++)
                {
                    dataRow[i] = row.Row[i];
                }
                table.Rows.Add(dataRow);
            }
            return table;
        }
    }
}
