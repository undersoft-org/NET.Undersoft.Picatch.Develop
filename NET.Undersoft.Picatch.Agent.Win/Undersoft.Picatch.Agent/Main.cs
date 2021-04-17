using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Globalization;
using Undersoft.Picatch.Agent.TCP;
using Undersoft.Picatch.Agent.BT;
using Undersoft.Picatch.Agent.DB;

namespace Undersoft.Picatch.Agent
{
    public partial class Main : Form
    {

        //OPCJE
        public string transfer;
        public string com;
        public string ip;
        public string ufile;
        public string dfile;
        public string bdll;
        public bool bflag;
        public bool ipflag;
        public bool dbflag;
        public int port;
        public string skaner;
        public string serverpcm;
        public string loginpcm;
        public string passwdpcm;
        public string bazapcm;
        private SqlConnection pcmn;

        public PICTCPAGENT frm2;
        public PICBTAGENT frm1;
        public PICDBCOM frm3;

        public Main()
        {
            InitializeComponent();
           
            Openbaza();
                 
           
            if (bflag == true)
            {
                PICBTAGENT formbt = new PICBTAGENT(this);
                formbt.Show();
            }
            if (ipflag == true)
            {
                PICTCPAGENT formtcp = new PICTCPAGENT(this);
                formtcp.Show();
            }

            if (dbflag == true)
            {
                PICDBCOM formdb = new PICDBCOM(this);
                formdb.Show();
            }
                      
            comboBox1.Text = "Nazwa";
            refreshdokgrid();
            refreshediheadimpgrid();
            refreshfediheadimpgrid();
            refreshswynikzlystan();
            refreshswynikzlacena();
            try
            {
                dataGridView1.Rows[0].Selected = true;
            }
            catch (Exception)
            {
            }

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;
            
            this.otwórzToolStripMenuItem.Click += otwórzToolStripMenuItem_Click;
            this.zamknijStripMenuItem.Click += zamknijStripMenuItem_Click;
           
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            if (m.Msg == WM_NCHITTEST)
                m.Result = (IntPtr)(HT_CAPTION);
        }

        private const int WM_NCHITTEST = 0x84;
        private const int HT_CLIENT = 0x1;
        private const int HT_CAPTION = 0x2;

