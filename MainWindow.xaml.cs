using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace ASP_MVC
{
    public  class ListThreads
    {
        public List<Thread> Threads;

        private readonly object locker = new object();

        public ListThreads()
        {
            Threads = new List<Thread>();
        }

        public void Add(Thread thread)
        {
            Threads.Add(thread);
        }

        public bool Remove(Thread thread)
        {
            bool isRemoved;

            lock (locker)
            {
                isRemoved = Threads.Remove(thread);
            }

            return (isRemoved);
        }
    }

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

            try
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                 {
                     tbFib.Text += a + " ";
                     try
                     {
                         time = int.Parse(tbTime.Text);
                         tbError.Text = String.Empty;
                     }
                     catch (FormatException)
                     {
                         time = 2;
                         tbError.Text = "Введите целое число";
                     }
                 }));
                Thread.Sleep(time * 1000);
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    tbFib.Text += b + " ";
                    try
                    {
                        time = int.Parse(tbTime.Text);
                        tbError.Text = String.Empty;
                    }
                    catch (FormatException)
                    {
                        time = 2;
                        tbError.Text = "Введите целое число";
                    }
                }));

                while (true)
                {
                    a = (b+=a)-a;
                    Thread.Sleep(time * 1000);
                    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                    {
                        tbFib.Text += a + " ";
                        try
                        {
                            time = int.Parse(tbTime.Text);
                            tbError.Text = String.Empty;
                        }
                        catch(FormatException)
                        {
                            time = 2;
                            tbError.Text = "Введите целое число";
                        }
                    }));
                }
            }
            catch (ThreadInterruptedException)
            {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
                {
                    MessageBox.Show("Поток был прерван", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                }));
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(new ThreadStart(AddNumberFibonacci));
            thread.Start();

            ListThreads list = new ListThreads();

            Thread thread1 = new Thread(() => { Thread.Sleep(5000); });
            Thread thread2 = new Thread(() => { });
            list.Add(thread1);
            thread1.Start();
            list.Add(thread2);

            while(!list.Remove(thread1));
        }
    }
}
