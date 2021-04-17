using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using InTheHand.Net;
using InTheHand.Net.Ports;
using InTheHand.Net.Bluetooth;
using InTheHand.Windows.Forms;
using InTheHand.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Globalization;


namespace Undersoft.Picatch.Agent.BT
{
    public partial class PICBTAGENT : Form
    
    {
        private BluetoothAddress[] btadress;
        private int index;
        private Undersoft.Picatch.Agent.Main main;
        private bool setexpstat;
        private int poszlo;
        public PICBTAGENT(Undersoft.Picatch.Agent.Main form)



        {

            FileStream Fs = new FileStream("btdevices.dat", FileMode.Open, FileAccess.Read);
            btadress = new BluetoothAddress[Fs.Length / 6];
            byte[] btopen = new byte[6];
            string[] stdevice = new string[Fs.Length / 6]; 
            int recb = 6;
            for (int i = 0; i < Fs.Length / 6; i++)
            {


                Fs.Read(btopen, 0, recb);
                stdevice[i] = Encoding.UTF8.GetString(btopen, 0, recb);
                btadress[i] = new BluetoothAddress(btopen);
            }

            Fs.Close();
            
            InitializeComponent();
            main = form;
            BackgroundWorker bw = new BackgroundWorker
            {
                WorkerReportsProgress = true,
                WorkerSupportsCancellation = true
            };
            bw.DoWork += (bwSender, bwArg) =>
            {
                //what happens here must not touch the form
                //as it's in a different thread        
                sendreciv_bt(main, stdevice);

            };
            bw.RunWorkerAsync();

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;

            this.otwórzToolStripMenuItem.Click += otwórzToolStripMenuItem_Click;
            this.ukryjToolStripMenuItem.Click += ukryjToolStripMenuItem_Click;

        }



        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used
        bool mLoadFired;        // Form was shown once

        protected override void SetVisibleCore(bool value)
        {
            if (!mAllowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!mAllowClose)
            {
                this.Hide();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowVisible = true;
            mLoadFired = true;
            Show();
        }

        private void ukryjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowClose = mAllowVisible = true;
            if (!mLoadFired) Show();
            Close();
        }


