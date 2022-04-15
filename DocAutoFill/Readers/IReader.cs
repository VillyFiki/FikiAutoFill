using DocAutoFill.DataTableConvert;
using DocAutoFill.DocAutoFiller;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace DocAutoFill.Readers
{
    public interface IReader
    {
        public bool IsRequiersTableName { get; }
        public string TableName { get; set; }
        public AutoFillDataRow[] ReadFile();
    }
}
