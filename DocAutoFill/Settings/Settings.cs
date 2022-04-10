using System;
using System.Collections.Generic;
using System.Text;

namespace DocAutoFill
{
    public class Settings : ViewModel
    {
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
        private string _outputDir;

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
