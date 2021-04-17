using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using Microsoft.WindowsCE.Forms;
using System.Data.SqlTypes;
using System.Data.SqlClient;

namespace SmartDeviceApplication2
{
	/// <summary>
	/// Summary description for Form2.
	/// </summary>
	public class Form18 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.TextBox kod_t;
		private System.Windows.Forms.Label kod_l;
		private System.Windows.Forms.TextBox cena_t;
		private System.Windows.Forms.Label cena_l;
		private System.Windows.Forms.Label nazwa_l;
		private System.Windows.Forms.TextBox nazwa_t;
		private System.Windows.Forms.Label ilosc_l;
		private System.Windows.Forms.Button exit_b;
		private System.Windows.Forms.Label stan_l;
		private System.Windows.Forms.TextBox ilosc_t;
		private System.Windows.Forms.Label zliczono_l;
		private System.Windows.Forms.TextBox zliczono_t;
		private System.Windows.Forms.Button ok_b;
		
		
		
		private string indeks;
		private System.Windows.Forms.Label label1;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox vat_t;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox cenasp_t;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button rezlicz_b;
		private System.Windows.Forms.TextBox wopak_t;
		private System.Windows.Forms.TextBox wymagane_t;
		private System.Windows.Forms.TextBox jm_t;
		private System.Windows.Forms.TextBox status_t;
		string kodzik;
		string asorcik1;
		string zliczono;
		int czydodac;
		int ebid;
		private SmartDeviceApplication2.Form17 form17;
		int rownum;
		public Form18(string dokus, string kod, string nazwik, string cenik, string ceniksp, string vacik, string wymagane, string zliczone, string statusik, string opak, string jedn, string asorcik, SmartDeviceApplication2.Form17 form17a, int rownumber, int addflag, string ilosc, int id)
		{
			kodzik = kod;
			indeks = dokus;
			rownum = rownumber;
			zliczono = zliczone;
			form17 = form17a;
			czydodac = addflag;
			ebid = id;
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();		
			
			indeks = dokus;
			kod_t.Text = kodzik;
			nazwa_t.Text = nazwik;
			cena_t.Text = cenik;
			cenasp_t.Text = ceniksp;
			vat_t.Text = vacik;
			wymagane_t.Text = Convert.ToString(decimal.Parse(wymagane) - decimal.Parse(zliczono));
			wopak_t.Text = opak;
			jm_t.Text = jedn;
			status_t.Text = statusik;
			asorcik1 = asorcik;
			zliczono_t.Text = zliczono;
			ilosc_t.Text = ilosc;
			ilosc_t.Focus();
			ilosc_t.SelectAll();
		}
		
		private void WriteLine ()
		{
           // try
           // {
                if (czydodac == 1)
                {

                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();

                    SqlCeCommand cmdb = cn.CreateCommand();
                    cmdb.CommandText = "INSERT INTO fedibody (NrDok, Nazwa, kod, Vat, Jm, Asortyment, Ilosc, Cena, IleWOpak, CenaSp, ebid, Wymagane) VALUES (@a, @b, @c, @d, @e, @f, @g, @h, @i, @j, @k, @l)";
                    cmdb.Parameters.Add("@a", SqlDbType.NVarChar, 30);
                    cmdb.Parameters.Add("@b", SqlDbType.NVarChar, 120);
                    cmdb.Parameters.Add("@c", SqlDbType.NVarChar, 20);
                    cmdb.Parameters.Add("@d", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@e", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@f", SqlDbType.NVarChar, 120);
                    cmdb.Parameters.Add("@g", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@h", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@i", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@j", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@k", SqlDbType.Int, 10);
                    cmdb.Parameters.Add("@l", SqlDbType.NVarChar, 10);

                    

                    cmdb.Parameters["@a"].Value = indeks;
                    cmdb.Parameters["@b"].Value = nazwa_t.Text;
                    cmdb.Parameters["@c"].Value = kod_t.Text;
                    cmdb.Parameters["@d"].Value = vat_t.Text;
                    cmdb.Parameters["@e"].Value = jm_t.Text;
                    cmdb.Parameters["@f"].Value = asorcik1;
                    cmdb.Parameters["@g"].Value = ilosc_t.Text;
                    cmdb.Parameters["@h"].Value = cena_t.Text;
                    cmdb.Parameters["@i"].Value = wopak_t.Text;
                    cmdb.Parameters["@j"].Value = cenasp_t.Text;
                    cmdb.Parameters["@k"].Value = ebid;
                    cmdb.Parameters["@l"].Value = wymagane_t.Text;
                    cmdb.Prepare();
                    cmdb.ExecuteNonQuery();

                    SqlCeCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
                    cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 30);
                    cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 30);
                    cmd1.Parameters["@k"].Value = kodzik;
                    cmd1.Parameters["@d"].Value = indeks;
                    cmd1.Prepare();
                    zliczono = "0";

