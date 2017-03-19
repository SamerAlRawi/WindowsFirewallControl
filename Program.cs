using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using FirewallControl.Properties;

namespace FirewallControl
{
    static class Program
    {
        private static NotifyIcon _trayIcon;

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            _trayIcon = new NotifyIcon
            {
                Text = Resources.TrayTitle,
                Icon = BuildIconFromResources(),
                ContextMenu = BuildContextMenu(),
                Visible = true
            };

            Application.Run();

            Console.Read();
        }

        private static Icon BuildIconFromResources()
        {
            var bmp = new Bitmap(Resources.shield);
            return Icon.FromHandle(bmp.GetHicon());
        }

        private static ContextMenu BuildContextMenu()
        {
            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Start", Start);
            trayMenu.MenuItems.Add("Stop", Stop);
            trayMenu.MenuItems.Add("Exit", Exit);
            return trayMenu;
        }

        private static void Start(object s, EventArgs a)
        {
            RunCommand("NetSh", "Advfirewall set allprofiles state on");
        }

        private static void Stop(object s, EventArgs a)
        {
            RunCommand("NetSh", "Advfirewall set allprofiles state off");
        }

        private static void RunCommand(string cmd, string args)
        {
            var p = new Process();
            var processStartInfo = new ProcessStartInfo(cmd, args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };
            p.StartInfo = processStartInfo;
            p.Start();
        }

        private static void Exit(object s, EventArgs a)
        {
            _trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
