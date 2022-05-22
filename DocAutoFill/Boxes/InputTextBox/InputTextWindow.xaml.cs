using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DocAutoFill.Boxes.InputTextBox
{
    /// <summary>
    /// Логика взаимодействия для InputTextWindow.xaml
    /// </summary>
    public partial class InputTextWindow : Window
    {
        public InputTextWindow()
        {
            InitializeComponent();
        }
        public void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }
    }
}
