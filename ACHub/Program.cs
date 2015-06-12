﻿using ACHub.Plugins;
using log4net.Config;
using System;
using System.Windows.Forms;

namespace ACHub
{
    internal static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            XmlConfigurator.Configure();
            Logging.Current.Info("ACHub startup");
            try
            {
                Application.EnableVisualStyles();
                AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                Logging.Current.Fatal(ex);
            }
            Logging.Current.Info("ACHub exit");
        }

        private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
        {
            //   Logging.Current.Warn("First chance exception catched", e.Exception);
        }
    }
}