        private void sendreciv_bt(Undersoft.Picatch.Agent.Main mainf, string[] device)
        {

           

            Guid fakeUuid = new Guid("{F13F471D-47CB-41d6-9609-BAD0690BF891}"); // A specially created value, so no matches.
            // Create client object
           
            for (; ; )
            {
                
                     
                bool inRange = false;
                for (int i = 0; i < btadress.Length; i++)
                 {
                    
                    Thread.Sleep(3000);
                    try
                    {
                        BluetoothDeviceInfo mac = new BluetoothDeviceInfo(btadress[i]);
                        ServiceRecord[] records = mac.GetServiceRecords(fakeUuid);
                        Debug.Assert(records.Length == 0, "Why are we getting any records?? len: " + records.Length);
                        inRange = true;
                        index = i;

                    }
                    catch (SocketException)
                    {
                        inRange = false;

                    }




                    if (inRange == true)
                    {

                        try
                        {
                            BluetoothClient cli = new BluetoothClient();
                            cli.Connect(new BluetoothEndPoint(btadress[index], BluetoothService.SerialPort));
                            byte[] btstring = btadress[index].ToByteArray();
                            string deviceadress = Encoding.UTF8.GetString(btstring, 0, btstring.Length);
                            poszlo = 0;
                            bool czyistnieje = false;
                            Stream peerStream = cli.GetStream();
                            byte[] flaga = new byte[1];
                            flaga[0] = 10;
                            byte[] bytecount = new byte[1024];
                            byte[] datebyte = new byte[128];
                            //Thread.Sleep();
                            peerStream.Read(flaga, 0, 1);
                            Thread.Sleep(1000);
                            peerStream.Read(datebyte, 0 , 16);
                           // Thread.Sleep(1000);
                          // MessageBox.Show(System.Text.Encoding.UTF8.GetString(datebyte));
                            DateTime impdate = DateTime.Parse(System.Text.Encoding.ASCII.GetString(datebyte));
                            //DateTime impdate = DateTime.FromBinary(BitConverter.ToInt64(datebyte, 0)); 
                          //  MessageBox.Show(flaga.ToString()+ " " + impdate.ToString());
                            
                            if (flaga[0] == 1)
                            {
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                File.Delete("inwent.imp");              
                                
                                peerStream.Read(bytecount, 0, bytecount.Length);
                                
                                int lenght = BitConverter.ToInt32(bytecount, 0);
                                Thread.Sleep(2000);
                                
                                byte[] buf = new byte[1024];
                               
                                if (peerStream.CanRead == true)
                                {

                                    int remaining = lenght;
                                    while (remaining > 0)
                                    {
                                        int read = peerStream.Read(buf, 0, buf.Length);
                                        
                                        if (remaining > 0)
                                        {
                                            System.IO.FileStream file = new FileStream("inwent.imp", FileMode.Append, FileAccess.Write);
                                            file.Write(buf, 0, read);
                                            file.Close();
                                        }
                                        remaining -= read;
                                        peerStream.Flush();
                                    }



                                }
                              

                                    
                                
                                string delimeter = ";";
                                string filename = "inwent.imp";
                                StreamReader sr = new StreamReader(filename, Encoding.UTF8);
                                string allData = sr.ReadToEnd();
                                string[] rows = allData.Split("\r\n".ToCharArray());
                                allData = "empty";
                                sr.DiscardBufferedData();
                                sr.Close();
                                cn.Open();
                                SqlCeCommand cmd = cn.CreateCommand();
                                cmd.CommandText = "INSERT INTO bufor (dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
                                cmd.Parameters.Add("@d", SqlDbType.Int, 10);
                                cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100);
                                cmd.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                                cmd.Parameters.Add("@i", SqlDbType.Decimal, 10);
                                cmd.Parameters["i"].Precision = 10;
                                cmd.Parameters["i"].Scale = 3;
                                cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10);
                                cmd.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                                cmd.Parameters.Add("@v", SqlDbType.NVarChar, 10);
                                
                                cmd.Prepare();

                                SqlCeCommand cmdh = cn.CreateCommand();
                                cmdh.CommandText = "INSERT INTO dok (nazwadok, typ, data, device) VALUES (?, ?, ?, ?)";
                                cmdh.Parameters.Add("@d", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                cmdh.Parameters.Add("@n", SqlDbType.DateTime);
                                cmdh.Parameters.Add("@de", SqlDbType.NVarChar, 25);
                                cmdh.Prepare();
                                
                                SqlCeCommand cmdt = cn.CreateCommand();
                                cmdt.CommandText = "SELECT * From dok";
                                cmdt.Prepare();

                                SqlCeDataReader dr = cmdt.ExecuteReader();
                               
                                
                                string[] testn = new string[10000];
                                int l = 0;                         
                                
                                while (dr.Read())
                                {
                                    testn[l] = dr.GetString(1);
                                    l += 1;
                                }

                                string[] testnazwa = new string[l];

                                for (int y = 0; y < l; y++)
                                {
                                    testnazwa[y] = testn[y];
                                }
                                
                                int x = 0;
                                int dokid = 0;
                                
                                
                                foreach (string r in rows)
                                {
                                    string[] items = r.Split(delimeter.ToCharArray());

                                    
                                    
                                    if (x == 0)
                                    {
                                        for (int z = 0; z < testnazwa.Length; z++)
                                        {
                                            if (items[0] == testnazwa[z])
                                            {
                                                DialogResult result = MessageBox.Show("Plik istnieje, Czy zapisać drugą wersję", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                                                if (result == DialogResult.No)
                                                {
                                                    czyistnieje = true;
                                                    z = testnazwa.Length;
                                                }
                                                z = testnazwa.Length;
                                            }
                                        }


                                        if (items[0] != "" && czyistnieje == false)
                                        {
                                            cmdh.Parameters["@d"].Value = items[0].ToString(CultureInfo.CurrentCulture);
                                            cmdh.Parameters["@k"].Value = items[1];
                                            cmdh.Parameters["@n"].Value = items[2];
                                            cmdh.Parameters["@de"].Value = device[index];
                                            cmdh.ExecuteNonQuery();
                                            SqlCeCommand cmdi = cn.CreateCommand();
                                            cmdi.CommandText = "Select MAX(id) From dok";
                                            SqlCeDataReader dri = cmdi.ExecuteReader();
                                            while (dri.Read())
                                            {
                                                dokid = dri.GetInt32(0);
                                            }

                                            x += 1;
                                        }
                                    }
                                    else
                                    {

                                        if (items[0] != "" && czyistnieje == false)
                                        {
                                            
                                            cmd.Parameters["@d"].Value = dokid;
                                            cmd.Parameters["@k"].Value = items[1];
                                            cmd.Parameters["@n"].Value = items[2].ToString(CultureInfo.CurrentCulture);
                                            cmd.Parameters["@cz"].Value = items[3];
                                            cmd.Parameters["@i"].Value = decimal.Parse(items[4], System.Globalization.CultureInfo.InvariantCulture);
                                            cmd.Parameters["@s"].Value = items[5];
                                            cmd.Parameters["@cs"].Value = items[6];
                                            cmd.Parameters["@v"].Value = items[7];
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }
                                senddoc();
                                cn.Close();	
                                


                            }

                            if (flaga[0] == 8)
                            {
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                File.Delete("sprawdzarka.imp");

                                peerStream.Read(bytecount, 0, bytecount.Length);

                                int lenght = BitConverter.ToInt32(bytecount, 0);
                                Thread.Sleep(2000);

                                byte[] buf = new byte[1024];

                                if (peerStream.CanRead == true)
                                {

                                    int remaining = lenght;
                                    while (remaining > 0)
                                    {
                                        int read = peerStream.Read(buf, 0, buf.Length);

                                        if (remaining > 0)
                                        {
                                            System.IO.FileStream file = new FileStream("sprawdzarka.imp", FileMode.Append, FileAccess.Write);
                                            file.Write(buf, 0, read);
                                            file.Close();
                                        }
                                        remaining -= read;
                                        peerStream.Flush();
                                    }



                                }




                                string delimeter = ";";
                                string filename = "sprawdzarka.imp";
                                StreamReader sr = new StreamReader(filename);
                                string allData = sr.ReadToEnd();
                                string[] rows = allData.Split("\r\n".ToCharArray());
                                allData = "empty";
                                sr.DiscardBufferedData();
                                sr.Close();
                                cn.Open();
                                SqlCeCommand cmd8 = cn.CreateCommand();
                                cmd8.CommandText = "UPDATE dane SET cenapolka = ?, zliczono = ?, bad_cena = ?, bad_stan = ? WHERE kod = ?";


                                cmd8.Parameters.Add("@p", SqlDbType.Decimal, 10);
                                cmd8.Parameters["@p"].Precision = 10;
                                cmd8.Parameters["@p"].Scale = 3;
                                cmd8.Parameters.Add("@z", SqlDbType.Decimal, 10);
                                cmd8.Parameters["@z"].Precision = 10;
                                cmd8.Parameters["@z"].Scale = 3;
                                cmd8.Parameters.Add("@bc", SqlDbType.Bit);
                                cmd8.Parameters.Add("@bs", SqlDbType.Bit);
                                cmd8.Parameters.Add("@k", SqlDbType.NVarChar, 15);

                                cmd8.Prepare();
                                bool val;
                                string input;



                                input = bool.FalseString;
                                val = bool.Parse(input);

                                
                                
                                foreach (string r in rows)
                                {
                                    string[] items = r.Split(delimeter.ToCharArray());



                                    if (items[0] != "")
                                    {



                                        cmd8.Parameters["@p"].Value = decimal.Parse(items[3], System.Globalization.CultureInfo.InvariantCulture);
                                        cmd8.Parameters["@z"].Value = decimal.Parse(items[4], System.Globalization.CultureInfo.InvariantCulture);
                                        cmd8.Parameters["@bc"].Value = items[1];
                                        cmd8.Parameters["@bs"].Value = items[2];
                                        cmd8.Parameters["@k"].Value = items[0];
                                        cmd8.ExecuteNonQuery();
                                      
                                    }


                                }
                                cn.Close();

                               MessageBox.Show("Odebrano dokument z urządzenia");

                               
                             



                            }
                            
                           /* if (flaga[0] == 5)
                            {
                            File.Delete("inwent.exp");
                                StreamWriter sw = new StreamWriter("inwent.exp");
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                cn.Open();
                                SqlCeCommand err = cn.CreateCommand();
                                err.CommandText = "UPDATE devdane SET devstat = 'Err' WHERE device = ?)";
                               
                                err.Parameters["@de"].Value = System.Text.Encoding.UTF8.GetString(btstring);
                                flaga[0] = 0;
                            }
                            
                            */
                            if (flaga[0] == 0)
                            {
                                
                                
                                File.Delete("inwent.exp");
                                StreamWriter sw = new StreamWriter("inwent.exp");
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                cn.Open();
                                            
                                


                              //  SqlCeCommand drt1 = cn.CreateCommand();
                               // drt1.CommandText = "SELECT dane.typ, dane.kod, dane.nazwa, dane.stan, dane.cenazk, dane.cenasp, dane.vat, devdane.device from dane right join devdane on dane.kod = devdane.kod";
                              //  drt1.Prepare();
                                
                                
                             //   SqlCeDataReader drt2 = drt1.ExecuteReader();

                              //  SqlCeCommand drt3 = cn.CreateCommand();
                              //  drt3.CommandText = "SELECT dane.typ, dane.kod, dane.nazwa, dane.stan, dane.cenazk, dane.cenasp, dane.vat, devdane.device, devdane.devstat from devdane right join dane on devdane.kod = dane.kod and devdane.kod <> null and (devdane.stan <> dane.stan or devdane.cenazk <> dane.cenazk)";
                              //  drt3.Prepare();


                              //  SqlCeDataReader drt4 = drt3.ExecuteReader();



                              //  while (drt2.Read())
                              //  {




                                    
                              /*      SqlCeCommand cmd1 = cn.CreateCommand();
                              //      cmd1.CommandText = "INSERT INTO devdane (typ, kod, nazwa, stan, cenazk, cenasp, vat, device, devstat ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
                              //      cmd1.Parameters.Add("@t", SqlDbType.NVarChar, 10);
                                    cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                    cmd1.Parameters.Add("@n", SqlDbType.NVarChar, 100);
                                    cmd1.Parameters.Add("@s", SqlDbType.NVarChar, 10);
                                    cmd1.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                                    cmd1.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                                    cmd1.Parameters.Add("@v", SqlDbType.NVarChar, 10);
                                    cmd1.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                    cmd1.Parameters.Add("@st", SqlDbType.NVarChar, 20);
                                    cmd1.Parameters["@t"].Value = drt2.GetString(0);
                                    cmd1.Parameters["@k"].Value = drt2.GetString(1);
                                    cmd1.Parameters["@n"].Value = drt2.GetString(2);
                                    cmd1.Parameters["@s"].Value = drt2.GetString(3);
                                    cmd1.Parameters["@cz"].Value = drt2.GetString(4);
                                    cmd1.Parameters["@cs"].Value = drt2.GetString(5);
                                    cmd1.Parameters["@v"].Value = drt2.GetString(6);
                                    cmd1.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring);
                                    cmd1.Parameters["@st"].Value = "New";
                                    cmd1.Prepare();
                                    cmd1.ExecuteNonQuery();



                                }

                                while (drt4.Read())
                                {
                                    SqlCeCommand cmd2 = cn.CreateCommand();
                                    cmd2.CommandText = "UPDATE devdane SET typ = ?, nazwa = ?, stan = ?, cenazk = ?, cenasp = ?, vat = ?, devstat = ? WHERE kod = ? AND device = ?)";
                                    cmd2.Parameters.Add("@t", SqlDbType.NVarChar, 10);
                                    cmd2.Parameters.Add("@n", SqlDbType.NVarChar, 100);
                                    cmd2.Parameters.Add("@s", SqlDbType.NVarChar, 10);
                                    cmd2.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                                    cmd2.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                                    cmd2.Parameters.Add("@v", SqlDbType.NVarChar, 10);
                                    cmd2.Parameters.Add("@st", SqlDbType.NVarChar, 20);
                                    cmd2.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                    cmd2.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                   
                                    
                                    cmd2.Parameters["@t"].Value = drt4.GetString(0);
                                    cmd2.Parameters["@n"].Value = drt4.GetString(2);
                                    cmd2.Parameters["@s"].Value = drt4.GetString(3);
                                    cmd2.Parameters["@cz"].Value = drt4.GetString(4);
                                    cmd2.Parameters["@cs"].Value = drt4.GetString(5);
                                    cmd2.Parameters["@v"].Value = drt4.GetString(6);
                                    cmd2.Parameters["@st"].Value = "Update";
                                    cmd2.Parameters["@k"].Value = drt4.GetString(1);
                                    cmd2.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring);
                                   
                                    cmd2.Prepare();
                                    cmd2.ExecuteNonQuery();
                                }
                                */
                               

                                SqlCeCommand cmd = cn.CreateCommand();
                                cmd.CommandText = "SELECT * FROM dane where cast(stan as decimal) > 0 AND datazmian > ? ORDER BY datazmian";
                                cmd.Parameters.Add("@data", SqlDbType.DateTime);
                                cmd.Prepare();
                                cmd.Parameters["@data"].Value = impdate;
                               // cmd.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                //cmd.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring, 0, btstring.Length);
                                SqlCeDataReader dr = cmd.ExecuteReader();
                                
                               
                                
                                while (dr.Read())
                                {

                                    //string send = dr.GetString(8);

                                    //if (send != "Sended")
                                    //{

                                    sw.WriteLine(dr.GetString(0) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + dr.GetString(4) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetDateTime(12).ToString() + ";" + dr.GetString(13) + ";" + dr.GetString(14));
                                        
                                 //   }
                                }
                                
                                sw.Close();
                                cn.Close();
                                
                                
                               
                                
                                byte[] buf = File.ReadAllBytes("inwent.exp");
                                int lenght = buf.Length;
                                byte[] bytecount2 = BitConverter.GetBytes(lenght);
                                peerStream.Write(bytecount2, 0, bytecount2.Length);
                                System.Threading.Thread.Sleep(2000);
                                peerStream.Write(buf, 0, buf.Length);
                                System.Threading.Thread.Sleep(1000);
                                poszlo = 1;

                            }
                            else if (flaga[0] == 19)
                            {
                                System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                                nfi.NumberDecimalSeparator = ".";
                                File.Delete("stany.exp");
                                StreamWriter sw = new StreamWriter("stany.exp");
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                cn.Open();




                                //  SqlCeCommand drt1 = cn.CreateCommand();
                                // drt1.CommandText = "SELECT dane.typ, dane.kod, dane.nazwa, dane.stan, dane.cenazk, dane.cenasp, dane.vat, devdane.device from dane right join devdane on dane.kod = devdane.kod";
                                //  drt1.Prepare();


                                //   SqlCeDataReader drt2 = drt1.ExecuteReader();

                                //  SqlCeCommand drt3 = cn.CreateCommand();
                                //  drt3.CommandText = "SELECT dane.typ, dane.kod, dane.nazwa, dane.stan, dane.cenazk, dane.cenasp, dane.vat, devdane.device, devdane.devstat from devdane right join dane on devdane.kod = dane.kod and devdane.kod <> null and (devdane.stan <> dane.stan or devdane.cenazk <> dane.cenazk)";
                                //  drt3.Prepare();


                                //  SqlCeDataReader drt4 = drt3.ExecuteReader();



                                //  while (drt2.Read())
                                //  {





                                /*      SqlCeCommand cmd1 = cn.CreateCommand();
                                //      cmd1.CommandText = "INSERT INTO devdane (typ, kod, nazwa, stan, cenazk, cenasp, vat, device, devstat ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                //      cmd1.Parameters.Add("@t", SqlDbType.NVarChar, 10);
                                      cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                      cmd1.Parameters.Add("@n", SqlDbType.NVarChar, 100);
                                      cmd1.Parameters.Add("@s", SqlDbType.NVarChar, 10);
                                      cmd1.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                                      cmd1.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                                      cmd1.Parameters.Add("@v", SqlDbType.NVarChar, 10);
                                      cmd1.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                      cmd1.Parameters.Add("@st", SqlDbType.NVarChar, 20);
                                      cmd1.Parameters["@t"].Value = drt2.GetString(0);
                                      cmd1.Parameters["@k"].Value = drt2.GetString(1);
                                      cmd1.Parameters["@n"].Value = drt2.GetString(2);
                                      cmd1.Parameters["@s"].Value = drt2.GetString(3);
                                      cmd1.Parameters["@cz"].Value = drt2.GetString(4);
                                      cmd1.Parameters["@cs"].Value = drt2.GetString(5);
                                      cmd1.Parameters["@v"].Value = drt2.GetString(6);
                                      cmd1.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring);
                                      cmd1.Parameters["@st"].Value = "New";
                                      cmd1.Prepare();
                                      cmd1.ExecuteNonQuery();



                                  }

                                  while (drt4.Read())
                                  {
                                      SqlCeCommand cmd2 = cn.CreateCommand();
                                      cmd2.CommandText = "UPDATE devdane SET typ = ?, nazwa = ?, stan = ?, cenazk = ?, cenasp = ?, vat = ?, devstat = ? WHERE kod = ? AND device = ?)";
                                      cmd2.Parameters.Add("@t", SqlDbType.NVarChar, 10);
                                      cmd2.Parameters.Add("@n", SqlDbType.NVarChar, 100);
                                      cmd2.Parameters.Add("@s", SqlDbType.NVarChar, 10);
                                      cmd2.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                                      cmd2.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                                      cmd2.Parameters.Add("@v", SqlDbType.NVarChar, 10);
                                      cmd2.Parameters.Add("@st", SqlDbType.NVarChar, 20);
                                      cmd2.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                      cmd2.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                   
                                    
                                      cmd2.Parameters["@t"].Value = drt4.GetString(0);
                                      cmd2.Parameters["@n"].Value = drt4.GetString(2);
                                      cmd2.Parameters["@s"].Value = drt4.GetString(3);
                                      cmd2.Parameters["@cz"].Value = drt4.GetString(4);
                                      cmd2.Parameters["@cs"].Value = drt4.GetString(5);
                                      cmd2.Parameters["@v"].Value = drt4.GetString(6);
                                      cmd2.Parameters["@st"].Value = "Update";
                                      cmd2.Parameters["@k"].Value = drt4.GetString(1);
                                      cmd2.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring);
                                   
                                      cmd2.Prepare();
                                      cmd2.ExecuteNonQuery();
                                  }
                                  */


                                SqlCeCommand cmd = cn.CreateCommand();
                                cmd.CommandText = "SELECT     stany.id, stany.kod, stany.stan, stany.MagId, stany.Nazwa, stany.datazmian FROM   stany INNER JOIN  dane ON stany.kod = dane.kod  WHERE (stany.datazmian > ?) AND (CAST(dane.stan AS decimal) > 0) order by datazmian";
                                cmd.Parameters.Add("@data", SqlDbType.DateTime);
                                cmd.Prepare();
                                cmd.Parameters["@data"].Value = impdate;
                                // cmd.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                //cmd.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring, 0, btstring.Length);
                                SqlCeDataReader dr = cmd.ExecuteReader();



                                while (dr.Read())
                                {

                                    //string send = dr.GetString(8);

                                    //if (send != "Sended")
                                    //{

                                    sw.WriteLine(dr.GetString(1) + ";" + dr.GetDecimal(2).ToString(nfi) + ";" + dr.GetInt32(3).ToString() + ";" + dr.GetString(4) + ";" + dr.GetDateTime(5).ToString());

                                    //   }
                                }

                                dr.Close();

                                SqlCeCommand cmd2 = cn.CreateCommand();
                                cmd2.CommandText = "SELECT     magazyn.Nazwa, magazyn.MagId FROM magazyn";
                                
                                cmd2.Prepare();
                                
                                // cmd.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                //cmd.Parameters["@de"].Value = Encoding.UTF8.GetString(btstring, 0, btstring.Length);
                                SqlCeDataReader dr2 = cmd2.ExecuteReader();

                                while (dr2.Read())
                                {

                                    //string send = dr.GetString(8);

                                    //if (send != "Sended")
                                    //{

                                    sw.WriteLine("MAG;" + dr2.GetString(0) + ";" + dr2.GetInt32(1).ToString());

                                    //   }
                                }


                                sw.Close();
                                cn.Close();




                                byte[] buf = File.ReadAllBytes("stany.exp");
                                int lenght = buf.Length;
                                byte[] bytecount2 = BitConverter.GetBytes(lenght);
                              //  Thread.Sleep(1000);
                                peerStream.Write(bytecount2, 0, bytecount2.Length);
                                System.Threading.Thread.Sleep(1200);
                                peerStream.Write(buf, 0, buf.Length);
                                System.Threading.Thread.Sleep(1000);
                                poszlo = 1;

                            }
                                
                            else if (flaga[0] == 21)
                            {
                                setexpstat = false;
                                StreamWriter sw = new StreamWriter("edi.exp");
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                cn.Open();
                                
                                SqlCeCommand cmd = cn.CreateCommand();
                                cmd.CommandText = "SELECT * FROM edihead WHERE status = 'Nowy'";
                                SqlCeDataReader dr = cmd.ExecuteReader();
                                
                                while (dr.Read())
                                {
                                    
                                        sw.WriteLine("edihead;" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + dr.GetString(4) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetString(7) + ";" + dr.GetString(8) + ";" + dr.GetString(9) + ";" + dr.GetString(10) + ";" + dr.GetString(11) + ";" + dr.GetString(12) + ";" + dr.GetString(13) + ";" + dr.GetString(14) + ";" + dr.GetString(15) + ";" + dr.GetString(16) + ";" + dr.GetString(17) + ";" + dr.GetString(18) + ";" + dr.GetString(19) + ";" + dr.GetString(20) + ";" + dr.GetString(21) + ";" + dr.GetString(22) + ";" + dr.GetString(23) + ";" + dr.GetString(24) + ";" + dr.GetString(25) + ";" + dr.GetString(26) + ";" + dr.GetString(27) + ";" + dr.GetString(28) + ";" + dr.GetString(29) + ";" + dr.GetString(30) + ";" + dr.GetString(31) + ";" + dr.GetString(32) + ";" + (Convert.ToInt32(dr.GetBoolean(33)).ToString()));
                                    
                                }


                                cmd.CommandText = "SELECT NrDok, Nazwa, kod, Vat, Jm, Asortyment, Sww, PKWiU, Cast(REPLACE(SUM(CONVERT(numeric(10,3), Ilosc)), ',','.') as nvarchar(10)), Cena, Wartosc, IleWOpak, CenaSp, status, complete FROM edibody GROUP BY NrDok, Nazwa, kod, Vat, Jm, Asortyment, Sww, PKWiU, Cena, Wartosc, IleWOpak, CenaSp, status, complete";
                                dr = cmd.ExecuteReader();

                                while (dr.Read())
                                {

                                    sw.WriteLine("edibody;" + dr.GetString(0) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + dr.GetString(4) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetString(7) + ";" + dr.GetString(8) + ";" + dr.GetString(9) + ";" + dr.GetString(10) + ";" + dr.GetString(11) + ";" + dr.GetString(12) + ";" + dr.GetString(13) + ";" + (Convert.ToInt32(dr.GetBoolean(14)).ToString()));
                                   
                                }

                                cmd.CommandText = "SELECT *  FROM ediend";
                                dr = cmd.ExecuteReader();

                                while (dr.Read())
                                {

                                    sw.WriteLine("ediend;" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + dr.GetString(4) + ";" + dr.GetString(5) + ";" + (Convert.ToInt32(dr.GetBoolean(6)).ToString()));
                                   
                                }

                              


                                sw.Close();
                                cn.Close();



                                byte[] buf = File.ReadAllBytes("edi.exp");
                                int lenght = buf.Length;
                                byte[] bytecount2 = BitConverter.GetBytes(lenght);
                                peerStream.Write(bytecount2, 0, bytecount2.Length);
                                System.Threading.Thread.Sleep(3000);
                                peerStream.Write(buf, 0, buf.Length);
                                System.Threading.Thread.Sleep(3000);
                                setexpstat = true;

                            }
                            else if (flaga[0] == 22)
                            {
                                File.Delete("edi.imp");     
                                peerStream.Read(bytecount, 0, bytecount.Length);

                                int lenght = BitConverter.ToInt32(bytecount, 0);
                                Thread.Sleep(2000);

                                byte[] buf = new byte[1024];

                                if (peerStream.CanRead == true)
                                {

                                    int remaining = lenght;
                                    while (remaining > 0)
                                    {
                                        int read = peerStream.Read(buf, 0, buf.Length);

                                        if (remaining > 0)
                                        {
                                            System.IO.FileStream file = new FileStream("edi.imp", FileMode.Append, FileAccess.Write);
                                            file.Write(buf, 0, read);
                                            file.Close();
                                        }
                                        remaining -= read;
                                        peerStream.Flush();
                                    }



                                }


                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);


                                string delimeter = ";";
                                string filename = "edi.imp";
                                StreamReader sr = new StreamReader(filename);
                                string allData = sr.ReadToEnd();
                                string[] rows = allData.Split("\r\n".ToCharArray());
                                allData = "empty";
                                sr.DiscardBufferedData();
                                sr.Close();
                                cn.Open();
                                SqlCeCommand cmdh = cn.CreateCommand();
                                cmdh.CommandText = "INSERT INTO fedihead (FileName, TypPolskichLiter, TypDok, NrDok, Data, DataRealizacji, Magazyn, SposobPlatn, TerminPlatn, IndeksCentralny, NazwaWystawcy, AdresWystawcy, KodWystawcy, MiastoWystawcy, UlicaWystawcy, NIPWystawcy, BankWystawcy, KontoWystawcy, TelefonWystawcy, NrWystawcyWSieciSklepow, NazwaOdbiorcy, AdresOdbiorcy, KodOdbiorcy, MiastoOdbiorcy, UlicaOdbiorcy, NIPOdbiorcy, BankOdbiorcy, KontoOdbiorcy, TelefonOdbiorcy, NrOdbiorcyWSieciSklepow, DoZaplaty, status, complete) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                cmdh.Parameters.Add("@0", SqlDbType.NVarChar, 40);
                                cmdh.Parameters.Add("@1", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@2", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@3", SqlDbType.NVarChar, 30);
                                cmdh.Parameters.Add("@4", SqlDbType.NVarChar, 30);
                                cmdh.Parameters.Add("@5", SqlDbType.NVarChar, 30);
                                cmdh.Parameters.Add("@6", SqlDbType.NVarChar, 30);
                                cmdh.Parameters.Add("@7", SqlDbType.NVarChar, 10);
                                cmdh.Parameters.Add("@8", SqlDbType.NVarChar, 10);
                                cmdh.Parameters.Add("@9", SqlDbType.NVarChar, 10);
                                cmdh.Parameters.Add("@10", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@11", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@12", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@13", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@14", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@15", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@16", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@17", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@18", SqlDbType.NVarChar, 30);
                                cmdh.Parameters.Add("@19", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@20", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@21", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@22", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@23", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@24", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@25", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@26", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@27", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@28", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@29", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@30", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@31", SqlDbType.NVarChar, 20);
                                cmdh.Parameters.Add("@32", SqlDbType.Bit);
                                cmdh.Prepare();
                                SqlCeCommand cmdb = cn.CreateCommand();
                                cmdb.CommandText = "INSERT INTO fedibody (NrDok, Nazwa, kod, Vat, Jm, Asortyment, Ilosc, Cena, IleWOpak, CenaSp) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                cmdb.Parameters.Add("@1", SqlDbType.NVarChar, 20);
                                cmdb.Parameters.Add("@2", SqlDbType.NVarChar, 120);
                                cmdb.Parameters.Add("@3", SqlDbType.NVarChar, 20);
                                cmdb.Parameters.Add("@4", SqlDbType.NVarChar, 10);
                                cmdb.Parameters.Add("@5", SqlDbType.NVarChar, 10);
                                cmdb.Parameters.Add("@6", SqlDbType.NVarChar, 120);
                                cmdb.Parameters.Add("@7", SqlDbType.NVarChar, 20);
                                cmdb.Parameters.Add("@8", SqlDbType.NVarChar, 20);
                                cmdb.Parameters.Add("@9", SqlDbType.NVarChar, 10);
                                cmdb.Parameters.Add("@10", SqlDbType.NVarChar, 10);
                               
                                cmdb.Prepare();
                                //SqlCeCommand cmde = cn.CreateCommand();
                                //cmde.CommandText = "INSERT INTO ediend (NrDok, Vat, SumaNet, SumaVat, status, complete) VALUES (?, ?, ?, ?, ?, ?)";
                                //cmde.Parameters.Add("@1", SqlDbType.NVarChar, 30);
                                //cmde.Parameters.Add("@2", SqlDbType.NVarChar, 20);
                                //cmde.Parameters.Add("@3", SqlDbType.NVarChar, 20);
                                //cmde.Parameters.Add("@4", SqlDbType.NVarChar, 20);
                                //cmde.Parameters.Add("@5", SqlDbType.NVarChar, 20);
                                //cmde.Parameters.Add("@6", SqlDbType.Bit);
                                //cmde.Prepare();
                               
                              
                                foreach (string r in rows)
                                {
                                    


                                    string[] items = r.Split(delimeter.ToCharArray());
                                    if (items[0] != "" && items[0] == "edihead")
                                    {
                                        cmdh.Parameters["@0"].Value = items[1];
                                        cmdh.Parameters["@1"].Value = items[2];
                                        cmdh.Parameters["@2"].Value = items[3];
                                        cmdh.Parameters["@3"].Value = items[4];
                                        cmdh.Parameters["@4"].Value = items[5];
                                        cmdh.Parameters["@5"].Value = items[6];
                                        cmdh.Parameters["@6"].Value = items[7];
                                        cmdh.Parameters["@7"].Value = items[8];
                                        cmdh.Parameters["@8"].Value = items[9];
                                        cmdh.Parameters["@9"].Value = items[10];
                                        cmdh.Parameters["@10"].Value = items[11];
                                        cmdh.Parameters["@11"].Value = items[12];
                                        cmdh.Parameters["@12"].Value = items[13];
                                        cmdh.Parameters["@13"].Value = items[14];
                                        cmdh.Parameters["@14"].Value = items[15];
                                        cmdh.Parameters["@15"].Value = items[16];
                                        cmdh.Parameters["@16"].Value = items[17];
                                        cmdh.Parameters["@17"].Value = items[18];
                                        cmdh.Parameters["@18"].Value = items[19];
                                        cmdh.Parameters["@19"].Value = items[20];
                                        cmdh.Parameters["@20"].Value = items[21];
                                        cmdh.Parameters["@21"].Value = items[22];
                                        cmdh.Parameters["@22"].Value = items[23];
                                        cmdh.Parameters["@23"].Value = items[24];
                                        cmdh.Parameters["@24"].Value = items[25];
                                        cmdh.Parameters["@25"].Value = items[26];
                                        cmdh.Parameters["@26"].Value = items[27];
                                        cmdh.Parameters["@27"].Value = items[28];
                                        cmdh.Parameters["@28"].Value = items[29];
                                        cmdh.Parameters["@29"].Value = items[30];
                                        cmdh.Parameters["@30"].Value = items[31];
                                        cmdh.Parameters["@31"].Value = items[32];
                                        cmdh.Parameters["@32"].Value = items[33];
                                        cmdh.ExecuteNonQuery();
                                    }

                                    if (items[0] != "" && items[0] == "edibody")
                                    {
                                        cmdb.Parameters["@1"].Value = items[1];
                                        cmdb.Parameters["@2"].Value = items[2];
                                        cmdb.Parameters["@3"].Value = items[3];
                                        cmdb.Parameters["@4"].Value = items[4];
                                        cmdb.Parameters["@5"].Value = items[5];
                                        cmdb.Parameters["@6"].Value = items[6];
                                        cmdb.Parameters["@7"].Value = items[7];
                                        cmdb.Parameters["@8"].Value = items[8];
                                        cmdb.Parameters["@9"].Value = items[9];
                                        cmdb.Parameters["@10"].Value = items[10];
                                        
                                        cmdb.ExecuteNonQuery();
                                    }

                                  //  if (items[0] != "" && items[0] == "ediend")
                                   // {
                                    //    cmde.Parameters["@1"].Value = items[1];
                                     //   cmde.Parameters["@2"].Value = items[2];
                                      //  cmde.Parameters["@3"].Value = items[3];
                                       // cmde.Parameters["@4"].Value = items[4];
                                        //cmde.Parameters["@5"].Value = items[5];
                                        //cmde.Parameters["@6"].Value = byte.Parse(items[6]);
                                        //cmde.ExecuteNonQuery();
                                   // }

                                }

                                cn.Close();
                                

                            }



                            peerStream.Read(flaga, 0, 1);

                            if (flaga[0] == 3)
                            {
                                peerStream.Dispose();
                                peerStream.Close();
                                cli.Dispose();
                                cli.Close();
                                if (czyistnieje == false)
                                {
                                    MessageBox.Show("Odebrano dokument z urządzenia");
                                   
                                }
                                else
                                {
                                    MessageBox.Show("Anulowano odbiór dokumentu");
                                }
                           //     Thread.Sleep(1000);

                            }
                            else if (flaga[0] == 2)
                            {
                                
                                peerStream.Dispose();
                                peerStream.Close(); 
                                cli.Dispose();
                                cli.Close();
                               // MessageBox.Show("Wysłano Dane do urządzenia");
                               // Thread.Sleep(1000);
                                
                                string connectionString;
                                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                                    cn.Open();
                              //  if (poszlo == 1)
                                //{
                                //SqlCeCommand cmd2 = cn.CreateCommand();
                                  //  cmd2.CommandText = "UPDATE devdane SET devstat = 'Sended' WHERE device = ? and devstat <> 'Sended'";
                                                            
                                    //cmd2.Parameters.Add("@de", SqlDbType.NVarChar, 20);
                                    //cmd2.Parameters["@de"].Value = System.Text.Encoding.UTF8.GetString(btstring);
                                    //
                                    //cmd2.Prepare();
                                    //cmd2.ExecuteNonQuery();
                               // }

                                if (setexpstat == true)
                                {
                                   
                                    SqlCeCommand cmdh = cn.CreateCommand();
                                    cmdh.CommandText = "UPDATE edihead SET status = 'Wyslany' Where status = 'Nowy'";
                                    
                                }
                                cn.Close();
                            }
                            
                            

                            
                        }
                        catch (SocketException e)
                        {
                           
                        }
                    }
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            this.Close();
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            this.Close();
        }
      
        public void senddoc()
        {
            StreamWriter sw = new StreamWriter("picatch.imp");

            int dokid = 0;
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmdi = cn.CreateCommand();
            cmdi.CommandText = "Select MAX(id) From dok";
            SqlCeDataReader dri = cmdi.ExecuteReader();
            while (dri.Read())
            {
                dokid = dri.GetInt32(0);
            }
            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat  FROM bufor WHERE dokid = ?";
            cmd.Parameters.Add("@d", SqlDbType.Int);
            cmd.Prepare();
            cmd.Parameters["@d"].Value = dokid;
            SqlCeDataReader dr = cmd.ExecuteReader();
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            while (dr.Read())
            {
                decimal n = dr.GetDecimal(4);
                int did = dr.GetInt32(0);
                sw.WriteLine(Convert.ToString(did) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + n.ToString(nfi) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetString(7));
            }

            sw.Close();
            cn.Close();
        }
    }
}
