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
	public class Info : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		
		
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox serial_t;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox deviceid;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private string serial;
		private string licence;
		
		public Info()
		{
			//
			// Required for Windows Form Designer support
			//
			
		
				
				InitializeComponent();

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
				
				serial = dr.GetString(11);

			}
			serial_t.Text = serial;
			
			licence = GetDeviceID().Substring(1, 10);
			deviceid.Text = licence;
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
				Update();
			
				//
				// TODO: Add any constructor code after InitializeComponent call
				//
			cn.Close();

			
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Info));
			this.label1 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.exit_b = new System.Windows.Forms.Button();
			this.serial_t = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.deviceid = new System.Windows.Forms.TextBox();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(0, 232);
			this.label1.Size = new System.Drawing.Size(240, 24);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT ";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(72, 8);
			this.pictureBox1.Size = new System.Drawing.Size(96, 88);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label2.Location = new System.Drawing.Point(0, 248);
			this.label2.Size = new System.Drawing.Size(240, 24);
			this.label2.Text = "e-mail: dariusz.hanc@jukado.com.pl";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label3.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label3.Location = new System.Drawing.Point(0, 264);
			this.label3.Size = new System.Drawing.Size(240, 24);
			this.label3.Text = "tel: +48 501-747-57";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("JACKIE", 18F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(16, 176);
			this.label4.Size = new System.Drawing.Size(208, 56);
			this.label4.Text = "PICATCH AXC 4.1 rev. 1.0.2";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(120, 144);
			this.exit_b.Size = new System.Drawing.Size(104, 24);
			this.exit_b.Text = "OK";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click_1);
			// 
			// serial_t
			// 
			this.serial_t.Location = new System.Drawing.Point(120, 120);
			this.serial_t.Size = new System.Drawing.Size(104, 20);
			this.serial_t.Text = "";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label5.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label5.Location = new System.Drawing.Point(120, 104);
			this.label5.Size = new System.Drawing.Size(104, 16);
			this.label5.Text = "nr seryjny licencji";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label6.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label6.Location = new System.Drawing.Point(8, 104);
			this.label6.Size = new System.Drawing.Size(104, 16);
			this.label6.Text = "id urz¹dzenia";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// deviceid
			// 
			this.deviceid.Location = new System.Drawing.Point(8, 120);
			this.deviceid.ReadOnly = true;
			this.deviceid.Size = new System.Drawing.Size(104, 20);
			this.deviceid.Text = "";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(8, 144);
			this.button1.Size = new System.Drawing.Size(104, 24);
			this.button1.Text = "KLAWIATURA";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Info
			// 
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.deviceid);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.serial_t);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label1);
			this.Text = "Transfer Danych";

		}
		#endregion
		
		private static Int32 METHOD_BUFFERED = 0;
		private static Int32 FILE_ANY_ACCESS = 0;
		private static Int32 FILE_DEVICE_HAL = 0x00000101;

		private const Int32 ERROR_NOT_SUPPORTED = 0x32;
		private const Int32 ERROR_INSUFFICIENT_BUFFER = 0x7A;

		private static Int32 IOCTL_HAL_GET_DEVICEID = 
			((FILE_DEVICE_HAL) << 16) | ((FILE_ANY_ACCESS) << 14) 
			| ((21) << 2) | (METHOD_BUFFERED);

		[DllImport("coredll.dll", SetLastError=true)]
		private static extern bool KernelIoControl(Int32 dwIoControlCode, 
			IntPtr lpInBuf, Int32 nInBufSize, byte[] lpOutBuf, 
			Int32 nOutBufSize, ref Int32 lpBytesReturned);

		private static string GetDeviceID()
		{
			// Initialize the output buffer to the size of a 
			// Win32 DEVICE_ID structure.
			byte[] outbuff = new byte[20];
			Int32  dwOutBytes;
			bool done = false;

			Int32 nBuffSize = outbuff.Length;

			// Set DEVICEID.dwSize to size of buffer.  Some platforms look at
			// this field rather than the nOutBufSize param of KernelIoControl
			// when determining if the buffer is large enough.
			BitConverter.GetBytes(nBuffSize).CopyTo(outbuff, 0);  
			dwOutBytes = 0;

			// Loop until the device ID is retrieved or an error occurs.
			while (! done)
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
				i < dwPlatformIDOffset + dwPlatformIDSize; i ++ )  
			{
				sb.Append( String.Format("{0:X2}", outbuff[i]));
			}
			return sb.ToString();
		}

		
		private void save_options()
		{
			if (serial_t.Text.Length == 10)
			{
				string connectionString;
				string fileName = "Baza.sdf";
				connectionString = "DataSource=Baza.sdf; Password=matrix1";
				SqlCeConnection cn = new SqlCeConnection(connectionString);
				cn.Open();
				SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM opcje", cn);
				DataTable table = new DataTable();
				da.Fill(table);
				da.UpdateCommand = new SqlCeCommand("UPDATE opcje SET licence = ? WHERE id =  1", cn);
				da.UpdateCommand.Parameters.Add("@s", SqlDbType.NVarChar, 40);
				da.UpdateCommand.Parameters["@s"].Value = serial_t.Text;
				da.UpdateCommand.ExecuteNonQuery();
				cn.Close();
			}
			else
			{
				serial_t.Text = "0000000000";
			}
		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void exit_b_Click_1(object sender, System.EventArgs e)
		{
			serial = serial_t.Text;
			save_options();
			int[] checklic = new int[10];
			int[] checkserial = new int[10];
			int[] checklicbuf = new int[10];
			int[] liccount = new int[10];
			if (serial_t.Text.Length == 10)
			{
				
			
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
			
				for (int i = 0; i < checklicbuf.Length; i++)  
				{
				
					char character = (char)checklicbuf[i];
				
					if (character == serial[i])
					{
						liccount[i] = 1;
					}
						
				}
				
			}
			string[] strings = new string[liccount.Length];
			for (int i = 0; i < liccount.Length; i++)
			{
				strings[i] = liccount[i].ToString();
			}
			string licresult = string.Join(string.Empty, strings);
			serial_t.Text = licresult;
			if (licresult == "1111111111")
			{
				MessageBox.Show("Wersja programu zosta³a zarejestrowana");
			}
			else
			{

				MessageBox.Show("Niepoprawny klucz licencji");
			}

			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (inputPanel1.Enabled == true)
			{
				inputPanel1.Enabled = false;
			}
			else
			{
				inputPanel1.Enabled = true;
			}
		}

		
	}
}
