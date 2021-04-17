using System;
using System.Data.Common;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using Microsoft.WindowsCE.Forms;
using System.Runtime.InteropServices;

namespace SmartDeviceApplication2
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form7_1_2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox kod_t;
		private System.Windows.Forms.Label kod_l;
		private System.Windows.Forms.TextBox cena_t;
		private System.Windows.Forms.Label cena_l;
		private System.Windows.Forms.TextBox stan_t;
		private System.Windows.Forms.Label nazwa_l;
		private System.Windows.Forms.TextBox nazwa_t;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button search_b;
		private System.Windows.Forms.Label stan_l;
		private System.Windows.Forms.Button ok_b;
		
		private string connectionString;
		private SqlCeConnection cn;
		private string index;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox cenasp_t;
		private int rownum;
		private System.Windows.Forms.Label ilosc_l;
		private System.Windows.Forms.TextBox ilosc_t;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox bad_stan_c;
		private System.Windows.Forms.CheckBox bad_cena_c;
		private System.Windows.Forms.TextBox vat_t;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox cenazk_t;
		private int lic;
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

		public Form7_1_2(int licence)
		{
			//
			// Required for Windows Form Designer support
			//
			lic = licence;
			
			
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			cn = new SqlCeConnection(connectionString);
			cn.Open();
			

			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			kod_t.Focus();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		
		private void FindIndex()
		{
			
			string kodbuf = kod_t.Text;
			int wagaflag = 0;
			string czywag = kodbuf.Substring(0, 2);
			string waga = "";
			string kodwag = "";
			string kodwag2 = "";
			if (czywag == "27" || czywag == "28" || czywag == "29")
			{
				if (kodbuf.Length == 13)
				{
					waga = kodbuf.Substring(kodbuf.Length - 6, 5);
					kodwag = kodbuf.Substring(0, 6);
					kodwag2 = kodbuf.Substring(2, 4);
					wagaflag = 1;
				}
			}

			//int rowqty = 0;
			kod_t.Text = "SZUKAM TOWARU W BAZIE";
			kod_t.Refresh();
			//SqlCeCommand cmd2 = cn.CreateCommand();
			//cmd2.CommandText = "SELECT kod, COUNT(nazwa) FROM dane WHERE kod = ? GROUP BY kod";
			//cmd2.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
			//cmd2.Parameters["@k"].Value = kodbuf;
			//cmd2.Prepare();
			//SqlCeDataReader dr1 = cmd2.ExecuteReader();
			
			//while (dr1.Read())
			//{
			//	rowqty = dr1.GetInt32(1);
			//}

			//if (rowqty > 0)
			//{
			if (wagaflag == 0)
			{
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "SELECT kod, nazwa, stan, cenazk, cenasp, vat, bad_cena, bad_stan, zliczono, cenapolka FROM dane WHERE kod = ?";
				cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
				cmd.Parameters["@k"].Value = kodbuf;

				cmd.Prepare();
				SqlCeDataReader dr = cmd.ExecuteReader();
			
				
				while (dr.Read())
				{
					nazwa_t.Text = dr.GetString(1);
					stan_t.Text = dr.GetString(2);
					cenazk_t.Text = dr.GetString(3);
					cenasp_t.Text = dr.GetString(4);
					vat_t.Text = dr.GetString(5);
					bad_cena_c.Checked = dr.GetBoolean(6);
					bad_stan_c.Checked = dr.GetBoolean(7);
					ilosc_t.Text = Convert.ToString(dr.GetSqlDecimal(8));
					cena_t.Text = Convert.ToString(dr.GetSqlDecimal(9));
				}
				cmd.Dispose();
				dr.Dispose();
			
				
				kod_t.Text = kodbuf;
			//	ilosc_t.Focus();
			}
			else if (wagaflag == 1)
			{
				string like = "kod LIKE '" + kodwag + ".......'";
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "SELECT kod, nazwa, stan, cenazk, cenasp, vat, bad_cena, bad_stan, zliczono, cenapolka FROM dane WHERE " + like;
				cmd.Prepare();
				SqlCeDataReader dr = cmd.ExecuteReader();
			
				
				while (dr.Read())
				{
					nazwa_t.Text = dr.GetString(1);
					stan_t.Text = dr.GetString(2);
					cena_t.Text = dr.GetString(3);
					cenasp_t.Text = dr.GetString(4);
					vat_t.Text = dr.GetString(5);
					bad_cena_c.Checked = dr.GetBoolean(6);
					bad_stan_c.Checked = dr.GetBoolean(7);
					ilosc_t.Text = Convert.ToString(dr.GetSqlDecimal(8));
					cena_t.Text = Convert.ToString(dr.GetSqlDecimal(9));
				}
				cmd.Dispose();
				dr.Dispose();
				
				
				kod_t.Text = kodwag;
			//	ilosc_t.Focus();
			}
			if (nazwa_t.Text == null || nazwa_t.Text == "")
			{
				DialogResult dialog = MessageBox.Show("Nie znaleziono kodu towaru");
					kod_t.Focus();
				
			}
		

		}

		private void WriteLine ()
		{
			
			/*string stop = "start";
			if (lic == 0)
			{
				int demo = 0;
				SqlCeCommand licek = cn.CreateCommand();
				licek.CommandText = "Select Count(id) From dane Where zmiany = true";
				SqlCeDataReader dr = licek.ExecuteReader();
				while (dr.Read())
				{
					demo = dr.GetInt32(0);
				}
				if (demo >= 5)
				{
					stop = "stop";
				}
			}
			
			
			if (stop == "stop")
			{
				MessageBox.Show("Wersja Demo zmienê 5 pozycji");
			}
			else
			{*/
			
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
				cmd.Parameters["@p"].Value = decimal.Parse(cena_t.Text);
				cmd.Parameters["@z"].Value = decimal.Parse(ilosc_t.Text);
				cmd.Parameters["@bc"].Value = bad_cena_c.CheckState;
				cmd.Parameters["@bs"].Value = bad_stan_c.CheckState;
			    cmd.Parameters["@k"].Value = kod_t.Text;
			
				
				cmd.Prepare();
				cmd.ExecuteNonQuery();
				kod_t.Text = null;
				nazwa_t.Text = null;
				cena_t.Text = null;
				ilosc_t.Text = null;
				stan_t.Text = null;
				cenasp_t.Text = null;
				vat_t.Text = null;
				bad_cena_c.Checked = false;
				bad_stan_c.Checked = false;
				kod_t.Focus();
			
			
				
			

		}
			
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			base.Dispose( disposing );
		}
		
		[DllImport("coredll.dll", SetLastError = true)]
			//[return: System.Runtime.InteropServices.Marshal(UnmanagedType.Bool)]
		public static extern bool SipGetInfo(ref SIPINFO sipInfo);

		[DllImport("coredll.dll", SetLastError = true)]
		//[return: MarshalAs(UnmanagedType.Bool)]
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

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.kod_t = new System.Windows.Forms.TextBox();
            this.search_b = new System.Windows.Forms.Button();
            this.kod_l = new System.Windows.Forms.Label();
            this.cena_t = new System.Windows.Forms.TextBox();
            this.cena_l = new System.Windows.Forms.Label();
            this.stan_t = new System.Windows.Forms.TextBox();
            this.stan_l = new System.Windows.Forms.Label();
            this.nazwa_l = new System.Windows.Forms.Label();
            this.nazwa_t = new System.Windows.Forms.TextBox();
            this.exit_b = new System.Windows.Forms.Button();
            this.ok_b = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cenasp_t = new System.Windows.Forms.TextBox();
            this.ilosc_l = new System.Windows.Forms.Label();
            this.ilosc_t = new System.Windows.Forms.TextBox();
            this.bad_stan_c = new System.Windows.Forms.CheckBox();
            this.bad_cena_c = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.vat_t = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cenazk_t = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // kod_t
            // 
            this.kod_t.BackColor = System.Drawing.Color.Gold;
            this.kod_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
            this.kod_t.ForeColor = System.Drawing.Color.Black;
            this.kod_t.Location = new System.Drawing.Point(16, 20);
            this.kod_t.Name = "kod_t";
            this.kod_t.Size = new System.Drawing.Size(208, 26);
            this.kod_t.TabIndex = 18;
            this.kod_t.GotFocus += new System.EventHandler(this.kod_t_GotFocus);
            this.kod_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.kod_t_KeyPress);
            // 
            // search_b
            // 
            this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.search_b.Location = new System.Drawing.Point(152, 48);
            this.search_b.Name = "search_b";
            this.search_b.Size = new System.Drawing.Size(72, 24);
            this.search_b.TabIndex = 17;
            this.search_b.Text = "SZUKAJ";
            this.search_b.Click += new System.EventHandler(this.search_b_Click);
            // 
            // kod_l
            // 
            this.kod_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.kod_l.ForeColor = System.Drawing.Color.Gold;
            this.kod_l.Location = new System.Drawing.Point(16, 5);
            this.kod_l.Name = "kod_l";
            this.kod_l.Size = new System.Drawing.Size(88, 16);
            this.kod_l.Text = "KOD Towaru";
            // 
            // cena_t
            // 
            this.cena_t.AcceptsReturn = true;
            this.cena_t.BackColor = System.Drawing.Color.Gold;
            this.cena_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.cena_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cena_t.Location = new System.Drawing.Point(56, 168);
            this.cena_t.Name = "cena_t";
            this.cena_t.Size = new System.Drawing.Size(72, 23);
            this.cena_t.TabIndex = 15;
            this.cena_t.GotFocus += new System.EventHandler(this.cena_t_GotFocus);
           
            // 
            // cena_l
            // 
            this.cena_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.cena_l.ForeColor = System.Drawing.Color.Gold;
            this.cena_l.Location = new System.Drawing.Point(52, 152);
            this.cena_l.Name = "cena_l";
            this.cena_l.Size = new System.Drawing.Size(84, 20);
            this.cena_l.Text = "Cena na pó³ce";
            this.cena_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // stan_t
            // 
            this.stan_t.BackColor = System.Drawing.Color.Azure;
            this.stan_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.stan_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.stan_t.Location = new System.Drawing.Point(144, 128);
            this.stan_t.Name = "stan_t";
            this.stan_t.ReadOnly = true;
            this.stan_t.Size = new System.Drawing.Size(72, 23);
            this.stan_t.TabIndex = 14;
            // 
            // stan_l
            // 
            this.stan_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.stan_l.ForeColor = System.Drawing.Color.Gold;
            this.stan_l.Location = new System.Drawing.Point(152, 112);
            this.stan_l.Name = "stan_l";
            this.stan_l.Size = new System.Drawing.Size(56, 20);
            this.stan_l.Text = "Stan sys.";
            this.stan_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // nazwa_l
            // 
            this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.nazwa_l.ForeColor = System.Drawing.Color.Gold;
            this.nazwa_l.Location = new System.Drawing.Point(16, 58);
            this.nazwa_l.Name = "nazwa_l";
            this.nazwa_l.Size = new System.Drawing.Size(104, 16);
            this.nazwa_l.Text = "Nazwa Towaru";
            // 
            // nazwa_t
            // 
            this.nazwa_t.BackColor = System.Drawing.Color.Azure;
            this.nazwa_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.nazwa_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nazwa_t.Location = new System.Drawing.Point(16, 73);
            this.nazwa_t.Multiline = true;
            this.nazwa_t.Name = "nazwa_t";
            this.nazwa_t.ReadOnly = true;
            this.nazwa_t.Size = new System.Drawing.Size(208, 40);
            this.nazwa_t.TabIndex = 12;
            this.nazwa_t.GotFocus += new System.EventHandler(this.nazwa_t_GotFocus);
            this.nazwa_t.LostFocus += new System.EventHandler(this.nazwa_t_LostFocus);
            // 
            // exit_b
            // 
            this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.exit_b.Location = new System.Drawing.Point(120, 230);
            this.exit_b.Name = "exit_b";
            this.exit_b.Size = new System.Drawing.Size(104, 32);
            this.exit_b.TabIndex = 10;
            this.exit_b.Text = "WYJŒCIE";
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            // 
            // ok_b
            // 
            this.ok_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.ok_b.Location = new System.Drawing.Point(8, 230);
            this.ok_b.Name = "ok_b";
            this.ok_b.Size = new System.Drawing.Size(104, 32);
            this.ok_b.TabIndex = 9;
            this.ok_b.Text = "DALEJ";
            this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(8, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 24);
            this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(136, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(88, 20);
            this.button1.TabIndex = 7;
            this.button1.Text = "KLAWIATURA";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Gold;
            this.label2.Location = new System.Drawing.Point(136, 152);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 16);
            this.label2.Text = "CenaSp (brutto)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cenasp_t
            // 
            this.cenasp_t.BackColor = System.Drawing.Color.Azure;
            this.cenasp_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.cenasp_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cenasp_t.Location = new System.Drawing.Point(144, 168);
            this.cenasp_t.Name = "cenasp_t";
            this.cenasp_t.ReadOnly = true;
            this.cenasp_t.Size = new System.Drawing.Size(72, 23);
            this.cenasp_t.TabIndex = 6;
            // 
            // ilosc_l
            // 
            this.ilosc_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.ilosc_l.ForeColor = System.Drawing.Color.Gold;
            this.ilosc_l.Location = new System.Drawing.Point(64, 112);
            this.ilosc_l.Name = "ilosc_l";
            this.ilosc_l.Size = new System.Drawing.Size(56, 20);
            this.ilosc_l.Text = "Stan fakt.";
            this.ilosc_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ilosc_t
            // 
            this.ilosc_t.BackColor = System.Drawing.Color.Gold;
            this.ilosc_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.ilosc_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.ilosc_t.Location = new System.Drawing.Point(56, 128);
            this.ilosc_t.Name = "ilosc_t";
            this.ilosc_t.Size = new System.Drawing.Size(72, 23);
            this.ilosc_t.TabIndex = 11;
            this.ilosc_t.GotFocus += new System.EventHandler(this.ilosc_t_GotFocus);
            this.ilosc_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ilosc_t_KeyPress);
            // 
            // bad_stan_c
            // 
            this.bad_stan_c.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
            this.bad_stan_c.Location = new System.Drawing.Point(20, 128);
            this.bad_stan_c.Name = "bad_stan_c";
            this.bad_stan_c.Size = new System.Drawing.Size(16, 20);
            this.bad_stan_c.TabIndex = 4;
            // 
            // bad_cena_c
            // 
            this.bad_cena_c.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
            this.bad_cena_c.Location = new System.Drawing.Point(20, 168);
            this.bad_cena_c.Name = "bad_cena_c";
            this.bad_cena_c.Size = new System.Drawing.Size(16, 20);
            this.bad_cena_c.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.Gold;
            this.label3.Location = new System.Drawing.Point(0, 112);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 20);
            this.label3.Text = "z³y stan";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Gold;
            this.label4.Location = new System.Drawing.Point(0, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 20);
            this.label4.Text = "z³a cena";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // vat_t
            // 
            this.vat_t.BackColor = System.Drawing.Color.Azure;
            this.vat_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.vat_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.vat_t.Location = new System.Drawing.Point(144, 204);
            this.vat_t.Name = "vat_t";
            this.vat_t.ReadOnly = true;
            this.vat_t.Size = new System.Drawing.Size(72, 23);
            this.vat_t.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.Gold;
            this.label5.Location = new System.Drawing.Point(128, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 16);
            this.label5.Text = "VAT %";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // cenazk_t
            // 
            this.cenazk_t.BackColor = System.Drawing.Color.Azure;
            this.cenazk_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.cenazk_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cenazk_t.Location = new System.Drawing.Point(56, 204);
            this.cenazk_t.Name = "cenazk_t";
            this.cenazk_t.ReadOnly = true;
            this.cenazk_t.Size = new System.Drawing.Size(72, 23);
            this.cenazk_t.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.Gold;
            this.label6.Location = new System.Drawing.Point(40, 192);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 16);
            this.label6.Text = "CenaZk(netto)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Form7_1_2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.ControlBox = false;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cenazk_t);
            this.Controls.Add(this.vat_t);
            this.Controls.Add(this.bad_cena_c);
            this.Controls.Add(this.bad_stan_c);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cenasp_t);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ok_b);
            this.Controls.Add(this.exit_b);
            this.Controls.Add(this.ilosc_t);
            this.Controls.Add(this.nazwa_t);
            this.Controls.Add(this.nazwa_l);
            this.Controls.Add(this.stan_t);
            this.Controls.Add(this.cena_t);
            this.Controls.Add(this.kod_l);
            this.Controls.Add(this.search_b);
            this.Controls.Add(this.kod_t);
            this.Controls.Add(this.stan_l);
            this.Controls.Add(this.ilosc_l);
            this.Controls.Add(this.cena_l);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form7_1_2";
            this.Text = "Wprowadzanie Pozycji";
            this.ResumeLayout(false);

		}
		#endregion

		private void search_b_Click(object sender, System.EventArgs e)
		{
			
			
			FindIndex();
		}

		private void kod_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
			
			if (e.KeyChar == 13)
			{
				
				FindIndex();
			}
		}

		private void ilosc_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
			
			
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}

			// only allow one decimal point
			if (e.KeyChar == '.' 
				&& (sender as TextBox).Text.IndexOf('.') > -1)
			{
				e.Handled = true;
			}

			if (e.KeyChar == 13)
			{
				if (ilosc_t.Text != "" && kod_t.Text != "")
				{
					cena_t.Focus();
					
									
				}
				else if (kod_t.Text == "")
				{
					MessageBox.Show("WprowadŸ kod towaru");
				}		
				
			}
		}

		private void ok_b_Click(object sender, System.EventArgs e)
		{
			if (kod_t.Text != "")
			{
				
				
				WriteLine();
				kod_t.Text = null;
				nazwa_t.Text = null;
				cenazk_t.Text = null;				
				stan_t.Text = null;
				cenasp_t.Text = null;
				vat_t.Text = null;
				stan_t.ReadOnly = true;
				
				nazwa_t.ReadOnly = true;
				
				cenasp_t.ReadOnly = true;
				vat_t.ReadOnly = true;
				cenazk_t.ReadOnly = true;
				kod_t.Focus();
		
			}
			else if (kod_t.Text == "")
			{
				MessageBox.Show("WprowadŸ kod towaru");
			}
			


		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = false;
			cn.Close();
			Form71 frm71 = new Form71();
			frm71.Show();
			this.Close();

		}

		private void nazwa_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				cena_t.Focus();
			}
		}

		private void cena_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
		}

		private void nazwa_t_GotFocus(object sender, System.EventArgs e)
		{
		ShowInputPanel();
		}

		/*private void inputPanel1_EnabledChanged(object sender, System.EventArgs e)
		{
			if (inputPanel1.Enabled == false)
			{
				// The SIP is disabled, so set the height of the tab control
				// to its original height with a variable (TabOriginalHeight),
				// which is determined during initialization of the form.
				VisibleRect = inputPanel1.VisibleDesktop;
				tabControl1.Height = TabOriginalHeight;
			}
			else
			{
				// The SIP is enabled, so the height of the tab control
				// is set to the height of the visible desktop area.
				VisibleRect = inputPanel1.VisibleDesktop;
				tabControl1.Height = VisibleRect.Height;
			}

			// The Bounds property always returns a width of 240 and a height of 80
			// pixels for Pocket PCs, regardless of whether or not the SIP is enabled.
			BoundsRect = inputPanel1.Bounds;

			// Show the VisibleDestkop and Bounds values
			// on the second tab for demonstration purposes.
			VisibleInfo.Text = String.Format("VisibleDesktop: X = {0}, " + "Y = {1}, Width = {2}, Height = {3}", VisibleRect.X,	VisibleRect.Y, VisibleRect.Width, VisibleRect.Height);
			BoundsInfo.Text = String.Format("Bounds: X = {0}, Y = {1}, " + "Width = {2}, Height = {3}", BoundsRect.X, BoundsRect.Y,	BoundsRect.Width, BoundsRect.Height);
		}*/

		private void nazwa_t_LostFocus(object sender, System.EventArgs e)
		{
		inputPanel1.Enabled = false;
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			if (inputPanel1.Enabled == true)
			{
				inputPanel1.Enabled = false;
			}
			else
			{
				ShowInputPanel();
			}
			ilosc_t.SelectAll();
		}

		private void kod_t_GotFocus(object sender, System.EventArgs e)
		{
			kod_t.Text = "";
		}

		private void ilosc_t_GotFocus(object sender, System.EventArgs e)
		{
			ilosc_t.SelectAll();
		}

		private void cena_t_GotFocus(object sender, System.EventArgs e)
		{
			cena_t.SelectAll();
		}

      

		

		

		

		

		

			

	

		

		
		

	}
}
