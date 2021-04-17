using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InwCopy
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length >=2 && args[0] == "-u")
            {

             Application.Run(new Inwentexp(args[1]));
            }
            else if (args.Length >= 2 && args[0] == "-d")
            {
              Application.Run(new Inwentimp(args[1]));
            }
            else
            {
            MessageBox.Show("niepoprawne argumenty wywołania: -d [filename] lub -u [filename]");
            }
            
            
            
            
           
        }
    }
}
