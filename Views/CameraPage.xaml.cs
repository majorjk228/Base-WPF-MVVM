using Macroscope.ViewModels;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Macroscope.Views
{
    /// <summary>
    /// Логика взаимодействия для CameraPage.xaml
    /// </summary>
    public partial class CameraPage : Page
    {
        public CameraPage()
        {
            InitializeComponent();
            DataContext = new CameraPageViewModel();
        }
    }
}
