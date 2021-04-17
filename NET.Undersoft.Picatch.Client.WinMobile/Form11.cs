using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using CalibCs;
using System.Net.Sockets;
using System.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using InTheHand.Net.Ports;


namespace SmartDeviceApplication2
{
    /// <summary>
    /// Summary description for Form5.
    /// </summary>
    public class Form11 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private int rownum;
        //OPCJE
        private string transfer;
        private string com;
        private string ip;
        private string ufile;
        private string dfile;
        private string bdll;
        private bool bflag;
        private bool ipflag;
        private int port;
        private string skaner;
        private byte testflag;
        private int lic;
        public Form11(int rownumber, int licence)
        {
            lic = licence;


            //
            // Required for Windows Form Designer support
            //
            rownum = rownumber;
            string connectionString;
            string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmd2 = cn.CreateCommand();
            cmd2.CommandText = "SELECT * FROM opcje WHERE id = 1";
            cmd2.Prepare();
            SqlCeDataReader dr = cmd2.ExecuteReader();

            while (dr.Read())
            {
                transfer = dr.GetString(1);
                com = dr.GetString(2);
                ip = dr.GetString(3);
                ufile = dr.GetString(4);
                dfile = dr.GetString(5);
                bdll = dr.GetString(6);
                bflag = dr.GetBoolean(7);
                ipflag = dr.GetBoolean(8);
                port = dr.GetInt32(9);
                skaner = dr.GetString(10);
            }
            cn.Close();
            InitializeComponent();


            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public void senddoc()
        {
            string connectionString;
            string filename = (ufile);
            StreamWriter sw = new StreamWriter(filename);
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
            DataTable table = new DataTable();
            da.Fill(table);
            string index = table.Rows[rownum][0].ToString();
            SqlCeCommand cmdh = cn.CreateCommand();
            cmdh.CommandText = "SELECT nazwadok, typ, data  FROM dok WHERE id = ?";
            cmdh.Parameters.Add("@d", SqlDbType.Int);
            cmdh.Prepare();
            cmdh.Parameters["@d"].Value = int.Parse(index);
            SqlCeDataReader drh = cmdh.ExecuteReader();
            while (drh.Read())
            {
                sw.WriteLine(drh.GetString(0) + ";" + drh.GetString(1) + ";" + Convert.ToString(drh.GetSqlDateTime(2)));
            }

            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat  FROM bufor WHERE dokid = ?";
            cmd.Parameters.Add("@d", SqlDbType.Int);
            cmd.Prepare();
            cmd.Parameters["@d"].Value = int.Parse(index);
            SqlCeDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                decimal n = dr.GetDecimal(4);
                int did = dr.GetInt32(0);
                sw.WriteLine(Convert.ToString(did) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + Convert.ToString(n) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetString(7));
            }


            sw.Close();
            cn.Close();
        }

        private void confirm_send()
        {






            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
            DataTable table = new DataTable();
            da.Fill(table);
            string index = table.Rows[rownum][0].ToString();
            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "UPDATE dok SET send = -1 WHERE id = ?";
            cmd.Parameters.Add("@k", SqlDbType.Int);

            cmd.Parameters["@k"].Value = int.Parse(index);
            cmd.Prepare();
            cmd.ExecuteNonQuery();
            cn.Close();



        }


