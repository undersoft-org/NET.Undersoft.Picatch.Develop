#ifndef _posnetH_
#define _posnetH_

/*

	Nag��wek w wersji: 1.0

*/


/*!
	\mainpage Biblioteka interfejsu drukarki POSNET COMBO DF
	
	Spis Tre�ci

	- \ref sec_intro 
	- \ref sec_flow 
	- \ref sec_async
	  - \ref sec_async_events
	  .
	- \ref sec_example
	- \ref sec_extern
	- \ref linux_port
	.

	\b "SPECYFIKACJA PROTOKO�U COMBO DF 1.01 / THERMAL 3.01 v 010"

	<hr>

	\section sec_intro	Wprowadzenie

	Podstawowe za�o�enia, kt�re zosta�y przyj�te przy projektowaniu biblioteki dla
	drukarki POSNET COMBO DF s� nast�puj�ce:
	  - komunikacja za po�rednictwem RS-232, USB (VCP lub DXX)
	  - wielow�tkowo�� i wielo-urz�dzeniowo�� biblioteki (brak zmiennych globalnych)
	  - obs�uga zdarze� asynchronicznych (zmiany statusu urz�dzenia)
	  - kolejkowanie rozkaz�w w celu przyspieszenia wymiany danych
	  - wykorzystanie z r�nych j�zyk�w programowania (konwencja wywo�a� __stdcall zgodna 
		z WINAPI)
	  - minimalna zale�no�� od systemu operacyjnego w zakresie interfejsu biblioteki -
	    praktycznie tylko 2 funkcja s� typowo dedykowane dla Windows - \ref POS_GetEvent
		i \ref POS_SetMessageParams.
	  - interfejs �ledzenia (\ref debugging)
	  - natywny port dla systemu Linux
	  - maksymalna zgodno�� API z bibliotek� dla kas COMBO/NEO
	  .
    
	Ze wzgl�du na to, �e ilo�� rozkaz�w wykorzystywanych w komunikacji z drukark� jest ogromna
	przy projektowaniu biblioteki zrezygnowano z literalnej realizacji ka�dego rozkazu
	jako osobnej funkcji. Grupowanie rozkaz�w w/g rodzaju parametr�w tak�e nie jest dobrym
	rozwi�zaniem. W tej sytuacji zosta� zastosowany model "obiektu rozkazowego (Request)",
	wraz z zestawem funkcji umo�liwiaj�cych ustawianie parametr�w (\ref req_param). 

	Zastosowanie takiego rozwi�zania ma dodatkow� zalet�. Drobne korekty w rozkazach obs�ugi
	drukarki w tym dodanie nowych czy zmiana ilo�ci i rodzaju parametr�w nie wymagaj� 
	zmian w bibliotece komunikacyjnej, a jedynie dostosowania aplikacji do tych zmian.

	Po wykonaniu takiego obiektu rozkazowego przez drukark� (por. \ref sec_flow) wyniki 
	dost�pne s� w analogiczny spos�b - przez zestaw funkcji pobieraj�cych je kolejno
	z obiektu rozkazowego.

	\section sec_flow	Przep�yw informacji

	W aplikacji istniej� 2 kolejki rozkazowe, pomi�dzy kt�rymi nast�puje przep�yw danych 
	jak w grafie poni�ej.
	\dot
	digraph flow {
		node [shape=record, fontname=Helvetica, fontsize=8];
		K0 [label="Kolejka rozkazowa"  shape="ellipse" ]
		K1 [label="Kolejka wynikow"  shape="ellipse" ]	
		NOWY -> OCZEKUJACY [label="POS_PostRequest" fontname=Helvetica fontsize=8];
		OCZEKUJACY -> K0;
		K0 -> WYSLANY [label="transmisja do drukarki" fontname=Helvetica fontsize=8];
		WYSLANY -> ZAKONCZONY [label="sekwencja wykonana\n lub blad" fontname=Helvetica fontsize=8];
		ZAKONCZONY -> K1 [label="Rozkaz" fontname=Helvetica fontsize=8]
	}
	\enddot

	Ca�o�� transmisji obs�ugiwana jest przez dwa niezale�ne w�tki, jeden transmisyjny,
	drugi odbiorczy operuj�ce na odpowiednich kolejkach rozkaz�w. Dodatkowo dla ka�dego
	urz�dzenia istnieje osobny w�tek wykonuj�cy cykliczne zapytania o status urz�dzenia.

	\section sec_async	Zdarzenia asynchroniczne

	\subsection sec_async_events Zdarzenia asynchroniczne

	Drukarka POSNET COMBO DF posiada mo�liwo�� generacji 
	zdarze� zmiany statusu urz�dzenia w spos�b asynchroniczny w 
	stosunku do wykonywanych polece�
	wydawanych przez nadzoruj�c� j� aplikacj� steruj�co-magazynow�.
	
	Do obs�ugi zdarze� w tym trybie zosta� przygotowany zestaw funkcji
	opisany w module \ref event_handling. Aplikacja g��wna mo�e zosta� 
	powiadomiona o przyj�ciu nowego zdarzenia
	za pomoc�:
	  - zdarzenia IPC typu "Event" systemu Windows,
	  - komunikatu Windows wys�anego do wskazanego okna aplikacji
	
	Aplikacja mo�e te� cyklicznie sprawdza� status urzadzenia (polling).

	UWAGA: Z natury dzia�ania drukarki sprawdzenie pe�nego statusu drukarki sk�ada si�
	z dw�ch niezaleznych operacji, w zwi�zku z tym w momencie przejscia drukarki ze stanu
	poprawngo do stanu awarii mechanizmu drukuj�cego wyst�pi� dwa kolejne zdarzenia 
	asynchroniczne (pierwsze wskazuj�ce zmiane statusu drukarki, drugie statusu mechanizmu).

	UWAGA: Je�li status drukarki nie wskazuje na b��d mechanizmu drukarki warto�� statusu
	mechanizmu jest nieistotna (niewa�na).

	\section sec_example	Przyk�adowa sekwencja polece�

	Obs�uga drukarki wymaga wykonania pewnego zestawu standardowych
	operacji. Poni�szy opis przedstawia jedn� z mo�liwo�ci na
	przyk�adzie prostego paragonu. Przyk�ad jest w j�zyku C++,
	dla czytelno�ci pomini�to obs�ug� sytuacji wyj�tkowych.

	\code

	\\ Przygotowanie i otwarcie urzadzenia 
	void *hDevice=NULL;
	hDevice=POS_CreateDeviceHandle(type);
	POS_SetDebugLevel(hDevice,POSNET_DEBUG_ALL & 0xFFFFFFFE);
	POS_SetDeviceParam(hDevice,POSNET_DEV_PARAM_COMSETTINGS,(void*)"COM1,9600,8,N,1,H");
	void *hLocalDevice=POS_OpenDevice(hDevice);

	\\ Wys�anie kolejnych rozkaz�w paragonu
	\\ Wersja z uzupe�nianiem parametr�w pojedynczo
	hRequest = POS_CreateRequest(hLocalDevice,"trinit");
	POS_PushRequestParam(hRequest,"bm","1");
	POS_PostRequest(hRequest,POSNET_REQMODE_SPOOL);

	\\ Oczekiwanie na wykonanie rozkazu
	POS_WaitForRequestCompleted(hRequest,5000);

	\\ Sprawdzenie statusu wykonania
	if (POS_GetRequestStatus(hRequest) != POSNET_STATUS_OK) { OBSLUGA_BLEDU }

	\\ Usuniecie rozkazu
	POS_DestroyRequest(hRequest);

	\\ Nastepne polecenia, wersja z tworzeniem rozkazu z parametrami	
	hRequest = POS_CreateRequestEx(hLocalDevice,"trline","na,Bu�ka Standardowa\nvt,0\npr,35");
	POS_PostRequest(hRequest,POSNET_REQMODE_SPOOL);
	POS_WaitForRequestCompleted(hRequest,5000);
	if (POS_GetRequestStatus(hRequest) != POSNET_STATUS_OK) { OBSLUGA_BLEDU }
	POS_DestroyRequest(hRequest);
	hRequest = POS_CreateRequestEx(hLocalDevice,"trpayment","ty,0\nre,0\nwa,35");
	POS_PostRequest(hRequest,POSNET_REQMODE_SPOOL);
	POS_WaitForRequestCompleted(hRequest,5000);
	if (POS_GetRequestStatus(hRequest) != POSNET_STATUS_OK) { OBSLUGA_BLEDU }
	POS_DestroyRequest(hRequest);
	hRequest = POS_CreateRequestEx(hLocalDevice,"trend","to,35\nfp,35");
	POS_PostRequest(hRequest,POSNET_REQMODE_SPOOL);
	POS_WaitForRequestCompleted(hRequest,5000);
	if (POS_GetRequestStatus(hRequest) != POSNET_STATUS_OK) { OBSLUGA_BLEDU }
	POS_DestroyRequest(hRequest);

	\\ zamkniecie urzadzenia
	POS_CloseDevice(hLocalDevice);
	POS_DestroyDeviceHandle(hDevice);

	\endcode

	Biblioteka posiada zestaw aplikacji demonstruj�cych r�ne sekwencje
	polece� dla drukarki.

	  -  demo_lowlevel - demonstruje przyk�adow� realizacj� pojedynczej transakcji (paragonu)
	  -  demo_requests - prezentuje r�ne sposoby obs�ugi parametr�w polece� oraz sposobu wysy�ania rozkaz�w i odbioru danych
	  -  demo_async2 - prezentuje realizacj� obs�ugi statusu drukarki za po�rednictwem zdarze� (Event) w systemie Windows lub semafor�w dla systemu Linux
	  -  demo_async3 - prezentuje realizacj� obs�ugi statusu drukarki za po�rednictwem funkcji typu callback
	  -  demo_all - prezentuje oko�o 20 r�nych przydatnych sekwencji drukarki, w tym r�ne warianty sprzeda�y, faktur� VAT, formatki, raporty i ustawienia,
	  demo to obs�uguje zdarzenia nieprzewidywane metod� pollingu za po�rednictwem funkcji POS_GetPrnDeviceStatus
	  .
    Kod poszczeg�lnych program�w demonstracyjnych zawiera w komenarzach dodatkowe informacje na temat ich dzia�ania.

	\section sec_extern	Wykorzystanie w j�zykach programowania

	W wersji prototypowej biblioteka zosta�a przetestowana w:
	  - VisualC++ .NET 2003
	  - Borland C++ Builder 4
	  - Delphi 7
	  - Visual Basic for Applications (MS Excel)
	  - Ch  (<a href='http://www.softintegration.com'>http://www.softintegration.com</a>)
	
	Poni�ej przedstawiono przyk�adowy kod dla VBA.
	\verbatim
	Private Declare Function POS_WaitForRequestCompleted Lib "posnet.dll" (ByVal H As Long, ByVal P As Long) As Long
	\endverbatim

	Podobny kod dla Delphi 7

	\verbatim
	function POS_WaitForRequestCompleted (hDevice : THandle; k: longint) : THandle; stdcall; external 'posnet.dll'
	\endverbatim

	\section linux_port Wersja dla systemu Linux

	Natywny port dla systemu Linux sk�ada si� z bibliotek:

	libposcmbth.so.1.0

	oraz 

	libptypes.so.2.0.2 (http://www.melikyan.com/ptypes/)

	Biblioteki te s� zale�ne tak�e od dost�pnych na licencji LGPL bibliotek obs�uguj�cych
	podsystem USB:

	libusb - w wersji 0.1.11+ dost�pnej standardowo w wi�kszo�ci dystrybucji
	(http://libusb.sourceforge.net/)

	oraz 

	libftdi w wersji 0.7+ (http://www.intra2net.com/de/produkte/opensource/ftdi/index.php)
	do obs�ugi konwertera FTDI zastosowanego w urz�dzeniu.

	UWAGA!!!  W przypadku korzystania z trybu natywnego USB (POSNET_INTERFACE_USB)
	do poprawnego dzia�ania drukarki niezb�dne jest zablokowanie automatycznego �adowania
	i usuni�cie jesli jest za�adowany modu�u j�dra ftdi_sio. W przeciwnym razie po��czenie z drukark� nie b�dzie
	mo�liwe (w pliku logu zg�aszany b�dzie b��d o kodzie -5), gdy� modu� ten przejmuje kontrol� nad konwerterem
	FTDI tworz�c wirtualny port szeregowy (/dev/ttyUSBx).

	W przypadku korzystania w trybie wirtualnego portu szeregowego, sterownik ftdi_sio jest potrzebny.
*/


