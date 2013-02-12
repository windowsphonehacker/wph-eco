using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace eco
{
    public partial class MainPage : PhoneApplicationPage
    {
        static CSharp___DllImport.Phone.WP7Process[] p;

        // Killing these applications causes issues (reboot, disabling features, etc). Don't kill them
        static string[] doNotKill = {"NK.EXE",
                            "udevice.exe",
                            "servicesd.exe",
                            "Compositor.exe",
                            "telshell.exe", 
                            "cprog.exe",
                            "SecSimTkit.exe",
                            "StartHost.exe",
                            "MmsTransHost.exe",
                            "XDrmRemoteServ.exe",
                            "ssupdate.exe",
                            "MITsMan.exe",
                            "dw.exe"};

        // Devices to turn on/off
        public static string[] devices = { "ACC1", "FUS1", "CAM1", "GYR1", "NLD1", "PRX1", "MAG1", "DLR1", "HTB1" };

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        public static void turnOffSensors()
        {
            foreach (string device in devices)
            {
                CSharp___DllImport.DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", device + ":", 1, (int)CSharp___DllImport.Phone.Network.WiFi.PowerState.D4);
            }
        }

        public static void killApps()
        {
            p = CSharp___DllImport.Phone.TaskManager.AllProcesses();
            foreach (var s in p)
            {
                if (!doNotKill.Contains(s.RAW.szExeFile))
                    s.Kill(0);
            }

            // Appears because Eco should kill itself through ---^
            MessageBox.Show("It appears that you do not have root privileges enabled for this application. Please mark this application as Trusted.");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            killApps();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            turnOffSensors();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            turnOffSensors();
            killApps();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            foreach (string device in devices)
            {
                CSharp___DllImport.DllImportCaller.lib.StringIntIntCall("coredll", "SetDevicePower", device + ":", 1, (int)CSharp___DllImport.Phone.Network.WiFi.PowerState.D1);
            }
        }

        private void image1_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                Microsoft.Phone.Shell.StandardTileData data = new Microsoft.Phone.Shell.StandardTileData
                {
                    Title = "",
                    BackgroundImage = new Uri("/runeco.png", UriKind.Relative)
                };

                Microsoft.Phone.Shell.ShellTile.Create(new Uri("/run.xaml", UriKind.Relative), data);
            }
            catch
            {
            }
        }

    }
}