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
using MStopWatch.VM;

namespace MStopWatch.WPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        StopWatchVM _vm;
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _vm = new StopWatchVM();
            this.DataContext = _vm;
            _vm.Reset();
        }

        private void clickStart(object sender, RoutedEventArgs e)
        {
            _vm.ClickStart();
        }

        private void clickLap(object sender, RoutedEventArgs e)
        {
            _vm.ClickLap();
        }
    }
}
