using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace DocAutoFill.DocAutoFiller
{
    public class AutoFill
    {
        private string _fileName { get; set; }
        public AutoFill(string fileName)
        {
            if (File.Exists(fileName))
                _fileName = fileName;
            else throw new FileNotFoundException();
        }

        public void Fill(IEnumerable<AutoFillCode> codes)
        {
            var fileName = Path.GetFileNameWithoutExtension(_fileName);
            FileInfo file = new FileInfo(_fileName);
            AutoFillCode[] codesArray = (AutoFillCode[])codes;
            var path = Path.Combine(file.Directory.FullName, Path.GetFileNameWithoutExtension(file.Name) + "-" + codesArray[0].TableName + file.Extension);
            File.Copy(_fileName, path);

            using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
            {
                string docText = null;
                var a = wordDoc.MainDocumentPart.Document;


                var body = wordDoc.MainDocumentPart.Document.Body;
                var paras = body.Elements<Paragraph>();
                var tables = body.Elements<Table>();

                foreach (var para in paras)
                {
                    foreach (var run in para.Elements<Run>())
                    {
                        foreach (var text in run.Elements<Text>())
                        {
                            foreach (var code in codes)
                            {
                                if (text.Text.Contains(code.Code))
                                {
                                    text.Text = text.Text.Replace(code.Code, code.Name);
                                }
                            }
                        }
                    }
                }

                foreach (var table in tables)
                {
                    foreach (var row in table.Elements<TableRow>())
                    {
                        foreach (var cell in row.Elements<TableCell>())
                        {
                            foreach (var parag in cell.Elements<Paragraph>())
                            {
                                foreach (var run in parag.Elements<Run>())
                                {
                                    foreach (var text in run.Elements<Text>())
                                    {
                                        foreach (var code in codes)
                                        {
                                            if (text.Text.Contains(code.Code))
                                            {
                                                text.Text = text.Text.Replace(code.Code, code.Name);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                wordDoc.Save();
            }

        }
    }

    public struct AutoFillCode
    {
        public string TableName { get; set; }
        public string Code { get; set; } // put code {Name}
        public string Name { get; set; } // put name
    }

    public class AutoFillRow
    {
        private string[] _columns;
        private string[] _row;
        private string _tableName;

        public AutoFillRow(DataRowView dataView)
        {
            _columns = new string[dataView.DataView.Table.Columns.Count];
            _row = new string[dataView.DataView.Table.Columns.Count];
            _tableName = dataView.DataView.Table.TableName;

            for (int i = 0; i < dataView.DataView.Table.Columns.Count; i++)
            {
                _columns[i] = dataView.DataView.Table.Columns[i].ColumnName;
                _row[i] = dataView.Row[i].ToString();
            }
        }

        public AutoFillCode[] GetAutoFillCodes()
        {
            AutoFillCode[] codes = new AutoFillCode[_row.Length];

            for (int i = 0; i < _row.Length; i++)
            {
                codes[i] = new AutoFillCode() { Code = StringToTag(_columns[i]), Name = _row[i] , TableName = _tableName};
            }

            return codes;
        }

        private string StringToTag(string str)
        {
            if (!(str[0] == '{' && str[^1] == '}'))
            {
                return "{" + str + "}";
            }



            str.Replace(" ", "");

            return str;
        }
    }
}
