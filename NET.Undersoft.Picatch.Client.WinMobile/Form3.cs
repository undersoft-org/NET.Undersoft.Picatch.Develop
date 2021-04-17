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
    /// Summary description for Form3.
    /// </summary>
    public class Form3 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button exit_b;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button add_b;
        public System.Windows.Forms.DataGrid dataGrid1;
        private string index;
        private string index2;
        private System.Windows.Forms.Label label1;
        private int rownum;
        int lic;
        public Form3(int rownumber, int licence)
        {
            //
            // Required for Windows Form Designer support
            //
            lic = licence;
            rownum = rownumber;
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            Update();
            Loaddata();
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }
        public void Loaddata()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            DataGridTableStyle ts = new DataGridTableStyle();

            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
            DataTable table = new DataTable();
            da.Fill(table);
            index = table.Rows[rownum][0].ToString();
            SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
            DataTable table2 = new DataTable();
            db.SelectCommand = new SqlCeCommand("SELECT * FROM bufor WHERE dokid =  ?", cn);
            db.SelectCommand.Parameters.Add("@k", SqlDbType.Int);
            db.SelectCommand.Parameters["@k"].Value = int.Parse(index);
            db.SelectCommand.ExecuteNonQuery();
            db.Fill(table2);

            dataGrid1.DataSource = table2.DefaultView;

            dataGrid1.TableStyles.Add(ts);
            dataGrid1.TableStyles[0].GridColumnStyles[0].Width = 0;
            dataGrid1.TableStyles[0].GridColumnStyles[1].Width = 0;
            dataGrid1.TableStyles[0].GridColumnStyles[2].Width = 70;
            dataGrid1.TableStyles[0].GridColumnStyles[3].Width = 85;
            dataGrid1.TableStyles[0].GridColumnStyles[4].Width = 0;
            dataGrid1.TableStyles[0].GridColumnStyles[5].Width = 25;
            dataGrid1.TableStyles[0].GridColumnStyles[6].Width = 0;

            cn.Close();
        }

        private void Deleterow()
        {
            DialogResult result = MessageBox.Show("Czy na pewno usun¹æ?", "Uwaga", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                try
                {
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();
                    SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
                    DataTable table2 = new DataTable();
                    db.SelectCommand = new SqlCeCommand("SELECT * FROM bufor WHERE dokid =  ?", cn);
                    db.SelectCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                    db.SelectCommand.Parameters["@k"].Value = int.Parse(index);
                    db.SelectCommand.ExecuteNonQuery();
                    db.Fill(table2);
                    index2 = table2.Rows[dataGrid1.CurrentCell.RowNumber][0].ToString();
                    SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
                    da.DeleteCommand = new SqlCeCommand("DELETE FROM bufor WHERE id =  ?", cn);
                    da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                    da.DeleteCommand.Parameters["@k"].Value = int.Parse(index2);
                    da.DeleteCommand.ExecuteNonQuery();
                    cn.Close();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Brak pozycji do usuniêcia");
                }
            }


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
            this.exit_b = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.add_b = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackColor = System.Drawing.Color.Azure;
            this.dataGrid1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
            this.dataGrid1.HeaderBackColor = System.Drawing.Color.Gold;
            this.dataGrid1.Location = new System.Drawing.Point(5, 5);
            this.dataGrid1.Size = new System.Drawing.Size(225, 187);
            this.dataGrid1.Text = "dataGrid1";
            // 
            // exit_b
            // 
            this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.exit_b.Location = new System.Drawing.Point(120, 230);
            this.exit_b.Size = new System.Drawing.Size(110, 32);
            this.exit_b.Text = "WYJŒCIE";
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            // 
            // delete
            // 
            this.delete.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.delete.Location = new System.Drawing.Point(5, 230);
            this.delete.Size = new System.Drawing.Size(110, 32);
            this.delete.Text = "USUÑ";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // edit
            // 
            this.edit.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.edit.Location = new System.Drawing.Point(120, 195);
            this.edit.Size = new System.Drawing.Size(110, 32);
            this.edit.Text = "EDYTUJ";
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // add_b
            // 
            this.add_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.add_b.Location = new System.Drawing.Point(5, 195);
            this.add_b.Size = new System.Drawing.Size(110, 32);
            this.add_b.Text = "DODAJ";
            this.add_b.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(8, 264);
            this.label1.Size = new System.Drawing.Size(224, 24);
            this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            // 
            // Form3
            // 
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.add_b);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.exit_b);
            this.Controls.Add(this.dataGrid1);
            this.Text = "Lista Pozycji Dokumentu";

        }
        #endregion


        private void exit_b_Click(object sender, System.EventArgs e)
        {
            Form8 frm8 = new Form8(lic);
            frm8.Show();
            this.Close();
        }

        private void delete_Click(object sender, System.EventArgs e)
        {

            DialogResult result = MessageBox.Show("Czy napenow usun¹æ", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                Deleterow();
            }

            Loaddata();
        }

        private void edit_Click(object sender, System.EventArgs e)
        {

            try
            {
                Form4 frm4 = new Form4(dataGrid1.CurrentCell.RowNumber, rownum, lic);
                frm4.Show();
                this.Close();
            }
            catch (Exception b)
            {
                MessageBox.Show("Brak pozycji");
            }

        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                Form2 frm2 = new Form2(rownum, lic);
                frm2.Show();
                this.Close();
            }
            catch (Exception c)
            {
                MessageBox.Show("Brak pozycji");
            }

        }





    }
}
