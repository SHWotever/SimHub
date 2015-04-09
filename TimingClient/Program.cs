using ACToolsUtilities.Automation;
using System;
using System.Windows.Forms;

namespace TimingClient
{
    internal static class Program
    {
        public static AHKMacroRunner MacroEngine = new AHKMacroRunner();

        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;

            Application.Run(new V2.MainForm());
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            lock (string.Intern("fcelogger"))
            {
                //System.IO.File.AppendAllText("fce.log", e.Exception.ToString() + "\r\n-----------------------------------------------");
            }
        }
    }
}