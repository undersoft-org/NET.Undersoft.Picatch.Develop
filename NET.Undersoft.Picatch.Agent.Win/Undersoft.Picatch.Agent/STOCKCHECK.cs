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

namespace Undersoft.Picatch.Agent
{
    public partial class STOCKCHECK : Form
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
        public string kod;
        public string nazwa;
        public string cenazk;
        public string cenasp;
        public string dokstan;
        public string vat;
        public bool dbflag;
        public string cenah;
        public string cenao;
        public int type;

        public STOCKCHECK(int typ)
        {
            InitializeComponent();
            type = typ;
            Openbaza();
            if (type == 0)
            {
                label20.Text = "KOLektor - deTAL";
            }
            if (type == 1)
            {
                label20.Text = "KOLektor - huRT";
            }



        }


        public void Openbaza()
        {
            string connectionString;
            string fileName = "Baza.sdf";
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);

            if (File.Exists(fileName) == false)
            {

                SqlCeEngine engine = new SqlCeEngine(connectionString);
                engine.CreateDatabase();
                cn.Open();
                SqlCeCommand cmd = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(40), stan nvarchar(10), cenazk nvarchar(10), cenasp nvarchar(10), vat nvarchar(5), devstat nvarchar(10), bad_cena bit, bad_stan bit, cenapolka numeric(6,3), zliczono numeric(6,3), datazmian datetime, cenahurt nvarchar(10), cenaoryg nvarchar(10))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE bufor (id int identity not null primary key, dokid int, kod nvarchar (15), nazwa nvarchar (100), cenazk nvarchar(10), ilosc numeric(10,3), stan nvarchar(10), cenasp nvarchar(10), vat nvarchar(10))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE dok (id int identity not null primary key, nazwadok nvarchar (120), typ nvarchar(10), data datetime, device nvarchar(20))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE opcje (id int identity not null primary key, transfer nvarchar (20), com nvarchar(20), ip nvarchar(20), ufile nvarchar (120), dfile nvarchar(120), bdll nvarchar(50), bflag bit, ipflag bit, port int, skaner nvarchar(120), serverpcm nvarchar (20), loginpcm nvarchar(20), passwdpcm nvarchar(20), bazapcm nvarchar(20), dbflag bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("INSERT INTO opcje (transfer, com, ip, ufile, dfile, bdll, bflag, ipflag, port, skaner, serverpcm, loginpcm, passwdpcm, bazapcm, dbflag) VALUES ('ZAM_ODB', 'COM5', '0.0.0.0', 'C:\\PICEDI\\EKSPORT', 'C:\\PICEDI\\IMPORT', 'MSStack', -1, -1, 8790, 'PC-MARKET', '84.10.33.10', 'sa', 'sqll', 'opencart', 0)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE edihead (id int identity not null primary key, FileName nvarchar(40), TypPolskichLiter nvarchar(20), TypDok nvarchar (20), NrDok nvarchar(30), Data nvarchar(30), DataRealizacji nvarchar (30), Magazyn nvarchar(30), SposobPlatn nvarchar(10), TerminPlatn  nvarchar(10), IndeksCentralny nvarchar(10), NazwaWystawcy  nvarchar(120), AdresWystawcy  nvarchar(120), KodWystawcy nvarchar(120), MiastoWystawcy nvarchar(120), UlicaWystawcy nvarchar(120), NIPWystawcy nvarchar(120), BankWystawcy nvarchar(120), KontoWystawcy nvarchar(120), TelefonWystawcy nvarchar(30), NrWystawcyWSieciSklepow nvarchar(20), NazwaOdbiorcy nvarchar(120), AdresOdbiorcy nvarchar(120), KodOdbiorcy nvarchar(20), MiastoOdbiorcy nvarchar(120), UlicaOdbiorcy nvarchar(120), NIPOdbiorcy nvarchar(120), BankOdbiorcy nvarchar(120), KontoOdbiorcy nvarchar(120), TelefonOdbiorcy nvarchar(120), NrOdbiorcyWSieciSklepow nvarchar(20), DoZaplaty nvarchar(20), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE edibody (id int identity not null primary key, NrDok nvarchar(30), Nazwa nvarchar (120), kod nvarchar(20), Vat nvarchar(7), Jm nvarchar (7), Asortyment nvarchar(120), Sww nvarchar(30), PKWiU nvarchar(30), Ilosc nvarchar(10), Cena nvarchar(10), Wartosc nvarchar(10), IleWOpak nvarchar(10), CenaSp nvarchar(10), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE ediend (id int identity not null primary key, NrDok nvarchar(30), Vat nvarchar (20), SumaNet nvarchar(20), SumaVat nvarchar(20), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fedihead (id int identity not null primary key, FileName nvarchar(40), TypPolskichLiter nvarchar(20), TypDok nvarchar (20), NrDok nvarchar(30), Data nvarchar(30), DataRealizacji nvarchar (30), Magazyn nvarchar(30), SposobPlatn nvarchar(10), TerminPlatn  nvarchar(10), IndeksCentralny nvarchar(10), NazwaWystawcy  nvarchar(120), AdresWystawcy  nvarchar(120), KodWystawcy nvarchar(120), MiastoWystawcy nvarchar(120), UlicaWystawcy nvarchar(120), NIPWystawcy nvarchar(120), BankWystawcy nvarchar(120), KontoWystawcy nvarchar(120), TelefonWystawcy nvarchar(30), NrWystawcyWSieciSklepow nvarchar(20), NazwaOdbiorcy nvarchar(120), AdresOdbiorcy nvarchar(120), KodOdbiorcy nvarchar(20), MiastoOdbiorcy nvarchar(120), UlicaOdbiorcy nvarchar(120), NIPOdbiorcy nvarchar(120), BankOdbiorcy nvarchar(120), KontoOdbiorcy nvarchar(120), TelefonOdbiorcy nvarchar(120), NrOdbiorcyWSieciSklepow nvarchar(20), DoZaplaty nvarchar(20), status nvarchar (20), complete bit, device nvarchar(20))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fedibody (id int identity not null primary key, NrDok nvarchar(30), Nazwa nvarchar (120), kod nvarchar(20), Vat nvarchar(7), Jm nvarchar (7), Asortyment nvarchar(120), Sww nvarchar(30), PKWiU nvarchar(30), Ilosc nvarchar(10), Cena nvarchar(10), Wartosc nvarchar(10), IleWOpak nvarchar(10), CenaSp nvarchar(10), status nvarchar, complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE fediend (id int identity not null primary key, NrDok nvarchar(30), Vat nvarchar (20), SumaNet nvarchar(20), SumaVat nvarchar(20), status nvarchar(20), complete bit)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE devdane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(100), stan nvarchar(10), cenazk nvarchar(20), cenasp nvarchar(20), vat nvarchar(5), device nvarchar(20), devstat nvarchar(10))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE wynikispr (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(100), stan nvarchar(10), cenazk nvarchar(20), cenasp nvarchar(20), vat nvarchar(5), devstat nvarchar(20), bad_cena bit, bad_stan bit, cenapolka numeric(10,3), zliczono numeric(10,3))", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE magazyn (id int identity not null primary key, Nazwa nvarchar (20), MagId int)", cn);
                cmd.ExecuteNonQuery();
                cmd = new SqlCeCommand("CREATE TABLE stany (id int identity not null primary key, kod nvarchar(15), stan numeric(10,3), MagId int, Nazwa nvarchar(50))", cn);
                cmd.ExecuteNonQuery();
                cn.Close();

            }
            cn.Open();
            SqlCeCommand cmd2 = cn.CreateCommand();
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

            SqlCeCommand cmd3 = cn.CreateCommand();
            cmd3.CommandText = "SELECT * FROM magazyn";
            cmd3.Prepare();
            SqlCeDataReader dr3 = cmd3.ExecuteReader();

            while (dr3.Read())
            {
                toolStripComboBox1.Items.Add(dr3["Nazwa"].ToString());
                comboBox1.Items.Add(dr3["Nazwa"].ToString());
            }

            dr3.Close();
            cn.Close();

            // DataGridTableStyle ts = new DataGridTableStyle();
            //  SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM stany", cn);
            //   DataTable table2 = new DataTable();
            //   db.SelectCommand = new SqlCeCommand("SELECT kod, nazwa, stan, cenazk, cenasp, vat FROM dane", cn);
            //  db.SelectCommand.ExecuteNonQuery();
            //  db.Fill(table2);

            //    if (table2.Rows.Count != 0)
            //    {
            //         dataGridView1.DataSource = table2.DefaultView;
            //    }
            //    cn.Close();







        }



