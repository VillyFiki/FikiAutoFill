using DocAutoFill.DataTableConvert;
using DocAutoFill.DocAutoFiller;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocAutoFill.Readers
{
    public interface IReader
    {
        public AutoFillDataRow[] ReadFile();
    }
}
