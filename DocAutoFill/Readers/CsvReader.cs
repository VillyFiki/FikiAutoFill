using DocAutoFill.DataTableConvert;
using DocAutoFill.DocAutoFiller;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace DocAutoFill.Readers
{
    public class CsvReader : IReader
    {
        public bool IsRequiersTableName { get; } = false;
        public string TableName { get; set; }

        private string _fileName;

        public CsvReader(string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _fileName = fileName;
        }

        public AutoFillDataRow[] ReadFile()
        {
            AutoFillDataRow[] _afRow;

            string[] _column;
            string[] _row;

            List<string[]> _lines = new List<string[]>();
            using (var fs = File.OpenRead(_fileName))
            using (var sr = new StreamReader(fs))
            {
                while (!sr.EndOfStream)
                    _lines.Add(sr.ReadLine().Split(','));
            }

            _column = _lines[0];

            _afRow = new AutoFillDataRow[_lines.Count - 1];

            for(int i = 0; i < _afRow.Length; i++)
            {
                _afRow[i] = new AutoFillDataRow();
                _afRow[i].Columns = _column;
                _afRow[i].Row = _lines[i+1];
                _afRow[i].TableName = Path.GetFileNameWithoutExtension(_fileName);
            }

            return _afRow;
        }

        public string[] GetTableList()
        {
            return new string[0];
        }
    }
}
