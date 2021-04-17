﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication4
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
          

           try
           {
               if (args.Length >= 1)
               {
                   Application.Run(new Form1(args));
                             
                  
              }
               else
              {
                  MessageBox.Show("Za mało argumentów");
                 Application.Exit();
              }
       }
         catch (Exception e)
         {
         MessageBox.Show(e.Message);
         }
        }
    }
}
