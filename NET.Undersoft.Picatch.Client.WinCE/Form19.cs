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
	public class Form19 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label nazwa_l;
		private System.Windows.Forms.TextBox nazwa_t;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button ok_b;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox data_t;
		private System.Windows.Forms.Label typ_l;
		private System.Windows.Forms.Label data_l;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.Label label4;
		private string index;
		private int editflag2;
		private SqlCeDataAdapter da;
		public Form19(int rownumber, int editflag)
		{
			//
			// Required for Windows Form Designer support
			//
			editflag2 = editflag;
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			DataTable table = new DataTable();
			if (editflag == 0)
			{
			
				da = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 0", cn);
				
				da.Fill(table);
			}
			else if (editflag == 1)
			{
				da = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 1", cn);
				
				da.Fill(table);
			}
				index = table.Rows[rownumber][0].ToString();
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "SELECT NrDok, TypDok, Data, NazwaOdbiorcy,  UlicaOdbiorcy, MiastoOdbiorcy, DataRealizacji, complete  FROM edihead WHERE id = ?";
				cmd.Parameters.Add("@k", SqlDbType.Int);	
				cmd.Parameters["@k"].Value = int.Parse(index);
				cmd.Prepare();
				SqlCeDataReader dr = cmd.ExecuteReader();
			
				while (dr.Read())
				{
					nazwa_t.Text = dr.GetString(3) +" "+ dr.GetString(4)+" "+dr.GetString(5);
					comboBox1.Text = dr.GetString(1);
					data_t.Text = dr.GetString(2);
					textBox2.Text = dr.GetString(6);
					checkBox1.Checked = dr.GetBoolean(7);
					textBox1.Text = dr.GetString(0);
				}
				nazwa_t.Focus();
			
			
			//
			// TODO: Add any constructor code after InitializeComponent call
		cn.Close();
			//
		}
	

		
		
		
		public void WriteLine ()
		{
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			DataTable table = new DataTable();
			if (editflag2 == 0)
			{
				da = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 0", cn);
				
				da.Fill(table);
			}
			else if (editflag2 == 1)
			{
				da = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 1", cn);
				
				da.Fill(table);
			}
				da.UpdateCommand = new SqlCeCommand("UPDATE edihead SET NrDok = ?, TypDok = ?, Data = ?, complete = ? WHERE id =  ?", cn);
				da.UpdateCommand.Parameters.Add("@k", SqlDbType.NVarChar, 120);	
				da.UpdateCommand.Parameters.Add("@n", SqlDbType.NVarChar, 20);	
				da.UpdateCommand.Parameters.Add("@cz", SqlDbType.NVarChar, 30);
				da.UpdateCommand.Parameters.Add("@compl", SqlDbType.Bit);
				da.UpdateCommand.Parameters.Add("@index", SqlDbType.Int);
				da.UpdateCommand.Parameters["@k"].Value = textBox1.Text;
				da.UpdateCommand.Parameters["@n"].Value = comboBox1.Text;
				da.UpdateCommand.Parameters["@cz"].Value = data_t.Text;
				da.UpdateCommand.Parameters["@compl"].Value = checkBox1.CheckState;	
				da.UpdateCommand.Parameters["@index"].Value = int.Parse(index);
				
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(Form19));
			this.typ_l = new System.Windows.Forms.Label();
			this.data_t = new System.Windows.Forms.TextBox();
			this.data_l = new System.Windows.Forms.Label();
			this.nazwa_l = new System.Windows.Forms.Label();
			this.nazwa_t = new System.Windows.Forms.TextBox();
			this.exit_b = new System.Windows.Forms.Button();
			this.ok_b = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			// 
			// typ_l
			// 
			this.typ_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.typ_l.ForeColor = System.Drawing.Color.White;
			this.typ_l.Location = new System.Drawing.Point(16, 120);
			this.typ_l.Size = new System.Drawing.Size(32, 16);
			this.typ_l.Text = "Typ";
			// 
			// data_t
			// 
			this.data_t.BackColor = System.Drawing.Color.WhiteSmoke;
			this.data_t.ForeColor = System.Drawing.Color.Black;
			this.data_t.Location = new System.Drawing.Point(120, 136);
			this.data_t.Size = new System.Drawing.Size(104, 20);
			this.data_t.Text = "";
			// 
			// data_l
			// 
			this.data_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.data_l.ForeColor = System.Drawing.Color.White;
			this.data_l.Location = new System.Drawing.Point(120, 120);
			this.data_l.Size = new System.Drawing.Size(48, 16);
			this.data_l.Text = "Data";
			// 
			// nazwa_l
			// 
			this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.nazwa_l.ForeColor = System.Drawing.Color.White;
			this.nazwa_l.Location = new System.Drawing.Point(16, 32);
			this.nazwa_l.Size = new System.Drawing.Size(128, 16);
			this.nazwa_l.Text = "Dane Odbiorcy";
			// 
			// nazwa_t
			// 
			this.nazwa_t.BackColor = System.Drawing.Color.WhiteSmoke;
			this.nazwa_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.nazwa_t.ForeColor = System.Drawing.Color.Black;
			this.nazwa_t.Location = new System.Drawing.Point(16, 48);
			this.nazwa_t.Size = new System.Drawing.Size(208, 24);
			this.nazwa_t.Text = "";
			this.nazwa_t.LostFocus += new System.EventHandler(this.nazwa_t_LostFocus);
			this.nazwa_t.GotFocus += new System.EventHandler(this.nazwa_t_GotFocus);
			this.nazwa_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nazwa_t_KeyPress);
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(128, 216);
			this.exit_b.Size = new System.Drawing.Size(96, 32);
			this.exit_b.Text = "WYJŒCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// ok_b
			// 
			this.ok_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.ok_b.Location = new System.Drawing.Point(16, 216);
			this.ok_b.Size = new System.Drawing.Size(104, 32);
			this.ok_b.Text = "OK";
			this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8F, System.Drawing.FontStyle.Regular);
			this.comboBox1.ForeColor = System.Drawing.Color.MidnightBlue;
			this.comboBox1.Items.Add("ZAM_ODB");
			this.comboBox1.Items.Add("ZAM_DOST");
			this.comboBox1.Items.Add("WZ");
			this.comboBox1.Items.Add("PZ");
			this.comboBox1.Items.Add("MW");
			this.comboBox1.Items.Add("MP");
			this.comboBox1.Location = new System.Drawing.Point(16, 136);
			this.comboBox1.Size = new System.Drawing.Size(100, 23);
			this.comboBox1.GotFocus += new System.EventHandler(this.comboBox1_GotFocus);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(8, 264);
			this.label1.Size = new System.Drawing.Size(224, 24);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(128, 8);
			this.button1.Size = new System.Drawing.Size(96, 24);
			this.button1.Text = "KLAWIATURA";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// textBox1
			// 
			this.textBox1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.textBox1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
			this.textBox1.ForeColor = System.Drawing.Color.Black;
			this.textBox1.Location = new System.Drawing.Point(16, 96);
			this.textBox1.Size = new System.Drawing.Size(208, 26);
			this.textBox1.Text = "";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(16, 80);
			this.label2.Size = new System.Drawing.Size(136, 16);
			this.label2.Text = "Numer Dokumentu";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.White;
			this.label3.Location = new System.Drawing.Point(16, 160);
			this.label3.Size = new System.Drawing.Size(96, 16);
			this.label3.Text = "Data Realizacji";
			// 
			// textBox2
			// 
			this.textBox2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.textBox2.ForeColor = System.Drawing.Color.Black;
			this.textBox2.Location = new System.Drawing.Point(16, 176);
			this.textBox2.Size = new System.Drawing.Size(104, 20);
			this.textBox2.Text = "";
			// 
			// checkBox1
			// 
			this.checkBox1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.checkBox1.Location = new System.Drawing.Point(168, 176);
			this.checkBox1.Size = new System.Drawing.Size(16, 24);
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.White;
			this.label4.Location = new System.Drawing.Point(136, 160);
			this.label4.Size = new System.Drawing.Size(72, 16);
			this.label4.Text = "Kompletny";
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form19
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(234, 294);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.ok_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.nazwa_t);
			this.Controls.Add(this.nazwa_l);
			this.Controls.Add(this.data_l);
			this.Controls.Add(this.data_t);
			this.Controls.Add(this.typ_l);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Text = "Edytuj Dane Kompletacji";
			this.Load += new System.EventHandler(this.Form19_Load);

		}
		#endregion

		

		private void ok_b_Click(object sender, System.EventArgs e)
		{
			if (nazwa_t.Text != "" && comboBox1.Text != "")
			{
				
				WriteLine();
				Form15 fr15 = new Form15();
				fr15.Show();
				
			}
			else if (nazwa_t.Text == "")
			{
				MessageBox.Show("WprowadŸ nazwê dokumentu");
			}
			else if (comboBox1.Text == "")
			{
				MessageBox.Show("WprowadŸ typ dokumentu");
			}
		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = false;
			this.Close();
			Form15 fr15 = new Form15();
			fr15.Show();
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

		private void nazwa_t_GotFocus(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = true;
		}

		private void nazwa_t_LostFocus(object sender, System.EventArgs e)
		{
			if (button1.Focus() != true)
			{
				inputPanel1.Enabled = false;
			}
		}

		private void nazwa_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				comboBox1.Focus();
			}
		}

		private void comboBox1_GotFocus(object sender, System.EventArgs e)
		{
		inputPanel1.Enabled = false;
		}

		private void Form19_Load(object sender, System.EventArgs e)
		{
		
		}

	

		

		
	}
}
