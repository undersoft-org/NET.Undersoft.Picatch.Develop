using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Runtime.InteropServices;


namespace SmartDeviceApplication2
{
    /// <summary>
    /// Summary description for Form4.
    /// </summary>
    public class Form10 : System.Windows.Forms.Form
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
        private System.Windows.Forms.Button button1;
        private string index;
        private System.Windows.Forms.CheckBox send_c;
        private System.Windows.Forms.Label label2;
        private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;

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
        private int lic;

        public Form10(int rownumber, int licence)
        {
            //
            // Required for Windows Form Designer support
            lic = licence;
            InitializeComponent();
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            Update();
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
            DataTable table = new DataTable();
            da.Fill(table);
            index = table.Rows[rownumber][0].ToString();
            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT nazwadok, typ, data, send FROM dok WHERE id = ?";
            cmd.Parameters.Add("@k", SqlDbType.Int);
            cmd.Parameters["@k"].Value = int.Parse(index);
            cmd.Prepare();
            SqlCeDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                nazwa_t.Text = dr.GetString(0);
                comboBox1.Text = dr.GetString(1);
                data_t.Text = dr.GetSqlDateTime(2).ToString();
                if (dr.GetBoolean(3) == true)
                {
                    send_c.Checked = dr.GetBoolean(3);
                }

            }
            nazwa_t.Focus();
            data_t.Text = System.DateTime.Now.ToString();
            //
            // TODO: Add any constructor code after InitializeComponent call
            cn.Close();
            //
        }

        [DllImport("coredll.dll", SetLastError = true)]
        //[return: System.Runtime.InteropServices.Marshal(UnmanagedType.Bool)]
        public static extern bool SipGetInfo(ref SIPINFO sipInfo);


        [DllImport("coredll.dll", SetLastError = true)]
        //[return: Marshal(UnmanagedType.Bool)]
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




        public void WriteLine()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM dok", cn);
            DataTable table = new DataTable();
            da.Fill(table);
            bool val;
            



          
            val = bool.Parse(send_c.Text);

            da.UpdateCommand = new SqlCeCommand("UPDATE dok SET nazwadok = ?, typ = ?, data = ? send = ? WHERE id =  ?", cn);
            da.UpdateCommand.Parameters.Add("@k", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@n", SqlDbType.NVarChar, 10);
            da.UpdateCommand.Parameters.Add("@cz", SqlDbType.DateTime);
            da.UpdateCommand.Parameters.Add("@f", SqlDbType.Bit);
            da.UpdateCommand.Parameters.Add("@index", SqlDbType.Int);
            da.UpdateCommand.Parameters["@k"].Value = nazwa_t.Text;
            da.UpdateCommand.Parameters["@n"].Value = comboBox1.Text;
            da.UpdateCommand.Parameters["@cz"].Value = Convert.ToDateTime(data_t.Text);
            da.UpdateCommand.Parameters["@f"].Value = val;
            da.UpdateCommand.Parameters["@index"].Value = int.Parse(index);
            da.UpdateCommand.ExecuteNonQuery();
            cn.Close();
            this.Close();


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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form10));
            this.typ_l = new System.Windows.Forms.Label();
            this.data_t = new System.Windows.Forms.TextBox();
            this.data_l = new System.Windows.Forms.Label();
            this.nazwa_l = new System.Windows.Forms.Label();
            this.nazwa_t = new System.Windows.Forms.TextBox();
            this.exit_b = new System.Windows.Forms.Button();
            this.ok_b = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.send_c = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
            this.SuspendLayout();
            // 
            // typ_l
            // 
            this.typ_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.typ_l.ForeColor = System.Drawing.Color.Gold;
            this.typ_l.Location = new System.Drawing.Point(16, 96);
            this.typ_l.Name = "typ_l";
            this.typ_l.Size = new System.Drawing.Size(32, 16);
            this.typ_l.Text = "Typ";
            // 
            // data_t
            // 
            this.data_t.BackColor = System.Drawing.Color.Azure;
            this.data_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.data_t.Location = new System.Drawing.Point(16, 176);
            this.data_t.Name = "data_t";
            this.data_t.Size = new System.Drawing.Size(96, 23);
            this.data_t.TabIndex = 10;
            // 
            // data_l
            // 
            this.data_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.data_l.ForeColor = System.Drawing.Color.Gold;
            this.data_l.Location = new System.Drawing.Point(16, 152);
            this.data_l.Name = "data_l";
            this.data_l.Size = new System.Drawing.Size(48, 20);
            this.data_l.Text = "Data";
            // 
            // nazwa_l
            // 
            this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.nazwa_l.ForeColor = System.Drawing.Color.Gold;
            this.nazwa_l.Location = new System.Drawing.Point(16, 24);
            this.nazwa_l.Name = "nazwa_l";
            this.nazwa_l.Size = new System.Drawing.Size(128, 20);
            this.nazwa_l.Text = "Nazwa dokumentu";
            // 
            // nazwa_t
            // 
            this.nazwa_t.BackColor = System.Drawing.Color.Azure;
            this.nazwa_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
            this.nazwa_t.ForeColor = System.Drawing.Color.MidnightBlue;
            this.nazwa_t.Location = new System.Drawing.Point(16, 48);
            this.nazwa_t.Multiline = true;
            this.nazwa_t.Name = "nazwa_t";
            this.nazwa_t.Size = new System.Drawing.Size(208, 40);
            this.nazwa_t.TabIndex = 7;
            this.nazwa_t.GotFocus += new System.EventHandler(this.nazwa_t_GotFocus);
            this.nazwa_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.nazwa_t_KeyPress);
            this.nazwa_t.LostFocus += new System.EventHandler(this.nazwa_t_LostFocus);
            // 
            // exit_b
            // 
            this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.exit_b.Location = new System.Drawing.Point(128, 216);
            this.exit_b.Name = "exit_b";
            this.exit_b.Size = new System.Drawing.Size(96, 32);
            this.exit_b.TabIndex = 6;
            this.exit_b.Text = "WYJŒCIE";
            this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
            // 
            // ok_b
            // 
            this.ok_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.ok_b.Location = new System.Drawing.Point(16, 216);
            this.ok_b.Name = "ok_b";
            this.ok_b.Size = new System.Drawing.Size(104, 32);
            this.ok_b.TabIndex = 5;
            this.ok_b.Text = "OK";
            this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
            this.comboBox1.ForeColor = System.Drawing.Color.MidnightBlue;
            this.comboBox1.Items.Add("WZ");
            this.comboBox1.Items.Add("PZ");
            this.comboBox1.Items.Add("MP");
            this.comboBox1.Items.Add("MW");
            this.comboBox1.Items.Add("FV");
            this.comboBox1.Items.Add("PA");
            this.comboBox1.Items.Add("MM");
            this.comboBox1.Location = new System.Drawing.Point(16, 120);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(100, 25);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.GotFocus += new System.EventHandler(this.comboBox1_GotFocus);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
            this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.label1.Location = new System.Drawing.Point(8, 264);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 24);
            this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(128, 104);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 40);
            this.button1.TabIndex = 2;
            this.button1.Text = "KLAWIATURA";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // send_c
            // 
            this.send_c.Location = new System.Drawing.Point(128, 176);
            this.send_c.Name = "send_c";
            this.send_c.Size = new System.Drawing.Size(16, 20);
            this.send_c.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.Color.Gold;
            this.label2.Location = new System.Drawing.Point(152, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 20);
            this.label2.Text = "Wys³any";
            // 
            // Form10
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(234, 294);
            this.ControlBox = false;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.send_c);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form10";
            this.Text = "Nowy Dokument";
            this.ResumeLayout(false);

        }
        #endregion



        private void ok_b_Click(object sender, System.EventArgs e)
        {
            if (nazwa_t.Text != "" && comboBox1.Text != "")
            {

                WriteLine();
                Form8 fr8 = new Form8(lic);
                fr8.Refresh();

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
            Form8 fr8 = new Form8(lic);
            fr8.Refresh();
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
