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
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Globalization;

namespace Undersoft.Picatch.Agent.TCP
{
    
    public partial class PICTCPAGENT : Form
    {
        BackgroundWorker bw = new BackgroundWorker
        {
            WorkerReportsProgress = true,
            WorkerSupportsCancellation = true
        };
        TcpListener Listener = null;
        private int porttcp;
        // bool czyuruchom = true;
        private Undersoft.Picatch.Agent.Main main;
        private bool setexpstat;
        
        
        public PICTCPAGENT(Undersoft.Picatch.Agent.Main form)
        {
            InitializeComponent();
            string connectionString;
            // string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmd2 = cn.CreateCommand();
            cmd2.CommandText = "SELECT * FROM opcje WHERE id = 1";
            cmd2.Prepare();
            SqlCeDataReader dr = cmd2.ExecuteReader();



            while (dr.Read())
            {


                // ip_t.Text = dr.GetString(3);
                // ufile_t.Text = dr.GetString(4);
                // dfile_t.Text = dr.GetString(5);

                //bflag = dr.GetBoolean(7);
                //ipflag = dr.GetBoolean(8);
                porttcp = dr.GetInt32(9);
                //skaner_t.Text = dr.GetString(10);
            }
            cn.Close();
            main = form;
            bw.DoWork += (bwSender, bwArg) =>
            {
                //what happens here must not touch the form
                //as it's in a different thread        
                ReceiveTCP(main);
                
            };
            bw.RunWorkerAsync();
          notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            
            this.otwórzToolStripMenuItem.Click += otwórzToolStripMenuItem_Click;
            this.zakończToolStripMenuItem.Click += zakończToolStripMenuItem_Click;
           
        }

        	
        

        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used
     //   bool mLoadFired;        // Form was shown once

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
          //  mLoadFired = true;
            Show();
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            this.Close();
        }
        
        
        
        

        public void ReceiveTCP(Undersoft.Picatch.Agent.Main mainf)
        {
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            try
            {
                Listener = new TcpListener(IPAddress.Any, porttcp);
                
                    Listener.Start();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            byte[] RecData = new byte[1024];
            int RecBytes;
            
               for( ; ;)
                {

                   if (bw.CancellationPending != true)
                   {
                    TcpClient client = null;
                    NetworkStream netstream = null;
                    string Status = string.Empty;
                    bool czyistnieje = false;
                    
                    try
                    {


                        if (!Listener.Pending())
                        {
                        }
                        else
                        {
                            
                            
                            client = Listener.AcceptTcpClient();
                            netstream = client.GetStream();
                            Status = "Klient połączony\n";
                            byte[] sendorec = new byte[1];
                            sendorec[0] = 0;
                            netstream.Read(sendorec, 0, 1);

                            if (sendorec[0] == 1)
                            {

                                int totalrecbytes = 0;
                                FileStream Fs = new FileStream("inwent.imp", FileMode.Create, FileAccess.Write);
                                while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                                {
                                    Fs.Write(RecData, 0, RecBytes);
                                    totalrecbytes += RecBytes;
                                }

                                Fs.Close();
                                netstream.Close();
                                client.Close();

                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);


                                string delimeter = ";";
                                string filename = "inwent.imp";
                                StreamReader sr = new StreamReader(filename);
                                string allData = sr.ReadToEnd();
                                string[] rows = allData.Split("\n".ToCharArray());
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
                                cmdh.CommandText = "INSERT INTO dok (nazwadok, typ, data) VALUES (?, ?, ?)";
                                cmdh.Parameters.Add("@d", SqlDbType.NVarChar, 120);
                                cmdh.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                cmdh.Parameters.Add("@n", SqlDbType.DateTime);
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
                                            cmdh.Parameters["@d"].Value = items[0];
                                            cmdh.Parameters["@k"].Value = items[1];
                                            cmdh.Parameters["@n"].Value = Convert.ToDateTime(items[2]);
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
                                            cmd.Parameters["@n"].Value = items[2];
                                            cmd.Parameters["@cz"].Value = items[3];
                                            cmd.Parameters["@i"].Value = Convert.ToDecimal(items[4], nfi);
                                            cmd.Parameters["@s"].Value = items[5];
                                            cmd.Parameters["@cs"].Value = items[6];
                                            cmd.Parameters["@v"].Value = items[7];
                                            cmd.ExecuteNonQuery();
                                        }
                                    }
                                }

                                cn.Close();

                                if (czyistnieje == false)
                                {
                                    MessageBox.Show("Odebrano dokument z urządzenia");
                                    senddoc();
                                }
                                else
                                {
                                    MessageBox.Show("Anulowano odbiór dokumentu");
                                }
                            }

                            else if (sendorec[0] == 8)
                            {

                                int totalrecbytes = 0;
                                FileStream Fs = new FileStream("sprawdzarka.imp", FileMode.Create, FileAccess.Write);
                                while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                                {
                                    Fs.Write(RecData, 0, RecBytes);
                                    totalrecbytes += RecBytes;
                                }

                                Fs.Close();
                                netstream.Close();
                                client.Close();

                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);


                                string delimeter = ";";
                                string filename = "sprawdzarka.exp";
                                StreamReader sr = new StreamReader(filename, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage), true);
                                string allData = sr.ReadToEnd();
                                string[] rows = allData.Split("\n".ToCharArray());
                                allData = "empty";
                                sr.DiscardBufferedData();
                                sr.Close();
                                cn.Open();
                                SqlCeCommand cmd = cn.CreateCommand();
                                cmd.CommandText = "UPDATE dane SET cenapolka = ?, zliczono = ?, bad_cena = ?, bad_stan = ? WHERE kod = ?";


                                cmd.Parameters.Add("@p", SqlDbType.Decimal, 10);
                                cmd.Parameters["@p"].Precision = 10;
                                cmd.Parameters["@p"].Scale = 3;
                                cmd.Parameters.Add("@z", SqlDbType.Decimal, 10);
                                cmd.Parameters["@z"].Precision = 10;
                                cmd.Parameters["@z"].Scale = 3;
                                cmd.Parameters.Add("@bc", SqlDbType.Bit);
                                cmd.Parameters.Add("@bs", SqlDbType.Bit);
                                cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                
                                cmd.Prepare();
                              
                                foreach (string r in rows)
                                {
                                    string[] items = r.Split(delimeter.ToCharArray());



                                   if (items[0] != "")
                                        {
                                            cmd.Parameters["@p"].Value = items[3];
                                            cmd.Parameters["@z"].Value = items[4];
                                            cmd.Parameters["@bc"].Value = items[1];
                                            cmd.Parameters["@bs"].Value = items[2];
                                            cmd.Parameters["@k"].Value = items[0];
                                            cmd.ExecuteNonQuery();
                                          
                                        }
                                   
                              
                                    }                             
                                cn.Close();

                                MessageBox.Show("Odebrano dokument z urządzenia");
                                  
                              
                            } 
                    
