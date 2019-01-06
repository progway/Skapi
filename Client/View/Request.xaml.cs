using ClientApp.Core;
using ClientApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ClientApp.View
{
    /// <summary>
    /// Interaction logic for Request.xaml
    /// </summary>
    public partial class Request : Window
    {
        private event EventHandler _close;

        public Request(EntryConferenceEventArgs param)
        {
            InitializeComponent();
            _close += Request__close;  
            DataContext = new RequestVM(param, _close);
        }

        private void Request__close(object sender, EventArgs e) => Close();
    }
}
