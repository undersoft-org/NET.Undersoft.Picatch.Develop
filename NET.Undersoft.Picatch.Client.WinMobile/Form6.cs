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

namespace SmartDeviceApplication2
{
	
	/// <summary>
	/// Summary description for Form6.
	/// </summary>
	public class Form6 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button btnDiscover;
		private System.Windows.Forms.ListBox lstDevices;
		private System.Windows.Forms.Button Exit;
		public IntPtr[] DeviceHandle = new IntPtr[CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM + 1];
		public BTST_DEVICEINFO[] bt_di = new BTST_DEVICEINFO[CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM];
		private System.Windows.Forms.Button wyslij;
		private System.Windows.Forms.Label label1;
		public BTST_LOCALINFO LocalInfo = new BTST_LOCALINFO();
		
		
			public Form6()
		{
			//
			// Required for Windows Form Designer support
			//
				
				InitializeComponent();
				this.Height = Screen.PrimaryScreen.Bounds.Height;
				this.Width = Screen.PrimaryScreen.Bounds.Width;
				Update();
			CalibCs.BTLibCs.BTLib_Initialize();
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
			DeviceHandle = new IntPtr[CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM + 1];
			uint bt_dmax = CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM;
			
			bt_di = new BTST_DEVICEINFO[CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM];
			
			int czy = CalibCs.BTLibCs.BTLib_Inquiry(DeviceHandle, ref bt_dmax);
			int j;

			if (czy == 0)
			{
				for (j = 0; j < bt_dmax; j++)
				{
					swork = new string(' ', 82);
					bt_di[j].DeviceHandle = new IntPtr(0);
					bt_di[j].DeviceName = swork.ToCharArray();
					bt_di[j].DeviceAddress = "                  ".ToCharArray();
					bt_di[j].DeviceClass = 0;
					bt_di[j].ProfileNumber = 0;
					//for (i = 0; i < CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM; i++)
					//{
					bt_di[j].ProfileType = new ushort[16];
					//}
				}
			
				for (j = 0; j < bt_dmax; j++)
				{
					czy = CalibCs.BTLibCs.BTLib_GetDeviceInfo(ref bt_di[j], DeviceHandle[j]);
				}
			
				if (czy == 0)
					foreach (CalibCs.BTST_DEVICEINFO bd in bt_di)
					{
						string s = new string(bd.DeviceName);
						this.lstDevices.Items.Add(s);
				
					}	
				else
				{
					string s = "nie mo¿na odczytaæ urz¹dzeñ";
					this.lstDevices.Items.Add(s);
		
				}
			}
			else
			{
				string s = "nie znaleziono urz¹dzeñ";
				this.lstDevices.Items.Add(s);
				
			}
			CalibCs.BTLib.BTLib_DeInitialize();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
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
			this.btnDiscover = new System.Windows.Forms.Button();
			this.Exit = new System.Windows.Forms.Button();
			this.lstDevices = new System.Windows.Forms.ListBox();
			this.wyslij = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			// 
			// btnDiscover
			// 
			this.btnDiscover.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.btnDiscover.Location = new System.Drawing.Point(24, 144);
			this.btnDiscover.Size = new System.Drawing.Size(184, 32);
			this.btnDiscover.Text = "WYSZUKAJ PONOWNIE";
			this.btnDiscover.Click += new System.EventHandler(this.btnDiscover_Click);
			// 
			// Exit
			// 
			this.Exit.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.Exit.Location = new System.Drawing.Point(24, 224);
			this.Exit.Size = new System.Drawing.Size(184, 32);
			this.Exit.Text = "WYJŒCIE";
			this.Exit.Click += new System.EventHandler(this.Exit_Click);
			// 
			// lstDevices
			// 
			this.lstDevices.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
			this.lstDevices.Location = new System.Drawing.Point(24, 8);
			this.lstDevices.Size = new System.Drawing.Size(184, 130);
			// 
			// wyslij
			// 
			this.wyslij.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.wyslij.Location = new System.Drawing.Point(24, 184);
			this.wyslij.Size = new System.Drawing.Size(184, 32);
			this.wyslij.Text = "ZAPAMIÊTAJ URZ¥DZENIE";
			this.wyslij.Click += new System.EventHandler(this.wyslij_Click);
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
			// Form6
			// 
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(234, 294);
			this.ControlBox = false;
			this.Controls.Add(this.label1);
			this.Controls.Add(this.lstDevices);
			this.Controls.Add(this.Exit);
			this.Controls.Add(this.wyslij);
			this.Controls.Add(this.btnDiscover);
			this.Text = "Urz¹dzenia Bluetooth";

		}
		#endregion

