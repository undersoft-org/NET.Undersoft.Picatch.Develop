using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using CalibCs;
using System.IO;
using System.Runtime.InteropServices;
using System.Data;
using System.Text;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace Undersoft.Picatch
{
	
	/// <summary>
	/// Summary description for Form6.
	/// </summary>
	public class Form7 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button search_b;
		private System.Windows.Forms.Label label1;
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
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private int lic;
		
		public Form7(int licence)
		{
			//
			// Required for Windows Form Designer support
			//
			lic = licence;
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
			try
			{
				if (bflag == true && bdll == "BTLibCs")
				{
			
					CalibCs.BTLibCs.BTLib_Initialize();
					BTST_LOCALINFO LocalInfo = new BTST_LOCALINFO();
					LocalInfo = new BTST_LOCALINFO();
					string swork = new string(' ', 82);
					LocalInfo.LocalName = swork.ToCharArray();
					LocalInfo.LocalAddress = "                  ".ToCharArray();
					LocalInfo.Encryption = false;
					LocalInfo.SecurityMode3 = false;
					LocalInfo.LocalMode = 0;
					LocalInfo.LocalClass = 0;
					CalibCs.BTLibCs.BTLib_GetLocalInfo(ref LocalInfo);
					LocalInfo.SecurityMode3 = false;
					LocalInfo.LocalMode = BTLibCs.BTMODE_GENERAL_ACCESSIBLE;
					LocalInfo.Encryption = false;
					//LocalInfo.LocalClass = BTLibCs.BTCOD_CAPTURING | BTLibCs.BTCOD_MINOR_PHONE_CORDLESS | BTLibCs.BTCOD_MINOR_PHONE_CELLULAR | BTLibCs.BTCOD_OBJECT_TRANSFER;
					//string s = new string(LocalInfo.LocalName);
					//this.lstDevices.Items.Add(s);
					CalibCs.BTLibCs.BTLib_SetLocalInfo(LocalInfo);
					CalibCs.BTLibCs.BTLib_RegistLocalInfo();
					CalibCs.BTLibCs.BTLib_DeInitialize();
				}
				InitializeComponent();
				this.Height = Screen.PrimaryScreen.Bounds.Height;
				this.Width = Screen.PrimaryScreen.Bounds.Width;
				Update();
				//
				// TODO: Add any constructor code after InitializeComponent call
				//
			}
			catch (Exception e)
			{
				MessageBox.Show(e.Message);
			}
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.exit_b = new System.Windows.Forms.Button();
			this.search_b = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.button3 = new System.Windows.Forms.Button();
			this.button4 = new System.Windows.Forms.Button();
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(16, 112);
			this.button1.Size = new System.Drawing.Size(208, 32);
			this.button1.Text = "ODBIERZ BAZÊ TOWARÓW";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.button2.Location = new System.Drawing.Point(16, 32);
			this.button2.Size = new System.Drawing.Size(208, 32);
			this.button2.Text = "WYŒLIJ DOKUMENT";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(16, 232);
			this.exit_b.Size = new System.Drawing.Size(208, 32);
			this.exit_b.Text = "WYJŒCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// search_b
			// 
			this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.search_b.Location = new System.Drawing.Point(16, 192);
			this.search_b.Size = new System.Drawing.Size(208, 32);
			this.search_b.Text = "URZ¥DZENIA BLUETOOTH";
			this.search_b.Click += new System.EventHandler(this.search_b_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(8, 272);
			this.label1.Size = new System.Drawing.Size(224, 16);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.button3.Location = new System.Drawing.Point(16, 152);
			this.button3.Size = new System.Drawing.Size(208, 32);
			this.button3.Text = "WYŒLIJ DANE SPRAWDZARKI";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button4
			// 
			this.button4.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.button4.Location = new System.Drawing.Point(13, 72);
			this.button4.Size = new System.Drawing.Size(208, 32);
			this.button4.Text = "AKTUALIZUJ TYLKO STANY";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form7
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(234, 294);
			this.ControlBox = false;
			this.Controls.Add(this.button4);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.search_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Text = "Transfer Danych";

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			/*CalibCs.BTLibCs.BTLib_Initialize();
			IntPtr btest = CalibCs.SerialFuncCs.CreateFile("COM5:", CalibCs.SerialFuncCs.GENERIC_WRITE | CalibCs.SerialFuncCs.GENERIC_READ, 0, IntPtr.Zero, CalibCs.SerialFuncCs.OPEN_EXISTING, CalibCs.SerialFuncCs.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
			if (btest != IntPtr.Zero)
			{
			//	MessageBox.Show("Com ok");
				CalibCs.SerialFuncCs.DCB portconfig = new CalibCs.SerialFuncCs.DCB();
				bool test2 = CalibCs.SerialFuncCs.GetCommState(btest, ref portconfig);
				if (test2 == true)
				{
			//		MessageBox.Show("Get com st ok");
					portconfig.BaudRate = SerialFuncCs.CBR_19200;
					portconfig.Flag32 |= SerialFuncCs.fBinary;
					portconfig.Parity = SerialFuncCs.NOPARITY;
					portconfig.ByteSize = 8;
					portconfig.StopBits = SerialFuncCs.ONESTOPBIT;
					test2 = CalibCs.SerialFuncCs.SetCommState(btest, ref portconfig);
					if (test2 == true)
					{
						this.status_l.Text = "CZEKAM NA PO£¥CZENIE";
						byte[] bytecount = new byte[1024];
						uint bytestw = new uint();
						uint byteswr = new uint();
						uint pEvtMask = SerialFuncCs.EV_RXCHAR;
						uint pEvtMask2 = SerialFuncCs.EV_RXCHAR;
						uint CTS = SerialFuncCs.EV_CTS;
						SerialFuncCs.WaitCommEvent(btest, ref pEvtMask, IntPtr.Zero);
						File.Delete("\\FlashDisk\\Inwent.exp");
						System.IO.FileStream file = new FileStream("\\FlashDisk\\Inwent.exp", FileMode.Append, FileAccess.Write);
						this.status_l.Text = "ODBIERAM DANE";
						test2 = SerialFuncCs.ReadFile(btest, bytecount, Convert.ToUInt32(bytecount.Length), ref byteswr, IntPtr.Zero);
						uint lenght = BitConverter.ToUInt32(bytecount, 0);	
						byte[] buffer = new byte[lenght];
						SerialFuncCs.ClearCommBreak(btest);
						SerialFuncCs.EscapeCommFunction(btest, CTS);
						SerialFuncCs.EscapeCommFunction(btest, pEvtMask);
						SerialFuncCs.WaitCommEvent(btest, ref pEvtMask2, IntPtr.Zero);
						test2 = SerialFuncCs.ReadFile(btest, buffer, Convert.ToUInt32(buffer.Length), ref byteswr, IntPtr.Zero);
						this.status_l.Text = "ZAPISUJE DANE";
						file.Write(buffer, 0, Convert.ToInt32(lenght));
						this.status_l.Text = "DANE ZAPISANE";
						SerialFuncCs.CloseHandle(btest);
						CalibCs.BTLibCs.BTLib_DeInitialize();
					}	
				}
			}*/
			Form5 frm5 = new Form5();
			frm5.Show();

		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			/*CalibCs.BTLibCs.BTLib_Initialize();
			IntPtr btest = CalibCs.SerialFuncCs.CreateFile("COM5:", CalibCs.SerialFuncCs.GENERIC_WRITE | CalibCs.SerialFuncCs.GENERIC_READ, 0, IntPtr.Zero, CalibCs.SerialFuncCs.OPEN_EXISTING, CalibCs.SerialFuncCs.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
			CalibCs.SerialFuncCs.DCB portconfig = new CalibCs.SerialFuncCs.DCB();
			bool test2 = CalibCs.SerialFuncCs.GetCommState(btest, ref portconfig);
			portconfig.BaudRate = SerialFuncCs.CBR_19200;
			portconfig.Flag32 |= SerialFuncCs.fBinary;
			portconfig.Parity = SerialFuncCs.NOPARITY;
			portconfig.ByteSize = 8;
			portconfig.StopBits = SerialFuncCs.ONESTOPBIT;
			test2 = CalibCs.SerialFuncCs.SetCommState(btest, ref portconfig);

			status_l.Text = "CZEKAM NA PO£¥CZENIE";
			
			int licznik = 1;
			uint modemstat = 0;
			while(modemstat != 176)
			{
				licznik += licznik;
				status_l.Text = "CZEKAM NA PO£¥CZENIE" + licznik.ToString();
				SerialFuncCs.GetCommModemStatus(btest, ref modemstat);
			}
			System.IO.FileStream file = new FileStream("\\FlashDisk\\Inwent.imp", FileMode.Open, FileAccess.Read);
			//uint pEvtMask2 = 0;
	
			
			//byte[] flaga = new byte[1];
			//uint flagaread = 0;
			byte[] buffer = new byte[file.Length];
			file.Read(buffer, 0, buffer.Length);
			long lenght = file.Length;
			
			byte[] bytec = BitConverter.GetBytes(lenght);
			//label1.Text = BitConverter.ToInt32(bytecount, 0).ToString();
			uint bytestw = Convert.ToUInt32(buffer.Length);
			uint byteswr = new uint();
			//	label1.Text = modemstat.ToString();
			//	label2.Text = SerialFuncCs.EV_RLSD.ToString();
			//	label3.Text = SerialFuncCs.EV_RING.ToString();
			//MessageBox.Show("po³¹cz");
			//	modemstat = 0;
			//	SerialFuncCs.GetCommModemStatus(btest, ref modemstat);
			//	label1.Text = modemstat.ToString();
			//	SerialFuncCs.WaitCommEvent(btest, ref pEvtMask, IntPtr.Zero);
			//	label2.Text = pEvtMask.ToString();
			//	pEvtMask = 0;
			//MessageBox.Show("wyœlij");
            //label2.Text = bytecount.Length.ToString();
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
			//pEvtMask = 0;
			//label2.Text = pEvtMask.ToString();
			SerialFuncCs.CloseHandle(btest);
			CalibCs.BTLibCs.BTLib_DeInitialize();
			file.Close();
		*/
		Form12 frm12 = new Form12(lic);
			frm12.Show();

		
		}

		private void search_b_Click(object sender, System.EventArgs e)
		{
			Form6 frm6 = new Form6();
			frm6.Show();
		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			Form7_1_2_1 frm7_1_2_1 = new Form7_1_2_1();
			frm7_1_2_1.Show();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			Form5_1 frm5_1 = new Form5_1();
			frm5_1.Show();
		}

		
	}
}