        private void savedoc()
        {

            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

            if (textBox1.Text != "" && textBox1.Text != null && kod != "" && kod != null && vat != "" && vat != null)
            {

                SqlCeCommand cmd = cn.CreateCommand();
                cmd.CommandText = "SELECT * FROM dok where (nazwadok = ?)";

                cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);
                cmd.Prepare();

                cmd.Parameters["@k"].Value = textBox1.Text + "-" + comboBox1.Text;

                int test = 0;
                int dokid = 0;

                SqlCeDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        test = 1;
                        dokid = dr.GetInt32(0);
                    }
                }
                dr.Close();

                if (test == 0)
                {

                    MessageBox.Show("Został utworzony nowy dokument o nazwie:" + textBox1.Text + "-" + comboBox1.Text);

                    cmd.Parameters.Clear();
                    cmd = cn.CreateCommand();
                    cmd.CommandText = "INSERT INTO dok (nazwadok, typ, data) VALUES (?, 'MM', ?)";

                    cmd.Parameters.Add("@k", SqlDbType.NVarChar, 50);
                    cmd.Parameters.Add("@d", SqlDbType.DateTime);
                    cmd.Prepare();

                    cmd.Parameters["@k"].Value = textBox1.Text + "-" + comboBox1.Text;
                    cmd.Parameters["@d"].Value = DateTime.Now;
                    cmd.ExecuteNonQuery();

                    savedoc();
                }
                else if (test == 1)
                {
                    SqlCeCommand cmd2 = cn.CreateCommand();
                    cmd2.CommandText = "INSERT INTO bufor (dokid, kod, nazwa, cenazk, ilosc, stan, cenasp, vat) VALUES (?, ?, ?, ?, ?, ?, ?, ?)";

                    cmd2.Parameters.Add("@d", SqlDbType.Int, 11);
                    cmd2.Parameters.Add("@k", SqlDbType.NVarChar, 20);
                    cmd2.Parameters.Add("@n", SqlDbType.NVarChar, 20);
                    cmd2.Parameters.Add("@cz", SqlDbType.NVarChar, 20);
                    cmd2.Parameters.Add("@i", SqlDbType.Decimal, 10);
                    cmd2.Parameters["@i"].Precision = 10;
                    cmd2.Parameters["@i"].Scale = 3;
                    cmd2.Parameters.Add("@s", SqlDbType.NVarChar, 20);
                    cmd2.Parameters.Add("@cs", SqlDbType.NVarChar, 20);
                    cmd2.Parameters.Add("@v", SqlDbType.NVarChar, 20);
                    cmd2.Prepare();

                    cmd2.Parameters["@d"].Value = dokid;
                    cmd2.Parameters["@k"].Value = kod;
                    cmd2.Parameters["@n"].Value = nazwa;
                    cmd2.Parameters["@cz"].Value = cenazk;
                    cmd2.Parameters["@i"].Value = Convert.ToDecimal(textBox2.Text);
                    cmd2.Parameters["@s"].Value = dokstan;
                    if (type == 0)
                    {
                        cmd2.Parameters["@cs"].Value = cenasp;
                    }
                    else if (type == 1)
                    {
                        cmd2.Parameters["@cs"].Value = cenao;
                    }

                    cmd2.Parameters["@v"].Value = vat;
                    cmd2.ExecuteNonQuery();

                    kod = "";
                    nazwa = "";
                    textBox2.Text = "1";
                    vat = "";
                    test = 0;
                    label1.Text = "DODANO DO DOKUMENTU";
                }
            }
            else
            {

                MessageBox.Show("Nie można dodać dokumentu jeżeli brak jest nazwy, oraz nie można dwukrotnie dodać tej samej pozycji, Aby dodać pozycję drugi raz wyszukaj ponownie kod towaru !!");
            }
            search_t.Focus();
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
            if (kodbuf != "" && toolStripComboBox1.Text != "" && toolStripComboBox1.Text != null)
            {



                SqlCeCommand cmd = cn.CreateCommand();
                cmd.CommandText = "SELECT     dane.kod, dane.nazwa, stany.stan, magazyn.Nazwa AS magazyn, dane.cenasp, dane.stan as calystan, dane.vat, dane.cenazk, dane.cenahurt, dane.cenaoryg FROM magazyn INNER JOIN stany ON magazyn.MagId = stany.MagId INNER JOIN dane ON stany.kod = dane.kod where (magazyn.Nazwa = ?) AND (dane.kod = ?)";
                cmd.Parameters.Add("@m", SqlDbType.NVarChar, 20);
                cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);
                cmd.Prepare();
                cmd.Parameters["@m"].Value = toolStripComboBox1.Text;
                cmd.Parameters["@k"].Value = kodbuf;

                int test = 0;


                SqlCeDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    if (!dr.IsDBNull(0))
                    {
                        System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
                        nfi.NumberDecimalSeparator = ".";
                        test = 1;
                        decimal stan = decimal.Round(dr.GetDecimal(2), 0);
                        decimal calystan = decimal.Parse(dr.GetString(5), nfi);
                        dokstan = dr.GetString(5);
                        cenasp = dr.GetString(4);
                        nazwa = dr.GetString(1);
                        kod = dr.GetString(0);
                        cenazk = dr.GetString(7);
                        vat = dr.GetString(6);
                        cenah = Convert.ToString(decimal.Round(Convert.ToDecimal(dr.GetString(8), nfi) / ((Convert.ToDecimal(vat, nfi) + 100) / 100), 2));
                        cenao = dr.GetString(9);

                        if (type == 0)
                        {
                            label1.Text = kod + "\n" + nazwa + "\n Detaliczna (brutto): " + cenasp;
                        }
                        else if (type == 1)
                        {
                            label1.Text = kod + "\n" + nazwa + "\n Hurt (netto): " + cenah + "   Oryginalna (brutto): " + cenao + "\n\nStan: " + stan.ToString();
                        }
                        if (stan > 0)
                        {
                            this.BackColor = Color.YellowGreen;

                            label1.Text += " - OK !!!!!";
                        }
                        else if (calystan > 0)
                        {
                            this.BackColor = Color.DodgerBlue;
                            label1.Text += "\n TOWAR JEST NA INNYM MAGAZYNIE";
                        }
                        else
                        {
                            this.BackColor = Color.Crimson;
                            label1.Text += "\n BRAK TOWARU";
                        }




                        DataGridTableStyle ts = new DataGridTableStyle();
                        SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT    stany.kod, stany.stan, magazyn.Nazwa AS magazyn FROM magazyn INNER JOIN stany ON magazyn.MagId = stany.MagId", cn);
                        DataTable table2 = new DataTable();
                        db.SelectCommand = new SqlCeCommand("SELECT     magazyn.Nazwa AS Magazyn, stany.stan As Stan FROM magazyn INNER JOIN stany ON magazyn.MagId = stany.MagId where (stany.kod = ?)", cn);
                        db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 15);
                        db.SelectCommand.Parameters["@k"].Value = kodbuf;
                        db.SelectCommand.ExecuteNonQuery();
                        db.Fill(table2);


                        if (table2.Rows.Count != 0)
                        {
                            dataGridView1.DataSource = table2.DefaultView;

                        }
                    }



                }
                dr.Close();

                if (test == 0)
                {
                    label1.Text = "NIE ZNALEZIONO TOWARU O PODANYM KODZIE !!!!";
                    this.BackColor = Color.DarkOrange;

                }
            }
            else
            {
                MessageBox.Show("Nie wybrałeś magazynu lub nie podałeś kodu kreskowego towaru");
            }
            search_t.Text = "";
            search_t.Focus();
            dataGridView1.Refresh();

            cn.Close();
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

        private void xToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void search_t_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                FindIndex();

            }

            if (e.KeyChar == 32)
            {

                savedoc();
                search_t.Text = "";

            }
        }

        private void toolStripComboBox1_TextUpdate(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != "")
            {
                magazynToolStripMenuItem.Text = toolStripComboBox1.Text;
            }
        }

        private void toolStripComboBox1_Validated(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != "")
            {
                magazynToolStripMenuItem.Text = toolStripComboBox1.Text;
            }
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripComboBox1.Text != null && toolStripComboBox1.Text != "")
            {
                magazynToolStripMenuItem.Text = toolStripComboBox1.Text;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            savedoc();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
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

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
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

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }

    
}
