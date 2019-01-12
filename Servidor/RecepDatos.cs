using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Net.Mail;
using System.Timers;
using MySql.Data.MySqlClient;
using System.Text;
using System.ServiceProcess;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO.Ports;
using System.Threading;

namespace Servidor
{
    #region Puerto 1

    public partial class RecepDatos : Form
    {
        #region inicio
        //Genera la conexión
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> _clienSockets = new List<Socket>();
        private static byte[] _buffer = new byte[10000];
        delegate void SetTextCallback(string TextoFin, string fecha);

        //ReceiveCallback
        int received;
        byte[] dataBuf;
        Socket socket;

        //Archivos de configuración
        public static string Direccion = Directory.GetCurrentDirectory();
        public static string Carpeta = @Direccion;
        public static string SubCarpeta = System.IO.Path.Combine(Carpeta, "Configuracion");
        public static string Archivo = System.IO.Path.Combine(SubCarpeta, "Configuracion.txt");

        //Lista de el archivo de configuración
        public static List<string> Config = new List<string>();

        //Fecha y hora
        DateTime FechaYa;

        //Archivos separados
        public static string DireccionDoc = Directory.GetCurrentDirectory();
        public static string CarpetaDoc = DireccionDoc;
        public static string PuertoDoc = System.IO.Path.Combine(CarpetaDoc, "Puerto 1");
        public static string SubCarpetaDoc = System.IO.Path.Combine(PuertoDoc, "Registro individual Puerto 1");
        public static string ArchivoDoc;
        public bool CreaArchivos = true;
        public bool CarEspecial = false;

        //Copia de seguridad
        public static string DireccionCopi = Directory.GetCurrentDirectory();
        public static string CarpetaCopi = DireccionCopi;
        public static string SubCarpetaCopi = System.IO.Path.Combine(PuertoDoc, "Registro mensual Puerto 1");
        public static string ArchivoCopi;

        //Envio Correo
        string horas;
        public string Subject;
        public string Body;

        //Listas de errores
        public static List<string> ErroresPuerto1 = new List<string>();
        public static List<string> ErroresEntradaDatosPuerto1 = new List<string>();
        public static List<string> ErroresSaliDatosPuerto1 = new List<string>();
        public static List<string> ErroresPuerto2 = new List<string>();
        public static List<string> ErroresEntradaDatosPuerto2 = new List<string>();
        public static List<string> ErroresSaliDatosPuerto2 = new List<string>();
        public static List<string> ErroresPuerto3 = new List<string>();
        public static List<string> ErroresEntradaDatosPuerto3 = new List<string>();
        public static List<string> ErroresSaliDatosPuerto3 = new List<string>();

        //Timer
        public int counter;
        public bool counter2 = true;
        System.Timers.Timer aTimer = new System.Timers.Timer();
        public string[] Minutos;

        //log errores
        public static string DireccionLog = Directory.GetCurrentDirectory();
        public static string CarpetaLog = @DireccionLog;
        public static string PuertoLog = System.IO.Path.Combine(CarpetaLog, "Log de errores");
        public static string SubCarpetaLog = System.IO.Path.Combine(PuertoLog, "Puerto 1");
        public static string ArchivoLog = System.IO.Path.Combine(SubCarpetaLog, "Log_Errores.txt");

        //Archivos salvados
        public static string DireccionSalv = Directory.GetCurrentDirectory();
        public static string CarpetaSalv = @DireccionSalv;
        public static string PuertoSalv = System.IO.Path.Combine(CarpetaSalv, "Archivos Salvados");
        public static string SubCarpetaSalv = System.IO.Path.Combine(PuertoSalv, "Archivos Salvados puerto 1");
        public static string ArchivoSalv = System.IO.Path.Combine(SubCarpetaSalv, "Archivos_Salvados.txt");

        //Puertos
        Puerto2 p2 = new Puerto2();
        Puerto3 p3 = new Puerto3();

        //Configuracion SQL
        public static string ArchivoSQL = System.IO.Path.Combine(SubCarpeta, "ConfiguracionSQL.txt");
        public static List<string> ProP1 = new List<string>();
        public static List<string> ProP2 = new List<string>();
        public static List<string> ProP3 = new List<string>();
        public List<string> ConfigE = new List<string>();
        public List<string> ConfigS = new List<string>();
        public List<string> ConfigI = new List<string>();
        public List<string> TiposLlamada = new List<string>();
        public List<string> TiposLlamada2 = new List<string>();
        public bool Igual = false;
        public string TipoLlamPos = null;
        public string TipoLlamPosE = null;
        public string TipoLlamPosS = null;
        public string TipoLlamPosI = null;
        public string[] TipoLlampos = null;
        public string[] TipoLlamposE = null;
        public string[] TipoLlamposS = null;
        public string[] TipoLlamposI = null;
        public string TipoLlamadaCad = null;
        public string Tipo = null;
        public int i = 0;
        public int Veces = 0;
        public bool ConfigCorrecta = true;

        //conexion SQL
        public static MySqlConnection Conexion;
        public string query;
        public MySqlCommand comando;
        public int IntentosCon = 0;
        MySqlDataReader lee;

        //Datos a SQL
        public string F_Fecha_Final_Lamada = "-";
        public string H_Hora_Final_Llamada = "-";
        public string E_Extension = "-";
        public string T_Troncal = "-";
        public string N_Numero_Marcado = "-";
        public string D_Duracion = "-";
        public string P_Codigo_Personal = "-";
        public string m_Fecha_Inicial_Llamada = "-";
        public string j_Hora_Inicial_Llamada = "-";
        public string l_Tipo_Llamada = "-";
        public string R_Trafico_Interno_Externo = "-";
        string ClaseLlamada = "-";
        string Destino = "-";
        string CentroDeCosto = "-";
        string DuracionLlamada = "-";
        string DuracionAprox = "-";
        string PlanTarifa = "-";
        string NumeroTarifa = "-";
        string DurRang1 = "-";
        string DurRang2 = "-";
        string DurRang3 = "-";
        string DurRang4 = "-";
        string DurRang5 = "-";
        string ValorRango1 = "-";
        string ValorRango2 = "-";
        string ValorRango3 = "-";
        string ValorRango4 = "-";
        string ValorRango5 = "-";
        string CargoFijo = "-";
        string RecargoServicioPorcentaje = "-";
        string RecargoServicioValor = "-";
        string Iva = "-";
        string BaseIVA = "-";
        string ValorIVA = "-";
        string ValorTotal = "-";
        string Folio = "-";
        string ClaseExtension = "-";
        string NombreExtension = "-";
        string CadenaCompleta = "-";
        string DurMinima = "-";
        string Operador = "-";
        string DigitosMinimos = "-";
        string NombreTarifa = "-";
        string Tarifa1 = "-";
        string Tarifa2 = "-";
        string Tarifa3 = "-";
        string Tarifa4 = "-";
        string Tarifa5 = "-";
        string ValorLlamadaTarifa = "-";
        string PorcentajeIVA = "-";
        string Errores = "-";
        string NombreTabla = "";
        string FormatoFechaSQL = "";

        //Caracteres especiales
        public static string ArchivoCarEsp = System.IO.Path.Combine(SubCarpeta, "Caracteres_especiales.txt");
        public bool CarEspe = false;
        public static List<string> P1 = new List<string>();
        public static List<string> P2 = new List<string>();
        public static List<string> P3 = new List<string>();
        public static bool pu1 = false;
        public static bool pu2 = false;
        public static bool pu3 = false;
        public bool YaEsp = false;

        //Progress bar
        delegate void vbar(int v);
        delegate void sbar(string s);

        //CounterSQL
        System.Timers.Timer bTimer = new System.Timers.Timer();
        List<string> Cadenas;
        public bool ApList = true;

        //SetText
        string TextoFin;
        string textofin;
        string[] caracteres;
        string[] hexValuesSplit;
        int value;
        string stringValue;
        string[] hexValuesSplit2;
        int Campos;
        string[] Partes;
        int SetVeces;
        string Fecha;
        
        //Auxiliares de tarificación
        string CTroncal;
        string AccesoTroncal;
        string NumMar;
        string Extn;
        bool Excluido = false;
        string Indic;
        int SoloUno;
        string Duracion;
        int DurMin;
        int DuracionLLam;
        int DuracionLlamAprox;
        string DuracionProc1;
        string DuracionProc2;
        int RangTar1;
        int RangTar2;
        int RangTar3;
        int RangTar4;
        int RangTar5;
        int Intervalo;
        int IncrementoInt;
        bool IntervaloSal;
        int VR1;
        int VR2;
        int VR3;
        int VR4;
        int VR5;
        int DR1;
        int DR2;
        int DR3;
        int DR4;
        int DR5;
        bool Redondeo_IVA = true;
        bool Redondeo_Recargo = false;
        bool saleExcluido = false;
        bool excluido = false;
        string error;
        bool SubeExpo = true;

        //Configuración serial
        string[] SerialConfig;
        string SerialPath = System.IO.Path.Combine(SubCarpeta, "SerialConf.txt");

        //Configuración Interfaces
        public static string ArchivoIntP1 = System.IO.Path.Combine(SubCarpeta, "IntConfP1.txt");
        List<string> InterfaceConfig;
        public static int PermiteInterfaces = 0;
        //Hotel Plus
        string In1CadenaFinal;
        int In1Inicia;
        int In1Aumenta;
        char In1Caracter;
        string In1a;
        string In1m;
        string In1d;
        string In1h;
        string In1n;
        string Init1Mes = "";
        string Init1Dia = "";
        string Init1Año = "";
        int Ini1Pos = 0;
        string Init1Horas = "";
        string Init1Minutos = "";
        bool In1Filtro;
        string directorioIn1 = "";
        int In1Calculal;

        public RecepDatos()
        {
            InitializeComponent();
            this.Load += RecepDatos_Load;
            this.FormClosing += Servidor_FormClosing;
        }

        private void RecepDatos_Load(object sender, EventArgs e)
        {
            string currPrsName = Process.GetCurrentProcess().ProcessName;
            Process[] allProcessWithThisName = Process.GetProcessesByName(currPrsName);
            if (allProcessWithThisName.Length > 1)
            {
                MessageBox.Show("El programa ya está abierto!");
                Contraseña.Sal = 3;
                Application.Exit();
            }
            else
            {
                Mensaje ms = new Mensaje();
                ms.Show();
                Application.DoEvents();
                InitializeTimer();
                CreaConfig();
                CargaConfigInterface();
                PuertosList();
                ms.Close();
            }
        }

