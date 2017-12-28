using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LY.CacheManager.Test
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

 

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            CacheManagerClass.Add("a",txtInput.Text);
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            ///
            txtShow.Text = CacheManagerClass.Get<string>("a");
        }

        private void Put_Click(object sender, RoutedEventArgs e)
        {
            CacheManagerClass.Put("a", txtInput.Text);
        }
    }
}
