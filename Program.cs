using QuotationApp.Helpers;
using System;
using System.Windows.Forms;

namespace QuotationApp
{
    static class PdfPageEventHelper
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DatabaseHelper.InitDatabase(); // ensure database and tables exist
            Application.Run(new login());
        }
    }
}
