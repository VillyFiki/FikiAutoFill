using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace DocAutoFill.Readers
{
    public static class ReaderCreator
    {
        public static IReader Create(string _fileName, string tableName = null)
        {
            IReader reader = null;
            if (_fileName.EndsWith(".csv"))
            {
                reader = new CsvReader(_fileName);
            }
            else if (_fileName.EndsWith(".db") || _fileName.EndsWith(".sqlite"))
            {
                reader = new SqliteReader(_fileName);
            }
            else
            {
                throw new NotSupportedException("This database not supported");
            }

            return reader;
        }
    }
}
