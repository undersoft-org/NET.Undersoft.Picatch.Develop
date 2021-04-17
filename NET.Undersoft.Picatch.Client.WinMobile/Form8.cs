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
    public class Form8 : System.Windows.Forms.Form
    {
        private System.Windows.Forms.Button exit_b;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button view;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.DataGrid dataGrid1;
        private System.Windows.Forms.Button delete_send_b;
        int lic;
        public Form8(int licence)
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
            dataGrid1.DataSource = table.DefaultView;

            dataGrid1.TableStyles.Add(ts);
            dataGrid1.TableStyles[0].GridColumnStyles[0].Width = 0;
            dataGrid1.TableStyles[0].GridColumnStyles[1].Width = 80;
            dataGrid1.TableStyles[0].GridColumnStyles[2].Width = 30;
            dataGrid1.TableStyles[0].GridColumnStyles[3].Width = 70;

            cn.Close();
        }

        private void Deleterow()
        {
            string connectionString;
            int index = dataGrid1.CurrentCell.RowNumber;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM dok", cn);
            DataTable table1 = new DataTable();
            db.Fill(table1);
            string dataindex = table1.Rows[index][0].ToString();
            db.DeleteCommand = new SqlCeCommand("DELETE FROM dok WHERE id =  ?", cn);
            db.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
            db.DeleteCommand.Parameters["@k"].Value = int.Parse(dataindex);
            db.DeleteCommand.ExecuteNonQuery();
            SqlCeCommand da = new SqlCeCommand("DELETE FROM bufor WHERE dokid =  ?", cn);
            da.Parameters.Add("@k", SqlDbType.Int, 10);
            da.Parameters["@k"].Value = int.Parse(dataindex);
            da.Prepare();
            da.ExecuteNonQuery();

            cn.Close();

        }
        private void Deleteall()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand da = new SqlCeCommand("DELETE FROM dok WHERE send =  '1'", cn);
            da.Prepare();
            da.ExecuteNonQuery();

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
            this.exit_b = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.view = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.delete_send_b = new System.Windows.Forms.Button();
            // 
            // dataGrid1
            // 
            this.dataGrid1.BackColor = System.Drawing.Color.Azure;
            this.dataGrid1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
            this.dataGrid1.HeaderBackColor = System.Drawing.Color.Gold;
            this.dataGrid1.Location = new System.Drawing.Point(5, 5);
            this.dataGrid1.Size = new System.Drawing.Size(225, 160);
            this.dataGrid1.Text = "dataGrid1";
            // 
            // exit_b
            // 
            this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.exit_b.Location = new System.Drawing.Point(168, 204);
            this.exit_b.Size = new System.Drawing.Size(64, 60);
            this.exit_b.Text = "WYJŒCIE";
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            // 
            // delete
            // 
            this.delete.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.delete.Location = new System.Drawing.Point(88, 204);
            this.delete.Size = new System.Drawing.Size(72, 32);
            this.delete.Text = "USUÑ";
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // edit
            // 
            this.edit.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.edit.Location = new System.Drawing.Point(8, 204);
            this.edit.Size = new System.Drawing.Size(72, 32);
            this.edit.Text = "EDYTUJ";
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(8, 168);
            this.button1.Size = new System.Drawing.Size(72, 32);
            this.button1.Text = "DODAJ";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // view
            // 
            this.view.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.view.Location = new System.Drawing.Point(88, 168);
            this.view.Size = new System.Drawing.Size(72, 32);
            this.view.Text = "OTWÓRZ";
            this.view.Click += new System.EventHandler(this.view_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button3.Location = new System.Drawing.Point(168, 168);
            this.button3.Size = new System.Drawing.Size(64, 32);
            this.button3.Text = "WYŒLIJ";
            this.button3.Click += new System.EventHandler(this.button3_Click);
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
            // delete_send_b
            // 
            this.delete_send_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.delete_send_b.Location = new System.Drawing.Point(8, 240);
            this.delete_send_b.Size = new System.Drawing.Size(152, 24);
            this.delete_send_b.Text = "USUÑ WYS£ANE";
            this.delete_send_b.Click += new System.EventHandler(this.delete_send_b_Click);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            // 
            // Form8
            // 
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.ControlBox = false;
            this.Controls.Add(this.delete_send_b);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.view);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.edit);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.exit_b);
            this.Controls.Add(this.dataGrid1);
            this.Text = "Lista Dokumentów";

        }
        #endregion


        private void exit_b_Click(object sender, System.EventArgs e)
        {
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
            Form10 frm10 = new Form10(dataGrid1.CurrentCell.RowNumber, lic);
            frm10.Show();


        }

        private void view_Click(object sender, System.EventArgs e)
        {

            try
            {
                Form3 frm3 = new Form3(dataGrid1.CurrentCell.RowNumber, lic);
                frm3.Show();
                this.Close();
            }
            catch (Exception d)
            {
                MessageBox.Show("Brak pozycji");
            }
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            try
            {
                Form9 frm9 = new Form9(lic);
                frm9.Show();
                this.Close();
            }
            catch (Exception f)
            {
                MessageBox.Show(f.Message);
            }
        }

        private void button3_Click(object sender, System.EventArgs e)
        {
            Form11 frm11 = new Form11(dataGrid1.CurrentCell.RowNumber, lic);
            frm11.Show();

        }

        private void delete_send_b_Click(object sender, System.EventArgs e)
        {
            DialogResult result = MessageBox.Show("Czy napenow usun¹æ", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (result == DialogResult.Yes)
            {
                Deleteall();
            }

            Loaddata();
        }

    }
}