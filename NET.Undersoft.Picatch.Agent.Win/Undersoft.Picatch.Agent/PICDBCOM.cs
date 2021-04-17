using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.Globalization;

namespace Undersoft.Picatch.Agent.DB
{
    public partial class PICDBCOM : Form
    {

        public string transfer;
        public string com;
        public string ip;
        public string ufile;
        public string dfile;
        public string bdll;
        public bool bflag;
        public bool ipflag;
        public int port;
        public string skaner;
        public string serverpcm;
        public string loginpcm;
        public string passwdpcm;
        public string bazapcm;
        private SqlConnection pcmn;
        // bool czyuruchom = true;
        private Undersoft.Picatch.Agent.Main main;
        private bool setexpstat;
        public DateTime datazmian;
        public DateTime lastimport;
 BackgroundWorker bw = new BackgroundWorker();
     bool running = true;
      

        public PICDBCOM(Undersoft.Picatch.Agent.Main form)
        {
           
            
            InitializeComponent();

            string connectionString;
            // string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmd2 = cn.CreateCommand();
            cmd2.CommandText = "SELECT * FROM opcje WHERE id = 1";
            cmd2.Prepare();
            SqlCeDataReader dr = cmd2.ExecuteReader();



            while (dr.Read())
            {


                // ip_t.Text = dr.GetString(3);
                // ufile_t.Text = dr.GetString(4);
                // dfile_t.Text = dr.GetString(5);

                //bflag = dr.GetBoolean(7);
                //ipflag = dr.GetBoolean(8);
                serverpcm = dr.GetString(11);
                loginpcm = dr.GetString(12);
                passwdpcm = dr.GetString(13);
                bazapcm = dr.GetString(14);
                //skaner_t.Text = dr.GetString(10);
                datazmian = dr.GetDateTime(16);
            }
            cn.Close();


            
            
                bw.WorkerReportsProgress = true;
                bw.WorkerSupportsCancellation = true;
            


            main = form;
            bw.DoWork += (bwSender, bwArg) =>
            {
                //what happens here must not touch the form
                //as it's in a different thread        
            //    downloaddata(main);
                
                 updatedata(main);

            };
            bw.RunWorkerAsync();

            notifyIcon1.ContextMenuStrip = contextMenuStrip1;

            this.otwórzToolStripMenuItem.Click += otwórzToolStripMenuItem_Click;
            this.zakończToolStripMenuItem.Click += zakończToolStripMenuItem_Click;


        }


        public void checklastimport()
        {

            string connectionString;

            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);

            cn.Open();

            try
            {
                cn.Open();
              

            }

            catch (Exception ex)
            {
                
                cn.Close();
                Invoke((MethodInvoker)(() => exkom_t.Text += "Błąd połączenia z bazą\n"));
            }

            SqlCeCommand lastdatecmd = cn.CreateCommand();
            lastdatecmd.CommandText = "SELECT     MAX(datazmian) AS lastdate FROM   dane ";
            lastdatecmd.Prepare();

            SqlCeDataReader lastdatedr = lastdatecmd.ExecuteReader();
            while (lastdatedr.Read())
            {
                if (!lastdatedr.IsDBNull(0))
                {
                    lastimport = lastdatedr.GetDateTime(0);
                }
                else
                {
                    lastimport = Convert.ToDateTime("1900-01-01 00:00:00");
                }
            }
            cn.Close();
        }

        public void downloaddata()
        {


            try
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
                    Invoke((MethodInvoker)(() => exkom_t.Text += "Błąd połączenia z bazą\n"));
                }

                string connectionString;

                connectionString = "DataSource=Baza.sdf; Password=matrix1";
                SqlCeConnection cn = new SqlCeConnection(connectionString);

                cn.Open();


                SqlCeCommand lastdatecmd = cn.CreateCommand();
                lastdatecmd.CommandText = "SELECT     MAX(datazmian) AS lastdate FROM   dane ";
                lastdatecmd.Prepare();

                SqlCeDataReader lastdatedr = lastdatecmd.ExecuteReader();
                while (lastdatedr.Read())
                {
                    if (!lastdatedr.IsDBNull(0))
                    {
                        lastimport = lastdatedr.GetDateTime(0);
                    }
                    else
                    {
                        lastimport = Convert.ToDateTime("1900-01-01 00:00:00");
                    }
                }



