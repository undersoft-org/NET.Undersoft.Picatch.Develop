using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Diagnostics;
using ErikEJ.SqlCe;
using System.Threading;

namespace Undersoft.Picatch.Agent
{
    public partial class PICATCHEDI : Form
    {
        //OPCJE
        private string transfer;
        private string com;
        private string ip;
        private string ufile;
        private string dfile;
        private string bdll;
        private bool bflag;
        private bool ipflag;
        private int port;
        private string skaner;
        DataGridViewRow selectedRow ;
       private string connectionString;

        public PICATCHEDI(int ediflag, DataGridViewRow selected)
        {
           selectedRow = selected;
            InitializeComponent();
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();
            SqlCeCommand cmd2 = cn.CreateCommand();
            cmd2.CommandText = "SELECT * FROM opcje WHERE id = 1";
            cmd2.Prepare();
            SqlCeDataReader dr = cmd2.ExecuteReader();



            while (dr.Read())
            {


                transfer = dr.GetString(1);
                ufile = dr.GetString(4);
                dfile = dr.GetString(5);

                bflag = dr.GetBoolean(7);
                ipflag = dr.GetBoolean(8);
                //port_t.Text = Convert.ToString(dr.GetInt32(9));
                skaner = dr.GetString(10);
            }
            cn.Close();

            if (skaner == "PC-MARKET")
            {
                if (ediflag == 0)
                {
                    pcmarket_schema_import();
                }
                if (ediflag == 1)
                {
                    pcmarket_schema_export();
                }
                if (ediflag == 2)
                {
                    pcmarket_schema_export_rem();
                }
                if (ediflag == 3)
                {
                    pcmarket_schema_export_ety();
                }
            }



        }


        private void pcmarket_schema_export()
        {
            StreamWriter sw = new StreamWriter(@ufile + "\\picedi_"+ DateTime.Now.Ticks.ToString() + ".txt");

            

            string index = Convert.ToString(selectedRow.Cells[0].Value);
            string nrdok = Convert.ToString(selectedRow.Cells[4].Value);
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

            SqlCeCommand cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT * FROM fedihead WHERE id = ?";
            cmd.Parameters.Add("@d", SqlDbType.Int);
            cmd.Prepare();
            cmd.Parameters["@d"].Value = int.Parse(index);
            SqlCeDataReader dr = cmd.ExecuteReader();
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            while (dr.Read())
            {


                sw.WriteLine("TypPolskichLiter:" + dr.GetString(2));
                sw.WriteLine("TypDok:" + dr.GetString(3));
                sw.WriteLine("NrDok:" + dr.GetString(4));
                sw.WriteLine("Data:" + dr.GetString(5));
                sw.WriteLine("DataRealizacji:" + dr.GetString(6));
                sw.WriteLine("Magazyn:" + dr.GetString(7));
                sw.WriteLine("SposobPlatn:" + dr.GetString(8));
                sw.WriteLine("TerminPlatn:" + dr.GetString(9));
                sw.WriteLine("IndeksCentralny:" + dr.GetString(10));
                sw.WriteLine("NazwaWystawcy:" + dr.GetString(11));
                sw.WriteLine("AdresWystawcy:" + dr.GetString(12));
                sw.WriteLine("KodWystawcy:" + dr.GetString(13));
                sw.WriteLine("MiastoWystawcy:" + dr.GetString(14));
                sw.WriteLine("UlicaWystawcy:" + dr.GetString(15));
                sw.WriteLine("NIPWystawcy:" + dr.GetString(16));
                sw.WriteLine("BankWystawcy:" + dr.GetString(17));
                sw.WriteLine("KontoWystawcy:" + dr.GetString(18));
                sw.WriteLine("TelefonWystawcy:" + dr.GetString(19));
                sw.WriteLine("NrWystawcyWSieciSklepow:" + dr.GetString(20));
                sw.WriteLine("NazwaOdbiorcy:" + dr.GetString(21));
                sw.WriteLine("AdresOdbiorcy:" + dr.GetString(22));
                sw.WriteLine("KodOdbiorcy:" + dr.GetString(23));
                sw.WriteLine("MiastoOdbiorcy:" + dr.GetString(24));
                sw.WriteLine("UlicaOdbiorcy:" + dr.GetString(25));
                sw.WriteLine("NIPOdbiorcy:" + dr.GetString(26));
                sw.WriteLine("BankOdbiorcy:" + dr.GetString(27));
                sw.WriteLine("KontoOdbiorcy:" + dr.GetString(28));
                sw.WriteLine("TelefonOdbiorcy:" + dr.GetString(29));
                sw.WriteLine("NrOdbiorcyWSieciSklepow:" + dr.GetString(30));
                sw.WriteLine("DoZaplaty:" + dr.GetString(31));

            }

            cmd.Dispose();
            dr.Dispose();

            cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT * FROM fedibody WHERE NrDok = ?";
            cmd.Parameters.Add("@d", SqlDbType.NVarChar, 30);
            cmd.Prepare();
            cmd.Parameters["@d"].Value = nrdok;
            dr = cmd.ExecuteReader();


            while (dr.Read())
            {




                sw.WriteLine("Linia:Nazwa{" + dr.GetString(2) + "}Kod{" + dr.GetString(3) + "}Vat{" + dr.GetString(4) + "}Jm{" + dr.GetString(5) + "}Asortyment{" + dr.GetString(6) + "}Sww{}PKWiU{}Ilosc{" + dr.GetString(9) + "}Cena{" + dr.GetString(10) + "}Wartosc{}IleWOpak{" + dr.GetString(12) + "}CenaSp{" + dr.GetString(13) + "}");


            }
            cn.Close();
            sw.Close();
        }



