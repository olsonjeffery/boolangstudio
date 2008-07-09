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
using System.Reflection;
using System.IO;
using System.Timers;
using System.Diagnostics;
using System.Windows.Threading;

namespace Splash
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        bool _copied;
        Timer _timer;
        string _tempPath;
        Process _process;

        public Window1()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Window1_Loaded);
        }

        void Window1_Loaded(object sender, RoutedEventArgs e)
        {
            _tempPath = System.IO.Path.ChangeExtension(System.IO.Path.GetTempFileName(), "msi");

            _timer = new Timer(2000);
            _timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _timer.Start();

            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Splash.BooLangStudio.msi");
            byte[] bytes = new byte[stream.Length];
            stream.Read(bytes, 0, (int)stream.Length);

            File.WriteAllBytes(_tempPath, bytes);

            _copied = true;
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_copied)
            {
                _timer.Stop();
                Dispatcher.Invoke(
                    DispatcherPriority.Send,
                    (Action)delegate { boo.Visibility = Visibility.Hidden; });

                string verb = null;
                if (System.Environment.OSVersion.Platform == PlatformID.Win32NT &&
                    System.Environment.OSVersion.Version > new Version("6.0.0.0"))
                    verb = "runas";

                _process = new Process();
                _process.StartInfo.FileName = "msiexec";
                _process.StartInfo.Arguments = string.Format(" /i \"{0}\"", _tempPath);
                _process.StartInfo.Verb = verb;
                _process.Start();
                _process.WaitForExit();

                Dispatcher.Invoke(DispatcherPriority.Send, (Action)Close);
            }
        }
    }
}
