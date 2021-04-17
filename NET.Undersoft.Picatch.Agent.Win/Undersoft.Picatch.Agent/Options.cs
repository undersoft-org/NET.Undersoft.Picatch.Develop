using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Net;
using InTheHand.Net;
using InTheHand.Net.Ports;
using InTheHand.Net.Bluetooth;
using InTheHand.Windows.Forms;
using InTheHand.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.SqlServerCe;

namespace Undersoft.Picatch.Agent
{
    public partial class Options : Form
    {

        InTheHand.Net.BluetoothAddress[] address_array = new BluetoothAddress[1000];
        BluetoothDeviceInfo[] array;
        //OPCJE
       private string transfer;
       private string com;
       private string ip;
       private string ufile;
       private string dfile;
       private string bdll;
       private bool bflag;
       private bool ipflag;
       private bool dbflag;
       private int port;
       private string skaner;
       private string serverpcm;
       private string loginpcm;
       private string passwdpcm;
       private string bazapcm;
       private SqlConnection pcmn;

        public Options()
        {


           
            
            InitializeComponent();
            btassigned();
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            ip_t.Text = localIP;

            string connectionString;
          //  string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmd2 = cn.CreateCommand();
            cmd2.CommandText = "SELECT * FROM opcje WHERE id = 1";
            cmd2.Prepare();
            SqlCeDataReader dr = cmd2.ExecuteReader();

            while (dr.Read())
            {


                typ_t.Text = dr.GetString(1);
                ufile_t.Text = dr.GetString(4);
                dfile_t.Text = dr.GetString(5);

                runpicatchbt_cb.Checked = dr.GetBoolean(7);
                runpicatchtcp_cb.Checked = dr.GetBoolean(8);
                port_t.Text = Convert.ToString(dr.GetInt32(9));
                skaner_t.Text = dr.GetString(10);
                server_t.Text = dr.GetString(11);
                login_t.Text = dr.GetString(12);
                haslo_t.Text = dr.GetString(13);
                baza_t.Text = dr.GetString(14);
                checkBox1.Checked = dr.GetBoolean(15);
                serverpcm = dr.GetString(11);
                loginpcm = dr.GetString(12);
                passwdpcm = dr.GetString(13);
                bazapcm = dr.GetString(14);
                dbflag = dr.GetBoolean(15);


            }
            cn.Close();

            
           
        }



