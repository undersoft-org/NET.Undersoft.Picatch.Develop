using System;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Data.SqlServerCe;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using CalibCs;
using System.Net;
using System.Net.Sockets;
using System.Globalization;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;
using InTheHand.Windows.Forms;
using InTheHand.Net.Ports;
using System.Threading;

namespace SmartDeviceApplication2
{
	/// <summary>
	/// Summary description for Form5.
	/// </summary>
	public class Form16 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label ile;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label zile;
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
		private byte testflag;
		private SmartDeviceApplication2.Form15 form15;
		private byte[] data = new byte[1024];
		private BluetoothClient client;

		public Form16(SmartDeviceApplication2.Form15 form15a)
		{
			//
			// Required for Windows Form Designer support
			//
			form15 = form15a;
			string connectionString;
			string fileName = "Baza.sdf";
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
				com = dr.GetString(2);
				ip = dr.GetString(3);
				ufile = dr.GetString(4);
				dfile = dr.GetString(5);
				bdll = dr.GetString(6);
				bflag = dr.GetBoolean(7);
				ipflag = dr.GetBoolean(8);
				port = dr.GetInt32(9);
				skaner = dr.GetString(10);
			}
			cn.Close();

			InitializeComponent();
		

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}
		
		private void recivetcp()
		{
			byte[] SendingBuffer = null;
			TcpClient client = null;
			NetworkStream netstream = null;
			byte[] sendorec = new byte[1];
			sendorec[0] = 21;
			byte[] RecData = new byte[1024];
			int RecBytes;
			
			try
			{
				client = new TcpClient(ip, port);
				label1.Text = "Po³¹czony z agentem";
				label1.Refresh();
				netstream = client.GetStream();
				netstream.Write(sendorec, 0, 1);
				
				
				int totalrecbytes = 0;
				int kropka = 0;
				FileStream Fs = new FileStream(dfile, FileMode.Create, FileAccess.Write);
				while ((RecBytes = netstream.Read(RecData, 0, RecData.Length)) > 0)
				{
					Fs.Write(RecData, 0, RecBytes);
					totalrecbytes += RecBytes;
					kropka += 1;
					if (kropka > 4)
					{
						kropka = 0;
					}
					label1.Text = "Odbieram dane" ;
					for (int i = 0; i <= kropka; i++)
					{
						label1.Text += ".";
					}
					label1.Refresh();

				}
				Fs.Close();
				testflag = 1;
				netstream.Close();
				client.Close();
				label1.Text = "DANE ZAPISANE";
				label1.Refresh();
			
			}
			catch (SocketException)
			{
				MessageBox.Show("B³¹d sieci. Nie mo¿na po³¹czyæ siê z agentem");
				testflag = 0;
			}
			
		
		}
		
		
		
		private void recive()
		{
			
			if (bflag == true && bdll == "BTLibCs")
			{
				CalibCs.BTLibCs.BTLib_Initialize();
				this.label1.Text = "OTWIERAM PORT";
				label1.Refresh();
				IntPtr btest = CalibCs.SerialFuncCs.CreateFile(com+":", CalibCs.SerialFuncCs.GENERIC_WRITE | CalibCs.SerialFuncCs.GENERIC_READ, 0, IntPtr.Zero, CalibCs.SerialFuncCs.OPEN_EXISTING, CalibCs.SerialFuncCs.FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
				if (btest != IntPtr.Zero)
				{
					//	MessageBox.Show("Com ok");
					CalibCs.SerialFuncCs.DCB portconfig = new CalibCs.SerialFuncCs.DCB();
					bool test2 = CalibCs.SerialFuncCs.GetCommState(btest, ref portconfig);
					if (test2 == true)
					{
						//		MessageBox.Show("Get com st ok");
						portconfig.BaudRate = SerialFuncCs.CBR_115200;
						portconfig.Flag32 |= SerialFuncCs.fBinary;
						portconfig.Parity = SerialFuncCs.NOPARITY;
						portconfig.ByteSize = 8;
						portconfig.StopBits = SerialFuncCs.ONESTOPBIT;
						test2 = CalibCs.SerialFuncCs.SetCommState(btest, ref portconfig);
						if (test2 == true)
						{
						

						
							byte[] bytecount = new byte[1024];
							uint bytestw = new uint();
							uint byteswr = new uint();
							//uint pEvtMask = SerialFuncCs.EV_RXCHAR;
							//uint pEvtMask2 = SerialFuncCs.EV_RXCHAR;
							//uint CTS = SerialFuncCs.EV_CTS;
							//SerialFuncCs.WaitCommEvent(btest, ref pEvtMask, IntPtr.Zero);
							
							System.IO.FileStream file = new FileStream(dfile, FileMode.Create, FileAccess.Write);
							byte[] flaga = new byte[1];
							flaga[0] = 21;
							int licznik = 0;
							uint modemstat = 0;
							while(modemstat != 176 && licznik != 30)
							{
								
									licznik += 1;
				
									label1.Text = "CZEKAM NA PO£¥CZENIE 30 sekund: " + licznik.ToString();
									label1.Focus();
									System.Threading.Thread.Sleep(1000);
									label1.Refresh();
									SerialFuncCs.GetCommModemStatus(btest, ref modemstat);
				
								
								
							}

							if (licznik >= 30)
							{
								file.Close();
								CalibCs.BTLibCs.BTLib_DeInitialize();
								CalibCs.SerialFuncCs.CloseHandle(btest);
								label1.Text = "KONIEC CZASU";
								label1.Refresh();
							}
							else
							{

							
								this.label1.Text = "CZEKAM NA DANE";
								label1.Refresh();
								test2 = SerialFuncCs.WriteFile(btest, flaga, Convert.ToUInt32(flaga.Length), ref byteswr, IntPtr.Zero);
								System.Threading.Thread.Sleep(2000);

								test2 = SerialFuncCs.ReadFile(btest, bytecount, Convert.ToUInt32(bytecount.Length), ref byteswr, IntPtr.Zero);
						
								uint lenght = BitConverter.ToUInt32(bytecount, 0);	
								this.label1.Text = "ODBIERAM DANE: "+(lenght/1024).ToString()+" Kb";
								label1.Refresh();
								byte[] buffer = new byte[lenght];
								//SerialFuncCs.WaitCommEvent(btest, ref pEvtMask2, IntPtr.Zero);
								test2 = SerialFuncCs.ReadFile(btest, buffer, Convert.ToUInt32(buffer.Length), ref byteswr, IntPtr.Zero);
								flaga[0] = 2;
								test2 = SerialFuncCs.WriteFile(btest, flaga, Convert.ToUInt32(flaga.Length), ref byteswr, IntPtr.Zero);
								SerialFuncCs.CloseHandle(btest);
								CalibCs.BTLibCs.BTLib_DeInitialize();

								this.label1.Text = "ZAPISUJE DANE: "+ (lenght/1024).ToString()+ "Kb "+"PROSZÊ CZEKAÆ";
								label1.Refresh();
								file.Write(buffer, 0, Convert.ToInt32(lenght));
													
								file.Close();
								this.label1.Text = "DANE ZAPISANE";
								label1.Refresh();
							}
							
						}
					}
				}
				else
				{
					MessageBox.Show("Niepoprawny port com");
					Form15 frm15 = new Form15();
					frm15.Refresh();
					this.Close();
				}
			}
			else if (bflag == true && bdll == "MSStack")
			{
				recivebtms();
			}
		
		}
		
		private void recivebtms()
		{
            
           
			this.label1.Text = "CZEKAM NA DANE";
			label1.Refresh();
			BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;
			byte[] bytecount = new byte[1024];
			File.Delete(dfile);
            
			byte[] flaga = new byte[1];
			flaga[0] = 0;
			BluetoothListener listener = new BluetoothListener(BluetoothService.SerialPort);
			listener.Start();
			int licznik = 0;
			client = listener.AcceptBluetoothClient();
			while (client.Connected == false && licznik != 30)
			{

				licznik += 1;

				label1.Text = "CZEKAM NA PO£¥CZENIE 30 sekund: " + licznik.ToString();
				label1.Focus();
				System.Threading.Thread.Sleep(1000);
				label1.Refresh();
               



			}

			if (licznik >= 30)
			{
				listener.Stop();


				label1.Text = "KONIEC CZASU";
				label1.Refresh();
			}
			else
			{
				//    Thread.Sleep(1000);
				//timeout += 1;


				Stream peerStream = client.GetStream();

				peerStream.Write(flaga, 0, 1);

				peerStream.Read(bytecount, 0, bytecount.Length);
				Thread.Sleep(2000);
				int lenght = BitConverter.ToInt32(bytecount, 0);
				this.label1.Text = "ODBIERAM DANE: " + (lenght / 1024).ToString() + " Kb";
				label1.Refresh();
				byte[] buffer = new byte[1024];



				if (peerStream.CanRead == true)
				{

					int remaining = lenght;
					while (remaining > 0)
					{
						peerStream.Flush();

						int read = peerStream.Read(data, 0, data.Length);

						if (remaining > 0)
						{
							System.IO.FileStream file = new FileStream(dfile, FileMode.Append, FileAccess.Write);
							file.Write(data, 0, read);
							file.Close();
						}
						remaining -= read;

					}



				}

				flaga[0] = 2;
				peerStream.Write(flaga, 0, 1);
				listener.Stop();


				this.label1.Text = "ZAPISUJE DANE: " + (lenght / 1024).ToString() + "Kb " + "PROSZÊ CZEKAÆ";
				label1.Refresh();

				this.label1.Text = "DANE ZAPISANE";
				label1.Refresh();




				//BluetoothRadio.PrimaryRadio.Mode = RadioMode.PowerOff;
			}
                      
		}

		private void Loadbaza()
		{
			this.label1.Text = "PRZYGOTOWANIE DO KONWERSJI";
			label1.Refresh();
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
			 
			
			string delimeter = ";";
			string filename = dfile;
			StreamReader sr = new StreamReader(filename);
			string allData = sr.ReadToEnd();
			string[] rows = allData.Split("\r\n".ToCharArray());
			allData = "empty";
			sr.DiscardBufferedData();
			sr.Close();
			cn.Open();
			SqlCeCommand cmdh = cn.CreateCommand();
			cmdh.CommandText = "INSERT INTO edihead (FileName, TypPolskichLiter, TypDok, NrDok, Data, DataRealizacji, Magazyn, SposobPlatn, TerminPlatn, IndeksCentralny, NazwaWystawcy, AdresWystawcy, KodWystawcy, MiastoWystawcy, UlicaWystawcy, NIPWystawcy, BankWystawcy, KontoWystawcy, TelefonWystawcy, NrWystawcyWSieciSklepow, NazwaOdbiorcy, AdresOdbiorcy, KodOdbiorcy, MiastoOdbiorcy, UlicaOdbiorcy, NIPOdbiorcy, BankOdbiorcy, KontoOdbiorcy, TelefonOdbiorcy, NrOdbiorcyWSieciSklepow, DoZaplaty, status, complete) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			cmdh.Parameters.Add("@0", SqlDbType.NVarChar, 40);
			cmdh.Parameters.Add("@1", SqlDbType.NVarChar, 20);
			cmdh.Parameters.Add("@2", SqlDbType.NVarChar, 20);	
			cmdh.Parameters.Add("@3", SqlDbType.NVarChar, 30);
			cmdh.Parameters.Add("@4", SqlDbType.NVarChar, 30);
			cmdh.Parameters.Add("@5", SqlDbType.NVarChar, 30);
			cmdh.Parameters.Add("@6", SqlDbType.NVarChar, 30);
			cmdh.Parameters.Add("@7", SqlDbType.NVarChar, 10);
			cmdh.Parameters.Add("@8", SqlDbType.NVarChar, 10);
			cmdh.Parameters.Add("@9", SqlDbType.NVarChar, 10);	
			cmdh.Parameters.Add("@10", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@11", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@12", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@13", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@14", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@15", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@16", SqlDbType.NVarChar, 120);	
			cmdh.Parameters.Add("@17", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@18", SqlDbType.NVarChar, 30);
			cmdh.Parameters.Add("@19", SqlDbType.NVarChar, 20);
			cmdh.Parameters.Add("@20", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@21", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@22", SqlDbType.NVarChar, 20);
			cmdh.Parameters.Add("@23", SqlDbType.NVarChar, 120);	
			cmdh.Parameters.Add("@24", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@25", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@26", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@27", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@28", SqlDbType.NVarChar, 120);
			cmdh.Parameters.Add("@29", SqlDbType.NVarChar, 20);
			cmdh.Parameters.Add("@30", SqlDbType.NVarChar, 20);	
			cmdh.Parameters.Add("@31", SqlDbType.NVarChar, 20);
			cmdh.Parameters.Add("@32", SqlDbType.Bit);
			cmdh.Prepare();
			SqlCeCommand cmdb = cn.CreateCommand();
			cmdb.CommandText = "INSERT INTO edibody (NrDok, Nazwa, kod, Vat, Jm, Asortyment, Sww, PKWiU, Ilosc, Cena, Wartosc, IleWOpak, CenaSp, status, complete) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
			cmdb.Parameters.Add("@1", SqlDbType.NVarChar, 20);
			cmdb.Parameters.Add("@2", SqlDbType.NVarChar, 120);	
			cmdb.Parameters.Add("@3", SqlDbType.NVarChar, 20);
			cmdb.Parameters.Add("@4", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@5", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@6", SqlDbType.NVarChar, 120);
			cmdb.Parameters.Add("@7", SqlDbType.NVarChar, 20);
			cmdb.Parameters.Add("@8", SqlDbType.NVarChar, 20);
			cmdb.Parameters.Add("@9", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@10", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@11", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@12", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@13", SqlDbType.NVarChar, 10);
			cmdb.Parameters.Add("@14", SqlDbType.NVarChar, 20);
			cmdb.Parameters.Add("@15", SqlDbType.Bit);
			cmdb.Prepare();
			SqlCeCommand cmde = cn.CreateCommand();
			cmde.CommandText = "INSERT INTO ediend (NrDok, Vat, SumaNet, SumaVat, status, complete) VALUES (?, ?, ?, ?, ?, ?)";
			cmde.Parameters.Add("@1", SqlDbType.NVarChar, 30);
			cmde.Parameters.Add("@2", SqlDbType.NVarChar, 20);	
			cmde.Parameters.Add("@3", SqlDbType.NVarChar, 20);
			cmde.Parameters.Add("@4", SqlDbType.NVarChar, 20);
			cmde.Parameters.Add("@5", SqlDbType.NVarChar, 20);
			cmde.Parameters.Add("@6", SqlDbType.Bit);
			cmde.Prepare();
			label1.Text = "KONWERTOWANIE DANYCH";
			label1.Refresh();
			zile.Text = rows.Length.ToString();
			zile.Refresh();
			int i = 0;
			foreach (string r in rows)
			{
				
				
				
				string[] items = r.Split(delimeter.ToCharArray());
				if (items[0] != "" && items[0] == "edihead")
				{
					i += 1;
					ile.Text = i.ToString(); 
					ile.Refresh();
					cmdh.Parameters["@0"].Value = items[1];
					cmdh.Parameters["@1"].Value = items[2];
					cmdh.Parameters["@2"].Value = items[3];
					cmdh.Parameters["@3"].Value = items[4];
					cmdh.Parameters["@4"].Value = items[5];
					cmdh.Parameters["@5"].Value = items[6];
					cmdh.Parameters["@6"].Value = items[7];
					cmdh.Parameters["@7"].Value = items[8];
					cmdh.Parameters["@8"].Value = items[9];
					cmdh.Parameters["@9"].Value = items[10];
					cmdh.Parameters["@10"].Value = items[11];
					cmdh.Parameters["@11"].Value = items[12];
					cmdh.Parameters["@12"].Value = items[13];
					cmdh.Parameters["@13"].Value = items[14];
					cmdh.Parameters["@14"].Value = items[15];
					cmdh.Parameters["@15"].Value = items[16];
					cmdh.Parameters["@16"].Value = items[17];
					cmdh.Parameters["@17"].Value = items[18];
					cmdh.Parameters["@18"].Value = items[19];
					cmdh.Parameters["@19"].Value = items[20];
					cmdh.Parameters["@20"].Value = items[21];
					cmdh.Parameters["@21"].Value = items[22];
					cmdh.Parameters["@22"].Value = items[23];
					cmdh.Parameters["@23"].Value = items[24];
					cmdh.Parameters["@24"].Value = items[25];
					cmdh.Parameters["@25"].Value = items[26];
					cmdh.Parameters["@26"].Value = items[27];
					cmdh.Parameters["@27"].Value = items[28];
					cmdh.Parameters["@28"].Value = items[29];
					cmdh.Parameters["@29"].Value = items[30];
					cmdh.Parameters["@30"].Value = items[31];
					cmdh.Parameters["@31"].Value = items[32];
					cmdh.Parameters["@32"].Value = byte.Parse(items[33]);
					cmdh.ExecuteNonQuery();
				}

				if (items[0] != "" && items[0] == "edibody")
				{
					i += 1;
					ile.Text = i.ToString(); 
					ile.Refresh();
					cmdb.Parameters["@1"].Value = items[1];
					cmdb.Parameters["@2"].Value = items[2];
					cmdb.Parameters["@3"].Value = items[3];
					cmdb.Parameters["@4"].Value = items[4];
					cmdb.Parameters["@5"].Value = items[5];
					cmdb.Parameters["@6"].Value = items[6];
					cmdb.Parameters["@7"].Value = items[7];
					cmdb.Parameters["@8"].Value = items[8];
					cmdb.Parameters["@9"].Value = items[9];
					cmdb.Parameters["@10"].Value = items[10];
					cmdb.Parameters["@11"].Value = items[11];
					cmdb.Parameters["@12"].Value = items[12];
					cmdb.Parameters["@13"].Value = items[13];
					cmdb.Parameters["@14"].Value = items[14];
					cmdb.Parameters["@15"].Value = byte.Parse(items[15]);
					cmdb.ExecuteNonQuery();				
				}

				if (items[0] != "" && items[0] == "ediend")
				{
					i += 1;
					ile.Text = i.ToString(); 
					ile.Refresh();
					cmde.Parameters["@1"].Value = items[1];
					cmde.Parameters["@2"].Value = items[2];
					cmde.Parameters["@3"].Value = items[3];
					cmde.Parameters["@4"].Value = items[4];
					cmde.Parameters["@5"].Value = items[5];
					cmde.Parameters["@6"].Value = byte.Parse(items[6]);
					cmde.ExecuteNonQuery();
				}
					
			}
				
			cn.Close();	
			form15.Loaddata();
			this.Close();
			
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
			this.label1 = new System.Windows.Forms.Label();
			this.ile = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.zile = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.Color.Gold;
			this.label1.Location = new System.Drawing.Point(8, 8);
			this.label1.Size = new System.Drawing.Size(184, 32);
			this.label1.Text = "WCZYTYWANIE \"URUCHOM WYSY£KE Z PROGRAMU\"";
			this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// ile
			// 
			this.ile.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
			this.ile.ForeColor = System.Drawing.Color.Gold;
			this.ile.Location = new System.Drawing.Point(16, 48);
			this.ile.Size = new System.Drawing.Size(72, 16);
			this.ile.TextAlign = System.Drawing.ContentAlignment.TopRight;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Lucida Sans Unicode", 8.25F, System.Drawing.FontStyle.Regular);
			this.label2.ForeColor = System.Drawing.Color.Gold;
			this.label2.Location = new System.Drawing.Point(96, 48);
			this.label2.Size = new System.Drawing.Size(8, 16);
			this.label2.Text = "z";
			// 
			// zile
			// 
			this.zile.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Regular);
			this.zile.ForeColor = System.Drawing.Color.Gold;
			this.zile.Location = new System.Drawing.Point(112, 48);
			this.zile.Size = new System.Drawing.Size(100, 16);
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button1.Location = new System.Drawing.Point(24, 72);
			this.button1.Text = "TAK";
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Lucida Sans Unicode", 9F, System.Drawing.FontStyle.Bold);
			this.button2.Location = new System.Drawing.Point(112, 72);
			this.button2.Text = "NIE";
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// Form16
			// 
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(202, 102);
			this.ControlBox = false;
			this.Controls.Add(this.button2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.zile);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.ile);
			this.Controls.Add(this.label1);
			this.Location = new System.Drawing.Point(16, 100);
			this.Text = "Wczytywanie towarów";
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form5_KeyPress);

		}
		#endregion

		private void button1_Click(object sender, System.EventArgs e)
		{
			
			if (ipflag == true && ip != "" && port != 0)
			{
				recivetcp();
			}
			if (bflag == true && testflag != 1)
			{
				if (ipflag == true)
				{
					DialogResult result = MessageBox.Show("Czy wys³aæ przez Bletooth", "Bluetooth", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
					if (result == DialogResult.Yes)
					{
						recive();
					}
				}
				else
				{
					recive();
				}
			}
					
			if (label1.Text == "DANE ZAPISANE")
			{
				Loadbaza();
			}
			else if (label1.Text == "KONIEC CZASU")
			{
				this.Close();
			}
			
			
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		private void Form5_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			if (e.KeyChar == 8)
			{
				
				
					this.Close();
				
			}
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			Loadbaza();
		}
	}
}
