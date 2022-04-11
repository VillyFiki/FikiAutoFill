using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DocAutoFill
{
    public class Settings : ViewModel
    {
        public string InputFile
        {
            get
            {
                return _inputFile;
            }
            set
            {
                _inputFile = value;
                SetProperty(value);
            }

        }
        private string _inputFile;

        public string OutputDir
        {
            get
            {
                return _outputDir;
            }
            set
            {
                _outputDir = value;
                SetProperty(value);
            }

        }
        private string _outputDir = Directory.GetCurrentDirectory();

        public string OutputType
        {
            get
            {
                return _outputType;
            }
            set
            {
                _outputType = value;
                SetProperty(value);
            }

        }
        private string _outputType;
    }

    enum OutputType
    {
        docx
    }
}