       /*public void updatedata()
        {

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            string connectionString2;
            connectionString2 = "server=" + serverpcm + ";user id=" + loginpcm + ";password=" + passwdpcm + ";Trusted_Connection=no; database=" + bazapcm + ";connection timeout=30";
            pcmn = new SqlConnection(connectionString2);

            try
            {
                pcmn.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            string connectionString;

            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);

            cn.Open();

            string connectionString3;

            connectionString3= "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cnn = new SqlCeConnection(connectionString3);

            cnn.Open();

            SqlCeCommand cmd3 = cn.CreateCommand();
            cmd3.CommandText = "INSERT INTO dane (typ, kod, nazwa, stan, cenazk, cenasp, vat, datazmian) VALUES ('TOW', ?, ?, ?, ?, ?, ?, ?)";

            cmd3.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd3.Parameters.Add("@n", SqlDbType.NVarChar, 40);
            cmd3.Parameters.Add("@s", SqlDbType.NVarChar, 10);

            cmd3.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
            cmd3.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
            cmd3.Parameters.Add("@v", SqlDbType.NVarChar, 5);
            cmd3.Parameters.Add("@d", SqlDbType.DateTime);
            cmd3.Prepare();

            SqlCeCommand cmd4 = cn.CreateCommand();
            cmd4.CommandText = "INSERT INTO stany (kod, stan, MagId, Nazwa) VALUES (?, ?, ?, ?)";

            cmd4.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd4.Parameters.Add("@i", SqlDbType.Decimal, 10);
            cmd4.Parameters["@i"].Precision = 10;
            cmd4.Parameters["@i"].Scale = 3;
            cmd4.Parameters.Add("@s", SqlDbType.Int, 11);
            cmd4.Parameters.Add("@cz", SqlDbType.NVarChar, 10);

            cmd4.Prepare();



            SqlCeCommand cmd10 = cn.CreateCommand();
            cmd10.CommandText = "UPDATE dane SET nazwa = ?, stan = ?, cenazk = ?, cenasp = ?, vat = ?, datazmian = ? WHERE kod = ?";


            cmd10.Parameters.Add("@n", SqlDbType.NVarChar, 40);
            cmd10.Parameters.Add("@s", SqlDbType.NVarChar, 10);

            cmd10.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
            cmd10.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
            cmd10.Parameters.Add("@v", SqlDbType.NVarChar, 5);
            cmd10.Parameters.Add("@d", SqlDbType.DateTime);
            cmd10.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd10.Prepare();




            SqlCeCommand cmd6 = cn.CreateCommand();
            cmd6.CommandText = "UPDATE stany SET stan = ? WHERE (kod = ?) AND (MagId = ?)";


            cmd6.Parameters.Add("@i", SqlDbType.Decimal, 10);
            cmd6.Parameters["@i"].Precision = 10;
            cmd6.Parameters["@i"].Scale = 3;
            cmd6.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd6.Parameters.Add("@m", SqlDbType.Int, 11);


            cmd6.Prepare();

            SqlCeCommand cmdcheck = cn.CreateCommand();
            cmdcheck.CommandText = "SELECT count(*) FROM dane where kod = ?";
            cmdcheck.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmdcheck.Prepare();

            SqlCeCommand cmdchecks = cnn.CreateCommand();
            cmdchecks.CommandText = "SELECT count(*) FROM stany where kod = ? and MagId = ?";
            cmdchecks.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmdchecks.Parameters.Add("@m", SqlDbType.Int, 11);
            cmdchecks.Prepare();


              SqlCeCommand delete = cn.CreateCommand();
              delete.CommandText = "DROP TABLE dane";
              delete.Prepare();
              delete.ExecuteNonQuery();
              SqlCeCommand cmd2 = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(10,3), datazmian datetime)", cn);
              cmd2.ExecuteNonQuery();
              delete = cn.CreateCommand();
              delete.CommandText = "DROP TABLE stany";
              delete.Prepare();
              delete.ExecuteNonQuery();
              cmd2 = cn.CreateCommand();
              cmd2.CommandText = "CREATE TABLE stany (id int identity not null primary key, kod nvarchar(15), stan numeric(10,3), MagId int, Nazwa nvarchar(50))";
              cmd2.Prepare();
              cmd2.ExecuteNonQuery();

              

            SqlCommand cmd = pcmn.CreateCommand();
            cmd.CommandText = "SELECT     Towar.Kod, Towar.Nazwa, SUM(Istw.StanMag) AS Expr1, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100 AS CenaD, Towar.Stawka / 100 AS Vat, Towar.ZmianaIstotna FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId GROUP BY Towar.Kod, Towar.Nazwa, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100, Towar.Stawka / 100, Towar.ZmianaIstotna, Towar.Aktywny   HAVING     (Towar.Aktywny = 1) ";
            cmd.Prepare();
            SqlCommand cmd5 = pcmn.CreateCommand();
            cmd5.CommandText = "SELECT     KodDod.Kod, Towar.Nazwa, SUM(Istw.StanMag) AS Expr1, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100 AS CenaD, Towar.Stawka / 100 AS Vat, Towar.ZmianaIstotna FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId GROUP BY KodDod.Kod, Towar.Nazwa, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100, Towar.Stawka / 100, Towar.ZmianaIstotna, Towar.Aktywny HAVING     (Towar.Aktywny = 1)";
            cmd5.Prepare();

            SqlDataReader sdr = cmd.ExecuteReader();

            while (sdr.Read())
            {

                string kod = sdr.GetString(0);
                string nazwa = sdr.GetString(1);
                string stan = decimal.Round(sdr.GetDecimal(2), 2).ToString(nfi);
                string cenazk = decimal.Round(sdr.GetDecimal(3), 2).ToString(nfi);
                string cenasp = decimal.Round(sdr.GetDecimal(4), 2).ToString(nfi);
                string vat = Convert.ToString(sdr.GetInt32(5));
                DateTime datazmian = sdr.GetDateTime(6);
                try
                {
                    int count = 0;

                    cmdcheck.Parameters["@k"].Value = kod;
                    SqlCeDataReader cmdcheckdr = cmdcheck.ExecuteReader();

                    while (cmdcheckdr.Read())
                    {
                        count = cmdcheckdr.GetInt32(0);
                    }
                    cmdcheckdr.Close();
                    if (count > 0)
                    {

                        cmd10.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                        cmd10.Parameters["@s"].Value = stan;
                        cmd10.Parameters["@cz"].Value = cenazk;
                        cmd10.Parameters["@cs"].Value = cenasp;
                        cmd10.Parameters["@v"].Value = vat;
                        cmd10.Parameters["@d"].Value = datazmian;
                        cmd10.Parameters["@k"].Value = kod;
                        cmd10.ExecuteNonQuery();
                    }
                    else
                    {

                        cmd3.Parameters["@k"].Value = kod;
                        cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                        cmd3.Parameters["@s"].Value = stan;
                        cmd3.Parameters["@cz"].Value = cenazk;
                        cmd3.Parameters["@cs"].Value = cenasp;
                        cmd3.Parameters["@v"].Value = vat;
                        cmd3.Parameters["@d"].Value = datazmian;
                        cmd3.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            sdr.Close();
            sdr = cmd5.ExecuteReader();

            while (sdr.Read())
            {
                string kod = sdr.GetString(0);
                string nazwa = sdr.GetString(1);
                string stan = decimal.Round(sdr.GetDecimal(2), 2).ToString(nfi);
                string cenazk = decimal.Round(sdr.GetDecimal(3), 2).ToString(nfi);
                string cenasp = decimal.Round(sdr.GetDecimal(4), 2).ToString(nfi);
                string vat = Convert.ToString(sdr.GetInt32(5));
                DateTime datazmian = sdr.GetDateTime(6);
                try
                {
                    int count = 0;
                    cmdcheck.Parameters["@k"].Value = kod;
                    SqlCeDataReader cmdcheckdr = cmdcheck.ExecuteReader();

                    while (cmdcheckdr.Read())
                    {
                        count = cmdcheckdr.GetInt32(0);
                    }
                    cmdcheckdr.Close();
                    if (count > 0)
                    {

                        cmd10.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                        cmd10.Parameters["@s"].Value = stan;
                        cmd10.Parameters["@cz"].Value = cenazk;
                        cmd10.Parameters["@cs"].Value = cenasp;
                        cmd10.Parameters["@v"].Value = vat;
                        cmd10.Parameters["@d"].Value = datazmian;
                        cmd10.Parameters["@k"].Value = kod;
                        cmd10.ExecuteNonQuery();
                    }
                    else
                    {


                        cmd3.Parameters["@k"].Value = kod;
                        cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                        cmd3.Parameters["@s"].Value = stan;
                        cmd3.Parameters["@cz"].Value = cenazk;
                        cmd3.Parameters["@cs"].Value = cenasp;
                        cmd3.Parameters["@v"].Value = vat;
                        cmd3.Parameters["@d"].Value = datazmian;
                        cmd3.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            sdr.Close();
            SqlCeCommand cmd2 = cn.CreateCommand();
            cmd2.CommandText = "SELECT Nazwa, MagId FROM magazyn";
            cmd2.Prepare();
            SqlCommand cmd9 = pcmn.CreateCommand();
            cmd9.CommandText = "SELECT     Towar.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag AS Expr1 FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId WHERE     (Towar.Aktywny = 1) AND Istw.MagId = @mag ";
            cmd9.Parameters.Add("@mag", SqlDbType.Int);
            cmd9.Prepare();
            SqlCommand cmd8 = pcmn.CreateCommand();
            cmd8.CommandText = "SELECT     KodDod.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag AS Expr1 FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId WHERE     (Towar.Aktywny = 1)   AND Istw.MagId = @mag ";
            cmd8.Parameters.Add("@mag", SqlDbType.Int);
            cmd8.Prepare();

            SqlCeDataReader cedr = cmd2.ExecuteReader();

            while (cedr.Read())
            {
                string magazyn = cedr.GetString(0);
                int magid = cedr.GetInt32(1);
                cmd9.Parameters["@mag"].Value = magid;
                cmd8.Parameters["@mag"].Value = magid;


                SqlDataReader szdr = cmd9.ExecuteReader();
                while (szdr.Read())
                {

                    int count = 0;
                    cmdchecks.Parameters["@m"].Value = magid;
                    cmdchecks.Parameters["@k"].Value = szdr.GetString(0);

                    SqlCeDataReader cmdchecksdr = cmdchecks.ExecuteReader();
                    while (cmdchecksdr.Read())
                    {
                        count = cmdchecksdr.GetInt32(0);
                    }
                    cmdchecksdr.Close();
                    if (count > 0)
                    {

                        cmd6.Parameters["@i"].Value = szdr.GetDecimal(3);
                        cmd4.Parameters["@k"].Value = szdr.GetString(0);
                        cmd6.Parameters["@m"].Value = magid;
                        cmd6.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd4.Parameters["@k"].Value = szdr.GetString(0);
                        cmd4.Parameters["@i"].Value = szdr.GetDecimal(3);
                        cmd4.Parameters["@s"].Value = szdr.GetDecimal(2);
                        cmd4.Parameters["@cz"].Value = szdr.GetString(1);
                        cmd4.ExecuteNonQuery();
                    }


                }

                szdr.Close();
                szdr = cmd8.ExecuteReader();
                while (szdr.Read())
                {

                    int count = 0;
                    cmdchecks.Parameters["@m"].Value = magid;
                    cmdchecks.Parameters["@k"].Value = szdr.GetString(0);

                    SqlCeDataReader cmdchecksdr = cmdchecks.ExecuteReader();
                    while (cmdchecksdr.Read())
                    {
                        count = cmdchecksdr.GetInt32(0);
                    }
                    cmdchecksdr.Close();
                    if (count > 0)
                    {

                        cmd6.Parameters["@i"].Value = szdr.GetDecimal(3);
                        cmd4.Parameters["@k"].Value = szdr.GetString(0);
                        cmd6.Parameters["@m"].Value = magid;
                        cmd6.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd4.Parameters["@k"].Value = szdr.GetString(0);
                        cmd4.Parameters["@i"].Value = szdr.GetDecimal(3);
                        cmd4.Parameters["@s"].Value = szdr.GetDecimal(2);
                        cmd4.Parameters["@cz"].Value = szdr.GetString(1);
                        cmd4.ExecuteNonQuery();
                    }


                }
                szdr.Close();


            }



            cn.Close();
            cnn.Close();
            pcmn.Close();




        }   */




