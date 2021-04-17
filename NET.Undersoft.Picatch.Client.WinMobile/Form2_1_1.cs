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
    /// Summary description for Form13.
    /// </summary>
    public class Form2_1_1 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button search_b;
        private System.Windows.Forms.TextBox search_t;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button details_b;
        private System.Windows.Forms.Button exit_b;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
        private System.Windows.Forms.DataGrid dataGrid1;
        public DataTable table2 = new DataTable();
        string kodzik;
        string nazwik;
        string cenik;
        string ceniksp;
        string vacik;
        string stanik;


        public Form2_1_1(string kodkresk)
        {
            //
            // Required for Windows Form Designer support
            //

            InitializeComponent();
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            Update();
            comboBox1.Text = "Nazwa";
            search_t.Focus();
            search_t.SelectAll();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        private void FindIndex()
        {

            string kodbuf = search_t.Text;
            search_t.Text = "SZUKAM TOWARÓW W BAZIE";
            search_t.Refresh();
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            if (comboBox1.Text == "Kod" && kodbuf != "")
            {

                //SqlCeCommand cmd = cn.CreateCommand();
                //cmd.CommandText = "SELECT kod, nazwa, stan, cenazk FROM dane WHERE kod = ?";
                //cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
                //cmd.Parameters["@k"].Value = kodbuf;

                //cmd.Prepare();
                //SqlCeDataReader dr = cmd.ExecuteReader();

                DataGridTableStyle ts = new DataGridTableStyle();
                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM dane", cn);
                table2 = new DataTable();
                db.SelectCommand = new SqlCeCommand("SELECT kod, nazwa, stan, cenazk, cenasp, vat FROM dane WHERE kod =  ?", cn);
                db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                db.SelectCommand.Parameters["@k"].Value = kodbuf;
                db.SelectCommand.ExecuteNonQuery();
                db.Fill(table2);

                if (table2.Rows.Count != 0)
                {
                    dataGrid1.DataSource = table2.DefaultView;

                }
                else
                {
                    MessageBox.Show("Nie znaleziono towarów");

                }
            }
            else if (comboBox1.Text == "Nazwa" && kodbuf.Length >= 3)
            {

                string delimeter = " ";
                string[] pozycje = kodbuf.Split(delimeter.ToCharArray());
                string like = "";
                for (int i = 0; i < pozycje.Length; i++)
                {
                    like += "nazwa LIKE '%" + pozycje[i] + "%'";
                    if (i <= pozycje.Length - 2)
                    {
                        like += " AND ";
                    }
                }

                //SqlCeCommand cmd = cn.CreateCommand();
                //cmd.CommandText = "SELECT TOP 100 kod, nazwa, stan FROM dane WHERE " + like;
                //cmd.Parameters.Add("@"+i.ToString(), SqlDbType.NVarChar, 20);	
                //cmd.Parameters["@k"].Value = kodbuf;

                //cmd.Prepare();
                //SqlCeDataReader dr = cmd.ExecuteReader();
                DataGridTableStyle ts = new DataGridTableStyle();
                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM dane", cn);
                table2 = new DataTable();
                db.SelectCommand = new SqlCeCommand("SELECT kod, nazwa, stan, cenazk, cenasp, vat FROM dane WHERE " + like, cn);
                db.SelectCommand.ExecuteNonQuery();
                db.Fill(table2);
                //if (dr.RecordsAffected != 0)
                //{
                //	DataSet ds = new DataSet();
                //
                //	DataTable dt = new DataTable("Table1");
                //
                //	ds.Tables.Add(dt);
                //
                //	ds.Load(dr, LoadOption.PreserveChanges, ds.Tables[0]);
                //
                //	dataGrid1.DataSource = ds.Tables[0];
                //
                //}
                if (table2.Rows.Count != 0)
                {
                    dataGrid1.DataSource = table2.DefaultView;

                }
                else
                {
                    MessageBox.Show("Nie znaleziono towarów");

                }
            }
            else
            {
                MessageBox.Show("Nie wpisano nazwy (min 3 znaki) lub kodu", "Towary");
            }
            search_t.Text = "";
            search_t.Focus();
            dataGrid1.Refresh();

            cn.Close();
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGrid1 = new System.Windows.Forms.DataGrid();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.search_b = new System.Windows.Forms.Button();
            this.search_t = new System.Windows.Forms.TextBox();
            this.details_b = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.exit_b = new System.Windows.Forms.Button();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackColor = System.Drawing.Color.Azure;
            this.dataGrid1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.dataGrid1.GridLineColor = System.Drawing.SystemColors.ControlText;
            this.dataGrid1.HeaderBackColor = System.Drawing.Color.Gold;
            this.dataGrid1.Location = new System.Drawing.Point(5, 56);
            this.dataGrid1.Size = new System.Drawing.Size(225, 176);
            this.dataGrid1.Text = "dataGrid1";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.comboBox1.Items.Add("Kod");
            this.comboBox1.Items.Add("Nazwa");
            this.comboBox1.Location = new System.Drawing.Point(5, 32);
            this.comboBox1.Size = new System.Drawing.Size(80, 23);
            // 
            // search_b
            // 
            this.search_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.search_b.Location = new System.Drawing.Point(88, 32);
            this.search_b.Size = new System.Drawing.Size(70, 23);
            this.search_b.Text = "SZUKAJ";
            this.search_b.Click += new System.EventHandler(this.search_b_Click);
            // 
            // search_t
            // 
            this.search_t.BackColor = System.Drawing.Color.Gold;
            this.search_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.search_t.Location = new System.Drawing.Point(5, 5);
            this.search_t.Size = new System.Drawing.Size(225, 24);
            this.search_t.Text = "Tutaj wpisz tekst lub kod";
            // 
            // details_b
            // 
            this.details_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.details_b.Location = new System.Drawing.Point(6, 232);
            this.details_b.Size = new System.Drawing.Size(109, 32);
            this.details_b.Text = "SZCZEGÓŁY";
            this.details_b.Click += new System.EventHandler(this.details_b_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.button2.Location = new System.Drawing.Point(160, 32);
            this.button2.Size = new System.Drawing.Size(70, 23);
            this.button2.Text = "KLAW.";
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            // exit_b
            // 
            this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
            this.exit_b.Location = new System.Drawing.Point(118, 232);
            this.exit_b.Size = new System.Drawing.Size(111, 32);
            this.exit_b.Text = "WYJŚCIE";
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            // 
            // Form13
            // 
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.Controls.Add(this.details_b);
            this.Controls.Add(this.exit_b);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.search_t);
            this.Controls.Add(this.search_b);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.dataGrid1);
            this.Text = "Towary";

        }
        #endregion

        private void search_b_Click(object sender, System.EventArgs e)
        {
            FindIndex();
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (inputPanel1.Enabled == true)
            {
                inputPanel1.Enabled = false;
            }
            else
            {
                inputPanel1.Enabled = true;
            }
            search_t.Focus();
            search_t.SelectAll();
        }

        private void exit_b_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void details_b_Click(object sender, System.EventArgs e)
        {


            kodzik = table2.Rows[dataGrid1.CurrentCell.RowNumber][0].ToString();
            nazwik = table2.Rows[dataGrid1.CurrentCell.RowNumber][1].ToString();
            cenik = table2.Rows[dataGrid1.CurrentCell.RowNumber][3].ToString();
            ceniksp = table2.Rows[dataGrid1.CurrentCell.RowNumber][4].ToString();
            vacik = table2.Rows[dataGrid1.CurrentCell.RowNumber][5].ToString();
            stanik = table2.Rows[dataGrid1.CurrentCell.RowNumber][2].ToString();
            details det = new details(kodzik, nazwik, cenik, ceniksp, vacik, stanik);
            det.Show();
        }









    }
}