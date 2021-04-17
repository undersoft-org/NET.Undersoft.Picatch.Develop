using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Globalization;

namespace InwCopy
{
    public partial class Inwentexp : Form
    {

        private string filename = "";
        private string port;
        private string ip;
        public Inwentexp(string param)
        {
            InitializeComponent();
            if (File.Exists("picconfig.cnf") != true)
            {
               
                StreamWriter sw = new StreamWriter("picconfig.cnf");
                sw.WriteLine("127.0.0.1");
                sw.WriteLine("8790");
                sw.Close();
            }

            StreamReader sr1 = new StreamReader("picconfig.cnf");
            ip = sr1.ReadLine();
            port = sr1.ReadLine();
            sr1.Close();
            //for (int i = 0; i < param.Length; i++)
            //{
               // StringReader sr = new StringReader(param[i]);
               // char[] check = new char[1];
               // sr.Read(check, 0, 1);
               // if (check[0] != '-')
               // {
                    filename = param;
                  
               // }
              //  sr.Close();
          //  }
           
           try
           {
               takefile();
           }
           catch (Exception e)
           {
               MessageBox.Show(e.Message);
           }
        
        }


        private void takefile()
        {



            
            DataTable table = new DataTable("CPT_EXP");
            table.Locale = CultureInfo.CurrentCulture;
            table.Columns.Add("typ", typeof(String));
            table.Columns.Add("kod", typeof(String));
            table.Columns.Add("nazwa", typeof(String));
            table.Columns.Add("stan", typeof(String));
            table.Columns.Add("cenazk", typeof(String));
            table.Columns.Add("cenasp", typeof(String));
            table.Columns.Add("vat", typeof(String));

            
            
            //start reading the textfile
            StreamReader reader = new StreamReader(filename, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage), true);
            string line;
           NumberFormatInfo nfi = new NumberFormatInfo();
           nfi.NumberDecimalSeparator = ".";
         
            while ((line = reader.ReadLine()) != null)
            {
                string[] items = line.Split(';');
                //make sure it has 3 items
               

               
                DataRow row = table.NewRow();
                row["typ"] = items[0];
                row["kod"] = items[1];
                row["nazwa"] = items[2];
                row["stan"] = (decimal.Parse(items[5]) / 1000).ToString(nfi);
                row["cenazk"] = (decimal.Parse(items[3]) / 100).ToString(nfi);
                row["cenasp"] = (decimal.Parse(items[4]) / 100).ToString(nfi); 
                row["vat"] = items[6];
                table.Rows.Add(row);

            }
            
            reader.Close();

            


            StringBuilder sb = new StringBuilder();
            
            foreach (DataRow row in table.Rows)
            {
                
                IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                
                sb.AppendLine(string.Join(";", fields));
            }

            File.WriteAllText("picatch.exp", sb.ToString());

            byte[] SendingBuffer = null;
			TcpClient client = null;
			NetworkStream netstream = null;
			byte[] sendorec = new byte[1];
			sendorec[0] = 11;
			try
			{
				client = new TcpClient(ip, int.Parse(port));
				netstream = client.GetStream();
				netstream.Write(sendorec, 0, 1);
				
				FileStream Fs = new FileStream("picatch.exp", FileMode.Open, FileAccess.Read);
				int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(1024)));
				
				int TotalLength = (int)Fs.Length, CurrentPacketLength, counter = 0;
				for (int i = 0; i < NoOfPackets; i++)
				{
					if (TotalLength > 1024)
					{
						CurrentPacketLength = 1024;
						TotalLength = TotalLength - CurrentPacketLength;
					}
					else
					{
						CurrentPacketLength = TotalLength;
					}
					SendingBuffer = new byte[CurrentPacketLength];
					Fs.Read(SendingBuffer, 0, CurrentPacketLength);
					netstream.Write(SendingBuffer, 0, (int)SendingBuffer.Length);
					

				}
                
				Fs.Close();
				netstream.Close();
				client.Close();

			}
			catch (SocketException)
			{
				MessageBox.Show("Błąd sieci. Nie można połączyć się z agentem");
			
			}

          //  this.Close();
            
          


    }


        //public static string ToGBString(this double value)
        //{
        //    return value.ToString(System.Globalization.CultureInfo.GetCultureInfo("en-GB"));
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Inwentexp_Shown(object sender, EventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                Thread.Sleep(1000);
                button1.Text = "OK sek." + (3 - i).ToString();
                button1.Refresh();
                if (i == 3)
                {
                    this.Close();
                }
            }
        }

    }
}
