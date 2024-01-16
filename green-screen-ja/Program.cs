using System;
using System.Windows.Forms;

namespace green_screen_ja
{
    internal static class Program
    {
        /// <summary>
        /// Główny punkt wejścia dla aplikacji.
        /// </summary>

        [STAThread]
        static void Main()
        {

            Form form = new Form1();
            Application.Run(form);

        }
    }
}