        private void sendbt()
        {

            if (bflag == true && bdll == "BTLibCs")
            {

                CalibCs.BTLibCs.BTLib_Initialize();
                IntPtr btest = CalibCs.SerialFuncCs.CreateFile(com + ":", CalibCs.SerialFuncCs.GENERIC_WRITE | CalibCs.SerialFuncCs.GENERIC_READ, 0, IntPtr.Zero, CalibCs.SerialFuncCs.OPEN_EXISTING, CalibCs.SerialFuncCs.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
                if (btest != IntPtr.Zero)
                {
                    CalibCs.SerialFuncCs.DCB portconfig = new CalibCs.SerialFuncCs.DCB();
                    bool test2 = CalibCs.SerialFuncCs.GetCommState(btest, ref portconfig);
                    portconfig.BaudRate = SerialFuncCs.CBR_19200;
                    portconfig.Flag32 |= SerialFuncCs.fBinary;
                    portconfig.Parity = SerialFuncCs.NOPARITY;
                    portconfig.ByteSize = 8;
                    portconfig.StopBits = SerialFuncCs.ONESTOPBIT;
                    test2 = CalibCs.SerialFuncCs.SetCommState(btest, ref portconfig);



                    int licznik = 0;
                    uint modemstat = 0;
                    while (modemstat != 176 && licznik != 30)
                    {

                        licznik += 1;

                        label1.Text = "CZEKAM NA PO£¥CZENIE 30 sekund: " + licznik.ToString();
                        label1.Refresh();
                        label1.Focus();
                        System.Threading.Thread.Sleep(1000);

                        SerialFuncCs.GetCommModemStatus(btest, ref modemstat);



                    }
                    if (licznik >= 30)
                    {
                        SerialFuncCs.CloseHandle(btest);
                        CalibCs.BTLibCs.BTLib_DeInitialize();


                        label1.Text = "KONIEC CZASU";


                    }
                    else
                    {

                        System.IO.FileStream file = new FileStream(ufile, FileMode.Open, FileAccess.Read);
                        byte[] buffer = new byte[file.Length];
                        file.Read(buffer, 0, buffer.Length);
                        long lenght = file.Length;
                        byte[] flaga = new byte[1];
                        flaga[0] = 1;
                        byte[] bytec = BitConverter.GetBytes(lenght);
                        uint bytestw = Convert.ToUInt32(buffer.Length);
                        uint byteswr = new uint();
                        label1.Text = "WYSY£AM";
                        label1.Focus();
                        label1.Refresh();
                        System.Threading.Thread.Sleep(3000);

                        test2 = SerialFuncCs.WriteFile(btest, flaga, Convert.ToUInt32(flaga.Length), ref byteswr, IntPtr.Zero);

                        System.Threading.Thread.Sleep(3000);

                        test2 = SerialFuncCs.WriteFile(btest, bytec, Convert.ToUInt32(bytec.Length), ref byteswr, IntPtr.Zero);
                        System.Threading.Thread.Sleep(3000);
                        //pEvtMask = 0; 
                        //SerialFuncCs.WaitCommEvent(btest, ref pEvtMask, IntPtr.Zero);	
                        //label3.Text = pEvtMask.ToString();
                        //MessageBox.Show("Wyœlij2");
                        //	pEvtMask = 0;
                        //	SerialFuncCs.WaitCommEvent(btest, ref pEvtMask, IntPtr.Zero);
                        //label1.Text = pEvtMask.ToString();;
                        //while(pEvtMask != SerialFuncCs.EV_TXEMPTY)
                        //{
                        //}
                        //status_l.Text = "WYSY£ANIE";
                        test2 = SerialFuncCs.WriteFile(btest, buffer, bytestw, ref byteswr, IntPtr.Zero);
                        System.Threading.Thread.Sleep(2000);
                        flaga[0] = 3;
                        test2 = SerialFuncCs.WriteFile(btest, flaga, Convert.ToUInt32(flaga.Length), ref byteswr, IntPtr.Zero);
                        SerialFuncCs.CloseHandle(btest);
                        CalibCs.BTLibCs.BTLib_DeInitialize();
                        //pEvtMask = 0;
                        //label2.Text = pEvtMask.ToString();
                        label1.Text = "DANE WYS£ANE";
                        label1.Focus();
                        label1.Refresh();

                        file.Close();
                        testflag = 1;
                        confirm_send();

                    }
                }
                else
                {
                    MessageBox.Show("Niepoprawny port COM");
                }
            }
            else if (bflag == true && bdll == "MSStack")
            {
                sendbtms();
            }
        }

        private void sendbtms()
        {

            BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;
            System.IO.FileStream file = new FileStream(ufile, FileMode.Open, FileAccess.Read);
            byte[] buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);
            long lenght = file.Length;
            byte[] flaga = new byte[1];
            byte[] bytec = BitConverter.GetBytes(lenght);
            label1.Text = "WYSY£AM";
            label1.Focus();
            label1.Refresh();
            flaga[0] = 1;
            BluetoothListener listener = new BluetoothListener(BluetoothService.SerialPort);
            listener.Start();
            int licznik = 0;
            BluetoothClient client = listener.AcceptBluetoothClient();
            while (client.Connected == false && licznik != 30)
            {
                licznik += 1;
                label1.Text = "CZEKAM NA PO£¥CZENIE 30 sekund: " + licznik.ToString();
                label1.Focus();
                System.Threading.Thread.Sleep(1000);
                label1.Refresh();

            }
            if (licznik >= 30)
            {
                listener.Stop();
                label1.Text = "KONIEC CZASU";
                label1.Refresh();
            }
            else
            {
                //    Thread.Sleep(1000);
                //timeout += 1;


                Stream peerStream = client.GetStream();

                peerStream.Write(flaga, 0, 1);
                System.Threading.Thread.Sleep(3000);
                peerStream.Write(bytec, 0, bytec.Length);


                this.label1.Text = "Wysy³am: " + (lenght / 1024).ToString() + " Kb";
                label1.Refresh();
                peerStream.Write(buffer, 0, buffer.Length);
                System.Threading.Thread.Sleep(3000);
                flaga[0] = 3;
                peerStream.Write(flaga, 0, 1);
                listener.Stop();


                this.label1.Text = "DANE WYS£ANE: " + (lenght / 1024).ToString() + "Kb " + "PROSZÊ CZEKAÆ";
                label1.Refresh();
                confirm_send();

            }



            //BluetoothRadio.PrimaryRadio.Mode = RadioMode.PowerOff;
        }


