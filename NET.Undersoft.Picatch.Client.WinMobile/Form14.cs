using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using Microsoft.WindowsCE.Forms;
using System.Data.SqlClient;


namespace SmartDeviceApplication2
{
	/// <summary>
	/// Summary description for Form14.
	/// </summary>
	public class Form14 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.TextBox dfile_t;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox ufile_t;
		private System.Windows.Forms.CheckBox bflag_t;
		private System.Windows.Forms.ComboBox bdll_t;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.ComboBox com_t;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox skaner_t;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.CheckBox ipflag_t;
		private System.Windows.Forms.TextBox ip_t;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox port_t;
		private System.Windows.Forms.TabPage tabPage2;
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
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Button button3;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private string skaner;

		public Form14()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			this.Update();
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
			
				com_t.Text = dr.GetString(2);
				ip_t.Text = dr.GetString(3);
				ufile_t.Text = dr.GetString(4);
				dfile_t.Text = dr.GetString(5);
				bdll_t.Text = dr.GetString(6);
				bflag_t.Checked = dr.GetBoolean(7);
				ipflag_t.Checked = dr.GetBoolean(8);
				port_t.Text = Convert.ToString(dr.GetInt32(9));
				skaner_t.Text = dr.GetString(10);
			}
			cn.Close();
			

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		private void save_options()
		{
			string connectionString;
			string fileName = "Baza.sdf";
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM opcje", cn);
			DataTable table = new DataTable();
			da.Fill(table);
			da.UpdateCommand = new SqlCeCommand("UPDATE opcje SET com = ?, ip = ?, ufile = ?, dfile = ?, bdll = ?, bflag = ?, ipflag = ?, port = ?, skaner = ? WHERE id =  1", cn);
			da.UpdateCommand.Parameters.Add("@c", SqlDbType.NVarChar, 20);	
			da.UpdateCommand.Parameters.Add("@i", SqlDbType.NVarChar, 20);	
			da.UpdateCommand.Parameters.Add("@u", SqlDbType.NVarChar, 120);
			da.UpdateCommand.Parameters.Add("@d", SqlDbType.NVarChar, 120);
			da.UpdateCommand.Parameters.Add("@bd", SqlDbType.NVarChar, 50);
			da.UpdateCommand.Parameters.Add("@bf", SqlDbType.Bit);
			da.UpdateCommand.Parameters.Add("@if", SqlDbType.Bit);
			da.UpdateCommand.Parameters.Add("@p", SqlDbType.Int);
			da.UpdateCommand.Parameters.Add("@s", SqlDbType.NVarChar, 120);
			da.UpdateCommand.Parameters["@c"].Value = com_t.Text;
			da.UpdateCommand.Parameters["@i"].Value = ip_t.Text;
			da.UpdateCommand.Parameters["@u"].Value = ufile_t.Text;
			da.UpdateCommand.Parameters["@d"].Value = dfile_t.Text;
			da.UpdateCommand.Parameters["@bd"].Value = bdll_t.Text;
			da.UpdateCommand.Parameters["@bf"].Value = bflag_t.CheckState;
			da.UpdateCommand.Parameters["@if"].Value = ipflag_t.CheckState;
			da.UpdateCommand.Parameters["@p"].Value = int.Parse(port_t.Text);
			da.UpdateCommand.Parameters["@s"].Value = skaner_t.Text;
			da.UpdateCommand.ExecuteNonQuery();
			cn.Close();
			this.Close();
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.skaner_t = new System.Windows.Forms.TextBox();
            this.ufile_t = new System.Windows.Forms.TextBox();
            this.dfile_t = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.com_t = new System.Windows.Forms.ComboBox();
            this.bdll_t = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bflag_t = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.port_t = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ip_t = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ipflag_t = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(234, 184);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.skaner_t);
            this.tabPage1.Controls.Add(this.ufile_t);
            this.tabPage1.Controls.Add(this.dfile_t);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(226, 156);
            this.tabPage1.Text = "G³ówne";
            // 
            // skaner_t
            // 
            this.skaner_t.Location = new System.Drawing.Point(24, 120);
            this.skaner_t.Name = "skaner_t";
            this.skaner_t.Size = new System.Drawing.Size(168, 23);
            this.skaner_t.TabIndex = 0;
            // 
            // ufile_t
            // 
            this.ufile_t.Location = new System.Drawing.Point(24, 80);
            this.ufile_t.Name = "ufile_t";
            this.ufile_t.Size = new System.Drawing.Size(168, 23);
            this.ufile_t.TabIndex = 1;
            // 
            // dfile_t
            // 
            this.dfile_t.Location = new System.Drawing.Point(24, 40);
            this.dfile_t.Name = "dfile_t";
            this.dfile_t.Size = new System.Drawing.Size(168, 23);
            this.dfile_t.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(24, 104);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 20);
            this.label5.Text = "Œcie¿ka programu skanera";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(24, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(160, 20);
            this.label2.Text = "Œcie¿ka pliku wysy³anego";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(24, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.Text = "Œcie¿ka pliku pobieranego";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.com_t);
            this.tabPage2.Controls.Add(this.bdll_t);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.bflag_t);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(217, 156);
            this.tabPage2.Text = "Bluetooth";
            // 
            // com_t
            // 
            this.com_t.Items.Add("COM1");
            this.com_t.Items.Add("COM2");
            this.com_t.Items.Add("COM3");
            this.com_t.Items.Add("COM4");
            this.com_t.Items.Add("COM5");
            this.com_t.Items.Add("COM6");
            this.com_t.Items.Add("COM7");
            this.com_t.Items.Add("COM8");
            this.com_t.Items.Add("COM9");
            this.com_t.Location = new System.Drawing.Point(24, 112);
            this.com_t.Name = "com_t";
            this.com_t.Size = new System.Drawing.Size(136, 23);
            this.com_t.TabIndex = 0;
            // 
            // bdll_t
            // 
            this.bdll_t.Items.Add("BTLibCs");
            this.bdll_t.Items.Add("MSStack");
            this.bdll_t.Location = new System.Drawing.Point(24, 64);
            this.bdll_t.Name = "bdll_t";
            this.bdll_t.Size = new System.Drawing.Size(136, 23);
            this.bdll_t.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(24, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 20);
            this.label4.Text = "Port COM";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(24, 48);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(128, 16);
            this.label3.Text = "Biblioteka obs³ugi";
            // 
            // bflag_t
            // 
            this.bflag_t.Location = new System.Drawing.Point(24, 16);
            this.bflag_t.Name = "bflag_t";
            this.bflag_t.Size = new System.Drawing.Size(136, 20);
            this.bflag_t.TabIndex = 4;
            this.bflag_t.Text = "Transfer Bluetooth";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.port_t);
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.ip_t);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.ipflag_t);
            this.tabPage3.Location = new System.Drawing.Point(4, 24);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(217, 156);
            this.tabPage3.Text = "Sieæ TCP/IP";
            // 
            // port_t
            // 
            this.port_t.Location = new System.Drawing.Point(24, 104);
            this.port_t.Name = "port_t";
            this.port_t.Size = new System.Drawing.Size(168, 23);
            this.port_t.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(24, 88);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 20);
            this.label7.Text = "Port Serwera";
            // 
            // ip_t
            // 
            this.ip_t.Location = new System.Drawing.Point(24, 64);
            this.ip_t.Name = "ip_t";
            this.ip_t.Size = new System.Drawing.Size(168, 23);
            this.ip_t.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(24, 48);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(168, 20);
            this.label6.Text = "Adres IP Serwera";
            // 
            // ipflag_t
            // 
            this.ipflag_t.Location = new System.Drawing.Point(24, 16);
            this.ipflag_t.Name = "ipflag_t";
            this.ipflag_t.Size = new System.Drawing.Size(160, 20);
            this.ipflag_t.TabIndex = 4;
            this.ipflag_t.Text = "Transfer TCP/IP";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(20, 224);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 40);
            this.button1.TabIndex = 3;
            this.button1.Text = "ZAPISZ";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(122, 224);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 40);
            this.button2.TabIndex = 2;
            this.button2.Text = "WYJŒCIE";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label8.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label8.Location = new System.Drawing.Point(8, 272);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(224, 16);
            this.label8.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(72, 188);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(90, 32);
            this.button3.TabIndex = 0;
            this.button3.Text = "KLAWIATURA";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form14
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.ControlBox = false;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form14";
            this.Text = "OPCJE";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			save_options();
			this.Close();
		}

		private void button3_Click(object sender, System.EventArgs e)
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
