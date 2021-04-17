using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Data.SqlClient;

namespace SmartDeviceApplication2
{
	/// <summary>
	/// Summary description for Form8.
	/// </summary>
	public class Form15 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Button delete;
		private System.Windows.Forms.Button edit;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button view;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		public System.Windows.Forms.DataGrid dataGrid2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button5;
		private System.Windows.Forms.Button button6;
		public System.Windows.Forms.DataGrid dataGrid1;
	
		public Form15()
		{
			//
			// Required for Windows Form Designer support
			//
			
			
			
			
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
        DataGridTableStyle ts2 = new DataGridTableStyle();
		SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 0", cn);
		DataTable table = new DataTable();
		da.Fill(table);
		dataGrid1.DataSource = table.DefaultView;

        dataGrid1.TableStyles.Add(ts);
        dataGrid1.TableStyles[0].GridColumnStyles[0].Width = 0;
        dataGrid1.TableStyles[0].GridColumnStyles[1].Width = 0;
        dataGrid1.TableStyles[0].GridColumnStyles[2].Width = 0;
        dataGrid1.TableStyles[0].GridColumnStyles[3].Width = 0;

        SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 1", cn);
        DataTable table2 = new DataTable();
        db.Fill(table2);
        dataGrid2.DataSource = table2.DefaultView;
        dataGrid2.TableStyles.Add(ts2);
        dataGrid2.TableStyles[0].GridColumnStyles[0].Width = 0;
        dataGrid2.TableStyles[0].GridColumnStyles[1].Width = 0;
        dataGrid2.TableStyles[0].GridColumnStyles[2].Width = 0;
        dataGrid2.TableStyles[0].GridColumnStyles[3].Width = 0;

	
		cn.Close();
		}
		
		private void Deleterow (int deleteflag)
		{
			string connectionString;
			
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			
			if (deleteflag == 1)
			{
				int index = dataGrid1.CurrentCell.RowNumber;
				SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 0", cn);
				DataTable table1 = new DataTable();
				db.Fill(table1);
				string dataindex = table1.Rows[index][0].ToString();
				string dokindex = table1.Rows[index][4].ToString();
				db.DeleteCommand = new SqlCeCommand("DELETE FROM edihead WHERE id =  ?", cn);
				db.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);	
				db.DeleteCommand.Parameters["@k"].Value = int.Parse(dataindex);
				db.DeleteCommand.ExecuteNonQuery();
			
				SqlCeCommand da = new SqlCeCommand("DELETE FROM edibody WHERE NrDok =  ?", cn);
				da.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				da.Parameters["@k"].Value = dokindex;
				da.Prepare();
				da.ExecuteNonQuery();
			
				SqlCeCommand dc = new SqlCeCommand("DELETE FROM ediend WHERE NrDok =  ?", cn);
				dc.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				dc.Parameters["@k"].Value = dokindex;
				dc.Prepare();
				dc.ExecuteNonQuery();

				SqlCeCommand dx = new SqlCeCommand("DELETE FROM fedibody WHERE NrDok =  ?", cn);
				dx.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				dx.Parameters["@k"].Value = dokindex;
				dx.Prepare();
				dx.ExecuteNonQuery();
			}
			if (deleteflag == 2)
			{
				
				int index = dataGrid2.CurrentCell.RowNumber;
				
				SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 1", cn);
				DataTable table1 = new DataTable();
				db.Fill(table1);
				string dataindex = table1.Rows[index][0].ToString();
				string dokindex = table1.Rows[index][4].ToString();
				db.DeleteCommand = new SqlCeCommand("DELETE FROM edihead WHERE id =  ?", cn);
				db.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);	
				db.DeleteCommand.Parameters["@k"].Value = int.Parse(dataindex);
				db.DeleteCommand.ExecuteNonQuery();
			
				SqlCeCommand da = new SqlCeCommand("DELETE FROM edibody WHERE NrDok =  ?", cn);
				da.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				da.Parameters["@k"].Value = dokindex;
				da.Prepare();
				da.ExecuteNonQuery();
			
				SqlCeCommand dc = new SqlCeCommand("DELETE FROM ediend WHERE NrDok =  ?", cn);
				dc.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				dc.Parameters["@k"].Value = dokindex;
				dc.Prepare();
				dc.ExecuteNonQuery();