		private void Exit_Click(object sender, System.EventArgs e)
		{
		this.Close();
			CalibCs.BTLib.BTLib_DeInitialize();
		}

		private void btnDiscover_Click(object sender, System.EventArgs e)
		{
			lstDevices.Items.Clear();
			CalibCs.BTLibCs.BTLib_Initialize();
			LocalInfo = new BTST_LOCALINFO();
			string swork = new string(' ', 82);
			LocalInfo.LocalName = swork.ToCharArray();
			LocalInfo.LocalAddress = "                  ".ToCharArray();
			LocalInfo.Encryption = false;
			LocalInfo.SecurityMode3 = false;
			LocalInfo.LocalMode = 0;
			LocalInfo.LocalClass = 0;
			CalibCs.BTLibCs.BTLib_GetLocalInfo(ref LocalInfo);
			CalibCs.BTLibCs.BTLib_SetLocalInfo(LocalInfo);
			CalibCs.BTLibCs.BTLib_RegistLocalInfo();
			DeviceHandle = new IntPtr[CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM + 1];
			uint bt_dmax = CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM;
			
			BTST_DEVICEINFO[] bt_di = new BTST_DEVICEINFO[CalibCs.BTLibCs.BTDEF_MAX_PROFILE_NUM];
			
			int czy = CalibCs.BTLibCs.BTLib_Inquiry(DeviceHandle, ref bt_dmax);
			int j;

			if (czy == 0)
			{
				for (j = 0; j < bt_dmax; j++)
				{
					swork = new string(' ', 82);
					bt_di[j].DeviceHandle = new IntPtr(0);
					bt_di[j].DeviceName = swork.ToCharArray();
					bt_di[j].DeviceAddress = "                  ".ToCharArray();
					bt_di[j].DeviceClass = 0;
					bt_di[j].ProfileNumber = 0;
					bt_di[j].ProfileType = new ushort[16];
				
				}
			
			
			
				for (j = 0; j < bt_dmax; j++)
				{
					czy = CalibCs.BTLibCs.BTLib_GetDeviceInfo(ref bt_di[j], DeviceHandle[j]);
				}
			
				if (czy == 0)
					foreach (CalibCs.BTST_DEVICEINFO bd in bt_di)
					{
						string s = new string(bd.DeviceName);
						this.lstDevices.Items.Add(s);
				
					}	
				else
				{
					string s = "nie mo¿na odczytaæ urz¹dzeñ";
					this.lstDevices.Items.Add(s);
				
				}
			}
			else
			{
				string s = "nie znaleziono urz¹dzeñ";
				this.lstDevices.Items.Add(s);
		
			}
			CalibCs.BTLib.BTLib_DeInitialize();
		}

			
		
		private void wyslij_Click(object sender, System.EventArgs e)
		{
			CalibCs.BTLibCs.BTLib_Initialize();
			
			int index = lstDevices.SelectedIndex;
			int test = CalibCs.BTLibCs.BTLib_SelectDevice(bt_di[index]);
			string s = new string(bt_di[index].DeviceName);
			this.lstDevices.Items.Add(s);
			if (test == 0)
			{
				MessageBox.Show("Urz¹dzenie pomyœlnie zarejestrowane");
				string pass = "1111";
				byte[] array = Encoding.ASCII.GetBytes(pass);

				CalibCs.BTLib.BTLib_SetPassKey(array);
				CalibCs.BTLibCs.BTLib_CreateBond(bt_di[index]);
				CalibCs.BTLibCs.BTLib_RegistDeviceInfo(bt_di[index]);
			}
			else
			{
				MessageBox.Show("Urz¹dzenie niezarejestrowane");
			}
		}

		
	}
}