 private void pcmarket_schema_export_rem()
        {
            StreamWriter sw = new StreamWriter(@ufile + "\\doc"+ DateTime.Now.Ticks.ToString() + ".txt", true, Encoding.GetEncoding("iso-8859-1"));

            

           // string index = Convert.ToString(selectedRow.Cells[0].Value);
           // string nrdok = Convert.ToString(selectedRow.Cells[4].Value);
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

           SqlCeCommand cmd = cn.CreateCommand();
            //cmd.CommandText = "SELECT * FROM dane WHERE bad_stan";
          //  cmd.Parameters.Add("@d", SqlDbType.Int);
        //    cmd.Prepare();
       //     cmd.Parameters["@d"].Value = int.Parse(index);
            
            System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
           


                
                
                
                
              sw.WriteLine("TypPolskichLiter:LA");
                sw.WriteLine("TypDok:RM" );
                sw.WriteLine("NrDok:REM/" + DateTime.Now.Ticks.ToString());
       //         sw.WriteLine("Data:" .ToString());
           //    sw.WriteLine("Magazyn:Mag nr 1");
        //        sw.WriteLine("SposobPlatn:GOT" );
           //     sw.WriteLine("TerminPlatn:0");
           //     sw.WriteLine("IndeksCentralny:NIE");
          //   sw.WriteLine("NazwaWystawcy:JUKADO FIRMA HANDLOWA KAROL WSZĘDYBYŁ" );
          //     sw.WriteLine("AdresWystawcy:83-110 TCZEW, " );
          //    sw.WriteLine("KodWystawcy:83-110" );
          //     sw.WriteLine("MiastoWystawcy:" );
         //      sw.WriteLine("UlicaWystawcy:" );
        //     sw.WriteLine("NIPWystawcy:" );
            //    sw.WriteLine("BankWystawcy:" + dr.GetString(17));
           //     sw.WriteLine("KontoWystawcy:" + dr.GetString(18));
            //   sw.WriteLine("TelefonWystawcy:" + dr.GetString(19));
            //   sw.WriteLine("NrWystawcyWSieciSklepow:4");
            //   sw.WriteLine("NrWystawcyObcyWSieciSklepow:");
     //     sw.WriteLine("NazwaOdbiorcy:" + dr.GetString(21));
           //     sw.WriteLine("AdresOdbiorcy:" + dr.GetString(22));
         //      sw.WriteLine("KodOdbiorcy:" + dr.GetString(23));
          //      sw.WriteLine("MiastoOdbiorcy:" + dr.GetString(24));
         //       sw.WriteLine("UlicaOdbiorcy:" + dr.GetString(25));
         //       sw.WriteLine("NIPOdbiorcy:" + dr.GetString(26));
         //       sw.WriteLine("BankOdbiorcy:" + dr.GetString(27));
          //      sw.WriteLine("KontoOdbiorcy:" + dr.GetString(28));
          //      sw.WriteLine("TelefonOdbiorcy:" + dr.GetString(29));
           //     sw.WriteLine("NrOdbiorcyWSieciSklepow:" + dr.GetString(30));
             sw.WriteLine("PoziomCen:Mag");
      //       sw.WriteLine("IloscLinii:3");



          

         //   cmd.Dispose();
         //   dr.Dispose();

            cmd = cn.CreateCommand();
            cmd.CommandText = "SELECT * FROM dane WHERE bad_stan = 1";

            SqlCeDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                string cenasp = "0";
                if (!dr.IsDBNull(5))
                {
                    cenasp = dr.GetString(5);
                }
                sw.WriteLine("Linia:Nazwa{}Kod{" + dr.GetString(1) + "}Vat{}Jm{}Asortyment{}Sww{}PKWiU{}Ilosc{" + dr.GetDecimal(11).ToString(nfi) + "}Cena{n" + dr.GetString(4) + "}Wartosc{n0.00}IleWOpak{1}"+"CenaSp{b" + cenasp + "}StanPocz{"+ dr.GetString(3)+"}"+"TowID{}"+"\n\r");
               
            }
            cn.Close();
            sw.Close();
        }