        public void updatedata()
        {

            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            string connectionString2;
            connectionString2 = "server=" + serverpcm + ";user id=" + loginpcm + ";password=" + passwdpcm + ";Trusted_Connection=no; database=" + bazapcm + ";connection timeout=30";
            pcmn = new SqlConnection(connectionString2);

            try
            {
                pcmn.Open();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            string connectionString;
           
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);

            cn.Open();

            SqlCeCommand cmd3 = cn.CreateCommand();
            cmd3.CommandText = "INSERT INTO dane (typ, kod, nazwa, stan, cenazk, cenasp, vat, datazmian, cenahurt, cenaoryg) VALUES ('TOW', ?, ?, ?, ?, ?, ?, ?, ?, ?)";
           
            cmd3.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd3.Parameters.Add("@n", SqlDbType.NVarChar, 40);
            cmd3.Parameters.Add("@s", SqlDbType.NVarChar, 10);
           
            cmd3.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
            cmd3.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
            cmd3.Parameters.Add("@v", SqlDbType.NVarChar, 5);
            cmd3.Parameters.Add("@d", SqlDbType.DateTime);
            cmd3.Parameters.Add("@ch", SqlDbType.NVarChar, 10);
            cmd3.Parameters.Add("@co", SqlDbType.NVarChar, 10);
            cmd3.Prepare();

            SqlCeCommand cmd4 = cn.CreateCommand();
            cmd4.CommandText = "INSERT INTO stany (kod, stan, MagId, Nazwa, datazmian) VALUES (?, ?, ?, ?, ?)";

            cmd4.Parameters.Add("@k", SqlDbType.NVarChar, 15);
            cmd4.Parameters.Add("@i", SqlDbType.Decimal, 10);
            cmd4.Parameters["@i"].Precision = 10;
            cmd4.Parameters["@i"].Scale = 3;
            cmd4.Parameters.Add("@s", SqlDbType.Int, 11);
            cmd4.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
            cmd4.Parameters.Add("@d", SqlDbType.DateTime);
            cmd4.Prepare();


            SqlCeCommand delete = cn.CreateCommand();
            delete.CommandText = "DROP TABLE dane";
            delete.Prepare();
            delete.ExecuteNonQuery();
            SqlCeCommand cmd2 = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar(15) not null, nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(6,3), datazmian datetime, cenahurt nvarchar(10), cenaoryg nvarchar(10))", cn);
            cmd2.ExecuteNonQuery();
            cmd2.CommandText = "CREATE INDEX kod ON dane (kod)";
            cmd2.ExecuteNonQuery();
            delete = cn.CreateCommand();

            delete.CommandText = "DROP TABLE stany";
            delete.Prepare();
            delete.ExecuteNonQuery();
            cmd2 = cn.CreateCommand();
            cmd2.CommandText = "CREATE TABLE stany (id int identity not null primary key, kod nvarchar(15), stan numeric(10,3), MagId int, Nazwa nvarchar(50), datazmian datetime)";
            cmd2.Prepare();
            cmd2.ExecuteNonQuery();
            cmd2 = new SqlCeCommand("CREATE INDEX kod ON stany (kod)", cn);   
            cmd2.ExecuteNonQuery();



            SqlCommand cmd = pcmn.CreateCommand();
            cmd.CommandText = "SELECT     Towar.Kod, Towar.Nazwa, SUM(Istw.StanMag) AS Expr1, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100 AS CenaD, Towar.Stawka / 100 AS Vat, Towar.Zmiana, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100 AS CenaH, Towar.Opis2 as CenaO FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId GROUP BY Towar.Kod, Towar.Nazwa, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100, Towar.Stawka / 100, Towar.Zmiana, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100, Towar.Aktywny, Towar.Opis2   HAVING     (Towar.Aktywny = 1) ";
            cmd.Prepare();
             SqlCommand cmd5 = pcmn.CreateCommand();
             cmd5.CommandText = "SELECT     KodDod.Kod, Towar.Nazwa, SUM(Istw.StanMag) AS Expr1, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100 AS CenaD, Towar.Stawka / 100 AS Vat, Towar.Zmiana, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100 AS CenaH, Towar.Opis2 as CenaO FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId GROUP BY KodDod.Kod, Towar.Nazwa, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100, Towar.Stawka / 100, Towar.Zmiana, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100, Towar.Aktywny, Towar.Opis2 HAVING     (Towar.Aktywny = 1)";
            cmd5.Prepare();

            SqlDataReader sdr = cmd.ExecuteReader();
            
