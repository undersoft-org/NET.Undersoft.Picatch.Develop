using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Data.SqlServerCe;
using System.Data.SqlClient;
using System.Threading;

namespace Undersoft.Picatch
{
	/// <summary>
	/// Summary description for Form3.
	/// </summary>
	public class Form17 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button delete;
		private System.Windows.Forms.Button edit;
		public System.Windows.Forms.DataGrid dataGrid1;
		private string index;
		private string index2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		public System.Windows.Forms.DataGrid dataGrid2;
		private System.Windows.Forms.TextBox kod_t;
		private System.Windows.Forms.Button search_b;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button5;
		private int rownum;
		private int poznum;
		string kodzik;
		string nazwik;
		string cenik;
		string ceniksp;
		string vacik;
		string wymagane;
		string zliczono;
		string statusik;
		string opak;
		string jedn;
		string asorcik;
		string ilosc;
		int addflag;
		int id;
		int idf;
		string ebid;
		private Microsoft.WindowsCE.Forms.InputPanel inputPanel1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button6;
		int goodfound;
		
		public Form17(string nrdok)
		{
			//
			// Required for Windows Form Designer support
			//
			
			index = nrdok;
			zliczono = "0";
			InitializeComponent();
			this.Height = Screen.PrimaryScreen.Bounds.Height;
			this.Width = Screen.PrimaryScreen.Bounds.Width;
			Update();
			Loaddata();
			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
	
		private void OpenIndex(int openflag)
		{
			try
			{
				if (openflag == 1)
				{
					poznum = dataGrid1.CurrentCell.RowNumber;
					string connectionString;
					connectionString = "DataSource=Baza.sdf; Password=matrix1";
					SqlCeConnection cn = new SqlCeConnection(connectionString);
					cn.Open();
			
		
					
					SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
					DataTable table2 = new DataTable();
					db.SelectCommand = new SqlCeCommand("SELECT kod, Nazwa, IleWOpak, Vat, Jm, Asortyment, Cena, CenaSp, status, Ilosc, id FROM edibody WHERE NrDok =  ? and complete = 0", cn);
					db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
					db.SelectCommand.Parameters["@k"].Value = index;
					db.SelectCommand.ExecuteNonQuery();
					db.Fill(table2);
			
					kodzik = table2.Rows[poznum][0].ToString();
					nazwik = table2.Rows[poznum][1].ToString();
					opak = table2.Rows[poznum][2].ToString();
					vacik = table2.Rows[poznum][3].ToString();
					jedn = table2.Rows[poznum][4].ToString();
					asorcik = table2.Rows[poznum][5].ToString();
					cenik = table2.Rows[poznum][6].ToString();
					ceniksp = table2.Rows[poznum][7].ToString();
					statusik = table2.Rows[poznum][8].ToString();
					wymagane = table2.Rows[poznum][9].ToString();
					ilosc = null;
					id = int.Parse(table2.Rows[poznum][10].ToString());
					zliczono = "0";
					SqlCeCommand cmd1 = cn.CreateCommand();
					cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
					cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
					cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 20);	
					cmd1.Parameters["@k"].Value = kodzik;
					cmd1.Parameters["@d"].Value = index;
					cmd1.Prepare();
					SqlCeDataReader dr1 = cmd1.ExecuteReader();
					while (dr1.Read())
					{
						zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
				
					}
					
			
					cn.Close();
			
					Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 1, ilosc, id);
					frm18.Show();
				}
				else if (openflag == 0)
				{
					poznum = dataGrid2.CurrentCell.RowNumber;
					string connectionString;
					connectionString = "DataSource=Baza.sdf; Password=matrix1";
					SqlCeConnection cn = new SqlCeConnection(connectionString);
					cn.Open();
			
		
					
					SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
					DataTable table2 = new DataTable();
					db.SelectCommand = new SqlCeCommand("SELECT kod, Nazwa, IleWOpak, Vat, Jm, Asortyment, Cena, CenaSp, status, Ilosc, id FROM fedibody WHERE NrDok =  ?", cn);
					db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
					db.SelectCommand.Parameters["@k"].Value = index;
					db.SelectCommand.ExecuteNonQuery();
					db.Fill(table2);
					zliczono = "0";
					kodzik = table2.Rows[poznum][0].ToString();
					nazwik = table2.Rows[poznum][1].ToString();
					opak = table2.Rows[poznum][2].ToString();
					vacik = table2.Rows[poznum][3].ToString();
					jedn = table2.Rows[poznum][4].ToString();
					asorcik = table2.Rows[poznum][5].ToString();
					cenik = table2.Rows[poznum][6].ToString();
					ceniksp = table2.Rows[poznum][7].ToString();
					statusik = table2.Rows[poznum][8].ToString();
					ilosc = table2.Rows[poznum][9].ToString();
					idf = int.Parse(table2.Rows[poznum][10].ToString());
			
					SqlCeCommand cmd1 = cn.CreateCommand();
					cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
					cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
					cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 20);	
					cmd1.Parameters["@k"].Value = kodzik;
					cmd1.Parameters["@d"].Value = index;
					cmd1.Prepare();
					SqlCeDataReader dr1 = cmd1.ExecuteReader();
					while (dr1.Read())
					{
						zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
				
					}
					
			
					cn.Close();
			
					Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 1, ilosc, idf);
					frm18.Show();
				}
				else if (openflag == 2)
				{
					poznum = dataGrid1.CurrentCell.RowNumber;
					string connectionString;
					connectionString = "DataSource=Baza.sdf; Password=matrix1";
					SqlCeConnection cn = new SqlCeConnection(connectionString);
					cn.Open();
			
		
					
					SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
					DataTable table2 = new DataTable();
					db.SelectCommand = new SqlCeCommand("SELECT kod, Nazwa, IleWOpak, Vat, Jm, Asortyment, Cena, CenaSp, status, Ilosc, id FROM edibody WHERE NrDok =  ? and complete = 0", cn);
					db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
					db.SelectCommand.Parameters["@k"].Value = index;
					db.SelectCommand.ExecuteNonQuery();
					db.Fill(table2);
			
					kodzik = table2.Rows[poznum][0].ToString();
					nazwik = table2.Rows[poznum][1].ToString();
					opak = table2.Rows[poznum][2].ToString();
					vacik = table2.Rows[poznum][3].ToString();
					jedn = table2.Rows[poznum][4].ToString();
					asorcik = table2.Rows[poznum][5].ToString();
					cenik = table2.Rows[poznum][6].ToString();
					ceniksp = table2.Rows[poznum][7].ToString();
					statusik = table2.Rows[poznum][8].ToString();
					wymagane = table2.Rows[poznum][9].ToString();
					ilosc = table2.Rows[poznum][9].ToString();
					id = int.Parse(table2.Rows[poznum][10].ToString());
					zliczono = "0";
					SqlCeCommand cmd1 = cn.CreateCommand();
					cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
					cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
					cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 20);	
					cmd1.Parameters["@k"].Value = kodzik;
					cmd1.Parameters["@d"].Value = index;
					cmd1.Prepare();
					SqlCeDataReader dr1 = cmd1.ExecuteReader();
					while (dr1.Read())
					{
						zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
				
					}
					
			
					cn.Close();
			
					Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 2, ilosc, id);
					frm18.Show();
				}
				else if (openflag == 3)
				{
				
					poznum = dataGrid2.CurrentCell.RowNumber;
				
					string connectionString;
					connectionString = "DataSource=Baza.sdf; Password=matrix1";
					SqlCeConnection cn = new SqlCeConnection(connectionString);
					cn.Open();
			
		
					
					SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
					DataTable table2 = new DataTable();
					db.SelectCommand = new SqlCeCommand("SELECT kod, Nazwa, IleWOpak, Vat, Jm, Asortyment, Cena, CenaSp, status, Ilosc, id, wymagane FROM fedibody WHERE NrDok =  ?", cn);
					db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
					db.SelectCommand.Parameters["@k"].Value = index;
					db.SelectCommand.ExecuteNonQuery();
					db.Fill(table2);
			
					kodzik = table2.Rows[poznum][0].ToString();
					nazwik = table2.Rows[poznum][1].ToString();
					opak = table2.Rows[poznum][2].ToString();
					vacik = table2.Rows[poznum][3].ToString();
					jedn = table2.Rows[poznum][4].ToString();
					asorcik = table2.Rows[poznum][5].ToString();
					cenik = table2.Rows[poznum][6].ToString();
					ceniksp = table2.Rows[poznum][7].ToString();
					statusik = table2.Rows[poznum][8].ToString();
					wymagane = table2.Rows[poznum][11].ToString();
					ilosc = table2.Rows[poznum][9].ToString();
					id = int.Parse(table2.Rows[poznum][10].ToString());
					
					zliczono = "0";
					SqlCeCommand cmd1 = cn.CreateCommand();
					cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
					cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
					cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 20);	
					cmd1.Parameters["@k"].Value = kodzik;
					cmd1.Parameters["@d"].Value = index;
					cmd1.Prepare();
					SqlCeDataReader dr1 = cmd1.ExecuteReader();
					while (dr1.Read())
					{
						zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
				
					}
					
			
					cn.Close();
			
					Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 3, ilosc, id);
					frm18.Show();
				}
			}
			catch (Exception e)
			{
				MessageBox.Show("brak pozycji");
			}



		}
		private void FindIndex()
		{
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			cn.Open();
			
			string kodbuf = kod_t.Text;
			kodzik = kodbuf;


			kod_t.Text = "SZUKAM TOWARU";
			kod_t.Refresh();
			
			SqlCeCommand cmd2 = cn.CreateCommand();
			cmd2.CommandText = "SELECT kod, count(Nazwa), NrDok, complete FROM edibody WHERE kod = ? and NrDok = ? and complete = 0 group by kod, NrDok, complete";
			cmd2.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
			cmd2.Parameters.Add("@nr", SqlDbType.NVarChar, 20);	
			cmd2.Parameters["@k"].Value = kodbuf;
			cmd2.Parameters["@nr"].Value = index;
			cmd2.Prepare();
			SqlCeDataReader dr2 = cmd2.ExecuteReader();
			int rowqty = 0;
			while (dr2.Read())
			{
				rowqty = dr2.GetInt32(1);
			}

			if (rowqty > 0)
			{

			SqlCeCommand cmd = cn.CreateCommand();
			cmd.CommandText = "SELECT kod, Nazwa, IleWOpak, Vat, Jm, Asortyment, Cena, CenaSp, status, Ilosc, id FROM edibody WHERE kod = ? and NrDok = ? and complete = 0";
			cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
			cmd.Parameters.Add("@nr", SqlDbType.NVarChar, 20);	
			cmd.Parameters["@k"].Value = kodbuf;
			cmd.Parameters["@nr"].Value = index;
			
			cmd.Prepare();
			SqlCeDataReader dr = cmd.ExecuteReader();
			
			
						
				while (dr.Read())
				{
					kodzik = dr.GetString(0);
					nazwik = dr.GetString(1);
					opak = dr.GetString(2);
					vacik = dr.GetString(3);
					jedn = dr.GetString(4);
					asorcik = dr.GetString(5);
					cenik = dr.GetString(6);
					ceniksp = dr.GetString(7);
					statusik = dr.GetString(8);
					wymagane = dr.GetString(9);
					id = dr.GetInt32(10);
				}
				
				SqlCeCommand cmd1 = cn.CreateCommand();
				cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
				cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 30);	
				cmd1.Parameters["@k"].Value = kodzik;
				cmd1.Parameters["@d"].Value = index;
				cmd1.Prepare();
				SqlCeDataReader dr1 = cmd1.ExecuteReader();
				while (dr1.Read())
				{
					zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr1.GetString(2))).ToString());
				}
				
				cn.Close();
				
				
				Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 1, ilosc, id);
				frm18.Show();
			}
			else
			{
				cn.Close();
				DialogResult result = MessageBox.Show("Brak towaru na liscie do kompletacji TAK - dodaj nowy z bazy NIE - wyjœcie ANULUJ - zast¹p zaznaczony towarem z bazy", "Pytanie", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (result == DialogResult.Yes)
				{
					FindIndexInBase();
					
				}
				else if (result == DialogResult.Cancel)
				{
					FindIndexInBase();
					if (goodfound > 0)
					{
						Deleterow(1);
						Loaddata();
					}
					

				}

			}
			
			kod_t.Text = "KOD LUB Z LISTY";
			kod_t.Refresh();
			
			/*	
			else
			{
				DialogResult result = MessageBox.Show("Brak Towaru na liœcie, czy dodaæ z bazy?", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
				if (result == DialogResult.Yes)
				{
						
					kod_t.Text = kodbuf;
					nazwa_t.ReadOnly = false;
					cena_t.ReadOnly = false;
					nazwa_t.Focus();
				}
				else if (dialog == DialogResult.No)
				{
					kod_t.Text = null;
					kod_t.Focus();
				}
				else if (dialog == DialogResult.Cancel)
				{
					nazwa_t.ReadOnly = true;
					cena_t.ReadOnly = true;
					kod_t.Text = kodbuf;
					ilosc_t.Focus();
				}
			}

		}*/

		}

			private void FindIndexInBase()
			{
				string connectionString;
				connectionString = "DataSource=Baza.sdf; Password=matrix1";
				SqlCeConnection cn = new SqlCeConnection(connectionString);
				cn.Open();
				string kodbuf = kodzik;
				int rowqty = 0;
				kod_t.Text = "SZUKAM TOWARU";
				kod_t.Refresh();
				SqlCeCommand cmd2 = cn.CreateCommand();
				cmd2.CommandText = "SELECT kod, COUNT(nazwa) FROM dane WHERE kod = ? GROUP BY kod";
				cmd2.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
				cmd2.Parameters["@k"].Value = kodbuf;
				cmd2.Prepare();
				SqlCeDataReader dr1 = cmd2.ExecuteReader();
			
				while (dr1.Read())
				{
					rowqty = dr1.GetInt32(1);
				}
				goodfound = rowqty;
				if (rowqty > 0)
				{
			
					SqlCeCommand cmd = cn.CreateCommand();
					cmd.CommandText = "SELECT kod, nazwa, stan, cenazk, cenasp, vat FROM dane WHERE kod = ?";
					cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
					cmd.Parameters["@k"].Value = kodbuf;
					kodzik = kodbuf;
					
					cmd.Prepare();
					SqlCeDataReader dr = cmd.ExecuteReader();
			
				
					while (dr.Read())
					{
						nazwik = dr.GetString(1);
						cenik = dr.GetString(3);
						ceniksp = dr.GetString(4);
						vacik = dr.GetString(5);
					}
					wymagane = "0";
					zliczono = "0";
					statusik = "Nowy";
					
					opak = "1";
					jedn = "szt.";
                    asorcik = "Default";
					ilosc = null;

					Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 0, ilosc, id);
					frm18.Show();
					cn.Close();
				}
				else
				{
					DialogResult dialog = MessageBox.Show("Nie znaleziono kodu towaru czy? dodaæ mimo to - Tak, Nie dodawaæ - Nie", "Brak towaru", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
					if (dialog == DialogResult.Yes)
					{
						goodfound = 1;
						kodzik = kodbuf;
						nazwik = "Nowy";
						cenik = "0";
						ceniksp = "0";
						vacik = "0";
						wymagane = "0";
						zliczono = "0";
						statusik = "Nowy";
						
						opak = "1";
						jedn = "szt.";
						asorcik = "Default";
						ilosc = null;
						cn.Close();
						Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 0, ilosc, id);
						frm18.Show();
						
					}
					else if (dialog == DialogResult.No)
					{
						kod_t.Text = "KOD LUB Z LISTY";
						kod_t.Focus();
						cn.Close();
					}
				}

			}
		
		public void Loaddata ()
		{	
		string connectionString;
		connectionString = "DataSource=Baza.sdf; Password=matrix1";
		SqlCeConnection cn = new SqlCeConnection(connectionString);
		cn.Open();
		DataGridTableStyle ts = new DataGridTableStyle();
		id = 0;
		idf =0;
		
		SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
		DataTable table2 = new DataTable();
		db.SelectCommand = new SqlCeCommand("SELECT * FROM edibody WHERE NrDok =  ? and complete = 0", cn);
		db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
		db.SelectCommand.Parameters["@k"].Value = index;
		db.SelectCommand.ExecuteNonQuery();
		db.Fill(table2);
		
		dataGrid1.DataSource = table2.DefaultView;
		
		dataGrid1.TableStyles.Add(ts);
		dataGrid1.TableStyles[0].GridColumnStyles[0].Width = 0;
		dataGrid1.TableStyles[0].GridColumnStyles[1].Width = 0;
		dataGrid1.TableStyles[0].GridColumnStyles[2].Width = 70;
		dataGrid1.TableStyles[0].GridColumnStyles[3].Width = 70;
		dataGrid1.TableStyles[0].GridColumnStyles[4].Width = 0;
		dataGrid1.TableStyles[0].GridColumnStyles[5].Width = 25;
		dataGrid1.TableStyles[0].GridColumnStyles[6].Width = 0;
		dataGrid1.TableStyles[0].GridColumnStyles[7].Width = 0;
		dataGrid1.TableStyles[0].GridColumnStyles[8].Width = 0;
		
		SqlCeDataAdapter dc = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
		DataTable table3 = new DataTable();
		dc.SelectCommand = new SqlCeCommand("SELECT * FROM fedibody WHERE NrDok =  ?", cn);
		dc.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
		dc.SelectCommand.Parameters["@k"].Value = index;
		dc.SelectCommand.ExecuteNonQuery();
		dc.Fill(table3);
		DataGridTableStyle ts2 = new DataGridTableStyle();
		dataGrid2.DataSource = table3.DefaultView;
			dataGrid2.TableStyles.Add(ts2);
			dataGrid2.TableStyles[0].GridColumnStyles[0].Width = 0;
			dataGrid2.TableStyles[0].GridColumnStyles[1].Width = 0;
			dataGrid2.TableStyles[0].GridColumnStyles[2].Width = 0;
			dataGrid2.TableStyles[0].GridColumnStyles[3].Width = 70;
			dataGrid2.TableStyles[0].GridColumnStyles[4].Width = 70;
			dataGrid2.TableStyles[0].GridColumnStyles[5].Width = 0;
			dataGrid2.TableStyles[0].GridColumnStyles[6].Width = 25;
			dataGrid2.TableStyles[0].GridColumnStyles[7].Width = 0;
			dataGrid2.TableStyles[0].GridColumnStyles[8].Width = 0;
			dataGrid2.TableStyles[0].GridColumnStyles[9].Width = 0;


		cn.Close();
		}
		
		private void Deleterow (int delflag)
		{
			if (delflag == 1)
			{
				string connectionString;
				connectionString = "DataSource=Baza.sdf; Password=matrix1";
				SqlCeConnection cn = new SqlCeConnection(connectionString);
				cn.Open();
				SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
				DataTable table2 = new DataTable();
				db.SelectCommand = new SqlCeCommand("SELECT * FROM edibody WHERE NrDok =  ?", cn);
				db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
				db.SelectCommand.Parameters["@k"].Value = index;
				db.SelectCommand.ExecuteNonQuery();
				db.Fill(table2);
				index2 = table2.Rows[dataGrid1.CurrentCell.RowNumber][0].ToString();
				SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM edibody", cn);
				da.DeleteCommand = new SqlCeCommand("DELETE FROM edibody WHERE id =  ?", cn);
				da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);	
				da.DeleteCommand.Parameters["@k"].Value = int.Parse(index2);
				da.DeleteCommand.ExecuteNonQuery();
				SqlCeDataAdapter dc = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
				dc.DeleteCommand = new SqlCeCommand("DELETE FROM fedibody WHERE ebid =  ?", cn);
				dc.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);	
				dc.DeleteCommand.Parameters["@k"].Value = int.Parse(index2);
				dc.DeleteCommand.ExecuteNonQuery();
				
				
				cn.Close();


				
			}
			else if (delflag == 0)
			{
				string connectionString;
				connectionString = "DataSource=Baza.sdf; Password=matrix1";
				SqlCeConnection cn = new SqlCeConnection(connectionString);
				cn.Open();
				SqlCeDataAdapter db = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
				DataTable table2 = new DataTable();
				db.SelectCommand = new SqlCeCommand("SELECT * FROM fedibody WHERE NrDok =  ?", cn);
				db.SelectCommand.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
				db.SelectCommand.Parameters["@k"].Value = index;
				db.SelectCommand.ExecuteNonQuery();
				db.Fill(table2);
				index2 = table2.Rows[dataGrid2.CurrentCell.RowNumber][0].ToString();
				kodzik = table2.Rows[dataGrid2.CurrentCell.RowNumber][4].ToString();
				ebid = table2.Rows[dataGrid2.CurrentCell.RowNumber][1].ToString();
				SqlCeDataAdapter da = new SqlCeDataAdapter("SELECT * FROM fedibody", cn);
				da.DeleteCommand = new SqlCeCommand("DELETE FROM fedibody WHERE id =  ?", cn);
				da.DeleteCommand.Parameters.Add("@k", SqlDbType.Int, 10);	
				da.DeleteCommand.Parameters["@k"].Value = int.Parse(index2);
				da.DeleteCommand.ExecuteNonQuery();
				
				SqlCeCommand cmd2 = cn.CreateCommand();
				cmd2.CommandText = "SELECT kod, NrDok, Ilosc FROM edibody WHERE id = ?";
				cmd2.Parameters.Add("@k", SqlDbType.Int, 10);	
				cmd2.Parameters["@k"].Value = int.Parse(ebid);
				
				cmd2.Prepare();
				
				
				SqlCeDataReader dr3 = cmd2.ExecuteReader();
				while (dr3.Read())
				{
					wymagane = dr3.GetString(2);
				}

				SqlCeCommand cmd1 = cn.CreateCommand();
				cmd1.CommandText = "SELECT kod, NrDok, Ilosc FROM fedibody WHERE kod = ? and NrDok = ?";
				cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 30);	
				cmd1.Parameters.Add("@d", SqlDbType.NVarChar, 30);	
				cmd1.Parameters["@k"].Value = kodzik;
				cmd1.Parameters["@d"].Value = index;
				cmd1.Prepare();
				zliczono = "0";
				
				SqlCeDataReader dr4 = cmd1.ExecuteReader();
				while (dr4.Read())
				{
					zliczono = ((decimal.Parse(zliczono) + decimal.Parse(dr4.GetString(2))).ToString());
				}
				
				if (decimal.Parse(zliczono) < decimal.Parse(wymagane))
				{
				
				
					SqlCeCommand cmdc = cn.CreateCommand();
					cmdc.CommandText = "UPDATE edibody SET status = 'W trakcie', complete = 0 WHERE id = ?";
					cmdc.Parameters.Add("@1", SqlDbType.Int, 10);
					cmdc.Parameters["@1"].Value = int.Parse(ebid);
					cmdc.Prepare();
					cmdc.ExecuteNonQuery();
				}
						
				
				cn.Close();

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
			this.dataGrid1 = new System.Windows.Forms.DataGrid();
			this.delete = new System.Windows.Forms.Button();
			this.edit = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.search_b = new System.Windows.Forms.Button();
			this.kod_t = new System.Windows.Forms.TextBox();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.button3 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.button5 = new System.Windows.Forms.Button();
			this.dataGrid2 = new System.Windows.Forms.DataGrid();
			this.inputPanel1 = new Microsoft.WindowsCE.Forms.InputPanel();
			this.button1 = new System.Windows.Forms.Button();
			this.button6 = new System.Windows.Forms.Button();
			// 
			// dataGrid1
			// 
			this.dataGrid1.BackColor = System.Drawing.Color.Azure;
			this.dataGrid1.ForeColor = System.Drawing.Color.Black;
			this.dataGrid1.GridLineColor = System.Drawing.Color.Black;
			this.dataGrid1.HeaderBackColor = System.Drawing.Color.DarkViolet;
			this.dataGrid1.HeaderForeColor = System.Drawing.Color.White;
			this.dataGrid1.Location = new System.Drawing.Point(0, 48);
			this.dataGrid1.Size = new System.Drawing.Size(224, 176);
			this.dataGrid1.Text = "dataGrid1";
			// 
			// delete
			// 
			this.delete.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.delete.Location = new System.Drawing.Point(0, 24);
			this.delete.Size = new System.Drawing.Size(72, 24);
			this.delete.Text = "USUÑ";
			this.delete.Click += new System.EventHandler(this.delete_Click);
			// 
			// edit
			// 
			this.edit.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.edit.Location = new System.Drawing.Point(72, 24);
			this.edit.Size = new System.Drawing.Size(64, 24);
			this.edit.Text = "EDYTUJ";
			this.edit.Click += new System.EventHandler(this.edit_Click);
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.WhiteSmoke;
			this.label1.Location = new System.Drawing.Point(0, 296);
			this.label1.Size = new System.Drawing.Size(232, 24);
			this.label1.Text = "DARIUSZ HANC ALAXA UNDERSOFT";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.tabControl1.Location = new System.Drawing.Point(0, 40);
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(232, 256);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.dataGrid1);
			this.tabPage1.Controls.Add(this.search_b);
			this.tabPage1.Controls.Add(this.kod_t);
			this.tabPage1.Controls.Add(this.delete);
			this.tabPage1.Controls.Add(this.edit);
			this.tabPage1.Location = new System.Drawing.Point(4, 24);
			this.tabPage1.Size = new System.Drawing.Size(224, 228);
			this.tabPage1.Text = "KOMPLETACJA";
			// 
			// search_b
			// 
			this.search_b.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
			this.search_b.Location = new System.Drawing.Point(136, 0);
			this.search_b.Size = new System.Drawing.Size(88, 48);
			this.search_b.Text = "KOMPLETUJ";
			this.search_b.Click += new System.EventHandler(this.search_b_Click);
			// 
			// kod_t
			// 
			this.kod_t.BackColor = System.Drawing.Color.GreenYellow;
			this.kod_t.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.kod_t.ForeColor = System.Drawing.Color.Black;
			this.kod_t.Size = new System.Drawing.Size(136, 24);
			this.kod_t.Text = "KOD LUB Z LISTY";
			this.kod_t.LostFocus += new System.EventHandler(this.kod_t_LostFocus);
			this.kod_t.GotFocus += new System.EventHandler(this.kod_t_GotFocus);
			this.kod_t.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.kod_t_KeyPress);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.button3);
			this.tabPage2.Controls.Add(this.button2);
			this.tabPage2.Controls.Add(this.button5);
			this.tabPage2.Controls.Add(this.dataGrid2);
			this.tabPage2.Location = new System.Drawing.Point(4, 24);
			this.tabPage2.Size = new System.Drawing.Size(224, 228);
			this.tabPage2.Text = "SKOMPLETOWANE";
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button3.Size = new System.Drawing.Size(64, 24);
			this.button3.Text = "EDYTUJ";
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button2.Location = new System.Drawing.Point(64, 0);
			this.button2.Size = new System.Drawing.Size(72, 24);
			this.button2.Text = "USUÑ";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button5
			// 
			this.button5.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.button5.Location = new System.Drawing.Point(136, 0);
			this.button5.Size = new System.Drawing.Size(88, 24);
			this.button5.Text = "ZATWIERD";
			this.button5.Click += new System.EventHandler(this.button5_Click);
			// 
			// dataGrid2
			// 
			this.dataGrid2.BackColor = System.Drawing.Color.Azure;
			this.dataGrid2.ForeColor = System.Drawing.Color.Black;
			this.dataGrid2.GridLineColor = System.Drawing.Color.Black;
			this.dataGrid2.HeaderBackColor = System.Drawing.Color.DarkViolet;
			this.dataGrid2.HeaderForeColor = System.Drawing.Color.White;
			this.dataGrid2.Location = new System.Drawing.Point(0, 24);
			this.dataGrid2.Size = new System.Drawing.Size(224, 200);
			this.dataGrid2.Text = "dataGrid2";
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(2, 2);
			this.button1.Size = new System.Drawing.Size(102, 30);
			this.button1.Text = "KLAWIATURA";
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// button6
			// 
			this.button6.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Bold);
			this.button6.Location = new System.Drawing.Point(128, 2);
			this.button6.Size = new System.Drawing.Size(104, 30);
			this.button6.Text = "WYJŒCIE";
			this.button6.Click += new System.EventHandler(this.button6_Click);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// 
			// Form17
			// 
			this.BackColor = System.Drawing.Color.DodgerBlue;
			this.ClientSize = new System.Drawing.Size(240, 320);
			this.ControlBox = false;
			this.Controls.Add(this.button6);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.label1);
			this.Text = "Lista Pozycji Dokumentu";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form17_Closing);
			this.Load += new System.EventHandler(this.Form17_Load);

		}
		#endregion

		
		private void exit_b_Click(object sender, System.EventArgs e)
		{
			
			this.Close();
			Form15 frm15 = new Form15();
			frm15.Show();
		}

		private void delete_Click(object sender, System.EventArgs e)
		{
			Deleterow(1);
			Loaddata();
		}

		private void edit_Click(object sender, System.EventArgs e)
		{
			
			OpenIndex(2);
		
		}

		

		private void search_b_Click(object sender, System.EventArgs e)
		{
			if (kod_t.Text == "KOD LUB Z LISTY")
			{
				OpenIndex(1);
			}
			else
			{
				FindIndex();
			}
		}

		private void add_b_Click(object sender, System.EventArgs e)
		{
			Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 0, ilosc, id);
			frm18.Show();
		}

		private void button1_Click(object sender, System.EventArgs e)
		{
			Form18 frm18 = new Form18(index, kodzik, nazwik, cenik, ceniksp, vacik, wymagane, zliczono, statusik, opak, jedn, asorcik, this, rownum, 1, ilosc, idf);
			frm18.Show();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			Deleterow(0);
			Loaddata();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			OpenIndex(3);
		}

		private void button4_Click(object sender, System.EventArgs e)
		{
			
			this.Close();
			Form15 frm15 = new Form15();
			frm15.Show();
		}

		

		private void button5_Click(object sender, System.EventArgs e)
		{
			
			DialogResult result = MessageBox.Show("Czy napewno chcesz zatwierdziæ", "Pytanie", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
			if (result == DialogResult.Yes)
			{
				string connectionString;
				connectionString = "DataSource=Baza.sdf; Password=matrix1";
				SqlCeConnection cn = new SqlCeConnection(connectionString);
				cn.Open();
				SqlCeCommand cmdc = cn.CreateCommand();
				cmdc.CommandText = "UPDATE edihead SET status = 'OK', complete = 1 WHERE NrDok = ?";
				cmdc.Parameters.Add("@1", SqlDbType.NVarChar, 20);
					
				cmdc.Parameters["@1"].Value = index;
				cmdc.Prepare();
				cmdc.ExecuteNonQuery();
				cn.Close();
			}
		}

		

		

		private void button1_Click_1(object sender, System.EventArgs e)
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

		private void Form17_Load(object sender, System.EventArgs e)
		{
		
		}

		private void Form17_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			inputPanel1.Enabled = false;
		}

		private void button6_Click(object sender, System.EventArgs e)
		{
			this.Close();
			Form15 frm15 = new Form15();
			frm15.Show();
		}

		private void kod_t_GotFocus(object sender, System.EventArgs e)
		{
		 kod_t.Text = null;
		}

		private void kod_t_LostFocus(object sender, System.EventArgs e)
		{
		 kod_t.Text = "KOD LUB Z LISTY";
		}

		private void kod_t_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
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
				if (kod_t.Text != "" || kod_t.Text != null)
				{
					FindIndex();
				
				}
				else if (kod_t.Text == "" || kod_t.Text != null)
				{
					kod_t.Text = "KOD LUB Z LISTY";
					MessageBox.Show("WprowadŸ kod towaru");

				}		
				
			}
		}

		
		
		

		

		}
}