 private void pcmarket_schema_export_ety()
 {
     StreamWriter sw = new StreamWriter(@ufile + "\\doc" + DateTime.Now.Ticks.ToString() + ".txt", true, Encoding.GetEncoding("iso-8859-1"));



     // string index = Convert.ToString(selectedRow.Cells[0].Value);
     // string nrdok = Convert.ToString(selectedRow.Cells[4].Value);
     string connectionString;
     connectionString = "DataSource=Baza.sdf; Password=matrix1";
     SqlCeConnection cn = new SqlCeConnection(connectionString);
     cn.Open();

     SqlCeCommand cmd = cn.CreateCommand();
     //cmd.CommandText = "SELECT * FROM dane WHERE bad_stan";
     //  cmd.Parameters.Add("@d", SqlDbType.Int);
     //    cmd.Prepare();
     //     cmd.Parameters["@d"].Value = int.Parse(index);

     System.Globalization.NumberFormatInfo nfi = new System.Globalization.NumberFormatInfo();
     nfi.NumberDecimalSeparator = ".";







     sw.WriteLine("TypPolskichLiter:LA");
     sw.WriteLine("TypDok:PZ");
     sw.WriteLine("NrDok:PZ/" + DateTime.Now.Ticks.ToString());
     //         sw.WriteLine("Data:" .ToString());
     //    sw.WriteLine("Magazyn:Mag nr 1");
     //        sw.WriteLine("SposobPlatn:GOT" );
     //     sw.WriteLine("TerminPlatn:0");
     //     sw.WriteLine("IndeksCentralny:NIE");
     //   sw.WriteLine("NazwaWystawcy:JUKADO FIRMA HANDLOWA KAROL WSZĘDYBYŁ" );
     //     sw.WriteLine("AdresWystawcy:83-110 TCZEW, " );
     //    sw.WriteLine("KodWystawcy:83-110" );
     //     sw.WriteLine("MiastoWystawcy:" );
     //      sw.WriteLine("UlicaWystawcy:" );
     //     sw.WriteLine("NIPWystawcy:" );
     //    sw.WriteLine("BankWystawcy:" + dr.GetString(17));
     //     sw.WriteLine("KontoWystawcy:" + dr.GetString(18));
     //   sw.WriteLine("TelefonWystawcy:" + dr.GetString(19));
     //   sw.WriteLine("NrWystawcyWSieciSklepow:4");
     //   sw.WriteLine("NrWystawcyObcyWSieciSklepow:");
     //     sw.WriteLine("NazwaOdbiorcy:" + dr.GetString(21));
     //     sw.WriteLine("AdresOdbiorcy:" + dr.GetString(22));
     //      sw.WriteLine("KodOdbiorcy:" + dr.GetString(23));
     //      sw.WriteLine("MiastoOdbiorcy:" + dr.GetString(24));
     //       sw.WriteLine("UlicaOdbiorcy:" + dr.GetString(25));
     //       sw.WriteLine("NIPOdbiorcy:" + dr.GetString(26));
     //       sw.WriteLine("BankOdbiorcy:" + dr.GetString(27));
     //      sw.WriteLine("KontoOdbiorcy:" + dr.GetString(28));
     //      sw.WriteLine("TelefonOdbiorcy:" + dr.GetString(29));
     //     sw.WriteLine("NrOdbiorcyWSieciSklepow:" + dr.GetString(30));
     sw.WriteLine("PoziomCen:Mag");
     //       sw.WriteLine("IloscLinii:3");





     //   cmd.Dispose();
     //   dr.Dispose();

     cmd = cn.CreateCommand();
     cmd.CommandText = "SELECT * FROM dane WHERE bad_cena = 1";

     SqlCeDataReader dr = cmd.ExecuteReader();


     while (dr.Read())
     {




         sw.WriteLine("Linia:Nazwa{}Kod{" + dr.GetString(1) + "}Vat{}Jm{}Asortyment{}Sww{}PKWiU{}Ilosc{1}Cena{n" + dr.GetString(4) + "}Wartosc{n0.00}IleWOpak{1}" + "CenaSp{b" + dr.GetString(5) + "TowID{}" + "\n\r");

     }
     cn.Close();
     sw.Close();
 }
        