            while (sdr.Read())
            {

                string kod = sdr.GetString(0);
                string nazwa = sdr.GetString(1);
                string stan = decimal.Round(sdr.GetDecimal(2), 2).ToString(nfi);
                string cenazk = decimal.Round(sdr.GetDecimal(3), 2).ToString(nfi);
                string cenasp = decimal.Round(sdr.GetDecimal(4), 2).ToString(nfi);
                string vat = Convert.ToString(sdr.GetInt32(5));
                DateTime datazmian = sdr.GetDateTime(6);
                string cenah = decimal.Round(sdr.GetDecimal(7), 2).ToString(nfi);
                string cenao = sdr.GetString(8);
                try
                {
                    cmd3.Parameters["@k"].Value = kod;
                    cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                    cmd3.Parameters["@s"].Value = stan;
                    cmd3.Parameters["@cz"].Value = cenazk;
                    cmd3.Parameters["@cs"].Value = cenasp;
                    cmd3.Parameters["@v"].Value = vat;
                    cmd3.Parameters["@d"].Value = datazmian;
                    cmd3.Parameters["@ch"].Value = cenah;
                    cmd3.Parameters["@co"].Value = cenao;
                    cmd3.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

            sdr.Close();
             sdr = cmd5.ExecuteReader();

            while (sdr.Read())
            {
                string kod = sdr.GetString(0);
                string nazwa = sdr.GetString(1);
                string stan = decimal.Round(sdr.GetDecimal(2), 2).ToString(nfi);
                string cenazk = decimal.Round(sdr.GetDecimal(3), 2).ToString(nfi);
                string cenasp = decimal.Round(sdr.GetDecimal(4), 2).ToString(nfi);
                string vat = Convert.ToString(sdr.GetInt32(5));
                DateTime datazmian = sdr.GetDateTime(6);
                string cenah = decimal.Round(sdr.GetDecimal(7), 2).ToString(nfi);
                try
                {
                    cmd3.Parameters["@k"].Value = kod;
                    cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                    cmd3.Parameters["@s"].Value = stan;
                    cmd3.Parameters["@cz"].Value = cenazk;
                    cmd3.Parameters["@cs"].Value = cenasp;
                    cmd3.Parameters["@v"].Value = vat;
                    cmd3.Parameters["@d"].Value = datazmian;
                    cmd3.Parameters["@ch"].Value = cenah;
                    cmd3.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            sdr.Close();

            cmd2.CommandText = "SELECT Nazwa, MagId FROM magazyn";
            cmd2.Prepare();
            SqlCommand cmd9 = pcmn.CreateCommand();
            cmd9.CommandText = "SELECT     Towar.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag AS Expr1, Towar.Zmiana  FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId WHERE     (Towar.Aktywny = 1) AND Istw.MagId = @mag ";
            cmd9.Parameters.Add("@mag", SqlDbType.Int);
            cmd9.Prepare();
            SqlCommand cmd8 = pcmn.CreateCommand();
            cmd8.CommandText = "SELECT     KodDod.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag AS Expr1, Towar.Zmiana FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId WHERE     (Towar.Aktywny = 1)   AND Istw.MagId = @mag ";
            cmd8.Parameters.Add("@mag", SqlDbType.Int);
            cmd8.Prepare();

            SqlCeDataReader cedr = cmd2.ExecuteReader();

            while (cedr.Read())
            {
                string magazyn = cedr.GetString(0);
                int magid = cedr.GetInt32(1);
                cmd9.Parameters["@mag"].Value = magid;
                cmd8.Parameters["@mag"].Value = magid;
                SqlDataReader szdr = cmd9.ExecuteReader();
                while (szdr.Read())
                {

                    cmd4.Parameters["@k"].Value = szdr.GetString(0);
                    cmd4.Parameters["@i"].Value = szdr.GetDecimal(3);
                    cmd4.Parameters["@s"].Value = szdr.GetDecimal(2);
                    cmd4.Parameters["@cz"].Value = szdr.GetString(1);
                    cmd4.Parameters["@d"].Value = szdr.GetDateTime(4);
                    cmd4.ExecuteNonQuery();

                               
                }

                szdr.Close();
                szdr = cmd8.ExecuteReader();
                while (szdr.Read())
                {

                    cmd4.Parameters["@k"].Value = szdr.GetString(0);
                    cmd4.Parameters["@i"].Value = szdr.GetDecimal(3);
                    cmd4.Parameters["@s"].Value = szdr.GetDecimal(2);
                    cmd4.Parameters["@cz"].Value = szdr.GetString(1);
                    cmd4.Parameters["@d"].Value = szdr.GetDateTime(4);
                    cmd4.ExecuteNonQuery();


                }
                szdr.Close();


            }

            SqlCeCommand updatedata = cn.CreateCommand();
            updatedata.CommandText = "UPDATE opcje SET datazmian = ? WHERE id = 1";
            updatedata.Parameters.Add("@data", SqlDbType.DateTime);
            updatedata.Prepare();
            updatedata.Parameters["@data"].Value = DateTime.Now;
            updatedata.ExecuteNonQuery();

            cn.Close();
            pcmn.Close();









        }

        private void refreshediheadimpgrid()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("Select * From edihead", cn);
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            da.Fill(table);
            //if (table.Rows.Count != 0)
            //{
                dataGridView3.DataSource = table.DefaultView;
               
            //}
            cn.Close();
        }

        private void refreshfediheadimpgrid()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter dc = new SqlCeDataAdapter("Select * From fedihead", cn);
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            dc.Fill(table);
            //if (table.Rows.Count != 0)
            //{
                dataGridView8.DataSource = table.DefaultView;

            //}
            cn.Close();
        }
        
        private void refreshedibodyimpgrid()
        {
            
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();
                    SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
                    DataTable table2 = new DataTable();
                    table2.Locale = CultureInfo.CurrentCulture;
                    db.Fill(table2);
                    if (table2.Rows.Count > 0)
                    {
                        table2.Clear();
                        int selectedindex = dataGridView3.SelectedCells[0].RowIndex;
                        DataGridViewRow selectedRow = dataGridView3.Rows[selectedindex];
                        string index = Convert.ToString(selectedRow.Cells[4].Value);
                        db.SelectCommand = new SqlCeCommand("SELECT * FROM edibody WHERE NrDok =  ?", cn);
                        db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);
                        db.SelectCommand.Parameters["@k"].Value = index;
                        db.SelectCommand.ExecuteNonQuery();
                    }
                    db.Fill(table2);
                    
                    dataGridView4.DataSource = table2.DefaultView;
                    cn.Close();
                
         }

