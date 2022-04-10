﻿using DocAutoFill.DataTableConvert;
using DocAutoFill.DocAutoFiller;
using DocAutoFill.Readers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

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

        #region Settings
        public Settings Settings
        {
            get
            {
                return _settings;
            }
            set
            {
                _settings = value;
                SetProperty(value);
            }

        }

        private Settings _settings = new Settings();
        #endregion

        #region Commands
        public Command OpenFileCommand { get; }
        public Command FillCommand { get; }
        public Command OpenAboutCommand { get; }
        public Command ChangeOutputDirectoryCommand { get; }
        #endregion

        #region Constructor
        public MainViewModel()
        {
            OpenFileCommand = new Command(OpenFile);
            ChangeOutputDirectoryCommand = new Command(ChangeOutputDirectory);
            FillCommand = new Command(Fill);
            OpenAboutCommand = new Command(OpenAbout);
        }
        #endregion

        #region Open Methods
        private string OpenFileExplorer()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.FileName;
            else return null;
        }
        private string OpenDirectoryExplorer()
        {
            var openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                return openFileDialog.SelectedPath;
            else return null;
        }

        private void OpenFile()
        {
            OpenCSV(OpenFileExplorer());
        }
        private void ChangeOutputDirectory()
        {
            Settings.OutputDir = OpenDirectoryExplorer();
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