        private void pcmarket_schema_import()
        {


            string[] filePaths = Directory.GetFiles(dfile, "*.txt");
            
            string connectionString;
            connectionString = "DataSource=Baza.sdf; Password=matrix1";
            SqlCeConnection cn = new SqlCeConnection(connectionString);
            cn.Open();

            foreach (string r in filePaths)
            {
                StreamReader sr = new StreamReader(r, Encoding.Default);
                DataTable edihead = new DataTable("edihead");
                //edihead.Locale = CultureInfo.CurrentCulture;
                edihead.Columns.Add("FileName", typeof(String));
                edihead.Columns.Add("TypPolskichLiter", typeof(String));
                edihead.Columns.Add("TypDok", typeof(String));
                edihead.Columns.Add("NrDok", typeof(String));
                edihead.Columns.Add("Data", typeof(String));
                edihead.Columns.Add("DataRealizacji", typeof(String));
                edihead.Columns.Add("Magazyn", typeof(String));
                edihead.Columns.Add("SposobPlatn", typeof(String));
                edihead.Columns.Add("TerminPlatn", typeof(String));
                edihead.Columns.Add("IndeksCentralny", typeof(String));
                edihead.Columns.Add("NazwaWystawcy", typeof(String));
                edihead.Columns.Add("AdresWystawcy", typeof(String));
                edihead.Columns.Add("KodWystawcy", typeof(String));
                edihead.Columns.Add("MiastoWystawcy", typeof(String));
                edihead.Columns.Add("UlicaWystawcy", typeof(String));
                edihead.Columns.Add("NIPWystawcy", typeof(String));
                edihead.Columns.Add("BankWystawcy", typeof(String));
                edihead.Columns.Add("KontoWystawcy", typeof(String));
                edihead.Columns.Add("TelefonWystawcy", typeof(String));
                edihead.Columns.Add("NrWystawcyWSieciSklepow", typeof(String));
                edihead.Columns.Add("NazwaOdbiorcy", typeof(String));
                edihead.Columns.Add("AdresOdbiorcy", typeof(String));
                edihead.Columns.Add("KodOdbiorcy", typeof(String));
                edihead.Columns.Add("MiastoOdbiorcy", typeof(String));
                edihead.Columns.Add("UlicaOdbiorcy", typeof(String));
                edihead.Columns.Add("NIPOdbiorcy", typeof(String));
                edihead.Columns.Add("BankOdbiorcy", typeof(String));
                edihead.Columns.Add("KontoOdbiorcy", typeof(String));
                edihead.Columns.Add("TelefonOdbiorcy", typeof(String));
                edihead.Columns.Add("NrOdbiorcyWSieciSklepow", typeof(String));
                edihead.Columns.Add("DoZaplaty", typeof(String));
                edihead.Columns.Add("status", typeof(String));
                edihead.Columns.Add("complete", typeof(Boolean));

                DataTable edibody = new DataTable("edibody");
               // edibody.Locale = CultureInfo.CurrentCulture;
                edibody.Columns.Add("NrDok", typeof(String));
                edibody.Columns.Add("Nazwa", typeof(String));
                edibody.Columns.Add("Kod", typeof(String));
                edibody.Columns.Add("Vat", typeof(String));
                edibody.Columns.Add("Jm", typeof(String));
                edibody.Columns.Add("Asortyment", typeof(String));
                edibody.Columns.Add("Sww", typeof(String));
                edibody.Columns.Add("PKWiU", typeof(String));
                edibody.Columns.Add("Ilosc", typeof(String));
                edibody.Columns.Add("Cena", typeof(String));
                edibody.Columns.Add("Wartosc", typeof(String));
                edibody.Columns.Add("IleWOpak", typeof(String));
                edibody.Columns.Add("CenaSp", typeof(String));
                edibody.Columns.Add("status", typeof(String));
                edibody.Columns.Add("complete", typeof(Boolean));
                DataTable ediend = new DataTable("ediend");
              //  ediend.Locale = CultureInfo.CurrentCulture;
                ediend.Columns.Add("NrDok", typeof(String));
                ediend.Columns.Add("Vat", typeof(String));
                ediend.Columns.Add("SumaNet", typeof(String));
                ediend.Columns.Add("SumaVat", typeof(String));
                ediend.Columns.Add("status", typeof(String));
                ediend.Columns.Add("complete", typeof(Boolean));
                string typdok = "";
                string Numerdok = "";
                DataRow row = edihead.NewRow();
                row["FileName"] = Path.GetFileName(r);
                row["status"] = "Nowy";
                row["complete"] = false;
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] items = line.Split(':');
                    //make sure it has 3 items


                    if (items[0] == "TypPolskichLiter") row["TypPolskichLiter"] = items[1];
                    if (items[0] == "TypDok")
                    {
                        row["TypDok"] = items[1];
                        typdok = items[1];
                    }
                    if (items[0] == "NrDok")
                    {
                        row["NrDok"] = items[1];
                        Numerdok = items[1];

                    }

                    if (items[0] == "Data") row["Data"] = items[1];
                    if (items[0] == "DataRealizacji") row["DataRealizacji"] = items[1];
                    if (items[0] == "Magazyn") row["Magazyn"] = items[1];
                    if (items[0] == "SposobPlatn") row["SposobPlatn"] = items[1];
                    if (items[0] == "TerminPlatn") row["TerminPlatn"] = items[1];
                    if (items[0] == "IndeksCentralny") row["IndeksCentralny"] = items[1];
                    if (items[0] == "NazwaWystawcy") row["NazwaWystawcy"] = items[1];
                    if (items[0] == "AdresWystawcy") row["AdresWystawcy"] = items[1];
                    if (items[0] == "KodWystawcy") row["KodWystawcy"] = items[1];
                    if (items[0] == "MiastoWystawcy") row["MiastoWystawcy"] = items[1];
                    if (items[0] == "UlicaWystawcy") row["UlicaWystawcy"] = items[1];
                    if (items[0] == "NIPWystawcy") row["NIPWystawcy"] = items[1];
                    if (items[0] == "BankWystawcy") row["BankWystawcy"] = items[1];
                    if (items[0] == "KontoWystawcy") row["KontoWystawcy"] = items[1];
                    if (items[0] == "TelefonWystawcy") row["TelefonWystawcy"] = items[1];
                    if (items[0] == "NrWystawcyWSieciSklepow") row["NrWystawcyWSieciSklepow"] = items[1];
                    if (items[0] == "NazwaOdbiorcy") row["NazwaOdbiorcy"] = items[1];
                    if (items[0] == "AdresOdbiorcy") row["AdresOdbiorcy"] = items[1];
                    if (items[0] == "KodOdbiorcy") row["KodOdbiorcy"] = items[1];
                    if (items[0] == "MiastoOdbiorcy") row["MiastoOdbiorcy"] = items[1];
                    if (items[0] == "UlicaOdbiorcy") row["UlicaOdbiorcy"] = items[1];
                    if (items[0] == "NIPOdbiorcy") row["NIPOdbiorcy"] = items[1];
                    if (items[0] == "BankOdbiorcy") row["BankOdbiorcy"] = items[1];
                    if (items[0] == "KontoOdbiorcy") row["KontoOdbiorcy"] = items[1];
                    if (items[0] == "TelefonOdbiorcy") row["TelefonOdbiorcy"] = items[1];
                    if (items[0] == "NrOdbiorcyWSieciSklepow") row["NrOdbiorcyWSieciSklepow"] = items[1];
                    if (items[0] == "DoZaplaty") row["DoZaplaty"] = items[1];

                    if (items[0] == "Linia")
                    {
                        char[] delim = new char[2];
                        delim[0] = '{';
                        delim[1] = '}';
                        string[] linie = items[1].Split(delim);
                        DataRow row1 = edibody.NewRow();
                        row1["NrDok"] = Numerdok;
                        row1["Nazwa"] = linie[1];
                        row1["Kod"] = linie[3];
                        row1["Vat"] = linie[5];
                        row1["Jm"] = linie[7];
                        row1["Asortyment"] = linie[9];
                        row1["Sww"] = linie[11];
                        row1["PKWiU"] = linie[13];
                        row1["Ilosc"] = linie[15];
                        row1["Cena"] = linie[17];
                        row1["Wartosc"] = linie[19];
                        row1["IleWOpak"] = linie[21];
                        row1["CenaSp"] = linie[23];
                        row1["status"] = "Nowy";
                        row1["complete"] = false;

                        edibody.Rows.Add(row1);
                    }

                    if (items[0] == "Stawka")
                    {
                        char[] delim = new char[2];
                        delim[0] = '{';
                        delim[1] = '}';
                        string[] linie = items[1].Split(delim);
                        DataRow row2 = ediend.NewRow();
                        row2["NrDok"] = Numerdok;
                        row2["Vat"] = linie[1];
                        row2["SumaNet"] = linie[3];
                        row2["SumaVat"] = linie[5];
                        row2["status"] = "Nowy";
                        row2["complete"] = false;
                        ediend.Rows.Add(row2);
                    }


                }

