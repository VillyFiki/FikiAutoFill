using System;
using System.Collections.Generic;
using System.Text;

namespace DocAutoFill.Readers
{
    public static class ReaderCreator
    {
        public static IReader Create(string _fileName)
        {
            if (_fileName.EndsWith(".csv"))
            {
                return new CsvReader(_fileName);
            }
            if (_fileName.EndsWith(".db"))
            {
                return new LiteDbReader(_fileName);
            }

            throw new Exception();
        }
    }
}