                    SqlCeDataReader dr1 = cmd1.ExecuteReader();
                    while (dr1.Read())
                    {
                        zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
                    }

                    if (decimal.Parse(zliczono) >= decimal.Parse(wymagane_t.Text))
                    {


                        SqlCeCommand cmdc = cn.CreateCommand();
                        cmdc.CommandText = "UPDATE edibody SET status = 'Ok', complete = 1 WHERE id = ?";
                        cmdc.Parameters.Add("@a", SqlDbType.Int, 10);

                        cmdc.Parameters["@a"].Value = ebid;
                        cmdc.Prepare();
                        cmdc.ExecuteNonQuery();
                    }
                    else if (decimal.Parse(zliczono) < decimal.Parse(wymagane_t.Text))
                    {
                        SqlCeCommand cmdc = cn.CreateCommand();
                        cmdc.CommandText = "UPDATE edibody SET status = 'W trakcie', complete = 0 WHERE id = ?";
                        cmdc.Parameters.Add("@a", SqlDbType.Int, 10);

                        cmdc.Parameters["@a"].Value = ebid;
                        cmdc.Prepare();
                        cmdc.ExecuteNonQuery();
                    }

                    int toclose = 0;
                    SqlCeCommand cmd2 = cn.CreateCommand();
                    cmd2.CommandText = "SELECT count(id), complete FROM edibody WHERE NrDok = ? and complete = 0 GROUP BY complete";
                    cmd2.Parameters.Add("@d", SqlDbType.NVarChar, 30);
                    cmd2.Parameters["@d"].Value = indeks;
                    cmd2.Prepare();
                    SqlCeDataReader dr2 = cmd2.ExecuteReader();
                    while (dr2.Read())
                    {
                        toclose = dr2.GetInt32(0);
                    }

                    if (toclose == 0)
                    {
                        SqlCeCommand cmdf = cn.CreateCommand();
                        cmdf.CommandText = "UPDATE edihead SET status = 'OK', complete = 1 WHERE NrDok = ?";
                        cmdf.Parameters.Add("@a", SqlDbType.NVarChar, 30);

                        cmdf.Parameters["@a"].Value = indeks;
                        cmdf.Prepare();
                        cmdf.ExecuteNonQuery();
                    }



