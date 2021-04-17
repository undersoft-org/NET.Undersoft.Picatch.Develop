using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Globalization;


namespace InwCopy
{
    public partial class Inwentimp : Form
    {
        private string filename;
        private string port;
        private string ip;
        public Inwentimp(string param)
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

            filename = param;
               
               
           
          
            try
            {
                putfile();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
            
           
        }

        private void putfile()
        {

            
            
            
            TcpClient client = null;
            NetworkStream netstream = null;
            byte[] sendorec = new byte[1];
            sendorec[0] = 12;
            byte[] RecData = new byte[1024];
            int RecBytes;

            try
            {

                FileStream Fs = new FileStream("picatch.imp", FileMode.Create, FileAccess.Write);
               
                client = new TcpClient(ip, int.Parse(port));
                netstream = client.GetStream();
                netstream.Write(sendorec, 0, 1);

                int totalrecbytes = 0;
                
                
                while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                {
                    Fs.Write(RecData, 0, RecBytes);
                    totalrecbytes += RecBytes;
                    
                }
                Fs.Close();
                Fs.Dispose();
                netstream.Close();
                client.Close();
            

               
                DataTable table = new DataTable("CPT_EXP");
                table.Columns.Add("nazwa", typeof(String));
                table.Columns.Add("kod", typeof(String));
                table.Columns.Add("ilosc", typeof(String));
                table.Columns.Add("cenazk", typeof(String));
                table.Columns.Add("cenasp", typeof(String));

                //start reading the textfile
                StreamReader sr = new StreamReader("picatch.imp");
                string allData = sr.ReadToEnd();
                string[] rows = allData.Split("\r\n".ToCharArray());
                allData = "empty";
                sr.DiscardBufferedData();
                sr.Close();
                string delimeter = ";";

               
                
                foreach (string r in rows)
                {
                    

                    string[] items = r.Split(delimeter.ToCharArray());

                  
                    //make sure it has 3 items
                    if (items[0] != "")
                    {
                        DataRow row = table.NewRow();

                        row["nazwa"] = "INW";
                        row["kod"] = items[1];
                        row["ilosc"] = (decimal.Parse(items[4], CultureInfo.InvariantCulture) * 1000).ToString();
                        row["cenazk"] = (decimal.Parse(items[3], CultureInfo.InvariantCulture) * 100).ToString();
                        row["cenasp"] = (decimal.Parse(items[5], CultureInfo.InvariantCulture) * 100).ToString();
                        table.Rows.Add(row);
                    }
                }
               


                StringBuilder sb = new StringBuilder();
                foreach (DataRow row in table.Rows)
                {
                    IEnumerable<string> fields = row.ItemArray.Select(field => field.ToString());
                    sb.AppendLine(string.Join(";", fields));
                }

                File.WriteAllText(filename, sb.ToString());

            }
            catch (SocketException)
            {
                MessageBox.Show("Błąd sieci. Nie można połączyć się z agentem");

            }

        
        
            

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Inwentimp_Shown(object sender, EventArgs e)
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