        private void sendtcp()
        {
            byte[] SendingBuffer = null;
            TcpClient client = null;
            NetworkStream netstream = null;
            byte[] sendorec = new byte[1];
            sendorec[0] = 1;
            try
            {
                client = new TcpClient(ip, port);
                label1.Text = "Connected to the Server...";
                netstream = client.GetStream();
                netstream.Write(sendorec, 0, 1);

                FileStream Fs = new FileStream(ufile, FileMode.Open, FileAccess.Read);
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

                label1.Text = "Wys³ano " + Fs.Length.ToString() + " bajtów do agenta";
                testflag = 1;
                Fs.Close();
                netstream.Close();
                client.Close();
                confirm_send();

            }
            catch (SocketException)
            {
                MessageBox.Show("B³¹d sieci. Nie mo¿na po³¹czyæ siê z agentem");
                testflag = 0;
            }


        }


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.Gold;
            this.label1.Location = new System.Drawing.Point(8, 16);
            this.label1.Size = new System.Drawing.Size(184, 32);
            this.label1.Text = "CZY CHCESZ WYS£AÆ DOKUEMENT?";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(24, 64);
            this.button1.Text = "TAK";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(112, 64);
            this.button2.Text = "NIE";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form11
            // 
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(202, 102);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Location = new System.Drawing.Point(16, 100);
            this.Text = "Wczytywanie towarów";

        }
        #endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            senddoc();

            if (ipflag == true && ip != "" && port != 0)
            {
                sendtcp();
            }
            if (bflag == true && testflag != 1)
            {
                if (ipflag == true)
                {
                    DialogResult result = MessageBox.Show("Czy wys³aæ przez Bletooth", "Bluetooth", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        sendbt();
                    }
                    else if (result == DialogResult.No)
                    {
                        label1.Text = "KONIEC CZASU";
                    }
                }
                else
                {
                    sendbt();
                }
            }
            if (bflag == false && ipflag == false)
            {
                MessageBox.Show("Ustaw typ wysy³ki w opcjach");
            }
            if (label1.Text == "KONIEC CZASU" || testflag == 1)
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

    }
}