                    cn.Close();
                    form17.Loaddata();
                    this.Close();
                }
                else if (czydodac == 0)
                {
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();

                    SqlCeCommand cmdb = cn.CreateCommand();
                    cmdb.CommandText = "INSERT INTO edibody (NrDok, Nazwa, kod, Vat, Jm, Asortyment, Ilosc, Cena, IleWOpak, CenaSp, status, complete) VALUES (@a, @b, @c, @d, @e, @f, @g, @a0, @a2, @3, @4, @5)";
                  cmdb.Parameters.Add("@a", SqlDbType.NVarChar, 30);
                    cmdb.Parameters.Add("@b", SqlDbType.NVarChar, 120);
                    cmdb.Parameters.Add("@c", SqlDbType.NVarChar, 20);
                    cmdb.Parameters.Add("@d", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@e", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@f", SqlDbType.NVarChar, 120);
                    cmdb.Parameters.Add("@g", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@a0", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@a2", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@a3", SqlDbType.NVarChar, 10);
                    cmdb.Parameters.Add("@a4", SqlDbType.NVarChar, 20);
                    cmdb.Parameters.Add("@a5", SqlDbType.Bit);
                   

                  cmdb.Parameters["@a"].Value = Convert.ToString(indeks);
                    cmdb.Parameters["@b"].Value = nazwa_t.Text;
                    cmdb.Parameters["@c"].Value = kod_t.Text;
                    cmdb.Parameters["@d"].Value = vat_t.Text;
                    cmdb.Parameters["@e"].Value = jm_t.Text;
                    cmdb.Parameters["@f"].Value = asorcik1;
                    cmdb.Parameters["@g"].Value = ilosc_t.Text;
                    cmdb.Parameters["@a0"].Value = cena_t.Text;
                    cmdb.Parameters["@a2"].Value = wopak_t.Text;
                    cmdb.Parameters["@a3"].Value = cenasp_t.Text;
                    cmdb.Parameters["@a4"].Value = "Nowy";
                    cmdb.Parameters["@a5"].Value = byte.Parse("0");
                    cmdb.Prepare();
                    cmdb.ExecuteNonQuery();

                    cn.Close();
                    form17.Loaddata();
                    this.Close();
                }
                else if (czydodac == 2)
                {

                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();

                    SqlCeCommand cmdb = cn.CreateCommand();
                    cmdb.CommandText = "Update edibody set Ilosc = ? WHERE id = ?";
                    cmdb.Parameters.Add("@a", SqlDbType.NVarChar, 20);
                    cmdb.Parameters.Add("@b", SqlDbType.Int, 10);


                    cmdb.Prepare();

                    cmdb.Parameters["@a"].Value = ilosc_t.Text;
                    cmdb.Parameters["@b"].Value = ebid;


                    cmdb.ExecuteNonQuery();

                    SqlCeCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
                    cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 30);
                    cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 30);
                    cmd1.Parameters["@k"].Value = kodzik;
                    cmd1.Parameters["@d"].Value = indeks;
                    cmd1.Prepare();
                    zliczono = "0";

                    SqlCeDataReader dr1 = cmd1.ExecuteReader();
                    while (dr1.Read())
                    {
                        zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
                    }

                    if (decimal.Parse(zliczono) >= decimal.Parse(wymagane_t.Text))
                    {


                        SqlCeCommand cmdc = cn.CreateCommand();
                        cmdc.CommandText = "UPDATE edibody SET status = 'Ok', complete = 1 WHERE id = ?";
                        cmdc.Parameters.Add("@a", SqlDbType.Int, 10);
                        cmdc.Prepare();
                        cmdc.Parameters["@a"].Value = ebid;
                        cmdc.ExecuteNonQuery();
                    }
                    else if (decimal.Parse(zliczono) < decimal.Parse(wymagane_t.Text))
                    {
                        SqlCeCommand cmdc = cn.CreateCommand();
                        cmdc.CommandText = "UPDATE edibody SET status = 'W trakcie', complete = 0 WHERE id = ?";
                        cmdc.Parameters.Add("@a", SqlDbType.Int, 10);
                        cmdc.Prepare();
                        cmdc.Parameters["@a"].Value = ebid;
                        cmdc.ExecuteNonQuery();
                    }

                    cn.Close();
                    form17.Loaddata();
                    this.Close();
                }
                else if (czydodac == 3)
                {
                    string connectionString;
                    connectionString = "DataSource=Baza.sdf; Password=matrix1";
                    SqlCeConnection cn = new SqlCeConnection(connectionString);
                    cn.Open();

                    SqlCeCommand cmdb = cn.CreateCommand();
                    cmdb.CommandText = "Update fedibody set Ilosc = ? WHERE id = ?";
                    cmdb.Parameters.Add("@a", SqlDbType.NVarChar, 20);
                    cmdb.Parameters.Add("@b", SqlDbType.Int, 10);


                    cmdb.Prepare();

                    cmdb.Parameters["@a"].Value = ilosc_t.Text;
                    cmdb.Parameters["@b"].Value = ebid;


                    cmdb.ExecuteNonQuery();

                    SqlCeCommand cmd1 = cn.CreateCommand();
                    cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
                    cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 20);
                    cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 20);
                    cmd1.Parameters["@k"].Value = kodzik;
                    cmd1.Parameters["@d"].Value = indeks;
                    cmd1.Prepare();
                    zliczono = "0";

                    SqlCeDataReader dr1 = cmd1.ExecuteReader();
                    while (dr1.Read())
                    {
                        zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
                    }

                    if (decimal.Parse(zliczono) >= decimal.Parse(wymagane_t.Text))
                    {


                        SqlCeCommand cmdc = cn.CreateCommand();
                        cmdc.CommandText = "UPDATE edibody SET status = 'Ok', complete = 1 WHERE id = ?";
                        cmdc.Parameters.Add("@a", SqlDbType.Int, 10);
                        cmdc.Prepare();
                        cmdc.Parameters["@a"].Value = ebid;
                        cmdc.ExecuteNonQuery();
                    }
                    else if (decimal.Parse(zliczono) < decimal.Parse(wymagane_t.Text))
                    {
                        SqlCeCommand cmdc = cn.CreateCommand();
                        cmdc.CommandText = "UPDATE edibody SET status = 'W trakcie', complete = 0 WHERE id = ?";
                        cmdc.Parameters.Add("@a", SqlDbType.Int, 10);
                        cmdc.Prepare();
                        cmdc.Parameters["@a"].Value = ebid;
                        cmdc.ExecuteNonQuery();
                    }


                    cn.Close();
                    form17.Loaddata();
                    this.Close();
               }
            
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
			this.kod_t = new System.Windows.Forms.TextBox();
			this.kod_l = new System.Windows.Forms.Label();
			this.cena_t = new System.Windows.Forms.TextBox();
			this.cena_l = new System.Windows.Forms.Label();
			this.wopak_t = new System.Windows.Forms.TextBox();
			this.stan_l = new System.Windows.Forms.Label();
			this.nazwa_l = new System.Windows.Forms.Label();
			this.nazwa_t = new System.Windows.Forms.TextBox();
			this.ilosc_t = new System.Windows.Forms.TextBox();
			this.ilosc_l = new System.Windows.Forms.Label();
			this.exit_b = new System.Windows.Forms.Button();
			this.ok_b = new System.Windows.Forms.Button();
			this.zliczono_t = new System.Windows.Forms.TextBox();
			this.zliczono_l = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.vat_t = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.cenasp_t = new System.Windows.Forms.TextBox();
			this.wymagane_t = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.jm_t = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.status_t = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.rezlicz_b = new System.Windows.Forms.Button();
			// 
			// kod_t
			// 
			this.kod_t.BackColor = System.Drawing.Color.Azure;
			this.kod_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.kod_t.ForeColor = System.Drawing.Color.Black;
			this.kod_t.Location = new System.Drawing.Point(8, 24);
			this.kod_t.Size = new System.Drawing.Size(160, 24);
			this.kod_t.Text = "";
			// 
			// kod_l
			// 
			this.kod_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.kod_l.ForeColor = System.Drawing.Color.Gold;
			this.kod_l.Location = new System.Drawing.Point(0, 8);
			this.kod_l.Size = new System.Drawing.Size(168, 16);
			this.kod_l.Text = "Kod";
			this.kod_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cena_t
			// 
			this.cena_t.AcceptsReturn = true;
			this.cena_t.BackColor = System.Drawing.Color.Azure;
			this.cena_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.cena_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.cena_t.Location = new System.Drawing.Point(8, 104);
			this.cena_t.ReadOnly = true;
			this.cena_t.Size = new System.Drawing.Size(80, 24);
			this.cena_t.Text = "";
			// 
			// cena_l
			// 
			this.cena_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.cena_l.ForeColor = System.Drawing.Color.Gold;
			this.cena_l.Location = new System.Drawing.Point(8, 88);
			this.cena_l.Size = new System.Drawing.Size(80, 16);
			this.cena_l.Text = "CenaZk (net)";
			this.cena_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// wopak_t
			// 
			this.wopak_t.BackColor = System.Drawing.Color.Azure;
			this.wopak_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.wopak_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.wopak_t.Location = new System.Drawing.Point(176, 144);
			this.wopak_t.ReadOnly = true;
			this.wopak_t.Size = new System.Drawing.Size(56, 24);
			this.wopak_t.Text = "";
			// 
			// stan_l
			// 
			this.stan_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.stan_l.ForeColor = System.Drawing.Color.Gold;
			this.stan_l.Location = new System.Drawing.Point(176, 128);
			this.stan_l.Size = new System.Drawing.Size(56, 16);
			this.stan_l.Text = "W opak";
			this.stan_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nazwa_l
			// 
			this.nazwa_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.nazwa_l.ForeColor = System.Drawing.Color.Gold;
			this.nazwa_l.Location = new System.Drawing.Point(0, 48);
			this.nazwa_l.Size = new System.Drawing.Size(232, 16);
			this.nazwa_l.Text = "Nazwa";
			this.nazwa_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// nazwa_t
			// 
			this.nazwa_t.BackColor = System.Drawing.Color.Azure;
			this.nazwa_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.nazwa_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.nazwa_t.Location = new System.Drawing.Point(8, 64);
			this.nazwa_t.Multiline = true;
			this.nazwa_t.ReadOnly = true;
			this.nazwa_t.Size = new System.Drawing.Size(224, 24);
			this.nazwa_t.Text = "";
			this.nazwa_t.LostFocus += new System.EventHandler(this.nazwa_t_LostFocus);
			this.nazwa_t.GotFocus += new System.EventHandler(this.nazwa_t_GotFocus);
			// 
			// ilosc_t
			// 
			this.ilosc_t.BackColor = System.Drawing.Color.Gold;
			this.ilosc_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.ilosc_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.ilosc_t.Location = new System.Drawing.Point(8, 144);
			this.ilosc_t.Size = new System.Drawing.Size(80, 24);
			this.ilosc_t.Text = "";
			this.ilosc_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.ilosc_t_KeyPress);
			// 
			// ilosc_l
			// 
			this.ilosc_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.ilosc_l.ForeColor = System.Drawing.Color.Gold;
			this.ilosc_l.Location = new System.Drawing.Point(8, 128);
			this.ilosc_l.Size = new System.Drawing.Size(80, 16);
			this.ilosc_l.Text = "Wpisz IloúÊ";
			this.ilosc_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// exit_b
			// 
			this.exit_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.exit_b.Location = new System.Drawing.Point(160, 216);
			this.exit_b.Size = new System.Drawing.Size(72, 40);
			this.exit_b.Text = "WYJåCIE";
			this.exit_b.Click += new System.EventHandler(this.exit_b_Click);
			// 
			// ok_b
			// 
			this.ok_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.ok_b.Location = new System.Drawing.Point(8, 216);
			this.ok_b.Size = new System.Drawing.Size(72, 40);
			this.ok_b.Text = "OK";
			this.ok_b.Click += new System.EventHandler(this.ok_b_Click);
			// 
			// zliczono_t
			// 
			this.zliczono_t.BackColor = System.Drawing.Color.Azure;
			this.zliczono_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.zliczono_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.zliczono_t.Location = new System.Drawing.Point(8, 184);
			this.zliczono_t.ReadOnly = true;
			this.zliczono_t.Size = new System.Drawing.Size(80, 24);
			this.zliczono_t.Text = "";
			// 
			// zliczono_l
			// 
			this.zliczono_l.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.zliczono_l.ForeColor = System.Drawing.Color.Gold;
			this.zliczono_l.Location = new System.Drawing.Point(8, 168);
			this.zliczono_l.Size = new System.Drawing.Size(80, 16);
			this.zliczono_l.Text = "Zliczono";
			this.zliczono_l.TextAlign = System.Drawing.ContentAlignment.TopCenter;
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
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(168, 8);
			this.button1.Size = new System.Drawing.Size(64, 40);
			this.button1.Text = "KLAW.";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label3.ForeColor = System.Drawing.Color.Gold;
			this.label3.Location = new System.Drawing.Point(176, 88);
			this.label3.Size = new System.Drawing.Size(56, 16);
			this.label3.Text = "VAT %";
			this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// vat_t
			// 
			this.vat_t.BackColor = System.Drawing.Color.Azure;
			this.vat_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.vat_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.vat_t.Location = new System.Drawing.Point(176, 104);
			this.vat_t.Size = new System.Drawing.Size(56, 24);
			this.vat_t.Text = "";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.Gold;
			this.label2.Location = new System.Drawing.Point(88, 88);
			this.label2.Size = new System.Drawing.Size(88, 16);
			this.label2.Text = "CenaSp (brt)";
			this.label2.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// cenasp_t
			// 
			this.cenasp_t.BackColor = System.Drawing.Color.Azure;
			this.cenasp_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.cenasp_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.cenasp_t.Location = new System.Drawing.Point(88, 104);
			this.cenasp_t.Size = new System.Drawing.Size(88, 24);
			this.cenasp_t.Text = "";
			// 
			// wymagane_t
			// 
			this.wymagane_t.BackColor = System.Drawing.Color.Gold;
			this.wymagane_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.wymagane_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.wymagane_t.Location = new System.Drawing.Point(88, 144);
			this.wymagane_t.ReadOnly = true;
			this.wymagane_t.Size = new System.Drawing.Size(88, 24);
			this.wymagane_t.Text = "";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = System.Drawing.Color.Gold;
			this.label4.Location = new System.Drawing.Point(88, 128);
			this.label4.Size = new System.Drawing.Size(88, 16);
			this.label4.Text = "Wymagane";
			this.label4.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// jm_t
			// 
			this.jm_t.BackColor = System.Drawing.Color.Azure;
			this.jm_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.jm_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.jm_t.Location = new System.Drawing.Point(88, 184);
			this.jm_t.ReadOnly = true;
			this.jm_t.Size = new System.Drawing.Size(56, 24);
			this.jm_t.Text = "";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = System.Drawing.Color.Gold;
			this.label5.Location = new System.Drawing.Point(88, 168);
			this.label5.Size = new System.Drawing.Size(56, 16);
			this.label5.Text = "JM";
			this.label5.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// status_t
			// 
			this.status_t.BackColor = System.Drawing.Color.Azure;
			this.status_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.status_t.ForeColor = System.Drawing.Color.MidnightBlue;
			this.status_t.Location = new System.Drawing.Point(144, 184);
			this.status_t.ReadOnly = true;
			this.status_t.Size = new System.Drawing.Size(88, 24);
			this.status_t.Text = "";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.label6.ForeColor = System.Drawing.Color.Gold;
			this.label6.Location = new System.Drawing.Point(144, 168);
			this.label6.Size = new System.Drawing.Size(88, 16);
			this.label6.Text = "Status";
			this.label6.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// rezlicz_b
			// 
			this.rezlicz_b.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.rezlicz_b.Location = new System.Drawing.Point(80, 216);
			this.rezlicz_b.Size = new System.Drawing.Size(80, 40);
			this.rezlicz_b.Text = "ROZLICZ";
			this.rezlicz_b.Click += new System.EventHandler(this.rezlicz_b_Click);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form18
			// 
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(234, 294);
			this.ControlBox = false;
			this.Controls.Add(this.rezlicz_b);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.status_t);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.jm_t);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.wymagane_t);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.vat_t);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.cenasp_t);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.zliczono_l);
			this.Controls.Add(this.zliczono_t);
			this.Controls.Add(this.ok_b);
			this.Controls.Add(this.exit_b);
			this.Controls.Add(this.ilosc_l);
			this.Controls.Add(this.ilosc_t);
			this.Controls.Add(this.nazwa_t);
			this.Controls.Add(this.nazwa_l);
			this.Controls.Add(this.stan_l);
			this.Controls.Add(this.wopak_t);
			this.Controls.Add(this.cena_l);
			this.Controls.Add(this.cena_t);
			this.Controls.Add(this.kod_l);
			this.Controls.Add(this.kod_t);
			this.Text = "Wprowadzanie Pozycji";

		}
		#endregion

		private void ilosc_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
			{
				e.Handled = true;
			}

			// only allow one decimal point
			if (e.KeyChar == '.' 
				&& (sender as TextBox).Text.IndexOf('.') > -1)
			{
				e.Handled = true;
			}

			if (e.KeyChar == 13)
			{
				if (ilosc_t.Text != "" && kod_t.Text != "")
				{
					//ilosc_t.Focus();
					WriteLine();
				
				}
				else if (ilosc_t.Text == "")
				{
					MessageBox.Show("Wprowadü iloúÊ");
				}
				else if (kod_t.Text == "")
				{
					MessageBox.Show("Wprowadü kod towaru");
				}		
				
			}
		}

		private void ok_b_Click(object sender, System.EventArgs e)
		{
			if (kod_t.Text != "" && ilosc_t.Text != "")
			{
				WriteLine();
				
		
			}
			else if (kod_t.Text == "")
			{
				MessageBox.Show("Wprowadü kod towaru");
			}
			else if (ilosc_t.Text == "")
			{
				MessageBox.Show("Wprowadü iloúÊ");
			}


		}

		private void exit_b_Click(object sender, System.EventArgs e)
		{
			this.Close();
			
		}

		

		private void nazwa_t_GotFocus(object sender, System.EventArgs e)
		{
		inputPanel1.Enabled = true;
		}

		/*private void inputPanel1_EnabledChanged(object sender, System.EventArgs e)
		{
			if (inputPanel1.Enabled == false)
			{
				// The SIP is disabled, so set the height of the tab control
				// to its original height with a variable (TabOriginalHeight),
				// which is determined during initialization of the form.
				VisibleRect = inputPanel1.VisibleDesktop;
				tabControl1.Height = TabOriginalHeight;
			}
			else
			{
				// The SIP is enabled, so the height of the tab control
				// is set to the height of the visible desktop area.
				VisibleRect = inputPanel1.VisibleDesktop;
				tabControl1.Height = VisibleRect.Height;
			}

			// The Bounds property always returns a width of 240 and a height of 80
			// pixels for Pocket PCs, regardless of whether or not the SIP is enabled.
			BoundsRect = inputPanel1.Bounds;

			// Show the VisibleDestkop and Bounds values
			// on the second tab for demonstration purposes.
			VisibleInfo.Text = String.Format("VisibleDesktop: X = {0}, " + "Y = {1}, Width = {2}, Height = {3}", VisibleRect.X,	VisibleRect.Y, VisibleRect.Width, VisibleRect.Height);
			BoundsInfo.Text = String.Format("Bounds: X = {0}, Y = {1}, " + "Width = {2}, Height = {3}", BoundsRect.X, BoundsRect.Y,	BoundsRect.Width, BoundsRect.Height);
		}*/

		private void nazwa_t_LostFocus(object sender, System.EventArgs e)
		{
		inputPanel1.Enabled = false;
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

		private void rezlicz_b_Click(object sender, System.EventArgs e)
		{
			DialogResult result = MessageBox.Show("Czy napewno chcesz zatwierdziÊ", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			if (result == DialogResult.Yes)
			{
				string connectionString;
				connectionString = "DataSource=Baza.sdf; Password=matrix1";
				SqlCeConnection cn = new SqlCeConnection(connectionString);
				cn.Open();
				
				SqlCeCommand cmdc = cn.CreateCommand();
				cmdc.CommandText = "UPDATE edibody SET status = 'Ok', complete = 1 WHERE id = ?";
				cmdc.Parameters.Add("@a", SqlDbType.Int, 10);
					
				cmdc.Parameters["@a"].Value = ebid;
				cmdc.Prepare();
				cmdc.ExecuteNonQuery();
				cn.Close();
			}
			this.Close();
		}

		
	
				

	}
}
