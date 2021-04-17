using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data.SqlServerCe;
using System.Reflection;
using System.Text;
using Microsoft.WindowsCE.Forms;
using Microsoft.WindowsMobile.Forms;


namespace SmartDeviceApplication2
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>


    public class Form1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button send;
        private System.Windows.Forms.Button exit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button add_b;
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
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button help_b;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
        private System.Windows.Forms.Label licence_l;
        private string skaner;
        private string serial;
        private int licflag = 0;
        [StructLayout(LayoutKind.Sequential)]
        public struct SIPINFO
        {
            /// <summary>
            /// Size, in bytes, of the SIPINFO structure. This member must be
            /// filled in by the application with the size of operator. Because
            /// the system can check the size of the structure to determine
            /// the operating system version number, this member allows for
            /// future enhancements to the SIPINFO structure while maintaining
            /// backward compatibility.
            /// </summary>
            public uint cbSize;

            /// <summary>
            /// Specifies flags representing state information of the
            /// software-based input panel. The following table shows the
            /// possible bit flags. These flags can be used in combination.
            /// </summary>
            public uint fdwFlags;

            /// <summary>
            /// Rectangle, in screen coordinates, that represents the area of
            /// the desktop not obscured by the software-based input panel.
            /// If the software-based input panel is floating, this rectangle
            /// is equivalent to the working area. Full-screen applications
            /// that respond to software-based input panel size changes can
            /// set their window rectangle to this rectangle. If the
            /// software-based input panel is docked but does not occupy
            /// an entire edge, then this rectangle represents the largest
            /// rectangle not obscured by the software-based input panel.
            /// If an application wants to use the screen space around the
            /// software-based input panel, it needs to reference rcSipRect.
            /// </summary>
            public RECT rcVisibleDesktop;

            /// <summary>
            /// Rectangle, in screen coordinates of the window rectangle and
            /// not the client area, the represents the size and location of
            /// the software-based input panel. An application does not
            /// generally use this information unless it needs to wrap
            /// around a floating or a docked software-based input panel
            /// that does not occupy an entire edge.
            /// </summary>
            public RECT rcSipRect;

            /// <summary>
            /// Specifies the size of the data pointed to by the pvImData member.
            /// </summary>
            public uint dwImDataSize;

            /// <summary>
            /// Void pointer to IM-defined data. The IM calls the
            /// IInputMethod::GetImData and IInputMethod::SetImData methods to
            /// send and receive information from this structure.
            /// </summary>
            public IntPtr pvImData;
        }

        /// <summary>
        /// This structure defines the coordinates of the upper-left and
        /// lower-right corners of a rectangle.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            /// <summary>
            /// Specifies the x-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int left;

            /// <summary>
            /// Specifies the y-coordinate of the upper-left corner of the rectangle.
            /// </summary>
            public int top;

            /// <summary>
            /// Specifies the x-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int right;

            /// <summary>
            /// Specifies the y-coordinate of the lower-right corner of the rectangle.
            /// </summary>
            public int bottom;
        }



        public Form1()
        {
            //
            // Required for Windows Form Designer support
            //
            int[] checklic = new int[10];
            int[] checkserial = new int[10];
            int[] checklicbuf = new int[10];
            int[] liccount = new int[10];

            ProcessInfo pi;
            serial = "0000000000";

            Openbaza();
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
                serial = dr.GetString(11);

            }
            cn.Close();

            CreateProcess(skaner, null, IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, IntPtr.Zero, out pi);

            InitializeComponent();

            FrmMain_Close();

            if (inputPanel1.Enabled == true)
            {
                inputPanel1.Enabled = false;
            }
            string licence = "";
            licence = GetDeviceID().Substring(1, 10);

            for (int i = 0; i < licence.Length; i++)
            {

                checklic[i] = (int)licence[i];

            }

            for (int i = 0; i < serial.Length; i++)
            {

                checkserial[i] = (int)serial[i];

            }

            for (int i = 0; i < checklic.Length; i++)
            {
                if (i == 0)
                {
                    checklicbuf[i] = ((checklic[checklic.Length - 1] + 27) + 36) - 67;
                }
                if (i == 1)
                {
                    checklicbuf[i] = ((checklic[checklic.Length - 3] + 27) - 2) - 21;
                }
                else if (i == 2)
                {
                    checklicbuf[i] = ((checklic[checklic.Length - 5] + 12) + 2) - 3;
                }
                else if (i == 3)
                {
                    checklicbuf[i] = (checklic[checklic.Length - 10] + 83) - 76;
                }
                else if (i == 4)
                {
                    checklicbuf[i] = (checklic[checklic.Length - 4] + 83) - 28 - 52;
                }
                else if (i == 5)
                {
                    checklicbuf[i] = (checklic[checklic.Length - 8] + 83) - 2 - 71;
                }
                else if (i == 6)
                {
                    checklicbuf[i] = (checklic[checklic.Length - 9] + 83) - 77;
                }
                else if (i == 7)
                {
                    checklicbuf[i] = checklic[checklic.Length - 6] + 27 - 18;
                }
                else if (i == 8)
                {
                    checklicbuf[i] = checklic[checklic.Length - 6] + 3 - 2;
                }
                else if (i == 9)
                {
                    checklicbuf[i] = checklic[checklic.Length - 7] + 83 - 57 - 12 - 2 - 3;
                }



            }

            string test = "";

            for (int i = 0; i < checklicbuf.Length; i++)
            {

                char character = (char)checklicbuf[i];
                test += character;
                if (character == serial[i])
                {
                    liccount[i] = 1;
                }

            }


            string[] strings = new string[liccount.Length];
            for (int i = 0; i < liccount.Length; i++)
            {
                strings[i] = liccount[i].ToString();
            }
            string licresult = string.Join(string.Empty, strings);
            //MessageBox.Show(licresult);
            if (licresult.ToString() == "1111111111")
            {
                licence_l.Text = "WERSJA PE£NA DLA ID: " + licence;
                licence_l.Refresh();
                licflag = 1;
            }
            else
            {

                licence_l.Text = "WERJA DEMO DLA ID: " + licence;
                licence_l.Refresh();
            }


        }


        private static Int32 METHOD_BUFFERED = 0;
        private static Int32 FILE_ANY_ACCESS = 0;
        private static Int32 FILE_DEVICE_HAL = 0x00000101;

        private const Int32 ERROR_NOT_SUPPORTED = 0x32;
        private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;

        private static Int32 IOCTL_HAL_GET_DEVICEID =
            ((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14)
            | ((21) << 2) | (METHOD_BUFFERED);

        [DllImport("coredll.dll", SetLastError = true)]
        private static extern bool KernelIoControl(Int32 dwIoControlCode,
            IntPtr lpInBuf, Int32 nInBufSize, byte[] lpOutBuf,
            Int32 nOutBufSize, ref Int32 lpBytesReturned);

        private static string GetDeviceID()
        {
            // Initialize the output buffer to the size of a 
            // Win32 DEVICE_ID structure.
            byte[] outbuff = new byte[20];
            Int32 dwOutBytes;
            bool done = false;

            Int32 nBuffSize = outbuff.Length;

            // Set DEVICEID.dwSize to size of buffer.  Some platforms look at
            // this field rather than the nOutBufSize param of KernelIoControl
            // when determining if the buffer is large enough.
            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
            dwOutBytes = 0;

            // Loop until the device ID is retrieved or an error occurs.
            while (!done)
            {
                if (KernelIoControl(IOCTL_HAL_GET_DEVICEID, IntPtr.Zero,
                    0, outbuff, nBuffSize, ref dwOutBytes))
                {
                    done = true;
                }
                else
                {
                    int error = Marshal.GetLastWin32Error();
                    switch (error)
                    {
                        case ERROR_NOT_SUPPORTED:
                            throw new NotSupportedException(
                                "IOCTL_HAL_GET_DEVICEID is not supported on this device",
                                new Win32Exception(error));

                        case ERROR_INSUFFICIENT_BUFFER:

                            // The buffer is not big enough for the data.  The
                            // required size is in the first 4 bytes of the output
                            // buffer (DEVICE_ID.dwSize).
                            nBuffSize = BitConverter.ToInt32(outbuff, 0);
                            outbuff = new byte[nBuffSize];

                            // Set DEVICEID.dwSize to size of buffer.  Some
                            // platforms look at this field rather than the
                            // nOutBufSize param of KernelIoControl when
                            // determining if the buffer is large enough.
                            BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);
                            break;

                        default:
                            throw new Win32Exception(error, "Unexpected error");
                    }
                }
            }

            // Copy the elements of the DEVICE_ID structure.
            Int32 dwPresetIDOffset = BitConverter.ToInt32(outbuff, 0x4);
            Int32 dwPresetIDSize = BitConverter.ToInt32(outbuff, 0x8);
            Int32 dwPlatformIDOffset = BitConverter.ToInt32(outbuff, 0xc);
            Int32 dwPlatformIDSize = BitConverter.ToInt32(outbuff, 0x10);
            StringBuilder sb = new StringBuilder();

            for (int i = dwPresetIDOffset;
                i < dwPresetIDOffset + dwPresetIDSize; i++)
            {
                sb.Append(String.Format("{0:X2}", outbuff[i]));
            }

            sb.Append("-");

            for (int i = dwPlatformIDOffset;
                i < dwPlatformIDOffset + dwPlatformIDSize; i++)
            {
                sb.Append(String.Format("{0:X2}", outbuff[i]));
            }
            return sb.ToString();
        }

        public void Openbaza()
        {
            string connectionString;
            string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);

            if (File.Exists(fileName) == false)
            {

                SqlCeEngine engine = new SqlCeEngine(connectionString);
                engine.CreateDatabase();
                cn.Open();
                SqlCeCommand cmd = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(100), stan nvarchar(10), cenazk nvarchar(20), cenasp nvarchar(20), vat nvarchar(5), devstat nvarchar(20), bad_cena bit, bad_stan bit, cenapolka numeric(10,3), zliczono numeric(10,3))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE bufor (id int identity not null primary key, dokid int, kod nvarchar (15), nazwa nvarchar (100), cenazk nvarchar(10), ilosc numeric(10,3), stan nvarchar(10), cenasp nvarchar(10), vat nvarchar(10))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE dok (id int identity not null primary key, nazwadok nvarchar (120), typ nvarchar(10), data datetime, send bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE opcje (id int identity not null primary key, transfer nvarchar (20), com nvarchar(20), ip nvarchar(20), ufile nvarchar (120), dfile nvarchar(120), bdll nvarchar(50), bflag bit, ipflag bit, port int, skaner nvarchar(120), licence nvarchar(40), devstat nvarchar(20))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("INSERT INTO opcje (transfer, com, ip, ufile, dfile, bdll, bflag, ipflag, port, skaner, licence) VALUES ('bluetooth', 'COM5', '0.0.0.0', '\\Inwent.imp', '\\Inwent.exp', 'MSStack', 0, 0, 8790, '\\folder\\program_czytnika.exe', '0000000000')", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE edihead (id int identity not null primary key, FileName nvarchar(40), TypPolskichLiter nvarchar(20), TypDok nvarchar (20), NrDok nvarchar(30), Data nvarchar(30), DataRealizacji nvarchar (30), Magazyn nvarchar(30), SposobPlatn nvarchar(10), TerminPlatn  nvarchar(10), IndeksCentralny nvarchar(10), NazwaWystawcy  nvarchar(120), AdresWystawcy  nvarchar(120), KodWystawcy nvarchar(120), MiastoWystawcy nvarchar(120), UlicaWystawcy nvarchar(120), NIPWystawcy nvarchar(120), BankWystawcy nvarchar(120), KontoWystawcy nvarchar(120), TelefonWystawcy nvarchar(30), NrWystawcyWSieciSklepow nvarchar(20), NazwaOdbiorcy nvarchar(120), AdresOdbiorcy nvarchar(120), KodOdbiorcy nvarchar(20), MiastoOdbiorcy nvarchar(120), UlicaOdbiorcy nvarchar(120), NIPOdbiorcy nvarchar(120), BankOdbiorcy nvarchar(120), KontoOdbiorcy nvarchar(120), TelefonOdbiorcy nvarchar(120), NrOdbiorcyWSieciSklepow nvarchar(20), DoZaplaty nvarchar(20), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE edibody (id int identity not null primary key, NrDok nvarchar(30), Nazwa nvarchar (120), kod nvarchar(20), Vat nvarchar(7), Jm nvarchar (7), Asortyment nvarchar(120), Sww nvarchar(30), PKWiU nvarchar(30), Ilosc nvarchar(10), Cena nvarchar(10), Wartosc nvarchar(10), IleWOpak nvarchar(10), CenaSp nvarchar(10), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE ediend (id int identity not null primary key, NrDok nvarchar(30), Vat nvarchar (20), SumaNet nvarchar(20), SumaVat nvarchar(20), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fedihead (id int identity not null primary key, ehid int, FileName nvarchar(40), TypPolskichLiter nvarchar(20), TypDok nvarchar (20), NrDok nvarchar(30), Data nvarchar(30), DataRealizacji nvarchar (30), Magazyn nvarchar(30), SposobPlatn nvarchar(10), TerminPlatn  nvarchar(10), IndeksCentralny nvarchar(10), NazwaWystawcy  nvarchar(120), AdresWystawcy  nvarchar(120), KodWystawcy nvarchar(120), MiastoWystawcy nvarchar(120), UlicaWystawcy nvarchar(120), NIPWystawcy nvarchar(120), BankWystawcy nvarchar(120), KontoWystawcy nvarchar(120), TelefonWystawcy nvarchar(30), NrWystawcyWSieciSklepow nvarchar(20), NazwaOdbiorcy nvarchar(120), AdresOdbiorcy nvarchar(120), KodOdbiorcy nvarchar(20), MiastoOdbiorcy nvarchar(120), UlicaOdbiorcy nvarchar(120), NIPOdbiorcy nvarchar(120), BankOdbiorcy nvarchar(120), KontoOdbiorcy nvarchar(120), TelefonOdbiorcy nvarchar(120), NrOdbiorcyWSieciSklepow nvarchar(20), DoZaplaty nvarchar(20), status nvarchar (20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fedibody (id int identity not null primary key, ebid int, NrDok nvarchar(30), Nazwa nvarchar (120), kod nvarchar(20), Vat nvarchar(7), Jm nvarchar (7), Asortyment nvarchar(120), Sww nvarchar(30), PKWiU nvarchar(30), Wymagane nvarchar(10), Ilosc nvarchar(10), Cena nvarchar(10), Wartosc nvarchar(10), IleWOpak nvarchar(10), CenaSp nvarchar(10), status nvarchar, complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fediend (id int identity not null primary key, eeid int, NrDok nvarchar(30), Vat nvarchar (20), SumaNet nvarchar(20), SumaVat nvarchar(20), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cn.Close();



            }
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
        }

        [DllImport("coredll.dll", SetLastError = true)]
        //[return: System.Runtime.InteropServices.Marshal(UnmanagedType.Bool)]
        public static extern bool SipGetInfo(ref SIPINFO sipInfo);


        [DllImport("coredll.dll", SetLastError = true)]
        //[return: Marshal(UnmanagedType.Bool)]
        public static extern bool SipSetInfo(ref SIPINFO sipInfo);

        private void ShowInputPanel()
        {
            SIPINFO sipInfo;
            int x = 0;
            int y = Screen.PrimaryScreen.Bounds.Height - this.inputPanel1.Bounds.Height;

            this.inputPanel1.Enabled = true;

            sipInfo = new SIPINFO();
            sipInfo.cbSize = (uint)Marshal.SizeOf(sipInfo);
            if (SipGetInfo(ref sipInfo))
            {
                sipInfo.rcSipRect.left = x;
                sipInfo.rcSipRect.top = y;

                SipSetInfo(ref sipInfo);
            }
        }


        /*public void senddoc()
        {
            string connectionString;
            string filename = ("\\FlashDisk\\Inwent.imp");
            StreamWriter sw = new StreamWriter(filename);
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT kod, (ilosc * 1000) AS qty  FROM bufor";
            cmd.Prepare();
            SqlCeDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                int n = (int) dr.GetSqlDecimal(1);
                sw.WriteLine("INW;"+ dr.GetString(0) + ";" + Convert.ToString(n));
            }
			
			
            sw.Close();
            cn.Close();
        }*/
        //
        // TODO: Add any constructor code after InitializeComponent call
        //


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
            System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form1));
            this.add_b = new System.Windows.Forms.Button();
            this.send = new System.Windows.Forms.Button();
            this.exit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.help_b = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.licence_l = new System.Windows.Forms.Label();
            // 
            // add_b
            // 
            this.add_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.add_b.Location = new System.Drawing.Point(16, 56);
            this.add_b.Size = new System.Drawing.Size(208, 40);
            this.add_b.Text = "KOLEKTOR";
            this.add_b.Click += new System.EventHandler(this.add_b_Click);
            // 
            // send
            // 
            this.send.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.send.Location = new System.Drawing.Point(16, 200);
            this.send.Size = new System.Drawing.Size(208, 40);
            this.send.Text = "TRANSFER DANYCH";
            this.send.Click += new System.EventHandler(this.send_Click);
            // 
            // exit
            // 
            this.exit.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.exit.Location = new System.Drawing.Point(128, 248);
            this.exit.Size = new System.Drawing.Size(96, 32);
            this.exit.Text = "KONIEC";
            this.exit.Click += new System.EventHandler(this.exit_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(16, 152);
            this.button1.Size = new System.Drawing.Size(208, 40);
            this.button1.Text = "KARTOTEKI";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(16, 248);
            this.button2.Size = new System.Drawing.Size(104, 32);
            this.button2.Text = "OPCJE";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(0, 280);
            this.label1.Size = new System.Drawing.Size(240, 16);
            this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(16, 104);
            this.button3.Size = new System.Drawing.Size(208, 40);
            this.button3.Text = "KOMPLETACJA";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.button4.Location = new System.Drawing.Point(56, 16);
            this.button4.Size = new System.Drawing.Size(128, 32);
            this.button4.Text = "KLAWIATURA I/O";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // help_b
            // 
            this.help_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.help_b.Location = new System.Drawing.Point(192, 16);
            this.help_b.Size = new System.Drawing.Size(32, 32);
            this.help_b.Text = "?";
            this.help_b.Click += new System.EventHandler(this.help_b_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(16, 16);
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // 
            // licence_l
            // 
            this.licence_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.licence_l.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.licence_l.Location = new System.Drawing.Point(0, 296);
            this.licence_l.Size = new System.Drawing.Size(240, 16);
            this.licence_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            // 
            // Form1
            // 
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(240, 320);
            this.ControlBox = false;
            this.Controls.Add(this.licence_l);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.help_b);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.exit);
            this.Controls.Add(this.send);
            this.Controls.Add(this.add_b);
            this.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Text = "PICATCH AXC4.1";
            this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
            this.Closed += new System.EventHandler(this.Form1_Closed);

        }
        #endregion
        /*private void Loadbaza ()
		{
			
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
				   
			
			string delimeter = ";";
			string filename = ("\\FlashDisk\\Inwent.exp");
			StreamReader sr = new StreamReader(filename);
			string allData = sr.ReadToEnd();
			string[] rows = allData.Split("\n".ToCharArray());
			cn.Open();
			SqlCeCommand cmd = cn.CreateCommand();
			cmd.CommandText = "INSERT INTO dane (typ, kod, nazwa, stan, cenazk, cenasp, vat) VALUES (?, ?, ?, ?, ?, ?, ?)";
			cmd.Parameters.Add("@t", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
			cmd.Parameters.Add("@n", SqlDbType.NVarChar, 120);
			cmd.Parameters.Add("@s", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@cz", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@cs", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@v", SqlDbType.NVarChar, 20);
			cmd.Prepare();
			foreach (string r in rows)
			{
				
				string[] items = r.Split(delimeter.ToCharArray());
				cmd.Parameters["@t"].Value = items[0];
				cmd.Parameters["@k"].Value = items[1];
				cmd.Parameters["@n"].Value = items[2];
				cmd.Parameters["@s"].Value = items[3];
				cmd.Parameters["@cz"].Value = items[4];
				cmd.Parameters["@cs"].Value = items[5];
				cmd.Parameters["@v"].Value = items[6];
				cmd.ExecuteNonQuery();
				
			}
			cn.Close();	
			sr.DiscardBufferedData();
			sr.Close();
			
		}*/

        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        public static extern int FindWindow(string lpClassName, string lpWindowName);
        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        public static extern bool ShowWindow(int hwnd, int nCmdShow);
        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        public static extern bool EnableWindow(int hwnd, bool enabled);
        [DllImport("coredll.dll", CharSet = CharSet.Auto)]
        private static extern bool SetWindowPos(int hwnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        //[DllImport("coredll.dll", CharSet=CharSet.Auto)]
        //private static extern bool GetWindowPos(int hwnd, int hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private void FrmMain_Close()
        {

            //const UInt32 SWP_NOSIZE     = 0x0001;
            //const UInt32 SWP_NOMOVE     = 0x0002;
            //const UInt32 SWP_NOACTIVATE = 0x0010;
            //const UInt32 SWP_HIDEWINDOW = 0x0080;
            //const int HWND_NOTTOPMOST   = -2;
            int h = FindWindow("HHTaskBar", "");
            ShowWindow(h, 0);
            //SetWindowPos(h, HWND_NOTTOPMOST, 0, 0, 0, 0, SWP_HIDEWINDOW | SWP_NOACTIVATE | SWP_NOSIZE);

            EnableWindow(h, false);



            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Update();
        }

        private void FrmMain_Open()
        {

            //const UInt32 SWP_NOSIZE     = 0x0001;
            //const UInt32 SWP_NOMOVE     = 0x0002;
            //const UInt32 SWP_NOACTIVATE = 0x0010;
            //const UInt32 SWP_HIDEWINDOW = 0x0080;
            //const int HWND_NOTTOPMOST   = -2;
            int h = FindWindow("HHTaskBar", "");
            ShowWindow(h, 5);
            //SetWindowPos(h, HWND_NOTTOPMOST, 0, 0, 0, 0, SWP_HIDEWINDOW | SWP_NOACTIVATE | SWP_NOSIZE);

            EnableWindow(h, true);

            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            Update();
        }

        [DllImport("coredll.dll", CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        static extern bool CreateProcess(string lpApplicationName, string lpCommandLine, IntPtr lpProcessAttributes, IntPtr lpThreadAttributes, bool bInheritHandles, uint dwCreationFlags, IntPtr lpEnvironment, string lpCurrentDirectory, IntPtr lpStartupInfo, out ProcessInfo lpProcessInformation);
        [StructLayout(LayoutKind.Sequential)]
        public struct ProcessInfo
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public Int32 ProcessId;
            public Int32 ThreadId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SecurityAttributes
        {
            public int length;
            public IntPtr lpSecurityDescriptor;
            public bool bInheritHandle;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct StartupInfo
        {
            public uint cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;

        }



        static void Main()
        {





            Application.Run(new Form1());


        }

        private void add_b_Click(object sender, System.EventArgs e)
        {
            Form8 newForm = new Form8(licflag);
            newForm.Show();
        }

        private void exit_Click(object sender, System.EventArgs e)
        {


            this.Close();

        }



        private void send_Click(object sender, System.EventArgs e)
        {

            Form7 frm7 = new Form7(licflag);
            frm7.Show();

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Form71 frm71 = new Form71();
            frm71.Show();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            Form14 frm14 = new Form14();
            frm14.Show();
        }

        private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            FrmMain_Open();
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Form15 frm15 = new Form15();
            frm15.Show();
        }

        private void button4_Click(object sender, System.EventArgs e)
        {
            if (inputPanel1.Enabled == true)
            {
                inputPanel1.Enabled = false;
            }
            else
            {
                ShowInputPanel();
            }
        }

        private void help_b_Click(object sender, System.EventArgs e)
        {
            Info info = new Info();
            info.Show();
        }

        private void Form1_Closed(object sender, System.EventArgs e)
        {
            FrmMain_Open();
        }




    }
}

