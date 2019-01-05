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
    /// Interaction logic for RootView.xaml
    /// </summary>
    public partial class RootView : Window
    {
        public RootView()
        {
            InitializeComponent();
            DataContext = new RootViewModel();
            Closed += RootView_Closed;
        }

        private void RootView_Closed(object sender, EventArgs e) => ((RootViewModel)DataContext).Model.Stop();
    }
}
