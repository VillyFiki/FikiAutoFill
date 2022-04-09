using Com.StellmanGreene.CSVReader;
using DocAutoFill.DataTableConvert;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace DocAutoFill.Readers
{
    public class LiteDbReader : IReader
    {
        private string _fileName;

        public LiteDbReader(string fileName)
        {
            var folder = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            _fileName = fileName;
        }

        public AutoFillDataRow[] ReadFile()
        {
            var dicts = new List<AutoFillDataRow>();

            using (var db = new LiteDatabase(_fileName))
            {
                var coll = db.GetCollection("test").FindAll();

                foreach (BsonDocument c in coll)
                {
                    var dict = new AutoFillDataRow();

                    var _columns = new List<string>();
                    var _row = new List<string>();

                    foreach (var item in c)
                    {
                        _columns.Add(item.Key);
                        _row.Add(item.Value.RawValue.ToString());
                    }

                    dict.Columns = _columns.ToArray();
                    dict.Row = _row.ToArray();

                    dicts.Add(dict);
                }
            }
            return dicts.ToArray();
        }
    }
}