        private void refreshfedibodyimpgrid()
        {
            
                string connectionString;
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
                DataTable table2 = new DataTable();
                table2.Locale = CultureInfo.CurrentCulture;
                db.Fill(table2);
                if (table2.Rows.Count > 0)
                {
                    table2.Clear();
                    int selectedindex = dataGridView8.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView8.Rows[selectedindex];
                    string index = Convert.ToString(selectedRow.Cells[4].Value);
                    db.SelectCommand = new SqlCeCommand("SELECT * FROM fedibody WHERE NrDok =  ?", cn);
                    db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);
                    db.SelectCommand.Parameters["@k"].Value = index;
                    db.SelectCommand.ExecuteNonQuery();
                }
                db.Fill(table2);
                dataGridView7.DataSource = table2.DefaultView;
                cn.Close();

          
        }

      /*  private void refreshediendimpgrid()
        {
            try
            {
                int selectedindex = dataGridView3.SelectedCells[0].RowIndex;
               
                    DataGridViewRow selectedRow = dataGridView3.Rows[selectedindex];

                    string index = Convert.ToString(selectedRow.Cells[4].Value);
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();
                    SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
                    DataTable table2 = new DataTable();
                    table2.Locale = CultureInfo.CurrentCulture;
                    db.SelectCommand = new SqlCeCommand("SELECT * FROM ediend WHERE NrDok =  ?", cn);
                    db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);
                    db.SelectCommand.Parameters["@k"].Value = index;
                    db.SelectCommand.ExecuteNonQuery();
                    db.Fill(table2);
                    dataGridView5.DataSource = table2.DefaultView;

                    cn.Close();
            }
            catch (Exception)
            {
            }
            
        }*/

