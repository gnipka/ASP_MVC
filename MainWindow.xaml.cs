using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ASP_MVC
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddNumberFibonacci()
        {
            int a = 0;
            int b = 1;
            int time = 0;

            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,new Action(() =>
            {
                tbFib.Text += a + " ";
                time = int.Parse(tbTime.Text);
            }));
            Thread.Sleep(time * 1000);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                tbFib.Text += b + " ";
                time = int.Parse(tbTime.Text);
            }));

            while (true)
            {
                a = (b+=a)-a;
                Thread.Sleep(time * 1000);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    tbFib.Text += a + " ";
                    time = int.Parse(tbTime.Text);
                }));                
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(AddNumberFibonacci));
            thread.Start();

        }
    }
}
