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

namespace MVVMsmoke.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для UserControl.xaml
    /// </summary>
    public partial class StoreBorder : UserControl
    {
        Window parent;
        public StoreBorder(Window parent)
        {
            this.parent = parent;
            InitializeComponent();
        }

        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            this.parent.Hide();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.parent.Close();
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) 
                parent.DragMove();
        }

        private void btnMaximazeSize_Click(object sender, RoutedEventArgs e)
        {
            if (this.parent.WindowState == WindowState.Maximized)
            {
                this.parent.WindowState = WindowState.Normal;
            }
            else
            {
                this.parent.WindowState = WindowState.Maximized;
            } 
        }
        private void TaskBarItem_TrayLeftMouseDown(object sender, RoutedEventArgs e)
        {
            this.parent.Show();
        }

        private void TaskbarIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            //ti.ShowBalloonTip("Подсказка", "Меню", Hardcodet.Wpf.TaskbarNotification.BalloonIcon.Info);
        }
    }
}
