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
	public class Form7_1_4 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label kod_l;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button search_b;
		private System.Windows.Forms.Button ok_b;
		
		private string connectionString;
		private SqlCeConnection cn;
		private string index;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private int rownum;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		public System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox search_t;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Label label7;
		private int lic;
		public string kod;
		public string nazwa;
		public string cenazk;
		public string cenasp;
		public string dokstan;
		public string vat;
		public bool dbflag;
		public string cenah;
		public string cenao;
		public int type;

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

		public Form7_1_4(int licence, int typ)
		{
			//
			// Required for Windows Form Designer support
			//
			lic = licence;
			type = typ;
			
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			cn = new SqlCeConnection(connectionString);
			cn.Open();
			

			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			search_t.Focus();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		
		private void FindIndex()
		{
			
			string kodbuf = search_t.Text;
			search_t.Text = "SZUKAM TOWARÓW W BAZIE";
			search_t.Refresh();
			
			if (kodbuf != "" && comboBox1.Text != "" && comboBox1.Text != null)
			{

                
                
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "SELECT     dane.kod, dane.nazwa, stany.stan, magazyn.Nazwa AS magazyn, dane.cenasp, dane.stan as calystan, dane.vat, dane.cenazk, dane.cenahurt, dane.cenaoryg FROM magazyn INNER JOIN stany ON magazyn.MagId = stany.MagId INNER JOIN dane ON stany.kod = dane.kod where (magazyn.Nazwa = ?) AND (dane.kod = ?)";
				cmd.Parameters.Add("@m", SqlDbType.NVarChar, 20);	
				cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);
				cmd.Prepare();
				cmd.Parameters["@m"].Value = comboBox1.Text;
				cmd.Parameters["@k"].Value = kodbuf;

				int test = 0;

              
				SqlCeDataReader dr = cmd.ExecuteReader();
				while (dr.Read())
				{
					if (!dr.IsDBNull(0))
					{
						System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
						nfi.NumberDecimalSeparator = ".";
						test = 1;
						string stan = dr.GetString(2);
						string calystan = dr.GetString(5);
						dokstan = dr.GetString(5);
						cenasp = dr.GetString(4);
						nazwa =  dr.GetString(1);
						kod =  dr.GetString(0);
						cenazk =  dr.GetString(7);
						vat = dr.GetString(6);
						cenah = dr.GetString(8);
						cenao = dr.GetString(9);

						if (type == 0)
						{
							label2.Text = kod;
							label3.Text = nazwa;
							label4.Text = "Detal(brutto): " + cenasp;
							label5.Text = "Stan: " + stan;
						}
						else if (type == 1)
						{
							
							label2.Text =kod;
							label3.Text = nazwa;
							label4.Text = "Hurt(net) " + cenah + "Original(brut) " + cenao;
							label5.Text = "Stan: " + stan;
						}
						if (Convert.ToDecimal(stan) > 0)
						{
							this.BackColor = Color.Green;
                           
							label5.Text += " - OK !!!!!";
						}
						else if (Convert.ToDecimal(calystan) > 0)
						{
							this.BackColor = Color.MidnightBlue;
							label2.Text += "\n TOWAR JEST NA INNYM MAGAZYNIE";
						}
						else
						{
							this.BackColor = Color.DarkRed;
							label2.Text += "\n BRAK TOWARU";
						}




					
						SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT    stany.kod, stany.stan, magazyn.Nazwa AS magazyn FROM magazyn INNER JOIN stany ON magazyn.MagId = stany.MagId", cn);
						DataTable table2 = new DataTable();
						db.SelectCommand = new SqlCeCommand("SELECT     magazyn.Nazwa AS Magazyn, stany.stan As Stan FROM magazyn INNER JOIN stany ON magazyn.MagId = stany.MagId where (stany.kod = ?)", cn);
						db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 15);
						db.SelectCommand.Parameters["@k"].Value = kodbuf;
						db.SelectCommand.ExecuteNonQuery();
						db.Fill(table2);


						if (table2.Rows.Count != 0)
						{
							dataGrid1.DataSource = table2.DefaultView;

						}
					}


                   
				}
				dr.Close();

				if (test == 0)
				{
					label2.Text = "NIE ZNALEZIONO TOWARU O PODANYM KODZIE !!!!";
					this.BackColor = Color.DarkOrange;
					search_t.Text = null;
					label3.Text = null;				
					label4.Text = null;
					label5.Text = null;
					

				}
			}
			else
			{
				MessageBox.Show("Nie wybra³eœ magazynu lub nie poda³eœ kodu kreskowego towaru");
			}
			search_t.Text = "";
			search_t.Focus();
			dataGrid1.Refresh();

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
			this.search_t = new System.Windows.Forms.TextBox();
			this.search_b = new System.Windows.Forms.Button();
			this.kod_l = new System.Windows.Forms.Label();
			this.exit_b = new System.Windows.Forms.Button();
			this.ok_b = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.label6 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			// 
			// search_t
			// 
			this.search_t.BackColor = System.Drawing.Color.GreenYellow;
			this.search_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.search_t.ForeColor = System.Drawing.Color.Black;
			this.search_t.Location = new System.Drawing.Point(36, 36);
			this.search_t.Size = new System.Drawing.Size(140, 27);
			this.search_t.Text = "";
			this.search_t.GotFocus += new System.EventHandler(this.search_t_GotFocus);
			this.search_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.search_t_KeyPress);
			// 
			// search_b
			// 
			this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.search_b.Location = new System.Drawing.Point(176, 36);
			this.search_b.Size = new System.Drawing.Size(56, 28);
			this.search_b.Text = "SZUKAJ";
			this.search_b.Click += new System.EventHandler(this.search_b_Click);
			// 
			// kod_l
			// 
			this.kod_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.kod_l.ForeColor = System.Drawing.Color.White;
			this.kod_l.Location = new System.Drawing.Point(4, 40);
			this.kod_l.Size = new System.Drawing.Size(32, 16);
			this.kod_l.Text = "KOD";
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(124, 264);
			this.exit_b.Size = new System.Drawing.Size(104, 32);
			this.exit_b.Text = "WYJŒCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// ok_b
			// 
			this.ok_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.ok_b.Location = new System.Drawing.Point(12, 264);
			this.ok_b.Size = new System.Drawing.Size(104, 32);
			this.ok_b.Text = "DALEJ";
			this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(8, 300);
			this.label1.Size = new System.Drawing.Size(224, 20);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(4, 4);
			this.button1.Size = new System.Drawing.Size(72, 24);
			this.button1.Text = "KLAWIAT.";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(8, 72);
			this.label2.Size = new System.Drawing.Size(224, 28);
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(8, 104);
			this.label3.Size = new System.Drawing.Size(224, 28);
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(8, 136);
			this.label4.Size = new System.Drawing.Size(224, 20);
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = System.Drawing.Color.White;
			this.label5.Location = new System.Drawing.Point(8, 160);
			this.label5.Size = new System.Drawing.Size(224, 20);
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// dataGrid1
			// 
			this.dataGrid1.BackColor = System.Drawing.Color.Azure;
			this.dataGrid1.ForeColor = System.Drawing.Color.Black;
			this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
			this.dataGrid1.HeaderBackColor = System.Drawing.Color.DarkViolet;
			this.dataGrid1.HeaderForeColor = System.Drawing.Color.White;
			this.dataGrid1.Location = new System.Drawing.Point(56, 184);
			this.dataGrid1.Size = new System.Drawing.Size(128, 76);
			this.dataGrid1.Text = "dataGrid1";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label6.ForeColor = System.Drawing.Color.White;
			this.label6.Location = new System.Drawing.Point(4, 184);
			this.label6.Size = new System.Drawing.Size(48, 32);
			this.label6.Text = "Stan wg mag";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// comboBox1
			// 
			this.comboBox1.Items.Add("Magazyn");
			this.comboBox1.Items.Add("Sklep");
			this.comboBox1.Items.Add("Sklep internetowy");
			this.comboBox1.Location = new System.Drawing.Point(144, 4);
			this.comboBox1.Size = new System.Drawing.Size(88, 21);
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.label7.ForeColor = System.Drawing.Color.White;
			this.label7.Location = new System.Drawing.Point(84, 8);
			this.label7.Size = new System.Drawing.Size(56, 16);
			this.label7.Text = "Magazyn:";
			this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form7_1_4
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.label7);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.ok_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.kod_l);
			this.Controls.Add(this.search_b);
			this.Controls.Add(this.search_t);
			this.Text = "Wprowadzanie Pozycji";

		}
		#endregion

		private void search_b_Click(object sender, System.EventArgs e)
		{
			
			
			FindIndex();
		}

		private void search_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			
			
			if (e.KeyChar == 13)
			{
				
				FindIndex();
			}
		}


		private void ok_b_Click(object sender, System.EventArgs e)
		{
			if (search_t.Text != "")
			{
				
				
				
				search_t.Text = null;
				label2.Text = null;
				label3.Text = null;				
				label4.Text = null;
				label5.Text = null;
				search_t.Focus();
		
			}
			else if (search_t.Text == "")
			{
				MessageBox.Show("WprowadŸ kod towaru");
			}
			


		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = false;
			cn.Close();
			
			this.Close();

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
			
		}

		private void search_t_GotFocus(object sender, System.EventArgs e)
		{
			search_t.Text = "";
		}


		

		

		

		

		

			

	

		

		
		

	}
}
