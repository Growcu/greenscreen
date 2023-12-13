using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace green_screen_ja
{
    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>

        [DllImport(@"C:\projekty\green-screen-ja\x64\Debug\JAAsm.dll")]
        static extern int MyProc1(int a, int b);

        [STAThread]
        static void Main()
        {

            int x = 5, y = 3;
            int retVal = MyProc1(x, y);
            Console.Write("Moja pierwsza wartość obliczona w asm to:");
            Console.WriteLine(retVal);
            Console.ReadLine();

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            Form form = new Form1();
            //Label label1 = new Label();
            //label1.Width = 500;
            //label1.Text = $"Moja pierwsza wartość obliczona w asm to: {retVal}";
            //form.Controls.Add(label1);
            Application.Run(form);

        }
    }
}
