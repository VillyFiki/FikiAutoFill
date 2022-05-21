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
        public string[] GetTableList()
        {
            var result = new List<string>();
            using (var connection = new SqliteConnection(@"Data Source=" + _fileName))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                @"SELECT name 
                    FROM sqlite_master 
                    WHERE type = 'table'; 
                ";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            result.Add(reader.GetString(i));
                        }
                    }
                }
            }

            return result.ToArray();
        }
        public AutoFillDataRow[] ReadFile()
        {
            var dicts = new List<AutoFillDataRow>();

            using (var connection = new SqliteConnection(@"Data Source=" + _fileName))
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
                        for (int i = 0; i < reader.VisibleFieldCount; i++)
                        {
                            string strRow = string.Empty;
                            if (!reader.IsDBNull(i))
                            {
                                strRow = reader.GetString(i);
                            }

                            _row.Add(strRow);
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
