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
        private string _outputDir { get; set; }
        public AutoFill(string fileName, string outputDir)
        {
            if (File.Exists(fileName))
                _fileName = fileName;
            else throw new FileNotFoundException();

            _outputDir = outputDir;
        }

        public bool Fill(IEnumerable<AutoFillCode> codes)
        {
            var fileName = Path.GetFileNameWithoutExtension(_fileName);
            FileInfo file = new FileInfo(_fileName);
            AutoFillCode[] codesArray = (AutoFillCode[])codes;
            var path = Path.Combine(_outputDir, Path.GetFileNameWithoutExtension(file.Name) + "-" + codesArray[0].TableName + file.Extension);

            try
            {
                File.Copy(_fileName, path);
                using (WordprocessingDocument wordDoc = WordprocessingDocument.Open(path, true))
                {
                    string docText = null;
                    var a = wordDoc.MainDocumentPart.Document;


                    var body = wordDoc.MainDocumentPart.Document.Body;
                    var paras = body.Elements<Paragraph>();

                    foreach (var para in paras)
                    {
                        bool flag = false;
                        string str = "";

                        foreach (var code in codes)
                        {
                            if (!para.InnerText.Contains(code.Code))
                                break;
                        }
                        foreach (var run in para.Elements<Run>())
                        {

                            foreach (var text in run.Elements<Text>())
                            {
                                if (text.Text.StartsWith('{'))
                                    flag = true;
                                if (flag)
                                {
                                    str += text.Text;
                                    text.Text = "";
                                }
                                if (str.EndsWith('}'))
                                {
                                    flag = false;
                                    foreach (var code in codes)
                                    {
                                        if (str.Contains(code.Code))
                                        {
                                            text.Text = code.Name;
                                            str = "";
                                        }
                                    }
                                }


                            }


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

                    wordDoc.Save();
                }

                return true;
            }
            catch { return false; }
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
                codes[i] = new AutoFillCode() { Code = StringToTag(_columns[i]), Name = _row[i], TableName = _tableName };
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
