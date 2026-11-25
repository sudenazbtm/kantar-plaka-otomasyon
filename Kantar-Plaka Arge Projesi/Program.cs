using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kantar_Plaka_Arge_Projesi
{
    internal static class Program
    {
        /// <summary>
        /// Uygulamanın ana girdi noktası.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                try
                {
                    string resimYolu = args[0];
                    if (!System.IO.File.Exists(resimYolu))
                    {
                        Console.WriteLine("Error: File not found: " + resimYolu);
                        return;
                    }
                    string sonuc = Form1.PlakaOkuOCR(resimYolu);
                    Console.WriteLine("OCR Result: " + sonuc);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.ToString());
                }
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (var loginForm = new LoginForm())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1());
                }
            }
        }
    }
}
