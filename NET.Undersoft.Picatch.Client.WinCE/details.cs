using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using Microsoft.WindowsCE.Forms;

namespace Undersoft.Picatch
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class details : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox kod_t;
		private System.Windows.Forms.Label kod_l;
		private System.Windows.Forms.TextBox cena_t;
		private System.Windows.Forms.Label cena_l;
		private System.Windows.Forms.TextBox stan_t;
		private System.Windows.Forms.Label nazwa_l;
		private System.Windows.Forms.TextBox nazwa_t;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Label stan_l;
		
		private string connectionString;
		private SqlCeConnection cn;
		private string index;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox vat_t;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox cenasp_t;
		string kodzik;
		string nazwik;
		string cenik;
		string ceniksp;
		string vacik;
		public System.Windows.Forms.DataGrid dataGrid1;
		private System.Windows.Forms.Label label4;
		string stanik;
		



		public details(string kod, string nazwa, string cena, string cenasp, string vat, string stan)
		{
			//
			// Required for Windows Form Designer support
			//
			
		
			
			
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			kod_t.Text = kod;
			nazwa_t.Text = nazwa;
			stan_t.Text = stan;
			cena_t.Text = cena;
			cenasp_t.Text = cenasp;
			vat_t.Text = vat;
			kod_t.ReadOnly = true;
			nazwa_t.ReadOnly = true;
			stan_t.ReadOnly = true;
			cena_t.ReadOnly = true;
			cenasp_t.ReadOnly = true;
			vat_t.ReadOnly = true;

			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			
			SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT Nazwa, stan FROM stany", cn);
			DataTable table2 = new DataTable();
			db.SelectCommand = new SqlCeCommand("SELECT Nazwa, stan FROM stany where kod = ?", cn);
			db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 15);	
			db.SelectCommand.Parameters["@k"].Value = kod;
			db.SelectCommand.ExecuteNonQuery();
			db.Fill(table2);
		
			dataGrid1.DataSource = table2.DefaultView;
		
			
			cn.Close();
			exit_b.Focus();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

	
	#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.kod_t = new System.Windows.Forms.TextBox();
			this.kod_l = new System.Windows.Forms.Label();
			this.cena_t = new System.Windows.Forms.TextBox();
			this.cena_l = new System.Windows.Forms.Label();
			this.stan_t = new System.Windows.Forms.TextBox();
			this.stan_l = new System.Windows.Forms.Label();
			this.nazwa_l = new System.Windows.Forms.Label();
			this.nazwa_t = new System.Windows.Forms.TextBox();
			this.exit_b = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.label3 = new System.Windows.Forms.Label();
			this.vat_t = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cenasp_t = new System.Windows.Forms.TextBox();
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.label4 = new System.Windows.Forms.Label();
			// 
			// kod_t
			// 
			this.kod_t.BackColor = System.Drawing.Color.Azure;
			this.kod_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9.75F, System.Drawing.FontStyle.Bold);
			this.kod_t.ForeColor = System.Drawing.Color.Black;
			this.kod_t.Location = new System.Drawing.Point(16, 20);
			this.kod_t.Size = new System.Drawing.Size(208, 27);
			this.kod_t.Text = "";
			// 
			// kod_l
			// 
			this.kod_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.kod_l.ForeColor = System.Drawing.Color.White;
			this.kod_l.Location = new System.Drawing.Point(16, 5);
			this.kod_l.Size = new System.Drawing.Size(160, 16);
			this.kod_l.Text = "KOD Towaru";
			// 
			// cena_t
			// 
			this.cena_t.AcceptsReturn = true;
			this.cena_t.BackColor = System.Drawing.Color.Azure;
			this.cena_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.cena_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.cena_t.Location = new System.Drawing.Point(48, 112);
			this.cena_t.ReadOnly = true;
			this.cena_t.Size = new System.Drawing.Size(64, 24);
			this.cena_t.Text = "";
			// 
			// cena_l
			// 
			this.cena_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.cena_l.ForeColor = System.Drawing.Color.White;
			this.cena_l.Location = new System.Drawing.Point(0, 112);
			this.cena_l.Size = new System.Drawing.Size(48, 24);
			this.cena_l.Text = "CenaZk";
			this.cena_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// stan_t
			// 
			this.stan_t.BackColor = System.Drawing.Color.Azure;
			this.stan_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.stan_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.stan_t.Location = new System.Drawing.Point(160, 112);
			this.stan_t.ReadOnly = true;
			this.stan_t.Size = new System.Drawing.Size(64, 24);
			this.stan_t.Text = "";
			// 
			// stan_l
			// 
			this.stan_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.stan_l.ForeColor = System.Drawing.Color.White;
			this.stan_l.Location = new System.Drawing.Point(128, 112);
			this.stan_l.Size = new System.Drawing.Size(32, 16);
			this.stan_l.Text = "Stan";
			this.stan_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nazwa_l
			// 
			this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.nazwa_l.ForeColor = System.Drawing.Color.White;
			this.nazwa_l.Location = new System.Drawing.Point(16, 48);
			this.nazwa_l.Size = new System.Drawing.Size(104, 16);
			this.nazwa_l.Text = "Nazwa Towaru";
			// 
			// nazwa_t
			// 
			this.nazwa_t.BackColor = System.Drawing.Color.Azure;
			this.nazwa_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.nazwa_t.ForeColor = System.Drawing.Color.Black;
			this.nazwa_t.Location = new System.Drawing.Point(16, 64);
			this.nazwa_t.Multiline = true;
			this.nazwa_t.ReadOnly = true;
			this.nazwa_t.Size = new System.Drawing.Size(208, 40);
			this.nazwa_t.Text = "";
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(120, 272);
			this.exit_b.Size = new System.Drawing.Size(96, 24);
			this.exit_b.Text = "WYJŒCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(8, 296);
			this.label1.Size = new System.Drawing.Size(224, 16);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(120, 144);
			this.label3.Size = new System.Drawing.Size(40, 16);
			this.label3.Text = "VAT %";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// vat_t
			// 
			this.vat_t.BackColor = System.Drawing.Color.Azure;
			this.vat_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.vat_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.vat_t.Location = new System.Drawing.Point(160, 144);
			this.vat_t.Size = new System.Drawing.Size(64, 24);
			this.vat_t.Text = "";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(0, 144);
			this.label2.Size = new System.Drawing.Size(48, 16);
			this.label2.Text = "CenaSp";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cenasp_t
			// 
			this.cenasp_t.BackColor = System.Drawing.Color.Azure;
			this.cenasp_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.cenasp_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.cenasp_t.Location = new System.Drawing.Point(48, 144);
			this.cenasp_t.Size = new System.Drawing.Size(64, 24);
			this.cenasp_t.Text = "";
			// 
			// dataGrid1
			// 
			this.dataGrid1.BackColor = System.Drawing.Color.Azure;
			this.dataGrid1.ForeColor = System.Drawing.Color.Black;
			this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
			this.dataGrid1.HeaderBackColor = System.Drawing.Color.DarkViolet;
			this.dataGrid1.HeaderForeColor = System.Drawing.Color.White;
			this.dataGrid1.Location = new System.Drawing.Point(48, 176);
			this.dataGrid1.Size = new System.Drawing.Size(176, 88);
			this.dataGrid1.Text = "dataGrid1";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(-8, 176);
			this.label4.Size = new System.Drawing.Size(56, 64);
			this.label4.Text = "Stan wg mag";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// details
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.dataGrid1);
			this.Controls.Add(this.cena_t);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.vat_t);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cenasp_t);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.nazwa_t);
			this.Controls.Add(this.nazwa_l);
			this.Controls.Add(this.stan_l);
			this.Controls.Add(this.stan_t);
			this.Controls.Add(this.cena_l);
			this.Controls.Add(this.kod_l);
			this.Controls.Add(this.kod_t);
			this.Controls.Add(this.label4);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Szczegó³y Towaru";

		}
		#endregion

		
		private void exit_b_Click(object sender, System.EventArgs e)
		{
			
			
			this.Close();

		}

		
		

		

	

	
		

		

	

		

		
		

	}
}
