using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

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

            var bmp = new Bitmap(Properties.Resources.shield);
            var icon = Icon.FromHandle(bmp.GetHicon());
            _trayIcon = new NotifyIcon
            {
                Text = "Firewall Control",
                Icon = icon
            };

            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Start", Start);
            trayMenu.MenuItems.Add("Stop", Stop);
            trayMenu.MenuItems.Add("Exit", Exit);

            _trayIcon.ContextMenu = trayMenu;
            _trayIcon.Visible = true;

            Application.Run();

            Console.Read();
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
