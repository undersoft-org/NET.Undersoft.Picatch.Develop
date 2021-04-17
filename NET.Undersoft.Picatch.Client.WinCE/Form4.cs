using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Data.SqlClient;


namespace Undersoft.Picatch
{
	/// <summary>
	/// Summary description for Form4.
	/// </summary>
	public class Form4 : System.Windows.Forms.Form
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
		private System.Windows.Forms.Label zliczono_l;
		private System.Windows.Forms.TextBox zliczono_t;
		private System.Windows.Forms.Button ok_b;
		private string index;
		private string index2;
		private int rownum;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox cenasp_t;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox vat_t;
		private int rownumb;
		private int lic;
		public Form4(int rownumber, int dokrow, int licence)
		{
			//
			// Required for Windows Form Designer support
			//
			lic = licence;
			rownum = dokrow;
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			rownumb = rownumber;
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			
			SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
			DataTable table = new DataTable();
			da.Fill(table);
			index = table.Rows[rownum][0].ToString();
			SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
			DataTable table2 = new DataTable();
			db.SelectCommand = new SqlCeCommand("SELECT * FROM bufor WHERE dokid =  ?", cn);
			db.SelectCommand.Parameters.Add("@k", SqlDbType.Int, 10);	
			db.SelectCommand.Parameters["@k"].Value = int.Parse(index);
			db.SelectCommand.ExecuteNonQuery();
			db.Fill(table2);
			index2 = table2.Rows[rownumb][0].ToString();
			SqlCeCommand cmd = cn.CreateCommand();
			cmd.CommandText = "SELECT kod, nazwa, cenazk, ilosc, stan, cenasp, vat FROM bufor WHERE id = ?";
			cmd.Parameters.Add("@k", SqlDbType.Int, 10);
			cmd.Parameters["@k"].Value = int.Parse(index2);
			cmd.Prepare();
			SqlCeDataReader dr = cmd.ExecuteReader();
			
			while (dr.Read())
			{
				kod_t.Text = dr.GetString(0);
				nazwa_t.Text = dr.GetString(1);
				cena_t.Text = dr.GetString(2);
				ilosc_t.Text = Convert.ToString(dr.GetSqlDecimal(3));
				stan_t.Text = dr.GetString(4);
				cenasp_t.Text = dr.GetString(5);
				vat_t.Text = dr.GetString(6);
			}
			
			
			string index3 = table2.Rows[rownumb][2].ToString();
			SqlCeCommand cmd1 = cn.CreateCommand();
			cmd1.CommandText = "SELECT kod, dokid, SUM(ilosc) AS zliczono FROM bufor GROUP BY kod, dokid HAVING kod = ? and dokid = ?";
			cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 15);	
			cmd1.Parameters.Add("@d", SqlDbType.Int, 10);
			cmd1.Parameters["@k"].Value = index3;
			cmd1.Parameters["@d"].Value = int.Parse(index);
			cmd1.Prepare();
			dr = cmd1.ExecuteReader();
			
			while (dr.Read())
			{
				zliczono_t.Text = Convert.ToString(dr.GetSqlDecimal(2));
			}


