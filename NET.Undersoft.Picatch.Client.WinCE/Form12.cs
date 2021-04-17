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
	/// Summary description for Form8.
	/// </summary>
	public class Form12 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button view_b;
		private System.Windows.Forms.Label label2;
		public System.Windows.Forms.DataGrid dataGrid1;
		private int lic;
		public Form12(int licence)
		{
			//
			// Required for Windows Form Designer support
			//
			lic = licence;
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			Loaddata();
			
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
	public void Loaddata ()
		{	
		string connectionString;
		connectionString = "DataSource=Baza.sdf; Password=matrix1";
		SqlCeConnection cn = new SqlCeConnection(connectionString);
		cn.Open();
		DataGridTableStyle ts = new DataGridTableStyle();
		SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
		DataTable table = new DataTable();
		da.Fill(table);
		dataGrid1.DataSource = table.DefaultView;
		
		dataGrid1.TableStyles.Add(ts);
		dataGrid1.TableStyles[0].GridColumnStyles[0].Width = 0;
		dataGrid1.TableStyles[0].GridColumnStyles[1].Width = 90;
		dataGrid1.TableStyles[0].GridColumnStyles[2].Width = 30;
		dataGrid1.TableStyles[0].GridColumnStyles[3].Width = 60;

		
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
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.exit_b = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.view_b = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			// 
			// dataGrid1
			// 
			this.dataGrid1.BackColor = System.Drawing.Color.Azure;
			this.dataGrid1.ForeColor = System.Drawing.Color.Black;
			this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
			this.dataGrid1.HeaderBackColor = System.Drawing.Color.DarkViolet;
			this.dataGrid1.HeaderForeColor = System.Drawing.Color.White;
			this.dataGrid1.Location = new System.Drawing.Point(5, 32);
			this.dataGrid1.Size = new System.Drawing.Size(225, 200);
			this.dataGrid1.Text = "dataGrid1";
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(160, 240);
			this.exit_b.Size = new System.Drawing.Size(70, 40);
			this.exit_b.Text = "WYJŒCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button3.Location = new System.Drawing.Point(8, 240);
			this.button3.Size = new System.Drawing.Size(70, 40);
			this.button3.Text = "WYŒLIJ";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Size = new System.Drawing.Size(208, 20);
			this.label1.Text = "Wybierz dokument do wys³ania";
			// 
			// view_b
			// 
			this.view_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.view_b.Location = new System.Drawing.Point(80, 240);
			this.view_b.Size = new System.Drawing.Size(77, 40);
			this.view_b.Text = "OTWÓRZ";
			this.view_b.Click += new System.EventHandler(this.view_b_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label2.Location = new System.Drawing.Point(8, 296);
			this.label2.Size = new System.Drawing.Size(224, 24);
			this.label2.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form12
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.label2);
			this.Controls.Add(this.view_b);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.button3);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.dataGrid1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Wysy³anie Dokumentu";

		}
		#endregion

		
		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		
		private void button3_Click(object sender, System.EventArgs e)
		{
			Form11 frm11 = new Form11(dataGrid1.CurrentCell.RowNumber, lic);
			frm11.Show();
			
		}

		private void view_b_Click(object sender, System.EventArgs e)
		{
			Form3 frm3 = new Form3(dataGrid1.CurrentCell.RowNumber, lic);
			frm3.Show();
			this.Close();
		}

		}
}
