using Com.StellmanGreene.CSVReader;
using DocAutoFill.DataTableConvert;
using DocAutoFill.DocAutoFiller;
using DocAutoFill.Readers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace DocAutoFill
{
    public class MainViewModel : ViewModel
    {

        #region DataTable
        public DataView DataTable
        {
            get
            {
                return _dataTable;
            }
            set
            {
                _dataTable = value;
                SetProperty(value);
            }

        }

        private DataView _dataTable = new DataView();
        #endregion

        #region SelectedItem
        public DataRowView SelectedItem
        {
            get
            {
                return _selectedItem as DataRowView;
            }
            set
            {
                _selectedItem = value;
                SetProperty(value);
            }

        }

        private object _selectedItem = new object();
        #endregion

        #region Commands
        public Command OpenFileExplorerCommand { get; }
        public Command FillCommand { get; }
        public Command OpenAboutCommand { get; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            OpenFileExplorerCommand = new Command(OpenFileExplorer);
            FillCommand = new Command(Fill);
            OpenAboutCommand = new Command(OpenAbout);
        }
        #endregion

        #region Open Methods
        private void OpenFileExplorer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
                OpenCSV(openFileDialog.FileName);
        }
        private void OpenAbout()
        {
            Process.Start(new ProcessStartInfo() { UseShellExecute = true, FileName = "https://github.com/VillyFiki" });
        }

        private void OpenCSV(string _fileName)
        {
            IReader reader = ReaderCreator.Create(_fileName);
            var converter = new DataTableConverter();
            DataTable = converter.CreateDataTable(reader.ReadFile()).DefaultView;
        }
        #endregion

        private void Fill()
        {
            if(SelectedItem != null)
            {
                AutoFillRow row = new AutoFillRow(SelectedItem);

                var codes = row.GetAutoFillCodes();

                var autoFill = new AutoFill(@"F:\test.docx");
                autoFill.Fill(codes);
            }
        }
    }
}
