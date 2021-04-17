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
	public class Form9 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label nazwa_l;
		private System.Windows.Forms.TextBox nazwa_t;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button ok_b;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.TextBox data_t;
		private System.Windows.Forms.Label typ_l;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label data_l;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox send_c;
		private int lic;
		public Form9(int licence)
		{
			//
			// Required for Windows Form Designer support
			//
			
			
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			nazwa_t.Focus();
			data_t.Text = System.DateTime.Now.ToString();
			lic = licence;
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		
		
		
		public void WriteLine ()
		{
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			string stop = "";
			if (lic == 0)
			{
				int demo = 0;
				SqlCeCommand licek = cn.CreateCommand();
				licek.CommandText = "Select Count(id) From dok";
				SqlCeDataReader dr = licek.ExecuteReader();
				while (dr.Read())
				{
					demo = dr.GetInt32(0);
				}
				if (demo >= 2)
				{
					stop = "stop";
				}
			}
			
			
			if (stop == "stop")
			{
		
				MessageBox.Show("Wersja Demo pozwala na wprowadzenie 2 dokument�w");
				
			}
			else
			{
				bool val;
				string input;
			
		

				input = bool.FalseString;
				val = bool.Parse(input);
				
				
				SqlCeCommand cmd = cn.CreateCommand();
				cmd.CommandText = "INSERT INTO dok (nazwadok, typ, data, send) VALUES (?, ?, ?, ?)";
				cmd.Parameters.Add("@d", SqlDbType.NVarChar, 120);
				cmd.Parameters.Add("@k", SqlDbType.NVarChar, 10);
				cmd.Parameters.Add("@n", SqlDbType.DateTime);	
				cmd.Parameters.Add("@f", SqlDbType.Bit);	
				cmd.Prepare();
				cmd.Parameters["@d"].Value = nazwa_t.Text;
				cmd.Parameters["@k"].Value = comboBox1.Text;
				cmd.Parameters["@n"].Value = Convert.ToDateTime(data_t.Text);
				cmd.Parameters["@f"].Value = val;
				cmd.ExecuteNonQuery();
				
			}
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
			this.typ_l = new System.Windows.Forms.Label();
			this.data_t = new System.Windows.Forms.TextBox();
			this.data_l = new System.Windows.Forms.Label();
			this.nazwa_l = new System.Windows.Forms.Label();
			this.nazwa_t = new System.Windows.Forms.TextBox();
			this.exit_b = new System.Windows.Forms.Button();
			this.ok_b = new System.Windows.Forms.Button();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.send_c = new System.Windows.Forms.CheckBox();
			// 
			// typ_l
			// 
			this.typ_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.typ_l.ForeColor = System.Drawing.Color.White;
			this.typ_l.Location = new System.Drawing.Point(16, 96);
			this.typ_l.Size = new System.Drawing.Size(32, 16);
			this.typ_l.Text = "Typ";
			// 
			// data_t
			// 
			this.data_t.BackColor = System.Drawing.Color.Azure;
			this.data_t.ForeColor = System.Drawing.Color.Black;
			this.data_t.Location = new System.Drawing.Point(16, 176);
			this.data_t.Size = new System.Drawing.Size(104, 26);
			this.data_t.Text = "";
			// 
			// data_l
			// 
			this.data_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.data_l.ForeColor = System.Drawing.Color.White;
			this.data_l.Location = new System.Drawing.Point(16, 152);
			this.data_l.Size = new System.Drawing.Size(48, 20);
			this.data_l.Text = "Data";
			// 
			// nazwa_l
			// 
			this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.nazwa_l.ForeColor = System.Drawing.Color.White;
			this.nazwa_l.Location = new System.Drawing.Point(16, 24);
			this.nazwa_l.Size = new System.Drawing.Size(128, 20);
			this.nazwa_l.Text = "Nazwa dokumentu";
			// 
			// nazwa_t
			// 
			this.nazwa_t.BackColor = System.Drawing.Color.Azure;
			this.nazwa_t.ForeColor = System.Drawing.Color.Black;
			this.nazwa_t.Location = new System.Drawing.Point(16, 56);
			this.nazwa_t.Size = new System.Drawing.Size(208, 26);
			this.nazwa_t.Text = "";
			this.nazwa_t.LostFocus += new System.EventHandler(this.nazwa_t_LostFocus);
			this.nazwa_t.GotFocus += new System.EventHandler(this.nazwa_t_GotFocus);
			this.nazwa_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nazwa_t_KeyPress);
			// 
			// exit_b
			// 
			this.exit_b.Location = new System.Drawing.Point(128, 216);
			this.exit_b.Size = new System.Drawing.Size(96, 32);
			this.exit_b.Text = "Wyj�cie";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// ok_b
			// 
			this.ok_b.Location = new System.Drawing.Point(16, 216);
			this.ok_b.Size = new System.Drawing.Size(104, 32);
			this.ok_b.Text = "OK";
			this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Items.Add("WZ");
			this.comboBox1.Items.Add("PZ");
			this.comboBox1.Items.Add("MP");
			this.comboBox1.Items.Add("MW");
			this.comboBox1.Items.Add("FV");
			this.comboBox1.Items.Add("PA");
			this.comboBox1.Items.Add("MM");
			this.comboBox1.Location = new System.Drawing.Point(16, 120);
			this.comboBox1.Size = new System.Drawing.Size(100, 24);
			this.comboBox1.GotFocus += new System.EventHandler(this.comboBox1_GotFocus);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(128, 104);
			this.button1.Size = new System.Drawing.Size(96, 40);
			this.button1.Text = "KLAWIATURA";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label2.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label2.Location = new System.Drawing.Point(8, 264);
			this.label2.Size = new System.Drawing.Size(224, 24);
			this.label2.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(160, 176);
			this.label1.Size = new System.Drawing.Size(64, 20);
			this.label1.Text = "Wys�any";
			// 
			// send_c
			// 
			this.send_c.Location = new System.Drawing.Point(136, 176);
			this.send_c.Size = new System.Drawing.Size(16, 20);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form9
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(234, 294);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.send_c);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.ok_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.nazwa_t);
			this.Controls.Add(this.nazwa_l);
			this.Controls.Add(this.data_l);
			this.Controls.Add(this.data_t);
			this.Controls.Add(this.typ_l);
			this.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.Text = "Wprowadzanie";

		}
		#endregion

		

		private void ok_b_Click(object sender, System.EventArgs e)
		{
			if (nazwa_t.Text != "" && comboBox1.Text != "")
			{
				
				WriteLine();
				Form8 fr8 = new Form8(lic);
				fr8.Show();
				
			}
			else if (nazwa_t.Text == "")
			{
				MessageBox.Show("Wprowad� nazw� dokumentu");
			}
			else if (comboBox1.Text == "")
			{
				MessageBox.Show("Wprowad� typ dokumentu");
			}
		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			inputPanel1.Enabled = false;
			this.Close();
			Form8 fr8 = new Form8(lic);
			fr8.Show();
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

		
	}
}