/*!
	\ingroup global_defs
	\brief	Standardowy spos�b obs�ugi bibliotek DLL
*/
/* Patch dla Borland C++ Builder'a */
#ifdef __WIN32__
#define WIN32 
#endif

#ifdef WIN32
#ifdef POSNET_EXPORTS
#define POSNET_API  __declspec(dllexport)	
#else
#define POSNET_API  __declspec(dllimport)	
#endif
#else
#define POSNET_API
#define __stdcall
#define __cdecl
#endif

extern "C"
{

	/*!
		\defgroup	global_defs Definicje globalne
		@{
		
	*/
	/*!
	\brief Definicja typu uchwytu urz�dzenia.
	*/
	typedef  void*					POSNET_HANDLE;
	/*!
	\brief Definicja typu warto�ci zwracanej jako status.
	*/
	typedef	 unsigned long			POSNET_STATUS;
	/*!
	\brief Definicja typu warto�ci zwracanej jako stan rozkazu.
	*/
	typedef	 unsigned long			POSNET_STATE;
	/*!
	\brief Definicja minimalnego rozmianu bufora znakowego nazwy polecenia, nazwy parametru
	*/
	#define	POSNET_NAMELENGTH_MAX	16

	/*!
	\brief	Pobierz wersj� biblioteki

	\return	Wersja jako liczba 32 bitowa. Liczba ta sk�ada si� z trzech cz�ci. Najstarsze 8 bit�w to g��wny
	 numer wersji, nast�pne 8 to podrz�dny numer wersji, oststanie 16 bit�w to numer kompilacji.
	*/

	POSNET_API unsigned long __stdcall POS_GetLibraryVersion();


	/*!
		@}
	*/


	/*! \defgroup debug_level	Poziomy informacyjno�ci
		\ingroup debugging
	@{
	*/
	#define	POSNET_DEBUG_NONE				0x00000000  //!< Brak rejestrowania informacji.
	#define POSNET_DEBUG_ALL				0xFFFFFFFF	//!< Rejestracja wszystkich informacji we wszystkich podsystemach
	#define POSNET_SUBSYSTEM_DEVICE			0x00001000	//!< Podsystem urz�dzenia
	#define POSNET_SUBSYSTEM_DEVICERS232	0x00002000  //!< Podsystem RS232
	#define	POSNET_SUBSYSTEM_DEVICEUSB		0x00008000  //!< Podsystem USB (u�ywa sterownika D2XX, w przypadku korzystania ze
														//!< sterownika VCP nale�y korzysta� z trybu RS232
	#define	POSNET_SUBSYSTEM_FRAME			0x00010000  //!< Podsystem ramki - wy�wietla zawarto�� wysy�anych i odbieranych ramek
														//!< w formacie hexadecymalnym - mo�liwo�� konwersji na posta� binarn� do��czonym konwerterem
	#define POSNET_DEBUG_EXTRA				0x00800000  //!< Rejestracja szczeg�owych informacji o stanach rozkaz�w
	/*!
	@}
	*/

	/*!
		\defgroup	debugging	Ustawienie poziomu �ledzenia (informacyjno�ci) biblioteki
		@{
	*/

	/*!
		\brief Ustaw poziom informacyjno�ci biblioteki.

		System debuggingu opiera si� na tzw. "podsystemach".\n
		Definicje POSNET_DEBUG_SUBSYSTEM_* definiuj� te podsystemy.\n
		S� one maskami bitowymi, kt�re zsumowane (OR) daja mo�liwo�� 
		jednoczesnego rejestrowania przep�ywu danych w r�znych podsystemach.\n
		4 najm�odsze bity definiuj� poziom ilo�ci wysy�anych danych (sta�e
		POSNET_DEBUG_LEVEL_*).
		
		Ustawienie poziomu debuggingu na warto�� r�n� od 0 powoduje otwarcie
		aktualnie wybranego pliku (domy�lnie "POS_DBG.txt", mo�na ustawi� przy
		wykorzystaniu \ref POS_SetDebugFileName . W pliku tym zostan� zapisane
		odpowiednie informacje umo�liwiaj�ce �ledzenie wykonania kodu biblioteki.
		Ponowne ustawienie poziomu debuggingu zamyka plik �ledz�cy.
		
		Maksymalna ilo�� podsystem�w okre�lona zosta�a na 28.
		\param hGlobalDevice	Uchwyt stworzonego po��czenia z urz�dzeniem
		\param debugLevel	Okre�lenie poziomu �ledzenia
	*/
	POSNET_API	void	__stdcall POS_SetDebugLevel(POSNET_HANDLE hGlobalDevice, unsigned long debugLevel);
	/*!
		\brief	Ustawienie nazwy pliku �ledz�cego.

		Funkcja umo�liwia zmian� domy�lnego pliku �ledz�cego.
		\param hGlobalDevice	Uchwyt stworzonego po��czenia z urz�dzeniem
		\param fileName nowa nazwa pliku
	*/
	POSNET_API  void	__stdcall POS_SetDebugFileName(POSNET_HANDLE hGlobalDevice, const char *fileName);

	/*!
		@}
	*/



	/*! 
	*	\addtogroup dev_types Rodzaje interfejsu urz�dzenia
		\ingroup basic_api

		@{
	*/

	/*!
		\brief	Pod��czenie przez RS232
	*/
	#define POSNET_INTERFACE_RS232	0x0001	
	/*!
		\brief	Pod��czenie przez USB

		Pod��czenie przez  USB mo�e by� wykonane na dwa sposoby:
		-  korzystaj�c ze sterownika FTDI VCP (Virtual ComPort), gdzie tworzony jest dodatkowy, virtualny port szeregowy (odpowiednik RS232),
		w tym przypadku nale�y korzysta� z trybu RS232.
		-  przy wykorzystaniu sterownika D2XX, korzysta si� z trybu USB. Nale�y w tym przypadku poda� zaprogramowany w interfejsie numer seryjny
		drukarki.
		.

	*/
	#define POSNET_INTERFACE_USB	0x0002

	/*! @}

	*/


	/*!
		\defgroup dev_params	Identyfikatory parametr�w urz�dzenia
		\ingroup basic_api
		@{
	*/
	
	/*!
		\brief	Parametry portu szeregowego

		Zapis/Odczyt
		
		Parametry portu przekazuje si� jako ci�g znak�w w formacie
		port,baud rate,bits,parity,stopbits,flowcontrol np. "COM1,9600,8,N,1,H"
		Flowcontrol: (N)one, (S)oftware XON/XOFF, (H)ardware RTS/CTS+DTR/DSR, (R)Hardware RTS/CTS, (D)Hardware DTR/DSR
	*/
	#define	POSNET_DEV_PARAM_COMSETTINGS	0x00020001

	/*!
		\brief Czas w [s] po jakim ma by� zaniechane wysy�anie ramki

		TYLKO ZAPIS

		Parametr - wska�nik na warto�c long
	*/
	#define POSNET_DEV_PARAM_SENDTIMEOUT		0x00020004


	/*!
		\brief Numer seryjny drukarki do otwarcia przez typ urz�dzenia \ref POSNET_INTERFACE_USB
		
		TYLKO ZAPIS

		Parametr - wska�nik na ci�g znak�w j�zyka C (zako�czony 0) zawieraj�cy numer seryjny.

	*/
	#define POSNET_DEV_PARAM_USBSERIAL			0x00020007

	/*!

		\brief Odczyt wszystkich numer�w seryjnych drukarek pod��czonych do komputera poprzez interfejs USB i sterownik
				FTDI - D2XX

		TYLKO ODCZYT

		Parametr - bufor na numery seryjne rozdzielone znakiem ko�ca linii - 
					(ka�dy numer ma max. 8znak�w+2 bajty - koniec linii = 10 bajt�w * max. 127 urz�dze� na USB = 1270)
					w zwi�zku z tym zalecany jest rozmiar bufora wi�kszy lub r�wny 1271 znak�w.

	*/

	#define POSNET_DEV_PARAM_LISTUSBSERIALS		0x00020008

	/*!

	\brief D�ugo�� kolejki wysy�kowej, po przekroczeniu, kt�rej rozkazy traktowane s� jak wysy�ane w trybie natychmiastowym.

	TYLKO ZAPIS

	Parametr - wska�nik na liczb� typu unsigned long (32 bit) zawieraj�c� ��dan� d�ugo�� kolejki wysy�kowej.

	*/

	#define POSNET_DEV_PARAM_OUTQUEUELENGTH		0x00020009
	/*!
	\brief Interwa� pomi�dzy automatycznymi odpytaniami o status drukarki

	TYLKO ZAPIS

	Parametr - wska�nik na liczb� typu unsigned long (32 bit) zawieraj�c� ��dany czas pomi�dzy zapytaniami w sekundach.
	*/
	#define POSNET_DEV_PARAM_STATUSPOLLINGINTERVAL		0x0002000A


	/*!
	\brief	Pobranie uchwytu portu szeregowego

	TYLKO ODCZYT

	Parametr - Windows - wska�nik na HANDLE, Linux - wska�nik na liczb� typu int
	*/

	#define POSNET_DEV_PARAM_FILEHANDLE		0x0002000E	


	/*!
		@}
	*/


	/*!
		\defgroup	basic_api	Obs�uga podstawowa urz�dzenia
		@{
	*/

	/*!
		\brief	Utworzenie uchwytu do nowego urz�dzenia. 
		
		Tworzy odpowiedni obiekt i ustawia
		nieb�dne jego w�a�ciwo�ci, ale fizycznie go nie otwiera. Przed otwarciem urz�dzenia 
		nale�y ustawi� nieb�dne parametry, kt�re s� r�zne w zale�no�ci od typu urz�dzenia.
		\param	deviceType	Typ pod��czonego urz�dzenia \ref dev_types "Typy pod��czenia"
		\return Uchwyt globalny utworzonego urz�dzenia. Warto�c NULL zostanie zwr�cona wy��cznie
				w przypadku braku pami�ci lub podania b��dnego typu urz�dzenia.
	*/
	POSNET_API POSNET_HANDLE	__stdcall POS_CreateDeviceHandle(unsigned long deviceType);

	/*!
		\brief	Otw�rz urz�dzenie.
		
		\param hGlobalDevice	Uchwyt globalny urzadzenia do otwarcia.
		\return Zwraca uchwyt lokalny do urz�dzenia w kontek�cie bie��cego w�tku. NULL w przypadku b��du,
				kod b��du mo�na odczyta� funkcj� POS_GetError
	*/
	POSNET_API POSNET_HANDLE	__stdcall POS_OpenDevice(POSNET_HANDLE hGlobalDevice);		  
	/*!
		\brief	Zamknij urz�dzenie. 
		
		Zamyka urzadzenie wskazywane przez uchwyt. Usuwa zawarto�ci kolejek
		komunikacyjnych. Wszytskie dane nale�y odczyta� przed zamkni�ciem urz�dzenia.

		\param hLocalDevice	Uchwyt urzadzenia do zamkni�cia.
	*/
	POSNET_API POSNET_STATUS	__stdcall POS_CloseDevice(POSNET_HANDLE hLocalDevice);
	/*!
		\brief	Usu� istniej�cy uchwyt urz�dzenia i zwolnij zajmowan� pami��.

		Ostatecznie usuwa obiekt komunikacyjny i zwalnia pami��.

		\param hGlobalDevice	Uchwyt globalny urz�dzenia do usuni�cia. Wszystkie w�tki musz� najpierw zamkn�� urzadzenie,
								w przeciwnym wypadku funkcja zwr�ci b��d POSNET_STATUS_BUSY.
	*/
	POSNET_API POSNET_STATUS	__stdcall POS_DestroyDeviceHandle(POSNET_HANDLE hGlobalDevice); 
	/*!
		\brief	Funkcja do ustawiania parametr�w urz�dzenia

		\param hDevice Uchwyt urz�dzenia - zar�wno lokalny jak i globalny
		\param paramCode Kod parametru por. \ref dev_params kody parametr�w
		\param paramValue Wska�nik lub warto�� parametru. Opis przy parametrach urz�dzenia.

	*/
	POSNET_API POSNET_STATUS	__stdcall POS_SetDeviceParam(POSNET_HANDLE hDevice, unsigned long paramCode,void *paramValue);	
	/*!
		\brief	Funkcja do pobierania parametr�w urz�dzenia

		\param hDevice Uchwyt urz�dzenia - zar�wno lokalny jak i globalny
		\param paramCode Kod parametru por. \ref dev_params kody parametr�w
		\param paramValue Wska�nik lub warto�� parametru. Opis przy parametrach urz�dzenia.

	*/
	POSNET_API POSNET_STATUS	__stdcall POS_GetDeviceParam(POSNET_HANDLE hDevice, unsigned long paramCode,void *paramValue);	



	/*!
		@}
	*/

	/*! \defgroup error_codes	Kody b��d�w biblioteki
		\ingroup	error_handling

		\brief	Kody b��d�w biblioteki. Biblioteka mo�e zwraca� kody
				b��d�w zar�wno w�asne jak i kody bezpo�rednio uzyskane
				z drukarki.
		@{
	*/
	
	#define POSNET_STATUS_OK						0x00000000 //!< Brak b��du
	#define POSNET_STATUS_OUTOFMEMORY				0x00000001 //!< Brak pami�ci
	#define POSNET_STATUS_FRAMETOOSHORT				0x00000002 //!< Za kr�tka ramka
	#define POSNET_STATUS_FRAMINGERROR				0x00000003 //!< B��d ramki odebranej z drukarki
	#define POSNET_STATUS_COULDNOTOPEN				0x00000005 //!< Nie mo�na otworzyc wskazanego urz�dzenia
	#define POSNET_STATUS_CRCERROR					0x00000006 //!< B��d CRC w odebranej ramce
	#define POSNET_STATUS_IPCERROR					0x00000007 //!< B��d utworzenia obiektu IPC (Event)
	#define POSNET_STATUS_COMMERROR					0x00000008 //!< B��d komunikacji
	#define POSNET_STATUS_USBERROR					0x00000009 //!< B��d krytyczny USB - urz�dzenie nie b�dzie funkcjonowa� poprawnie
	#define POSNET_STATUS_FTLIBIMPORTFAIL			0x0000000A //!< Nieudany import sterownika FTDI
	#define POSNET_STATUS_COULDNOTSETUPPORT			0x0000000B //!< B��d ustawienia parametr�w otwieranego portu
	#define POSNET_STATUS_COULDNOTOPEN_ACCESSDENIED 0x0000000C //!< B��d otwarcia urz�dzenia - dost�p zabroniony
	#define POSNET_STATUS_COULDNOTOPEN_FILENOTFOUND 0x0000000D //!< B��d otwarcia urz�dzenia - brak takiego pliku (urz�dzenia)
	#define POSNET_STATUS_SETUP_INVALIDBAUD			0x0000000E //!< B��dne parametry portu - baudrate
	#define POSNET_STATUS_SETUP_INVALIDDATA			0x0000000F //!< B��dne parametry portu - databits
	#define POSNET_STATUS_SETUP_INVALIDPARITY		0x00000010 //!< B��dne parametry portu - parity
	#define POSNET_STATUS_SETUP_INVALIDSTOP			0x00000011 //!< B��dne parametry portu - stop bits
	#define POSNET_STATUS_SETUP_INVALIDHANDSHAKE	0x00000012 //!< B��dne parametry portu - handshake
	#define POSNET_STATUS_INVALIDSTATE				0x00000013 //!< Wydano polecenie REPEAT dla ramki znajduj�cej si� w niew�a�ciwym stanie (innym ni� SENT lub ACK)
	#define POSNET_STATUS_DEVICE_BUSY				0x00000014 //!< Urz�dzenie zaj�te

	#define	POSNET_STATUS_BUSY					0x00000020 //!< Urz�dzenie zaj�te

	#define POSNET_STATUS_ALREADY_COMPLETED		0x00010000 //!< Rozkaz ju� wykonany \sa POS_CancelRequest
	#define POSNET_STATUS_EMPTY					0x00010001 //!< Brak danych w kolejce
	#define	POSNET_STATUS_INVALIDVALUE			0x00010002 //!< B��dna warto��
	#define	POSNET_STATUS_TIMEOUT				0x00010003 //!< Oczekiwanie zako�czone up�yni�ciem czasu (timeout)
	#define	POSNET_STATUS_PENDING				0x00010004 //!< Polecenie w trakcie wykonywania
	#define POSNET_STATUS_INVALIDCOMMAND		0x00010005 //!< B��dny numer polecenia
	#define POSNET_STATUS_INVALIDHANDLE			0x00010006 //!< B��dny uchwyt
	#define POSNET_STATUS_BUFFERTOOSHORT		0x00010007 //!< Przekazany bufor znakowy jest za ma�y
	#define POSNET_STATUS_OUTOFRANGE			0x00010008 //!< Poza zakresem licznika
	#define POSNET_STATUS_INVALIDSPOOLMODE		0x00010009 //!< B��dny tryb kolejkowania
	#define POSNET_STATUS_CANCELLED				0x0001000A //!< Rozkaz anulowany

	#define POSNET_STATUS_INVALID_PARAM1		0x00010101 //!< B��dny 1 parametr polecenia
	#define POSNET_STATUS_INVALID_PARAM2		0x00010102 //!< B��dny 2 parametr polecenia
	#define POSNET_STATUS_INVALID_PARAM3		0x00010103 //!< B��dny 3 parametr polecenia
	#define POSNET_STATUS_INVALID_PARAM4		0x00010104 //!< B��dny 4 parametr polecenia
	#define POSNET_STATUS_INVALID_PARAM5		0x00010105 //!< B��dny 5 parametr polecenia
	#define POSNET_STATUS_INVALID_PARAM6		0x00010106 //!< B��dny 6 parametr polecenia

	#define	POSNET_STATUS_CASHREGBASE			0x00430000 //!< Bazowy kod b��du wykonania rozkazu drukarki [b��dy 0x00430000 - 0x0043FFFF]
	#define	POSNET_STATUS_CASHREGCOMMBASE		0x00440000 //!< Bazowy kod b��du transmisji rozkazu do drukarki [b��dy 0x00440000 - 0x0044FFFF]
	/*!
		@}
	*/


	/*!
		\defgroup	error_handling	Obs�uga b��d�w
		@{
	*/

	/*!
		\brief	Podaj kod statusu zwi�zany z uchwytem urz�dzenia
		
		Zwraca ostatnio ustawiony kod statusu zwi�zany z uchwytem urz�dzenia. 
		Ka�da wywo�ywana funkcja opr�cz zwr�cenia tej warto�ci jako wyniku dzia�ania
		(o ile nie zwraca innego rodzaju rezultatu)	ustawia wewn�trzny znacznik b��du. 
		\param hLocalDevice Uchwyt lokalny urz�dzenia lub uchwyt globalny urz�dzenia.
		\return Kod statusu patrz \ref error_codes Kody b��d�w.
	*/
	POSNET_API POSNET_STATUS	__stdcall POS_GetError(POSNET_HANDLE hLocalDevice);	
	/*!
		\brief	Zwr�� tekstowy opis b��du

		Funkcja zwraca tekstowy opis b��du zrozumia�y dla u�ytkownika / programisty.
		Komunikat mo�e by� zwr�cony we wskazanym j�zyku (o ile przygotowano
		niezb�dny plik j�zykowy). W przeciwnym wypadku komunikat zwracany jest
		w domy�lnym j�zyku biblioteki. 

		<em>UWAGA:</em> W systemie Linux pliki z tekstowymi opisami kod�w b��d�w (pliki j�zykowe
		o nazwach w postaci posnet_xx.lng mog� by� umieszczone w katalogu, z kt�rego
		uruchomiono program (nie zalecane) lub w miejscu wskazywanym przez zmienn�
		�rodowiskow� POSNET_LIB_SHARE_DIR (zalecane).
		W systemie WINDOWS katalog wskazany w POSNET_LIB_SHARE_DIR musi by� zako�czony 
		znakiem �\� rozdzielaj�cym katalogi np. POSNET_LIB_SHARE_DIR=c:\myapp\bin\

		\param code	Kod b��du do wyja�nienia
		\param lang	2 znakowy kod j�zyka wg ISO-3166
		\return Ci�g znak�w opisuj�cych zdarzenia (C-string) zakocz�ony znakiem 0. Ci�g ten zwracany
				jest z wewn�trznego bufora. Nie wolno go zwalnia� funkcjami allokacji pami�ci.
	*/
	POSNET_API const char *			__stdcall POS_GetErrorString(POSNET_STATUS code, char *lang);
	/*!
		\brief	Zwr�� status rozkazu

		UWAGA: W celu efektywnego zarz�dzania b��dami (w sytuacji, gdy drukarka zwraca b��dy w r�znych postaciach)
		zarz�dzanie b��dami zosta�o zunifikowane - nazwa rozkazu zostaje umieszczona w polu "cm" odpowiedzi, 
		kod b��du natomiast umieszczany jest tak�e w polu o nazwie "!error" odpowiedzi. 
		Dzi�ki temu posta� odpowiedzi jest identyczna w przypadku b��d�w ramki i b��d�w.

		\param hRequest Uchwyt rozkazu (��dania wychodz�cego lub odpowiedzi z drukarki)
		\return Funkcja zwraca kod b��du zwi�zany z rozkazem. W przypadku odebrania odpowiedzi o b��dzie 
				wykonania rozkazu zwracane s� kody odebrane od drukarki, s� one tak�e dost�pne w polach rezultatu
				komendy w postaci zgodnej z Instrukcj� Programisty.
				Kody o warto�ciach 0x00000000-0x0042FFFF s�u�� do przekazywania stanu rozkazu w ramach biblioteki.
				Kody o warto�ciach 0x00430000-0x0043FFFF wykorzystywane s� w przypadku negatywnej odpowiedzi drukarki na rozkaz, po jego zako�czeniu w wyniku b��du drukarki � kod b��du drukarki zwi�kszony jest o warto�� 0x00430000.
				Kody o warto�ciach 0x00440000-0x0044FFFF wykorzystywane s� w przypadku negatywnej odpowiedzi drukarki na rozkaz, po jego zako�czeniu w wyniku b��du transmisji � kod b��du transmisji zwi�kszony jest o warto�� 0x00440000.
		\sa sync_api "Obs�uga synchroniczna drukarki"
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_GetRequestStatus(POSNET_HANDLE hRequest);

	/*!
		\brief Pobranie statusu drukarki. Drukarka jest periodycznie odpytywana o status i rezultat tej operacji jest przechowywany.
		Funkcja jest blokuj�ca je�li wykonywane jest odpytywanie.

		\param hLocalDevice Lokalny uchwyt urz�dzenia
		\param statusMode Spos�b uzyskania statusu - 0 - zwr�� ostatni status automatyczny , 1 - odpytaj urz�dzenie je�li brak statusu - blokuj�ce!
		\param globalStatus Status urz�dzenia zwracany przez rozkaz sdev (-1 je�li nie odpytano)
		\param printerStatus Status mechanizmu drukuj�cego  (nieistotny je�li sdev=-1)
		\return Funkcja zwraca status wykonania rozkazu, oraz wype�nia pola parametr�w globalStatus i printerStatus
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_GetPrnDeviceStatus(POSNET_HANDLE hLocalDevice,const char statusMode,long *globalStatus,long *printerStatus);

	/*!
		@}
	*/



	// --------------------------------------------------------------------------------------------------------
	/*! \defgroup event_types Typy zdarze�
		\ingroup	event_handling
		@{
	*/
	/*!
		\brief	Wy��czenie obs�ugi zdarze� asynchronicznych - w tym trybie
		wszystkie zdarzenia s� automatycznie usuwane, a aplikacja nie jest
		o nich informowana.
	*/
	#define POSNET_EVENT_DISABLE	0x0000
#ifdef _WIN32
	/*!
		\brief	Sygnalizacja otrzymania zdarzenia za pomoc� komunikacji mi�dzyprocesowej
		typu "Event" (zdarzenie) w systemie Windows.
		Po wybraniu tego typu obs�ugi za pomoc� funkcji \ref POS_GetEvent mo�na
		uzyska� uchwyt zdarzenia zarezerwowanego przez urz�dzenie. 

		Tylko dla WIN32
	*/
	#define POSNET_EVENT_EVENT		0x0001
	/*!
		\brief	Sygnalizacja otrzymania zdarzenia za pomoc� komunikat�w systemu Windows.
		Po wybraniu tego trybu nale�y za pomoc� funkcji \ref POS_SetMessageParams
		ustawi� kod komunikatu i uchwyt okna do kt�rego ma on by� wys�any. Wysy�any
		komunikat ma jako lParam umieszczony uchwyt urz�dzenia, kt�re go wygenerowa�o.

		Tylko dla WIN32
	*/
	#define	POSNET_EVENT_MESSAGE	0x0002
#else
	/*!
		\brief Sygnalizacja otzrymania zdarzenia za pomoca semafora pthread

		Tylko dla Linux
	*/	
	#define POSNET_EVENT_SEMAPHORE	0x0005

#endif
	/*!
		\brief	Tryb obs�ugi poprzez wywo�anie funkcji w momencie otrzymania
		pakietu danych asynchronicznych.
	*/
	#define POSNET_EVENT_CALLBACK	0x0004
	/*! 
		@}
	*/

	/*!
		\defgroup	event_handling	Obs�uga zdarze� asynchronicznych statusu drukarek
		@{
	*/


	/*!
		\brief Ustaw metod� obs�ugi zdarze� asynchronicznych 

		Drukarka ma mo�liwo�� generacji zdarze� asynchronicznych w postaci zmian statusu drukarki, kt�re musz� by� przes�ane do aplikacji. 
		Tego typu zdarzenia mo�na przes�a� do aplikacji w nast�puj�cy spos�b:
		- za pomoc� IPC typu "Event"
		- za pomoc� komunikat�w systemu Windows (Windows Messages)
		- za pomoc� systemu pollingu, gdzie aplikacja odpytuje o status
		
		Funkcja s�u�y to ustawienia trybu sygnalizacji zdarze�.
		\param hLocalDevice	Uchwyt lokalny urz�dzenia. Funkcja pomimo pobierania uchwytu lokalnego ma zasi�g
				globalny dla urz�dzenia.
		\param eventMode Typ sygnalizacji zdarze� definiowany przez \ref event_types "Typy zdarze�"
		\return Status wykonania funkcji (POSNET_STATUS_OK w przypadku sukcesu)
	*/
	POSNET_API POSNET_STATUS	__stdcall POS_SetEventHandlingMode(POSNET_HANDLE hLocalDevice, unsigned long eventMode);
#ifdef _WIN32
	/*!
		\brief	Funkcja ustawia kod komunikatu i uchwyt okna do kt�rego ma by� on wys�any
				przy obs�udze zdarze� asynchronicznych. Tylko dla Windows. Jako lParam takiego komunikatu wysy�any jest
				z�o�ony status urz�dzenia (w/g wzoru: (Status urz�dzenia * 65536)+status mechanizmu )
		\param hLocalDevice	Uchwyt lokalny urz�dzenia. Funkcja pomimo pobierania uchwytu lokalnego ma zasi�g
				globalny dla urz�dzenia.
		\param	messageCode	Kod komunikatu (WM_xxxx)
		\param	hWnd	Uchwyt okna

		Tylko dla WIN32
	*/
	POSNET_API POSNET_STATUS	__stdcall POS_SetMessageParams(POSNET_HANDLE hLocalDevice, UINT messageCode, HANDLE hWnd);
	/*!
		\brief	Funkcja zwraca uchwyt do zdarzenia. Obiekt zdarzenia jest zarz�dzany przez
				bibliotek�. Zdarzenie pracuje w trybie Manual Reset 
				patrz \sa http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dllproc/base/createevent.asp MSDN.
		\param hLocalDevice	Uchwyt lokalny urz�dzenia. Funkcja pomimo pobierania uchwytu lokalnego ma zasi�g
				globalny dla urz�dzenia.
		\return	NULL w przypadku b��du, w przeciwnym przypadku uchwyt zdarzenia (Event).

		Tylko dla WIN32
	*/
	POSNET_API HANDLE			__stdcall POS_GetEvent(POSNET_HANDLE hLocalDevice);
#else
	/*!
		\brief Pobranie semafora synchronizuj�cego w�tki pthread (typ sem_t)
		\param hLocalDevice Uchwyt lokalny urz�dzenia. Funkcja pomimo pobierania uchwytu lokalnego ma zasi�g
				globalny dla urz�dzenia.
		\return NULL w przypadku b��du, w przeciwnym wypadku wska�nik na semafor.

		Tylko dla Linux
	*/
	POSNET_API void * __stdcall POS_GetSemaphore(POSNET_HANDLE hLocalDevice);

#endif
	/*!
		\brief Typ definiuj�cy funkcj� callback dla zdarze� asynchronicznych. Przyjmuje jeden parametr
		- z�o�ony status urz�dzenia (w/g wzoru: (Status urz�dzenia * 65536)+status mechanizmu ) . 
	*/
	typedef void (POSNET_CALLBACK_T)(unsigned long status);

	/*!
		\brief Funkcja ustawia w bibliotece funkcj� callback, kt�ra b�dzie wywo�ywana po otrzymaniu zdarzenia asynchronicznego.
		\param hLocalDevice Uchwyt lokalny urz�dzenia. Funkcja pomimo pobierania uchwytu lokalnego ma zasi�g
				globalny dla urz�dzenia.
		\param callback wska�nik na funkcj� typu callback.
	*/
	POSNET_API POSNET_STATUS	__stdcall POS_SetCallback(POSNET_HANDLE hLocalDevice, POSNET_CALLBACK_T *callback);

	/*!
		@}
	*/
	// --------------------------------------------------------------------------------------------------------


	/* MAIN COMMAND API */

	/*!
		\defgroup	req_modes	Tryby wysy�ania rozkaz�w
		\ingroup sync_api
		@{
	*/

	#define	POSNET_REQMODE_SPOOL	0x00 //!< Tryb kolejkowania [domy�lny], w tym trybie rozkaz umieszczany jest na 
										 //!< ko�cu kolejki rozkaz�w do wys�ania, za wyj�tkiem nast�puj�cych komend,
										 //!< kt�re ze swej natury umieszczane s� na samym pocz�tku kolejki:
										 //!< !sdev, !sprn
	#define POSNET_REQMODE_IMMEDIATE 0x01 //!< Tryb bezpo�redni. Podobny do \ref POSNET_REQMODE_SPOOL kolejkowania, lecz
										  //!< w przypadku, gdy w kolejce s� ju� jakie� rozkazy zwraca b��d - efektywnie
										  //!< umo�liwia umieszczenie rozkazu w kolejce WY��CZNIE gdy jest ona pusta,
										  //!< za wyj�tkiem rozkaz�w !sdev i !sprn
	#define	POSNET_REQMODE_SPOOLSPECIAL	0x02 //!< Tryb kolejkowania specjalnego, w kt�rym
											 //!< w kolejce odbiorczej odpowied� OK (brak b��du)
											 //!< na rozkaz nie zwracaj�cy wyniku zostanie automatycznie
											 //!< usuni�ta z kolejki odbiorczej. Ten tryb musi by� u�ywany 
											 //!< jedynie w trybie odbioru z kolejki odbiorczej (nie wolno
											 //!< stosowa� \ref POS_WaitForRequestCompleted POS_WaitForRequestCompleted, gdy�
											 //!< nie ma gwarancji poprawno�ci uchwytu urz�dzenia - nie dotyczy to rozkaz�w
											 //!< wykonanych znajduj�cych si� w kolejce odbiorczej).
	#define POSNET_REQMODE_AUTOCLEAR 0x03 //!< Ten tryb powoduje automatyczne czyszczenie odpowiedzi,
										  //!< bez wzgl�du na kod powrotu. W momencie wys�ania rozkazu w tym trybie nale�y uzna�,
										  //!< �e uchwyt rozkazu jest b��dny.

	/*!
		@}
	*/
	
	/*!
		\defgroup	req_states	Stany rozkazu
		\ingroup sync_api
		@{
	*/

	#define POSNET_RSTATE_NEW			0x00019000	//!< Nowe polecenie, nie umieszczone w kolejce
	#define POSNET_RSTATE_PENDING		0x00019001	//!< Nowe polecenie, umieszczone w kolejce
	#define POSNET_RSTATE_SENT			0x00019002	//!< Polecenie wys�ane, nie potwierdzone
	#define	POSNET_RSTATE_COMPLETED		0x00019004	//!< Wykonanie rozkazu zako�czone
	#define	POSNET_RSTATE_ERRCOMPLETED	0x00019005	//!< Wykonanie rozkazu zako�czone b��dem
	/*!
		@}
	*/


	// --------------------------------------------------------------------------------------------------------


	/*!
		\defgroup	request_base	Obs�uga obiekt�w rozkazowych
		\ingroup sync_api

		@{
	*/

	/*!
		\brief	Funkcja tworzy nowy obiekt rozkazowy dla drukarki.

		Funkcja tworzy obiekt rozkazowy dla drukarki. Obiekt tego typu jest podstawow�
		kom�rk� transmisyjn� pomi�dzy aplikacj� a drukark�. Jego zadaniem jest realizacja
		pojedynczej transakcji drukarka-aplikacja.

		Wi�cej informacji we wprowadzeniu - \ref sec_flow "Przep�yw informacji" 

		\param	hLocalDevice	Uchwyt lokalny urz�dzenia
		\param	command	Identyfikator rozkazu dla drukarki
		\return	Zwracany jest uchwyt do nowego obiektu rozkazowego lub NULL w przypadku b��du
	*/
	POSNET_API	POSNET_HANDLE	 __stdcall POS_CreateRequest(POSNET_HANDLE hLocalDevice, const char *command);
	/*!
		\brief	Funkcja tworzy nowy obiekt rozkazowy dla drukarki oraz dodaje parametry rozkazu.

		Funkcja tworzy obiekt rozkazowy dla drukarki. Obiekt tego typu jest podstawow�
		kom�rk� transmisyjn� pomi�dzy aplikacj� a drukark�. Jego zadaniem jest realizacja
		pojedynczej transakcji drukarka-aplikacja.

		Wi�cej informacji we wprowadzeniu - \ref sec_flow "Przep�yw informacji" 

		\param	hLocalDevice	Uchwyt lokalny urz�dzenia
		\param	command	Identyfikator rozkazu dla drukarki
		\param  parameters Lista parametr�w do dodania (zast�puje dodatkowe wywo�ania \ref POS_PushRequestParam) 
		format: [nazwa parametru],[wartosc]\n[nazwa parametru],[wartosc]\n....[nazwa parametru],[wartosc]
		UWAGA: Je�li warto�� parametru zawiera w sobie znaki nowej linii lub przecinka nale�y u�y� \ref POS_PushRequestParam aby
		doda� taki parametr do rozkazu.
		\return	Zwracany jest uchwyt do nowego obiektu rozkazowego lub NULL w przypadku b��du
	*/
	POSNET_API      POSNET_HANDLE  __stdcall POS_CreateRequestEx(POSNET_HANDLE hLocalDevice, const char* command,const char *parameters);
	/*!
		\brief Funkcja s�u�y do pobrania uchwytu urz�dzenia zwi�zanego z rozkazem

		\param hRequest	Uchwyt rozkazu
		\return uchwyt lokalny urz�dzenia lub NULL w przypadku b��du gdy� nie mo�na ustali� adresata rozkazu.
	*/
	POSNET_API POSNET_HANDLE	__stdcall POS_RequestDevice(POSNET_HANDLE hRequest);

	/*!
		\brief	Fukcja umieszcza obiekt rozkazowy w kolejce rozkaz�w do wykonania.

		Istniej� dwa warianty wysy�ania rozkaz�w 
		  - tryb kolejkowania,
		  - tryb bezpo�redni.
		
		W trybie kolejkowania nowy rozkaz jest umieszczany w kolejce do wys�ania do drukarki i
		automatycznie wysy�any w miar� zwalniania si� bufora drukarki. 
		\warning D�ugo�c kolejki jest ograniczona. Po jej przekroczeniu rozkazy traktowane s� 
				 jakby by�y wysy�ane w trybie bezpo�rednim.
	   
	    Tryb bezpo�redni kontroluje, czy drukarka jest wolna i dopiero gdy jest wolna umo�liwia
		umieszczenie rozkazu w buforze. W ten spos�b w danej chwili do drukarki wys�any jest 
		co najwy�ej 1 rozkaz.

		\param	hRequest	Uchwyt rozkazu
		\param	mode		Wyb�r trybu wysy�ania rozkazu \ref req_modes "Tryby wysy�ania rozkaz�w"
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_PostRequest(POSNET_HANDLE hRequest,unsigned char mode);
	/*!
		\brief	Anulowanie obiektu rozkazowego

		Funkcja anuluje wykonanie rozkazu. W zale�no�ci od stanu rozkazu mo�e to oznacza�:
		  - je�li rozkaz nie zosta� jeszcze wys�any zostaje on usuni�ty z kolejki rozkaz�w,
		  - je�li rozkaz zosta� wys�any do drukarki zostaje on usuni�ty z kolejki rozkaz�w,
		  - je�li rozkaz by� ju� wykonany to nic nie jest wykonywane i zostaje zwr�cony
			kod b��du \ref POSNET_STATUS_ALREADY_COMPLETED .

		W ka�dym przypadku za zniszczenie obiektu odpowiada aplikacja -  dla anulowanego rozkazu nale�y
		wywo�a� \ref POS_DestroyRequest .

		\param hRequest	Uchwyt rozkazu
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_CancelRequest(POSNET_HANDLE hRequest);
	/*!
		\brief	Zniszczenie obiektu rozkazowego i zwolnienie zajmowanej przeze� pami�ci.

		Funkcja kasuje obiekt rozkazowy oraz wszystkie powi�zane z nim dane.
		Musi by� u�yta dla ka�dego rozkazu allokowanego poprzez \ref POS_CreateRequest POS_CreateRequest 
		kt�ry nie jest automatycznie usuwany w ytybach SPOOLSPECIAL i AUTOCLEAR.

		\param hRequest Uchwyt rozkazu (��dania wychodz�cego lub odpowiedzi z drukarki)
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_DestroyRequest(POSNET_HANDLE hRequest);

	/*!
		\brief	Wiek rozkazu - czas jaki up�yn�� od wywo�ania \ref POS_PostRequest

		Funkcja ta ma na celu ustalenie wieku rozkazu i mo�e by� przydatna w obs�udze
		sytuacji nadzwyczajnych. Czas podawany jest w milisekundach.

		\param hRequest Uchwyt rozkazu (��dania wychodz�cego lub odpowiedzi z drukarki)
	*/
	POSNET_API	unsigned long	__stdcall POS_GetRequestAge(POSNET_HANDLE hRequest);
	/*!
		\brief	Pobierz numer polecenia, kt�ry zosta� u�yty przy tworzeniu obiektu

		\param hRequest Uchwyt rozkazu (��dania wychodz�cego lub odpowiedzi z drukarki)
		\param retCommand   Bufor znakowy, w kt�rym zostanie umieszczona nazwa polecenia
		\return Status wykonania rozkazu
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_GetRequestCommandID(POSNET_HANDLE hRequest,char *retCommand);
	/*!
		\brief	Pobierz numer seryjny rozkazu

		\param hRequest Uchwyt rozkazu (��dania wychodz�cego lub odpowiedzi z drukarki)
		\return Token (numer seryjny) powi�zany z rozkazem
	*/
	POSNET_API	unsigned long	__stdcall POS_GetRequestCommandSerial(POSNET_HANDLE hRequest);
	/*!
		\brief	Zwr�� stan rozkazu

		Funkcja zwraca stan w jakim aktualnie znajduje si� rozkaz.
		Wi�cej informacji we wprowadzeniu - \ref sec_flow "Przep�yw informacji" 

		\param hRequest Uchwyt rozkazu (��dania wychodz�cego lub odpowiedzi z drukarki)
	*/
	POSNET_API	POSNET_STATE	__stdcall POS_GetRequestState(POSNET_HANDLE hRequest);
	/*!
		\brief	Czekaj na zako�czenie rozkazu

		Funkcja powoduje zatrzymanie (u�pienie) wo�aj�cej j� aplikacji do momentu
		zmiany stanu rozkazu na \ref POSNET_RSTATE_COMPLETED lub POSNET_RSTATE_ERRCOMPLETED lub up�yni�cia czasu
		wskazanego przez parametr timeout.
 
		\param hRequest	Uchwyt rozkazu
		\param	timeout Czas w ms na jaki ma aplikacja by� u�piona. 0 oznacza natychmiastowy 
				powr�t
		\return	zwraca POSNET_STATUS_OK je�li rozkaz zosta� juz uko�czony lub POSNET_STATUS_TIMEOUT 
				je�li rozkaz nie zd��y� si� zako�czy� we wskazanym czasie.
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_WaitForRequestCompleted(POSNET_HANDLE hRequest,unsigned long timeout);

	/*!
		@}
	*/

	// --------------------------------------------------------------------------------------------------------

	/*!
		\defgroup req_param	Obs�uga parametr�w rozkazu
		\ingroup sync_api

		\brief  W zale�no�ci od typu rozkazu mo�e on posiada� odpowiedni�
				ilo�� parametr�w. Parametry te  odk�adane s� na list� parametr�w
				za pomoc� kolejnych wywo�a� funkcji POS_PushRequestParam. Drukarka 
				COMBO DF przyjmuje parametry bez wzgl�du na ich kolejno��.
		@{
	*/

	/*!
		\brief	Wstaw parametr rozkazu
		\param	hRequest	Uchwyt rozkazu
		\param  param_name		Nazwa parametru reprezentowana jako ci�g znak�w w konwencji j�zyka C (zako�czony znakiem 0)
		\param	param_value		Warto�� parametru reprezentowana jako ci�g znak�w w konwencji j�zyka C (zako�czony znakiem 0)
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_PushRequestParam(POSNET_HANDLE hRequest,const char *param_name,const char *param_value);

	/*!
		@}
	*/
	// --------------------------------------------------------------------------------------------------------

	/*!
		\defgroup	response_handling	Analiza p�l odpowiedzi
		\ingroup sync_api

		@{
	*/
	/*!
		\brief	Pobierz ilo�� otrzymanych p�l warto�ci

		Funkcja zwraca ilo�� p�l DATA otrzymanych w odpowiedzi na rozkaz. Poprawna tylko
		wtedy, gdy sam rozkaz jest w stanie \ref POSNET_RSTATE_COMPLETED

		\param hRequest	Uchwyt zapytania/odpowiedzi
		\return Ilo�� otrzymanych p�l odpowiedzi lub -1 w przypadku b��du
	*/
	POSNET_API	long			__stdcall POS_GetResponseValueCount(POSNET_HANDLE hRequest);

	/*!
		\brief	Pobierz warto�� parametru i przesu� na nast�pne pole

		\param	hRequest	Uchwyt zapytania/odpowiedzi
		\param retName Wska�nik do bufora znakowego, w kt�rym funkcja umie�ci nazw� odebranego parametru. D�ugo�� tego bufora MUSI by� conajmniej
						POSNET_NAMELENGTH_MAX
		\param  retVal Wska�nik do bufora znakowego, w kt�rym funkcja umie�ci odebrany ci�g znak�w. Aplikacja musi zapewni� odpowiedni�
						pojemno�� bufora, kt�r� przekazuje w parametrze retValLen. W przypadku zbyt kr�tkiego
						bufora zostanie zwr�cony b��d \ref POSNET_STATUS_BUFFERTOOSHORT
		\param	retValLen	D�ugo�� bufora
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_PopResponseValue(POSNET_HANDLE hRequest,char *retName, char * retVal, unsigned long retValLen);
	/*!
		\brief	Pobierz wato�� parametru po nazwie

		\param	hRequest	Uchwyt zapytania/odpowiedzi
		\param paramName Wska�nik do bufora znakowego, w kt�rym znajduje si� nazwa poszukiwanego pola odpowiedzi
		\param  retVal Wska�nik do bufora znakowego, w kt�rym funkcja umie�ci odebrany ci�g znak�w. Aplikacja musi zapewni� odpowiedni�
						pojemno�� bufora, kt�r� przekazuje w parametrze retValLen. W przypadku zbyt kr�tkiego
						bufora zostanie zwr�cony b��d \ref POSNET_STATUS_BUFFERTOOSHORT
		\param	retValLen	D�ugo�� bufora
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_GetResponseValue(POSNET_HANDLE hRequest,const char *paramName, char * retVal, unsigned long retValLen);
	/*!
		\brief	Przesu� wska�nik p�l na pocz�tek listy

		Funkcja umo�liwia rozpocz�cie analizy p�l od pocz�tku.

		\param	hRequest	Uchwyt zapytania/odpowiedzi
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_ResponseRewind(POSNET_HANDLE hRequest);
	
	/*!
		@}
	*/

	/*!
		\defgroup req_queue Obs�uga kolejek rozkaz�w i wynik�w
		\ingroup sync_api

		Aplikacja mo�e obs�ugiwa� wysy�ane rozkazy w dw�ch trybach.
		  * W trybie "wy�lij-zapomnij" aplikacja tworzy rozkaz, wype�nia parametrami a nast�pnie
			umieszcza w kolejce rozkaz�w do wykonania. Jednocze�nie nie przechowuje samodzielnie 
			uchwyt�w wys�anych rozkaz�w. Wyniki rozkaz�w pobiera z kolejki wynik�w za po�rednictwem
			\ref POS_GetNextResponse i obs�uguje odpowiednio.
		  * W trybie "interaktywnym" aplikacja tworzy rozkaz, wype�nia parametrami i wstawia
			do kolejki rozkaz�w. Jednak�e zapami�tuje uchwyt rozkazu i obserwuj�c stan rozkazu
			za po�rednictwem \ref POS_GetRequestState obs�uguje go w momencie zako�czenia.
		
		Tryb "wy�lij-zapomnij" jest bardziej odpowiedni dla operacji masowych (np. sprawdzanie bazy towar�w)
		gdy� umo�liwia efektywniejsze (szybsze) wykonywanie rozkaz�w, kt�rych wynik jest
		mniej istotny.

		Tryb "interaktywny" wskazany jest dla rozkaz�w, kt�rych wynik jest w danej chwili niezb�dny dla
		dalszej pracy aplikacji. W tym celu wprowadzony jest te� rozkaz \ref POS_WaitForRequestCompleted
		kt�ry umozliwia synchronizowane oczekiwanie na zako�czenie wykonania rozkazu przez drukark�.

		\warning	Nie ma mo�liwo�ci przegl�dania kolejki rozkaz�w oczekuj�cych/wys�anych. Jest to spowodowane 
		ich asynchronicznym w stosunku do apliacki wykonywaniem i zawarto�� tej kolejki mo�e si�
		zmienia� w trakcie wykonywania zapyta� przegl�daj�cych (rozkazy mog� znika� z kolejki mimo
		i� aplikacja pobra�a je jeszcze z kolejki oczekuj�cych).

		@{
	*/

	/*!
		\brief	Pobierz z kolejki odpowiedzi (rozkaz�w zako�czonych) nast�pny dost�pny wynik.

		Z kolejki odpowiedzi zostaje pobrany (i usuni�ty) nast�pny wynik. Pobrany wynik nale�y 
		zniszczy� za pomoc� \ref POS_DestroyRequest. Pobierane s� wy��cznie wyniki otrzymane na
		rozkazy wys�ane za po�rednictwem bie��cego kontekstu urz�dzenia (hLocalDevice). Rozkazy 
		wys�ane za pomoc� innych kontekst�w tego samego urz�dzenia s� pomijane.
		UWAGA: Uchwyt wyniku jest to�samy z uchwytem rozkazu, kt�ry go spowodowa� - oznacza to,
		�e niszczenie za pomoc� \ref POS_DestroyRequest rozkazu jest r�wnoznaczne ze zniszczeniem
		odpowiedzi i odwrotnie.

		Funkcja jest nieblokuj�ca.

		\param	hLocalDevice	Uchwyt lokalny urz�dzenia
		\return	Uchwyt do odpowiedzi lub NULL w przypadku b��du lub braku nast�pnej odpowiedzi
	*/
	POSNET_API	POSNET_HANDLE	__stdcall POS_GetNextResponse(POSNET_HANDLE hLocalDevice);
	/*!
		\brief	Pobierz ilo�� wynik�w w kolejce wynik�w

		\param hLocalDevice	Uchwyt lokalny urz�dzenia
		\return Ilo�� wynik�w dla bie��cego kontekstu urz�dzenia.
	*/
	POSNET_API	unsigned long	__stdcall POS_GetResponseCount(POSNET_HANDLE hLocalDevice);
	/*!
		\brief	Pobierz ilo�� rozkaz�w oczekuj�cych w kolejce rozkaz�w wys�anych przez bie��cy kontekst
				urz�dzenia lub wszystkich rozkaz�w oczekuj�cych w kolejce.

		\param hLocalDevice	Uchwyt lokalny urz�dzenia
		\param globalnie Je�li parametr ten jest r�ny od 0 zwracana warto�� podaje ilo�� wszystkich rozkaz�w 
				oczekuj�cych w kolejce.
	*/
	POSNET_API	unsigned long	__stdcall POS_GetRequestCount(POSNET_HANDLE hLocalDevice,unsigned char globalnie=0);

	/*!
		\brief	Pobierz statystyki kolejek. Statystyki s� zwracane w kontek�cie ca�ego urz�dzenia.

		\param hLocalDevice	Uchwyt lokalny urz�dzenia
		\param sent		Do tej zmiennej wpisywana jest ilo�� wszystkich wys�anych do drukarki
						rozkaz�w
		\param completed	Do tej zmiennej wpisywana jest ilo�� wszystkich uko�czonych rozkaz�w
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_RequestQueueStats(POSNET_HANDLE hLocalDevice, unsigned long *sent, unsigned long *completed);	


	/*!
		\brief	Wyczy�� kolejk� rozkaz�w oczekuj�cych na wys�anie (nie ignoruj odpowiedzi na rozkazy ju� wys�ane)

		Czy�ci kolejk� rozkaz�w oczekuj�cych na wys�anie. Funkcja powinna by� wykorzystywana wy��cznie
		w sytuacji, gdy nast�pi� krytyczny b��d urz�dzenia. Rozkazy ju� wys�ane zostan� obs�u�one, je�li
		w dalszej perspektywie pojawi si� na nie odpowied�. Domy�lnie rozkazy czyszczone s� wy��cznie dla bie��cego
		kontekstu urz�dzenia.

		\param	hLocalDevice	Uchwyt lokalny urz�dzenia
		\param globalnie Je�li parametr ten jest r�ny od 0 ignorowany jest kontekst urz�dzenia.
	*/
	POSNET_API	POSNET_STATUS	__stdcall POS_RequestQueueCancelPending(POSNET_HANDLE hLocalDevice,unsigned char globalnie=0);
	/*!
		\brief	Wyczy�� kolejk� rozkaz�w oczekuj�cych na wykonanie (zar�wno wys�anych jak i oczekuj�cych na wys�anie)

		Czy�ci kolejk� rozkaz�w wychodz�cych. Funkcja powinna by� wykorzystywana wy��cznie
		w sytuacji, gdy nast�pi� krytyczny b��d urz�dzenia. Wszystkie otrzymane p�niej
		wyniki dla rozkaz�w, kt�re by�y w trakcie wykonywania zostan� odrzucone. 
		Domy�lnie rozkazy czyszczone s� wy��cznie dla bie��cego
		kontekstu urz�dzenia.

		\param	hLocalDevice	Uchwyt lokalny urz�dzenia
		\param globalnie Je�li parametr ten jest r�ny od 0 ignorowany jest kontekst urz�dzenia.
	*/
	POSNET_API  POSNET_STATUS	__stdcall POS_RequestQueueCancelAll(POSNET_HANDLE hLocalDevice,unsigned char globalnie=0);

	/*!
		\brief	Wysy�a do drukarki ��danie powt�rzenia transmisji odpowiedzi

		Funkcja jest przydatna w sytuacji, gdy chcemy powt�rzy� odbi�r ramki je�li nast�pi� timeout i istnieje
		podejrzenie, �e drukarka otrzyma�a rozkaz ale odpowied� do nas nie powr�ci�a. Do powt�rzenia u�ywana
		jest de facto sekwencja 'rpt'. Uniemo�liwia to przypadkowe podw�jne wykonanie sekwencji je�li wcze�niej nie
		dotar�a do drukarki (zostanie zwr�cony b��d 13).
		U�ycie mo�liwe jest je�li rozkaz znajduje si� w stanie POSNET_RSTATE_SENT. 
		(por. \ref POS_GetRequestState)

		\param hRequest	Uchwyt rozkazu, kt�ry ma by� ponowiony
		\return Status wykonania
	*/
	POSNET_API      POSNET_STATUS  __stdcall POS_RepeatRequest(POSNET_HANDLE hRequest);

	/*!
		@}
	*/

	// --------------------------------------------------------------------------------------------------------
	/*!
	\defgroup	sync_api	Obs�uga synchroniczna drukarki
	@{
	*/
	
	// PLACEHOLDER

	/*!
		@}
	*/
	// --------------------------------------------------------------------------------------------------------

}


#endif