                SqlCeCommand cmd3 = cn.CreateCommand();
                cmd3.CommandText = "INSERT INTO bufordane (typ, kod, nazwa, stan, cenazk, cenasp, vat, datazmian, cenahurt, cenaoryg) VALUES ('TOW', ?, ?, ?, ?, ?, ?, ?, ?, ?)";

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
                cmd4.CommandText = "INSERT INTO buforstany (kod, stan, MagId, Nazwa, datazmian) VALUES (?, ?, ?, ?, ?)";

                cmd4.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                cmd4.Parameters.Add("@i", SqlDbType.Decimal, 10);
                cmd4.Parameters["@i"].Precision = 10;
                cmd4.Parameters["@i"].Scale = 3;
                cmd4.Parameters.Add("@s", SqlDbType.Int, 11);
                cmd4.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                cmd4.Parameters.Add("@d", SqlDbType.DateTime);

                cmd4.Prepare();


                SqlCeCommand delete = cn.CreateCommand();
                delete.CommandText = "DROP TABLE bufordane";
                delete.Prepare();
                delete.ExecuteNonQuery();
                SqlCeCommand cmd2 = new SqlCeCommand("CREATE TABLE bufordane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(10,3), datazmian datetime, cenahurt nvarchar(10), cenaoryg nvarchar(10))", cn);
                cmd2.ExecuteNonQuery();
                delete = cn.CreateCommand();
                delete.CommandText = "DROP TABLE buforstany";
                delete.Prepare();
                delete.ExecuteNonQuery();
                cmd2 = cn.CreateCommand();
                cmd2.CommandText = "CREATE TABLE buforstany (id int identity not null primary key, kod nvarchar(15), stan numeric(10,3), MagId int, Nazwa nvarchar(50), datazmian datetime)";
                cmd2.Prepare();
                cmd2.ExecuteNonQuery();



                SqlCommand cmd = pcmn.CreateCommand();
                cmd.CommandText = "SELECT     Towar.Kod, Towar.Nazwa, SUM(Istw.StanMag) AS Expr1, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100 AS CenaD, Towar.Stawka / 100 AS Vat, Towar.Zmiana, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100 AS CenaH,                     Towar.Opis2 AS CenaO  FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId  GROUP BY Towar.Kod, Towar.Nazwa, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100, Towar.Stawka / 100, Towar.Zmiana, Towar.Aktywny, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100, Towar.Opis2 HAVING     (Towar.Aktywny = 1) AND (Towar.Zmiana > @data) ORDER BY Towar.Zmiana";
                cmd.Parameters.Add("@data", SqlDbType.DateTime);
                cmd.Prepare();
                cmd.Parameters["@data"].Value = lastimport;
                SqlCommand cmd5 = pcmn.CreateCommand();
                cmd5.CommandText = "SELECT     KodDod.Kod, Towar.Nazwa, SUM(Istw.StanMag) AS Expr1, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100 AS CenaD, Towar.Stawka / 100 AS Vat, Towar.Zmiana, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100 AS CenaH,                   Towar.Opis2 AS CenaO FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId GROUP BY KodDod.Kod, Towar.Nazwa, Towar.CenaEw, Towar.CenaDet * (Towar.Stawka / 100 + 100) / 100, Towar.Stawka / 100, Towar.Zmiana, Towar.Aktywny, Towar.CenaHurt * (Towar.Stawka / 100 + 100) / 100, Towar.Opis2 HAVING     (Towar.Aktywny = 1) AND (Towar.Zmiana > @data) ORDER BY Towar.Zmiana";
                cmd5.Parameters.Add("@data", SqlDbType.DateTime);
                cmd5.Prepare();
                cmd5.Parameters["@data"].Value = lastimport;

                SqlDataReader sdr = cmd.ExecuteReader();