                            else if (sendorec[0] == 2)
                            {


                                File.Delete("picatch.exp");
                                byte[] SendingBuffer = null;
                                
                                StreamWriter sw = new StreamWriter("picatch.exp", true, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage));
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);
                                cn.Open();

                                SqlCeCommand cmd = cn.CreateCommand();
                                cmd.CommandText = "SELECT *  FROM dane";
                                SqlCeDataReader dr = cmd.ExecuteReader();

                                while (dr.Read())
                                {

                                    sw.WriteLine(dr.GetString(0) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + dr.GetString(4) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetDateTime(12).ToString());
                                }

                                sw.Close();
                                cn.Close();
                                
                                
                                FileStream Fs = new FileStream("picatch.exp", FileMode.Open, FileAccess.Read);
                                
                                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(1024)));
                                int TotalLength = (int)Fs.Length, CurrentPacketLength;
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
                            else if (sendorec[0] == 11)
                            {
                                int totalrecbytes = 0;
                                FileStream Fs = new FileStream("inwent.exp", FileMode.Create, FileAccess.Write);
                                while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                                {
                                    Fs.Write(RecData, 0, RecBytes);
                                    totalrecbytes += RecBytes;
                                }

                                Fs.Close();
                                netstream.Close();
                                client.Close();
                                string connectionString;
                                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                                SqlCeConnection cn = new SqlCeConnection(connectionString);


                                string delimeter = ";";

                                StreamReader sr = new StreamReader("inwent.exp", Encoding.UTF8, true);
                                string allData = sr.ReadToEnd();
                                string[] rows = allData.Split("\n".ToCharArray());
                                allData = "empty";
                                sr.DiscardBufferedData();
                                sr.Close();
                                cn.Open();
                                SqlCeCommand delete = cn.CreateCommand();
                                delete.CommandText = "DROP TABLE dane";
                                delete.Prepare();
                                delete.ExecuteNonQuery();
                                SqlCeCommand cmd = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(10,3), datazmian datetime)", cn);
                                cmd.ExecuteNonQuery();
                                cmd.CommandText = "INSERT INTO dane (typ, kod, nazwa, stan, cenazk, cenasp, vat) VALUES (?, ?, ?, ?, ?, ?, ?)";
                                cmd.Parameters.Add("@t", SqlDbType.NVarChar, 7);
                                cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                                cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100);
                                cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10);
                                cmd.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                                cmd.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                                cmd.Parameters.Add("@v", SqlDbType.NVarChar, 5);
                                cmd.Prepare();

                                int i = 0;
                                foreach (string r in rows)
                                {
                                    i += 1;


                                    string[] items = r.Split(delimeter.ToCharArray());
                                    if (items[0] != "")
                                    {
                                        cmd.Parameters["@t"].Value = items[0];
                                        cmd.Parameters["@k"].Value = items[1];
                                        cmd.Parameters["@n"].Value = items[2].ToString(CultureInfo.CurrentCulture);
                                        cmd.Parameters["@s"].Value = items[3];
                                        cmd.Parameters["@cz"].Value = items[4];
                                        cmd.Parameters["@cs"].Value = items[5];
                                        cmd.Parameters["@v"].Value = items[6];
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                MessageBox.Show("Odebrano nową bazę towarów");
                                
                                cn.Close();
                            }
                            else if (sendorec[0] == 12)
                            {
                                byte[] SendingBuffer = null;
                                FileStream Fs = new FileStream("picatch.imp", FileMode.Open, FileAccess.Read);
                                
                                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(1024)));
                                int TotalLength = (int)Fs.Length, CurrentPacketLength; //counter = 0;
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

                                // label1.Text = "Wysłano " + Fs.Length.ToString() + " bajtów do urządzenia";
                                Fs.Close();
                                netstream.Close();
                                client.Close();
                            }
                            else if (sendorec[0] == 21)
                            {

                                byte[] SendingBuffer = null;
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

                                FileStream Fs = new FileStream("edi.exp", FileMode.Open, FileAccess.Read);

                                int NoOfPackets = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Fs.Length) / Convert.ToDouble(1024)));
                                int TotalLength = (int)Fs.Length, CurrentPacketLength;
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
                            else if (sendorec[0] == 22)
                            {

                                int totalrecbytes = 0;
                                FileStream Fs = new FileStream("edi.imp", FileMode.Create, FileAccess.Write);
                                while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
                                {
                                    Fs.Write(RecData, 0, RecBytes);
                                    totalrecbytes += RecBytes;
                                }

                                Fs.Close();
                                netstream.Close();
                                client.Close();

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

                            netstream.Close();
                            client.Close();
                        }


                    }


                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        //netstream.Close();
                    }
                    
                   }
                   else
                   {
                       Listener.Stop();
                   }
                }
            
           
        }

        private void Loadbaza()
        {
           
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);


            string delimeter = ";";

            StreamReader sr = new StreamReader("inwent.exp", Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage), true);
            string allData = sr.ReadToEnd();
            string[] rows = allData.Split("\r\n".ToCharArray());
            allData = "empty";
            sr.DiscardBufferedData();
            sr.Close();
            cn.Open();
            SqlCeCommand delete = cn.CreateCommand();
            delete.CommandText = "DROP TABLE dane";
            delete.Prepare();
            delete.ExecuteNonQuery();
            SqlCeCommand cmd = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(6,3), datazmian datetime, cenahurt nvarchar(10), cenaoryg nvarchar(10))", cn);
            cmd.ExecuteNonQuery();
            cmd.CommandText = "INSERT INTO dane (typ, kod, nazwa, stan, cenazk, cenasp, vat) VALUES (?, ?, ?, ?, ?, ?, ?)";
            cmd.Parameters.Add("@t", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
            cmd.Parameters.Add("@v", SqlDbType.NVarChar, 10);
            cmd.Prepare();
                      
            int i = 0;
            foreach (string r in rows)
            {
                i += 1;
                

                string[] items = r.Split(delimeter.ToCharArray());
                if (items[0] != "")
                {
                    cmd.Parameters["@t"].Value = items[0];
                    cmd.Parameters["@k"].Value = items[1];
                    cmd.Parameters["@n"].Value = items[2];
                    cmd.Parameters["@s"].Value = items[3];
                    cmd.Parameters["@cz"].Value = items[4];
                    cmd.Parameters["@cs"].Value = items[5];
                    cmd.Parameters["@v"].Value = items[6];
                    cmd.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Odebrano nową bazę towarów");
            cn.Close();

            

        }



        private void button1_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            this.Close();
        }

        private void PICTCPAGENT_FormClosing(object sender, FormClosingEventArgs e)
        {

           
            bw.CancelAsync();
            
            Listener.Stop();
            bw.Dispose();
            
                       
        }

        private void ukryjToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }
        public void senddoc()
        {
            StreamWriter sw = new StreamWriter("picatch.imp", true, Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage));

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