				SqlCeCommand dx = new SqlCeCommand("DELETE FROM fedibody WHERE NrDok =  ?", cn);
				dx.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				dx.Parameters["@k"].Value = dokindex;
				dx.Prepare();
				dx.ExecuteNonQuery();
			}
			
			
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
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.view = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.dataGrid2 = new System.Windows.Forms.DataGrid();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackColor = System.Drawing.Color.Azure;
            this.dataGrid1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGrid1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
            this.dataGrid1.HeaderBackColor = System.Drawing.Color.Gold;
            this.dataGrid1.Location = new System.Drawing.Point(0, 0);
            this.dataGrid1.Name = "dataGrid1";
            this.dataGrid1.Size = new System.Drawing.Size(216, 176);
            this.dataGrid1.TabIndex = 0;
            // 
            // exit_b
            // 
            this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.exit_b.Location = new System.Drawing.Point(144, 208);
            this.exit_b.Name = "exit_b";
            this.exit_b.Size = new System.Drawing.Size(72, 32);
            this.exit_b.TabIndex = 5;
            this.exit_b.Text = "WYJŒCIE";
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            // 
            // delete
            // 
            this.delete.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.delete.Location = new System.Drawing.Point(144, 176);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(72, 32);
            this.delete.TabIndex = 4;
            this.delete.Text = "USUÑ";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // edit
            // 
            this.edit.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.edit.Location = new System.Drawing.Point(72, 176);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(72, 32);
            this.edit.TabIndex = 3;
            this.edit.Text = "EDYTUJ";
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(0, 208);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "ODBIERZ NOWE";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // view
            // 
            this.view.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.view.Location = new System.Drawing.Point(0, 176);
            this.view.Name = "view";
            this.view.Size = new System.Drawing.Size(72, 32);
            this.view.TabIndex = 1;
            this.view.Text = "OTWÓRZ";
            this.view.Click += new System.EventHandler(this.view_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(0, 208);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(144, 32);
            this.button3.TabIndex = 5;
            this.button3.Text = "WYŒLIJ GOTOWE";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(8, 272);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 16);
            this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(234, 267);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGrid1);
            this.tabPage1.Controls.Add(this.view);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.edit);
            this.tabPage1.Controls.Add(this.delete);
            this.tabPage1.Controls.Add(this.exit_b);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(226, 239);
            this.tabPage1.Text = "KOMPLETACJE";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.button2);
            this.tabPage2.Controls.Add(this.button4);
            this.tabPage2.Controls.Add(this.button5);
            this.tabPage2.Controls.Add(this.button6);
            this.tabPage2.Controls.Add(this.dataGrid2);
            this.tabPage2.Controls.Add(this.button3);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(217, 239);
            this.tabPage2.Text = "SKOMPLETOWANE";
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(0, 176);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 32);
            this.button2.TabIndex = 0;
            this.button2.Text = "OTWÓRZ";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button4.Location = new System.Drawing.Point(72, 176);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(72, 32);
            this.button4.TabIndex = 1;
            this.button4.Text = "EDYTUJ";
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button5.Location = new System.Drawing.Point(144, 176);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(72, 32);
            this.button5.TabIndex = 2;
            this.button5.Text = "USUÑ";
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button6.Location = new System.Drawing.Point(144, 208);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(72, 32);
            this.button6.TabIndex = 3;
            this.button6.Text = "WYJŒCIE";
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // dataGrid2
            // 
            this.dataGrid2.BackColor = System.Drawing.Color.Azure;
            this.dataGrid2.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dataGrid2.ForeColor = System.Drawing.Color.MidnightBlue;
            this.dataGrid2.GridLineColor = System.Drawing.Color.Black;
            this.dataGrid2.HeaderBackColor = System.Drawing.Color.Gold;
            this.dataGrid2.Location = new System.Drawing.Point(0, 0);
            this.dataGrid2.Name = "dataGrid2";
            this.dataGrid2.Size = new System.Drawing.Size(216, 176);
            this.dataGrid2.TabIndex = 4;
            // 
            // Form15
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form15";
            this.Text = "Kompletacja Dokumentów";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		#endregion

		
		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void delete_Click(object sender, System.EventArgs e)
		{
			Deleterow(1);
			Loaddata();
		}

		private void edit_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Form19 frm19 = new Form19(dataGrid1.CurrentCell.RowNumber, 0);
			frm19.Show();
			
			
		}

		private void view_Click(object sender, System.EventArgs e)
		{
			string connectionString;
			int index = dataGrid1.CurrentCell.RowNumber;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 0", cn);
			DataTable table1 = new DataTable();
			db.Fill(table1);
			
			string dokindex = table1.Rows[index][4].ToString();
			cn.Close();
			
			Form17 frm17 = new Form17(dokindex);
			frm17.Show();
			this.Close();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Form16 frm16 = new Form16(this);
			frm16.Show();
			
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			Form20 frm20 = new Form20(dataGrid1.CurrentCell.RowNumber);
			frm20.Show();
			
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			string connectionString;
			int index = dataGrid2.CurrentCell.RowNumber;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edihead WHERE complete = 1", cn);
			DataTable table1 = new DataTable();
			db.Fill(table1);
			
			string dokindex = table1.Rows[index][4].ToString();
			
			cn.Close();
			Form17 frm17 = new Form17(dokindex);
			frm17.Show();
			this.Close();
		}

		private void button5_Click(object sender, System.EventArgs e)
		{
			Deleterow(2);
			Loaddata();
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Form19 frm19 = new Form19(dataGrid1.CurrentCell.RowNumber, 1);
			frm19.Show();
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		}
}
