using System;
using System.Threading;
using System.Windows.Forms;

namespace QTranslateNet
{
    internal static class Program
    {
        private static Form? _activeForm;

        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            // 1. Handle UI thread exceptions
            Application.ThreadException += new ThreadExceptionEventHandler(GlobalThreadException);

            // 2. Force exceptions to always go through our handler
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);

            // 3. Handle non-UI thread exceptions (e.g., background workers)
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalUnhandledException);

            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            _activeForm = new Form1();
            Application.Run(_activeForm);
        }

        private static void GlobalThreadException(object sender, ThreadExceptionEventArgs e)
        {
            // Logic for UI thread errors (e.g., log and show message)

            // MessageBox.Show("A UI error occurred: " + e.Exception.Message);
            _activeForm!.Controls["textBoxTo"]!.Text = "[ERR] " + e.Exception.Message;
        }

        private static void GlobalUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // Logic for non-UI threads. Note: The app will likely terminate after this.
            Exception ex = (Exception)e.ExceptionObject;

            // MessageBox.Show("A fatal background error occurred: " + ex.Message);
            _activeForm!.Controls["textBoxTo"]!.Text = "[ERR] " + ex.Message;
        }
    }
}