        private void Servidor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Contraseña.Sal == 1)
            {
                serialPort1.Close();
                FechaYa = DateTime.Now;
                string nuevoSal = FechaYa.ToString();
                e.Cancel = false;
                Subject = "Estado del servidor";
                Body = "Se ha cerrado el servidor! " + nuevoSal;
                LogErrores("Servidor cerrado el: " + nuevoSal);
                EnvioCorreo(Subject, Body);
            }
            else if(Contraseña.Sal == 0)
            {
                e.Cancel = true;
                Contraseña contra = new Contraseña(2);
                contra.Show();
            }
        }

        public void PuertosList()
        {
            try
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                int NumPuertos = Convert.ToInt32(Config[12]);
                if (NumPuertos == 0)
                {
                    Puerto1.Size = new Size(1080, 364);
                    pictureBox5.BackColor = Color.Transparent;
                    pictureBox6.BackColor = Color.Transparent;
                    pictureBox7.BackColor = Color.Transparent;
                    pictureBox8.BackColor = Color.Transparent;
                    pictureBox9.BackColor = Color.Transparent;
                    pictureBox10.BackColor = Color.Transparent;
                }
                else if (NumPuertos == 1)
                {
                    Puerto1.Size = new Size(530, 364);
                    this.Puerto2.Location = new Point(560, 22);
                    this.Puerto2.Size = new Size(530, 364);
                    p2.Inicio(this);
                    Puerto3.Visible = false;
                    pictureBox6.BackColor = Color.Transparent;
                    pictureBox9.BackColor = Color.Transparent;
                    pictureBox10.BackColor = Color.Transparent;
                }
                else if (NumPuertos == 2)
                {
                    p2.Inicio(this);
                    p3.Inicio(this);
                }
            }
            catch
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                MessageBox.Show("Error al iniciar los puertos");
                LogErrores("Error al iniciar los puertos " + horas);
                Contraseña.Sal = 1;
                Application.Exit();
            }
        }

        #endregion

        //servidor

        #region Config
        public void CreaConfig()
        {
            try
            {
                if (!File.Exists(Archivo))
                {
                    GeneraIP Datos = new GeneraIP();
                    Datos.ShowDialog();
                    SQL();
                }
                else
                {
                    SQL();
                }

            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                MessageBox.Show("Ocurrió un error al crear el archivo de configuración " + horas + "\n\n" + e.ToString());
                pictureBox2.BackColor = Color.Red;
                pictureBox5.BackColor = Color.Red;
                pictureBox6.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + "Ocurrió un error al crear el archivo de configuración, más información:  \nl\nl" + e.ToString();
                EnvioCorreo(Subject, Body);
                Contraseña.Sal = 1;
                Application.Exit();
            }

        }

        public void SQL()
        {
            LeeConfig();

            if (!File.Exists(ArchivoSQL))
            {
                ConfiguraSQL sql = new ConfiguraSQL();
                sql.ShowDialog();
                if (!File.Exists(ArchivoSQL))
                {
                    Application.Exit();
                }
                else
                {
                    SQL();
                }
            }
            else
            {
                try
                {
                    using (Conexion = new MySqlConnection(Config[25]))
                    {
                        Conexion.Open();
                        Conexion.Close();
                    }
                    CargaConfigSQL();
                    if(ConfigCorrecta == true){
                        CarEsp();
                        SetupServer();
                        SetupSerial();
                    }
                    else
                    {
                        this.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("No se ha podido conectar con la base de datos, revise la conexión");
                    ConfiguraSQL sql = new ConfiguraSQL();
                    sql.ShowDialog();
                    if(ConfiguraSQL.Salir == true)
                    {
                        SQL();
                    }
                }
            }
        }


        public void LeeConfig()
        {

            try
            {
                string Line;
                StreamReader file = new StreamReader(Archivo);
                Config = new List<string>();
                while ((Line = file.ReadLine()) != null)
                {
                    Config.Add(Line);
                }
                file.Close();
                ImprimeConfig();
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                MessageBox.Show("Ocurrió un problema al leer el archivo Configuración.txt");
                pictureBox2.BackColor = Color.Red;
                pictureBox5.BackColor = Color.Red;
                pictureBox6.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas +  "Ocurrió un problema al leer el archivo Configuración.txt, más información:  \nl\nl" + e.ToString();
                EnvioCorreo(Subject, Body);
            }
        }

        public void ImprimeConfig()
        {
            try
            {
                label4.Text = Config[0];
                label5.Text = Config[1];
                label7.Text = Config[2];
                label1.Text = Config[10];
                label15.Text = Config[11];
                Minutos = Config[16].Split('-');
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                MessageBox.Show("Ocurrió un problema al imprimir la configuración de conexión");
                pictureBox2.BackColor = Color.Red;
                pictureBox5.BackColor = Color.Red;
                pictureBox6.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + "Ocurrió un problema al imprimir la configuración de conexión, más información:  \nl\nl" + e.ToString();
                EnvioCorreo(Subject, Body);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Contraseña contraseña = new Contraseña(1);
            contraseña.Show();
        }

        #endregion

        #region Correos y panel de control
        
        public void Progressbar(int v)
        {
            if (this.progressBar1.InvokeRequired)
            {
                vbar d = new vbar(Progressbar);
                this.Invoke(d, new object[] { v });
            }
            else
            {
                progressBar1.Value = v;
            }
        }

        public void Stringprogress(string s)
        {
            if (this.label24.InvokeRequired)
            {
                sbar d = new sbar(Stringprogress);
                this.Invoke(d, new object[] { s });
            }
            else
            {
                label24.Text = s;
            }
        }
        
        public void EnvioCorreo(string subject, string body)
        {
            Stringprogress("Enviando correo de reporte");
            SmtpClient client = new SmtpClient("", 0);
            NetworkCredential credentials = new NetworkCredential("", "");
            Progressbar(10);
            try
            {
                string Contraseña = Seguridad.DesEncriptar(Config[6]);

                client = new SmtpClient(Config[3], Convert.ToInt32(Config[4]));
                client.EnableSsl = true;

                credentials = new NetworkCredential(Config[5], Contraseña);
                client.Credentials = credentials;

                Progressbar(30);
                try
                {
                    MailMessage Mensaje = new MailMessage();
                    for (int i = 7; i <= 9; i++)
                    {
                        if (!Config[i].Equals(""))
                        {
                            Mensaje.To.Add(new MailAddress(Config[i]));
                        }
                    }
                    Progressbar(60);
                    Mensaje.From = new MailAddress(Config[5]);
                    Mensaje.Subject = Hotel + ": " + subject;
                    Mensaje.Body = body;
                    try
                    {
                        client.Send(Mensaje);
                        Progressbar(90);
                    }
                    catch (Exception e)
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        LogErrores(horas + " Error al enviar el correo de reporte, más información: \nl\nl" + e.ToString() + "\nl\nlMensaje no enviado: \nl\nl" + body);
                        pictureBox2.BackColor = Color.Red;
                        pictureBox5.BackColor = Color.Red;
                        pictureBox6.BackColor = Color.Red;
                    }
                }
                catch
                {
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    LogErrores(horas + "Error al contactar con el servidor SMTP");
                    ErroresPuerto1.Add("Error al contactar con el servidor SMTP" + horas);
                    pictureBox2.BackColor = Color.Red;
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                LogErrores(horas + " Error al configurar el envío de mensajes, más información: \nl\nl" + e.ToString() + "\nl\nlMensaje no enviado: \nl\nl" + body);
                pictureBox2.BackColor = Color.Red;
                pictureBox5.BackColor = Color.Red;
                pictureBox6.BackColor = Color.Red;

            }
            Progressbar(100);
            Progressbar(0);
            Stringprogress("");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lista_de_errores errores = new Lista_de_errores();
            errores.Show();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea restaurar el panel de control?", "Atención", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                int NumPuertos = Convert.ToInt32(Config[12]);
                
                if (NumPuertos == 0)
                {
                    pictureBox2.BackColor = Color.Lime;
                    pictureBox3.BackColor = Color.Lime;
                    pictureBox4.BackColor = Color.Lime;
                    pictureBox5.BackColor = Color.Transparent;
                    pictureBox6.BackColor = Color.Transparent;
                    pictureBox7.BackColor = Color.Transparent;
                    pictureBox8.BackColor = Color.Transparent;
                    pictureBox9.BackColor = Color.Transparent;
                    pictureBox10.BackColor = Color.Transparent;
                    aTimer.Enabled = true;
                    counter = 0;
                    counter2 = true;
                }
                else if (NumPuertos == 1)
                {
                    pictureBox2.BackColor = Color.Lime;
                    pictureBox3.BackColor = Color.Lime;
                    pictureBox4.BackColor = Color.Lime;
                    pictureBox5.BackColor = Color.Lime;
                    pictureBox8.BackColor = Color.Lime;
                    pictureBox7.BackColor = Color.Lime;
                    pictureBox6.BackColor = Color.Transparent;
                    pictureBox9.BackColor = Color.Transparent;
                    pictureBox10.BackColor = Color.Transparent;
                    aTimer.Enabled = true;
                    counter = 0;
                    counter2 = true;
                    p2.aTimer.Enabled = true;
                    p2.counter = 0;
                    p2.counter2 = true;   
                }
                else if (NumPuertos == 2)
                {
                    pictureBox2.BackColor = Color.Lime;
                    pictureBox3.BackColor = Color.Lime;
                    pictureBox4.BackColor = Color.Lime;
                    pictureBox5.BackColor = Color.Lime;
                    pictureBox6.BackColor = Color.Lime;
                    pictureBox7.BackColor = Color.Lime;
                    pictureBox8.BackColor = Color.Lime;
                    pictureBox9.BackColor = Color.Lime;
                    pictureBox10.BackColor = Color.Lime;
                    aTimer.Enabled = true;
                    counter = 0;
                    counter2 = true;
                    p2.aTimer.Enabled = true;
                    p2.counter = 0;
                    p2.counter2 = true;
                    p3.counter = 0;
                    p3.counter2 = true;
                    p3.aTimer.Enabled = true;
                }
                
            }
        }
        
        #endregion

        //Puerto 1

        #region Setup

        public void SetupServer()
        {
            try
            {
                if (RevisaLicIn() == true)
                {
                    FechaYa = DateTime.Now;
                    string nuevoin = FechaYa.ToString();
                    LogErrores("Nuevo inicio del servidor el: " + nuevoin);
                    Subject = "Estado del servidor";
                    Body = nuevoin + " Se ha iniciado el servidor";
                    EnvioCorreo(Subject, Body);

                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    Puerto1.Items.Add("configurando puerto 1...");
                    _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(Config[0]), Convert.ToInt32(Config[1])));
                    _serverSocket.Listen(5);
                    _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                    Puerto1.Items.Add("Puerto 1 listo");
                    LogErrores(horas + " Se ha iniciado el puerto 1");
                    Subject = "Estado del servidor";
                    Body = horas + " Se ha iniciado el puerto 1";

                    EnvioCorreo(Subject, Body);

                    DireccionDoc = Config[2];
                    CarpetaDoc = DireccionDoc;
                    PuertoDoc = Path.Combine(CarpetaDoc, "Puerto " + Config[1]);
                    SubCarpetaDoc = Path.Combine(PuertoDoc, "Registro individual");

                    bTimer.Enabled = true;
                    sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    sck.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                    ConectaExpo();
                }
                else
                {
                    Contraseña.Sal = 3;
                    MessageBox.Show("No se ha encontrado una licencia válida");
                    Application.Exit();
                }
                
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                Puerto1.Items.Add(horas + " Problema al configurar el puerto 1!");
                ErroresPuerto1.Add(horas + " Problema al configurar el puerto 1: probablemente la IP no es correcta");
                LogErrores(horas + " Problema al configurar el puerto 1: probablemente la IP no es correcta");
                Subject = "Error del servidor";
                Body = horas + " Problema al configurar el puerto 1: probablemente la IP no es correcta, más información:  \nl\nl" + e.ToString();
                pictureBox2.BackColor = Color.Red;
                EnvioCorreo(Subject, Body);
            }

        }
        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socketAccept = _serverSocket.EndAccept(AR);
                _clienSockets.Add(socketAccept);
                socketAccept.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socketAccept);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresPuerto1.Add(horas + " Puerto 1: Problema al comunicarse con el cliente (AcceptCallback)");
                LogErrores(horas + " Puerto 1: Problema al comunicarse con el cliente (AcceptCallback)");
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Problema al comunicarse con el cliente (AcceptCallback), más información:  \nl\nl" + e.ToString();
                EnvioCorreo(Subject, Body);
                pictureBox3.BackColor = Color.Red;
                aTimer.Enabled = false;
                counter2 = false;
            }
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            socket = (Socket)AR.AsyncState;
            try
            {
                received = socket.EndReceive(AR);
                dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);
                SetText(BitConverter.ToString(dataBuf));
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch
            {
                socket.Dispose();
                socket.Close();
            }
        }

        public void SetupSerial()
        {
            if (File.Exists(SerialPath))
            {
                try
                {
                    SerialConfig = new string[7];
                    using (StreamReader lector = new StreamReader(SerialPath))
                    {
                        for (int i = 0; i < 7; i++)
                        {
                            SerialConfig[i] = lector.ReadLine();
                        }
                        lector.Close();
                    }
                    label27.Text = SerialConfig[0];
                    DetaSerial();
                    serialPort1.PortName = SerialConfig[0];
                    serialPort1.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("No se ha podido leer la configuración del puerto serial!");
                }
            }
        }

        string SerialIn;
        char[] SerialIntCharVal;
        int SerialIntVal;
        string SerialInFinal;
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(1000);
            SerialInFinal = "";
            try
            {
                SerialIn = serialPort1.ReadExisting();
                SerialIntCharVal = SerialIn.ToCharArray();
                foreach(char l in SerialIntCharVal)
                {
                    SerialIntVal = Convert.ToInt32(l);
                    if (SerialInFinal.Equals(""))
                    {
                        if(String.Format("{0:X}", SerialIntVal).Length == 1)
                        {
                            SerialInFinal = "0" + String.Format("{0:X}", SerialIntVal);
                        }
                        else
                        {
                            SerialInFinal = String.Format("{0:X}", SerialIntVal);
                        }
                    }
                    else
                    {
                        if (String.Format("{0:X}", SerialIntVal).Length == 1)
                        {
                            SerialInFinal += "-0" + String.Format("{0:X}", SerialIntVal);
                        }
                        else
                        {
                            SerialInFinal += "-" + String.Format("{0:X}", SerialIntVal);
                        }
                    }
                }
                SetText(SerialInFinal);
            }
            catch (Exception ex)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresPuerto1.Add(horas + " Puerto 1: Problema al Recibir un dato (Serial)");
                LogErrores(horas + " Puerto 1: Problema al Recibir un dato (Serial)");
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Problema al Recibir un dato (Serial), más información:  \nl\nl" + ex.ToString();
                EnvioCorreo(Subject, Body);
                pictureBox3.BackColor = Color.Red;
                aTimer.Enabled = false;
                counter2 = false;
            }
        }
        public void DetaSerial()
        {
            try
            {
                serialPort1.BaudRate = Convert.ToInt32(SerialConfig[1]);

                if (SerialConfig[2].Equals("EVEN")) { serialPort1.Parity = Parity.Even; }
                else if (SerialConfig[2].Equals("MARK")) { serialPort1.Parity = Parity.Mark; }
                else if (SerialConfig[2].Equals("ODD")) { serialPort1.Parity = Parity.Odd; }
                else if (SerialConfig[2].Equals("NONE")) { serialPort1.Parity = Parity.None; }
                else if (SerialConfig[2].Equals("SPACE")) { serialPort1.Parity = Parity.Space; }

                if (SerialConfig[3].Equals("NONE")) { serialPort1.StopBits = StopBits.None; }
                else if (SerialConfig[3].Equals("ONE")) { serialPort1.StopBits = StopBits.One; }
                else if (SerialConfig[3].Equals("ONE POINT FIVE")) { serialPort1.StopBits = StopBits.OnePointFive; }
                else if (SerialConfig[3].Equals("TWO")) { serialPort1.StopBits = StopBits.Two; }

                serialPort1.DataBits = Convert.ToInt32(SerialConfig[4]);

                if (SerialConfig[5].Equals("NONE")) { serialPort1.Handshake = Handshake.None; }
                else if (SerialConfig[5].Equals("REQUEST TO SEND")) { serialPort1.Handshake = Handshake.RequestToSend; }
                else if (SerialConfig[5].Equals("XONXOFF")) { serialPort1.Handshake = Handshake.XOnXOff; }
                else if (SerialConfig[5].Equals("REQUESTTOSENDXONXOFF")) { serialPort1.Handshake = Handshake.RequestToSendXOnXOff; }

                if (SerialConfig[6].Equals("TRUE")) { serialPort1.RtsEnable = true; }
                else if (SerialConfig[6].Equals("FALSE")) { serialPort1.RtsEnable = false; }

                serialPort1.DtrEnable = true;
                serialPort1.RtsEnable = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("No se ha podido configurar el puerto serial!");
            }
        }
        
        private void button4_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Al entrar a la configuración, la conexión serial se pausará hasta cerrar la ventana de configuración, ¿Desea continuar?", "Advertencia!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                serialPort1.Close();
                SerialConfig SC = new SerialConfig();
                SC.Show();
                SC.FormClosed += SC_FormClosed;
            }
        }

        private void SC_FormClosed(object sender, FormClosedEventArgs e)
        {
            SetupSerial();
        }
        
        #endregion

        #region Txt

        public void SetText(string text)
        {
            try
            {
                FechaYa = DateTime.Now;
                TextoFin = "";
                counter = 0;
                if (text.Length > 3)
                {
                    if(counter2 == true)
                    {
                        pictureBox3.BackColor = Color.Lime;
                    }

                    textofin = "";
                    caracteres = text.Split('-');
                    foreach (string Caracteres in caracteres)
                    {
                        if (Caracteres.ToLower().Equals("0a") || Caracteres.ToLower().Equals("0d"))
                        {
                            if (textofin.Length > 2)
                            {
                                hexValuesSplit = textofin.Split(' ');
                                foreach (String hex in hexValuesSplit)
                                {
                                    value = Convert.ToInt32(hex, 16);
                                    stringValue = Char.ConvertFromUtf32(value);
                                    TextoFin = TextoFin + stringValue;
                                }
                                if (Config[19] == "1")
                                {
                                    CantCar(TextoFin, FechaYa.ToString());
                                }
                                else if (Config[19] == "2")
                                {
                                    SepCar(TextoFin, FechaYa.ToString());
                                }
                                TextoFin = "";
                                textofin = "";
                            }
                        }
                        else
                        {
                            if (textofin.Equals(""))
                            {
                                textofin = Caracteres;
                            }
                            else
                            {
                                textofin = textofin + " " + Caracteres;
                            }
                        }
                    }
                    if (textofin.Length > 2)
                    {
                        hexValuesSplit2 = textofin.Split(' ');
                        foreach (String hex in hexValuesSplit2)
                        {
                            value = Convert.ToInt32(hex, 16);
                            stringValue = Char.ConvertFromUtf32(value);
                            TextoFin = TextoFin + stringValue;
                        }
                        if(Config[19] == "1")
                        {
                            CantCar(TextoFin, FechaYa.ToString());
                        }
                        else if(Config[19] == "2")
                        {
                            SepCar(TextoFin, FechaYa.ToString());
                        }
                        TextoFin = "";
                    }
                }
            }
            catch(Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresEntradaDatosPuerto1.Add("Puerto 1: Ocurrió un error al recibir un dato (SetText)" + horas);
                LogErrores(horas + " Puerto 1: Ocurrió un error al recibir un dato (SetText)");
                aTimer.Enabled = false;
                Subject = "Puerto 1: Error del servidor";
                Body = horas + " Puerto 1: Ocurrió un error al recibir un dato (SetText), más información: \n\n" + e.ToString() + "\n\n Datos recibidos: " + text;
                pictureBox3.BackColor = Color.Red;
                counter2 = false;
                ArchivosSalvados("Desde SetText: " + horas + " " + text);
                EnvioCorreo(Subject, Body);
            }
        }
        public void CantCar(string TextoFin, string fecha)
        {
            if (TextoFin.Length == Convert.ToUInt32(RecepDatos.Config[13]))
            {
                RecibeCadCar(TextoFin);
                Salida(TextoFin, fecha);
            }
            else
            {
                ErrorCad(TextoFin, fecha);
            }
        }

        public void SepCar(string TextoFin, string fecha)
        {
            Campos = 0;
            if (pu1 == true && YaEsp == false)
            {
                try
                {
                    foreach (string s in P1)
                    {
                        TextoFin = TextoFin.Replace(s[1], RecepDatos.Config[22][0]);
                    }
                }
                catch (Exception r)
                {
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    ErroresEntradaDatosPuerto1.Add("Puerto 1: Ocurrió un error al reemplazar el caracter especial" + horas);
                    LogErrores(horas + " Puerto 1: Ocurrió un error al reemplazar el caracter especial");
                    Subject = "Puerto 1: Error del servidor";
                    aTimer.Enabled = false;
                    Body = horas + " Puerto 1: Ocurrió un error al reemplazar el caracter especial, más información: \n\n" + r.ToString();
                    pictureBox3.BackColor = Color.Red;
                    counter2 = false;
                    EnvioCorreo(Subject, Body);
                }
            }
            Partes = TextoFin.Split(Config[22][0]);
            foreach (var s in Partes)
            {
                Campos++;
            }
            if (Campos == Convert.ToInt32(Config[13]))
            {
                RecibeCadCamp(TextoFin);
                Salida(TextoFin, fecha);
            }
            else
            {
                ErrorCad(TextoFin, fecha);
            }
        }

        public void ErrorCad(string TextoFin, string fecha)
        {
            FechaYa = DateTime.Now;
            horas = FechaYa.ToString();
            ErroresEntradaDatosPuerto1.Add(horas + " Puerto 1: Se ha recibido un dato erroneo: Revisar correo.");
            pictureBox3.BackColor = Color.Red;
            counter2 = false;
            aTimer.Enabled = false;
            TextoFin = fecha + "  " + TextoFin;
            LogErrores("Puerto 1: Se ha recibido una entrada no válida:  " + TextoFin);
            Subject = "Puerto 1: Entrada errónea";
            Body = "Puerto 1: Se ha recibido una entrada no válida:  " + TextoFin;
            EnvioCorreo(Subject, Body);
        }

        public void Salida(string TextoFin, string fecha)
        {
            if (ApList == true)
            {
                if (Puerto1.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Salida);
                    Invoke(d, new object[] { TextoFin, fecha });
                }
                else
                {
                    Puerto1.Items.Add(fecha + "  " + TextoFin);
                    Puerto1.TopIndex = Puerto1.Items.Count - 1;

                    CarpetaCopia(fecha + "  " + TextoFin);
                    if (CreaArchivos == true)
                    {
                        CreaTxt(TextoFin);
                        bTimer.Enabled = true;
                    }
                }
            }
        }

        public void CreaTxt(string text)
        {
            FechaYa = DateTime.Now;
            Fecha = FechaYa.ToString("dd-MM-yyyy H-mm-ss");

            try
            {
                if (!Directory.Exists(PuertoDoc))
                {
                    Directory.CreateDirectory(PuertoDoc);
                }

                if (Directory.Exists(SubCarpetaDoc))
                {
                    CreaArchivo(text, Fecha);
                }
                else
                {
                    Directory.CreateDirectory(SubCarpetaDoc);
                    CreaArchivo(text, Fecha);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Error al generar o leer la carpeta Puerto 1 en la dirección de almacenamiento de los datos");
                LogErrores(horas + " Error al generar o leer la carpeta Registro en la dirección de almacenamiento de los datos");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Error al generar o leer la carpeta Registro en la dirección de almacenamiento de los datos, más información:  \nl\nl" + e.ToString() + "\nl\nlRecuperación de dato: " + Fecha + "   " + text;
                ArchivosSalvados("Desde archivos individuales: " + horas + "  " + text);
                EnvioCorreo(Subject, Body);
            }

        }
        private void CreaArchivo(string text, string fecha)
        {
            try
            {
                ArchivoDoc = Path.Combine(SubCarpetaDoc, fecha + ".txt");
                if (File.Exists(ArchivoDoc))
                {
                    SetVeces = 0;
                    while (File.Exists(ArchivoDoc))
                    {
                        SetVeces++;
                        ArchivoDoc = Path.Combine(SubCarpetaDoc, fecha + "(" + SetVeces + ")" + ".txt");
                    }
                    using (StreamWriter escritor = new StreamWriter(ArchivoDoc))
                    {
                        escritor.WriteLine(text);
                        escritor.Close();
                    }
                }
                else
                {
                    using (StreamWriter escritor = new StreamWriter(ArchivoDoc))
                    {
                        escritor.WriteLine(text);
                        escritor.Close();
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Error al generar los archivos de texto individuales");
                LogErrores(horas + " Error al generar los archivos de texto individuales");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Error al generar los archivos de texto individuales, más información:  \nl\nl" + e.ToString() + "\nl\nlRecuperación de dato: " + fecha + "   " + text;
                EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde archivos individuales: " + horas + "  " + text);
            }
        }

        #endregion
        
        #region CopiaSeguridad

        public void CarpetaCopia(string text)
        {
            try
            {
                DireccionCopi = Config[2];

                CarpetaCopi = DireccionCopi;
                PuertoDoc = System.IO.Path.Combine(CarpetaCopi, "Puerto " + Config[1]);
                if (!System.IO.Directory.Exists(PuertoDoc))
                {
                    System.IO.Directory.CreateDirectory(PuertoDoc);
                }

                SubCarpetaCopi = System.IO.Path.Combine(PuertoDoc, "Registro mensual");
                if (System.IO.Directory.Exists(SubCarpetaCopi))
                {
                    CreaArchivoCopi(text);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(SubCarpetaCopi);
                    CreaArchivoCopi(text);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Puerto 1: Ocurrió un error al generar o leer la carpeta de registro mensual");
                LogErrores(horas + " Puerto 1: Ocurrió un error al generar o leer la carpeta de registro mensual");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ocurrió un error al generar o leer la carpeta de registro mensual, más información:  \nl\nl" + e.ToString();
                EnvioCorreo(Subject, Body);
            }
        }

        public void CreaArchivoCopi(string text)
        {
            try
            {
                FechaYa = DateTime.Now;
                string NombreMes = FechaYa.ToString("MMMM-yyyy");
                ArchivoCopi = System.IO.Path.Combine(SubCarpetaCopi, NombreMes + ".txt");

                using (StreamWriter w = File.AppendText(ArchivoCopi))
                {
                    Log(text, w);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Puerto 1: Error al crear el archivo de registro mensual");
                pictureBox4.BackColor = Color.Red;
                LogErrores(horas + "Puerto 1: Error al crear el archivo de registro mensual");
                Subject = "Error del servidor";
                Body = horas + " Puerto1: Error al escribir en el archivo de registros mensuales, más información: \n\n" + e.ToString() + "\n\nRecuperación de datos:  " + text;
                ArchivosSalvados("Desde copia de seguridad: " + text);
                EnvioCorreo(Subject, Body);
            }
        }
        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine(logMessage);
        }

        #endregion

        #region ArchivosSalvados

        public void ArchivosSalvados(string textsalv)
        {
            if (!Directory.Exists(PuertoSalv))
            {
                Directory.CreateDirectory(PuertoSalv);
            }

            SubCarpetaSalv = Path.Combine(PuertoSalv, "Puerto " + Config[1]);
            ArchivoSalv = Path.Combine(SubCarpetaSalv, "Archivos_Salvados.txt");

            if (!File.Exists(ArchivoSalv))
            {
                try
                {
                    Directory.CreateDirectory(SubCarpetaSalv);
                    using (StreamWriter w = File.AppendText(ArchivoSalv))
                    {
                        Log(textsalv, w);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Puerto 1: Ocurrió un problema al crear los archivos salvados");
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    ErroresSaliDatosPuerto1.Add(horas + " Puerto 1: Ocurrió un problema al crear los archivos salvados");
                    LogErrores(horas + " Puerto 1: Ocurrió un problema al crear los archivos salvados");
                    pictureBox4.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: Ocurrió un problema al crear los archivos salvados, más información:  \nl\nl" + e.ToString() + "\nl\nlArchivo no guardado: " + textsalv;
                    EnvioCorreo(Subject, Body);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter w = File.AppendText(ArchivoSalv))
                    {
                        Log(textsalv, w);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Puerto 1: Ocurrió un problema al escribir los archivos salvados");
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    ErroresSaliDatosPuerto1.Add(horas + " Puerto 1: Ocurrió un problema al escribir los archivos salvados");
                    LogErrores(horas + " Puerto 1: Ocurrió un problema al escribir los archivos salvados");
                    pictureBox4.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: Ocurrió un problema al crear los archivos salvados, más información:  \nl\nl" + e.ToString() + "\nl\nlArchivo no guardado: " + textsalv;
                    EnvioCorreo(Subject, Body);
                }
            }
        }


        #endregion

        #region Caracteres Especiales

        public void CarEsp()
        {
            string Line = null;
            bool Puerto1 = false;
            bool Puerto2 = false;
            bool Puerto3 = false;
            if (File.Exists(ArchivoCarEsp))
            {
                using (StreamReader lector = new StreamReader(ArchivoCarEsp))
                {
                    while ((Line = lector.ReadLine()) != null)
                    {
                        if ((Line.Equals("Puerto 1:") || Puerto1 == true) && !Line.Equals("Puerto 2:"))
                        {
                            Puerto1 = true;
                            if (!Line.Equals("Puerto 1:"))
                            {
                                P1.Add(Line);
                            }
                        }
                        if ((Line.Equals("Puerto 2:") || Puerto2 == true) && !Line.Equals("Puerto 3:"))
                        {
                            Puerto1 = false;
                            Puerto2 = true;
                            if (!Line.Equals("Puerto 2:"))
                            {
                                P2.Add(Line);
                            }
                        }
                        if (Line.Equals("Puerto 3:") || Puerto3 == true)
                        {
                            Puerto1 = false;
                            Puerto2 = false;
                            Puerto3 = true;
                            if (!Line.Equals("Puerto 3:"))
                            {
                                P3.Add(Line);
                            }
                        }
                        if(P1.Count != 0)
                        {
                            pu1 = true;
                        }
                        if (P2.Count != 0)
                        {
                            pu2 = true;
                        }
                        if (P3.Count != 0)
                        {
                            pu3 = true;
                        }
                    }
                    lector.Close();
                }

            }
            else
            {
                CarEspe = false;
            }
        }

        #endregion

        #region Precarga y Tipos de llamada

        public void CargaConfigSQL()
        {
            try
            {
                using (StreamReader lector = new StreamReader(ArchivoSQL))
                {
                    string linea;
                    bool Puerto1 = false;
                    bool Puerto2 = false;
                    bool Puerto3 = false;
                    while ((linea = lector.ReadLine()) != null)
                    {
                        if ((linea.Equals("Puerto 1:") || Puerto1 == true) && !linea.Equals("Puerto 2:"))
                        {
                            Puerto1 = true;
                            if (!linea.Equals("Puerto 1:"))
                            {
                                ProP1.Add(linea);
                            }
                        }
                        if ((linea.Equals("Puerto 2:") || Puerto2 == true) && !linea.Equals("Puerto 3:"))
                        {
                            Puerto1 = false;
                            Puerto2 = true;
                            if (!linea.Equals("Puerto 2:"))
                            {
                                ProP2.Add(linea);
                            }
                        }
                        if (linea.Equals("Puerto 3:") || Puerto3 == true)
                        {
                            Puerto1 = false;
                            Puerto2 = false;
                            Puerto3 = true;
                            if (!linea.Equals("Puerto 3:"))
                            {
                                ProP3.Add(linea);
                            }
                        }
                    }
                    lector.Close();
                }

                try
                {
                    bool LlamadasE = false;
                    bool LlamadasS = false;
                    bool LlamadasI = false;

                    foreach (string linea in ProP1)
                    {
                        if ((linea.Equals("Llamadas entrantes:") || LlamadasE == true) && !linea.Equals("Llamadas salientes:"))
                        {
                            LlamadasE = true;
                            if (!linea.Equals("Llamadas entrantes:"))
                            {
                                ConfigE.Add(linea);
                            }
                        }
                        if ((linea.Equals("Llamadas salientes:") || LlamadasS == true) && !linea.Equals("Llamadas internas:"))
                        {
                            LlamadasE = false;
                            LlamadasS = true;
                            if (!linea.Equals("Llamadas salientes:"))
                            {
                                ConfigS.Add(linea);
                            }
                        }
                        if (linea.Equals("Llamadas internas:") || LlamadasI == true)
                        {
                            LlamadasE = false;
                            LlamadasS = false;
                            LlamadasI = true;
                            if (!linea.Equals("Llamadas internas:"))
                            {
                                ConfigI.Add(linea);
                            }
                        }
                    }

                    string confige = "";
                    string configs = "";
                    string configi = "";

                    try
                    {

                        foreach (string s in ConfigE)
                        {
                            if (s[0].Equals('l'))
                            {
                                string[] Partes = s.Split('-');
                                confige = Partes[1];
                            }
                        }
                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('l'))
                            {
                                string[] Partes = s.Split('-');
                                configs = Partes[1];
                            }
                        }
                        foreach (string s in ConfigI)
                        {
                            if (s[0].Equals('l'))
                            {
                                string[] Partes = s.Split('-');
                                configi = Partes[1];
                            }
                        }

                        try
                        {
                            if (confige.Equals(configs) && confige.Equals(configi) && configs.Equals(configi))
                            {
                                if (Config[19].Equals("1"))
                                {
                                    TipoLlampos = configs.Split(':');
                                }
                                else if (Config[19].Equals("2"))
                                {
                                    TipoLlamPos = confige;
                                }
                                Igual = true;
                            }
                            else
                            {
                                if (Config[19].Equals("1"))
                                {
                                    TipoLlamposE = confige.Split(':');
                                    TipoLlamposS = configs.Split(':');
                                    TipoLlamposI = configi.Split(':');
                                }
                                else if (Config[19].Equals("2"))
                                {
                                    TipoLlamPosE = confige;
                                    TipoLlamPosS = configs;
                                    TipoLlamPosI = configi;
                                }
                                Igual = false;
                            }

                            try
                            {
                                using (Conexion = new MySqlConnection(Config[25]))
                                {
                                    Conexion.Open();
                                    query = "Select * From formatos Where Tipo_Formato = 'l'";
                                    comando = new MySqlCommand(query, Conexion);

                                    MySqlDataReader lee = comando.ExecuteReader();

                                    while (lee.Read())
                                    {
                                        TiposLlamada.Add(lee["Strg_Formato"].ToString());
                                        TiposLlamada2.Add(lee["Deta_Formato"].ToString());
                                    }
                                    lee.Close();
                                    Conexion.Close();
                                }

                                try
                                {
                                    using (Conexion = new MySqlConnection(Config[25]))
                                    {
                                        Conexion.Open();
                                        comando = new MySqlCommand("Select * From parametros where parametro = 'Redondeo_IVA'", Conexion);
                                        lee = comando.ExecuteReader();
                                        lee.Read();
                                        if (lee["seleccion"].ToString().Equals("si"))
                                        {
                                            Redondeo_IVA = true;
                                        }
                                        else
                                        {
                                            Redondeo_IVA = false;
                                        }
                                        lee.Close();
                                        comando = new MySqlCommand("Select * From parametros where parametro = 'Redondeo_Recargo'", Conexion);
                                        lee = comando.ExecuteReader();
                                        lee.Read();
                                        if (lee["seleccion"].ToString().Equals("si"))
                                        {
                                            Redondeo_Recargo = true;
                                        }
                                        else
                                        {
                                            Redondeo_Recargo = false;
                                        }
                                        lee.Close();
                                    }
                                }
                                catch (Exception e)
                                {
                                    MessageBox.Show("Ocurrió un error al precargar los parámetros, más información: \n\n" + e.ToString());
                                    MessageBox.Show("El programa no se podrá iniciar");
                                    Contraseña.Sal = 1;
                                    ConfigCorrecta = false;

                                }

                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Ocurrió un error al precargar los tipos de llamada, más información: \n\n" + e.ToString());
                                MessageBox.Show("El programa no se podrá iniciar");
                                Contraseña.Sal = 1;
                                ConfigCorrecta = false;
                            }

                        }
                        catch (Exception e)
                        {
                            MessageBox.Show("Ocurrió un error al detectar la posición del tipo de llamada, más información: \n\n" + e.ToString());
                            MessageBox.Show("El programa no se podrá iniciar");
                            Contraseña.Sal = 1;
                            ConfigCorrecta = false;
                        }

                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("No se ha detectado el campo \"Tipo de llamada\" revise el archivo ConfiguracionSQL.txt, más información: \n\n" + e.ToString());
                        MessageBox.Show("El programa no se podrá iniciar");
                        Contraseña.Sal = 1;
                        ConfigCorrecta = false;
                    }


                }
                catch (Exception e)
                {
                    MessageBox.Show("Ocurrió un error al separar los tipos de llamadas, más información: \n\n" + e.ToString());
                    MessageBox.Show("El programa no podrá iniciar, revise el archivo ConfiguracionSQL.txt");
                    Contraseña.Sal = 1;
                    ConfigCorrecta = false;
                }


            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al leer la configuración de los puertos, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no podrá iniciar, revise el archivo ConfiguracionSQL.txt");
                Contraseña.Sal = 1;
                ConfigCorrecta = false;
            }
            
        }

        public void RecibeCadCar(string Cadena)
        {
            CreaArchivos = true;
            Tipo = null;
            i = 0;
            try
            {
                if (Igual == true)
                {
                    TipoLlamadaCad = Cadena.Substring(Convert.ToInt32(TipoLlampos[0]), Convert.ToInt32(TipoLlampos[1]));
                    
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(TipoLlamadaCad))
                        {
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    if (!string.IsNullOrEmpty(Tipo))
                    {
                        ProcesaPosCar(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = false;
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\nTipo no encontrado: |" + TipoLlamadaCad + "|";
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("No se ha podido detectar el tipo de llamada de la cadena"), Cadena, horas, "No se ha podido detectar el tipo de llamada de la cadena");
                    }
                }
                else
                {
                    Veces = 0;
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals((Cadena.Substring(Convert.ToInt32(TipoLlamposE[0]), Convert.ToInt32(TipoLlamposE[1])))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals((Cadena.Substring(Convert.ToInt32(TipoLlamposS[0]), Convert.ToInt32(TipoLlamposS[1])))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals((Cadena.Substring(Convert.ToInt32(TipoLlamposI[0]), Convert.ToInt32(TipoLlamposI[1])))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    if (!string.IsNullOrEmpty(Tipo) && Veces == 1)
                    {
                        ProcesaPosCar(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = false;
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\nveces encontrado: " + Veces + "\nTipo obtenido: |" + Tipo + "|";
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("No se ha podido detectar el tipo de llamada de la cadena"), Cadena, horas, "No se ha podido detectar el tipo de llamada de la cadena");
                    }
                }
            }

            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información\n ");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                CreaArchivos = false;
                EnvioCorreo(Subject, Body);
                CadenasFallidas(CodigoError("No se ha podido detectar el tipo de llamada de la cadena"), Cadena, horas, " Ha ocurrido un error al detectar el tipo de llamada en la cadena");
            }
        }
        
        public void RecibeCadCamp(string Cadena)
        {
            CreaArchivos = true;
            Tipo = null;
            i = 0;
            try
            {
                if (Igual == true)
                {
                    TipoLlamadaCad = Cadena.Split(Config[22][0])[Convert.ToInt32(TipoLlamPos)];
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(TipoLlamadaCad))
                        {
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    ProcesaPosCamp(Tipo, Cadena);
                }
                else
                {
                    Veces = 0;
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(Cadena.Split(Config[22][0])[Convert.ToInt32(TipoLlamPosE)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Split(Config[22][0])[Convert.ToInt32(TipoLlamPosS)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Split(Config[22][0])[Convert.ToInt32(TipoLlamPosI)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }

                    if (!string.IsNullOrEmpty(Tipo) && Veces == 1)
                    {
                        ProcesaPosCamp(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = false;
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\nveces encontrado: " + Veces + "\nTipo obtenido: |" + Tipo + "|";
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("No se ha podido detectar el tipo de llamada de la cadena"), Cadena, horas, "No se ha podido detectar el tipo de llamada de la cadena");
                    }
                }
            }

            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                CreaArchivos = false;
                EnvioCorreo(Subject, Body);
                CadenasFallidas(CodigoError("No se ha podido detectar el tipo de llamada de la cadena"), Cadena, horas, " Ha ocurrido un error al detectar el tipo de llamada en la cadena");
            }

        }

        #endregion

        #region Carga Datos Cadena

        public void ProcesaPosCar(string Tipo, string Cadena)
        {
            IntentosCon = 0;
            try
            {
                if (Tipo.Equals("Llamada entrante"))
                {
                    foreach (string s in ConfigE)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    if (ConfigE[0].Equals("1"))
                    {
                        ProcesamientoCar(Tipo, Cadena);
                    }
                }
                else if (Tipo.Equals("Llamada saliente"))
                {
                    foreach (string s in ConfigS)
                    {
                        AgregaValoresCar(Cadena, s);
                    }

                    if (ConfigS[0].Equals("1"))
                    {
                        ProcesamientoCar(Tipo, Cadena);

                    }
                }
                else if (Tipo.Equals("Llamada interna"))
                {
                    foreach (string s in ConfigI)
                    {
                        AgregaValoresCar(Cadena, s);
                    }

                    if (ConfigI[0].Equals("1"))
                    {
                        ProcesamientoCar(Tipo, Cadena);
                    }
                }
                else if(Tipo.Equals("Llamada invalida"))
                {
                    SubeExpo = false;
                    LogErrores("Se ha recibido una llamada inválida:    " + Cadena);
                    CadenasFallidas(CodigoError("Llamada invalida"), Cadena, horas, " Se ha recibido una llamada inválida");
                    CreaArchivos = false;
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al detectar el tipo de llamada en la cadena, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                CreaArchivos = false;
                EnvioCorreo(Subject, Body);
                CadenasFallidas(CodigoError("Ha ocurrido un error al detectar el tipo de llamada en la cadena"), Cadena, horas, " Ha ocurrido un error al detectar el tipo de llamada en la cadena");
            }
        }

        public void ProcesaPosCamp(string Tipo, string Cadena)
        {
            IntentosCon = 0;
            try
            {
                if (Tipo.Equals("Llamada entrante"))
                {
                    foreach (string s in ConfigE)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }

                    if (ConfigE[0].Equals("1"))
                    {
                        ProcesamientoCamp(Tipo, Cadena);
                    }
                }
                else if (Tipo.Equals("Llamada saliente"))
                {
                    foreach (string s in ConfigS)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }

                    if (ConfigS[0].Equals("1"))
                    {
                        ProcesamientoCamp(Tipo, Cadena);
                    }
                }
                else if (Tipo.Equals("Llamada interna"))
                {
                    foreach (string s in ConfigI)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }

                    if (ConfigI[0].Equals("1"))
                    {
                        ProcesamientoCamp(Tipo, Cadena);
                    }
                }
                else if (Tipo.Equals("Llamada invalida"))
                {
                    LogErrores("Se ha recibido una llamada inválida:    " + Cadena);
                    CadenasFallidas(CodigoError("Llamada invalida"), Cadena, horas, " Se ha recibido una llamada inválida");
                    CreaArchivos = false;
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al detectar el tipo de llamada en la cadena, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                CreaArchivos = false;
                EnvioCorreo(Subject, Body);
                CadenasFallidas(CodigoError("Ha ocurrido un error al detectar el tipo de llamada en la cadena"), Cadena, horas, " Ha ocurrido un error al detectar el tipo de llamada en la cadena");
            }
        }

        #endregion
        
        #region Sube Tabla e interface

        public void AgregaValoresCar(string Cadena, string s)
        {
            try
            {
                if (s[0].Equals('F'))
                {
                    F_Fecha_Final_Lamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('H'))
                {
                    H_Hora_Final_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('E'))
                {
                    E_Extension = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('T'))
                {
                    T_Troncal = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('N'))
                {
                    N_Numero_Marcado = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('D'))
                {
                    D_Duracion = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('P'))
                {
                    P_Codigo_Personal = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('m'))
                {
                    m_Fecha_Inicial_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('j'))
                {
                    j_Hora_Inicial_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('l'))
                {
                    l_Tipo_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('R'))
                {
                    R_Trafico_Interno_Externo = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al cargar los datos de la cadena en memoria, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                CreaArchivos = true;
                EnvioCorreo(Subject, Body);
            }
        }

        public void AgregaValoresCamp(string Cadena, string s)
        {
            try
            {
                if (s[0].Equals('F'))
                {
                    F_Fecha_Final_Lamada = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('H'))
                {
                    H_Hora_Final_Llamada = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('E'))
                {
                    E_Extension = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('T'))
                {
                    T_Troncal = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('N'))
                {
                    N_Numero_Marcado = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('D'))
                {
                    D_Duracion = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('P'))
                {
                    P_Codigo_Personal = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('m'))
                {
                    m_Fecha_Inicial_Llamada = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('j'))
                {
                    j_Hora_Inicial_Llamada = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('l'))
                {
                    l_Tipo_Llamada = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('R'))
                {
                    R_Trafico_Interno_Externo = Cadena.Split(Config[22][0])[Convert.ToInt32(s.Split('-')[1])];
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al cargar los datos de la cadena en memoria, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                CreaArchivos = true;
                EnvioCorreo(Subject, Body);
            }
        }

        string FormatoFechaEnv;
        string FormatoHoraEnv;
        string HoraFinEnv;
        int PosEnv = 0;
        string HoraEnv = "";
        string MinEnv = "";
        string AmPmEnv = "";
        bool ConvertirH;

        public void SubeTabla(bool InterEnv)
        {
            try
            {
                using (Conexion = new MySqlConnection(Config[25]))
                {
                    Conexion.Open();
                    FormatoFechaSQL = "";
                    foreach (string s in ConfigS)
                    {
                        if (s[0].Equals('F'))
                        {
                            FormatoFechaSQL = s.Split('-')[2];
                        }
                    }
                    if (string.IsNullOrEmpty(FormatoFechaSQL))
                    {
                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('m'))
                            {
                                FormatoFechaSQL = s.Split('-')[2];
                            }
                        }
                        if (string.IsNullOrEmpty(FormatoFechaSQL))
                        {
                            FormatoFechaSQL = "error";
                        }
                    }

                    if(!string.IsNullOrEmpty(FormatoFechaSQL) || !FormatoFechaSQL.Equals("error"))
                    {
                        FormatoFechaSQL = FormatoFechaSQL.ToLower();
                        LeePos(FormatoFechaSQL);
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString("yyyy");
                        if(TieneAño(FormatoFechaSQL) == true)
                        {
                            if (!F_Fecha_Final_Lamada.Equals("-"))
                            {
                                if (F_Fecha_Final_Lamada.Substring(Convert.ToInt32(PosAño.Split('-')[0]), Convert.ToInt32(PosAño.Split('-')[1])).Length == 2)
                                {
                                    NombreTabla = "20" + F_Fecha_Final_Lamada.Substring(Convert.ToInt32(PosAño.Split('-')[0]), Convert.ToInt32(PosAño.Split('-')[1]));
                                }
                                else
                                {
                                    NombreTabla = F_Fecha_Final_Lamada.Substring(Convert.ToInt32(PosAño.Split('-')[0]), Convert.ToInt32(PosAño.Split('-')[1]));
                                }
                                NombreTabla += " " + F_Fecha_Final_Lamada.Substring(Convert.ToInt32(PosMes.Split('-')[0]), Convert.ToInt32(PosMes.Split('-')[1]));

                            }
                            else if (!m_Fecha_Inicial_Llamada.Equals("-"))
                            {
                                if (m_Fecha_Inicial_Llamada.Substring(Convert.ToInt32(PosAño.Split('-')[0]), Convert.ToInt32(PosAño.Split('-')[1])).Length == 2)
                                {
                                    NombreTabla = "20" + m_Fecha_Inicial_Llamada.Substring(Convert.ToInt32(PosAño.Split('-')[0]), Convert.ToInt32(PosAño.Split('-')[1]));
                                }
                                else
                                {
                                    NombreTabla = m_Fecha_Inicial_Llamada.Substring(Convert.ToInt32(PosAño.Split('-')[0]), Convert.ToInt32(PosAño.Split('-')[1]));
                                }
                                NombreTabla += " " + m_Fecha_Inicial_Llamada.Substring(Convert.ToInt32(PosMes.Split('-')[0]), Convert.ToInt32(PosMes.Split('-')[1]));
                            }
                        }
                        else
                        {
                            if (!F_Fecha_Final_Lamada.Equals("-"))
                            {
                                NombreTabla = horas;
                                NombreTabla += " " + F_Fecha_Final_Lamada.Substring(Convert.ToInt32(PosMes.Split('-')[0]), Convert.ToInt32(PosMes.Split('-')[1]));
                            }
                            else if (!m_Fecha_Inicial_Llamada.Equals("-"))
                            {
                                NombreTabla = horas;
                                NombreTabla += " " + m_Fecha_Inicial_Llamada.Substring(Convert.ToInt32(PosMes.Split('-')[0]), Convert.ToInt32(PosMes.Split('-')[1]));
                            }
                        }
                    }
                    else
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al leer el formato de la fecha");
                        LogErrores(horas + " Ha ocurrido un error al leer el formato de la fecha");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Ha ocurrido un error al leer el formato de la fecha, La cadena se guardará en la carpeta de archivos separados";
                        CreaArchivos = true;
                        EnvioCorreo(Subject, Body);

                    }

                    #region Formatos y 24h

                    FormatoFechaEnv = "";
                    FormatoHoraEnv = "";
                    HoraFinEnv = "";

                    if (!F_Fecha_Final_Lamada.Equals("-"))
                    {
                        HoraFinEnv = H_Hora_Final_Llamada;
                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('F'))
                            {
                                FormatoFechaEnv = s.Split('-')[2];
                            }
                        }
                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('H'))
                            {
                                FormatoHoraEnv = s.Split('-')[2];
                            }
                        }
                    }
                    else if (!m_Fecha_Inicial_Llamada.Equals("-"))
                    {
                        HoraFinEnv = j_Hora_Inicial_Llamada;
                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('m'))
                            {
                                FormatoFechaEnv = s.Split('-')[2];
                            }
                        }
                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('j'))
                            {
                                FormatoHoraEnv = s.Split('-')[2];
                            }
                        }
                    }
                    

                    ConvertirH = false;
                    FormatoHoraEnv = FormatoHoraEnv.ToLower();

                    foreach(char s in FormatoHoraEnv)
                    {
                        if(s == 'x')
                        {
                            ConvertirH = true;
                        }
                    }

                    if(ConvertirH == true)
                    {
                        HoraEnv = "";
                        MinEnv = "";
                        AmPmEnv = "";
                        PosEnv = 0;

                        foreach (char s in FormatoHoraEnv)
                        {
                            if (s == 'h')
                            {
                                HoraEnv += PosEnv.ToString();
                            }
                            else if (s == 'm')
                            {
                                MinEnv += PosEnv.ToString();
                            }
                            else if (s == 'x')
                            {
                                AmPmEnv += PosEnv.ToString();
                            }
                            PosEnv++;
                        }
                        if (!HoraEnv.Equals(""))
                        {
                            HoraEnv = HoraFinEnv.Substring(Convert.ToInt32(HoraEnv[0].ToString()), HoraEnv.Length);
                        }
                        if (!MinEnv.Equals(""))
                        {
                            MinEnv = HoraFinEnv.Substring(Convert.ToInt32(MinEnv[0].ToString()), MinEnv.Length);
                        }
                        if (!AmPmEnv.Equals(""))
                        {
                            AmPmEnv = HoraFinEnv.Substring(Convert.ToInt32(AmPmEnv[0].ToString()), AmPmEnv.Length).ToLower();
                            FormatoHoraEnv = "HHmm";
                            if (AmPmEnv.Equals("am") && HoraEnv.Equals("12"))
                            {
                                HoraEnv = "00";
                            }
                            if (AmPmEnv.Equals("pm") && (Convert.ToInt32(HoraEnv) >= 1 && Convert.ToInt32(HoraEnv) <= 11))
                            {
                                HoraEnv = (Convert.ToInt32(HoraEnv) + 12).ToString();
                            }
                            if (!H_Hora_Final_Llamada.Equals("-"))
                            {
                                H_Hora_Final_Llamada = HoraEnv + MinEnv;
                            }
                            else if (!j_Hora_Inicial_Llamada.Equals("-"))
                            {
                                j_Hora_Inicial_Llamada = HoraEnv + MinEnv;
                            }
                        }

                    }

                    #endregion

                    query = @"CREATE TABLE IF NOT EXISTS `anacostel`.`" + NombreTabla + @"` (
                      `idLlamadasTelefonicas` INT(11) NOT NULL AUTO_INCREMENT COMMENT 'Consecutivo Interno de llamadas. Cronologico',
                      `FFechaFinalLlamada` VARCHAR(10) NOT NULL DEFAULT '-' COMMENT 'Fecha en la que se colgo la llamada',
                      `HHoraFinalLlamada` VARCHAR(20) NOT NULL DEFAULT '-' COMMENT 'Hora en la que se colgo la llamada',
                      `EExtension` VARCHAR(7) NOT NULL DEFAULT '-' COMMENT 'Extension desde la  cual se realizo la llamada',
                      `TTroncal` VARCHAR(7) NOT NULL DEFAULT '-' COMMENT 'Troncal que sirvio de enlace para reallizar la llamada',
                      `NNumeroMarcado` VARCHAR(25) NOT NULL DEFAULT '-' COMMENT 'Numero marcado en llamada saliente o numero desde el cual marcaron en llamada entrante',
                      `DDuracion` VARCHAR(10) NOT NULL DEFAULT '-' COMMENT 'Duracion de la comunicacion',
                      `PCodigoPersonal` VARCHAR(6) NOT NULL DEFAULT '-' COMMENT 'Codigo personal marcado para ejecutar una llamada',
                      `mFechaInicialLlamada` VARCHAR(10) NOT NULL DEFAULT '-' COMMENT 'Fecha en la que se inicio la llamada',
                      `jHoraInicialLlamada` VARCHAR(45) NOT NULL DEFAULT '-' COMMENT 'Hora en la que se inicio la llamada',
                      `lTipoLlamada` VARCHAR(10) NOT NULL DEFAULT '-' COMMENT 'Tipo de llamada DDN CEL LOC TOL DDI ENT ITH INV SAT TOL',
                      `RTraficoInternoExterno` VARCHAR(5) NOT NULL DEFAULT '-' COMMENT 'Llamada interna o externa',
                      `ClaseLlamada` VARCHAR(45) NOT NULL,
                      `Destino` VARCHAR(45) NOT NULL,
                      `CentroDeCosto` VARCHAR(45) NOT NULL,
                      `DuracionLlamada` VARCHAR(45) NOT NULL,
                      `DuracionLlamadaAproximada` VARCHAR(45) NOT NULL,
                      `DurMinima` VARCHAR(45) NOT NULL,
                      `PlanTarifa` VARCHAR(45) NOT NULL,
                      `NumeroTarifa` VARCHAR(45) NOT NULL,
                      `NumeFolio` VARCHAR(45) NOT NULL,
                      `ClaseExtension` VARCHAR(45) NOT NULL,
                      `NombreExtension` VARCHAR(45) NOT NULL,
                      `DuracionRango1` VARCHAR(45) NOT NULL,
                      `DuracionRango2` VARCHAR(45) NOT NULL,
                      `DuracionRango3` VARCHAR(45) NOT NULL,
                      `DuracionRango4` VARCHAR(45) NOT NULL,
                      `DuracionRango5` VARCHAR(45) NOT NULL,
                      `ValorRango1` VARCHAR(45) NOT NULL,
                      `ValorRango2` VARCHAR(45) NOT NULL,
                      `ValorRango3` VARCHAR(45) NOT NULL,
                      `ValorRango4` VARCHAR(45) NOT NULL,
                      `ValorRango5` VARCHAR(45) NOT NULL,
                      `CargoFijo` VARCHAR(45) NOT NULL,
                      `RecargoServicioPorcentaje` VARCHAR(45) NOT NULL,
                      `RecargoServicioValor` VARCHAR(45) NOT NULL,
                      `Base_IVA` VARCHAR(45) NOT NULL,
                      `IVA_Incluye_Recago` VARCHAR(45) NOT NULL,
                      `ValorIVA` VARCHAR(45) NOT NULL,
                      `ValorTotal` VARCHAR(45) NOT NULL,
                      `TramaCompleta` VARCHAR(200) NOT NULL,
                      `Operador` VARCHAR(45) NOT NULL,
                      `DigitosMinimos` VARCHAR(45) NOT NULL,
                      `NombreTarifa` VARCHAR(45) NOT NULL,
                      `Tarifa1` VARCHAR(45) NOT NULL,
                      `Tarifa2` VARCHAR(45) NOT NULL,
                      `Tarifa3` VARCHAR(45) NOT NULL,
                      `Tarifa4` VARCHAR(45) NOT NULL,
                      `Tarifa5` VARCHAR(45) NOT NULL,
                      `ValorLlamadaTarifa` VARCHAR(45) NOT NULL,
                      `PorcentajeIVA` VARCHAR(45) NOT NULL,
                      `Errores` VARCHAR(45) NOT NULL,
                      PRIMARY KEY (`idLlamadasTelefonicas`),
                      UNIQUE INDEX `idLLAMADAS_TELEFONICAS_UNIQUE` (`idLlamadasTelefonicas` ASC))
                    ENGINE = InnoDB
                    AUTO_INCREMENT = 0;";
                    comando = new MySqlCommand(query, Conexion);
                    comando.ExecuteNonQuery();


                    query = @"insert into `" + NombreTabla + @"` (FFechaFinalLlamada, HHoraFinalLlamada, EExtension,
                    TTroncal, NNumeroMarcado, DDuracion, PCodigoPersonal, mFechaInicialLlamada, jHoraInicialLlamada,
                    lTipoLlamada, RTraficoInternoExterno, ClaseLlamada, Destino, CentroDeCosto, DuracionLlamada, DuracionLlamadaAproximada, DurMinima, PlanTarifa,
                    NumeroTarifa, NumeFolio, ClaseExtension, NombreExtension, DuracionRango1, DuracionRango2, DuracionRango3, DuracionRango4, DuracionRango5,
                    ValorRango1, ValorRango2, ValorRango3, ValorRango4, ValorRango5, CargoFijo, RecargoServicioPorcentaje, RecargoServicioValor, 
                    Base_IVA, IVA_Incluye_Recago, ValorIVA, ValorTotal, TramaCompleta, Operador, DigitosMinimos, NombreTarifa, Tarifa1, Tarifa2,
                    Tarifa3, Tarifa4, Tarifa5, ValorLlamadaTarifa, PorcentajeIVA, Errores)
                    values (?FF, ?HF, ?EE, ?TT, ?NN, ?DD, ?CP, ?mF, ?jH, ?lT, ?RT, ?Clsl, ?Des, ?CT, ?DLL, ?DLLA, ?DurM, ?PT, ?NuT, ?NuF, ?ClEx, ?NomEx,
                    ?DR1, ?DR2, ?DR3, ?DR4, ?DR5, ?VR1, ?VR2, ?VR3, ?VR4, ?VR5, ?CAF, ?RSP, ?RSV, ?BIV, ?IVR, ?VIV, ?VT, ?TrC, ?Oper, ?DigitM, ?NombT,
                    ?Tar1, ?Tar2, ?Tar3, ?Tar4, ?Tar5, ?VlT, ?PIVA, ?errores)";
                    comando = new MySqlCommand(query, Conexion);
                    comando.Parameters.AddWithValue("?FF", F_Fecha_Final_Lamada);
                    comando.Parameters.AddWithValue("?HF", H_Hora_Final_Llamada);
                    comando.Parameters.AddWithValue("?EE", E_Extension);
                    comando.Parameters.AddWithValue("?TT", T_Troncal);
                    comando.Parameters.AddWithValue("?NN", N_Numero_Marcado.Replace(" ", ""));
                    comando.Parameters.AddWithValue("?DD", D_Duracion);
                    comando.Parameters.AddWithValue("?CP", P_Codigo_Personal);
                    comando.Parameters.AddWithValue("?mF", m_Fecha_Inicial_Llamada);
                    comando.Parameters.AddWithValue("?jH", j_Hora_Inicial_Llamada);
                    comando.Parameters.AddWithValue("?lT", l_Tipo_Llamada);
                    comando.Parameters.AddWithValue("?RT", R_Trafico_Interno_Externo);
                    comando.Parameters.AddWithValue("?Clsl", ClaseLlamada);
                    comando.Parameters.AddWithValue("?Des", Destino);
                    comando.Parameters.AddWithValue("?CT", CentroDeCosto);
                    comando.Parameters.AddWithValue("?DLL", DuracionLlamada);
                    comando.Parameters.AddWithValue("?DLLA", DuracionAprox);
                    comando.Parameters.AddWithValue("?DurM", DurMinima);
                    comando.Parameters.AddWithValue("?PT", PlanTarifa);
                    comando.Parameters.AddWithValue("?NuT", NumeroTarifa);
                    comando.Parameters.AddWithValue("?NuF", Folio);
                    comando.Parameters.AddWithValue("?ClEx", ClaseExtension);
                    comando.Parameters.AddWithValue("?NomEx", NombreExtension);
                    comando.Parameters.AddWithValue("?DR1", DurRang1);
                    comando.Parameters.AddWithValue("?DR2", DurRang2);
                    comando.Parameters.AddWithValue("?DR3", DurRang3);
                    comando.Parameters.AddWithValue("?DR4", DurRang4);
                    comando.Parameters.AddWithValue("?DR5", DurRang5);
                    comando.Parameters.AddWithValue("?VR1", ValorRango1);
                    comando.Parameters.AddWithValue("?VR2", ValorRango2);
                    comando.Parameters.AddWithValue("?VR3", ValorRango3);
                    comando.Parameters.AddWithValue("?VR4", ValorRango4);
                    comando.Parameters.AddWithValue("?VR5", ValorRango5);
                    comando.Parameters.AddWithValue("?CAF", CargoFijo);
                    comando.Parameters.AddWithValue("?RSP", RecargoServicioPorcentaje);
                    comando.Parameters.AddWithValue("?RSV", RecargoServicioValor);
                    comando.Parameters.AddWithValue("?BIV", BaseIVA);
                    comando.Parameters.AddWithValue("?IVR", Iva);
                    comando.Parameters.AddWithValue("?VIV", ValorIVA);
                    comando.Parameters.AddWithValue("?VT", ValorTotal);
                    comando.Parameters.AddWithValue("?TrC", CadenaCompleta);
                    comando.Parameters.AddWithValue("?Oper", Operador);
                    comando.Parameters.AddWithValue("?DigitM", DigitosMinimos);
                    comando.Parameters.AddWithValue("?NombT", NombreTarifa);
                    comando.Parameters.AddWithValue("?Tar1", Tarifa1);
                    comando.Parameters.AddWithValue("?Tar2", Tarifa2);
                    comando.Parameters.AddWithValue("?Tar3", Tarifa3);
                    comando.Parameters.AddWithValue("?Tar4", Tarifa4);
                    comando.Parameters.AddWithValue("?Tar5", Tarifa5);
                    comando.Parameters.AddWithValue("?VlT", ValorLlamadaTarifa);
                    comando.Parameters.AddWithValue("?PIVA", PorcentajeIVA);
                    comando.Parameters.AddWithValue("?errores", Errores);

                    comando.ExecuteNonQuery();

                    query = @"insert into llamadas_telefonicas (FFechaFinalLlamada, HHoraFinalLlamada, EExtension,
                    TTroncal, NNumeroMarcado, DDuracion, PCodigoPersonal, mFechaInicialLlamada, jHoraInicialLlamada,
                    lTipoLlamada, RTraficoInternoExterno, ClaseLlamada, Destino, CentroDeCosto, DuracionLlamada, DuracionLlamadaAproximada, DurMinima, PlanTarifa,
                    NumeroTarifa, NumeFolio, ClaseExtension, NombreExtension, DuracionRango1, DuracionRango2, DuracionRango3, DuracionRango4, DuracionRango5,
                    ValorRango1, ValorRango2, ValorRango3, ValorRango4, ValorRango5, CargoFijo, RecargoServicioPorcentaje, RecargoServicioValor, 
                    Base_IVA, IVA_Incluye_Recago, ValorIVA, ValorTotal, TramaCompleta, Operador, DigitosMinimos, NombreTarifa, Tarifa1, Tarifa2,
                    Tarifa3, Tarifa4, Tarifa5, ValorLlamadaTarifa, PorcentajeIVA, Errores)
                    values (?FF, ?HF, ?EE, ?TT, ?NN, ?DD, ?CP, ?mF, ?jH, ?lT, ?RT, ?Clsl, ?Des, ?CT, ?DLL, ?DLLA, ?DurM, ?PT, ?NuT, ?NuF, ?ClEx, ?NomEx,
                    ?DR1, ?DR2, ?DR3, ?DR4, ?DR5, ?VR1, ?VR2, ?VR3, ?VR4, ?VR5, ?CAF, ?RSP, ?RSV, ?BIV, ?IVR, ?VIV, ?VT, ?TrC, ?Oper, ?DigitM, ?NombT,
                    ?Tar1, ?Tar2, ?Tar3, ?Tar4, ?Tar5, ?VlT, ?PIVA, ?errores)";
                    comando = new MySqlCommand(query, Conexion);
                    comando.Parameters.AddWithValue("?FF", F_Fecha_Final_Lamada);
                    comando.Parameters.AddWithValue("?HF", H_Hora_Final_Llamada);
                    comando.Parameters.AddWithValue("?EE", E_Extension);
                    comando.Parameters.AddWithValue("?TT", T_Troncal);
                    comando.Parameters.AddWithValue("?NN", N_Numero_Marcado.Replace(" ", ""));
                    comando.Parameters.AddWithValue("?DD", D_Duracion);
                    comando.Parameters.AddWithValue("?CP", P_Codigo_Personal);
                    comando.Parameters.AddWithValue("?mF", m_Fecha_Inicial_Llamada);
                    comando.Parameters.AddWithValue("?jH", j_Hora_Inicial_Llamada);
                    comando.Parameters.AddWithValue("?lT", l_Tipo_Llamada);
                    comando.Parameters.AddWithValue("?RT", R_Trafico_Interno_Externo);
                    comando.Parameters.AddWithValue("?Clsl", ClaseLlamada);
                    comando.Parameters.AddWithValue("?Des", Destino);
                    comando.Parameters.AddWithValue("?CT", CentroDeCosto);
                    comando.Parameters.AddWithValue("?DLL", DuracionLlamada);
                    comando.Parameters.AddWithValue("?DLLA", DuracionAprox);
                    comando.Parameters.AddWithValue("?DurM", DurMinima);
                    comando.Parameters.AddWithValue("?PT", PlanTarifa);
                    comando.Parameters.AddWithValue("?NuT", NumeroTarifa);
                    comando.Parameters.AddWithValue("?NuF", Folio);
                    comando.Parameters.AddWithValue("?ClEx", ClaseExtension);
                    comando.Parameters.AddWithValue("?NomEx", NombreExtension);
                    comando.Parameters.AddWithValue("?DR1", DurRang1);
                    comando.Parameters.AddWithValue("?DR2", DurRang2);
                    comando.Parameters.AddWithValue("?DR3", DurRang3);
                    comando.Parameters.AddWithValue("?DR4", DurRang4);
                    comando.Parameters.AddWithValue("?DR5", DurRang5);
                    comando.Parameters.AddWithValue("?VR1", ValorRango1);
                    comando.Parameters.AddWithValue("?VR2", ValorRango2);
                    comando.Parameters.AddWithValue("?VR3", ValorRango3);
                    comando.Parameters.AddWithValue("?VR4", ValorRango4);
                    comando.Parameters.AddWithValue("?VR5", ValorRango5);
                    comando.Parameters.AddWithValue("?CAF", CargoFijo);
                    comando.Parameters.AddWithValue("?RSP", RecargoServicioPorcentaje);
                    comando.Parameters.AddWithValue("?RSV", RecargoServicioValor);
                    comando.Parameters.AddWithValue("?BIV", BaseIVA);
                    comando.Parameters.AddWithValue("?IVR", Iva);
                    comando.Parameters.AddWithValue("?VIV", ValorIVA);
                    comando.Parameters.AddWithValue("?VT", ValorTotal);
                    comando.Parameters.AddWithValue("?TrC", CadenaCompleta);
                    comando.Parameters.AddWithValue("?Oper", Operador);
                    comando.Parameters.AddWithValue("?DigitM", DigitosMinimos);
                    comando.Parameters.AddWithValue("?NombT", NombreTarifa);
                    comando.Parameters.AddWithValue("?Tar1", Tarifa1);
                    comando.Parameters.AddWithValue("?Tar2", Tarifa2);
                    comando.Parameters.AddWithValue("?Tar3", Tarifa3);
                    comando.Parameters.AddWithValue("?Tar4", Tarifa4);
                    comando.Parameters.AddWithValue("?Tar5", Tarifa5);
                    comando.Parameters.AddWithValue("?VlT", ValorLlamadaTarifa);
                    comando.Parameters.AddWithValue("?PIVA", PorcentajeIVA);
                    comando.Parameters.AddWithValue("?errores", Errores);

                    comando.ExecuteNonQuery();

                    Conexion.Close();
                }

                #region Envio Interface

                if (InterEnv == true && InterfaceConfig != null)
                {
                    if(InterfaceConfig[0].Equals("Hotel Plus"))
                    {
                        In1Filtro = false;
                        if (ClaseExtension.Equals("A") && InterfaceConfig[20].Equals("si")) { In1Filtro = true; }
                        else if (ClaseExtension.Equals("H") && InterfaceConfig[21].Equals("si")) { In1Filtro = true; }
                        else if (ClaseExtension.Equals("S") && InterfaceConfig[22].Equals("si")) { In1Filtro = true; }
                        else if (ClaseExtension.Equals("V") && InterfaceConfig[23].Equals("si")) { In1Filtro = true; }

                        if (In1Filtro == true)
                        {
                            In1Filtro = false;

                            if (ClaseLlamada.Equals("CEL") && InterfaceConfig[24].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("DDI") && InterfaceConfig[25].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("DDN") && InterfaceConfig[26].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("ENT") && InterfaceConfig[27].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("EXC") && InterfaceConfig[28].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("INT") && InterfaceConfig[29].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("INV") && InterfaceConfig[30].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("ITH") && InterfaceConfig[31].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("LOC") && InterfaceConfig[32].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("SAT") && InterfaceConfig[33].Equals("si")) { In1Filtro = true; }
                            else if (ClaseLlamada.Equals("TOL") && InterfaceConfig[34].Equals("si")) { In1Filtro = true; }
                            
                            if (In1Filtro == true)
                            {
                                In1Calculal = 0;
                                if (!ValorRango1.Equals("-")) { In1Calculal += Convert.ToInt32(ValorRango1); }
                                if (!ValorRango2.Equals("-")) { In1Calculal += Convert.ToInt32(ValorRango2); }
                                if (!ValorRango3.Equals("-")) { In1Calculal += Convert.ToInt32(ValorRango3); }
                                if (!ValorRango4.Equals("-")) { In1Calculal += Convert.ToInt32(ValorRango4); }
                                if (!ValorRango5.Equals("-")) { In1Calculal += Convert.ToInt32(ValorRango5); }
                                if (!CargoFijo.Equals("-")) { In1Calculal += Convert.ToInt32(CargoFijo); }

                                if (!F_Fecha_Final_Lamada.Equals("-"))
                                {
                                    In1Procesa(Folio, E_Extension, ValorLlamadaTarifa, ValorTotal, N_Numero_Marcado.Replace(" ", ""), ClaseLlamada, T_Troncal, In1Calculal.ToString(), RecargoServicioValor, ValorIVA, "--", F_Fecha_Final_Lamada, H_Hora_Final_Llamada, DuracionAprox, FormatoFechaEnv, FormatoHoraEnv, Destino, CadenaCompleta);
                                }
                                else if (!m_Fecha_Inicial_Llamada.Equals("-"))
                                {
                                    In1Procesa(Folio, E_Extension, ValorLlamadaTarifa, ValorTotal, N_Numero_Marcado.Replace(" ", ""), ClaseLlamada, T_Troncal, In1Calculal.ToString(), RecargoServicioValor, ValorIVA, "--", m_Fecha_Inicial_Llamada, j_Hora_Inicial_Llamada, DuracionAprox, FormatoFechaEnv, FormatoHoraEnv, Destino, CadenaCompleta);
                                }
                            }
                        }
                    }
                }

                #endregion

                Reset();
                CreaArchivos = false;
            }
            catch (Exception ex)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en la tabla, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en la tabla, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al cargar los datos de la cadena en la tabla, más información:  \nl\nl" + ex.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                CreaArchivos = true;
                EnvioCorreo(Subject, Body);
            }
        }
        int Posc = 0;
        string PosAño;
        string PosDia;
        string PosMes;
        public void LeePos(string Formato)
        {
            PosAño = "";
            PosDia = "";
            PosMes = "";
            Posc = 0;
            foreach (char s in Formato.ToLower())
            {
                if (s.Equals('y'))
                {
                    PosAño += Posc.ToString();
                }
                else if (s.Equals('m'))
                {
                    PosMes += Posc.ToString();
                }
                else if (s.Equals('d'))
                {
                    PosDia += Posc.ToString();
                }
                Posc++;
            }
            if (PosAño.Length >= 2)
            {
                PosAño = PosAño[0] + "-" + PosAño.Length;
            }
            if (PosMes.Length >= 2)
            {
                PosMes = PosMes[0] + "-" + PosMes.Length;
            }
            if (PosDia.Length >= 2)
            {
                PosDia = PosDia[0] + "-" + PosDia.Length;
            }
        }

        bool Tiene = false;
        public bool TieneAño(string Formato)
        {
            foreach (char s in Formato.ToLower())
            {
                if (s.Equals('y'))
                {
                    Tiene = true;
                }
            }
            if (Tiene == true)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }

        public void Reset()
        {
            F_Fecha_Final_Lamada = "-";
            H_Hora_Final_Llamada = "-";
            E_Extension = "-";
            T_Troncal = "-";
            N_Numero_Marcado = "-";
            D_Duracion = "-";
            P_Codigo_Personal = "-";
            m_Fecha_Inicial_Llamada = "-";
            j_Hora_Inicial_Llamada = "-";
            l_Tipo_Llamada = "-";
            R_Trafico_Interno_Externo = "-";
            ClaseLlamada = "-";
            Destino = "-";
            CentroDeCosto = "-";
            DuracionLlamada = "-";
            DuracionAprox = "-";
            DurMinima = "-";
            PlanTarifa = "-";
            NumeroTarifa = "-";
            DurRang1 = "-";
            DurRang2 = "-";
            DurRang3 = "-";
            DurRang4 = "-";
            DurRang5 = "-";
            ValorRango1 = "-";
            ValorRango2 = "-";
            ValorRango3 = "-";
            ValorRango4 = "-";
            ValorRango5 = "-";
            ValorTotal = "-";
            CargoFijo = "-";
            RecargoServicioPorcentaje = "-";
            RecargoServicioValor = "-";
            BaseIVA = "-";
            Iva = "-";
            ValorIVA = "-";
            Folio = "-";
            ClaseExtension = "-";
            NombreExtension = "-";
            CadenaCompleta = "-";
            Operador = "-";
            DigitosMinimos = "-";
            NombreTarifa = "-";
            Tarifa1 = "-";
            Tarifa2 = "-";
            Tarifa3 = "-";
            Tarifa4 = "-";
            Tarifa5 = "-";
            ValorLlamadaTarifa = "-";
            PorcentajeIVA = "-";
            Errores = "-";
        }

        #endregion
        
        #region ProcesamientoCar

        public void ProcesamientoCar(string Tipo, string Cadena)
        {
            CreaArchivos = true;

            #region Llamada entrante
            if (Tipo.Equals("Llamada entrante"))
            {
                CadenaCompleta = Cadena;
                try
                {
                    foreach (string s in ConfigE)
                    {
                        if (s[0].Equals('E'))
                        {
                            E_Extension = Cadena.Substring(Convert.ToInt32(s.Split('-')[1].Split(':')[0]), Convert.ToInt32(s.Split('-')[1].Split(':')[1]));
                            E_Extension = E_Extension.Replace(" ", "");
                        }
                        if (s[0].Equals('P'))
                        {
                            P_Codigo_Personal = Cadena.Substring(Convert.ToInt32(s.Split('-')[1].Split(':')[0]), Convert.ToInt32(s.Split('-')[1].Split(':')[1]));
                        }
                    }
                    try
                    {
                        using (Conexion = new MySqlConnection(Config[25]))
                        {
                            Conexion.Open();
                            query = "Select * From extensiones where Nume_Extension = ?NExt";
                            comando = new MySqlCommand(query, Conexion);
                            comando.Parameters.AddWithValue("?NExt", E_Extension);

                            lee = comando.ExecuteReader();
                            lee.Read();
                            ClaseExtension = lee["Clas_Extension"].ToString();
                            CentroDeCosto = lee["Codi_Centro"].ToString();
                            Folio = lee["Nume_Folio"].ToString();
                            NombreExtension = lee["Nomb_Extension"].ToString();
                            lee.Close();

                            query = "Select * From clase_extensiones where Clas_Extension = ?Cext";
                            comando = new MySqlCommand(query, Conexion);
                            comando.Parameters.AddWithValue("?Cext", ClaseExtension);

                            lee = comando.ExecuteReader();
                            lee.Read();
                            PlanTarifa = lee["Plan_Tarifario"].ToString();
                            lee.Close();
                            Conexion.Close();
                        }

                        try
                        {
                            using (Conexion = new MySqlConnection(Config[25]))
                            {
                                Conexion.Open();
                                query = "Select * From clase_llamadas where Clase_Llamada = 'ENT'";
                                comando = new MySqlCommand(query, Conexion);

                                lee = comando.ExecuteReader();
                                lee.Read();
                                ClaseLlamada = lee["Clase_Llamada"].ToString();
                                Destino = lee["Nombre_Clase_Llamada"].ToString();
                                NumeroTarifa = lee["tarifa"].ToString();

                                lee.Close();

                                Tarifa(Cadena, false);

                                Conexion.Close();
                            }
                            
                        }
                        catch(Exception e)
                        {
                            FechaYa = DateTime.Now;
                            horas = FechaYa.ToString();
                            if (ErrorSQLoCad() == true)
                            {
                                ErrorSql(e);
                            }
                            else
                            {
                                ErroresSaliDatosPuerto1.Add(horas + " No se ha encontrado el registro ENT en clase_llamadas, ver correo paea más información");
                                LogErrores(horas + " No se ha encontrado el registro ENT en clase_llamadas, Revisar correo para más información");
                                pictureBox4.BackColor = Color.Red;
                                Subject = "Error del servidor";
                                Body = horas + " Puerto 1: No se ha encontrado el registro ENT en clase_llamadas, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en el log de cadenas fallidas";
                                EnvioCorreo(Subject, Body);
                                CadenasFallidas(CodigoError("No se ha encontrado el registro ENT en clase_llamadas"), Cadena, horas, e.ToString());
                                CreaArchivos = false;
                            }
                        }
                        
                    }
                    catch (Exception e)
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        if(ErrorSQLoCad() == true)
                        {
                            ErrorSql(e);
                        }
                        else
                        {
                            ErroresSaliDatosPuerto1.Add(horas + " No se ha encontrado la extension en la tabla extensiones o la clase de extension en la tabla Clas_Extension, ver correo paea más información");
                            LogErrores(horas + "No se ha encontrado la extension en la tabla extensiones o la clase de extension en la tabla Clas_Extension, Revisar correo para más información");
                            pictureBox4.BackColor = Color.Red;
                            Subject = "Error del servidor";
                            Body = horas + " Puerto 1: No se ha encontrado la extension en la tabla extensiones o la clase de extension en la tabla Clas_Extension, más información:  \nl\nl" + e.ToString() + "\n\n La extension obtenida fue: |" + E_Extension + "|" + "\n\n Cadena: " + Cadena;
                            EnvioCorreo(Subject, Body);
                            CadenasFallidas(CodigoError("No se ha encontrado la extension en la tabla extensiones o la clase de extension en la tabla Clas_Extension"), Cadena, horas, e.ToString());
                            CreaArchivos = false;
                        }

                    }
                }
                catch (Exception e)
                {
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al obtener la extension o el codigo personal de la cadena, Revisar correo para más información");
                    LogErrores(horas + " Ha ocurrido un error al obtener la extensiono el codigo personal de la cadena, Revisar correo para más información");
                    pictureBox4.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: Ha ocurrido un error al obtener la extension o el codigo personal de la cadena, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                    EnvioCorreo(Subject, Body);
                    CadenasFallidas(CodigoError("Ha ocurrido un error al obtener la extension o el codigo personal de la cadena"), Cadena, horas, e.ToString());
                    CreaArchivos = false;
                }
            }

            #endregion

            #region Llamada saliente

            else if (Tipo.Equals("Llamada saliente"))
            {
                CadenaCompleta = Cadena;
                try
                {
                    foreach (string s in ConfigS)
                    {
                        if (s[0].Equals('T'))
                        {
                            CTroncal = s.Split('-')[1];
                        }
                        if (s[0].Equals('P'))
                        {
                            P_Codigo_Personal = Cadena.Substring(Convert.ToInt32(s.Split('-')[1].Split(':')[0]), Convert.ToInt32(s.Split('-')[1].Split(':')[1]));
                        }
                    }
                    CTroncal = Cadena.Substring(Convert.ToInt32(CTroncal.Split(':')[0]), Convert.ToInt32(CTroncal.Split(':')[1]));
                    using (Conexion = new MySqlConnection(Config[25]))
                    {
                        Conexion.Open();
                        query = "Select * From troncales where Line_Troncal = ?Ctroncal";

                        comando = new MySqlCommand(query, Conexion);
                        comando.Parameters.AddWithValue("?Ctroncal", CTroncal);

                        lee = comando.ExecuteReader();
                        lee.Read();
                        Operador = lee["Operador"].ToString();
                        AccesoTroncal = lee["Nume_Acceso_Troncal"].ToString();
                        lee.Close();

                        NumMar = "";
                        Extn = "";

                        foreach (string s in ConfigS)
                        {
                            if (s[0].Equals('N'))
                            {
                                NumMar = s.Split('-')[1];
                            }
                            if (s[0].Equals('E'))
                            {
                                Extn = s.Split('-')[1];
                            }
                        }
                        
                        NumMar = Cadena.Substring(Convert.ToInt32(NumMar.Split(':')[0]), Convert.ToInt32(NumMar.Split(':')[1])).Replace(" ", "");
                        Extn = Cadena.Substring(Convert.ToInt32(Extn.Split(':')[0]), Convert.ToInt32(Extn.Split(':')[1]));

                        if (!string.IsNullOrEmpty(NumMar) && !string.IsNullOrEmpty(Extn))
                        {
                            try
                            {
                                if (!string.IsNullOrEmpty(AccesoTroncal))
                                {
                                    if (NumMar.Substring(0, AccesoTroncal.Length).Equals(AccesoTroncal))
                                    {
                                        NumMar = NumMar.Remove(0, AccesoTroncal.Length);
                                    }
                                }
                                
                                query = "Select * From numeros_importantes";
                                comando = new MySqlCommand(query, Conexion);

                                lee = comando.ExecuteReader();
                                Excluido = false;

                                while (lee.Read())
                                {
                                    if (lee["Nume_Marcado"].ToString().Equals(NumMar))
                                    {
                                        Excluido = true;
                                    }
                                }
                                lee.Close();
                                
                                if (Excluido == false)
                                {
                                    try
                                    {
                                        query = "Select * From extensiones where Nume_Extension = ?Ext";
                                        comando = new MySqlCommand(query, Conexion);
                                        comando.Parameters.AddWithValue("?Ext", Extn);

                                        lee = comando.ExecuteReader();
                                        lee.Read();
                                        ClaseExtension = lee["Clas_Extension"].ToString();
                                        CentroDeCosto = lee["Codi_Centro"].ToString();
                                        Folio = lee["Nume_Folio"].ToString();
                                        NombreExtension = lee["Nomb_Extension"].ToString();
                                        lee.Close();

                                        try
                                        {
                                            query = "Select * From clase_extensiones where Clas_Extension = ?CExt";
                                            comando = new MySqlCommand(query, Conexion);
                                            comando.Parameters.AddWithValue("?CExt", ClaseExtension);

                                            lee = comando.ExecuteReader();
                                            lee.Read();
                                            PlanTarifa = lee["Plan_Tarifario"].ToString();
                                            lee.Close();

                                            try
                                            {
                                                if (!string.IsNullOrEmpty(PlanTarifa))
                                                {
                                                    SoloUno = 0;
                                                    if (Operador.Equals("BG"))
                                                    {
                                                        query = "Select * From indicativosbg";
                                                        comando = new MySqlCommand(query, Conexion);

                                                        lee = comando.ExecuteReader();
                                                        while (lee.Read())
                                                        {
                                                            if (lee["Indicativo"].ToString().Length <= NumMar.Length)
                                                            {
                                                                Indic = NumMar.Substring(0, lee["Indicativo"].ToString().Length);
                                                                if (Indic.Equals(lee["Indicativo"].ToString()))
                                                                {
                                                                    SoloUno++;
                                                                    DigitosMinimos =lee["Digitos_Minimos"].ToString();
                                                                    NumeroTarifa = lee["Tarifa"].ToString();
                                                                    ClaseLlamada = lee["Clase_Llamada"].ToString();
                                                                    Destino = lee["Destino"].ToString();
                                                                }
                                                            }
                                                        }
                                                        lee.Close();
                                                    }
                                                    else if (Operador.Equals("CF"))
                                                    {
                                                        query = "Select * From indicativoscf";
                                                        comando = new MySqlCommand(query, Conexion);

                                                        lee = comando.ExecuteReader();
                                                        while (lee.Read())
                                                        {
                                                            if (lee["Indicativo"].ToString().Length <= NumMar.Length)
                                                            {
                                                                Indic = NumMar.Substring(0, lee["Indicativo"].ToString().Length);
                                                                if (Indic.Equals(lee["Indicativo"].ToString()))
                                                                {
                                                                    SoloUno++;
                                                                    DigitosMinimos = lee["Digitos_Minimos"].ToString();
                                                                    NumeroTarifa = lee["Tarifa"].ToString();
                                                                    ClaseLlamada = lee["Clase_Llamada"].ToString();
                                                                    Destino = lee["Destino"].ToString();
                                                                }
                                                            }
                                                        }
                                                        lee.Close();
                                                    }
                                                    else if (Operador.Equals("IP"))
                                                    {
                                                        query = "Select * From indicativosip";
                                                        comando = new MySqlCommand(query, Conexion);

                                                        lee = comando.ExecuteReader();
                                                        while (lee.Read())
                                                        {
                                                            if (lee["Indicativo"].ToString().Length <= NumMar.Length)
                                                            {
                                                                Indic = NumMar.Substring(0, lee["Indicativo"].ToString().Length);
                                                                if (Indic.Equals(lee["Indicativo"].ToString()))
                                                                {
                                                                    SoloUno++;
                                                                    DigitosMinimos = lee["Digitos_Minimos"].ToString();
                                                                    NumeroTarifa = lee["Tarifa"].ToString();
                                                                    ClaseLlamada = lee["Clase_Llamada"].ToString();
                                                                    Destino = lee["Destino"].ToString();
                                                                }
                                                            }
                                                        }
                                                        lee.Close();
                                                    }

                                                    if (!DigitosMinimos.Equals("-") && !NumeroTarifa.Equals("-") && SoloUno == 1)
                                                    {
                                                        if (NumMar.Length >= Convert.ToInt32(DigitosMinimos))
                                                        {
                                                            Tarifa(Cadena, true);
                                                        }
                                                        else
                                                        {
                                                            FechaYa = DateTime.Now;
                                                            horas = FechaYa.ToString();
                                                            ErroresSaliDatosPuerto1.Add(horas + " El número marcado no cumple con la cantidad mínima de dígitos, Revisar correo para más información");
                                                            LogErrores(horas + "  El número marcado no cumple con la cantidad mínima de dígitos, Revisar correo para más información");
                                                            Subject = "Error del servidor";
                                                            Body = horas + " Puerto 1: El número marcado no cumple con la cantidad mínima de dígitos, más información:  \nl\nl" + "\n\nCadena: " + Cadena + "La cantidad de números obtenidos fue: " + NumMar.Length;
                                                            CadenasFallidas(CodigoError("El número marcado no cumple con la cantidad mínima de dígitos"), Cadena, horas, "El número marcado no cumple con la cantidad mínima de dígitos");
                                                            EnvioCorreo(Subject, Body);
                                                            CreaArchivos = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        FechaYa = DateTime.Now;
                                                        horas = FechaYa.ToString();
                                                        ErroresSaliDatosPuerto1.Add(horas + " No se ha podido extraer la cantidad de digitos minima o el numero de tarifa, Revisar correo para más información");
                                                        LogErrores(horas + "  No se ha podido extraer la cantidad de digitos minima o el numero de tarifa, Revisar correo para más información");
                                                        pictureBox4.BackColor = Color.Red;
                                                        Subject = "Error del servidor";
                                                        Body = horas + " Puerto 1: No se ha podido extraer la cantidad de digitos minima o el numero de tarifa, más información:  \nl\nl" + "\n\nCadena: " + Cadena;
                                                        EnvioCorreo(Subject, Body);
                                                        CadenasFallidas(CodigoError("No se ha podido extraer la cantidad de digitos minima o el numero de tarifa"), Cadena, horas, "No se ha podido extraer la cantidad de digitos minima o el numero de tarifa");
                                                        CreaArchivos = false;
                                                    }
                                                }
                                                else
                                                {
                                                    FechaYa = DateTime.Now;
                                                    horas = FechaYa.ToString();
                                                    ErroresSaliDatosPuerto1.Add(horas + " No se ha podido extraer el plan tarifario, Revisar correo para más información");
                                                    LogErrores(horas + "  No se ha podido extraer el plan tarifario, Revisar correo para más información");
                                                    pictureBox4.BackColor = Color.Red;
                                                    Subject = "Error del servidor";
                                                    Body = horas + " Puerto 1: No se ha podido extraer el plan tarifario, más información:  \nl\nl" + "\n\nCadena: " + Cadena;
                                                    EnvioCorreo(Subject, Body);
                                                    CadenasFallidas(CodigoError("No se ha podido extraer el plan tarifario"), Cadena, horas, "No se ha podido extraer el plan tarifario");
                                                    CreaArchivos = false;
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                FechaYa = DateTime.Now;
                                                horas = FechaYa.ToString();
                                                if (ErrorSQLoCad() == true)
                                                {
                                                    ErrorSql(e);
                                                    CreaArchivos = true;
                                                }
                                                else
                                                {
                                                    ErroresSaliDatosPuerto1.Add(horas + " Ocurrió un problema al detectar o leer el tipo de indicativo, Revisar correo para más información");
                                                    LogErrores(horas + "  Ocurrió un problema al detectar o leer el tipo de indicativo, Revisar correo para más información");
                                                    pictureBox4.BackColor = Color.Red;
                                                    Subject = "Error del servidor";
                                                    Body = horas + " Puerto 1: Ocurrió un problema al detectar o leer el tipo de indicativo, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena;
                                                    EnvioCorreo(Subject, Body);
                                                    CadenasFallidas(CodigoError("Ocurrió un problema al detectar o leer el tipo de indicativo"), Cadena, horas, e.ToString());
                                                    CreaArchivos = false;
                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {
                                            FechaYa = DateTime.Now;
                                            horas = FechaYa.ToString();
                                            if (ErrorSQLoCad() == true)
                                            {
                                                ErrorSql(e);
                                                CreaArchivos = true;
                                            }
                                            else
                                            {
                                                ErroresSaliDatosPuerto1.Add(horas + " Ocurrió un problema al buscar la clase de extensión en la tabla Clas_Extension, Revisar correo para más información");
                                                LogErrores(horas + "  Ocurrió un problema al buscar la clase de extensión en la tabla Clas_Extension, Revisar correo para más información");
                                                pictureBox4.BackColor = Color.Red;
                                                Subject = "Error del servidor";
                                                Body = horas + " Puerto 1: Ocurrió un problema al buscar la clase de extensión en la tabla Clas_Extension, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena + "\n\n La clase de extensión buscada fue: " + "|" + ClaseExtension + "|";
                                                EnvioCorreo(Subject, Body);
                                                CadenasFallidas(CodigoError("Ocurrió un problema al buscar la clase de extensión en la tabla Clas_Extension"), Cadena, horas, e.ToString());
                                                CreaArchivos = false;
                                            }
                                        }

                                    }
                                    catch (Exception e)
                                    {
                                        FechaYa = DateTime.Now;
                                        horas = FechaYa.ToString();
                                        if (ErrorSQLoCad() == true)
                                        {
                                            ErrorSql(e);
                                            CreaArchivos = true;
                                        }
                                        else
                                        {
                                            ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones, Revisar correo para más información");
                                            LogErrores(horas + "  Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones, Revisar correo para más información");
                                            pictureBox4.BackColor = Color.Red;
                                            Subject = "Error del servidor";
                                            Body = horas + " Puerto 1: Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena + "\n\n Numero de extensión buscado: " + "|" + Extn + "|";
                                            EnvioCorreo(Subject, Body);
                                            CadenasFallidas(CodigoError("Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones"), Cadena, horas, e.ToString());
                                            CreaArchivos = false;
                                        }
                                    }
                                }
                                else
                                {
                                    saleExcluido = false;
                                    excluido = true;
                                    query = "Select * From numeros_importantes";
                                    comando = new MySqlCommand(query, Conexion);

                                    lee = comando.ExecuteReader();
                                    
                                    while (saleExcluido == false && lee.Read())
                                    {
                                        if (lee["Nume_Marcado"].ToString().Equals(NumMar))
                                        {
                                            if (lee["Clas_Llamada"].ToString().Equals("EXC"))
                                            {
                                                saleExcluido = true;
                                                excluido = true;
                                                CreaArchivos = false;
                                                SubeExpo = false;
                                                CadenasFallidas(CodigoError("Numero excluido"), Cadena, horas, "Numero excluido");
                                            }
                                            else
                                            {
                                                saleExcluido = true;
                                                NumeroTarifa = lee["Codi_Tarifa"].ToString();
                                                PlanTarifa = lee["Plan_Tarifario"].ToString();
                                                Destino = lee["Nomb_Destino"].ToString();
                                                ClaseLlamada = lee["Clas_Llamada"].ToString();
                                                excluido = false;
                                            }
                                        }
                                    }
                                    lee.Close();

                                    if (excluido == false)
                                    {
                                        Tarifa(Cadena, true);
                                    }
                                }

                            }
                            catch (Exception e)
                            {
                                FechaYa = DateTime.Now;
                                horas = FechaYa.ToString();
                                if (ErrorSQLoCad() == true)
                                {
                                    ErrorSql(e);
                                    CreaArchivos = true;
                                }
                                else
                                {
                                    ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al determinar si es un numero importante de la tabla numeros_importantes, Revisar correo para más información");
                                    LogErrores(horas + "  Ha ocurrido un error al determinar si es un numero importante de la tabla numeros_importantes, Revisar correo para más información");
                                    pictureBox4.BackColor = Color.Red;
                                    Subject = "Error del servidor";
                                    Body = horas + " Puerto 1: Ha ocurrido un error al determinar si es un numero importante de la tabla numeros_importantes, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena + "\n\nNumero: " + "|" + NumMar + "|";
                                    EnvioCorreo(Subject, Body);
                                    CadenasFallidas(CodigoError("Ha ocurrido un error al determinar si es un numero importante de la tabla numeros_importantes"), Cadena, horas, e.ToString());
                                    CreaArchivos = false;
                                }

                            }
                        }
                        else
                        {
                            FechaYa = DateTime.Now;
                            horas = FechaYa.ToString();
                            ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al extraer la extensión o extraer el número marcado, Revisar correo para más información");
                            LogErrores(horas + "  Ha ocurrido un error al extraer la extensión o extraer el número marcado, Revisar correo para más información");
                            pictureBox4.BackColor = Color.Red;
                            Subject = "Error del servidor";
                            Body = horas + " Puerto 1: Ha ocurrido un error al extraer la extensión o extraer el número marcado "+ "\n\nCadena: " + Cadena + "\n\nNumero marcado obtenido: " + "|" + NumMar + "|" + " Extension obtenida: " + "|" + Extn + "|";
                            EnvioCorreo(Subject, Body);
                            CadenasFallidas(CodigoError("Ha ocurrido un error al extraer la extensión o extraer el número marcado"), Cadena, horas, "Ha ocurrido un error al extraer la extensión o extraer el número marcado");
                            CreaArchivos = false;
                        }
                        Conexion.Close();
                    }
                }
                catch (Exception e)
                {
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    if (ErrorSQLoCad() == true)
                    {
                        ErrorSql(e);
                        CreaArchivos = true;
                    }
                    else
                    {
                        ErroresSaliDatosPuerto1.Add(horas + " No se ha encontrado la troncal en la tabla troncales, Revisar correo para más información");
                        LogErrores(horas + " No se ha encontrado la troncal en la tabla troncales, Revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: No se ha encontrado la troncal en la tabla troncales, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena + "\n\nTroncal obtenida: " + "|" + CTroncal + "|";
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("No se ha encontrado la troncal en la tabla troncales"), Cadena, horas, e.ToString());
                        CreaArchivos = false;
                    }
                }
            }

            #endregion

            #region Llamada interna

            else if (Tipo.Equals("Llamada interna"))
            {
                CadenaCompleta = Cadena;

                try {

                    using (Conexion = new MySqlConnection(Config[25]))
                    {
                        Conexion.Open();
                        query = "Select * From parametros where parametro = 'Troncal_Virtual_Internas'";
                        comando = new MySqlCommand(query, Conexion);
                        lee = comando.ExecuteReader();
                        lee.Read();

                        T_Troncal = lee["seleccion"].ToString();
                        lee.Close();

                        query = "Select * From troncales where Line_Troncal = ?Ctroncal";

                        comando = new MySqlCommand(query, Conexion);
                        comando.Parameters.AddWithValue("?Ctroncal", T_Troncal);

                        lee = comando.ExecuteReader();
                        lee.Read();
                        Operador = lee["Operador"].ToString();
                        AccesoTroncal = lee["Nume_Acceso_Troncal"].ToString();
                        lee.Close();

                        Conexion.Close();
                    }

                    try
                    {
                        foreach (string s in ConfigI)
                        {
                            if (s[0].Equals('E'))
                            {
                                E_Extension = Cadena.Substring(Convert.ToInt32(s.Split('-')[1].Split(':')[0]), Convert.ToInt32(s.Split('-')[1].Split(':')[1]));
                                E_Extension = E_Extension.Replace(" ", "");
                            }
                            if (s[0].Equals('P'))
                            {
                                P_Codigo_Personal = Cadena.Substring(Convert.ToInt32(s.Split('-')[1].Split(':')[0]), Convert.ToInt32(s.Split('-')[1].Split(':')[1]));
                            }
                            if (s[0].Equals('N'))
                            {
                                NumMar = s.Split('-')[1];
                                NumMar = Cadena.Substring(Convert.ToInt32(NumMar.Split(':')[0]), Convert.ToInt32(NumMar.Split(':')[1])).Replace(" ", "");
                            }
                        }
                        try
                        {
                            using (Conexion = new MySqlConnection(Config[25]))
                            {
                                Conexion.Open();
                                query = "Select * From extensiones where Nume_Extension = ?NExt";
                                comando = new MySqlCommand(query, Conexion);
                                comando.Parameters.AddWithValue("?NExt", E_Extension);

                                lee = comando.ExecuteReader();
                                lee.Read();
                                ClaseExtension = lee["Clas_Extension"].ToString();
                                CentroDeCosto = lee["Codi_Centro"].ToString();
                                Folio = lee["Nume_Folio"].ToString();
                                NombreExtension = lee["Nomb_Extension"].ToString();
                                lee.Close();

                                query = "Select * From clase_extensiones where Clas_Extension = ?Cext";
                                comando = new MySqlCommand(query, Conexion);
                                comando.Parameters.AddWithValue("?Cext", ClaseExtension);

                                lee = comando.ExecuteReader();
                                lee.Read();
                                PlanTarifa = lee["Plan_Tarifario"].ToString();
                                lee.Close();

                                Conexion.Close();
                            }

                            try
                            {
                                using (Conexion = new MySqlConnection(Config[25]))
                                {
                                    Conexion.Open();
                                    query = "Select * From clase_llamadas where Clase_Llamada = 'INT'";
                                    comando = new MySqlCommand(query, Conexion);

                                    lee = comando.ExecuteReader();
                                    lee.Read();
                                    ClaseLlamada = lee["Clase_Llamada"].ToString();
                                    lee.Close();


                                    if (Operador.Equals("IT"))
                                    {
                                        query = "Select * From indicativosit";
                                        comando = new MySqlCommand(query, Conexion);

                                        lee = comando.ExecuteReader();
                                        while (lee.Read())
                                        {
                                            if (lee["Indicativo"].ToString().Length <= NumMar.Length)
                                            {
                                                Indic = NumMar.Substring(0, lee["Indicativo"].ToString().Length);
                                                if (Indic.Equals(lee["Indicativo"].ToString()))
                                                {
                                                    SoloUno++;
                                                    DigitosMinimos = lee["Digitos_Minimos"].ToString();
                                                    NumeroTarifa = lee["Tarifa"].ToString();
                                                    ClaseLlamada = lee["Clase_Llamada"].ToString();
                                                    Destino = lee["Destino"].ToString();
                                                }
                                            }
                                        }
                                        lee.Close();
                                    }

                                    Conexion.Close();
                                }
                                if (NumMar.Length >= Convert.ToInt32(DigitosMinimos))
                                {
                                    Tarifa(Cadena, false);
                                }
                                else
                                {
                                    FechaYa = DateTime.Now;
                                    horas = FechaYa.ToString();
                                    ErroresSaliDatosPuerto1.Add(horas + " El número marcado no cumple con la cantidad mínima de dígitos, Revisar correo para más información");
                                    LogErrores(horas + "  El número marcado no cumple con la cantidad mínima de dígitos, Revisar correo para más información");
                                    pictureBox4.BackColor = Color.Red;
                                    Subject = "Error del servidor";
                                    Body = horas + " Puerto 1: El número marcado no cumple con la cantidad mínima de dígitos, más información:  \nl\nl" + "\n\nCadena: " + Cadena + "\n\n La cadena se guardará en el log de Archivos Fallidos";
                                    CadenasFallidas(CodigoError("El número marcado no cumple con la cantidad mínima de dígitos"), Cadena, horas, "El número marcado no cumple con la cantidad mínima de dígitos");
                                    EnvioCorreo(Subject, Body);
                                    CreaArchivos = false;
                                }

                            }
                            catch (Exception e)
                            {
                                FechaYa = DateTime.Now;
                                horas = FechaYa.ToString();
                                if (ErrorSQLoCad() == true)
                                {
                                    ErrorSql(e);
                                }
                                else
                                {
                                    ErroresSaliDatosPuerto1.Add(horas + " No se ha encontrado el registro INT en clase_llamadas, ver correo paea más información");
                                    LogErrores(horas + " No se ha encontrado el registro INT en clase_llamadas, Revisar correo para más información");
                                    pictureBox4.BackColor = Color.Red;
                                    Subject = "Error del servidor";
                                    Body = horas + " Puerto 1: No se ha encontrado el registro INT en clase_llamadas, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                                    EnvioCorreo(Subject, Body);
                                    CadenasFallidas(CodigoError("No se ha encontrado el registro INT en clase_llamadas"), Cadena, horas, e.ToString());
                                    CreaArchivos = false;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            FechaYa = DateTime.Now;
                            horas = FechaYa.ToString();
                            if (ErrorSQLoCad() == true)
                            {
                                ErrorSql(e);
                            }
                            else
                            {
                                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones, ver correo paea más información");
                                LogErrores(horas + " Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones, Revisar correo para más información");
                                pictureBox4.BackColor = Color.Red;
                                Subject = "Error del servidor";
                                Body = horas + " Puerto 1: Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena + "\n\n La extension obtenida fue: |" + E_Extension + "|";
                                EnvioCorreo(Subject, Body);
                                CadenasFallidas(CodigoError("Ha ocurrido un problema al leer el número de la extensión en la tabla extensiones"), Cadena, horas, e.ToString());
                                CreaArchivos = false;
                            }

                        }
                    }
                    catch (Exception e)
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al obtener la extension o el codigo personal de la cadena, Revisar correo para más información");
                        LogErrores(horas + " Ha ocurrido un error al obtener la extension o el codigo personal de la cadena, Revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Ha ocurrido un error al obtener la extension o el codigo personal de la cadena, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("Ha ocurrido un error al obtener la extension o el codigo personal de la cadena"), Cadena, horas, e.ToString());
                        CreaArchivos = false;
                    }

                }
                catch (Exception e)
                {
                    FechaYa = DateTime.Now;
                    horas = FechaYa.ToString();
                    if (ErrorSQLoCad() == true)
                    {
                        ErrorSql(e);
                    }
                    else
                    {
                        ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al obtener la troncal de la cadena, Revisar correo para más información");
                        LogErrores(horas + " Ha ocurrido un error al obtener la troncal de la cadena, Revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Ha ocurrido un error al obtener la troncal de la cadena, más información:  \nl\nl" + e.ToString() + "\n\n Cadena: " + Cadena;
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("Ha ocurrido un error al obtener la troncal de la cadena"), Cadena, horas, e.ToString());
                        CreaArchivos = false;
                    }
                    
                }

                
            }

            #endregion


        }

        #region Extras

        public int DuracionCal(string Cadena)
        {
            try {
                DuracionProc1 = null;
                DuracionProc2 = null;
                foreach (string s in ConfigS)
                {
                    if (s[0].Equals('D'))
                    {
                        DuracionProc1 = s.Split('-')[1];
                        DuracionProc2 = s.Split('-')[2];
                    }
                }
                Duracion = Cadena.Substring(Convert.ToInt32(DuracionProc1.Split(':')[0]), Convert.ToInt32(DuracionProc1.Split(':')[1]));

                if (DuracionProc2.Equals("HMMSS") && Duracion.Length == 5)
                {
                    return ((Convert.ToInt32(Duracion.Substring(0, 1)) * 3600) + (Convert.ToInt32(Duracion.Substring(1, 2)) * 60) + (Convert.ToInt32(Duracion.Substring(3, 2))));
                }
                else if (DuracionProc2.Equals("HH:MM:SS") && Duracion.Length == 8)
                {
                    return ((Convert.ToInt32(Duracion.Split(':')[0]) * 3600) + (Convert.ToInt32(Duracion.Split(':')[1]) * 60) + (Convert.ToInt32(Duracion.Split(':')[2])));
                }
                else if (DuracionProc2.Equals("HH:MM.SS") && Duracion.Length == 8)
                {
                    return ((Convert.ToInt32(Duracion.Split(':')[0]) * 3600) + (Convert.ToInt32(Duracion.Split(':')[1].Split('.')[0]) * 60) + (Convert.ToInt32(Duracion.Split('.')[1])));
                }
                else if (DuracionProc2.Equals("HH.MM.SS") && Duracion.Length == 8)
                {
                    return ((Convert.ToInt32(Duracion.Split('.')[0]) * 3600) + (Convert.ToInt32(Duracion.Split('.')[1]) * 60) + (Convert.ToInt32(Duracion.Split('.')[2])));
                }
                else if (DuracionProc2.Equals("SSSSSS") && Duracion.Length == 6)
                {
                    /*Caractes especial*/
                    return (-1);
                }
                else if (DuracionProc2.Equals("SSSSS") && Duracion.Length == 5)
                {
                    /*Caractes especial*/
                    return (-1);
                }
                else if (DuracionProc2.Equals("SSSS") && Duracion.Length == 4)
                {
                    /*Caractes especial*/
                    return (-1);
                }
                else if (DuracionProc2.Equals("MM.SS") && Duracion.Length == 5)
                {
                    return ((Convert.ToInt32(Duracion.Split('.')[0]) * 60) + (Convert.ToInt32(Duracion.Split('.')[1])));
                }
                else if (DuracionProc2.Equals("H.MM.D") && Duracion.Length == 6)
                {
                    return ((Convert.ToInt32(Duracion.Split('.')[0]) * 3600) + (Convert.ToInt32(Duracion.Split('.')[1]) * 60) + (Convert.ToInt32(Duracion.Split('.')[2])));
                }
                else if (DuracionProc2.Equals("H.MM.SS") && Duracion.Length == 7)
                {
                    return ((Convert.ToInt32(Duracion.Split('.')[0]) * 3600) + (Convert.ToInt32(Duracion.Split('.')[1]) * 60) + (Convert.ToInt32(Duracion.Split('.')[2])));
                }
                else if (DuracionProc2.Equals("HMM.SS") && Duracion.Length == 6)
                {
                    return ((Convert.ToInt32(Duracion.Split('.')[0].Substring(0, 1)) * 3600) + (Convert.ToInt32(Duracion.Split('.')[0].Substring(1, 2)) * 60) + (Convert.ToInt32(Duracion.Split('.')[1])));
                }
                else if (DuracionProc2.Equals("H.MMSS") && Duracion.Length == 6)
                {
                    return ((Convert.ToInt32(Duracion.Split('.')[0]) * 3600) + (Convert.ToInt32(Duracion.Split('.')[1].Substring(0, 2)) * 60) + (Convert.ToInt32(Duracion.Split('.')[1].Substring(4, 2))));
                }
                else if(DuracionProc2.Equals("HH:MM'SS") && Duracion.Length == 8)
                {
                    return ((Convert.ToInt32(Duracion.Split(':')[0]) * 3600) + (Convert.ToInt32(Duracion.Split('\'')[0].Split(':')[1]) * 60) + (Convert.ToInt32(Duracion.Split('\'')[1])));
                }
                else
                {
                    return (-1);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al calcular la duración de la llamada, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al calcular la duración de la llamada, Revisar correo para más información");
                pictureBox4.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Ha ocurrido un error al calcular la duración de la llamada, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena + "\n\n La cadena se guardará en el log de Archivos Fallidos";
                EnvioCorreo(Subject, Body);
                return (-1);
            }
        }

        public bool ErrorSQLoCad()
        {
            try
            {
                if(Conexion.State == ConnectionState.Closed)
                {
                    using (Conexion = new MySqlConnection(Config[25]))
                    {
                        Conexion.Open();
                        return (false);
                    }
                }
                else
                {
                    return (false);
                }
            }
            catch
            {
                return (true);
            }
        }

        public void ErrorSql(Exception e)
        {
            Conexion.Close();
            ErroresSaliDatosPuerto1.Add(horas + " Ocurrió un error al conectarse con la base de datos, ver correo para más información");
            LogErrores(horas + " Ocurrió un error al conectarse con la base de datos, ver correo para más información");
            pictureBox4.BackColor = Color.Red;
            Subject = "Error del servidor";
            Body = horas + " Puerto 1:  Ocurrió un error al conectarse con la base de datos, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados y se re-subirá cuando se reanude la conexión";
            EnvioCorreo(Subject, Body);
            CreaArchivos = true;
        }

        public void Tarifa(string Cadena, bool InterEnv)
        {
            if (PlanTarifa.Equals("001"))
            {
                query = "Select * From plan_tarifario_001 where Codi_Tarifa = ?NumTarifa";
                ProcTarifa(Cadena, query, InterEnv);
            }
            else if (PlanTarifa.Equals("002"))
            {
                query = "Select * From plan_tarifario_002 where Codi_Tarifa = ?NumTarifa";
                ProcTarifa(Cadena, query, InterEnv);
            }
            else if (PlanTarifa.Equals("003"))
            {
                query = "Select * From plan_tarifario_003 where Codi_Tarifa = ?NumTarifa";
                ProcTarifa(Cadena, query, InterEnv);
            }
        }


        public void ProcTarifa(string Cadena, string query, bool InterEnv)
        {
            try {
                using (MySqlConnection Conexion = new MySqlConnection(Config[25]))
                {
                    Conexion.Open();

                    comando = new MySqlCommand(query, Conexion);
                    comando.Parameters.AddWithValue("?NumTarifa", NumeroTarifa);
                    lee = comando.ExecuteReader();
                    lee.Read();
                    DurMin = (Convert.ToInt32(lee["Dura_Minima"].ToString().Split(':')[0]) * 60) + (Convert.ToInt32(lee["Dura_Minima"].ToString().Split(':')[1]));
                    Intervalo = (Convert.ToInt32(lee["Inte_Cobro"].ToString().Split(':')[0]) + 60);
                    DuracionLLam = DuracionCal(Cadena);

                    RangTar1 = (Convert.ToInt32(lee["Rango_Tar_1"].ToString().Split(':')[0]));
                    RangTar2 = (Convert.ToInt32(lee["Rango_Tar_2"].ToString().Split(':')[0]));
                    RangTar3 = (Convert.ToInt32(lee["Rango_Tar_3"].ToString().Split(':')[0]));
                    RangTar4 = (Convert.ToInt32(lee["Rango_Tar_4"].ToString().Split(':')[0]));
                    RangTar5 = (Convert.ToInt32(lee["Rango_Tar_5"].ToString().Split(':')[0]));

                    Tarifa1 = lee["Rango_Tar_1_Vr_1"].ToString().Split('.')[0];
                    Tarifa2 = lee["Rango_Tar_2_Vr_2"].ToString().Split('.')[0];
                    Tarifa3 = lee["Rango_Tar_3_Vr_3"].ToString().Split('.')[0];
                    Tarifa4 = lee["Rango_Tar_4_Vr_4"].ToString().Split('.')[0];
                    Tarifa5 = lee["Rango_Tar_5_Vr_5"].ToString().Split('.')[0];

                    DurMinima = DurMin.ToString();
                    CargoFijo = lee["Cargo_Fijo"].ToString().Split('.')[0];
                    NombreTarifa = lee["Nomb_Tarifa"].ToString();

                    if (DuracionLLam >= 0)
                    {
                        if (DuracionLLam >= DurMin)
                        {
                            IntervaloSal = false;
                            IncrementoInt = Intervalo;

                            while (IntervaloSal == false)
                            {
                                if (DuracionLLam <= IncrementoInt)
                                {
                                    IntervaloSal = true;
                                    DuracionLlamAprox = IncrementoInt / 60;
                                }
                                IncrementoInt += Intervalo;
                            }

                            DuracionLlamada = DuracionLLam.ToString();
                            DuracionAprox = DuracionLlamAprox.ToString();

                            if (DuracionLlamAprox <= RangTar1)
                            {
                                DR1 = Convert.ToInt32((lee["Rango_Tar_1"].ToString().Split(':')[0]));
                                VR1 = DR1 * Convert.ToInt32(lee["Rango_Tar_1_Vr_1"].ToString().Split('.')[0]);

                                DurRang1 = DR1.ToString();
                                ValorRango1 = VR1.ToString();

                                ValorTotal = VR1.ToString();

                                ValorLlamadaTarifa = ValorTotal;
                                
                                ValorTotal = (VR1 + Convert.ToInt32(CargoFijo)).ToString();

                                RecargoServicioPorcentaje = lee["Reca_Servicio_Porcentual"].ToString().Split('.')[0];
                                RecargoServicioValor = (VR1 * ((Convert.ToDecimal(RecargoServicioPorcentaje)) / 100)).ToString();
                                
                                if (lee["IVA_Incluye_Recago"].ToString().Equals("N"))
                                {
                                    Iva = "N";
                                    BaseIVA = ValorTotal;
                                }
                                else
                                {
                                    Iva = "S";
                                    BaseIVA = (Convert.ToInt32(ValorTotal) + Convert.ToInt32(Convert.ToDecimal(RecargoServicioValor))).ToString();
                                }
                                PorcentajeIVA = lee["Porc_IVA"].ToString();
                                ValorIVA = (((Convert.ToDecimal(lee["Porc_IVA"].ToString().Split('.')[0])) / 100) * (Convert.ToDecimal(ValorTotal))).ToString();

                                if (Redondeo_IVA == true)
                                {
                                    ValorIVA = Math.Ceiling(Convert.ToDecimal(ValorIVA)).ToString();
                                }
                                if(Redondeo_Recargo == true)
                                {
                                    RecargoServicioValor = Math.Ceiling(Convert.ToDecimal(RecargoServicioValor)).ToString();
                                }

                                ValorTotal = (Convert.ToDecimal(BaseIVA) + Convert.ToDecimal(ValorIVA)).ToString();

                                SubeTabla(InterEnv);

                            }
                            else if (DuracionLlamAprox > RangTar1 && DuracionLlamAprox <= RangTar2)
                            {
                                DR1 = Convert.ToInt32((lee["Rango_Tar_1"].ToString().Split(':')[0]));
                                DR2 = DuracionLlamAprox - DR1;

                                VR1 = DR1 * Convert.ToInt32(lee["Rango_Tar_2_Vr_1"].ToString().Split('.')[0]);
                                VR2 = DR2 * Convert.ToInt32(lee["Rango_Tar_2_Vr_2"].ToString().Split('.')[0]);
                                
                                DurRang1 = DR1.ToString();
                                DurRang2 = DR2.ToString();
                                ValorRango1 = VR1.ToString();
                                ValorRango2 = VR2.ToString();

                                ValorTotal = (VR1 + VR2).ToString();

                                ValorLlamadaTarifa = ValorTotal;
                                
                                ValorTotal = ((VR1 + VR2) + Convert.ToInt32(CargoFijo)).ToString();

                                RecargoServicioPorcentaje = lee["Reca_Servicio_Porcentual"].ToString().Split('.')[0];
                                RecargoServicioValor = ((VR1 + VR2) * ((Convert.ToDecimal(RecargoServicioPorcentaje)) / 100)).ToString();

                                if (lee["IVA_Incluye_Recago"].ToString().Equals("N"))
                                {
                                    Iva = "N";
                                    BaseIVA = ValorTotal;
                                }
                                else
                                {
                                    Iva = "S";
                                    BaseIVA = (Convert.ToInt32(ValorTotal) + Convert.ToInt32(Convert.ToDecimal(RecargoServicioValor))).ToString();
                                }

                                PorcentajeIVA = lee["Porc_IVA"].ToString();
                                ValorIVA = (((Convert.ToDecimal(lee["Porc_IVA"].ToString().Split('.')[0])) / 100) * (Convert.ToDecimal(ValorTotal))).ToString();

                                if (Redondeo_IVA == true)
                                {
                                    ValorIVA = Math.Ceiling(Convert.ToDecimal(ValorIVA)).ToString();
                                }
                                if (Redondeo_Recargo == true)
                                {
                                    RecargoServicioValor = Math.Ceiling(Convert.ToDecimal(RecargoServicioValor)).ToString();
                                }

                                PorcentajeIVA = lee["Porc_IVA"].ToString();
                                ValorTotal = (Convert.ToDecimal(BaseIVA) + Convert.ToDecimal(ValorIVA)).ToString();

                                SubeTabla(InterEnv);
                            }
                            else if (DuracionLlamAprox > RangTar2 && DuracionLlamAprox <= RangTar3)
                            {
                                DR1 = Convert.ToInt32((lee["Rango_Tar_1"].ToString().Split(':')[0]));
                                DR2 = Convert.ToInt32((lee["Rango_Tar_2"].ToString().Split(':')[0])) - DR1;
                                DR3 = DuracionLlamAprox - DR2 - DR1;


                                VR1 = DR1 * Convert.ToInt32(lee["Rango_Tar_3_Vr_1"].ToString().Split('.')[0]);
                                VR2 = DR2 * Convert.ToInt32(lee["Rango_Tar_3_Vr_2"].ToString().Split('.')[0]);
                                VR3 = DR3 * Convert.ToInt32(lee["Rango_Tar_3_Vr_3"].ToString().Split('.')[0]);

                                DurRang1 = DR1.ToString();
                                DurRang2 = DR2.ToString();
                                DurRang3 = DR3.ToString();
                                ValorRango1 = VR1.ToString();
                                ValorRango2 = VR2.ToString();
                                ValorRango3 = VR3.ToString();

                                ValorTotal = (VR1 + VR2 + VR3).ToString();

                                ValorLlamadaTarifa = ValorTotal;
                                
                                ValorTotal = ((VR1 + VR2 + VR3) + Convert.ToInt32(CargoFijo)).ToString();

                                RecargoServicioPorcentaje = lee["Reca_Servicio_Porcentual"].ToString().Split('.')[0];
                                RecargoServicioValor = ((VR1 + VR2 + VR3) * ((Convert.ToDecimal(RecargoServicioPorcentaje)) / 100)).ToString();

                                if (lee["IVA_Incluye_Recago"].ToString().Equals("N"))
                                {
                                    Iva = "N";
                                    BaseIVA = ValorTotal;
                                }
                                else
                                {
                                    Iva = "S";
                                    BaseIVA = (Convert.ToInt32(ValorTotal) + Convert.ToInt32(Convert.ToDecimal(RecargoServicioValor))).ToString();
                                }

                                PorcentajeIVA = lee["Porc_IVA"].ToString();
                                ValorIVA = (((Convert.ToDecimal(lee["Porc_IVA"].ToString().Split('.')[0])) / 100) * (Convert.ToDecimal(ValorTotal))).ToString();

                                if (Redondeo_IVA == true)
                                {
                                    ValorIVA = Math.Ceiling(Convert.ToDecimal(ValorIVA)).ToString();
                                }
                                if (Redondeo_Recargo == true)
                                {
                                    RecargoServicioValor = Math.Ceiling(Convert.ToDecimal(RecargoServicioValor)).ToString();
                                }

                                ValorTotal = (Convert.ToDecimal(BaseIVA) + Convert.ToDecimal(ValorIVA)).ToString();

                                SubeTabla(InterEnv);
                            }
                            else if (DuracionLlamAprox > RangTar3 && DuracionLlamAprox <= RangTar4)
                            {
                                DR1 = Convert.ToInt32((lee["Rango_Tar_1"].ToString().Split(':')[0]));
                                DR2 = Convert.ToInt32((lee["Rango_Tar_2"].ToString().Split(':')[0])) - DR1;
                                DR3 = Convert.ToInt32((lee["Rango_Tar_3"].ToString().Split(':')[0])) - DR2 - DR1;
                                DR4 = DuracionLlamAprox - DR3 - DR2 - DR1;

                                VR1 = DR1 * Convert.ToInt32(lee["Rango_Tar_4_Vr_1"].ToString().Split('.')[0]);
                                VR2 = DR2 * Convert.ToInt32(lee["Rango_Tar_4_Vr_2"].ToString().Split('.')[0]);
                                VR3 = DR3 * Convert.ToInt32(lee["Rango_Tar_4_Vr_3"].ToString().Split('.')[0]);
                                VR4 = DR4 * Convert.ToInt32(lee["Rango_Tar_4_Vr_4"].ToString().Split('.')[0]);

                                DurRang1 = DR1.ToString();
                                DurRang2 = DR2.ToString();
                                DurRang3 = DR3.ToString();
                                DurRang4 = DR4.ToString();
                                ValorRango1 = VR1.ToString();
                                ValorRango2 = VR2.ToString();
                                ValorRango3 = VR3.ToString();
                                ValorRango4 = VR4.ToString();

                                ValorTotal = (VR1 + VR2 + VR3 + VR4).ToString();

                                ValorLlamadaTarifa = ValorTotal;
                                
                                ValorTotal = ((VR1 + VR2 + VR3 + VR4) + Convert.ToInt32(CargoFijo)).ToString();

                                RecargoServicioPorcentaje = lee["Reca_Servicio_Porcentual"].ToString().Split('.')[0];
                                RecargoServicioValor = ((VR1 + VR2 + VR3 + VR4) * ((Convert.ToDecimal(RecargoServicioPorcentaje)) / 100)).ToString();

                                if (lee["IVA_Incluye_Recago"].ToString().Equals("N"))
                                {
                                    Iva = "N";
                                    BaseIVA = ValorTotal;
                                }
                                else
                                {
                                    Iva = "S";
                                    BaseIVA = (Convert.ToInt32(ValorTotal) + Convert.ToInt32(Convert.ToDecimal(RecargoServicioValor))).ToString();
                                }

                                PorcentajeIVA = lee["Porc_IVA"].ToString();
                                ValorIVA = (((Convert.ToDecimal(lee["Porc_IVA"].ToString().Split('.')[0])) / 100) * (Convert.ToDecimal(ValorTotal))).ToString();

                                if (Redondeo_IVA == true)
                                {
                                    ValorIVA = Math.Ceiling(Convert.ToDecimal(ValorIVA)).ToString();
                                }
                                if (Redondeo_Recargo == true)
                                {
                                    RecargoServicioValor = Math.Ceiling(Convert.ToDecimal(RecargoServicioValor)).ToString();
                                }

                                ValorTotal = (Convert.ToDecimal(BaseIVA) + Convert.ToDecimal(ValorIVA)).ToString();

                                SubeTabla(InterEnv);
                            }
                            else if (DuracionLlamAprox > RangTar4)
                            {
                                DR1 = Convert.ToInt32((lee["Rango_Tar_1"].ToString().Split(':')[0]));
                                DR2 = Convert.ToInt32((lee["Rango_Tar_2"].ToString().Split(':')[0])) - DR1;
                                DR3 = Convert.ToInt32((lee["Rango_Tar_3"].ToString().Split(':')[0])) - DR2 - DR1;
                                DR4 = Convert.ToInt32((lee["Rango_Tar_4"].ToString().Split(':')[0])) - DR3 - DR2 - DR1;
                                DR5 = DuracionLlamAprox - DR4 - DR3 - DR2 - DR1;

                                VR1 = DR1 * Convert.ToInt32(lee["Rango_Tar_5_Vr_1"].ToString().Split('.')[0]);
                                VR2 = DR2 * Convert.ToInt32(lee["Rango_Tar_5_Vr_2"].ToString().Split('.')[0]);
                                VR3 = DR3 * Convert.ToInt32(lee["Rango_Tar_5_Vr_3"].ToString().Split('.')[0]);
                                VR4 = DR4 * Convert.ToInt32(lee["Rango_Tar_5_Vr_4"].ToString().Split('.')[0]);
                                VR5 = DR5 * Convert.ToInt32(lee["Rango_Tar_5_Vr_5"].ToString().Split('.')[0]);

                                DurRang1 = DR1.ToString();
                                DurRang2 = DR2.ToString();
                                DurRang3 = DR3.ToString();
                                DurRang4 = DR4.ToString();
                                DurRang5 = DR5.ToString();
                                ValorRango1 = VR1.ToString();
                                ValorRango2 = VR2.ToString();
                                ValorRango3 = VR3.ToString();
                                ValorRango4 = VR4.ToString();
                                ValorRango5 = VR5.ToString();

                                ValorTotal = (VR1 + VR2 + VR3 + VR4 + VR5).ToString();

                                ValorLlamadaTarifa = ValorTotal;
                                
                                ValorTotal = ((VR1 + VR2 + VR3 + VR4 + VR5) + Convert.ToInt32(CargoFijo)).ToString();

                                RecargoServicioPorcentaje = lee["Reca_Servicio_Porcentual"].ToString().Split('.')[0];
                                RecargoServicioValor = ((VR1 + VR2 + VR3 + VR4 + VR5) * ((Convert.ToDecimal(RecargoServicioPorcentaje)) / 100)).ToString();

                                if (lee["IVA_Incluye_Recago"].ToString().Equals("N"))
                                {
                                    Iva = "N";
                                    BaseIVA = ValorTotal;
                                }
                                else
                                {
                                    Iva = "S";
                                    BaseIVA = (Convert.ToInt32(ValorTotal) + Convert.ToInt32(Convert.ToDecimal(RecargoServicioValor))).ToString();
                                }

                                PorcentajeIVA = lee["Porc_IVA"].ToString();
                                ValorIVA = (((Convert.ToDecimal(lee["Porc_IVA"].ToString().Split('.')[0])) / 100) * (Convert.ToDecimal(ValorTotal))).ToString();

                                if (Redondeo_IVA == true)
                                {
                                    ValorIVA = Math.Ceiling(Convert.ToDecimal(ValorIVA)).ToString();
                                }
                                if (Redondeo_Recargo == true)
                                {
                                    RecargoServicioValor = Math.Ceiling(Convert.ToDecimal(RecargoServicioValor)).ToString();
                                }

                                ValorTotal = (Convert.ToDecimal(BaseIVA) + Convert.ToDecimal(ValorIVA)).ToString();

                                SubeTabla(InterEnv);
                            }
                            else
                            {
                                FechaYa = DateTime.Now;
                                horas = FechaYa.ToString();
                                ErroresSaliDatosPuerto1.Add(horas + " No se ha podido definir el rango en el cual la duración de la llamada se encuentra, Revisar correo para más información");
                                LogErrores(horas + " No se ha podido definir el rango en el cual la duración de la llamada se encuentra, Revisar correo para más información");
                                pictureBox4.BackColor = Color.Red;
                                Subject = "Error del servidor";
                                Body = horas + " Puerto 1: No se ha podido definir el rango en el cual la duración de la llamada se encuentra, más información:  \nl\nl" + "\n\nCadena: " + Cadena;
                                EnvioCorreo(Subject, Body);
                                CadenasFallidas(CodigoError("No se ha podido definir el rango en el cual la duración de la llamada se encuentra"), Cadena, horas, "No se ha podido definir el rango en el cual la duración de la llamada se encuentra"); ;
                                CreaArchivos = false;
                            }
                        }
                        else
                        {
                            FechaYa = DateTime.Now;
                            horas = FechaYa.ToString();
                            CadenasFallidas(CodigoError("La llamada no ha tarificado por duración mínima"), Cadena, horas, "La llamada no ha tarificado por duración mínima");
                            CreaArchivos = false;
                        }
                    }
                    else
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error al calcular la duración de la llamada, Revisar correo para más información");
                        LogErrores(horas + " Ha ocurrido un error al calcular la duración de la llamada, Revisar correo para más información");
                        pictureBox4.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Ha ocurrido un error al calcular la duración de la llamada, más información:  \nl\nl" + "\n\nCadena: " + Cadena;
                        EnvioCorreo(Subject, Body);
                        CadenasFallidas(CodigoError("Ha ocurrido un error al calcular la duración de la llamada"), Cadena, horas, "Ha ocurrido un error al calcular la duración de la llamada");
                        CreaArchivos = false;
                    }
                    lee.Close();
                    Conexion.Close();
                }
            }
            catch(Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                if (ErrorSQLoCad() == true)
                {
                    ErrorSql(e);
                    CreaArchivos = true;
                }
                else
                {
                    ErroresSaliDatosPuerto1.Add(horas + " Ha ocurrido un error en el proceso de tarificación, Revisar correo para más información");
                    LogErrores(horas + " Ha ocurrido un error en el proceso de tarificación, Revisar correo para más información");
                    pictureBox4.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: Ha ocurrido un error en el proceso de tarificación, más información:  \nl\nl" + e.ToString() + "\n\nCadena: " + Cadena + "Número de tarifa obtenido: " + "|" + NumeroTarifa + "|";
                    EnvioCorreo(Subject, Body);
                    CadenasFallidas(CodigoError("Ha ocurrido un error en el proceso de tarificación"), Cadena, horas, e.ToString());
                    CreaArchivos = false;
                }
            }
        }

        #endregion
        
        #endregion

        #region ProcesamientoCamp

        public void ProcesamientoCamp(string Tipo, string Cadena)
        {
            CreaArchivos = true;

            #region Llamada entrante
            if (Tipo.Equals("Llamada entrante"))
            {

            }

            #endregion

            #region Llamada saliente
            if (Tipo.Equals("Llamada saliente"))
            {

            }

            #endregion

            #region Llamada interna
            if (Tipo.Equals("Llamada interna"))
            {

            }

            #endregion

        }

        #region Extras



        #endregion

        #endregion

        #region Log_Cadenas_Fallidas

        public string CodigoError(string mensaje)
        {
            try
            {
                using (Conexion = new MySqlConnection(Config[25]))
                {
                    Conexion.Open();

                    query = "Select * From errores where MensajeError = ?MeneErr";
                    comando = new MySqlCommand(query, Conexion);
                    comando.Parameters.AddWithValue("?MeneErr", mensaje);
                    lee = comando.ExecuteReader();
                    lee.Read();
                    error = lee["idErrores"].ToString();
                    Conexion.Close();
                }
                return (error);
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                if (ErrorSQLoCad() == true)
                {
                    ErrorSql(e);
                }
                else
                {
                    ErroresSaliDatosPuerto1.Add(horas + " No se ha encontrado el id del error");
                    LogErrores(horas + " No se ha encontrado el id del error, Revisar correo para más información");
                    pictureBox4.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: No se ha encontrado el id del error, más información:  \nl\nl" + e.ToString();
                    EnvioCorreo(Subject, Body);
                }
                return ("NE");
            }
        }

        public void CadenasFallidas(string coderror, string cadenaerror, string fecha, string mensaje)
        {
            try
            {
                using (Conexion = new MySqlConnection(Config[25]))
                {
                    Conexion.Open();

                    query = @"insert into cadenas_erroneas (Fecha, idError, MensajeError, Cadena) values (?fecherr, ?iderr, ?menerr, ?caderr)";
                    comando = new MySqlCommand(query, Conexion);
                    comando.Parameters.AddWithValue("?iderr", coderror);
                    comando.Parameters.AddWithValue("?fecherr", fecha);
                    comando.Parameters.AddWithValue("?menerr", mensaje);
                    comando.Parameters.AddWithValue("?caderr", cadenaerror);
                    comando.ExecuteNonQuery();

                    Conexion.Close();
                    
                }
                Errores = coderror;
                if(SubeExpo == true)
                {
                    SubeTabla(false);
                }
                else
                {
                    Reset();
                    SubeExpo = true;
                }
                
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                horas = FechaYa.ToString();
                if (ErrorSQLoCad() == true)
                {
                    ErrorSql(e);
                }
                else
                {
                    ErroresSaliDatosPuerto1.Add(horas + " Ocurrió un error al subir una cadena fallida a SQL");
                    LogErrores(horas + " Ocurrió un error al subir una cadena fallida a SQL, Revisar correo para más información");
                    pictureBox4.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: Ocurrió un error al subir una cadena fallida a SQL, más información:  \nl\nl" + e.ToString();
                    EnvioCorreo(Subject, Body);
                }
            }
        }
        #endregion

        #region Log_Errores

        public void LogErrores(string mensj)
        {
            if (GeneraIP.Borrar != 1)
            {

                if (!Directory.Exists(PuertoLog))
                {
                    Directory.CreateDirectory(PuertoLog);
                }

                SubCarpetaLog = Path.Combine(PuertoLog, "Puerto " + Config[1]);
                ArchivoLog = Path.Combine(SubCarpetaLog, "Log_Errores.txt");

                if (!File.Exists(ArchivoLog))
                {
                    try
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        Directory.CreateDirectory(SubCarpetaLog);
                        using (StreamWriter w = File.AppendText(ArchivoLog))
                        {
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log("Se ha creado un nuevo archivo log el " + horas, w);
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log(mensj, w);
                        }
                    }
                    catch(Exception e)
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " Puerto 1: Ocurrió un problema al crear el archivo Log de Errores.");
                        pictureBox4.BackColor = Color.Red;
                        pictureBox2.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Ocurrió un problema al crear el archivo Log de Errores, más información:  \nl\nl" + e.ToString() + "\nl\nl Error no guardado: " + mensj;
                        EnvioCorreo(Subject, Body);
                    }
                }
                else
                {
                    try
                    {
                        using (StreamWriter w = File.AppendText(ArchivoLog))
                        {
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log(mensj, w);
                        }
                    }
                    catch (Exception e)
                    {
                        FechaYa = DateTime.Now;
                        horas = FechaYa.ToString();
                        ErroresSaliDatosPuerto1.Add(horas + " Puerto 1: Ocurrió un problema al escribir en el archivo Log de Errores.");
                        pictureBox4.BackColor = Color.Red;
                        pictureBox2.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Ocurrió un problema al escribir en el archivo Log de Errores, más información:  \nl\nl" + e.ToString() + "\nl\nl Error no guardado: " + mensj;
                        EnvioCorreo(Subject, Body);
                    }
                }
            }
        }



        #endregion

        #region counter

        private void InitializeTimer()
        {
            counter = 0;
            aTimer.Interval = 60000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;

            bTimer.Interval = 2000;
            bTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent2);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            counter++;
            try
            {
                if (counter == Convert.ToInt32(Minutos[0]))
                {
                    if(pictureBox3.BackColor != Color.Red)
                    {
                        pictureBox3.BackColor = Color.Yellow;
                    }
                    Subject = "Notificación de inactividad";
                    Body = "El puerto 1 ha permanecido " + counter + " minutos inactivo";
                    ErroresEntradaDatosPuerto1.Add("El puerto 1 ha permanecido " + counter + " minutos inactivo");
                    LogErrores("El puerto 1 ha permanecido " + counter + " minutos inactivo");
                    EnvioCorreo(Subject, Body);
                }
                if (counter == Convert.ToInt32(Minutos[1]))
                {
                    pictureBox3.BackColor = Color.Red;
                    Subject = "Alerta de inactividad";
                    Body = "El puerto 1 ha permanecido " + counter + " minutos inactivo";
                    ErroresEntradaDatosPuerto1.Add("Alerta: el puerto 1 ha permanecido " + counter + " minutos inactivo");
                    LogErrores("Alerta: el puerto 1 ha permanecido " + counter + " minutos inactivo");
                    counter2 = false;
                    EnvioCorreo(Subject, Body);
                }
            }
            catch (Exception r)
            {
                ErroresPuerto1.Add(r.ToString());
            }
        }

        #endregion

        #region Counter SQL

        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            if(Directory.Exists(SubCarpetaDoc))
            {
                if (Directory.GetFiles(SubCarpetaDoc).Length != 0)
                {
                    using (Conexion = new MySqlConnection(Config[25]))
                    {
                        try
                        {
                            Conexion.Open();
                            pictureBox11.BackColor = Color.Lime;
                            CargaDatosFalt();
                            Conexion.Close();
                        }
                        catch
                        {
                            pictureBox11.BackColor = Color.Red;
                        }
                    }
                }
                else
                {
                    bTimer.Enabled = false;
                }
            }
            else
            {
                bTimer.Enabled = false;
            }
        }

        public void CargaDatosFalt()
        {
            bTimer.Enabled = false;
            FechaYa = DateTime.Now;
            string fecha = FechaYa.ToString();
            Cadenas = new List<string>();
            try
            {
                string[] Archivos = Directory.GetFiles(SubCarpetaDoc);
                foreach (string s in Archivos)
                {
                    using (StreamReader lector = new StreamReader(s))
                    {
                        Cadenas.Add(lector.ReadLine());
                        lector.Close();
                    }
                    File.Delete(s);
                }
            }
            catch (Exception e)
            {
                pictureBox3.BackColor = Color.Red;
                Subject = "Error en la entrada de datos";
                Body = fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos, más información: \n\n" + e.ToString();
                ErroresEntradaDatosPuerto1.Add(fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos");
                LogErrores(fecha  + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos");
                counter2 = false;
                EnvioCorreo(Subject, Body);
            }

            foreach(string s in Cadenas)
            {
                ApList = false;
                if (Config[19] == "1")
                {
                    CantCar(s, fecha);
                }
                else if (Config[19] == "2")
                {
                    YaEsp = true;
                    SepCar(s, fecha);
                    YaEsp = false;
                }
                ApList = true;
            }
        }

        #endregion
        
        #region Conexión ExpoDatos

        Socket sck;
        EndPoint epLocal, epRemote;
        byte[] bufferExpo = new byte[1024];
        string FormatoHora = "";
        string FormatoFecha = "";
        string DiaI = "";
        string DiaF = "";
        string serviceName = "";
        bool FormatoHExpo;

        private void MessageCallBack(IAsyncResult aResult)
        {
            if (sck.Connected)
            {
                try
                {
                    int size = sck.EndReceiveFrom(aResult, ref epRemote);
                    if (size > 0)
                    {
                        byte[] recivedData = new byte[1024];
                        recivedData = (byte[])aResult.AsyncState;
                        ASCIIEncoding eEncoding = new ASCIIEncoding();
                        string recivedMessage = eEncoding.GetString(recivedData);
                        pictureBox12.BackColor = Color.Lime;
                        recivedMessage = recivedMessage.Substring(0, 2);

                        if (recivedMessage.Equals("01"))
                        {
                            Envia("mensaje:Conexion correcta:");
                        }
                        else if (recivedMessage.Equals("02"))
                        {
                            serviceName = "MySQL57";
                            ServiceController service = new ServiceController(serviceName);
                            try
                            {
                                service.Stop();
                                Envia("mensaje:El proceso se ha detenido:");
                            }
                            catch (Exception ex)
                            {
                                Envia("mensaje:No se ha podido detener el servicio:\n\n" + ex);
                            }
                        }
                        else if (recivedMessage.Equals("03"))
                        {
                            serviceName = "MySQL57";
                            ServiceController service = new ServiceController(serviceName);
                            try
                            {
                                service.Start();
                                Envia("mensaje:El proceso se ha iniciado:");
                            }
                            catch (Exception ex)
                            {
                                Envia("mensaje:No se ha podido iniciar el servicio:\n\n" + ex);
                            }
                        }
                        else if (recivedMessage.Equals("04"))
                        {
                            foreach (string s in ConfigS)
                            {
                                if (s[0].Equals('F'))
                                {
                                    FormatoFecha = s.Split('-')[2];
                                    DiaI = s.Split('-')[1].Split(':')[0];
                                    DiaF = s.Split('-')[1].Split(':')[1];
                                }
                            }
                            if (string.IsNullOrEmpty(FormatoFecha))
                            {
                                foreach (string s in ConfigS)
                                {
                                   if (s[0].Equals('m'))
                                    {
                                        FormatoFecha = s.Split('-')[2];
                                        DiaI = s.Split('-')[1].Split(':')[0];
                                        DiaF = s.Split('-')[1].Split(':')[1];
                                    }
                                }
                                if (string.IsNullOrEmpty(FormatoFecha))
                                {
                                    Envia("formatof|error");
                                }
                                else
                                {
                                    Envia("formatof|" + FormatoFecha + "-" + DiaI + "." + DiaF);
                                }
                            }
                            else
                            {
                                Envia("formatof|" + FormatoFecha + "-" + DiaI + "." + DiaF);
                            }
                        }
                        else if (recivedMessage.Equals("05"))
                        {
                            Application.Restart();
                        }
                        else if (recivedMessage.Equals("06"))
                        {
                            foreach (string s in ConfigS)
                            {
                                if (s[0].Equals('H'))
                                {
                                    FormatoHora = s.Split('-')[2];
                                }
                            }
                            if (string.IsNullOrEmpty(FormatoHora))
                            {
                                foreach (string s in ConfigS)
                                {
                                    if (s[0].Equals('j'))
                                    {
                                        FormatoHora = s.Split('-')[2];
                                    }
                                }
                                if (string.IsNullOrEmpty(FormatoHora))
                                {
                                    Envia("formatoh|error");
                                }
                                else
                                {
                                    FormatoHExpo = false;
                                    foreach(char s in FormatoHora.ToLower())
                                    {
                                        if(s == 'x')
                                        {
                                            FormatoHExpo = true;
                                        }
                                    }
                                    if (FormatoHExpo == true)
                                    {
                                        Envia("formatoh|" + "HHmm");
                                    }
                                    else
                                    {
                                        Envia("formatoh|" + FormatoHora);
                                    }
                                }
                            }
                            else
                            {
                                FormatoHExpo = false;
                                foreach (char s in FormatoHora.ToLower())
                                {
                                    if (s == 'x')
                                    {
                                        FormatoHExpo = true;
                                    }
                                }
                                if (FormatoHExpo == true)
                                {
                                    Envia("formatoh|" + "HHmm");
                                }
                                else
                                {
                                    Envia("formatoh|" + FormatoHora);
                                }
                            }
                        }
                    }
                    bufferExpo = new byte[1024];
                    sck.BeginReceiveFrom(bufferExpo, 0, bufferExpo.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), bufferExpo);
                }
                catch
                {
                    pictureBox12.BackColor = Color.Red;
                }
            }
        }
        public void ConectaExpo()
        {
            if (!sck.Connected)
            {
                try
                {
                    using (Conexion = new MySqlConnection(Config[25]))
                    {
                        Conexion.Open();
                        query = "Select * From parametros where parametro = ?NExt";
                        comando = new MySqlCommand(query, Conexion);
                        comando.Parameters.AddWithValue("?NExt", "Puerto RecepExpo");
                        lee = comando.ExecuteReader();
                        lee.Read();

                        epLocal = new IPEndPoint(IPAddress.Parse(Config[0]), Convert.ToInt32(lee["seleccion"].ToString()));
                        sck.Bind(epLocal);
                        epRemote = new IPEndPoint(IPAddress.Parse(Config[26]), Convert.ToInt32(Config[27]));
                        sck.Connect(epRemote);
                        sck.BeginReceiveFrom(bufferExpo, 0, bufferExpo.Length, SocketFlags.None, ref epRemote, new AsyncCallback(MessageCallBack), bufferExpo);

                        lee.Close();
                        Conexion.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    pictureBox12.BackColor = Color.Red;
                }
            }
            else
            {
                pictureBox12.BackColor = Color.Yellow;
            }
        }

        public void Envia(String Respuesta)
        {
            try
            {
                ASCIIEncoding enc = new ASCIIEncoding();
                byte[] msg = new byte[1024];
                msg = enc.GetBytes(Respuesta);
                sck.Send(msg);
                pictureBox12.BackColor = Color.Lime;
            }
            catch
            {
                pictureBox12.BackColor = Color.Red;
            }
        }

        #endregion

        #region Revisa Licencia
        
        public bool RevisaLicIn()
        {
            if (File.Exists(@"C:\Windows\bfsvc.txt"))
            {
                if(RevisaLic() == true)
                {
                    return (true);
                }
                else
                {
                    return (false);
                }
            }
            else
            {
                return (false);
            }
        }

        string Linea = "";
        string Hotel = "";
        public bool RevisaLic()
        {
            try
            {
                using (Conexion = new MySqlConnection(Config[25]))
                {
                    Conexion.Open();
                    query = "Select * From parametros where parametro = 'Reportes Hotel'";
                    comando = new MySqlCommand(query, Conexion);
                    lee = comando.ExecuteReader();
                    lee.Read();
                    Hotel = lee["seleccion"].ToString();
                    Conexion.Close();
                }
                if (File.Exists(@"C:\Windows\bfsvc.txt"))
                {
                    Linea = "";
                    using (StreamReader lector = new StreamReader(@"C:\Windows\bfsvc.txt"))
                    {
                        Linea = lector.ReadLine();
                        string horas = DateTime.Now.ToString(@"yyyy/MM/dd");
                        if (!string.IsNullOrEmpty(Linea))
                        {
                            try
                            {
                                if (descifrar(Linea).Split('-')[0].Equals(Hotel))
                                {
                                    lector.Close();
                                    return (true);
                                }
                                else
                                {
                                    lector.Close();
                                    return (false);
                                }
                            }
                            catch 
                            {
                                lector.Close();
                                return (false);
                            }
                        }
                        else
                        {
                            lector.Close();
                            return (false);
                        }
                    }
                }
                else
                {
                    return (false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return (true);
            }
        }

        string clave = "llave0315RD";
        byte[] llave;
        byte[] arreglo;
        MD5CryptoServiceProvider md5;
        TripleDESCryptoServiceProvider tripledes;
        byte[] resultado;
        

        public string descifrar(string cadena)
        {
            arreglo = Convert.FromBase64String(cadena); 
            md5 = new MD5CryptoServiceProvider();
            llave = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(clave));
            md5.Clear();

            tripledes = new TripleDESCryptoServiceProvider();
            tripledes.Key = llave;
            tripledes.Mode = CipherMode.ECB;
            tripledes.Padding = PaddingMode.PKCS7;
            ICryptoTransform convertir = tripledes.CreateDecryptor();
            resultado = convertir.TransformFinalBlock(arreglo, 0, arreglo.Length);
            tripledes.Clear();

            return (UTF8Encoding.UTF8.GetString(resultado));
        }

        #endregion

        #region Envio Interface
        
        public void CargaConfigInterface()
        {
            try
            {
                if (File.Exists(ArchivoIntP1))
                {
                    InterfaceConfig = new List<string>();
                    using (StreamReader lector = new StreamReader(ArchivoIntP1))
                    {
                        string Line;
                        while ((Line = lector.ReadLine()) != null)
                        {
                            InterfaceConfig.Add(Line);
                        }
                        lector.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                MessageBox.Show("Ocurrió un problema al leer el archivo " + ArchivoIntP1);
                pictureBox2.BackColor = Color.Red;
                pictureBox5.BackColor = Color.Red;
                pictureBox6.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + "Ocurrió un problema al leer el archivo " + ArchivoIntP1 + ", más información:  \nl\nl" + ex.ToString();
                EnvioCorreo(Subject, Body);
            }
        }

        #region Acciones

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    using (StreamReader lector = new StreamReader(openFileDialog1.FileName))
                    {
                        label65.Text = lector.ReadLine();
                        textBox4.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                label65.Text = "";
                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label70.Text = "";
            for (int i = 0; i < textBox4.Text.Length - 2; i++)
            {
                label70.Text += " ";
            }
            label70.Text += "| |";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                try
                {
                    if (string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox1.Text) || (!comboBox1.Text.Equals("si") && !comboBox1.Text.Equals("no")) || (!comboBox2.Text.Equals("si") && !comboBox2.Text.Equals("no")) || (!comboBox3.Text.Equals("si") && !comboBox3.Text.Equals("no")) || (!comboBox4.Text.Equals("si") && !comboBox4.Text.Equals("no")))
                    {
                        MessageBox.Show("Hay campos vacíos!");
                    }
                    else
                    {
                        GuardaInt();
                        DialogResult dialogResult = MessageBox.Show("El programa deberá reiniciarse para aplicar la configuración, ¿Dese reinicia ahora?", "Advertencia!", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            Contraseña.Sal = 1;
                            Application.Restart();
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (File.Exists(ArchivoIntP1))
                    {
                        File.Delete(ArchivoIntP1);
                    }
                    MessageBox.Show("Ha ocurrido un error al guardar la configuración:\n\n" + ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Aún no se puede guardar en otro puerto diferente al 1!");
            }
        }

        public void GuardaInt()
        {
            if (File.Exists(ArchivoIntP1))
            {
                File.Delete(ArchivoIntP1);
            }
            using (StreamWriter escritor = new StreamWriter(ArchivoIntP1))
            {
                escritor.WriteLine("Hotel Plus");
                escritor.WriteLine(textBox4.Text);
                if (string.IsNullOrEmpty(textBox6.Text)) { escritor.WriteLine("a- "); } else if (textBox6.Text.Length == 2) { escritor.WriteLine("a-" + textBox6.Text); } else { escritor.WriteLine("a- " + textBox6.Text); }
                if (string.IsNullOrEmpty(textBox7.Text)) { escritor.WriteLine("m- "); } else if (textBox7.Text.Length == 2) { escritor.WriteLine("m-" + textBox7.Text); } else { escritor.WriteLine("m- " + textBox7.Text); }
                if (string.IsNullOrEmpty(textBox8.Text)) { escritor.WriteLine("d- "); } else if (textBox8.Text.Length == 2) { escritor.WriteLine("d-" + textBox8.Text); } else { escritor.WriteLine("d- " + textBox8.Text); }
                if (string.IsNullOrEmpty(textBox9.Text)) { escritor.WriteLine("h- "); } else if (textBox9.Text.Length == 2) { escritor.WriteLine("h-" + textBox9.Text); } else { escritor.WriteLine("h- " + textBox9.Text); }
                if (string.IsNullOrEmpty(textBox10.Text)) { escritor.WriteLine("n- "); } else if (textBox10.Text.Length == 2) { escritor.WriteLine("n-" + textBox10.Text); } else { escritor.WriteLine("n- " + textBox10.Text); }
                if (string.IsNullOrEmpty(textBox11.Text)) { escritor.WriteLine("e- "); } else if (textBox11.Text.Length == 2) { escritor.WriteLine("e-" + textBox11.Text); } else { escritor.WriteLine("e- " + textBox11.Text); }
                if (string.IsNullOrEmpty(textBox12.Text)) { escritor.WriteLine("b- "); } else if (textBox12.Text.Length == 2) { escritor.WriteLine("b-" + textBox12.Text); } else { escritor.WriteLine("b- " + textBox12.Text); }
                if (string.IsNullOrEmpty(textBox13.Text)) { escritor.WriteLine("t- "); } else if (textBox13.Text.Length == 2) { escritor.WriteLine("t-" + textBox13.Text); } else { escritor.WriteLine("t- " + textBox13.Text); }
                if (string.IsNullOrEmpty(textBox14.Text)) { escritor.WriteLine("#- "); } else if (textBox14.Text.Length == 2) { escritor.WriteLine("#-" + textBox14.Text); } else { escritor.WriteLine("#- " + textBox14.Text); }
                if (string.IsNullOrEmpty(textBox15.Text)) { escritor.WriteLine("u- "); } else if (textBox15.Text.Length == 2) { escritor.WriteLine("u-" + textBox15.Text); } else { escritor.WriteLine("u- " + textBox15.Text); }
                if (string.IsNullOrEmpty(textBox16.Text)) { escritor.WriteLine("l- "); } else if (textBox16.Text.Length == 2) { escritor.WriteLine("l-" + textBox16.Text); } else { escritor.WriteLine("l- " + textBox16.Text); }
                if (string.IsNullOrEmpty(textBox17.Text)) { escritor.WriteLine("r- "); } else if (textBox17.Text.Length == 2) { escritor.WriteLine("r-" + textBox17.Text); } else { escritor.WriteLine("r- " + textBox17.Text); }
                if (string.IsNullOrEmpty(textBox18.Text)) { escritor.WriteLine("i- "); } else if (textBox18.Text.Length == 2) { escritor.WriteLine("i-" + textBox18.Text); } else { escritor.WriteLine("i- " + textBox18.Text); }
                if (string.IsNullOrEmpty(textBox19.Text)) { escritor.WriteLine("w- "); } else if (textBox19.Text.Length == 2) { escritor.WriteLine("w-" + textBox19.Text); } else { escritor.WriteLine("w- " + textBox19.Text); }
                if (string.IsNullOrEmpty(textBox20.Text)) { escritor.WriteLine("v- "); } else if (textBox20.Text.Length == 2) { escritor.WriteLine("v-" + textBox20.Text); } else { escritor.WriteLine("v- " + textBox20.Text); }
                if (string.IsNullOrEmpty(textBox21.Text)) { escritor.WriteLine("k- "); } else if (textBox21.Text.Length == 2) { escritor.WriteLine("k-" + textBox21.Text); } else { escritor.WriteLine("k- " + textBox21.Text); }
                if (string.IsNullOrEmpty(textBox22.Text)) { escritor.WriteLine("z- "); } else if (textBox22.Text.Length == 2) { escritor.WriteLine("z-" + textBox22.Text); } else { escritor.WriteLine("z- " + textBox22.Text); }
                if (string.IsNullOrEmpty(textBox2.Text)) { escritor.WriteLine("c- "); } else if (textBox2.Text.Length == 2) { escritor.WriteLine("c-" + textBox2.Text); } else { escritor.WriteLine("c- " + textBox2.Text); }
                escritor.WriteLine(comboBox1.Text);
                escritor.WriteLine(comboBox2.Text);
                escritor.WriteLine(comboBox3.Text);
                escritor.WriteLine(comboBox4.Text);
                escritor.WriteLine(comboBox5.Text);
                escritor.WriteLine(comboBox6.Text);
                escritor.WriteLine(comboBox7.Text);
                escritor.WriteLine(comboBox8.Text);
                escritor.WriteLine(comboBox9.Text);
                escritor.WriteLine(comboBox10.Text);
                escritor.WriteLine(comboBox11.Text);
                escritor.WriteLine(comboBox12.Text);
                escritor.WriteLine(comboBox13.Text);
                escritor.WriteLine(comboBox14.Text);
                escritor.WriteLine(comboBox15.Text);
                escritor.WriteLine(textBox1.Text);
                escritor.Close();

                MessageBox.Show("La configuración se ha guardado con éxito!");
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 1)
            {
                PermiteInterfaces = 0;
                Contraseña contraseña = new Contraseña(3);
                contraseña.ShowDialog();
                if(PermiteInterfaces == 1)
                {
                    if (tabControl2.SelectedIndex != 0)
                    {
                        tabControl2.SelectTab(0);
                    }
                    else
                    {
                        tabControl2_Selecting(null, new TabControlCancelEventArgs(tabPage3, 0, false, TabControlAction.Selecting));
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if(e.TabPageIndex == 0)
            {
                if(InterfaceConfig != null)
                {
                    textBox4.Text = InterfaceConfig[1];
                    textBox6.Text = InterfaceConfig[2].Split('-')[1].Replace(" ", "");
                    textBox7.Text = InterfaceConfig[3].Split('-')[1].Replace(" ", "");
                    textBox8.Text = InterfaceConfig[4].Split('-')[1].Replace(" ", "");
                    textBox9.Text = InterfaceConfig[5].Split('-')[1].Replace(" ", "");
                    textBox10.Text = InterfaceConfig[6].Split('-')[1].Replace(" ", "");
                    textBox11.Text = InterfaceConfig[7].Split('-')[1].Replace(" ", "");
                    textBox12.Text = InterfaceConfig[8].Split('-')[1].Replace(" ", "");
                    textBox13.Text = InterfaceConfig[9].Split('-')[1].Replace(" ", "");
                    textBox14.Text = InterfaceConfig[10].Split('-')[1].Replace(" ", "");
                    textBox15.Text = InterfaceConfig[11].Split('-')[1].Replace(" ", "");
                    textBox16.Text = InterfaceConfig[12].Split('-')[1].Replace(" ", "");
                    textBox17.Text = InterfaceConfig[13].Split('-')[1].Replace(" ", "");
                    textBox18.Text = InterfaceConfig[14].Split('-')[1].Replace(" ", "");
                    textBox19.Text = InterfaceConfig[15].Split('-')[1].Replace(" ", "");
                    textBox20.Text = InterfaceConfig[16].Split('-')[1].Replace(" ", "");
                    textBox21.Text = InterfaceConfig[17].Split('-')[1].Replace(" ", "");
                    textBox22.Text = InterfaceConfig[18].Split('-')[1].Replace(" ", "");
                    textBox2.Text = InterfaceConfig[19].Split('-')[1].Replace(" ", "");
                    comboBox1.Text = InterfaceConfig[20];
                    comboBox2.Text = InterfaceConfig[21];
                    comboBox3.Text = InterfaceConfig[22];
                    comboBox4.Text = InterfaceConfig[23];
                    comboBox5.Text = InterfaceConfig[24];
                    comboBox6.Text = InterfaceConfig[25];
                    comboBox7.Text = InterfaceConfig[26];
                    comboBox8.Text = InterfaceConfig[27];
                    comboBox9.Text = InterfaceConfig[28];
                    comboBox10.Text = InterfaceConfig[29];
                    comboBox11.Text = InterfaceConfig[30];
                    comboBox12.Text = InterfaceConfig[31];
                    comboBox13.Text = InterfaceConfig[32];
                    comboBox14.Text = InterfaceConfig[33];
                    comboBox15.Text = InterfaceConfig[34];
                    textBox1.Text = InterfaceConfig[35];
                }
            }
        }

        #endregion

        #region Procesa

        public void In1Procesa(string In1b, string In1e, string In1w, string In1v, string In1num, string In1z, string In1t, string In1l, string In1r, string In1i, string In1k, string Int1Fecha, string Int1Hora, string In1u, string init1FormatoFecha, string init1FormatoHora, string In1c, string In1Cad)
        {
            try
            {
                if (!init1FormatoFecha.Equals(""))
                {
                    init1FormatoFecha = init1FormatoFecha.ToLower();
                    Init1Mes = "";
                    Init1Dia = "";
                    Init1Año = "";
                    Init1Horas = "";
                    Init1Minutos = "";
                    Ini1Pos = 0;

                    foreach (char s in init1FormatoFecha)
                    {
                        if (s == 'm')
                        {
                            Init1Mes += Ini1Pos.ToString();
                        }
                        else if (s == 'd')
                        {
                            Init1Dia += Ini1Pos.ToString();
                        }
                        else if (s == 'y')
                        {
                            Init1Año += Ini1Pos.ToString();
                        }
                        Ini1Pos++;
                    }
                    if (!Init1Dia.Equals(""))
                    {
                        In1d = Int1Fecha.Substring(Convert.ToInt32(Init1Dia[0].ToString()), Init1Dia.Length);
                    }
                    if (!Init1Mes.Equals(""))
                    {
                        In1m = Int1Fecha.Substring(Convert.ToInt32(Init1Mes[0].ToString()), Init1Mes.Length);
                    }
                    if (!Init1Año.Equals(""))
                    {
                        if(Init1Año.Length == 2)
                        {
                            In1a = "20" + Int1Fecha.Substring(Convert.ToInt32(Init1Año[0].ToString()), Init1Año.Length);
                        }
                        else
                        {
                            In1a = Int1Fecha.Substring(Convert.ToInt32(Init1Año[0].ToString()), Init1Año.Length);
                        }
                    }

                    if (!init1FormatoHora.Equals(""))
                    {
                        init1FormatoHora = init1FormatoHora.ToLower();
                        Ini1Pos = 0;

                        foreach (char s in init1FormatoHora)
                        {
                            if (s == 'h')
                            {
                                Init1Horas += Ini1Pos.ToString();
                            }
                            else if (s == 'm')
                            {
                                Init1Minutos += Ini1Pos.ToString();
                            }
                            Ini1Pos++;
                        }
                        if (!Init1Horas.Equals(""))
                        {
                            In1h = Int1Hora.Substring(Convert.ToInt32(Init1Horas[0].ToString()), Init1Horas.Length);
                        }
                        if (!Init1Minutos.Equals(""))
                        {
                            In1n = Int1Hora.Substring(Convert.ToInt32(Init1Minutos[0].ToString()), Init1Minutos.Length);
                        }

                        In1CadenaFinal = "";
                        for (int i = 0; i < InterfaceConfig[1].Length; i++)
                        {
                            In1Caracter = InterfaceConfig[1][i];
                            In1Inicia = i;
                            In1Aumenta = 0;
                            for (int n = i; n < InterfaceConfig[1].Length; n++)
                            {
                                if (InterfaceConfig[1][n].Equals(In1Caracter))
                                {
                                    In1Aumenta++;
                                }
                                else
                                {
                                    n = InterfaceConfig[1].Length;
                                }
                            }
                            if (In1Caracter.Equals('a'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[2].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[2].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1a.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[2].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1a;
                                    }
                                    else if (InterfaceConfig[2].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1a;
                                        for (int n = 0; n < (In1Aumenta - In1a.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[2].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1a;
                                }
                            }
                            else if (In1Caracter.Equals('m'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[3].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[3].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1m.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[3].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1m;
                                    }
                                    else if (InterfaceConfig[3].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1m;
                                        for (int n = 0; n < (In1Aumenta - In1m.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[3].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1m;
                                }
                            }
                            else if (In1Caracter.Equals('d'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[4].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[4].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1d.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[4].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1d;
                                    }
                                    else if (InterfaceConfig[4].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1d;
                                        for (int n = 0; n < (In1Aumenta - In1d.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[4].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1d;
                                }
                            }
                            else if (In1Caracter.Equals('h'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[5].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[5].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1h.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[5].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1h;
                                    }
                                    else if (InterfaceConfig[5].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1h;
                                        for (int n = 0; n < (In1Aumenta - In1h.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[5].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1h;
                                }
                            }
                            else if (In1Caracter.Equals('n'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[6].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[6].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1n.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[6].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1n;
                                    }
                                    else if (InterfaceConfig[6].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1n;
                                        for (int n = 0; n < (In1Aumenta - In1n.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[6].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1n;
                                }
                            }
                            else if (In1Caracter.Equals('e'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[7].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[7].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1e.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[7].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1e;
                                    }
                                    else if (InterfaceConfig[7].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1e;
                                        for (int n = 0; n < (In1Aumenta - In1e.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[7].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1e;
                                }
                            }
                            else if (In1Caracter.Equals('b'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[8].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[8].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1b.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[8].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1b;
                                    }
                                    else if (InterfaceConfig[8].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1b;
                                        for (int n = 0; n < (In1Aumenta - In1b.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[8].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1b;
                                }
                            }
                            else if (In1Caracter.Equals('t'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[9].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[9].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1t.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[9].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1t;
                                    }
                                    else if (InterfaceConfig[9].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1t;
                                        for (int n = 0; n < (In1Aumenta - In1t.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[9].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1t;
                                }
                            }
                            else if (In1Caracter.Equals('#'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[10].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[10].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1num.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[10].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1num;
                                    }
                                    else if (InterfaceConfig[10].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1num;
                                        for (int n = 0; n < (In1Aumenta - In1num.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[10].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1num;
                                }
                            }
                            else if (In1Caracter.Equals('u'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[11].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[11].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1u.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[11].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1u;
                                    }
                                    else if (InterfaceConfig[11].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1u;
                                        for (int n = 0; n < (In1Aumenta - In1u.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[11].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1u;
                                }
                            }
                            else if (In1Caracter.Equals('l'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[12].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[12].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1l.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[12].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1l;
                                    }
                                    else if (InterfaceConfig[12].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1l;
                                        for (int n = 0; n < (In1Aumenta - In1l.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[12].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1l;
                                }
                            }
                            else if (In1Caracter.Equals('r'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[13].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[13].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1r.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[13].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1r;
                                    }
                                    else if (InterfaceConfig[13].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1r;
                                        for (int n = 0; n < (In1Aumenta - In1r.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[13].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1r;
                                }
                            }
                            else if (In1Caracter.Equals('i'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[14].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[14].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1i.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[14].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1i;
                                    }
                                    else if (InterfaceConfig[14].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1i;
                                        for (int n = 0; n < (In1Aumenta - In1i.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[14].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1i;
                                }
                            }
                            else if (In1Caracter.Equals('w'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[15].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[15].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1w.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[15].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1w;
                                    }
                                    else if (InterfaceConfig[15].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1w;
                                        for (int n = 0; n < (In1Aumenta - In1w.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[15].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1w;
                                }
                            }
                            else if (In1Caracter.Equals('v'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[16].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[16].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1v.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[16].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1v;
                                    }
                                    else if (InterfaceConfig[16].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1v;
                                        for (int n = 0; n < (In1Aumenta - In1v.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[16].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1v;
                                }
                            }
                            else if (In1Caracter.Equals('k'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[17].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[17].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1k.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[17].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1k;
                                    }
                                    else if (InterfaceConfig[17].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1k;
                                        for (int n = 0; n < (In1Aumenta - In1k.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[17].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1k;
                                }
                            }
                            else if (In1Caracter.Equals('z'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[18].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[18].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1z.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[18].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1z;
                                    }
                                    else if (InterfaceConfig[18].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1z;
                                        for (int n = 0; n < (In1Aumenta - In1z.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[18].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1z;
                                }
                            }
                            else if (In1Caracter.Equals('c'))
                            {
                                i = In1Inicia + In1Aumenta;
                                i--;
                                if (!InterfaceConfig[19].Split('-')[1].Equals(" "))
                                {
                                    if (InterfaceConfig[19].Split('-')[1][1].Equals('d'))
                                    {
                                        for (int n = 0; n < (In1Aumenta - In1c.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[19].Split('-')[1][0];
                                        }
                                        In1CadenaFinal += In1c;
                                    }
                                    else if (InterfaceConfig[19].Split('-')[1][1].Equals('i'))
                                    {
                                        In1CadenaFinal += In1c;
                                        for (int n = 0; n < (In1Aumenta - In1c.Length); n++)
                                        {
                                            In1CadenaFinal += InterfaceConfig[19].Split('-')[1][0];
                                        }
                                    }
                                }
                                else
                                {
                                    In1CadenaFinal += In1c;
                                }
                            }
                            else
                            {
                                if (!Convert.ToByte(In1Caracter).ToString("x2").Equals("c2"))
                                {
                                    In1CadenaFinal += In1Caracter;
                                }
                            }
                        }
                        EnviaIn1(InterfaceConfig[35], In1CadenaFinal);
                    }
                    else
                    {
                        horas = DateTime.Now.ToString();
                        ErroresPuerto1.Add(horas + " Puerto 1: Problema al procesar la hora de la llamada (Envio int)");
                        LogErrores(horas + " Puerto 1: Problema al procesar la hora de la llamada (Envio int)");
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 1: Problema al procesar la hora de la llamada (Envio int), más información:  \nl\nlLa llamada se guardará en: " + DirectorioInt1() + "\\errores";
                        EnvioCorreo(Subject, Body);
                        pictureBox3.BackColor = Color.Red;
                        aTimer.Enabled = false;
                        counter2 = false;
                        GuardaErroresInt1(DirectorioInt1(), In1Cad, horas, "Problema al procesar la hora de la llamada (Envio int)");
                    }

                }
                else
                {
                    horas = DateTime.Now.ToString();
                    ErroresPuerto1.Add(horas + " Puerto 1: Problema al procesar la fecha de la llamada (Envio int)");
                    LogErrores(horas + " Puerto 1: Problema al procesar la fecha de la llamada (Envio int)");
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 1: Problema al procesar la fecha de la llamada (Envio int), más información:  \nl\nlLa llamada se guardará en: " + DirectorioInt1() + "\\errores";
                    EnvioCorreo(Subject, Body);
                    pictureBox3.BackColor = Color.Red;
                    aTimer.Enabled = false;
                    counter2 = false;
                    GuardaErroresInt1(DirectorioInt1(), In1Cad, horas, "Problema al procesar la fecha de la llamada (Envio int)");
                }
            }
            catch (Exception ex)
            {
                horas = DateTime.Now.ToString();
                ErroresPuerto1.Add(horas + " Puerto 1: Problema al procesar la llamada (Envio int)");
                LogErrores(horas + " Puerto 1: Problema al procesar la llamada (Envio int)");
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Problema al procesar la llamada (Envio int), más información:  \nl\nl" + ex.ToString() + "\n\nLa llamada se guardará en: " + DirectorioInt1() + "\\errores";
                EnvioCorreo(Subject, Body);
                pictureBox3.BackColor = Color.Red;
                aTimer.Enabled = false;
                counter2 = false;
                GuardaErroresInt1(DirectorioInt1(), In1Cad, horas, "Problema al procesar la llamada (Envio int)");
            }
        }

        public string DirectorioInt1()
        {
            if (directorioIn1.Equals(""))
            {
                for (int i = 0; i < InterfaceConfig[35].Split('\\').Length - 1; i++)
                {
                    directorioIn1 += InterfaceConfig[35].Split('\\')[i] + "\\";
                }
                return (directorioIn1);
            }
            else
            {
                return (directorioIn1);
            }
            
        }
        
        public void EnviaIn1(string path, string cadena)
        {
            try
            {
                if (!Directory.Exists(DirectorioInt1()))
                {
                    Directory.CreateDirectory(DirectorioInt1());
                }
                using (StreamWriter escritor = new StreamWriter(path, true))
                {
                    escritor.WriteLine(cadena);
                    escritor.Close();
                }
            }
            catch(Exception ex)
            {
                horas = DateTime.Now.ToString();
                ErroresPuerto1.Add(horas + " Puerto 1: Problema al guardar la llamada (Envio int)");
                LogErrores(horas + " Puerto 1: Problema al guardar la llamada (Envio int)");
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Problema al guardar la llamada (Envio int), más información:  \nl\nl" + ex.ToString() + "\n\nLa llamada se guardará en: " + DirectorioInt1() + "\\errores";
                EnvioCorreo(Subject, Body);
                pictureBox3.BackColor = Color.Red;
                aTimer.Enabled = false;
                counter2 = false;
                GuardaErroresInt1(DirectorioInt1(), cadena, horas, "Problema al guardar la llamada (Envio int)");
            }
        }

        public void GuardaErroresInt1(string path, string cadena, string horas, string mensaje)
        {
            if (!Directory.Exists(DirectorioInt1()))
            {
                Directory.CreateDirectory(DirectorioInt1());
            }
            using (StreamWriter escritor = new StreamWriter(path + "errores.txt", true))
            {
                escritor.WriteLine(mensaje + " hora: " + horas + " cadena: " + cadena);
                escritor.Close();
            }
        }

        #endregion

        #endregion

    }

    #endregion

    #region Puerto 2
    class Puerto2
    {

        #region Inicio

        DateTime FechaYa;

        delegate void SetTextCallback(string TextoFin, string fecha);
        public RecepDatos parentForm;

        //Setup
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> _clienSockets = new List<Socket>();
        private static byte[] _buffer = new byte[1024];
        
        //log errores
        public static string DireccionLog = Directory.GetCurrentDirectory();
        public static string CarpetaLog = @DireccionLog;
        public static string PuertoLog = System.IO.Path.Combine(CarpetaLog, "Log de errores");
        public static string SubCarpetaLog = System.IO.Path.Combine(PuertoLog, "Puerto 2");
        public static string ArchivoLog = System.IO.Path.Combine(SubCarpetaLog, "Log_Errores.txt");

        //Envio Correo
        public string Subject;
        public string Body;

        //Timer
        public int counter;
        public bool counter2 = true;
        public System.Timers.Timer aTimer = new System.Timers.Timer();
        public string[] Minutos;

        //Archivos separados
        public static string DireccionDoc = Directory.GetCurrentDirectory();
        public static string CarpetaDoc = DireccionDoc;
        public static string PuertoDoc = System.IO.Path.Combine(CarpetaDoc, "Puerto 2");
        public static string SubCarpetaDoc = System.IO.Path.Combine(PuertoDoc, "Registro individual Puerto 2");
        public static string ArchivoDoc;
        public bool CreaArchivos = true;

        //Copia de seguridad
        public static string DireccionCopi = Directory.GetCurrentDirectory();
        public static string CarpetaCopi = DireccionCopi;
        public static string SubCarpetaCopi = System.IO.Path.Combine(PuertoDoc, "Registro mensual Puerto 2");
        public static string ArchivoCopi;

        //Archivos salvados
        public static string DireccionSalv = Directory.GetCurrentDirectory();
        public static string CarpetaSalv = @DireccionSalv;
        public static string PuertoSalv = System.IO.Path.Combine(CarpetaSalv, "Archivos Salvados");
        public static string SubCarpetaSalv = System.IO.Path.Combine(PuertoSalv, "Archivos Salvados puerto 2");
        public static string ArchivoSalv = System.IO.Path.Combine(SubCarpetaSalv, "Archivos_Salvados.txt");

        //Configuracion SQL
        public List<string> ProP2 = RecepDatos.ProP2;
        public List<string> ConfigE = new List<string>();
        public List<string> ConfigS = new List<string>();
        public List<string> ConfigI = new List<string>();
        public List<string> TiposLlamada = new List<string>();
        public List<string> TiposLlamada2 = new List<string>();
        public bool Igual = false;
        public string TipoLlamPos = null;
        public string TipoLlamPosE = null;
        public string TipoLlamPosS = null;
        public string TipoLlamPosI = null;
        public string[] TipoLlampos = null;
        public string[] TipoLlamposE = null;
        public string[] TipoLlamposS = null;
        public string[] TipoLlamposI = null;
        public string TipoLlamadaCad = null;
        public string Tipo = null;
        public int i = 0;
        public int Veces = 0;

        //Conexion SQL
        public string query;
        public MySqlCommand comando;
        public int IntentosCon = 0;

        //Datos a SQL
        public string F_Fecha_Final_Lamada = "-";
        public string H_Hora_Final_Llamada = "-";
        public string E_Extension = "-";
        public string T_Troncal = "-";
        public string N_Numero_Marcado = "-";
        public string D_Duracion = "-";
        public string P_Codigo_Personal = "-";
        public string m_Fecha_Inicial_Llamada = "-";
        public string j_Hora_Inicial_Llamada = "-";
        public string l_Tipo_Llamada = "-";
        public string R_Trafico_Interno_Externo = "-";

        //Caracteres Especiales
        List<string> P2 = new List<string>();
        bool pu2 = false;
        public bool YaEsp = false;

        //CounterSQL
        System.Timers.Timer bTimer = new System.Timers.Timer();
        List<string> Cadenas;
        public bool ApList = true;

        public void Inicio(RecepDatos parentForms)
        {
            parentForm = parentForms;
            InitializeTimer();
            CargaConfigSQL();
            SetupServer();
        }

        #endregion

        #region Setup
        public void SetupServer()
        {
            P2 = RecepDatos.P2;
            pu2 = RecepDatos.pu2;
            try
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                parentForm.Puerto2.Items.Add("configurando puerto 2...");
                _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(RecepDatos.Config[0]), Convert.ToInt32(RecepDatos.Config[10])));
                _serverSocket.Listen(5);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                parentForm.Puerto2.Items.Add("Puerto 2 listo");
                LogErrores(horas + " Se ha iniciado el puerto 2");
                Subject = "Estado del servidor";
                Body = horas + " Se ha iniciado el puerto 2";

                parentForm.EnvioCorreo(Subject, Body);

                DireccionDoc = RecepDatos.Config[2];
                CarpetaDoc = DireccionDoc;
                PuertoDoc = Path.Combine(CarpetaDoc, "Puerto " + RecepDatos.Config[10]);
                SubCarpetaDoc = Path.Combine(PuertoDoc, "Registro individual");

                bTimer.Enabled = true;
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                parentForm.Puerto2.Items.Add(horas + " Problema al configurar el puerto 2!");
                RecepDatos.ErroresPuerto2.Add(horas + " Problema al configurar el puerto 2: probablemente la IP no es correcta");
                LogErrores(horas + " Problema al configurar el puerto 2: probablemente la IP no es correcta");
                Subject = "Error del servidor";
                Body = horas + " Problema al configurar el puerto 2: probablemente la IP no es correcta, más información:  \nl\nl" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox5.BackColor = Color.Red;

            }
        }
        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(AR);
                _clienSockets.Add(socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresPuerto2.Add(horas + " Puerto 2: Problema al comunicarse con el cliente (AcceptCallback)");
                LogErrores(horas + " Puerto 2: Problema al comunicarse con el cliente (AcceptCallback)");
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Problema al comunicarse con el cliente (AcceptCallback), más información:  \nl\nl" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox7.BackColor = Color.Red;
                aTimer.Enabled = false;
                counter2 = false;
            }
        }
        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            try
            {
                int received = socket.EndReceive(AR);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);
                string text = BitConverter.ToString(dataBuf);
                SetText(text);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch
            {
                socket.Dispose();
                socket.Close();
            }
        }
        #endregion

        #region Txt

        public void SetText(string text)
        {
            try
            {
                FechaYa = DateTime.Now;
                string fecha = FechaYa.ToString();
                string TextoFin = "";
                counter = 0;
                char[] split = { '-' };
                if (text.Length > 3)
                {
                    if (counter2 == true)
                    {
                        parentForm.pictureBox7.BackColor = Color.Lime;
                    }

                    string textofin = "";
                    string[] caracteres = text.Split(split);
                    foreach (string Caracteres in caracteres)
                    {
                        if (Caracteres.ToLower().Equals("0a") || Caracteres.ToLower().Equals("0d"))
                        {
                            if (textofin.Length > 2)
                            {
                                string[] hexValuesSplit = textofin.Split(' ');
                                foreach (String hex in hexValuesSplit)
                                {
                                    int value = Convert.ToInt32(hex, 16);
                                    string stringValue = Char.ConvertFromUtf32(value);
                                    TextoFin = TextoFin + stringValue;
                                }

                                if (RecepDatos.Config[20] == "1")
                                {
                                    CantCar(TextoFin, fecha);
                                }
                                else if (RecepDatos.Config[20] == "2")
                                {
                                    SepCar(TextoFin, fecha);
                                }

                                TextoFin = "";
                                textofin = "";
                            }
                        }
                        else
                        {
                            if (textofin.Equals(""))
                            {
                                textofin = Caracteres;
                            }
                            else
                            {
                                textofin = textofin + " " + Caracteres;
                            }
                        }
                    }
                    if (textofin.Length > 2)
                    {

                        string[] hexValuesSplit2 = textofin.Split(' ');
                        foreach (String hex in hexValuesSplit2)
                        {
                            int value = Convert.ToInt32(hex, 16);
                            string stringValue = Char.ConvertFromUtf32(value);
                            TextoFin = TextoFin + stringValue;
                        }
                        if (RecepDatos.Config[20] == "1")
                        {
                            CantCar(TextoFin, fecha);
                        }
                        else if (RecepDatos.Config[20] == "2")
                        {
                            SepCar(TextoFin, fecha);
                        }
                        TextoFin = "";
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresEntradaDatosPuerto2.Add("Puerto 2: Ocurrió un error al recibir un dato (SetText)" + horas);
                LogErrores(horas + " Puerto 2: Ocurrió un error al recibir un dato (SetText)");
                Subject = "Puerto 2: Error del servidor";
                aTimer.Enabled = false;
                Body = horas + " Puerto 2: Ocurrió un error al recibir un dato (SetText), más información: \n\n" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox7.BackColor = Color.Red;
                counter2 = false;
            }
        }

        public void CantCar(string TextoFin, string fecha)
        {
            if (TextoFin.Length == Convert.ToUInt32(RecepDatos.Config[14]))
            {
                RecibeCadCar(TextoFin);
                Salida(TextoFin, fecha);
            }
            else
            {
                ErrorCad(TextoFin, fecha);
            }
        }

        public void SepCar(string TextoFin, string fecha)
        {
            int Campos = 0;
            if (pu2 == true && YaEsp == false)
            {
                try
                {
                    foreach (string s in P2)
                    {
                        TextoFin = TextoFin.Replace(s[1], RecepDatos.Config[23][0]);
                    }
                }
                catch(Exception r)
                {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresEntradaDatosPuerto2.Add("Puerto 2: Ocurrió un error al reemplazar el caracter especial" + horas);
                LogErrores(horas + " Puerto 2: Ocurrió un error al reemplazar el caracter especial");
                Subject = "Puerto 2: Error del servidor";
                aTimer.Enabled = false;
                Body = horas + " Puerto 2: Ocurrió un error al reemplazar el caracter especial, más información: \n\n" + r.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox7.BackColor = Color.Red;
                counter2 = false;
                }
            }
            string[] Partes = TextoFin.Split(RecepDatos.Config[23][0]);
            foreach (var s in Partes)
            {
                Campos++;
            }

            if (Campos == Convert.ToInt32(RecepDatos.Config[14]))
            {
                RecibeCadCamp(TextoFin);
                Salida(TextoFin, fecha);
            }
            else
            {
                ErrorCad(TextoFin, fecha);
            }
        }

        public void ErrorCad(string TextoFin, string fecha)
        {
            FechaYa = DateTime.Now;
            string horas = FechaYa.ToString();
            RecepDatos.ErroresEntradaDatosPuerto2.Add(horas + " Puerto 2: Se ha recibido un dato erroneo: Revisar correo.");
            parentForm.pictureBox7.BackColor = Color.Red;
            counter2 = false;
            aTimer.Enabled = false;
            TextoFin = fecha + "  " + TextoFin;
            LogErrores("Puerto 2: Se ha recibido una entrada no válida:  " + TextoFin);
            Subject = "Puerto 2: Entrada errónea";
            Body = "Puerto 2: Se ha recibido una entrada no válida:  " + TextoFin;
            parentForm.EnvioCorreo(Subject, Body);
        }

        public void Salida(string TextoFin, string fecha)
        {
            if (CreaArchivos == true)
            {
                CreaTxt(TextoFin);
            }

            if (ApList == true)
            {
                CarpetaCopia(fecha + "  " + TextoFin);
                if (parentForm.Puerto2.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Salida);
                    parentForm.Invoke(d, new object[] { TextoFin, fecha });
                }
                else
                {
                    parentForm.Puerto2.Items.Add(fecha + "  " + TextoFin);
                    parentForm.Puerto2.TopIndex = parentForm.Puerto2.Items.Count - 1;
                }
            }
        }

        public void CreaTxt(string text)
        {
            FechaYa = DateTime.Now;
            string Fecha = FechaYa.ToString("dd-MM-yyyy H-mm-ss");

            try
            {
                if (!Directory.Exists(PuertoDoc))
                {
                    Directory.CreateDirectory(PuertoDoc);
                }

                if (Directory.Exists(SubCarpetaDoc))
                {
                    CreaArchivo(text, Fecha);
                }
                else
                {
                    Directory.CreateDirectory(SubCarpetaDoc);
                    CreaArchivo(text, Fecha);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Error al generar o leer la carpeta Puerto 2 en la dirección de almacenamiento de los datos");
                LogErrores(horas + " Error al generar o leer la carpeta Registro en la dirección de almacenamiento de los datos");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Error al generar o leer la carpeta Registro en la dirección de almacenamiento de los datos, más información:  \nl\nl" + e.ToString() + "\nl\nlRecuperación de dato: " + Fecha + "   " + text;
                parentForm.EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde archivos individuales: " + horas + "  " + text);
            }

        }

        private void CreaArchivo(string text, string fecha)
        {
            try
            {
                ArchivoDoc = Path.Combine(SubCarpetaDoc, fecha + ".txt");
                if (File.Exists(ArchivoDoc))
                {
                    int i = 0;
                    while (File.Exists(ArchivoDoc))
                    {
                        i++;
                        ArchivoDoc = Path.Combine(SubCarpetaDoc, fecha + "(" + i + ")" + ".txt");
                    }
                    using (StreamWriter escritor = new StreamWriter(ArchivoDoc))
                    {
                        escritor.WriteLine(text);
                        escritor.Close();
                    }
                }
                else
                {
                    using (StreamWriter escritor = new StreamWriter(ArchivoDoc))
                    {
                        escritor.WriteLine(text);
                        escritor.Close();
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Error al generar los archivos de texto individuales");
                LogErrores(horas + " Error al generar los archivos de texto individuales");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 1: Error al generar los archivos de texto individuales, más información:  \nl\nl" + e.ToString() + "\nl\nlRecuperación de dato: " + fecha + "   " + text;
                parentForm.EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde archivos individuales: " + horas + "  " + text);
            }
        }



        #endregion

        #region Tipo de llamada

        public void CargaConfigSQL()
        {
            try
            {
                bool LlamadasE = false;
                bool LlamadasS = false;
                bool LlamadasI = false;

                foreach (string linea in ProP2)
                {
                    if ((linea.Equals("Llamadas entrantes:") || LlamadasE == true) && !linea.Equals("Llamadas salientes:"))
                    {
                        LlamadasE = true;
                        if (!linea.Equals("Llamadas entrantes:"))
                        {
                            ConfigE.Add(linea);
                        }
                    }
                    if ((linea.Equals("Llamadas salientes:") || LlamadasS == true) && !linea.Equals("Llamadas internas:"))
                    {
                        LlamadasE = false;
                        LlamadasS = true;
                        if (!linea.Equals("Llamadas salientes:"))
                        {
                            ConfigS.Add(linea);
                        }
                    }
                    if (linea.Equals("Llamadas internas:") || LlamadasI == true)
                    {
                        LlamadasE = false;
                        LlamadasS = false;
                        LlamadasI = true;
                        if (!linea.Equals("Llamadas internas:"))
                        {
                            ConfigI.Add(linea);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al separar los tipos de llamadas, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no podrá iniciar, revise el archivo ConfiguracionSQL.txt");
                Contraseña.Sal = 1;
                Application.Exit();
            }

            string confige = "";
            string configs = "";
            string configi = "";

            try
            {
                foreach (string s in ConfigE)
                {
                    if (s[0].Equals('l'))
                    {
                        string[] Partes = s.Split('-');
                        confige = Partes[1];
                    }
                }
                foreach (string s in ConfigS)
                {
                    if (s[0].Equals('l'))
                    {
                        string[] Partes = s.Split('-');
                        configs = Partes[1];
                    }
                }
                foreach (string s in ConfigI)
                {
                    if (s[0].Equals('l'))
                    {
                        string[] Partes = s.Split('-');
                        configi = Partes[1];
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No se ha detectado el campo \"Tipo de llamada\" revise el archivo ConfiguracionSQL.txt, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no se podrá iniciar");
                Contraseña.Sal = 1;
                Application.Exit();
            }
            
            try
            {
                if (confige.Equals(configs) && confige.Equals(configi) && configs.Equals(configi))
                {
                    if (RecepDatos.Config[20].Equals("1"))
                    {
                        TipoLlampos = configs.Split(':');
                    }
                    else if (RecepDatos.Config[20].Equals("2"))
                    {
                        TipoLlamPos = confige;
                    }
                    Igual = true;
                }
                else
                {
                    if (RecepDatos.Config[20].Equals("1"))
                    {
                        TipoLlamposE = confige.Split(':');
                        TipoLlamposS = configs.Split(':');
                        TipoLlamposI = configi.Split(':');
                    }
                    else if (RecepDatos.Config[20].Equals("2"))
                    {
                        TipoLlamPosE = confige;
                        TipoLlamPosS = configs;
                        TipoLlamPosI = configi;
                    }
                    Igual = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al detectar la posición del tipo de llamada, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no se podrá iniciar");
                Contraseña.Sal = 1;
                Application.Exit();
            }

            try
            {
                query = "Select * From formatos Where Tipo_Formato = 'l'";
                comando = new MySqlCommand(query, RecepDatos.Conexion);

                MySqlDataReader lee = comando.ExecuteReader();

                while (lee.Read())
                {
                    TiposLlamada.Add(lee["Strg_Formato"].ToString());
                    TiposLlamada2.Add(lee["Deta_Formato"].ToString());
                }
                lee.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al precargar los tipos de llamada, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no se podrá iniciar");
                Contraseña.Sal = 1;
                Application.Exit();
            }
        }

        public void RecibeCadCar(string Cadena)
        {
            CreaArchivos = true;
            Tipo = null;
            i = 0;
            try
            {
                if (Igual == true)
                {
                    TipoLlamadaCad = Cadena.Substring(Convert.ToInt32(TipoLlampos[0]), Convert.ToInt32(TipoLlampos[1]));
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(TipoLlamadaCad))
                        {
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    ProcesaPosCar(Tipo, Cadena);
                }
                else
                {
                    Veces = 0;
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(Cadena.Substring(Convert.ToInt32(TipoLlamposE[0]), Convert.ToInt32(TipoLlamposE[1]))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Substring(Convert.ToInt32(TipoLlamposS[0]), Convert.ToInt32(TipoLlamposS[1]))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Substring(Convert.ToInt32(TipoLlamposI[0]), Convert.ToInt32(TipoLlamposI[1]))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }

                    if (!string.IsNullOrEmpty(Tipo) && Veces == 1)
                    {
                        ProcesaPosCar(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = true;
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        parentForm.pictureBox8.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 2: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\n La cadena se guardará en la carpeta de archivos separados";
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void RecibeCadCamp(string Cadena)
        {
            CreaArchivos = true;
            i = 0;
            Tipo = null;
            try
            {
                if (Igual == true)
                {
                    TipoLlamadaCad = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(TipoLlamPos)];
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(TipoLlamadaCad))
                        {
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    ProcesaPosCamp(Tipo, Cadena);
                }
                else
                {
                    Veces = 0;
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(TipoLlamPosE)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(TipoLlamPosS)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(TipoLlamPosI)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    if (!string.IsNullOrEmpty(Tipo) && Veces == 1)
                    {
                        ProcesaPosCamp(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = true;
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        parentForm.pictureBox8.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 2: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\n La cadena se guardará en la carpeta de archivos separados";
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        #endregion

        #region Carga Datos Cadena
        
        public void ProcesaPosCar(string Tipo, string Cadena)
        {
            IntentosCon = 0;
            try
            {
                if (Tipo.Equals("Llamada entrante"))
                {
                    foreach (string s in ConfigE)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    SubeTabla();

                }
                else if (Tipo.Equals("Llamada saliente"))
                {
                    foreach (string s in ConfigS)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    SubeTabla();
                }
                else if (Tipo.Equals("Llamada interna"))
                {
                    foreach (string s in ConfigI)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    SubeTabla();
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void ProcesaPosCamp(string Tipo, string Cadena)
        {
            IntentosCon = 0;
            try
            {
                if (Tipo.Equals("Llamada entrante"))
                {
                    foreach (string s in ConfigE)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }
                    SubeTabla();
                }
                else if (Tipo.Equals("Llamada saliente"))
                {
                    foreach (string s in ConfigS)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }
                    SubeTabla();
                }
                else if (Tipo.Equals("Llamada interna"))
                {
                    foreach (string s in ConfigI)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }
                    SubeTabla();
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }
        #endregion

        #region Sube Tabla

        public void AgregaValoresCar(string Cadena, string s)
        {
            try
            {
                if (s[0].Equals('F'))
                {
                    F_Fecha_Final_Lamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('H'))
                {
                    H_Hora_Final_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('E'))
                {
                    E_Extension = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('T'))
                {
                    T_Troncal = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('N'))
                {
                    N_Numero_Marcado = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('D'))
                {
                    D_Duracion = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('P'))
                {
                    P_Codigo_Personal = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('m'))
                {
                    m_Fecha_Inicial_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('j'))
                {
                    j_Hora_Inicial_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('l'))
                {
                    l_Tipo_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('R'))
                {
                    R_Trafico_Interno_Externo = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ha ocurrido un error al cargar los datos de la cadena en memoria, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void AgregaValoresCamp(string Cadena, string s)
        {
            try
            {
                if (s[0].Equals('F'))
                {
                    F_Fecha_Final_Lamada = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('H'))
                {
                    H_Hora_Final_Llamada = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('E'))
                {
                    E_Extension = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('T'))
                {
                    T_Troncal = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('N'))
                {
                    N_Numero_Marcado = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('D'))
                {
                    D_Duracion = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('P'))
                {
                    P_Codigo_Personal = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('m'))
                {
                    m_Fecha_Inicial_Llamada = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('j'))
                {
                    j_Hora_Inicial_Llamada = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('l'))
                {
                    l_Tipo_Llamada = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('R'))
                {
                    R_Trafico_Interno_Externo = Cadena.Split(RecepDatos.Config[23][0])[Convert.ToInt32(s.Split('-')[1])];
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ha ocurrido un error al cargar los datos de la cadena en memoria, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void SubeTabla()
        {
            try
            {
                query = @"insert into llamadas_telefonicas (FFechaFinalLlamada, HHoraFinalLlamada, EExtension,
                        TTroncal, NNumeroMarcado, DDuracion, PCodigoPersonal, mFechaInicialLlamada, jHoraInicialLlamada,
                        lTipoLlamada, RTraficoInternoExterno) values (?FF, ?HF, ?EE, ?TT, ?NN, ?DD, ?CP, ?mF, ?jH, ?lT, ?RT)";
                comando = new MySqlCommand(query, RecepDatos.Conexion);
                comando.Parameters.AddWithValue("?FF", F_Fecha_Final_Lamada);
                comando.Parameters.AddWithValue("?HF", H_Hora_Final_Llamada);
                comando.Parameters.AddWithValue("?EE", E_Extension);
                comando.Parameters.AddWithValue("?TT", T_Troncal);
                comando.Parameters.AddWithValue("?NN", N_Numero_Marcado);
                comando.Parameters.AddWithValue("?DD", D_Duracion);
                comando.Parameters.AddWithValue("?CP", P_Codigo_Personal);
                comando.Parameters.AddWithValue("?mF", m_Fecha_Inicial_Llamada);
                comando.Parameters.AddWithValue("?jH", j_Hora_Inicial_Llamada);
                comando.Parameters.AddWithValue("?lT", l_Tipo_Llamada);
                comando.Parameters.AddWithValue("?RT", R_Trafico_Interno_Externo);
                comando.ExecuteNonQuery();
                Reset();
                parentForm.pictureBox11.BackColor = Color.Lime;
                CreaArchivos = false;
            }
            catch (Exception e)
            {
                bTimer.Enabled = true;
                parentForm.pictureBox11.BackColor = Color.Red;
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Ocurrió un error al subir la cadena en la base de datos, Revisar correo para más información");
                LogErrores(horas + " Ocurrió un error al subir la cadena en la base de datos, Revisar correo para más información");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ocurrió un error al subir la cadena en la base de datos, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
                Reset();
            }
        }

        public void Reset()
        {
            F_Fecha_Final_Lamada = "-";
            H_Hora_Final_Llamada = "-";
            E_Extension = "-";
            T_Troncal = "-";
            N_Numero_Marcado = "-";
            D_Duracion = "-";
            P_Codigo_Personal = "-";
            m_Fecha_Inicial_Llamada = "-";
            j_Hora_Inicial_Llamada = "-";
            l_Tipo_Llamada = "-";
            R_Trafico_Interno_Externo = "-";
        }
        #endregion

        #region CopiaSeguridad

        public void CarpetaCopia(string text)
        {
            try
            {
                DireccionCopi = RecepDatos.Config[2];

                CarpetaCopi = DireccionCopi;
                PuertoDoc = System.IO.Path.Combine(CarpetaCopi, "Puerto " + RecepDatos.Config[10]);
                if (!System.IO.Directory.Exists(PuertoDoc))
                {
                    System.IO.Directory.CreateDirectory(PuertoDoc);
                }

                SubCarpetaCopi = System.IO.Path.Combine(PuertoDoc, "Registro mensual");
                if (System.IO.Directory.Exists(SubCarpetaCopi))
                {
                    CreaArchivoCopi(text);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(SubCarpetaCopi);
                    CreaArchivoCopi(text);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Puerto 2: Ocurrió un error al generar o leer la carpeta de registro mensual");
                LogErrores(horas + " Puerto 2: Ocurrió un error al generar o leer la carpeta de registro mensual");
                parentForm.pictureBox8.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Ocurrió un error al generar o leer la carpeta de registro mensual, más información:  \nl\nl" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void CreaArchivoCopi(string text)
        {
            try
            {
                FechaYa = DateTime.Now;
                string NombreMes = FechaYa.ToString("MMMM-yyyy");
                ArchivoCopi = System.IO.Path.Combine(SubCarpetaCopi, NombreMes + ".txt");

                using (StreamWriter w = File.AppendText(ArchivoCopi))
                {
                    Log(text, w);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Puerto 2: Error al crear el archivo de registro mensual");
                parentForm.pictureBox8.BackColor = Color.Red;
                LogErrores(horas + "Puerto 2: Error al crear el archivo de registro mensual");
                Subject = "Error del servidor";
                Body = horas + " Puerto 2: Error al escribir en el archivo de registros mensuales, más información: \n\n" + e.ToString() + "\n\nRecuperación de datos:  " + text;
                parentForm.EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde copia de seguridad: " + text);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine(logMessage);
        }



        #endregion

        #region ArchivosSalvados

        public void ArchivosSalvados(string textsalv)
        {
            if (!System.IO.Directory.Exists(PuertoSalv))
            {
                System.IO.Directory.CreateDirectory(PuertoSalv);
            }

            SubCarpetaSalv = System.IO.Path.Combine(PuertoSalv, "Puerto " + RecepDatos.Config[10]);
            ArchivoSalv = System.IO.Path.Combine(SubCarpetaSalv, "Archivos_Salvados.txt");

            if (!System.IO.File.Exists(ArchivoSalv))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(SubCarpetaSalv);
                    using (StreamWriter w = File.AppendText(ArchivoSalv))
                    {
                        Log(textsalv, w);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Puerto 2: Ocurrió un problema al crear los archivos salvados");
                    FechaYa = DateTime.Now;
                    string horas = FechaYa.ToString();
                    RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Puerto 2: Ocurrió un problema al crear los archivos salvados");
                    LogErrores(horas + " Puerto 2: Ocurrió un problema al crear los archivos salvados");
                    parentForm.pictureBox8.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 2: Ocurrió un problema al crear los archivos salvados, más información:  \nl\nl" + e.ToString() + "\nl\nlArchivo no guardado: " + textsalv;
                    parentForm.EnvioCorreo(Subject, Body);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter w = File.AppendText(ArchivoSalv))
                    {
                        Log(textsalv, w);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Puerto 2: Ocurrió un problema al escribir los archivos salvados");
                    FechaYa = DateTime.Now;
                    string horas = FechaYa.ToString();
                    RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Puerto 2: Ocurrió un problema al escribir los archivos salvados");
                    LogErrores(horas + " Puerto 2: Ocurrió un problema al escribir los archivos salvados");
                    parentForm.pictureBox8.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 2: Ocurrió un problema al crear los archivos salvados, más información:  \nl\nl" + e.ToString() + "\nl\nlArchivo no guardado: " + textsalv;
                    parentForm.EnvioCorreo(Subject, Body);
                }
            }
        }


        #endregion

        #region LogErrores

        public void LogErrores(string mensj)
        {
            if (GeneraIP.Borrar != 1)
            {

                if (!Directory.Exists(PuertoLog))
                {
                    Directory.CreateDirectory(PuertoLog);
                }

                SubCarpetaLog = Path.Combine(PuertoLog, "Puerto " + RecepDatos.Config[10]);
                ArchivoLog = Path.Combine(SubCarpetaLog, "Log_Errores.txt");

                if (!File.Exists(ArchivoLog))
                {
                    try
                    {
                        FechaYa = DateTime.Now;
                        string nuevolog = FechaYa.ToString();
                        Directory.CreateDirectory(SubCarpetaLog);
                        using (StreamWriter w = File.AppendText(ArchivoLog))
                        {
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log("Se ha creado un nuevo archivo log el " + nuevolog, w);
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log(mensj, w);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Puerto 2: Ocurrió un problema al crear el archivo Log de Errores.");
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Puerto 2: Ocurrió un problema al crear el archivo Log de Errores.");
                        parentForm.pictureBox5.BackColor = Color.Red;
                        parentForm.pictureBox8.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 2: Ocurrió un problema al crear el archivo Log de Errores, más información:  \nl\nl" + e.ToString() + "\nl\nl Error no guardado: " + mensj;
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
                else
                {
                    try
                    {
                        using (StreamWriter w = File.AppendText(ArchivoLog))
                        {
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log(mensj, w);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Puerto 2: Ocurrió un problema al escribir en el archivo Log de Errores.");
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto2.Add(horas + " Puerto 2: Ocurrió un problema al escribir en el archivo Log de Errores.");
                        parentForm.pictureBox5.BackColor = Color.Red;
                        parentForm.pictureBox8.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 2: Ocurrió un problema al escribir en el archivo Log de Errores, más información:  \nl\nl" + e.ToString() + "\nl\nl Error no guardado: " + mensj;
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
            }
        }



        #endregion

        #region counter

        private void InitializeTimer()
        {
            counter = 0;
            aTimer.Interval = 60000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
            Minutos = RecepDatos.Config[17].Split('-');
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            counter++;
            if (counter == Convert.ToInt32(Minutos[0]))
            {
                if (parentForm.pictureBox7.BackColor != Color.Red)
                {
                    parentForm.pictureBox7.BackColor = Color.Yellow;
                }
                Subject = "Notificación de inactividad";
                Body = "El puerto 2 ha permanecido " + counter + " minutos inactivo";
                parentForm.EnvioCorreo(Subject, Body);
                RecepDatos.ErroresEntradaDatosPuerto2.Add("El puerto 2 ha permanecido " + counter + " minutos inactivo");
                LogErrores("El puerto 2 ha permanecido " + counter + " minutos inactivo");
            }
            if (counter == Convert.ToInt32(Minutos[1]))
            {
                parentForm.pictureBox7.BackColor = Color.Red;
                Subject = "Alerta de inactividad";
                Body = "El puerto 2 ha permanecido " + counter + " minutos inactivo";
                parentForm.EnvioCorreo(Subject, Body);
                RecepDatos.ErroresEntradaDatosPuerto2.Add("Alerta: el puerto 2 ha permanecido " + counter + " minutos inactivo");
                LogErrores("Alerta: el puerto 2 ha permanecido " + counter + " minutos inactivo");
                counter2 = false;
            }
        }

        #endregion

        #region Counter SQL

        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            if (Directory.Exists(SubCarpetaDoc))
            {
                if (Directory.GetFiles(SubCarpetaDoc).Length != 0)
                {
                    if (RecepDatos.Conexion.State != ConnectionState.Open)
                    {
                        try
                        {
                            RecepDatos.Conexion.Open();
                            parentForm.pictureBox11.BackColor = Color.Lime;
                            CargaDatosFalt();
                        }
                        catch
                        {
                            parentForm.pictureBox11.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        parentForm.pictureBox11.BackColor = Color.Lime;
                        CargaDatosFalt();
                    }
                }
                else
                {
                    bTimer.Enabled = false;
                }
            }
            else
            {
                bTimer.Enabled = false;
            }
        }

        public void CargaDatosFalt()
        {
            bTimer.Enabled = false;
            FechaYa = DateTime.Now;
            string fecha = FechaYa.ToString();
            Cadenas = new List<string>();
            try
            {
                string[] Archivos = Directory.GetFiles(SubCarpetaDoc);
                foreach (string s in Archivos)
                {
                    using (StreamReader lector = new StreamReader(s))
                    {
                        Cadenas.Add(lector.ReadLine());
                        lector.Close();
                    }
                    File.Delete(s);
                }
            }
            catch (Exception e)
            {
                parentForm.pictureBox7.BackColor = Color.Red;
                Subject = "Error en la entrada de datos";
                Body = fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos, más información: \n\n" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                RecepDatos.ErroresEntradaDatosPuerto2.Add(fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos");
                LogErrores(fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos");
                counter2 = false;
            }

            foreach (string s in Cadenas)
            {
                ApList = false;
                if (RecepDatos.Config[20] == "1")
                {
                    CantCar(s, fecha);
                }
                else if (RecepDatos.Config[20] == "2")
                {
                    YaEsp = true;
                    SepCar(s, fecha);
                    YaEsp = false;
                }
                ApList = true;
            }
        }


        #endregion

    }


    #endregion

    #region Puerto 3
    class Puerto3
    {

        #region Inicio

        DateTime FechaYa;

        delegate void SetTextCallback(string TextoFin, string fecha);
        public RecepDatos parentForm;

        //Setup
        private static Socket _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static List<Socket> _clienSockets = new List<Socket>();
        private static byte[] _buffer = new byte[1024];

        //log errores
        public static string DireccionLog = Directory.GetCurrentDirectory();
        public static string CarpetaLog = @DireccionLog;
        public static string PuertoLog = System.IO.Path.Combine(CarpetaLog, "Log de errores");
        public static string SubCarpetaLog = System.IO.Path.Combine(PuertoLog, "Puerto 3");
        public static string ArchivoLog = System.IO.Path.Combine(SubCarpetaLog, "Log_Errores.txt");

        //Envio Correo
        public string Subject;
        public string Body;

        //Timer
        public int counter;
        public bool counter2 = true;
        public System.Timers.Timer aTimer = new System.Timers.Timer();
        public string[] Minutos;

        //Archivos separados
        public static string DireccionDoc = Directory.GetCurrentDirectory();
        public static string CarpetaDoc = DireccionDoc;
        public static string PuertoDoc = System.IO.Path.Combine(CarpetaDoc, "Puerto 3");
        public static string SubCarpetaDoc = System.IO.Path.Combine(PuertoDoc, "Registro individual Puerto 3");
        public static string ArchivoDoc;
        public bool CreaArchivos = true;

        //Copia de seguridad
        public static string DireccionCopi = Directory.GetCurrentDirectory();
        public static string CarpetaCopi = DireccionCopi;
        public static string SubCarpetaCopi = System.IO.Path.Combine(PuertoDoc, "Registro mensual Puerto 3");
        public static string ArchivoCopi;

        //Archivos salvados
        public static string DireccionSalv = Directory.GetCurrentDirectory();
        public static string CarpetaSalv = @DireccionSalv;
        public static string PuertoSalv = System.IO.Path.Combine(CarpetaSalv, "Archivos Salvados");
        public static string SubCarpetaSalv = System.IO.Path.Combine(PuertoSalv, "Archivos Salvados puerto 3");
        public static string ArchivoSalv = System.IO.Path.Combine(SubCarpetaSalv, "Archivos_Salvados.txt");

        //Configuracion SQL
        public List<string> ProP3 = RecepDatos.ProP3;
        public List<string> ConfigE = new List<string>();
        public List<string> ConfigS = new List<string>();
        public List<string> ConfigI = new List<string>();
        public List<string> TiposLlamada = new List<string>();
        public List<string> TiposLlamada2 = new List<string>();
        public bool Igual = false;
        public string TipoLlamPos = null;
        public string TipoLlamPosE = null;
        public string TipoLlamPosS = null;
        public string TipoLlamPosI = null;
        public string[] TipoLlampos = null;
        public string[] TipoLlamposE = null;
        public string[] TipoLlamposS = null;
        public string[] TipoLlamposI = null;
        public string TipoLlamadaCad = null;
        public string Tipo = null;
        public int i = 0;
        public int Veces = 0;

        //Conexion SQL
        public string query;
        public MySqlCommand comando;
        public int IntentosCon = 0;

        //Datos a SQL
        public string F_Fecha_Final_Lamada = "-";
        public string H_Hora_Final_Llamada = "-";
        public string E_Extension = "-";
        public string T_Troncal = "-";
        public string N_Numero_Marcado = "-";
        public string D_Duracion = "-";
        public string P_Codigo_Personal = "-";
        public string m_Fecha_Inicial_Llamada = "-";
        public string j_Hora_Inicial_Llamada = "-";
        public string l_Tipo_Llamada = "-";
        public string R_Trafico_Interno_Externo = "-";

        //Caracteres Especiales
        List<string> P3 = new List<string>();
        bool pu3 = false;
        public bool YaEsp = false;

        //CounterSQL
        System.Timers.Timer bTimer = new System.Timers.Timer();
        List<string> Cadenas;
        public bool ApList = true;

        public void Inicio(RecepDatos parentForms)
        {
            parentForm = parentForms;
            InitializeTimer();
            SetupServer();
            CargaConfigSQL();
        }

        #endregion

        #region Setup
        public void SetupServer()
        {
            P3 = RecepDatos.P3;
            pu3 = RecepDatos.pu3;
            try
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                parentForm.Puerto3.Items.Add("configurando puerto 3...");
                _serverSocket.Bind(new IPEndPoint(IPAddress.Parse(RecepDatos.Config[0]), Convert.ToInt32(RecepDatos.Config[11])));
                _serverSocket.Listen(5);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
                parentForm.Puerto3.Items.Add("Puerto 3 listo");
                LogErrores(horas + " Se ha iniciado el puerto 3");
                Subject = "Estado del servidor";
                Body = horas + " Se ha iniciado el puerto 3";

                parentForm.EnvioCorreo(Subject, Body);

                DireccionDoc = RecepDatos.Config[2];
                CarpetaDoc = DireccionDoc;
                PuertoDoc = Path.Combine(CarpetaDoc, "Puerto " + RecepDatos.Config[11]);
                SubCarpetaDoc = Path.Combine(PuertoDoc, "Registro individual");

                bTimer.Enabled = true;
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                parentForm.Puerto3.Items.Add(horas + " Problema al configurar el puerto 3!");
                RecepDatos.ErroresPuerto3.Add(horas + " Problema al configurar el puerto 3: probablemente la IP no es correcta");
                LogErrores(horas + " Problema al configurar el puerto 3: probablemente la IP no es correcta");
                Subject = "Error del servidor";
                Body = horas + " Problema al configurar el puerto 3: probablemente la IP no es correcta, más información:  \nl\nl" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox6.BackColor = Color.Red;

            }
        }
        private void AcceptCallback(IAsyncResult AR)
        {
            try
            {
                Socket socket = _serverSocket.EndAccept(AR);
                _clienSockets.Add(socket);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
                _serverSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresPuerto3.Add(horas + " Puerto 3: Problema al comunicarse con el cliente (AcceptCallback)");
                LogErrores(horas + " Puerto 3: Problema al comunicarse con el cliente (AcceptCallback)");
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Problema al comunicarse con el cliente (AcceptCallback), más información:  \nl\nl" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox9.BackColor = Color.Red;
                aTimer.Enabled = false;
                counter2 = false;
            }
        }
        private void ReceiveCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            try
            {
                int received = socket.EndReceive(AR);
                byte[] dataBuf = new byte[received];
                Array.Copy(_buffer, dataBuf, received);
                string text = BitConverter.ToString(dataBuf);
                SetText(text);
                socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
            }
            catch
            {
                socket.Dispose();
                socket.Close();
            }
        }
        #endregion

        #region Txt

        public void SetText(string text)
        {
            try
            {
                FechaYa = DateTime.Now;
                string fecha = FechaYa.ToString();
                string TextoFin = "";

                char[] split = { '-' };
                if (text.Length > 3)
                {
                    if (counter2 == true)
                    {
                        parentForm.pictureBox9.BackColor = Color.Lime;
                    }

                    string textofin = "";
                    string[] caracteres = text.Split(split);
                    foreach (string Caracteres in caracteres)
                    {
                        if (Caracteres.ToLower().Equals("0a") || Caracteres.ToLower().Equals("0d"))
                        {
                            if (textofin.Length > 2)
                            {
                                string[] hexValuesSplit = textofin.Split(' ');
                                foreach (String hex in hexValuesSplit)
                                {
                                    int value = Convert.ToInt32(hex, 16);
                                    string stringValue = Char.ConvertFromUtf32(value);
                                    TextoFin = TextoFin + stringValue;
                                }

                                if (RecepDatos.Config[21] == "1")
                                {
                                    CantCar(TextoFin, fecha);
                                }
                                else if (RecepDatos.Config[21] == "2")
                                {
                                    SepCar(TextoFin, fecha);
                                }

                                TextoFin = "";
                                textofin = "";
                            }
                        }
                        else
                        {
                            if (textofin.Equals(""))
                            {
                                textofin = Caracteres;
                            }
                            else
                            {
                                textofin = textofin + " " + Caracteres;
                            }
                        }
                    }
                    if (textofin.Length > 2)
                    {

                        string[] hexValuesSplit2 = textofin.Split(' ');
                        foreach (String hex in hexValuesSplit2)
                        {
                            int value = Convert.ToInt32(hex, 16);
                            string stringValue = Char.ConvertFromUtf32(value);
                            TextoFin = TextoFin + stringValue;
                        }

                        if (RecepDatos.Config[21] == "1")
                        {
                            CantCar(TextoFin, fecha);
                        }
                        else if (RecepDatos.Config[21] == "2")
                        {
                            SepCar(TextoFin, fecha);
                        }

                        TextoFin = "";
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresEntradaDatosPuerto3.Add("Puerto 3: Ocurrió un error al recibir un dato (SetText)" + horas);
                LogErrores(horas + " Puerto 3: Ocurrió un error al recibir un dato (SetText)");
                Subject = "Puerto 3: Error del servidor";
                aTimer.Enabled = false;
                Body = horas + " Puerto 3: Ocurrió un error al recibir un dato (SetText), más información: \n\n" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                parentForm.pictureBox9.BackColor = Color.Red;
                counter2 = false;

            }
        }

        public void CantCar(string TextoFin, string fecha)
        {
            if (TextoFin.Length == Convert.ToUInt32(RecepDatos.Config[15]))
            {
                RecibeCadCar(TextoFin);
                Salida(TextoFin, fecha);
            }
            else
            {
                ErrorCad(TextoFin, fecha);
            }
        }

        public void SepCar(string TextoFin, string fecha)
        {
            int Campos = 0;
            if (pu3 == true && YaEsp == false)
            {
                try
                {
                    foreach (string s in P3)
                    {
                        TextoFin = TextoFin.Replace(s[1], RecepDatos.Config[24][0]);
                    }
                }
                catch (Exception r)
                {
                    FechaYa = DateTime.Now;
                    string horas = FechaYa.ToString();
                    RecepDatos.ErroresEntradaDatosPuerto3.Add("Puerto 3: Ocurrió un error al reemplazar el caracter especial" + horas);
                    LogErrores(horas + " Puerto 3: Ocurrió un error al reemplazar el caracter especial");
                    Subject = "Puerto 3: Error del servidor";
                    aTimer.Enabled = false;
                    Body = horas + " Puerto 3: Ocurrió un error al reemplazar el caracter especial, más información: \n\n" + r.ToString();
                    parentForm.EnvioCorreo(Subject, Body);
                    parentForm.pictureBox9.BackColor = Color.Red;
                    counter2 = false;
                }
            }
            string[] Partes = TextoFin.Split(RecepDatos.Config[24][0]);
            foreach (var s in Partes)
            {
                Campos++;
            }
            if (Campos == Convert.ToInt32(RecepDatos.Config[15]))
            {
                RecibeCadCamp(TextoFin);
                Salida(TextoFin, fecha);
            }
            else
            {
                ErrorCad(TextoFin, fecha);
            }
        }

        public void ErrorCad(string TextoFin, string fecha)
        {
            FechaYa = DateTime.Now;
            string horas = FechaYa.ToString();
            RecepDatos.ErroresEntradaDatosPuerto3.Add(horas + " Puerto 3: Se ha recibido un dato erroneo: Revisar correo.");
            parentForm.pictureBox9.BackColor = Color.Red;
            counter2 = false;
            aTimer.Enabled = false;
            TextoFin = fecha + "  " + TextoFin;
            LogErrores("Puerto 3: Se ha recibido una entrada no válida:  " + TextoFin);
            Subject = "Puerto 3: Entrada errónea";
            Body = ("Puerto 3: Se ha recibido una entrada no válida:  \n" + TextoFin);
            parentForm.EnvioCorreo(Subject, Body);
        }

        public void Salida(string TextoFin, string fecha)
        {
            if (CreaArchivos == true)
            {
                CreaTxt(TextoFin);
            }

            if (ApList == true)
            {
                CarpetaCopia(fecha + "  " + TextoFin);
                if (parentForm.Puerto3.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(Salida);
                    parentForm.Invoke(d, new object[] { TextoFin });
                }
                else
                {
                    parentForm.Puerto3.Items.Add(fecha + "  " + TextoFin);
                    parentForm.Puerto3.TopIndex = parentForm.Puerto3.Items.Count - 1;
                }
            }
        }

        public void CreaTxt(string text)
        {
            FechaYa = DateTime.Now;
            string Fecha = FechaYa.ToString("dd-MM-yyyy H-mm-ss");

            try
            {
                if (!Directory.Exists(PuertoDoc))
                {
                    Directory.CreateDirectory(PuertoDoc);
                }

                if (Directory.Exists(SubCarpetaDoc))
                {
                    CreaArchivo(text, Fecha);
                }
                else
                {
                    Directory.CreateDirectory(SubCarpetaDoc);
                    CreaArchivo(text, Fecha);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Error al generar o leer la carpeta Puerto 3 en la dirección de almacenamiento de los datos");
                LogErrores(horas + " Error al generar o leer la carpeta Registro en la dirección de almacenamiento de los datos");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Error al generar o leer la carpeta Registro en la dirección de almacenamiento de los datos, más información:  \nl\nl" + e.ToString() + "\nl\nlRecuperación de dato: " + Fecha + "   " + text;
                parentForm.EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde archivos individuales: " + horas + "  " + text);
            }

        }

        private void CreaArchivo(string text, string fecha)
        {
            try
            {
                ArchivoDoc = Path.Combine(SubCarpetaDoc, fecha + ".txt");
                if (File.Exists(ArchivoDoc))
                {
                    int i = 0;
                    while (File.Exists(ArchivoDoc))
                    {
                        i++;
                        ArchivoDoc = Path.Combine(SubCarpetaDoc, fecha + "(" + i + ")" + ".txt");
                    }
                    using (StreamWriter escritor = new StreamWriter(ArchivoDoc))
                    {
                        escritor.WriteLine(text);
                        escritor.Close();
                    }
                }
                else
                {
                    using (StreamWriter escritor = new StreamWriter(ArchivoDoc))
                    {
                        escritor.WriteLine(text);
                        escritor.Close();
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Error al generar los archivos de texto individuales");
                LogErrores(horas + " Error al generar los archivos de texto individuales");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Error al generar los archivos de texto individuales, más información:  \nl\nl" + e.ToString() + "\nl\nlRecuperación de dato: " + fecha + "   " + text;
                parentForm.EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde archivos individuales: " + horas + "  " + text);
            }
        }



        #endregion
        
        #region Tipo de llamada

        public void CargaConfigSQL()
        {
            try
            {
                bool LlamadasE = false;
                bool LlamadasS = false;
                bool LlamadasI = false;

                foreach (string linea in ProP3)
                {
                    if ((linea.Equals("Llamadas entrantes:") || LlamadasE == true) && !linea.Equals("Llamadas salientes:"))
                    {
                        LlamadasE = true;
                        if (!linea.Equals("Llamadas entrantes:"))
                        {
                            ConfigE.Add(linea);
                        }
                    }
                    if ((linea.Equals("Llamadas salientes:") || LlamadasS == true) && !linea.Equals("Llamadas internas:"))
                    {
                        LlamadasE = false;
                        LlamadasS = true;
                        if (!linea.Equals("Llamadas salientes:"))
                        {
                            ConfigS.Add(linea);
                        }
                    }
                    if (linea.Equals("Llamadas internas:") || LlamadasI == true)
                    {
                        LlamadasE = false;
                        LlamadasS = false;
                        LlamadasI = true;
                        if (!linea.Equals("Llamadas internas:"))
                        {
                            ConfigI.Add(linea);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al separar los tipos de llamadas, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no podrá iniciar, revise el archivo ConfiguracionSQL.txt");
                Contraseña.Sal = 1;
                Application.Exit();
            }

            string confige = "";
            string configs = "";
            string configi = "";

            try
            {
                foreach (string s in ConfigE)
                {
                    if (s[0].Equals('l'))
                    {
                        string[] Partes = s.Split('-');
                        confige = Partes[1];
                    }
                }
                foreach (string s in ConfigS)
                {
                    if (s[0].Equals('l'))
                    {
                        string[] Partes = s.Split('-');
                        configs = Partes[1];
                    }
                }
                foreach (string s in ConfigI)
                {
                    if (s[0].Equals('l'))
                    {
                        string[] Partes = s.Split('-');
                        configi = Partes[1];
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("No se ha detectado el campo \"Tipo de llamada\" revise el archivo ConfiguracionSQL.txt, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no se podrá iniciar");
                Contraseña.Sal = 1;
                Application.Exit();
            }

            try
            {
                if (confige.Equals(configs) && confige.Equals(configi) && configs.Equals(configi))
                {
                    if (RecepDatos.Config[21].Equals("1"))
                    {
                        TipoLlampos = configs.Split(':');
                    }
                    else if (RecepDatos.Config[21].Equals("2"))
                    {
                        TipoLlamPos = confige;
                    }
                    Igual = true;
                }
                else
                {
                    if (RecepDatos.Config[21].Equals("1"))
                    {
                        TipoLlamposE = confige.Split(':');
                        TipoLlamposS = configs.Split(':');
                        TipoLlamposI = configi.Split(':');
                    }
                    else if (RecepDatos.Config[21].Equals("2"))
                    {
                        TipoLlamPosE = confige;
                        TipoLlamPosS = configs;
                        TipoLlamPosI = configi;
                    }
                    Igual = false;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al detectar la posición del tipo de llamada, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no se podrá iniciar");
                Contraseña.Sal = 1;
                Application.Exit();
            }

            try
            {
                query = "Select * From formatos Where Tipo_Formato = 'l'";
                comando = new MySqlCommand(query, RecepDatos.Conexion);

                MySqlDataReader lee = comando.ExecuteReader();

                while (lee.Read())
                {
                    TiposLlamada.Add(lee["Strg_Formato"].ToString());
                    TiposLlamada2.Add(lee["Deta_Formato"].ToString());
                }
                lee.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ocurrió un error al precargar los tipos de llamada, más información: \n\n" + e.ToString());
                MessageBox.Show("El programa no se podrá iniciar");
                Contraseña.Sal = 1;
                Application.Exit();
            }
        }

        public void RecibeCadCar(string Cadena)
        {
            CreaArchivos = true;
            i = 0;
            Tipo = null;
            try
            {
                if (Igual == true)
                {
                    TipoLlamadaCad = Cadena.Substring(Convert.ToInt32(TipoLlampos[0]), Convert.ToInt32(TipoLlampos[1]));
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(TipoLlamadaCad))
                        {
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    ProcesaPosCar(Tipo, Cadena);
                }
                else
                {
                    Veces = 0;
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(Cadena.Substring(Convert.ToInt32(TipoLlamposE[0]), Convert.ToInt32(TipoLlamposE[1]))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Substring(Convert.ToInt32(TipoLlamposS[0]), Convert.ToInt32(TipoLlamposS[1]))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Substring(Convert.ToInt32(TipoLlamposI[0]), Convert.ToInt32(TipoLlamposI[1]))))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }

                    if (!string.IsNullOrEmpty(Tipo) && Veces == 1)
                    {
                        ProcesaPosCar(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = true;
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        parentForm.pictureBox10.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 3: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\n La cadena se guardará en la carpeta de archivos separados";
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void RecibeCadCamp(string Cadena)
        {
            CreaArchivos = true;
            i = 0;
            Tipo = null;
            try
            {
                if (Igual == true)
                {
                    TipoLlamadaCad = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(TipoLlamPos)];
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(TipoLlamadaCad))
                        {
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    ProcesaPosCamp(Tipo, Cadena);
                }
                else
                {
                    Veces = 0;
                    foreach (string s in TiposLlamada)
                    {
                        if (s.Equals(Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(TipoLlamPosE)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(TipoLlamPosS)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        else if (s.Equals(Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(TipoLlamPosI)]))
                        {
                            Veces++;
                            Tipo = TiposLlamada2[i];
                        }
                        i++;
                    }
                    if (!string.IsNullOrEmpty(Tipo) && Veces == 1)
                    {
                        ProcesaPosCamp(Tipo, Cadena);
                    }
                    else
                    {
                        CreaArchivos = true;
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        LogErrores(horas + " No se ha podido detectar el tipo de llamada de la cadena, revisar correo para más información");
                        parentForm.pictureBox10.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 3: No se ha podido detectar el tipo de llamada de la cadena: \n\n" + Cadena + "\n\n La cadena se guardará en la carpeta de archivos separados";
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        #endregion

        #region Carga Datos Cadena
        public void ProcesaPosCar(string Tipo, string Cadena)
        {
            IntentosCon = 0;
            try
            {
                if (Tipo.Equals("Llamada entrante"))
                {
                    foreach (string s in ConfigE)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    SubeTabla();

                }
                else if (Tipo.Equals("Llamada saliente"))
                {
                    foreach (string s in ConfigS)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    SubeTabla();
                }
                else if (Tipo.Equals("Llamada interna"))
                {
                    foreach (string s in ConfigI)
                    {
                        AgregaValoresCar(Cadena, s);
                    }
                    SubeTabla();
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void ProcesaPosCamp(string Tipo, string Cadena)
        {
            IntentosCon = 0;
            try
            {
                if (Tipo.Equals("Llamada entrante"))
                {
                    foreach (string s in ConfigE)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }
                    SubeTabla();
                }
                else if (Tipo.Equals("Llamada saliente"))
                {
                    foreach (string s in ConfigS)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }
                    SubeTabla();
                }
                else if (Tipo.Equals("Llamada interna"))
                {
                    foreach (string s in ConfigI)
                    {
                        AgregaValoresCamp(Cadena, s);
                    }
                    SubeTabla();
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ha ocurrido un error al detectar el tipo de llamada en la cadena, Revisar correo para más información, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        #endregion

        #region Sube Tabla

        public void AgregaValoresCar(string Cadena, string s)
        {
            try
            {
                if (s[0].Equals('F'))
                {
                    F_Fecha_Final_Lamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('H'))
                {
                    H_Hora_Final_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('E'))
                {
                    E_Extension = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('T'))
                {
                    T_Troncal = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('N'))
                {
                    N_Numero_Marcado = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('D'))
                {
                    D_Duracion = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('P'))
                {
                    P_Codigo_Personal = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('m'))
                {
                    m_Fecha_Inicial_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('j'))
                {
                    j_Hora_Inicial_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('l'))
                {
                    l_Tipo_Llamada = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
                else if (s[0].Equals('R'))
                {
                    R_Trafico_Interno_Externo = Cadena.Substring(Convert.ToInt32((s.Split('-')[1]).Split(':')[0]), Convert.ToInt32((s.Split('-')[1]).Split(':')[1]));
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ha ocurrido un error al cargar los datos de la cadena en memoria, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void AgregaValoresCamp(string Cadena, string s)
        {
            try
            {
                if (s[0].Equals('F'))
                {
                    F_Fecha_Final_Lamada = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('H'))
                {
                    H_Hora_Final_Llamada = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('E'))
                {
                    E_Extension = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('T'))
                {
                    T_Troncal = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('N'))
                {
                    N_Numero_Marcado = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('D'))
                {
                    D_Duracion = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('P'))
                {
                    P_Codigo_Personal = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('m'))
                {
                    m_Fecha_Inicial_Llamada = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('j'))
                {
                    j_Hora_Inicial_Llamada = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('l'))
                {
                    l_Tipo_Llamada = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
                else if (s[0].Equals('R'))
                {
                    R_Trafico_Interno_Externo = Cadena.Split(RecepDatos.Config[24][0])[Convert.ToInt32(s.Split('-')[1])];
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                LogErrores(horas + " Ha ocurrido un error al cargar los datos de la cadena en memoria, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ha ocurrido un error al cargar los datos de la cadena en memoria, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void SubeTabla()
        {
            try
            {
                query = @"insert into llamadas_telefonicas (FFechaFinalLlamada, HHoraFinalLlamada, EExtension,
                        TTroncal, NNumeroMarcado, DDuracion, PCodigoPersonal, mFechaInicialLlamada, jHoraInicialLlamada,
                        lTipoLlamada, RTraficoInternoExterno) values (?FF, ?HF, ?EE, ?TT, ?NN, ?DD, ?CP, ?mF, ?jH, ?lT, ?RT)";
                comando = new MySqlCommand(query, RecepDatos.Conexion);
                comando.Parameters.AddWithValue("?FF", F_Fecha_Final_Lamada);
                comando.Parameters.AddWithValue("?HF", H_Hora_Final_Llamada);
                comando.Parameters.AddWithValue("?EE", E_Extension);
                comando.Parameters.AddWithValue("?TT", T_Troncal);
                comando.Parameters.AddWithValue("?NN", N_Numero_Marcado);
                comando.Parameters.AddWithValue("?DD", D_Duracion);
                comando.Parameters.AddWithValue("?CP", P_Codigo_Personal);
                comando.Parameters.AddWithValue("?mF", m_Fecha_Inicial_Llamada);
                comando.Parameters.AddWithValue("?jH", j_Hora_Inicial_Llamada);
                comando.Parameters.AddWithValue("?lT", l_Tipo_Llamada);
                comando.Parameters.AddWithValue("?RT", R_Trafico_Interno_Externo);
                comando.ExecuteNonQuery();
                Reset();
                parentForm.pictureBox11.BackColor = Color.Lime;
                CreaArchivos = false;
            }
            catch (Exception e)
            {
                bTimer.Enabled = true;
                parentForm.pictureBox11.BackColor = Color.Red;
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Ocurrió un error al subir la cadena en la base de datos, Revisar correo para más información");
                LogErrores(horas + " Ocurrió un error al subir la cadena en la base de datos, Revisar correo para más información");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ocurrió un error al subir la cadena en la base de datos, más información:  \nl\nl" + e.ToString() + "\n\n La cadena se guardará en la carpeta de archivos separados";
                parentForm.EnvioCorreo(Subject, Body);
                Reset();
            }
        }
        public void Reset()
        {
            F_Fecha_Final_Lamada = "-";
            H_Hora_Final_Llamada = "-";
            E_Extension = "-";
            T_Troncal = "-";
            N_Numero_Marcado = "-";
            D_Duracion = "-";
            P_Codigo_Personal = "-";
            m_Fecha_Inicial_Llamada = "-";
            j_Hora_Inicial_Llamada = "-";
            l_Tipo_Llamada = "-";
            R_Trafico_Interno_Externo = "-";
        }

        #endregion

        #region CopiaSeguridad

        public void CarpetaCopia(string text)
        {
            try
            {
                DireccionCopi = RecepDatos.Config[2];

                CarpetaCopi = DireccionCopi;
                PuertoDoc = System.IO.Path.Combine(CarpetaCopi, "Puerto " + RecepDatos.Config[11]);
                if (!System.IO.Directory.Exists(PuertoDoc))
                {
                    System.IO.Directory.CreateDirectory(PuertoDoc);
                }

                SubCarpetaCopi = System.IO.Path.Combine(PuertoDoc, "Registro mensual");
                if (System.IO.Directory.Exists(SubCarpetaCopi))
                {
                    CreaArchivoCopi(text);
                }
                else
                {
                    System.IO.Directory.CreateDirectory(SubCarpetaCopi);
                    CreaArchivoCopi(text);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Puerto 3: Ocurrió un error al generar o leer la carpeta de registro mensual");
                LogErrores(horas + " Puerto 3: Ocurrió un error al generar o leer la carpeta de registro mensual");
                parentForm.pictureBox10.BackColor = Color.Red;
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Ocurrió un error al generar o leer la carpeta de registro mensual, más información:  \nl\nl" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
            }
        }

        public void CreaArchivoCopi(string text)
        {
            try
            {
                FechaYa = DateTime.Now;
                string NombreMes = FechaYa.ToString("MMMM-yyyy");
                ArchivoCopi = System.IO.Path.Combine(SubCarpetaCopi, NombreMes + ".txt");

                using (StreamWriter w = File.AppendText(ArchivoCopi))
                {
                    Log(text, w);
                }
            }
            catch (Exception e)
            {
                FechaYa = DateTime.Now;
                string horas = FechaYa.ToString();
                RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Puerto 3: Error al crear el archivo de registro mensual");
                parentForm.pictureBox10.BackColor = Color.Red;
                LogErrores(horas + "Puerto 3: Error al crear el archivo de registro mensual");
                Subject = "Error del servidor";
                Body = horas + " Puerto 3: Error al escribir en el archivo de registros mensuales, más información: \n\n" + e.ToString() + "\n\nRecuperación de datos:  " + text;
                parentForm.EnvioCorreo(Subject, Body);
                ArchivosSalvados("Desde copia de seguridad: " + text);
            }
        }

        public static void Log(string logMessage, TextWriter w)
        {
            w.WriteLine(logMessage);
        }



        #endregion

        #region ArchivosSalvados

        public void ArchivosSalvados(string textsalv)
        {
            if (!System.IO.Directory.Exists(PuertoSalv))
            {
                System.IO.Directory.CreateDirectory(PuertoSalv);
            }

            SubCarpetaSalv = System.IO.Path.Combine(PuertoSalv, "Puerto " + RecepDatos.Config[11]);
            ArchivoSalv = System.IO.Path.Combine(SubCarpetaSalv, "Archivos_Salvados.txt");

            if (!System.IO.File.Exists(ArchivoSalv))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(SubCarpetaSalv);
                    using (StreamWriter w = File.AppendText(ArchivoSalv))
                    {
                        Log(textsalv, w);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Puerto 3: Ocurrió un problema al crear los archivos salvados");
                    FechaYa = DateTime.Now;
                    string horas = FechaYa.ToString();
                    RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Puerto 3: Ocurrió un problema al crear los archivos salvados");
                    LogErrores(horas + " Puerto 3: Ocurrió un problema al crear los archivos salvados");
                    parentForm.pictureBox10.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 3: Ocurrió un problema al crear los archivos salvados, más información:  \nl\nl" + e.ToString() + "\nl\nlArchivo no guardado: " + textsalv;
                    parentForm.EnvioCorreo(Subject, Body);
                }
            }
            else
            {
                try
                {
                    using (StreamWriter w = File.AppendText(ArchivoSalv))
                    {
                        Log(textsalv, w);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Puerto 3: Ocurrió un problema al escribir los archivos salvados");
                    FechaYa = DateTime.Now;
                    string horas = FechaYa.ToString();
                    RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Puerto 3: Ocurrió un problema al escribir los archivos salvados");
                    LogErrores(horas + " Puerto 3: Ocurrió un problema al escribir los archivos salvados");
                    parentForm.pictureBox10.BackColor = Color.Red;
                    Subject = "Error del servidor";
                    Body = horas + " Puerto 3: Ocurrió un problema al crear los archivos salvados, más información:  \nl\nl" + e.ToString() + "\nl\nlArchivo no guardado: " + textsalv;
                    parentForm.EnvioCorreo(Subject, Body);
                }
            }
        }


        #endregion

        #region LogErrores

        public void LogErrores(string mensj)
        {
            if (GeneraIP.Borrar != 1)
            {

                if (!Directory.Exists(PuertoLog))
                {
                    Directory.CreateDirectory(PuertoLog);
                }

                SubCarpetaLog = Path.Combine(PuertoLog, "Puerto " + RecepDatos.Config[11]);
                ArchivoLog = Path.Combine(SubCarpetaLog, "Log_Errores.txt");

                if (!File.Exists(ArchivoLog))
                {
                    try
                    {
                        FechaYa = DateTime.Now;
                        string nuevolog = FechaYa.ToString();
                        Directory.CreateDirectory(SubCarpetaLog);
                        using (StreamWriter w = File.AppendText(ArchivoLog))
                        {
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log("Se ha creado un nuevo archivo log el " + nuevolog, w);
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log(mensj, w);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Puerto 3: Ocurrió un problema al crear el archivo Log de Errores.");
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Puerto 3: Ocurrió un problema al crear el archivo Log de Errores.");
                        parentForm.pictureBox10.BackColor = Color.Red;
                        parentForm.pictureBox6.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 3: Ocurrió un problema al crear el archivo Log de Errores, más información:  \nl\nl" + e.ToString() + "\nl\nl Error no guardado: " + mensj;
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
                else
                {
                    try
                    {
                        using (StreamWriter w = File.AppendText(ArchivoLog))
                        {
                            Log("-----------------------------------------------------------------------------------------", w);
                            Log(mensj, w);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Puerto 3: Ocurrió un problema al escribir en el archivo Log de Errores.");
                        FechaYa = DateTime.Now;
                        string horas = FechaYa.ToString();
                        RecepDatos.ErroresSaliDatosPuerto3.Add(horas + " Puerto 3: Ocurrió un problema al escribir en el archivo Log de Errores.");
                        parentForm.pictureBox10.BackColor = Color.Red;
                        parentForm.pictureBox6.BackColor = Color.Red;
                        Subject = "Error del servidor";
                        Body = horas + " Puerto 3: Ocurrió un problema al escribir en el archivo Log de Errores, más información:  \nl\nl" + e.ToString() + "\nl\nl Error no guardado: " + mensj;
                        parentForm.EnvioCorreo(Subject, Body);
                    }
                }
            }
        }



        #endregion

        #region counter

        private void InitializeTimer()
        {
            counter = 0;
            aTimer.Interval = 60000;
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Enabled = true;
            Minutos = RecepDatos.Config[18].Split('-');
        }
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            counter++;
            if (counter == Convert.ToInt32(Minutos[0]))
            {
                if (parentForm.pictureBox9.BackColor != Color.Red)
                {
                    parentForm.pictureBox9.BackColor = Color.Yellow;
                }

                Subject = "Notificación de inactividad";
                Body = "El puerto 3 ha permanecido " + counter + " minutos inactivo";
                parentForm.EnvioCorreo(Subject, Body);
                RecepDatos.ErroresEntradaDatosPuerto3.Add("El puerto 3 ha permanecido " + counter + " minutos inactivo");
                LogErrores("El puerto 3 ha permanecido " + counter + " minutos inactivo");
            }
            if (counter == Convert.ToInt32(Minutos[1]))
            {
                parentForm.pictureBox9.BackColor = Color.Red;
                Subject = "Alerta de inactividad";
                Body = "El puerto 3 ha permanecido " + counter + " minutos inactivo";
                parentForm.EnvioCorreo(Subject, Body);
                RecepDatos.ErroresEntradaDatosPuerto3.Add("Alerta: el puerto 3 ha permanecido " + counter + " minutos inactivo");
                LogErrores("Alerta: el puerto 3 ha permanecido " + counter + " minutos inactivo");
                counter2 = false;
            }
        }

        #endregion

        #region Counter SQL

        private void OnTimedEvent2(object source, ElapsedEventArgs e)
        {
            if (Directory.Exists(SubCarpetaDoc))
            {
                if (Directory.GetFiles(SubCarpetaDoc).Length != 0)
                {
                    if (RecepDatos.Conexion.State != ConnectionState.Open)
                    {
                        try
                        {
                            RecepDatos.Conexion.Open();
                            parentForm.pictureBox11.BackColor = Color.Lime;
                            CargaDatosFalt();
                        }
                        catch
                        {
                            parentForm.pictureBox11.BackColor = Color.Red;
                        }
                    }
                    else
                    {
                        parentForm.pictureBox11.BackColor = Color.Lime;
                        CargaDatosFalt();
                    }
                }
                else
                {
                    bTimer.Enabled = false;
                }
            }
            else
            {
                bTimer.Enabled = false;
            }
        }

        public void CargaDatosFalt()
        {
            bTimer.Enabled = false;
            FechaYa = DateTime.Now;
            string fecha = FechaYa.ToString();
            Cadenas = new List<string>();
            try
            {
                string[] Archivos = Directory.GetFiles(SubCarpetaDoc);
                foreach (string s in Archivos)
                {
                    using (StreamReader lector = new StreamReader(s))
                    {
                        Cadenas.Add(lector.ReadLine());
                        lector.Close();
                    }
                    File.Delete(s);
                }
            }
            catch (Exception e)
            {
                parentForm.pictureBox9.BackColor = Color.Red;
                Subject = "Error en la entrada de datos";
                Body = fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos, más información: \n\n" + e.ToString();
                parentForm.EnvioCorreo(Subject, Body);
                RecepDatos.ErroresEntradaDatosPuerto3.Add(fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos");
                LogErrores(fecha + "Ha ocurrido un error al re-subir archivos faltantes a la base de datos");
                counter2 = false;
            }

            foreach (string s in Cadenas)
            {
                ApList = false;
                if (RecepDatos.Config[21] == "1")
                {
                    CantCar(s, fecha);
                }
                else if (RecepDatos.Config[21] == "2")
                {
                    YaEsp = true;
                    SepCar(s, fecha);
                    YaEsp = false;
                }
                ApList = true;
            }
        }


        #endregion
    }
    #endregion
}