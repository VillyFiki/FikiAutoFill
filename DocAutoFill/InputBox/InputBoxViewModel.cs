using System;
using System.Collections.Generic;
using System.Text;

namespace DocAutoFill.InputBox
{
    public partial class InputBoxViewModel : ViewModel
    {
        #region DataTable
        public string[] Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                SetProperty(value);
            }

        }

        private string[] _items = new string[2];
        #endregion
    }
}