        private void refreshswynikzlystan()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("Select kod, nazwa, stan, zliczono, cenazk, cenasp From dane Where bad_stan = 1", cn);
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            da.Fill(table);
            // if (table.Rows.Count != 0)
            //{
            dataGridView9.DataSource = table.DefaultView;
            //}
            cn.Close();
        }

        private void refreshswynikzlacena()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("Select kod, nazwa, cenasp, cenapolka, vat, stan From dane Where bad_cena = 1", cn);
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            da.Fill(table);
            // if (table.Rows.Count != 0)
            //{
            dataGridView10.DataSource = table.DefaultView;
            //}
            cn.Close();
        }

        private void refreshdokgrid()
        {
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeDataAdapter da = new SqlCeDataAdapter("Select * From dok", cn);
            DataTable table = new DataTable();
            table.Locale = CultureInfo.CurrentCulture;
            da.Fill(table);
           // if (table.Rows.Count != 0)
            //{
                dataGridView1.DataSource = table.DefaultView;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[2].Width = 30;
            //}
            cn.Close();
        }

        private void refreshpozgrid()
        {
            try
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                if (selectedindex != 0)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];

                    string index = Convert.ToString(selectedRow.Cells[0].Value);
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();
                    SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
                    DataTable table2 = new DataTable();
                    table2.Locale = CultureInfo.CurrentCulture;
                    db.SelectCommand = new SqlCeCommand("SELECT * FROM bufor WHERE dokid =  ?", cn);
                    db.SelectCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                    db.SelectCommand.Parameters["@k"].Value = int.Parse(index);
                    db.SelectCommand.ExecuteNonQuery();
                    db.Fill(table2);
                    dataGridView2.DataSource = table2.DefaultView;
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].Visible = false;
                    dataGridView2.Columns[2].Width = 80;
                    dataGridView2.Columns[3].Width = 230;
                    dataGridView2.Columns[4].Width = 50;
                    dataGridView2.Columns[5].Width = 50;
                    dataGridView2.Columns[6].Width = 50;
                    dataGridView2.Columns[7].Width = 50;
                    dataGridView2.Columns[8].Width = 30;
                    cn.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public void senddoc()
        {
            StreamWriter sw = new StreamWriter("picatch.imp");
            int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];

            string index = Convert.ToString(selectedRow.Cells[0].Value);
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat  FROM bufor WHERE dokid = ?";
            cmd.Parameters.Add("@d", SqlDbType.Int);
            cmd.Prepare();
            cmd.Parameters["@d"].Value = int.Parse(index);
            SqlCeDataReader dr = cmd.ExecuteReader();
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            while (dr.Read())
            {
                decimal n = dr.GetDecimal(4);
                int did = dr.GetInt32(0);
                sw.WriteLine(Convert.ToString(did) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + n.ToString(nfi) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetString(7));
            }

            sw.Close();
            cn.Close();
        }

        public void senddocall()
        {
            StreamWriter sw = new StreamWriter("picatch.imp");
            int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
            DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];

            string index = Convert.ToString(selectedRow.Cells[0].Value);
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat  FROM bufor";
           
            SqlCeDataReader dr = cmd.ExecuteReader();
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            while (dr.Read())
            {
                decimal n = dr.GetDecimal(4);
                int did = dr.GetInt32(0);
                sw.WriteLine(Convert.ToString(did) + ";" + dr.GetString(1) + ";" + dr.GetString(2) + ";" + dr.GetString(3) + ";" + n.ToString(nfi) + ";" + dr.GetString(5) + ";" + dr.GetString(6) + ";" + dr.GetString(7));
            }

            sw.Close();
            cn.Close();
        }
        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used
        bool mLoadFired;        // Form was shown once

    /*    protected override void SetVisibleCore(bool value)
        {
            if (!mAllowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }
        */
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!mAllowClose)
            {
                this.Hide();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowVisible = true;
            mLoadFired = true;
            Show();
        }

        private void zamknijStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowClose = mAllowVisible = true;
            if (!mLoadFired) Show();
            Close();
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
                DataTable table2 = new DataTable();
                db.SelectCommand = new SqlCeCommand("SELECT kod, nazwa, stan, cenazk, cenahurt, cenasp, cenaoryg FROM dane WHERE kod =  ?", cn);
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
               
                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM dane", cn);
                DataTable table2 = new DataTable();
                db.SelectCommand = new SqlCeCommand("SELECT kod, nazwa, stan, cenazk, cenasp, cenahurt, cenaoryg, vat FROM dane WHERE " + like, cn);
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

        private void deleteedihead()
        {
            string connectionString;
            if (dataGridView3.RowCount > 0)
            {
                int selectedindex = dataGridView3.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView3.Rows[selectedindex];
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                string index = Convert.ToString(selectedRow.Cells[0].Value);
                string nrdok = Convert.ToString(selectedRow.Cells[4].Value);

                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
                da.DeleteCommand = new SqlCeCommand("DELETE FROM edibody WHERE NrDok =  ?", cn);
                da.DeleteCommand.Parameters.Add("@k", SqlDbType.NVarChar, 100);
                da.DeleteCommand.Parameters["@k"].Value = nrdok;
                da.DeleteCommand.ExecuteNonQuery();

                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edihead", cn);
                db.DeleteCommand = new SqlCeCommand("DELETE FROM edihead WHERE id =  ?", cn);
                db.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                db.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                db.DeleteCommand.ExecuteNonQuery();

                SqlCeDataAdapter dc = new SqlCeDataAdapter("SELECT * FROM ediend", cn);
                dc.DeleteCommand = new SqlCeCommand("DELETE FROM ediend WHERE NrDok =  ?", cn);
                dc.DeleteCommand.Parameters.Add("@k", SqlDbType.NVarChar, 100);
                dc.DeleteCommand.Parameters["@k"].Value = nrdok;
                dc.DeleteCommand.ExecuteNonQuery();

                cn.Close();
                refreshediheadimpgrid();
                refreshedibodyimpgrid();
            }
        }

        private void deleteedibody()
        {
            string connectionString;
            if (dataGridView4.RowCount > 0)
            {
                int selectedindex = dataGridView4.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView4.Rows[selectedindex];
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                string index = Convert.ToString(selectedRow.Cells[0].Value);


                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
                da.DeleteCommand = new SqlCeCommand("DELETE FROM edibody WHERE id =  ?", cn);
                da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                da.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                da.DeleteCommand.ExecuteNonQuery();
                cn.Close();
                refreshedibodyimpgrid();
            }
        }

        private void deletefedihead()
        {
            string connectionString;
            if (dataGridView8.RowCount > 0)
            {
                int selectedindex = dataGridView8.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView8.Rows[selectedindex];
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                string index = Convert.ToString(selectedRow.Cells[0].Value);
                string nrdok = Convert.ToString(selectedRow.Cells[4].Value);

                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
                da.DeleteCommand = new SqlCeCommand("DELETE FROM fedibody WHERE NrDok =  ?", cn);
                da.DeleteCommand.Parameters.Add("@k", SqlDbType.NVarChar, 100);
                da.DeleteCommand.Parameters["@k"].Value = nrdok;
                da.DeleteCommand.ExecuteNonQuery();

                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM fedihead", cn);
                db.DeleteCommand = new SqlCeCommand("DELETE FROM fedihead WHERE id =  ?", cn);
                db.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                db.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                db.DeleteCommand.ExecuteNonQuery();

                SqlCeDataAdapter dc = new SqlCeDataAdapter("SELECT * FROM fediend", cn);
                dc.DeleteCommand = new SqlCeCommand("DELETE FROM fediend WHERE NrDok =  ?", cn);
                dc.DeleteCommand.Parameters.Add("@k", SqlDbType.NVarChar, 100);
                dc.DeleteCommand.Parameters["@k"].Value = nrdok;
                dc.DeleteCommand.ExecuteNonQuery();

                cn.Close();
                refreshfediheadimpgrid();
                refreshfedibodyimpgrid();
            }
        }

        private void deletefedibody()
        {
            string connectionString;
            if (dataGridView7.RowCount > 0)
            {
                int selectedindex = dataGridView7.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView7.Rows[selectedindex];
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                string index = Convert.ToString(selectedRow.Cells[0].Value);


                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
                da.DeleteCommand = new SqlCeCommand("DELETE FROM fedibody WHERE id =  ?", cn);
                da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                da.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                da.DeleteCommand.ExecuteNonQuery();
                cn.Close();
                refreshfedibodyimpgrid();
            }
        }
        
        
        private void deletedok()
        {
            string connectionString;
            if (dataGridView1.RowCount > 0)
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                string index = Convert.ToString(selectedRow.Cells[0].Value);


                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
                da.DeleteCommand = new SqlCeCommand("DELETE FROM bufor WHERE dokid =  ?", cn);
                da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                da.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                da.DeleteCommand.ExecuteNonQuery();
                SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM dok", cn);

                db.DeleteCommand = new SqlCeCommand("DELETE FROM dok WHERE id =  ?", cn);
                db.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                db.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                db.DeleteCommand.ExecuteNonQuery();
                cn.Close();
                refreshdokgrid();
                refreshpozgrid();
            }
        }

        private void deleterow()
        {
            string connectionString;
            if (dataGridView2.RowCount > 0)
            {
                int selectedindex = dataGridView2.SelectedCells[0].RowIndex;
                DataGridViewRow selectedRow = dataGridView2.Rows[selectedindex];
                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);
                cn.Open();
                string index = Convert.ToString(selectedRow.Cells[0].Value);


                SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM bufor", cn);
                da.DeleteCommand = new SqlCeCommand("DELETE FROM bufor WHERE id =  ?", cn);
                da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);
                da.DeleteCommand.Parameters["@k"].Value = int.Parse(index);
                da.DeleteCommand.ExecuteNonQuery();
                cn.Close();
                refreshpozgrid();
            }
        }
        
        
        public void Openbaza()
        {
            string connectionString;
            string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection dcn = new SqlCeConnection(connectionString);

            if (File.Exists(fileName) == false)
            {

                SqlCeEngine engine = new SqlCeEngine(connectionString);
                engine.CreateDatabase();
                dcn.Open();
                SqlCeCommand cmd = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar(15) not null, nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(6,3), datazmian datetime, cenahurt nvarchar(10), cenaoryg nvarchar(10))", dcn);
                cmd.ExecuteNonQuery();
                cmd.CommandText = "CREATE INDEX kod ON dane (kod)";
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE bufor (id int identity not null primary key, dokid int, kod nvarchar (15) not null, nazwa nvarchar (100), cenazk nvarchar(10), ilosc numeric(10,3), stan nvarchar(10), cenasp nvarchar(10), vat nvarchar(10))", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE INDEX dokid ON bufor (dokid)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE INDEX kod ON bufor (kod)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE bufordane (typ nvarchar (7), kod nvarchar (15) not null primary key, nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(10,3), datazmian datetime, cenahurt nvarchar(10), cenaoryg nvarchar(10))", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE buforstany (id int identity not null primary key, kod nvarchar(15), stan numeric(10,3), MagId int, Nazwa nvarchar(50), datazmian datetime)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE dok (id int identity not null primary key, nazwadok nvarchar (120), typ nvarchar(10), data datetime, device nvarchar(20))", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE opcje (id int identity not null primary key, transfer nvarchar (20), com nvarchar(20), ip nvarchar(20), ufile nvarchar (120), dfile nvarchar(120), bdll nvarchar(50), bflag bit, ipflag bit, port int, skaner nvarchar(120), serverpcm nvarchar (20), loginpcm nvarchar(20), passwdpcm nvarchar(20), bazapcm nvarchar(20), dbflag bit, datazmian datetime)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("INSERT INTO opcje (transfer, com, ip, ufile, dfile, bdll, bflag, ipflag, port, skaner, serverpcm, loginpcm, passwdpcm, bazapcm, dbflag, datazmian) VALUES ('ZAM_ODB', 'COM5', '0.0.0.0', 'C:\\PICEDI\\EKSPORT', 'C:\\PICEDI\\IMPORT', 'MSStack', 0, -1, 8790, 'PC-MARKET', '10.0.0.10\\sqlexpress', 'sa', '$t0kkk3', 'stokki', 0, '1900-01-01 00:00:00')", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE edihead (id int identity not null primary key, FileName nvarchar(40), TypPolskichLiter nvarchar(20), TypDok nvarchar (20), NrDok nvarchar(30), Data nvarchar(30), DataRealizacji nvarchar (30), Magazyn nvarchar(30), SposobPlatn nvarchar(10), TerminPlatn  nvarchar(10), IndeksCentralny nvarchar(10), NazwaWystawcy  nvarchar(120), AdresWystawcy  nvarchar(120), KodWystawcy nvarchar(120), MiastoWystawcy nvarchar(120), UlicaWystawcy nvarchar(120), NIPWystawcy nvarchar(120), BankWystawcy nvarchar(120), KontoWystawcy nvarchar(120), TelefonWystawcy nvarchar(30), NrWystawcyWSieciSklepow nvarchar(20), NazwaOdbiorcy nvarchar(120), AdresOdbiorcy nvarchar(120), KodOdbiorcy nvarchar(20), MiastoOdbiorcy nvarchar(120), UlicaOdbiorcy nvarchar(120), NIPOdbiorcy nvarchar(120), BankOdbiorcy nvarchar(120), KontoOdbiorcy nvarchar(120), TelefonOdbiorcy nvarchar(120), NrOdbiorcyWSieciSklepow nvarchar(20), DoZaplaty nvarchar(20), status nvarchar(20), complete bit)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE edibody (id int identity not null primary key, NrDok nvarchar(30), Nazwa nvarchar (120), kod nvarchar(20), Vat nvarchar(7), Jm nvarchar (7), Asortyment nvarchar(120), Sww nvarchar(30), PKWiU nvarchar(30), Ilosc nvarchar(10), Cena nvarchar(10), Wartosc nvarchar(10), IleWOpak nvarchar(10), CenaSp nvarchar(10), status nvarchar(20), complete bit)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE ediend (id int identity not null primary key, NrDok nvarchar(30), Vat nvarchar (20), SumaNet nvarchar(20), SumaVat nvarchar(20), status nvarchar(20), complete bit)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fedihead (id int identity not null primary key, FileName nvarchar(40), TypPolskichLiter nvarchar(20), TypDok nvarchar (20), NrDok nvarchar(30), Data nvarchar(30), DataRealizacji nvarchar (30), Magazyn nvarchar(30), SposobPlatn nvarchar(10), TerminPlatn  nvarchar(10), IndeksCentralny nvarchar(10), NazwaWystawcy  nvarchar(120), AdresWystawcy  nvarchar(120), KodWystawcy nvarchar(120), MiastoWystawcy nvarchar(120), UlicaWystawcy nvarchar(120), NIPWystawcy nvarchar(120), BankWystawcy nvarchar(120), KontoWystawcy nvarchar(120), TelefonWystawcy nvarchar(30), NrWystawcyWSieciSklepow nvarchar(20), NazwaOdbiorcy nvarchar(120), AdresOdbiorcy nvarchar(120), KodOdbiorcy nvarchar(20), MiastoOdbiorcy nvarchar(120), UlicaOdbiorcy nvarchar(120), NIPOdbiorcy nvarchar(120), BankOdbiorcy nvarchar(120), KontoOdbiorcy nvarchar(120), TelefonOdbiorcy nvarchar(120), NrOdbiorcyWSieciSklepow nvarchar(20), DoZaplaty nvarchar(20), status nvarchar (20), complete bit, device nvarchar(20))", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fedibody (id int identity not null primary key, NrDok nvarchar(30), Nazwa nvarchar (120), kod nvarchar(20), Vat nvarchar(7), Jm nvarchar (7), Asortyment nvarchar(120), Sww nvarchar(30), PKWiU nvarchar(30), Ilosc nvarchar(10), Cena nvarchar(10), Wartosc nvarchar(10), IleWOpak nvarchar(10), CenaSp nvarchar(10), status nvarchar, complete bit)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fediend (id int identity not null primary key, NrDok nvarchar(30), Vat nvarchar (20), SumaNet nvarchar(20), SumaVat nvarchar(20), status nvarchar(20), complete bit)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE devdane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(100), stan nvarchar(10), cenazk nvarchar(20), cenasp nvarchar(20), vat nvarchar(5), device nvarchar(20), devstat nvarchar(10))", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE wynikispr (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(100), stan nvarchar(10), cenazk nvarchar(20), cenasp nvarchar(20), vat nvarchar(5), devstat nvarchar(20), bad_cena bit, bad_stan bit, cenapolka numeric(10,3), zliczono numeric(10,3))", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE magazyn (id int identity not null primary key, Nazwa nvarchar (20), MagId int)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE INDEX MagId ON magazyn (MagId)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE stany (id int identity not null primary key, kod nvarchar(15), stan numeric(10,3), MagId int, Nazwa nvarchar(50), datazmian DateTime)", dcn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE INDEX kod ON stany (kod)", dcn);
                cmd.ExecuteNonQuery();
                dcn.Close();

            }
            try
            {
              
                dcn.Open();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
                SqlCeCommand cmd2 = dcn.CreateCommand();
                cmd2.CommandText = "SELECT * FROM opcje WHERE id = 1";
                cmd2.Prepare();
                SqlCeDataReader dr = cmd2.ExecuteReader();
           
            while (dr.Read())
            {
                transfer = dr.GetString(1);
                com = dr.GetString(2);
                ip = dr.GetString(3);
                ufile = dr.GetString(4);
                dfile = dr.GetString(5);
                bdll = dr.GetString(6);
                bflag = dr.GetBoolean(7);
                ipflag = dr.GetBoolean(8);
                port = dr.GetInt32(9);
                skaner = dr.GetString(10);
                serverpcm = dr.GetString(11);
                loginpcm = dr.GetString(12);
                passwdpcm = dr.GetString(13);
                bazapcm = dr.GetString(14);
                dbflag = dr.GetBoolean(15);
             
               
            }
            dr.Close();
            
            DataGridTableStyle ts = new DataGridTableStyle();
            SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM dane", dcn);
            DataTable table2 = new DataTable();
            db.SelectCommand = new SqlCeCommand("SELECT kod, nazwa, stan, cenazk, cenasp, cenahurt, cenaoryg, vat FROM dane", dcn);
            db.SelectCommand.ExecuteNonQuery();
            db.Fill(table2);
            
          if (table2.Rows.Count != 0)
          {
            dataGrid1.DataSource = table2.DefaultView;
          }
          dcn.Close();
            
            

          
            
            
           
        }

        private void search_b_Click(object sender, EventArgs e)
        {
            FindIndex();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PICBTAGENT frm1 = new PICBTAGENT(this);
            frm1.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            PICTCPAGENT frm2 = new PICTCPAGENT(this);
            frm2.Show();
        }

        
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int selectedindex = dataGridView1.SelectedCells[0].RowIndex;
                if (selectedindex != -1)
                {
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedindex];

                    string index = Convert.ToString(selectedRow.Cells[0].Value);
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
                    dataGridView2.DataSource = table2.DefaultView;
                    dataGridView2.Columns[0].Visible = false;
                    dataGridView2.Columns[1].Visible = false;
                    dataGridView2.Columns[2].Width = 80;
                    dataGridView2.Columns[3].Width = 230;
                    dataGridView2.Columns[4].Width = 50;
                    dataGridView2.Columns[5].Width = 50;
                    dataGridView2.Columns[6].Width = 50;
                    dataGridView2.Columns[7].Width = 50;
                    dataGridView2.Columns[8].Width = 30;
                    cn.Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Brak dokumentów");
            }
        }

        private void uRUCHOMAGENTABLUETOOTHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICBTAGENT frm1 = new PICBTAGENT(this);
            frm1.Show();
        }

        private void uRUCHOMAGENTATCPIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICTCPAGENT frm2 = new PICTCPAGENT(this);
            frm2.Show();
        }

        private void zATRZYMAJAGENTABLUETOOTHToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICBTAGENT frm3 = (PICBTAGENT)Application.OpenForms["PICBTAGENT"];
            frm3.Close();
        }

        private void zATRZYMAJAGENTATCPIPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICTCPAGENT frm4 = (PICTCPAGENT)Application.OpenForms["PICTCPAGENT"];
            frm4.Close();
        }

        private void oPCJEAGENTAToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options frm3 = new Options();
            frm3.Show();
        }

        private void exit_mt_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            this.Close();
        }

        private void uruchomAgentaBluetoothToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frm1 = new PICBTAGENT(this);
            frm1.Show();
        }

        private void uruchomAgentaTCPIPToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frm2 = new PICTCPAGENT(this);
            frm2.Show();
        }

        private void zatrzymajAgentaBluetoothToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frm1 = (PICBTAGENT)Application.OpenForms["PICBTAGENT"];

            frm1.Close();
        }

        private void zatrzymajAgentaTCPIPToolStripMenuItem1_Click(object sender, EventArgs e)
        {
           // PICTCPAGENT frm2 = (PICTCPAGENT)Application.OpenForms["PICTCPAGENT"];
            frm2.Close();
        }

        private void ukryjStripMenuItem_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }

        private void usuńToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deletedok();
        }

        private void usuńToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            deleterow();
        }

        private void wyślijToolStripMenuItem_Click(object sender, EventArgs e)
        {
            senddoc();
        }

        private void refresh_b_Click(object sender, EventArgs e)
        {
            Openbaza();
            refreshdokgrid();

           
            refreshpozgrid();
        }

        private void refresh1_b_Click(object sender, EventArgs e)
        {
            Openbaza();
            refreshdokgrid();

           
            refreshpozgrid();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            senddoc();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            PICATCHEDI frm5 = new PICATCHEDI(0, null);
            frm5.Show();
        }

        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            refreshedibodyimpgrid();
          //  refreshediendimpgrid();
        }

        private void refreshedi_b_Click(object sender, EventArgs e)
        {
            refreshediheadimpgrid();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            refreshfediheadimpgrid();
        }

        private void dataGridView8_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            refreshfedibodyimpgrid();
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView8.RowCount > 0)
            {
                PICATCHEDI frm5 = new PICATCHEDI(1, dataGridView8.Rows[dataGridView8.SelectedCells[0].RowIndex]);
                frm5.Show();
            }
        }

        private void usuńToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            deleteedihead();
        }

        private void odbierzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PICATCHEDI frm5 = new PICATCHEDI(0, null);
            frm5.Show();
        }

        private void usuńToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            deleteedibody();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            deletefedihead();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            PICATCHEDI frm5 = new PICATCHEDI(1, dataGridView8.Rows[dataGridView8.SelectedCells[0].RowIndex]);
            frm5.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            deletefedibody();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            refreshswynikzlystan();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            refreshswynikzlacena();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (dataGridView9.RowCount > 0)
            {
                PICATCHEDI frm5 = new PICATCHEDI(2, null);
                frm5.Show();
            }
        }

        private void otwórzToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            refreshdokgrid();
            refreshediheadimpgrid();
            refreshfediheadimpgrid();
            refreshswynikzlystan();
            refreshswynikzlacena();
        }

        private void oDŚWIEŻWSZYSTKOToolStripMenuItem_Click(object sender, EventArgs e)
        {
            refreshdokgrid();
            refreshediheadimpgrid();
            refreshfediheadimpgrid();
            refreshswynikzlystan();
            refreshswynikzlacena();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            PICATCHEDI frm5 = new PICATCHEDI(3, null);
            frm5.Show();
        }

        private void aKTUALIZUJBAZĘTOWARÓWToolStripMenuItem_Click(object sender, EventArgs e)
        {
            updatedata();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            senddocall();
        }

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void xToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   

        private void agentWymianyDanychToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frm3 = new PICDBCOM(this);
            frm3.Show();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            if (dbflag == true)
            {
                MessageBox.Show("Aby wymienić bazę, należy wyłączyć w opcjach autostart wymiany i zrestartować program");
            }
            else
            {

                updatedata();
            }
        }

        private void detalicznyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STOCKCHECK stcheck = new STOCKCHECK(0);
            stcheck.Show();
        }

        private void hurtowyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            STOCKCHECK stcheck = new STOCKCHECK(1);
            stcheck.Show();
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            mAllowClose = true;
            this.Close();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
             this.WindowState = FormWindowState.Minimized;
        }

        

       
        

       

        

       
       
        

       

        

        

        

    }
}