        public void connect_pcm()
        {
            string connectionString;
            connectionString = "server=" + serverpcm + ";user id=" + loginpcm + ";password=" + passwdpcm + ";Trusted_Connection=no; database=" + bazapcm + ";connection timeout=30";
            pcmn = new SqlConnection(connectionString);

            try
            {
                pcmn.Open();
                DataGridTableStyle ts = new DataGridTableStyle();
                SqlDataAdapter db = new SqlDataAdapter("SELECT * FROM Magazyn", pcmn);
                DataTable table2 = new DataTable();
                db.SelectCommand = new SqlCommand("SELECT Nazwa, MagId FROM Magazyn", pcmn);
                db.SelectCommand.ExecuteNonQuery();
                db.Fill(table2);

                if (table2.Rows.Count != 0)
                {
                    dataGridView1.DataSource = table2.DefaultView;
                }
                pcmn.Close();
               
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                SqlCeDataAdapter dbc = new SqlCeDataAdapter("SELECT * FROM magazyn", cn);
                table2 = new DataTable();
                dbc.SelectCommand = new SqlCeCommand("SELECT Nazwa, MagId FROM magazyn", cn);
                dbc.SelectCommand.ExecuteNonQuery();
                dbc.Fill(table2);
                if (table2.Rows.Count != 0)
                {
                    dataGridView2.DataSource = table2.DefaultView;
                }
                cn.Close();
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void dodajmagazyn()
        {
            try
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                if (selectedindex != -1)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                    string magazyn = Convert.ToString(selectedRow.Cells[0].Value);
                    string index = Convert.ToString(selectedRow.Cells[1].Value);
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();
                    SqlCeCommand cmd = new SqlCeCommand("INSERT INTO magazyn (Nazwa, MagId) VALUES (?, ?)", cn);
                    cmd.Parameters.Add("@sp", SqlDbType.NVarChar, 120);
                    cmd.Parameters.Add("@lp", SqlDbType.Int);
                    cmd.Parameters["sp"].Value = magazyn;
                    cmd.Parameters["lp"].Value = int.Parse(index);
                    cmd.ExecuteNonQuery();

                    SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM magazyn", cn);
                    DataTable table2 = new DataTable();
                    db.SelectCommand = new SqlCeCommand("SELECT Nazwa, MagId FROM magazyn", cn);
                    db.SelectCommand.ExecuteNonQuery();
                    db.Fill(table2);
                    dataGridView2.DataSource = table2.DefaultView;
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void usunmagazyn()
        {
            try
            {
                int selectedindex = dataGridView2.SelectedCells[0].RowIndex;
                if (selectedindex != -1)
                {
                    DataGridViewRow selectedRow = dataGridView2.Rows[selectedindex];
                    string magazyn = Convert.ToString(selectedRow.Cells[0].Value);
                    string index = Convert.ToString(selectedRow.Cells[1].Value);
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();
                    SqlCeCommand cmd = new SqlCeCommand("DELETE FROM magazyn WHERE MagId = ?", cn);
                    cmd.Parameters.Add("@lp", SqlDbType.Int);
                   
                    cmd.Parameters["lp"].Value = int.Parse(index);
                    cmd.ExecuteNonQuery();
                    SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM magazyn", cn);
                    DataTable table2 = new DataTable();
                    db.SelectCommand = new SqlCeCommand("SELECT Nazwa, MagId FROM magazyn", cn);
                    db.SelectCommand.ExecuteNonQuery();
                    db.Fill(table2);
                    dataGridView2.DataSource = table2.DefaultView;
                    cn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void addbt_b_Click(object sender, EventArgs e)
        {
            if (lstDevices.SelectedIndex != -1)
            {
                int index = lstDevices.SelectedIndex;
                BluetoothDeviceInfo mac = array[index];
                FileStream Fs = new FileStream("btdevices.dat", FileMode.Append, FileAccess.Write);
                byte[] btsave = mac.DeviceAddress.ToByteArray();
                bool isinlist = lstremDevices.Items.IndexOf(mac.DeviceAddress) != -1;
                if (isinlist != true)
                {
                    Fs.Write(btsave, 0, 6);
                }
                Fs.Close();
                btassigned();
            }
        }

        private void btndiscover_Click(object sender, EventArgs e)
        {
            
        
            this.lstDevices.Items.Clear();
            BluetoothClient bc = new BluetoothClient();

            array = bc.DiscoverDevices();
            for (int i = 0; i < array.Length; i++)
            {
                address_array[i] = array[i].DeviceAddress;
                lstDevices.Items.Add(array[i].DeviceName);
            }


        
        }

        private void btassigned()
        {
            lstremDevices.Items.Clear();
            FileStream Fs = new FileStream("btdevices.dat", FileMode.OpenOrCreate, FileAccess.Read);
            BluetoothAddress[] btadress = new BluetoothAddress[Fs.Length / 6];
            byte[] btopen = new byte[6];
            int recb = 6;
            for (int i = 0; i < Fs.Length / 6; i++)
            {


                Fs.Read(btopen, 0, recb);
                btadress[i] = new BluetoothAddress(btopen);
                this.lstremDevices.Items.Add(btadress[i]);
            }
            Fs.Close();
        }

        private void rembt_b_Click(object sender, EventArgs e)
        {
            if (lstremDevices.SelectedIndex != -1)
            {
                FileStream Fs = new FileStream("btdevices.dat", FileMode.Open, FileAccess.Read);
                FileStream Fb = new FileStream("btbuffor.dat", FileMode.Append, FileAccess.Write);

                int index = this.lstremDevices.SelectedIndex;

                byte[] btrem = new byte[6];

                for (int i = 0; i < Fs.Length / 6; i++)
                {
                    byte[] btbuf = new byte[6];

                    Fs.Read(btbuf, 0, btbuf.Length);
                    if (i != index)
                    {
                        Fb.Write(btbuf, 0, btbuf.Length);
                    }
                }
                Fs.Close();
                Fb.Close();
                File.Delete("btdevices.dat");
                File.Move("btbuffor.dat", "btdevices.dat");
                File.Delete("btbuffor.dat");
                btassigned();
            }
        }

        private void exit_b_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void addbt_b_Click_1(object sender, EventArgs e)
        {
            if (lstDevices.SelectedIndex != -1)
            {
                int index = lstDevices.SelectedIndex;
                BluetoothDeviceInfo mac = array[index];
                FileStream Fs = new FileStream("btdevices.dat", FileMode.Append, FileAccess.Write);
                byte[] btsave = mac.DeviceAddress.ToByteArray();
                bool isinlist = lstremDevices.Items.IndexOf(mac.DeviceAddress) != -1;
                if (isinlist != true)
                {
                    Fs.Write(btsave, 0, 6);
                }
                Fs.Close();
                btassigned();
            }
        }

        private void save_options()
        {
            string connectionString;
           // string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM opcje", cn);
            DataTable table = new DataTable();
            da.Fill(table);
            da.UpdateCommand = new SqlCeCommand("UPDATE opcje SET transfer = ?, bflag = ?, ipflag = ?, ufile = ?, dfile = ?, skaner = ?, port = ?, serverpcm = ?, loginpcm = ?, passwdpcm = ?, bazapcm = ?, dbflag = ? WHERE id =  1", cn);
            da.UpdateCommand.Parameters.Add("@tf", SqlDbType.NVarChar, 50);
            da.UpdateCommand.Parameters.Add("@bf", SqlDbType.Bit);
            da.UpdateCommand.Parameters.Add("@if", SqlDbType.Bit); 
            da.UpdateCommand.Parameters.Add("@uf", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@df", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@sk", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@p", SqlDbType.Int);
            da.UpdateCommand.Parameters.Add("@sp", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@lp", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@hp", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@bp", SqlDbType.NVarChar, 120);
            da.UpdateCommand.Parameters.Add("@db", SqlDbType.Bit);
            da.UpdateCommand.Parameters["@tf"].Value = typ_t.Text;
            da.UpdateCommand.Parameters["@bf"].Value = runpicatchbt_cb.CheckState;
            da.UpdateCommand.Parameters["@if"].Value = runpicatchtcp_cb.CheckState;
            da.UpdateCommand.Parameters["@uf"].Value = ufile_t.Text;
            da.UpdateCommand.Parameters["@df"].Value = dfile_t.Text;
            da.UpdateCommand.Parameters["@sk"].Value = skaner_t.Text;
            da.UpdateCommand.Parameters["@p"].Value = int.Parse(port_t.Text);
            da.UpdateCommand.Parameters["@sp"].Value = server_t.Text;
            da.UpdateCommand.Parameters["@lp"].Value = login_t.Text;
            da.UpdateCommand.Parameters["@hp"].Value = haslo_t.Text;
            da.UpdateCommand.Parameters["@bp"].Value = baza_t.Text;
            da.UpdateCommand.Parameters["@db"].Value = checkBox1.CheckState;
            da.UpdateCommand.ExecuteNonQuery();
            cn.Close();
            this.Close();
        }

        private void save_b_Click(object sender, EventArgs e)
        {
            save_options();
            MessageBox.Show("Aby zmiany zastosować zmiany należy uruchomić ponownie program");
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            dfile_t.Text = fbd.SelectedPath;

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            ufile_t.Text = fbd.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dodajmagazyn();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            usunmagazyn();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            connect_pcm();
        }

       

    }
}
