using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace green_screen_ja
{
    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>

        [DllImport(@"C:\projekty\greenscreen\x64\Debug\JAAsm.dll")]
        static extern int MyProc1(int a, int b);

        [DllImport("CppLib.dll")]
        static extern int DLLTestFn(int a, int b);

        [STAThread]
        static void Main()
        {
            int x = 5, y = 3;
            int asmDLL = MyProc1(x, y);
            int cppDLL = DLLTestFn(x, y);
            Console.Write("ASM: ");
            Console.WriteLine(asmDLL);
            Console.Write("CPP: ");
            Console.WriteLine(cppDLL);
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
