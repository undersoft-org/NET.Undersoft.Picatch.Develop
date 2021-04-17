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

namespace SmartDeviceApplication2
{
	
	/// <summary>
	/// Summary description for Form6.
	/// </summary>
	public class Form71 : System.Windows.Forms.Form
	{
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
		private System.Windows.Forms.Button spcen_b;
		private System.Windows.Forms.Button towary_b;
		private int lic;
		
		public Form71()
		{
			//
			// Required for Windows Form Designer support
			//
			//lic = licence;
			string connectionString;
		
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
			
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			
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
			this.spcen_b = new System.Windows.Forms.Button();
			this.towary_b = new System.Windows.Forms.Button();
			this.exit_b = new System.Windows.Forms.Button();
			this.search_b = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			// 
			// spcen_b
			// 
			this.spcen_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.spcen_b.Location = new System.Drawing.Point(16, 88);
			this.spcen_b.Size = new System.Drawing.Size(208, 48);
			this.spcen_b.Text = "SPRAWDZARKA";
			this.spcen_b.Click += new System.EventHandler(this.spcen_b_Click);
			// 
			// towary_b
			// 
			this.towary_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.towary_b.Location = new System.Drawing.Point(16, 32);
			this.towary_b.Size = new System.Drawing.Size(208, 48);
			this.towary_b.Text = "TOWARY";
			this.towary_b.Click += new System.EventHandler(this.towary_b_Click);
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(16, 200);
			this.exit_b.Size = new System.Drawing.Size(208, 48);
			this.exit_b.Text = "WYJŒCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// search_b
			// 
			this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.search_b.Location = new System.Drawing.Point(16, 144);
			this.search_b.Size = new System.Drawing.Size(208, 48);
			this.search_b.Text = "KONTRAHENCI";
			this.search_b.Click += new System.EventHandler(this.search_b_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(8, 264);
			this.label1.Size = new System.Drawing.Size(224, 24);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form71
			// 
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(234, 294);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.search_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.towary_b);
			this.Controls.Add(this.spcen_b);
			this.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Kartoteki";

		}
		#endregion

		private void spcen_b_Click(object sender, System.EventArgs e)
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
			Form7_1_2 frm7_1_2 = new Form7_1_2(lic);
			frm7_1_2.Show();

		}

		private void towary_b_Click(object sender, System.EventArgs e)
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
		Form13 frm13 = new Form13();
			frm13.Show();

		
		}

		private void search_b_Click(object sender, System.EventArgs e)
		{
			Form7_1_3 frm7_1_3 = new Form7_1_3();
			frm7_1_3.Show();
		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		
	}
}
