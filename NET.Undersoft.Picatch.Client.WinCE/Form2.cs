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

namespace Undersoft.Picatch
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form2 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox kod_t;
		private System.Windows.Forms.Label kod_l;
		private System.Windows.Forms.TextBox cena_t;
		private System.Windows.Forms.Label cena_l;
		private System.Windows.Forms.TextBox stan_t;
		private System.Windows.Forms.Label nazwa_l;
		private System.Windows.Forms.TextBox nazwa_t;
		private System.Windows.Forms.Label ilosc_l;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button search_b;
		private System.Windows.Forms.Label stan_l;
		private System.Windows.Forms.TextBox ilosc_t;
		private System.Windows.Forms.TextBox zliczono_t;
		private System.Windows.Forms.Button ok_b;
		
		private string connectionString;
		private SqlCeConnection cn;
		private string index;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox vat_t;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox cenasp_t;
		private int rownum;
		private System.Windows.Forms.Button button2;
		private int lic;
		private System.Windows.Forms.Button list_b;
		private string kodbuf;

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

		public Form2(int dokid, int licence, string kod, int run)
		{
			//
			// Required for Windows Form Designer support
			//
			lic = licence;
			rownum = dokid;
			
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			cn = new SqlCeConnection(connectionString);
			cn.Open();
			SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
			DataTable table = new DataTable();
			da.Fill(table);
			index = table.Rows[rownum][0].ToString();;

			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			kod_t.Focus();
			
			if (run == 1)
			{
				kod_t.Text = kod;
				FindIndex();
			}
			
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		
		private void FindIndex()
		{
			
			kodbuf = kod_t.Text;
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
				cmd.CommandText = "SELECT kod, nazwa, stan, cenazk, cenasp, vat FROM dane WHERE kod = ?";
				cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
				cmd.Parameters["@k"].Value = kodbuf;

				cmd.Prepare();
				SqlCeDataReader dr = cmd.ExecuteReader();
			
				
				while (dr.Read())
				{
					nazwa_t.Text = dr.GetString(1);
					stan_t.Text = dr.GetString(2);
					cena_t.Text = dr.GetString(3);
					cenasp_t.Text = dr.GetString(4);
					vat_t.Text = dr.GetString(5);
				}
				cmd.Dispose();
				dr.Dispose();
			//	string zliczono = "0";
			//	cmd = cn.CreateCommand();
				
			//	cmd.CommandText = "SELECT kod, dokid, ilosc FROM bufor WHERE kod = ? and dokid = ?";
			//	cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);	
			//	cmd.Parameters.Add("@d", SqlDbType.Int, 10);	
			//	cmd.Parameters["@k"].Value = kodbuf;
			//	cmd.Parameters["@d"].Value = int.Parse(index);
			//	cmd.Prepare();
			//	dr = cmd.ExecuteReader();
			//	while (dr.Read())
			//	{
			//		zliczono = ((decimal.Parse(zliczono) + dr.GetSqlDecimal(2)).ToString());;
			//	}
			//	zliczono_t.Text = zliczono;
				kod_t.Text = kodbuf;
				ilosc_t.Text = "1";
				
				ilosc_t.Focus();
				ilosc_t.SelectAll();
			}
			else if (wagaflag == 1)
			{
				string like = "kod LIKE '" + kodwag + ".......'";
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "SELECT kod, nazwa, stan, cenazk, cenasp, vat FROM dane WHERE " + like;
				cmd.Prepare();
				SqlCeDataReader dr = cmd.ExecuteReader();
			
				
				while (dr.Read())
				{
					nazwa_t.Text = dr.GetString(1);
					stan_t.Text = dr.GetString(2);
					cena_t.Text = dr.GetString(3);
					cenasp_t.Text = dr.GetString(4);
					vat_t.Text = dr.GetString(5);
					ilosc_t.Text = (int.Parse(waga.Substring(0, 2))).ToString()+"."+waga.Substring(2, 3);
				}
				cmd.Dispose();
				dr.Dispose();
				string zliczono = "0";
				cmd = cn.CreateCommand();
			
				cmd.CommandText = "SELECT kod, dokid, ilosc FROM bufor WHERE dokid = ? and " + like;
				cmd.Parameters.Add("@d", SqlDbType.Int, 10);	
				cmd.Parameters["@d"].Value = int.Parse(index);
				cmd.Prepare();
				dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					zliczono = ((decimal.Parse(zliczono) + dr.GetSqlDecimal(2)).ToString());;
				}
				zliczono_t.Text = zliczono;
				kod_t.Text = kodwag;
				ilosc_t.Focus();
				ilosc_t.SelectAll();
			}
			if (nazwa_t.Text == null || nazwa_t.Text == "")
			{
				DialogResult dialog = MessageBox.Show("Nie znaleziono kodu towaru czy? dodaÊ - Tak, dodaÊ bez nazwy - Anuluj, Nie dodawaÊ - Nie", "Brak towaru", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (dialog == DialogResult.Yes)
				{
					kod_t.Text = kodbuf;
					nazwa_t.ReadOnly = false;
					cena_t.ReadOnly = false;
					nazwa_t.Focus();
					cena_t.Text = "0";
					cenasp_t.Text = "0";
					stan_t.Text = "0";
					vat_t.Text = "0";
					if (wagaflag == 1)
					{
						ilosc_t.Text = waga.Substring(0, 2)+"."+waga.Substring(2, 3);
					}

				}
				else if (dialog == DialogResult.No)
				{
					kod_t.Text = null;
				
					kod_t.Focus();
				}
				else if (dialog == DialogResult.Cancel)
				{
					nazwa_t.ReadOnly = true;
					cena_t.ReadOnly = true;
					kod_t.Text = kodbuf;
					if (wagaflag == 1)
					{
						ilosc_t.Text = waga.Substring(0, 2)+"."+waga.Substring(2, 3);
					}
					
					ilosc_t.Text = "1";
				
					ilosc_t.Focus();
					ilosc_t.SelectAll();
					cena_t.Text = "0";
					cenasp_t.Text = "0";
					stan_t.Text = "0";
					vat_t.Text = "0";

				}
			}

		}

		private void WriteLine ()
		{
			
			string stop = "start";
			if (lic == 0)
			{
				int demo = 0;
				SqlCeCommand licek = cn.CreateCommand();
				licek.CommandText = "Select Count(id) From bufor";
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
				MessageBox.Show("Wersja Demo pozwala na wprowadzenie 5 pozycji");
			}
			else
			{
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "INSERT INTO bufor (dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";
				cmd.Parameters.Add("@d", SqlDbType.Int, 10);
				cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);
				cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100);	
				cmd.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
				cmd.Parameters.Add("@i", SqlDbType.Decimal, 10);
				cmd.Parameters["@i"].Precision = 10;
				cmd.Parameters["@i"].Scale = 3;
				cmd.Parameters.Add("@s", SqlDbType.NVarChar, 10);
				cmd.Parameters.Add("@csp", SqlDbType.NVarChar, 10);
				cmd.Parameters.Add("@v", SqlDbType.NVarChar, 10);
			
				cmd.Parameters["@d"].Value = int.Parse(index);
				cmd.Parameters["@k"].Value = kod_t.Text;
				cmd.Parameters["@n"].Value = nazwa_t.Text;
				cmd.Parameters["@cz"].Value = cena_t.Text;
				cmd.Parameters["@i"].Value = ilosc_t.Text;
				cmd.Parameters["@s"].Value = stan_t.Text;
				cmd.Parameters["@csp"].Value = cenasp_t.Text;
				cmd.Parameters["@v"].Value = vat_t.Text;
				cmd.Prepare();
				cmd.ExecuteNonQuery();
				kod_t.Text = null;
				nazwa_t.Text = null;
				cena_t.Text = null;
				ilosc_t.Text = null;
				zliczono_t.Text = null;
				stan_t.Text = null;
				cenasp_t.Text = null;
				vat_t.Text = null;
				kod_t.Focus();
			}
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
			this.ilosc_t = new System.Windows.Forms.TextBox();
			this.ilosc_l = new System.Windows.Forms.Label();
			this.exit_b = new System.Windows.Forms.Button();
			this.ok_b = new System.Windows.Forms.Button();
			this.zliczono_t = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.vat_t = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cenasp_t = new System.Windows.Forms.TextBox();
			this.button2 = new System.Windows.Forms.Button();
			this.list_b = new System.Windows.Forms.Button();
			// 
			// kod_t
			// 
			this.kod_t.BackColor = System.Drawing.Color.GreenYellow;
			this.kod_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.kod_t.ForeColor = System.Drawing.Color.Black;
			this.kod_t.Location = new System.Drawing.Point(16, 24);
			this.kod_t.Size = new System.Drawing.Size(208, 27);
			this.kod_t.Text = "";
			this.kod_t.GotFocus += new System.EventHandler(this.kod_t_GotFocus);
			this.kod_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.kod_t_KeyPress);
			// 
			// search_b
			// 
			this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.search_b.Location = new System.Drawing.Point(152, 56);
			this.search_b.Text = "SZUKAJ";
			this.search_b.Click += new System.EventHandler(this.search_b_Click);
			// 
			// kod_l
			// 
			this.kod_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.kod_l.ForeColor = System.Drawing.Color.White;
			this.kod_l.Location = new System.Drawing.Point(16, 8);
			this.kod_l.Size = new System.Drawing.Size(160, 16);
			this.kod_l.Text = "Wprowadü KOD Towaru";
			// 
			// cena_t
			// 
			this.cena_t.AcceptsReturn = true;
			this.cena_t.BackColor = System.Drawing.Color.Azure;
			this.cena_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.cena_t.ForeColor = System.Drawing.Color.Black;
			this.cena_t.Location = new System.Drawing.Point(56, 224);
			this.cena_t.ReadOnly = true;
			this.cena_t.Size = new System.Drawing.Size(72, 24);
			this.cena_t.Text = "";
			// 
			// cena_l
			// 
			this.cena_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.cena_l.ForeColor = System.Drawing.Color.White;
			this.cena_l.Location = new System.Drawing.Point(8, 216);
			this.cena_l.Size = new System.Drawing.Size(48, 32);
			this.cena_l.Text = "CenaZk (netto)";
			this.cena_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// stan_t
			// 
			this.stan_t.BackColor = System.Drawing.Color.Azure;
			this.stan_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.stan_t.ForeColor = System.Drawing.Color.Black;
			this.stan_t.Location = new System.Drawing.Point(56, 192);
			this.stan_t.ReadOnly = true;
			this.stan_t.Size = new System.Drawing.Size(72, 24);
			this.stan_t.Text = "";
			// 
			// stan_l
			// 
			this.stan_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.stan_l.ForeColor = System.Drawing.Color.White;
			this.stan_l.Location = new System.Drawing.Point(24, 192);
			this.stan_l.Size = new System.Drawing.Size(32, 20);
			this.stan_l.Text = "Stan";
			this.stan_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// nazwa_l
			// 
			this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.nazwa_l.ForeColor = System.Drawing.Color.White;
			this.nazwa_l.Location = new System.Drawing.Point(16, 64);
			this.nazwa_l.Size = new System.Drawing.Size(48, 16);
			this.nazwa_l.Text = "Nazwa";
			// 
			// nazwa_t
			// 
			this.nazwa_t.BackColor = System.Drawing.Color.Azure;
			this.nazwa_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.nazwa_t.ForeColor = System.Drawing.Color.Black;
			this.nazwa_t.Location = new System.Drawing.Point(16, 80);
			this.nazwa_t.Multiline = true;
			this.nazwa_t.ReadOnly = true;
			this.nazwa_t.Size = new System.Drawing.Size(208, 40);
			this.nazwa_t.Text = "";
			this.nazwa_t.LostFocus += new System.EventHandler(this.nazwa_t_LostFocus);
			this.nazwa_t.GotFocus += new System.EventHandler(this.nazwa_t_GotFocus);
			// 
			// ilosc_t
			// 
			this.ilosc_t.BackColor = System.Drawing.Color.GreenYellow;
			this.ilosc_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.ilosc_t.ForeColor = System.Drawing.Color.Black;
			this.ilosc_t.Location = new System.Drawing.Point(56, 128);
			this.ilosc_t.Size = new System.Drawing.Size(72, 24);
			this.ilosc_t.Text = "";
			this.ilosc_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ilosc_t_KeyPress);
			// 
			// ilosc_l
			// 
			this.ilosc_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.ilosc_l.ForeColor = System.Drawing.Color.White;
			this.ilosc_l.Location = new System.Drawing.Point(24, 128);
			this.ilosc_l.Size = new System.Drawing.Size(32, 20);
			this.ilosc_l.Text = "IloúÊ";
			this.ilosc_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(128, 256);
			this.exit_b.Size = new System.Drawing.Size(96, 32);
			this.exit_b.Text = "WYJåCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// ok_b
			// 
			this.ok_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.ok_b.Location = new System.Drawing.Point(16, 256);
			this.ok_b.Size = new System.Drawing.Size(104, 32);
			this.ok_b.Text = "OK";
			this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
			// 
			// zliczono_t
			// 
			this.zliczono_t.BackColor = System.Drawing.Color.Azure;
			this.zliczono_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.zliczono_t.ForeColor = System.Drawing.Color.Black;
			this.zliczono_t.Location = new System.Drawing.Point(56, 160);
			this.zliczono_t.ReadOnly = true;
			this.zliczono_t.Size = new System.Drawing.Size(72, 24);
			this.zliczono_t.Text = "";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(8, 296);
			this.label1.Size = new System.Drawing.Size(224, 24);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(134, 128);
			this.button1.Size = new System.Drawing.Size(90, 27);
			this.button1.Text = "KLAWIATURA";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(144, 160);
			this.label3.Size = new System.Drawing.Size(72, 16);
			this.label3.Text = "VAT %";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// vat_t
			// 
			this.vat_t.BackColor = System.Drawing.Color.Azure;
			this.vat_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.vat_t.ForeColor = System.Drawing.Color.Black;
			this.vat_t.Location = new System.Drawing.Point(144, 176);
			this.vat_t.ReadOnly = true;
			this.vat_t.Size = new System.Drawing.Size(72, 24);
			this.vat_t.Text = "";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(128, 200);
			this.label2.Size = new System.Drawing.Size(104, 16);
			this.label2.Text = "CenaSp (brutto)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cenasp_t
			// 
			this.cenasp_t.BackColor = System.Drawing.Color.Azure;
			this.cenasp_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.cenasp_t.ForeColor = System.Drawing.Color.Black;
			this.cenasp_t.Location = new System.Drawing.Point(144, 216);
			this.cenasp_t.ReadOnly = true;
			this.cenasp_t.Size = new System.Drawing.Size(72, 24);
			this.cenasp_t.Text = "";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(0, 160);
			this.button2.Size = new System.Drawing.Size(56, 24);
			this.button2.Text = "Zliczono";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// list_b
			// 
			this.list_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.list_b.Location = new System.Drawing.Point(72, 56);
			this.list_b.Text = "Z LISTY";
			this.list_b.Click += new System.EventHandler(this.list_b_Click);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form2
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.list_b);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.vat_t);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cenasp_t);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.zliczono_t);
			this.Controls.Add(this.ok_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.ilosc_l);
			this.Controls.Add(this.ilosc_t);
			this.Controls.Add(this.nazwa_t);
			this.Controls.Add(this.nazwa_l);
			this.Controls.Add(this.stan_l);
			this.Controls.Add(this.stan_t);
			this.Controls.Add(this.cena_l);
			this.Controls.Add(this.cena_t);
			this.Controls.Add(this.search_b);
			this.Controls.Add(this.kod_t);
			this.Controls.Add(this.kod_l);
			this.Text = "Wprowadzanie Pozycji";

		}
		#endregion

		private void search_b_Click(object sender, System.EventArgs e)
		{
			ilosc_t.Focus();
			FindIndex();
		}

		private void kod_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				ilosc_t.Focus();
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
					ilosc_t.Focus();
					
					WriteLine();
					
					nazwa_t.ReadOnly = true;
					cena_t.ReadOnly = true;
					cenasp_t.ReadOnly = true;
					vat_t.ReadOnly = true;
				
				}
				else if (ilosc_t.Text == "")
				{
					MessageBox.Show("Wprowadü iloúÊ");
				}
				else if (kod_t.Text == "")
				{
					MessageBox.Show("Wprowadü kod towaru");
				}		
				
			}
		}

		private void ok_b_Click(object sender, System.EventArgs e)
		{
			if (kod_t.Text != "" && ilosc_t.Text != "")
			{
				WriteLine();
				nazwa_t.ReadOnly = true;
				cena_t.ReadOnly = true;
				cenasp_t.ReadOnly = true;
				vat_t.ReadOnly = true;
		
			}
			else if (kod_t.Text == "")
			{
				MessageBox.Show("Wprowadü kod towaru");
			}
			else if (ilosc_t.Text == "")
			{
				MessageBox.Show("Wprowadü iloúÊ");
			}


		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = false;
			cn.Close();
			Form3 frm3 = new Form3(rownum, lic);
			frm3.Show();
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
			if (e.KeyChar == 13)
			{
				ilosc_t.Focus();
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

		private void button2_Click(object sender, System.EventArgs e)
		{
				string zliczono = "0";
				
				SqlCeCommand cmd = cn.CreateCommand();
				
				cmd.CommandText = "SELECT kod, dokid, ilosc FROM bufor WHERE kod = ? and dokid = ?";
				cmd.Parameters.Add("@k", SqlDbType.NVarChar, 15);	
				cmd.Parameters.Add("@d", SqlDbType.Int, 10);	
				cmd.Parameters["@k"].Value = kodbuf;
				cmd.Parameters["@d"].Value = int.Parse(index);
				cmd.Prepare();
				SqlCeDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					zliczono = ((decimal.Parse(zliczono) + dr.GetSqlDecimal(2)).ToString());;
				}
				zliczono_t.Text = zliczono;
		}

		private void list_b_Click(object sender, System.EventArgs e)
		{
			cn.Close();
			this.Close();
			Form2_1_1 frm2_1_1 = new Form2_1_1(rownum, lic);
			frm2_1_1.Show();
		}

		

		

		

		

			

	

		

		
		

	}
}
