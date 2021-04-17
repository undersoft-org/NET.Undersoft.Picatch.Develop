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
	public class Form5 : System.Windows.Forms.Form
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
		private byte[] data = new byte[1024];
		private System.Windows.Forms.ProgressBar progressBar1;
		private BluetoothClient client;
		private string devstat;
		
		public Form5()
		{
			//
			// Required for Windows Form Designer support
			//
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
				devstat = dr.GetString(11);
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
			sendorec[0] = 2;
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
			
			string connectionString;
			connectionString = "DataSource=Baza.sdf; Password=matrix1";
			SqlCeConnection cn = new SqlCeConnection(connectionString);
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
							flaga[0] = 0;
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
								SerialFuncCs.CloseHandle(btest);
								label1.Text = "KONIEC CZASU";
								label1.Refresh();
							}
							else
							{
								//
								//									if (devstat == "Err")
								//									{
								//										flaga[0] = 5;
								//									
								//									}
								this.label1.Text = "CZEKAM NA DANE";
								label1.Refresh();
								test2 = SerialFuncCs.WriteFile(btest, flaga, Convert.ToUInt32(flaga.Length), ref byteswr, IntPtr.Zero);
								System.Threading.Thread.Sleep(1000);

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
						else
						{
							
						}
					}
				}
				else
				{
					MessageBox.Show("Niepoprawny port com");
					this.Close();
				}
			}
			else if (bflag == true && bdll == "MSStack")
			{
				recivebtms();
			}
			//SqlCeCommand cmd6 = new SqlCeCommand("UPDATE opcje SET devstat = 'Err')", cn);
			//cmd6.Prepare();
			//cmd6.ExecuteNonQuery();
			cn.Close();
		}
		//catch
		//{
		//	string connectionString;
		//	connectionString = "DataSource=Baza.sdf; Password=matrix1";
		//	SqlCeConnection cn = new SqlCeConnection(connectionString);
		//	SqlCeCommand cmd6 = new SqlCeCommand("UPDATE opcje SET devstat = 'Err')", cn);
		//	cmd6.Prepare();
		//	cmd6.ExecuteNonQuery();
		//	cn.Close();
		//}
			
	


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
				
		private void Loadbaza ()
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
			SqlCeCommand delete = cn.CreateCommand();
			delete.CommandText = "DROP TABLE dane";
			delete.Prepare();
			delete.ExecuteNonQuery();
            SqlCeCommand cmd = new SqlCeCommand("CREATE TABLE dane (typ nvarchar (7), kod nvarchar (15), nazwa nvarchar(100), stan nvarchar(10), cenazk nvarchar(20), cenasp nvarchar(20), vat nvarchar(5), devstat nvarchar(20), bad_cena bit, bad_stan bit, cenapolka numeric(10,3), zliczono numeric(10,3))", cn);
			cmd.ExecuteNonQuery();
			
			
			/*SqlCeCommand cmd = new SqlCeCommand("UPDATE dane SET typ = ? kod = ?, nazwa  = ?, stan = ?, cenazk = ?, cenasp = ?, vat = ? WHERE kod = ?)", cn);
			cmd.Parameters.Add("@t", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@k", SqlDbType.NVarChar, 20);	
			cmd.Parameters.Add("@n", SqlDbType.NVarChar, 100);
			cmd.Parameters.Add("@s", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@cz", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@cs", SqlDbType.NVarChar, 20);
			cmd.Parameters.Add("@v", SqlDbType.NVarChar, 20);
            cmd.Parameters.Add("@k1", SqlDbType.NVarChar, 20);	
			*/

            SqlCeCommand cmd1 = cn.CreateCommand();
            cmd1.CommandText = "INSERT INTO dane (typ, kod, nazwa, stan, cenazk, cenasp, vat, bad_cena, bad_stan, cenapolka, zliczono) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
            cmd1.Parameters.Add("@t", SqlDbType.NVarChar, 20);
            cmd1.Parameters.Add("@k", SqlDbType.NVarChar, 20);
            cmd1.Parameters.Add("@n", SqlDbType.NVarChar, 120);
            cmd1.Parameters.Add("@s", SqlDbType.NVarChar, 20);
            cmd1.Parameters.Add("@cz", SqlDbType.NVarChar, 20);
            cmd1.Parameters.Add("@cs", SqlDbType.NVarChar, 20);
            cmd1.Parameters.Add("@v", SqlDbType.NVarChar, 20);
            cmd1.Parameters.Add("@bc", SqlDbType.Bit);
            cmd1.Parameters.Add("@bs", SqlDbType.Bit);
            cmd1.Parameters.Add("@cp", SqlDbType.Decimal, 10);
            cmd1.Parameters["@cp"].Precision = 10;
            cmd1.Parameters["@cp"].Scale = 3;
            cmd1.Parameters.Add("@zl", SqlDbType.Decimal, 10);
            cmd1.Parameters["@zl"].Precision = 10;
            cmd1.Parameters["@zl"].Scale = 3;
            /*SqlCeCommand cmd3 = cn.CreateCommand();
            cmd3.CommandText = "DELETE FROM dane WHERE kod = ?";
            cmd3.Parameters.Add("@t", SqlDbType.NVarChar, 20);
            */

            label1.Text = "KONWERTOWANIE DANYCH";
            label1.Refresh();
            progressBar1.Maximum = (rows.Length / 3);
            progressBar1.Minimum = 0;

            bool val;
            string input;



            input = bool.FalseString;
            val = bool.Parse(input);


            int i = 0;

            foreach (string r in rows)
            {

                string[] items = r.Split(delimeter.ToCharArray());
                if (items[0] != "")
                {

                    /* if (items[7] == "Update")
                     {
                     i += 1;
                     progressBar1.Value = i;
                     progressBar1.Update();
                     cmd.Parameters["@t"].Value = items[0];
                     cmd.Parameters["@k"].Value = items[1];
                     cmd.Parameters["@n"].Value = items[2];
                     cmd.Parameters["@s"].Value = items[3];
                     cmd.Parameters["@cz"].Value = items[4];
                     cmd.Parameters["@cs"].Value = items[5];
                     cmd.Parameters["@v"].Value = items[6];
                     cmd.Parameters["@k1"].Value = items[1];
                     cmd.ExecuteNonQuery();
                 }
                 */
                    //else if (items[7] == "New")
                    //{
                    i += 1;
                    progressBar1.Value = i;
                    progressBar1.Update();

                    //cmd3.Parameters["@k"].Value = items[1];
                    //cmd3.ExecuteNonQuery();


                    cmd1.Parameters["@t"].Value = items[0];
                    cmd1.Parameters["@k"].Value = items[1];
                    cmd1.Parameters["@n"].Value = items[2];
                    cmd1.Parameters["@s"].Value = items[3];
                    cmd1.Parameters["@cz"].Value = items[4];
                    cmd1.Parameters["@cs"].Value = items[5];
                    cmd1.Parameters["@v"].Value = items[6];
                    cmd1.Parameters["@bc"].Value = val;
                    cmd1.Parameters["@bs"].Value = val;
                    cmd1.Parameters["@cp"].Value = "0";
                    cmd1.Parameters["@zl"].Value = "0";
                    cmd1.ExecuteNonQuery();
                    //}
                    /*	else if (items[7] == "Err")
                        {
                            i += 1;
                            progressBar1.Value = i;
                            progressBar1.Update();
							
                            cmd3.Parameters["@k"].Value = items[1];
                            cmd3.ExecuteNonQuery();
							
                            progressBar1.Value = i;
                            progressBar1.Update();
                            cmd1.Parameters["@t"].Value = items[0];
                            cmd1.Parameters["@k"].Value = items[1];
                            cmd1.Parameters["@n"].Value = items[2];
                            cmd1.Parameters["@s"].Value = items[3];
                            cmd1.Parameters["@cz"].Value = items[4];
                            cmd1.Parameters["@cs"].Value = items[5];
                            cmd1.Parameters["@v"].Value = items[6];
                            cmd1.ExecuteNonQuery();
                        }
                    */


                }
            }
            /*SqlCeCommand cmd6 = new SqlCeCommand("UPDATE opcje SET devstat = 'Ok')", cn);
            cmd.Prepare();
			
            cmd6.ExecuteNonQuery();
            */
            cn.Close();


            //catch
            //{
            //	SqlCeCommand cmd6 = new SqlCeCommand("UPDATE opcje SET devstat = 'Err')", cn);
            //	cmd6.Prepare();
            //	
            //	cmd6.ExecuteNonQuery();
            //	cn.Close();
            //	
            //}


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
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
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
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(24, 48);
			this.progressBar1.Size = new System.Drawing.Size(160, 16);
			// 
			// Form5
			// 
			this.BackColor = System.Drawing.Color.MidnightBlue;
			this.ClientSize = new System.Drawing.Size(202, 102);
			this.ControlBox = false;
			this.Controls.Add(this.progressBar1);
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
	}
}