                string testdokeh = "";
                int flagaimp = 0;
                SqlCeCommand testnrdok = cn.CreateCommand();
                testnrdok.CommandText = "SELECT NrDok From edihead";
                testnrdok.Prepare();
                
                SqlCeDataReader sdr = testnrdok.ExecuteReader();
                while (sdr.Read())
                {
                    if (sdr.IsDBNull(0) != true)
                    {
                        testdokeh = sdr.GetString(0);
                        if (testdokeh == Numerdok)
                        {
                            flagaimp = 1;
                        }
                    }

                }

                if (typdok != transfer)
                {
                    flagaimp = 1;
                }

                if (flagaimp != 1)
                {

                    edihead.Rows.Add(row);

                    SqlCeBulkCopy bulkcopy = new SqlCeBulkCopy(connectionString);

                    bulkcopy.DestinationTableName = edihead.TableName;
                    try
                    {
                        bulkcopy.WriteToServer(edihead);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    bulkcopy.DestinationTableName = edibody.TableName;
                    try
                    {
                        bulkcopy.WriteToServer(edibody);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                    bulkcopy.DestinationTableName = ediend.TableName;
                    try
                    {
                        bulkcopy.WriteToServer(ediend);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }
                    sr.Close();
                    if (Directory.Exists(dfile + "\\usunięte\\") != true)
                    {
                        Directory.CreateDirectory(dfile + "\\usunięte\\");
                    }
                    if (File.Exists(dfile + "\\usunięte\\" + Path.GetFileName(r)) != false)
                    {
                        File.Delete(dfile + "\\usunięte\\" + Path.GetFileName(r));
                    }
                    
                    File.Move(r, dfile + "\\usunięte\\" + Path.GetFileName(r));
                }
                
            }
            cn.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        
       

       

    
    
    }
}