			ilosc_t.Focus();
			ilosc_t.SelectAll();
			cn.Close();
		
							
				

			
			
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		
		
		
		private void WriteLine ()
		{
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			SqlCeCommand cmd = cn.CreateCommand();
			cn.Open();
			SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
			DataTable table = new DataTable();
			da.Fill(table);
			da.UpdateCommand = new SqlCeCommand("UPDATE bufor SET kod = ?, nazwa = ?, cenazk = ?, ilosc = ?, dokid = ?, cenasp = ?, vat = ? WHERE id =  ?", cn);
			da.UpdateCommand.Parameters.Add("@k", SqlDbType.NVarChar, 15);	
			da.UpdateCommand.Parameters.Add("@n", SqlDbType.NVarChar, 100);	
			da.UpdateCommand.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
			da.UpdateCommand.Parameters.Add("@i", SqlDbType.Decimal, 10);
			da.UpdateCommand.Parameters.Add("@d", SqlDbType.Int, 10);
			da.UpdateCommand.Parameters["@i"].Precision = 10;
			da.UpdateCommand.Parameters["@i"].Scale = 3;
			da.UpdateCommand.Parameters.Add("@csp", SqlDbType.NVarChar, 10);
			da.UpdateCommand.Parameters.Add("@v", SqlDbType.NVarChar, 10);
			da.UpdateCommand.Parameters.Add("@index", SqlDbType.Int, 10);
			da.UpdateCommand.Parameters["@k"].Value = kod_t.Text;
			da.UpdateCommand.Parameters["@n"].Value = nazwa_t.Text;
			da.UpdateCommand.Parameters["@cz"].Value = cena_t.Text;
			da.UpdateCommand.Parameters["@i"].Value = ilosc_t.Text;
			da.UpdateCommand.Parameters["@d"].Value = int.Parse(index);
			da.UpdateCommand.Parameters["@csp"].Value = cenasp_t.Text;
			da.UpdateCommand.Parameters["@v"].Value = vat_t.Text;
			da.UpdateCommand.Parameters["@index"].Value = int.Parse(index2);
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
			this.zliczono_l = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.cenasp_t = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.vat_t = new System.Windows.Forms.TextBox();
			// 
			// kod_t
			// 
			this.kod_t.BackColor = System.Drawing.Color.WhiteSmoke;
			this.kod_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.kod_t.ForeColor = System.Drawing.Color.Black;
			this.kod_t.Location = new System.Drawing.Point(16, 24);
			this.kod_t.ReadOnly = true;
			this.kod_t.Size = new System.Drawing.Size(208, 27);
			this.kod_t.Text = "";
			// 
			// search_b
			// 
			this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.search_b.Location = new System.Drawing.Point(152, 56);
			this.search_b.Text = "SZUKAJ";
			this.search_b.Visible = false;
			// 
			// kod_l
			// 
			this.kod_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.kod_l.ForeColor = System.Drawing.Color.White;
			this.kod_l.Location = new System.Drawing.Point(16, 8);
			this.kod_l.Size = new System.Drawing.Size(160, 16);
			this.kod_l.Text = "KOD Towaru";
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
			this.cena_l.Size = new System.Drawing.Size(48, 31);
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
			this.nazwa_l.Size = new System.Drawing.Size(104, 16);
			this.nazwa_l.Text = "Nazwa Towaru";
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
			// zliczono_l
			// 
			this.zliczono_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.zliczono_l.ForeColor = System.Drawing.Color.White;
			this.zliczono_l.Location = new System.Drawing.Point(0, 160);
			this.zliczono_l.Size = new System.Drawing.Size(56, 20);
			this.zliczono_l.Text = "Zliczono";
			this.zliczono_l.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(136, 200);
			this.label2.Size = new System.Drawing.Size(96, 16);
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
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form4
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.label3);
			this.Controls.Add(this.vat_t);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cenasp_t);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.zliczono_l);
			this.Controls.Add(this.zliczono_t);
			this.Controls.Add(this.ok_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.ilosc_l);
			this.Controls.Add(this.ilosc_t);
			this.Controls.Add(this.nazwa_t);
			this.Controls.Add(this.stan_l);
			this.Controls.Add(this.stan_t);
			this.Controls.Add(this.cena_l);
			this.Controls.Add(this.cena_t);
			this.Controls.Add(this.search_b);
			this.Controls.Add(this.kod_t);
			this.Controls.Add(this.kod_l);
			this.Controls.Add(this.nazwa_l);
			this.Text = "Edycja Pozycji";

		}
		#endregion

		private void ilosc_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) 
				&& !char.IsDigit(e.KeyChar) 
				&& e.KeyChar != '.')
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
				
					WriteLine();
					Form3 fr3 = new Form3(rownum, lic);
					fr3.Refresh();
				
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
			if (ilosc_t.Text != "" && kod_t.Text != "")
			{
				
				WriteLine();
				Form3 fr3 = new Form3(rownum, lic);
				fr3.Show();
				
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

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Form3 fr3 = new Form3(rownum, lic);
			fr3.Show();
			inputPanel1.Enabled = false;
		}

		private void nazwa_t_GotFocus(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = true;
		}

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
				inputPanel1.Enabled = true;
			}
		}

	}
}