                while (sdr.Read())
                {
                    //      MessageBox.Show(sdr.GetString(0));
                    string kod = sdr.GetString(0);
                    string nazwa = sdr.GetString(1);
                    string stan = decimal.Round(sdr.GetDecimal(2), 2).ToString(nfi);
                    string cenazk = decimal.Round(sdr.GetDecimal(3), 2).ToString(nfi);
                    string cenasp = decimal.Round(sdr.GetDecimal(4), 2).ToString(nfi);
                    string vat = Convert.ToString(sdr.GetInt32(5));
                    DateTime impdatazmiany = sdr.GetDateTime(6);
                    string cenah = decimal.Round(sdr.GetDecimal(7), 2).ToString(nfi);
                    string cenao = sdr.GetString(7);
                    try
                    {
                        cmd3.Parameters["@k"].Value = kod;
                        cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                        cmd3.Parameters["@s"].Value = stan;
                        cmd3.Parameters["@cz"].Value = cenazk;
                        cmd3.Parameters["@cs"].Value = cenasp;
                        cmd3.Parameters["@v"].Value = vat;
                        cmd3.Parameters["@d"].Value = impdatazmiany;
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
                    DateTime impdatazmiany = sdr.GetDateTime(6);
                    string cenah = decimal.Round(sdr.GetDecimal(7), 2).ToString(nfi);
                    string cenao = sdr.GetString(7);
                    try
                    {
                        cmd3.Parameters["@k"].Value = kod;
                        cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                        cmd3.Parameters["@s"].Value = stan;
                        cmd3.Parameters["@cz"].Value = cenazk;
                        cmd3.Parameters["@cs"].Value = cenasp;
                        cmd3.Parameters["@v"].Value = vat;
                        cmd3.Parameters["@d"].Value = impdatazmiany;
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

                cmd2.CommandText = "SELECT Nazwa, MagId FROM magazyn";
                cmd2.Prepare();
                SqlCommand cmd9 = pcmn.CreateCommand();
                cmd9.CommandText = "SELECT     Towar.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag, Towar.Zmiana AS Expr1 FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId WHERE     (Towar.Aktywny = 1) AND Istw.MagId = @mag AND (Towar.Zmiana > @data) ORDER BY Towar.Zmiana";
                cmd9.Parameters.Add("@mag", SqlDbType.Int);
                cmd9.Parameters.Add("@data", SqlDbType.DateTime);
                cmd9.Prepare();
                cmd9.Parameters["@data"].Value = lastimport;


                SqlCommand cmd8 = pcmn.CreateCommand();
                cmd8.CommandText = "SELECT     KodDod.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag, Towar.Zmiana AS Expr1 FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId WHERE     (Towar.Aktywny = 1)   AND Istw.MagId = @mag AND (Towar.Zmiana > @data) ORDER BY Towar.Zmiana";
                cmd8.Parameters.Add("@mag", SqlDbType.Int);
                cmd8.Parameters.Add("@data", SqlDbType.DateTime);
                cmd8.Prepare();
                cmd8.Parameters["@data"].Value = lastimport;


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
                cedr.Close();

                //     datazmian = DateTime.Now;
                //     SqlCeCommand updatedata = cn.CreateCommand();
                //     updatedata.CommandText = "UPDATE opcje SET datazmian = ? WHERE id = 1";
                //     updatedata.Parameters.Add("@data", SqlDbType.DateTime);
                ////     updatedata.Prepare();
                //     updatedata.Parameters["@data"].Value = datazmian;
                //     updatedata.ExecuteNonQuery();


                cn.Close();
                pcmn.Close();
                Invoke((MethodInvoker)(() => exkom_t.Text = "Dane pobrane z pc-market\n"));
            }
            catch
            {

                Invoke((MethodInvoker)(() => exkom_t.Text += "Błąd połączenia z bazą\n"));
            }

            





        }

        public void updatedata(Undersoft.Picatch.Agent.Main mainf)
        {



            while(running == true)
            {

                downloaddata();

                    try
                    {                       
                        Invoke((MethodInvoker)(() => exkom_t.Text += "KONWERTUJE DANE Z BAZY PCMARKET\n"));
                        
                       

                        System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                        nfi.NumberDecimalSeparator = ".";

                        //  string connectionString2;
                        //      connectionString2 = "server=" + serverpcm + ";user id=" + loginpcm + ";password=" + passwdpcm + ";Trusted_Connection=no; database=" + bazapcm + ";connection timeout=30";
                        //       pcmn = new SqlConnection(connectionString2);
                    }
                    catch (Exception ex)
                    {
                        Invoke((MethodInvoker)(() => exkom_t.Text = "BŁĄD KONWERSJI DANYCH Z BAZY PCMARKET\n"));
                    }
                    string connectionString;

                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);



                    string connectionString3;

                    connectionString3 = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cnn = new SqlCeConnection(connectionString3);


                    string connectionString2;

                    connectionString2 = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cnnn = new SqlCeConnection(connectionString2);

                    int dbtest = 0;

                    try
                    {
                        cnnn.Open();
                        cn.Open();
                        cnn.Open();

                    }
                    catch (Exception ex)
                    {
                        Invoke((MethodInvoker)(() => exkom_t.Text += "Błąd połączenia z bazą\n"));
                        dbtest = 1;

                    }

                    if (dbtest == 0)
                    {
                        try
                        {

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



                            SqlCeCommand cmd10 = cn.CreateCommand();
                            cmd10.CommandText = "UPDATE dane SET nazwa = ?, stan = ?, cenazk = ?, cenasp = ?, vat = ?, datazmian = ?, cenahurt = ?, cenaoryg = ? WHERE kod = ?";


                            cmd10.Parameters.Add("@n", SqlDbType.NVarChar, 40);
                            cmd10.Parameters.Add("@s", SqlDbType.NVarChar, 10);

                            cmd10.Parameters.Add("@cz", SqlDbType.NVarChar, 10);
                            cmd10.Parameters.Add("@cs", SqlDbType.NVarChar, 10);
                            cmd10.Parameters.Add("@v", SqlDbType.NVarChar, 5);
                            cmd10.Parameters.Add("@d", SqlDbType.DateTime);
                            cmd10.Parameters.Add("@ch", SqlDbType.NVarChar, 10);
                            cmd10.Parameters.Add("@co", SqlDbType.NVarChar, 10);
                            cmd10.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                           
                            cmd10.Prepare();




                            SqlCeCommand cmd6 = cn.CreateCommand();
                            cmd6.CommandText = "UPDATE stany SET stan = ?, datazmian = ? WHERE (kod = ?) AND (MagId = ?)";


                            cmd6.Parameters.Add("@i", SqlDbType.Decimal, 10);
                            cmd6.Parameters["@i"].Precision = 10;
                            cmd6.Parameters["@i"].Scale = 3;
                            cmd6.Parameters.Add("@d", SqlDbType.DateTime);
                            cmd6.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                            cmd6.Parameters.Add("@m", SqlDbType.Int, 11);


                            cmd6.Prepare();

                            SqlCeCommand cmdcheck = cnn.CreateCommand();
                            cmdcheck.CommandText = "SELECT count(*) FROM dane where kod = ?";
                            cmdcheck.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                            cmdcheck.Prepare();

                            SqlCeCommand cmdchecks = cnn.CreateCommand();
                            cmdchecks.CommandText = "SELECT count(*) FROM stany where kod = ? and MagId = ?";
                            cmdchecks.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                            cmdchecks.Parameters.Add("@m", SqlDbType.Int, 11);
                            cmdchecks.Prepare();



                            SqlCeCommand cmd = cnnn.CreateCommand();
                            cmd.CommandText = "SELECT     kod, nazwa, stan, cenazk, cenasp, vat, datazmian, cenahurt FROM        bufordane";
                            cmd.Prepare();
                            //    SqlCeCommand cmd5 = cnnn.CreateCommand();
                            //    cmd5.CommandText = "SELECT     kod, stan, MagId, Nazwa FROM        buforstany";
                            //      cmd5.Prepare();


                            SqlCeDataReader sdr = cmd.ExecuteReader();

                            while (sdr.Read())
                            {

                                string kod = sdr.GetString(0);
                                string nazwa = sdr.GetString(1);
                                string stan = sdr.GetString(2);// decimal.Round(sdr.GetDecimal(2), 2).ToString(nfi);
                                string cenazk = sdr.GetString(3);// decimal.Round(sdr.GetDecimal(3), 2).ToString(nfi);
                                string cenasp = sdr.GetString(4);// decimal.Round(sdr.GetDecimal(4), 2).ToString(nfi);
                                string vat = sdr.GetString(5);  // Convert.ToString(sdr.GetInt32(5));
                                DateTime impdatazmian = sdr.GetDateTime(6);
                                string cenah = sdr.GetString(7);
                                string cenao = sdr.GetString(8);
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
                                        cmd10.Parameters["@d"].Value = impdatazmian;
                                        cmd10.Parameters["@ch"].Value = cenah;
                                        cmd10.Parameters["@k"].Value = kod;
                                        cmd10.ExecuteNonQuery();




                                       if (this.exkom_t.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)(() => exkom_t.Text += kod + ";" + nazwa + ";" + stan + ";" + impdatazmian.ToString() + "; - Zaktualizowano\n"));
                                                  Invoke((MethodInvoker)(() => exkom_t.SelectionStart = exkom_t.Text.Length));
                                                 Invoke((MethodInvoker)(() => exkom_t.ScrollToCaret()));
                                        }
                                        else
                                        {
                                            exkom_t.Text += kod + ";" + nazwa + ";" + stan + ";" + datazmian.ToString() + "; - Zaktualizowano\n";
                                             exkom_t.SelectionStart = exkom_t.Text.Length;
                                             exkom_t.ScrollToCaret();
                                        } 


                                    }
                                    else
                                    {

                                        cmd3.Parameters["@k"].Value = kod;
                                        cmd3.Parameters["@n"].Value = (nazwa.Replace("?", "")).Replace(";", "");
                                        cmd3.Parameters["@s"].Value = stan;
                                        cmd3.Parameters["@cz"].Value = cenazk;
                                        cmd3.Parameters["@cs"].Value = cenasp;
                                        cmd3.Parameters["@v"].Value = vat;
                                        cmd3.Parameters["@d"].Value = impdatazmian;
                                        cmd3.Parameters["@ch"].Value = cenah;
                                        cmd3.Parameters["@co"].Value = cenao;
                                        cmd3.ExecuteNonQuery();

                                        if (this.exkom_t.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)(() => exkom_t.Text += kod + ";" + nazwa + ";" + stan + ";" + impdatazmian.ToString() + "; - Dodano\n"));
                                             Invoke((MethodInvoker)(() => exkom_t.SelectionStart = exkom_t.Text.Length));
                                              Invoke((MethodInvoker)(() => exkom_t.ScrollToCaret()));
                                        }
                                        else
                                        {
                                            exkom_t.Text += kod + ";" + nazwa + ";" + stan + ";" + datazmian.ToString() + "; - Dodano\n";
                                            exkom_t.SelectionStart = exkom_t.Text.Length;
                                             exkom_t.ScrollToCaret();
                                        }
                                      

                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.ToString());
                                }
                            }

                            sdr.Close();
                                if (this.exkom_t.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)(() => exkom_t.Text += "Zaktualizowano dane o towarach\n"));
                                            //       Invoke((MethodInvoker)(() => exkom_t.SelectionStart = exkom_t.Text.Length));
                                            //        Invoke((MethodInvoker)(() => exkom_t.ScrollToCaret()));
                                        }
                                        else
                                        {
                                            exkom_t.Text += "Zaktualizowano dane o towarach\n";
                                            //      exkom_t.SelectionStart = exkom_t.Text.Length;
                                            //      exkom_t.ScrollToCaret();
                                        } 

                          
                            SqlCeCommand cmd2 = cn.CreateCommand();
                            cmd2.CommandText = "SELECT Nazwa, MagId FROM magazyn";
                            cmd2.Prepare();
                            SqlCeCommand cmd9 = cnn.CreateCommand();
                            cmd9.CommandText = "SELECT     kod, stan, MagId, Nazwa, datazmian FROM  buforstany WHERE  MagId = @mag ";
                            cmd9.Parameters.Add("@mag", SqlDbType.Int);
                            cmd9.Prepare();
                            //    SqlCommand cmd8 = pcmn.CreateCommand();
                            //     cmd8.CommandText = "SELECT     KodDod.Kod, Magazyn.Nazwa, Istw.MagId, Istw.StanMag AS Expr1 FROM        Towar INNER JOIN                   Istw ON Towar.TowId = Istw.TowId INNER JOIN                   Magazyn ON Istw.MagId = Magazyn.MagId INNER JOIN                   KodDod ON Towar.TowId = KodDod.TowId WHERE     (Towar.Aktywny = 1)   AND Istw.MagId = @mag ";
                            //       cmd8.Parameters.Add("@mag", SqlDbType.Int);
                            //      cmd8.Prepare();

                            SqlCeDataReader cedr = cmd2.ExecuteReader();

                            while (cedr.Read())
                            {
                                string magazyn = cedr.GetString(0);
                                int magid = cedr.GetInt32(1);
                                cmd9.Parameters["@mag"].Value = magid;
                                //    cmd8.Parameters["@mag"].Value = magid;


                                SqlCeDataReader szdr = cmd9.ExecuteReader();
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

                                        cmd6.Parameters["@i"].Value = szdr.GetDecimal(1);
                                        cmd6.Parameters["@k"].Value = szdr.GetString(0);
                                        cmd6.Parameters["@m"].Value = magid;
                                        cmd6.ExecuteNonQuery();

                                        if (this.exkom_t.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)(() => exkom_t.Text += szdr.GetString(0) + ";" + szdr.GetDecimal(1).ToString() + ";" + magazyn + "; - Zaktualizowano\n"));
                                            Invoke((MethodInvoker)(() => exkom_t.SelectionStart = exkom_t.Text.Length));
                                            Invoke((MethodInvoker)(() => exkom_t.ScrollToCaret()));

                                        }
                                        else
                                        {
                                            exkom_t.Text += szdr.GetString(0) + ";" + szdr.GetDecimal(3).ToString() + ";" + magazyn + "; - Zaktualizowano\n";
                                            exkom_t.SelectionStart = exkom_t.Text.Length;
                                            exkom_t.ScrollToCaret();

                                        }

                                    }
                                    else
                                    {
                                        cmd4.Parameters["@k"].Value = szdr.GetString(0);
                                        cmd4.Parameters["@i"].Value = szdr.GetDecimal(3);
                                        cmd4.Parameters["@s"].Value = szdr.GetDecimal(2);
                                        cmd4.Parameters["@cz"].Value = szdr.GetString(1);
                                        cmd4.Parameters["@d"].Value = szdr.GetDateTime(4);
                                        cmd4.ExecuteNonQuery();

                                        if (this.exkom_t.InvokeRequired)
                                        {
                                            Invoke((MethodInvoker)(() => exkom_t.Text += szdr.GetString(0) + ";" + szdr.GetDecimal(3).ToString() + ";" + magazyn + "; - Dodano\n"));
                                            Invoke((MethodInvoker)(() => exkom_t.SelectionStart = exkom_t.Text.Length));
                                            Invoke((MethodInvoker)(() => exkom_t.ScrollToCaret()));


                                        }
                                        else
                                        {
                                            exkom_t.Text += szdr.GetString(0) + ";" + szdr.GetDecimal(3).ToString() + ";" + magazyn + "; - Dodano\n";
                                            exkom_t.SelectionStart = exkom_t.Text.Length;
                                            exkom_t.ScrollToCaret();

                                        }
                                    }


                                }
                                
                                szdr.Close();
                           

                            }
                            cedr.Close();


                            cn.Close();
                            cnn.Close();
                            cnnn.Close();

                            if (this.exkom_t.InvokeRequired)
                            {
                                Invoke((MethodInvoker)(() => exkom_t.Text += "Zaktualizowano dane o stanach\n"));
                                      Invoke((MethodInvoker)(() => exkom_t.SelectionStart = exkom_t.Text.Length));
                                       Invoke((MethodInvoker)(() => exkom_t.ScrollToCaret()));
                                       Invoke((MethodInvoker)(() => exkom_t.Text += "Dane aktualne na:" + DateTime.Now.ToString()+ "\n"));
                            }
                            else
                            {
                                exkom_t.Text += "Zaktualizowano dane o stanach\n";
                                      exkom_t.SelectionStart = exkom_t.Text.Length;
                                     exkom_t.ScrollToCaret();
                            } 
                        }
                        catch (Exception eg)
                        {
                            cnn.Close();
                            cn.Close();
                            cnnn.Close();
                            Invoke((MethodInvoker)(() => exkom_t.Text += eg.ToString() + "\n"));
                        }
                    }

                    Thread.Sleep(10000);
                   // exkom_t.Text = "";
                }
               
                    
            

        }

        bool mAllowVisible;     // ContextMenu's Show command used
        bool mAllowClose;       // ContextMenu's Exit command used
        //   bool mLoadFired;        // Form was shown once

        protected override void SetVisibleCore(bool value)
        {
            if (!mAllowVisible)
            {
                value = false;
                if (!this.IsHandleCreated) CreateHandle();
            }
            base.SetVisibleCore(value);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!mAllowClose)
            {
                this.Hide();
                e.Cancel = true;
            }
            base.OnFormClosing(e);
        }

        private void zakończToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
            mAllowClose = true;
            this.Close();
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mAllowVisible = true;
            //  mLoadFired = true;
            Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            running = false;
            if (bw.WorkerSupportsCancellation == true)
            {
                bw.CancelAsync();
            }
            mAllowClose = true;
            this.Close();
        }

    }
}
