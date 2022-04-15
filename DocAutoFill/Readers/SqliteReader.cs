using DocAutoFill.DataTableConvert;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocAutoFill.Readers
{
    public class SqliteReader : IReader
    {
        public bool IsRequiersTableName => true;

        public string TableName { get; set; }

        private string _fileName;

        public SqliteReader(string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _fileName = fileName;
        }

        public AutoFillDataRow[] ReadFile()
        {

            var dicts = new List<AutoFillDataRow>();

            using (var connection = new SqliteConnection(@"Data Source="+_fileName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"
                    SELECT *
                    FROM " + TableName;

                using (var reader = command.ExecuteReader())
                {
                    var _columns = new List<string>();
                    for (int i = 0; i < reader.VisibleFieldCount; i++)
                    {
                        _columns.Add(reader.GetName(i));
                    }

                    while (reader.Read())
                    {
                        var dict = new AutoFillDataRow();

                        var _row = new List<string>();
                        for(int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            _row.Add(reader.GetString(i));
                        }
                        dict.Columns = _columns.ToArray();
                        dict.Row = _row.ToArray();
                        dict.TableName = TableName;

                        dicts.Add(dict);
                    }
                }
            }

            return dicts.ToArray();
        }
    }
}
