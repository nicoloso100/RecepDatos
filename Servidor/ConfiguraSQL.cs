using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.IO;
using System.Data.SqlClient;

namespace Servidor
{
    public partial class ConfiguraSQL : Form
    {

        #region inicio

        //conexion SQL
        public string Conexion;

        //Cadena
        public float SizeCad;

        //Archivos de configuración
        public static string Direccion = Directory.GetCurrentDirectory();
        public static string Carpeta = @Direccion;
        public static string SubCarpeta = System.IO.Path.Combine(Carpeta, "Configuracion");
        public static string Archivo = System.IO.Path.Combine(SubCarpeta, "Configuracion.txt");
        public static List<string> Config = new List<string>();

        //Configuracion SQL
        public static string ArchivoSQL = System.IO.Path.Combine(SubCarpeta, "ConfiguracionSQL.txt");

        //SQL
        public string Seleccion;
        public static string NoCadena;

        //Selecciones llamadas entrantes
        string ELD = "D->" + "Duración" + "-" + "No asignado";
        string ELE = "E->" + "Extensión" + "-" + "No asignado";
        string ELF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
        string ELH = "H->" + "Hora final de llamada" + "-" + "No asignado";
        string ELj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
        string ELl = "l->" + "Tipo de llamada" + "-" + "No asignado";
        string ELm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
        string ELN = "N->" + "Número marcado" + "-" + "No asignado";
        string ELP = "P->" + "Código proyecto" + "-" + "No asignado";
        string ELR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
        string ELT = "T->" + "Troncal" + "-" + "No asignado";

        //Selecciones llamadas salientes

        string SLD = "D->" + "Duración" + "-" + "No asignado";
        string SLE = "E->" + "Extensión" + "-" + "No asignado";
        string SLF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
        string SLH = "H->" + "Hora final de llamada" + "-" + "No asignado";
        string SLj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
        string SLl = "l->" + "Tipo de llamada" + "-" + "No asignado";
        string SLm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
        string SLN = "N->" + "Número marcado" + "-" + "No asignado";
        string SLP = "P->" + "Código proyecto" + "-" + "No asignado";
        string SLR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
        string SLT = "T->" + "Troncal" + "-" + "No asignado";

        //llamadas internas

        string ILD = "D->" + "Duración" + "-" + "No asignado";
        string ILE = "E->" + "Extensión" + "-" + "No asignado";
        string ILF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
        string ILH = "H->" + "Hora final de llamada" + "-" + "No asignado";
        string ILj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
        string ILl = "l->" + "Tipo de llamada" + "-" + "No asignado";
        string ILm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
        string ILN = "N->" + "Número marcado" + "-" + "No asignado";
        string ILP = "P->" + "Código proyecto" + "-" + "No asignado";
        string ILR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
        string ILT = "T->" + "Troncal" + "-" + "No asignado";


        //tabcontrol
        bool reset = false;

        //Cadenas
        string LlamEn;
        string LlamSal;
        string LlamInt;
        char Caracter;

        //Salir
        public static bool Salir = false;

        //Procesamiento
        string ProcesarE;
        string ProcesarS;
        string ProcesarI;

        //Caracteres especiales
        public static string ArchivoCarEsp = System.IO.Path.Combine(RecepDatos.SubCarpeta, "Caracteres_especiales.txt");
        List<string> P1 = new List<string>();
        List<string> P2 = new List<string>();
        List<string> P3 = new List<string>();
        bool p1 = false;
        bool p2 = false;
        bool p3 = false;

        public ConfiguraSQL()
        {
            InitializeComponent();
            LeeConfig();
            CaracteresEpeciales();
            this.FormClosing += ConfiguraSQL_FormClosing;
        }

        private void ConfiguraSQL_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Salir == false)
            {
                Contraseña.Sal = 3;
                Application.Exit();
            }
        }

        #endregion
        
        #region Caracteres Especiales

        public void CaracteresEpeciales()
        {
            string Line = null;
            bool Puerto1 = false;
            bool Puerto2 = false;
            bool Puerto3 = false;

            if (System.IO.File.Exists(ArchivoCarEsp))
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
                    }
                    lector.Close();
                }
                if (P1.Count != 0)
                {
                    p1 = true;
                }
                if (P2.Count != 0)
                {
                    p2 = true;
                }
                if (P3.Count != 0)
                {
                    p3 = true;
                }
            }
        }

        #endregion

        #region Conexion a SQL

        private void button1_Click(object sender, EventArgs e)
        {

            string server = "server=" + textBox1.Text + ";";
            string port = "port=" + textBox2.Text + ";";
            string database = "database=" + textBox5.Text + ";";
            string Uid = "Uid=" + textBox3.Text + ";";
            string pwd = "pwd=" + textBox4.Text + ";";
            Conexion = server + port + database + Uid + pwd;

            if (button1.Text.Equals("Conectar"))
            {
                try
                {
                    using (MySqlConnection conexion = new MySqlConnection(Conexion))
                    {
                        conexion.Open();
                        label6.Text = "Conectado!";
                        button1.Text = "Desconectar";
                        MessageBox.Show("Conectado correctamente!");
                        conexion.Close();
                    }
                    EscrSQL();
                    SQLCad();
                }
                catch
                {
                    label6.Text = "Problema al conectar!";
                    MessageBox.Show("Ocurrió un problema al conectarse!");
                    button2.Visible = true;
                }
            }
            else if (button1.Text.Equals("Desconectar"))
            {
                button1.Text = "Conectar";
                label6.Text = "Desconectado!";
            }

        }
        public void LeeConfig()
        {
            string Line;
            using (System.IO.StreamReader file = new System.IO.StreamReader(Archivo))
            {
                while ((Line = file.ReadLine()) != null)
                {
                    Config.Add(Line);
                }
                file.Close();
            }
            CarConfig();
        }

        public void CarConfig()
        {
            try
            {
                string[] Partes = Config[25].Split(';');
                string[] partes = Partes[0].Split('=');
                textBox1.Text = partes[1];
                partes = Partes[1].Split('=');
                textBox2.Text = partes[1];
                partes = Partes[3].Split('=');
                textBox3.Text = partes[1];
                partes = Partes[4].Split('=');
                textBox4.Text = partes[1];
                partes = Partes[2].Split('=');
                textBox5.Text = partes[1];
            }
            catch
            {
                MessageBox.Show(NoCadena);
            }
        }

        public void EscrSQL()
        {
            if (System.IO.File.Exists(Archivo))
            {
                try
                {
                    if (!Config[25].Equals(Conexion))
                    {
                        try
                        {
                            Config[25] = Conexion;
                            System.IO.Directory.CreateDirectory(SubCarpeta);
                            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(Archivo))
                            {
                                for(int i = 0; i < Config.Count; i++)
                                {
                                    escritor.WriteLine(Config[i]);
                                }
                            }
                            
                        }
                        catch
                        {
                            MessageBox.Show("Ha ocurrido un problema al escibir en el archivo configuración.txt");
                            Application.Exit();
                        }
                    }
                }
                catch
                {
                    try
                    {
                        System.IO.Directory.CreateDirectory(SubCarpeta);
                        using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(Archivo, true))
                        {
                            escritor.WriteLine(Conexion);
                        }
                    }
                    catch
                    {
                        MessageBox.Show("Ha ocurrido un problema al escibir en el archivo configuración.txt");
                        Application.Exit();
                    }
                }
            }
            else
            {
                MessageBox.Show("Ha ocurrido un problema al encontrar Configuracion.txt, asegúrese de que el archivo exista");
                Application.Exit();
            }
        }

        public void SQLCad()
        {
            if (!System.IO.File.Exists(ArchivoSQL))
            {
                tabControl1.SelectTab(1);
            }
            else
            {
                MessageBox.Show("Se ha detectado una configuración de conexión a SQL, si desea cambiar la configuración de SQL, diríjase a la configuración del programa y luego a configuración SQL");
                Salir = true;
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            label6.Text = "";
        }

        #endregion

        #region Puertos
        
        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.DataSource = null;
            listBox2.DataSource = null;
            openFileDialog1.ShowDialog();
            textBox6.Text = openFileDialog1.FileName;
            listBox1.Visible = true;
            listBox2.Visible = true;
            label12.Visible = true;
            label13.Visible = true;
            groupBox3.Enabled = true;
            groupBox4.Enabled = true;
            if (tabControl1.SelectedIndex == 1)
            {
                if (Config[19].Equals("1"))
                {
                    label10.Visible = true;
                    label15.Visible = false;
                    textBox8.Visible = false;
                }
                else
                {
                    label15.Visible = true;
                    textBox8.Visible = true;
                    label10.Visible = false;
                }
            }
            else if (tabControl1.SelectedIndex == 2)
            {
                if (Config[20].Equals("1"))
                {
                    label10.Visible = true;
                    label15.Visible = false;
                    textBox8.Visible = false;
                }
                else
                {
                    label15.Visible = true;
                    textBox8.Visible = true;
                    label10.Visible = false;
                }
            }
            else if (tabControl1.SelectedIndex == 3)
            {
                if (Config[21].Equals("1"))
                {
                    label10.Visible = true;
                    label15.Visible = false;
                    textBox8.Visible = false;
                }
                else
                {
                    label15.Visible = true;
                    textBox8.Visible = true;
                    label10.Visible = false;
                }
            }

            try
            {
                string muestra;
                using (System.IO.StreamReader cad = new System.IO.StreamReader(textBox6.Text))
                {
                    muestra = cad.ReadLine();
                    cad.Close();
                }

                label11.TextAlign = ContentAlignment.MiddleLeft;

                try {
                    if (tabControl1.SelectedIndex == 1 && p1 == true)
                    {
                        foreach (string s in P1)
                        {
                            muestra = muestra.Replace(s[1], Config[22][0]);
                        }
                    }
                    else if (tabControl1.SelectedIndex == 2 && p2 == true)
                    {
                        foreach (string s in P2)
                        {
                            muestra = muestra.Replace(s[1], Config[23][0]);
                        }
                    }
                    else if (tabControl1.SelectedIndex == 3 && p3 == true)
                    {
                        foreach (string s in P3)
                        {
                            muestra = muestra.Replace(s[1], Config[24][0]);
                        }
                    }
                }
                catch (Exception s)
                {
                    MessageBox.Show("Ocurrió un problema al reemplazar un caracter especial\n\n" + s.ToString());
                }

                label11.Text = muestra;
                if(tabControl2.SelectedIndex == 0)
                {
                    LlamEn = muestra;
                }
                else if(tabControl2.SelectedIndex == 1)
                {
                    LlamSal = muestra;
                }
                else if(tabControl2.SelectedIndex == 2)
                {
                    LlamInt = muestra;
                }
                button4.Enabled = true;
                button5.Enabled = true;
                Leetabla();
            }
            catch
            {
                textBox6.Text = "";
                MessageBox.Show("Seleccione un archivo válido!");
            }
            SizeCad = 8.25F;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label11.Font = new Font(label11.Font.Name, SizeCad+=1.25F, label11.Font.Style);
            LD.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LE.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LF.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LH.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            Lj.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            Ll.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            Lm.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LN.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LP.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LR.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LT.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label11.Font = new Font(label11.Font.Name, SizeCad-=1.255F, label11.Font.Style);
            LD.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LE.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LF.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LH.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            Lj.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            Ll.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            Lm.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LN.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LP.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LR.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LT.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.Enabled == true)
            {
                try
                {
                    button6.Enabled = true;
                    button12.Enabled = true;
                    try {
                        using (MySqlConnection conexion = new MySqlConnection(Conexion))
                        {
                            conexion.Open();
                            MySqlCommand comando = new MySqlCommand("Select * From tipos_formato Where Nomb_Tipo = '" + listBox1.Text + "'", conexion);
                            MySqlDataReader myreader = comando.ExecuteReader();
                            myreader.Read();
                            Seleccion = myreader["Codi_Tipo"].ToString();
                            myreader.Close();

                            MySqlDataAdapter adapter = new MySqlDataAdapter("Select * From formatos Where Tipo_Formato = '" + Seleccion + "'", conexion);
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            listBox2.DataSource = table;
                            listBox2.DisplayMember = "Strg_Formato";
                            adapter.Dispose();
                            conexion.Close();
                        }
                    }
                    catch (Exception s)
                    {
                        MessageBox.Show("Ocurrió un problema al leer la base de datos \n\n" + s.ToString());
                    }
                }
                catch
                {
                    MessageBox.Show("Error al buscar el tipo de formato en la tabla: formatos");
                }

            }
            else
            {
                listBox1.ClearSelected();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string formato = listBox2.Text;
            groupBox1.Enabled = true;
            if (Seleccion.Equals("D"))
            {
                LD.Text = formato;
                radioButtonLD.Text = listBox1.Text;
                radioButtonLD.Enabled = true;
                radioButtonLD.Select();
            }
            else if (Seleccion.Equals("E"))
            {
                LE.Text = formato;
                radioButtonLE.Text = listBox1.Text;
                radioButtonLE.Enabled = true;
                radioButtonLE.Select();
            }
            else if (Seleccion.Equals("F"))
            {
                LF.Text = formato;
                radioButtonLF.Text = listBox1.Text;
                radioButtonLF.Enabled = true;
                radioButtonLF.Select();
            }
            else if (Seleccion.Equals("H"))
            {
                LH.Text = formato;
                radioButtonLH.Text = listBox1.Text;
                radioButtonLH.Enabled = true;
                radioButtonLH.Select();
            }
            else if (Seleccion.Equals("j"))
            {
                Lj.Text = formato;
                radioButtonLj.Text = listBox1.Text;
                radioButtonLj.Enabled = true;
                radioButtonLj.Select();
            }
            else if (Seleccion.Equals("l"))
            {
                Ll.Text = formato;
                radioButtonLl.Text = listBox1.Text;
                radioButtonLl.Enabled = true;
                radioButtonLl.Select();
            }
            else if (Seleccion.Equals("m"))
            {
                Lm.Text = formato;
                radioButtonLm.Text = listBox1.Text;
                radioButtonLm.Enabled = true;
                radioButtonLm.Select();
            }
            else if (Seleccion.Equals("N"))
            {
                LN.Text = formato;
                radioButtonLN.Text = listBox1.Text;
                radioButtonLN.Enabled = true;
                radioButtonLN.Select();
            }
            else if (Seleccion.Equals("P"))
            {
                LP.Text = formato;
                radioButtonLP.Text = listBox1.Text;
                radioButtonLP.Enabled = true;
                radioButtonLP.Select();
            }
            else if (Seleccion.Equals("R"))
            {
                LR.Text = formato;
                radioButtonLR.Text = listBox1.Text;
                radioButtonLR.Enabled = true;
                radioButtonLR.Select();
            }
            else if (Seleccion.Equals("T"))
            {
                LT.Text = formato;
                radioButtonLT.Text = listBox1.Text;
                radioButtonLT.Enabled = true;
                radioButtonLT.Select();
            }

        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (radioButtonLD.Checked)
            {
                radioButtonLD.Enabled = false;
                radioButtonLD.Text = "No seleccionado";
                radioButtonLD.Checked = false;
                LD.Text = "";
                if(tabControl2.SelectedIndex == 0)
                {
                    ELD = "D->" + "Duración" + "-" + "No asignado";
                }
                else if(tabControl2.SelectedIndex == 1)
                {
                    SLD = "D->" + "Duración" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILD = "D->" + "Duración" + "-" + "No asignado";
                }
            }
            else if (radioButtonLE.Checked)
            {
                radioButtonLE.Enabled = false;
                radioButtonLE.Text = "No seleccionado";
                radioButtonLE.Checked = false;
                LE.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELE = "E->" + "Extensión" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLE = "E->" + "Extensión" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILE = "E->" + "Extensión" + "-" + "No asignado";
                }
            }
            else if (radioButtonLF.Checked)
            {
                radioButtonLF.Enabled = false;
                radioButtonLF.Text = "No seleccionado";
                radioButtonLF.Checked = false;
                LF.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
                }
            }
            else if (radioButtonLH.Checked)
            {
                radioButtonLH.Enabled = false;
                radioButtonLH.Text = "No seleccionado";
                radioButtonLH.Checked = false;
                LH.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELH = "H->" + "Hora final de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLH = "H->" + "Hora final de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILH = "H->" + "Hora final de llamada" + "-" + "No asignado";
                }
            }
            else if (radioButtonLj.Checked)
            {
                radioButtonLj.Enabled = false;
                radioButtonLj.Text = "No seleccionado";
                radioButtonLj.Checked = false;
                Lj.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
                }                
            }
            else if (radioButtonLl.Checked)
            {
                radioButtonLl.Enabled = false;
                radioButtonLl.Text = "No seleccionado";
                radioButtonLl.Checked = false;
                Ll.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELl = "l->" + "Tipo de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLl = "l->" + "Tipo de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILl = "l->" + "Tipo de llamada" + "-" + "No asignado";
                }
            }
            else if (radioButtonLm.Checked)
            {
                radioButtonLm.Enabled = false;
                radioButtonLm.Text = "No seleccionado";
                radioButtonLm.Checked = false;
                Lm.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
                }
            }
            else if (radioButtonLN.Checked)
            {
                radioButtonLN.Enabled = false;
                radioButtonLN.Text = "No seleccionado";
                radioButtonLN.Checked = false;
                LN.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELN = "N->" + "Número marcado" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLN = "N->" + "Número marcado" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILN = "N->" + "Número marcado" + "-" + "No asignado";
                }
            }
            else if (radioButtonLP.Checked)
            {
                radioButtonLP.Enabled = false;
                radioButtonLP.Text = "No seleccionado";
                radioButtonLP.Checked = false;
                LP.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELP = "P->" + "Código proyecto" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLP = "P->" + "Código proyecto" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILP = "P->" + "Código proyecto" + "-" + "No asignado";
                }
            }
            else if (radioButtonLR.Checked)
            {
                radioButtonLR.Enabled = false;
                radioButtonLR.Text = "No seleccionado";
                radioButtonLR.Checked = false;
                LR.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
                }
            }
            else if (radioButtonLT.Checked)
            {
                radioButtonLT.Enabled = false;
                radioButtonLT.Text = "No seleccionado";
                radioButtonLT.Checked = false;
                LT.Text = "";
                if (tabControl2.SelectedIndex == 0)
                {
                    ELT = "T->" + "Troncal" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLT = "T->" + "Troncal" + "-" + "No asignado";
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILT = "T->" + "Troncal" + "-" + "No asignado";
                }
            }
        }

        #region Controles

        private void radioButtonLD_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLD.Checked)
            {
                LD.Visible = true;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                LD_TextChanged(null, null);
            }
        }
        private void radioButtonLE_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLE.Checked)
            {
                LD.Visible = false;
                LE.Visible = true;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                LE_TextChanged(null, null);
            }
        }
        private void radioButtonLF_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLF.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = true;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                LF_TextChanged(null, null);
            }
        }
        private void radioButtonLH_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLH.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = true;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                LH_TextChanged(null, null);
            }
        }
        private void radioButtonLj_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLj.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = true;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                Lj_TextChanged(null, null);
            }
        }
        private void radioButtonLl_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLl.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = true;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                Ll_TextChanged(null, null);
            }
        }
        private void radioButtonLm_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLm.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = true;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                Lm_TextChanged(null, null);
            }
        }
        private void radioButtonLN_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLN.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = true;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = false;
                LN_TextChanged(null, null);
            }
        }
        private void radioButtonLP_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLP.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = true;
                LR.Visible = false;
                LT.Visible = false;
                LP_TextChanged(null, null);
            }
        }
        private void radioButtonLR_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLR.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = true;
                LT.Visible = false;
                LR_TextChanged(null, null);
            }
        }
        private void radioButtonLT_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonLT.Checked)
            {
                LD.Visible = false;
                LE.Visible = false;
                LF.Visible = false;
                LH.Visible = false;
                Lj.Visible = false;
                Ll.Visible = false;
                Lm.Visible = false;
                LN.Visible = false;
                LP.Visible = false;
                LR.Visible = false;
                LT.Visible = true;
                LT_TextChanged(null, null);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (radioButtonLD.Checked)
            {
                LD.Text = " " + LD.Text;
            }
            else if (radioButtonLE.Checked)
            {
                LE.Text = " " + LE.Text;
            }
            else if (radioButtonLF.Checked)
            {
                LF.Text = " " + LF.Text;
            }
            else if (radioButtonLH.Checked)
            {
                LH.Text = " " + LH.Text;
            }
            else if (radioButtonLj.Checked)
            {
                Lj.Text = " " + Lj.Text;
            }
            else if (radioButtonLl.Checked)
            {
                Ll.Text = " " + Ll.Text;
            }
            else if (radioButtonLm.Checked)
            {
                Lm.Text = " " + Lm.Text;
            }
            else if (radioButtonLN.Checked)
            {
                LN.Text = " " + LN.Text;
            }
            else if (radioButtonLP.Checked)
            {
                LP.Text = " " + LP.Text;
            }
            else if (radioButtonLR.Checked)
            {
                LR.Text = " " + LR.Text;
            }
            else if (radioButtonLT.Checked)
            {
                LT.Text = " " + LT.Text;
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (radioButtonLD.Checked)
            {
                LD.Text = "        " + LD.Text;
            }
            else if (radioButtonLE.Checked)
            {
                LE.Text = "        " + LE.Text;
            }
            else if (radioButtonLF.Checked)
            {
                LF.Text = "        " + LF.Text;
            }
            else if (radioButtonLH.Checked)
            {
                LH.Text = "        " + LH.Text;
            }
            else if (radioButtonLj.Checked)
            {
                Lj.Text = "        " + Lj.Text;
            }
            else if (radioButtonLl.Checked)
            {
                Ll.Text = "        " + Ll.Text;
            }
            else if (radioButtonLm.Checked)
            {
                Lm.Text = "        " + Lm.Text;
            }
            else if (radioButtonLN.Checked)
            {
                LN.Text = "        " + LN.Text;
            }
            else if (radioButtonLP.Checked)
            {
                LP.Text = "        " + LP.Text;
            }
            else if (radioButtonLR.Checked)
            {
                LR.Text = "        " + LR.Text;
            }
            else if (radioButtonLT.Checked)
            {
                LT.Text = "        " + LT.Text;
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (radioButtonLD.Checked)
            {
                if (LD.Text.StartsWith(" "))
                    LD.Text = LD.Text.Remove(0, 1);
            }
            else if (radioButtonLE.Checked)
            {
                if (LE.Text.StartsWith(" "))
                    LE.Text = LE.Text.Remove(0, 1);
            }
            else if (radioButtonLF.Checked)
            {
                if (LF.Text.StartsWith(" "))
                    LF.Text = LF.Text.Remove(0, 1);
            }
            else if (radioButtonLH.Checked)
            {
                if (LH.Text.StartsWith(" "))
                    LH.Text = LH.Text.Remove(0, 1);
            }
            else if (radioButtonLj.Checked)
            {
                if (Lj.Text.StartsWith(" "))
                    Lj.Text = Lj.Text.Remove(0, 1);
            }
            else if (radioButtonLl.Checked)
            {
                if (Ll.Text.StartsWith(" "))
                    Ll.Text = Ll.Text.Remove(0, 1);
            }
            else if (radioButtonLm.Checked)
            {
                if (Lm.Text.StartsWith(" "))
                    Lm.Text = Lm.Text.Remove(0, 1);
            }
            else if (radioButtonLN.Checked)
            {
                if (LN.Text.StartsWith(" "))
                    LN.Text = LN.Text.Remove(0, 1);
            }
            else if (radioButtonLP.Checked)
            {
                if (LP.Text.StartsWith(" "))
                    LP.Text = LP.Text.Remove(0, 1);
            }
            else if (radioButtonLR.Checked)
            {
                if (LR.Text.StartsWith(" "))
                    LR.Text = LR.Text.Remove(0, 1);
            }
            else if (radioButtonLT.Checked)
            {
                if (LT.Text.StartsWith(" "))
                    LT.Text = LT.Text.Remove(0, 1);
            }
        }
        private void button10_Click(object sender, EventArgs e)
        {
            if (radioButtonLD.Checked)
            {
                if (LD.Text.StartsWith("        "))
                    LD.Text = LD.Text.Remove(0, 8);
            }
            else if (radioButtonLE.Checked)
            {
                if (LE.Text.StartsWith("        "))
                    LE.Text = LE.Text.Remove(0, 8);
            }
            else if (radioButtonLF.Checked)
            {
                if (LF.Text.StartsWith("        "))
                    LF.Text = LF.Text.Remove(0, 8);
            }
            else if (radioButtonLH.Checked)
            {
                if (LH.Text.StartsWith("        "))
                    LH.Text = LH.Text.Remove(0, 8);
            }
            else if (radioButtonLj.Checked)
            {
                if (Lj.Text.StartsWith("        "))
                    Lj.Text = Lj.Text.Remove(0, 8);
            }
            else if (radioButtonLl.Checked)
            {
                if (Ll.Text.StartsWith("        "))
                    Ll.Text = Ll.Text.Remove(0, 8);
            }
            else if (radioButtonLm.Checked)
            {
                if (Lm.Text.StartsWith("        "))
                    Lm.Text = Lm.Text.Remove(0, 8);
            }
            else if (radioButtonLN.Checked)
            {
                if (LN.Text.StartsWith("        "))
                    LN.Text = LN.Text.Remove(0, 8);
            }
            else if (radioButtonLP.Checked)
            {
                if (LP.Text.StartsWith("        "))
                    LP.Text = LP.Text.Remove(0, 8);
            }
            else if (radioButtonLR.Checked)
            {
                if (LR.Text.StartsWith("        "))
                    LR.Text = LR.Text.Remove(0, 8);
            }
            else if (radioButtonLT.Checked)
            {
                if (LT.Text.StartsWith("        "))
                    LT.Text = LT.Text.Remove(0, 8);
            }
        }


        private void LD_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LD.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELD = "D->" + "Duración" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLD = "D->" + "Duración" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILD = "D->" + "Duración" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
            }
        }
        private void LE_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LE.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELE = "E->" + "Extensión" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLE = "E->" + "Extensión" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILE = "E->" + "Extensión" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
            }

        }
        private void LF_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LF.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELF = "F->" + "Fecha final de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLF = "F->" + "Fecha final de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILF = "F->" + "Fecha final de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
            }
        }
        private void LH_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LH.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELH = "H->" + "Hora final de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLH = "H->" + "Hora final de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILH = "H->" + "Hora final de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
            }
        }
        private void Lj_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = Lj.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELj = "j->" + "Hora inicial de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLj = "j->" + "Hora inicial de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILj = "j->" + "Hora inicial de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
            }
        }
        private void Ll_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = Ll.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELl = "l->" + "Tipo de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLl = "l->" + "Tipo de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILl = "l->" + "Tipo de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
            }
        }
        private void Lm_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = Lm.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELm = "m->" + "Fecha inicial de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLm = "m->" + "Fecha inicial de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILm = "m->" + "Fecha inicial de llamada" + "-" + Comi + ":" + Term + "-" + textBox7.Text + "-" + listBox2.Text;
                }
            }
        }
        private void LN_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LN.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELN = "N->" + "Número marcado" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLN = "N->" + "Número marcado" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILN = "N->" + "Número marcado" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
            }
        }
        private void LP_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LP.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELP = "P->" + "Código proyecto" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLP = "P->" + "Código proyecto" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILP = "P->" + "Código proyecto" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
            }
        }
        private void LR_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LR.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELR = "R->" + "Tipo de tráfico" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLR = "R->" + "Tipo de tráfico" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILR = "R->" + "Tipo de tráfico" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
            }
        }
        private void LT_TextChanged(object sender, EventArgs e)
        {
            if (reset == false)
            {
                char[] Car = LT.Text.ToCharArray();
                int Comi = 0;
                int Term = 0;
                foreach (object s in Car)
                {
                    if (s.ToString().Equals(" "))
                    {
                        Comi++;
                    }
                    else
                    {
                        Term++;
                    }
                }
                textBox7.Text = label11.Text.Substring(Comi, Term);
                if (tabControl2.SelectedIndex == 0)
                {
                    ELT = "T->" + "Troncal" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 1)
                {
                    SLT = "T->" + "Troncal" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
                else if (tabControl2.SelectedIndex == 2)
                {
                    ILT = "T->" + "Troncal" + "-" + Comi + ":" + Term + "-" + textBox7.Text;
                }
            }
        }
        #endregion

        #region SQL Tablas

        public void Leetabla()
        {
            try
            {
                
                listBox1.Enabled = false;
                listBox2.Enabled = false;
                try
                {
                    using (MySqlConnection conexion = new MySqlConnection(Conexion))
                    {
                        conexion.Open();
                        MySqlDataAdapter adapter = new MySqlDataAdapter("SELECT * FROM tipos_formato", conexion);
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        listBox1.DataSource = table;
                        listBox1.DisplayMember = "Nomb_Tipo";
                        adapter.Dispose();
                        conexion.Close();
                    }
                    listBox1.Enabled = true;
                    listBox2.Enabled = true;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ocurrió un problema al leer la base de datos\n\n" + e.ToString());
                }
            }
            catch
            {
                MessageBox.Show("Error al leer la tabla: tipos_formato");
            }
        }


        #endregion

        #region Continuar

        private void button11_Click(object sender, EventArgs e)
        {
            bool vacios = VaciosE();
            if (vacios == true)
            {
                if (textBox8.Visible == true)
                {
                    Caracter = textBox8.Text[0];
                }
                if(checkBox1.Checked == false)
                {
                    ProcesarE = "0";
                }
                else if(checkBox1.Checked == true)
                {
                    ProcesarE = "1";
                }
                DialogResult dialogResult = MessageBox.Show("Al establecer una configuración, solo se podrá editar cuando el programa inicie, ¿Está seguro que desea guardar la siguiente configuración?: \n\n" + ELD + "\n" + ELE + "\n" + ELF + "\n" + ELH + "\n" + ELj + "\n" + ELl + "\n" + ELm + "\n" + ELN + "\n" + ELP + "\n" + ELR + "\n" + ELT + "\n", "Atención", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    reset = true;
                    tabControl2.SelectTab(1);
                }
                else
                {
                    reset = false;
                }
            }
            else
            {
                MessageBox.Show("No se ha definido ninguna configuración o no se ha escrito el carácter de separación");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            bool vacios = VaciosS();
            if (vacios == true)
            {
                if (checkBox1.Checked == false)
                {
                    ProcesarS = "0";
                }
                else if (checkBox1.Checked == true)
                {
                    ProcesarS = "1";
                }
                DialogResult dialogResult = MessageBox.Show("Al establecer una configuración, solo se podrá editar cuando el programa inicie, ¿Está seguro que desea guardar la siguiente configuración?: \n\n" + SLD + "\n" + SLE + "\n" + SLF + "\n" + SLH + "\n" + SLj + "\n" + SLl + "\n" + SLm + "\n" + SLN + "\n" + SLP + "\n" + SLR + "\n" + SLT + "\n", "Atención", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    reset = true;
                    tabControl2.SelectTab(2);
                }
                else
                {
                    reset = false;
                }
            }
            else
            {
                MessageBox.Show("No se ha definido ninguna configuración!");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            bool vacios = VaciosI();
            if (vacios == true)
            {
                if (checkBox1.Checked == false)
                {
                    ProcesarI = "0";
                }
                else if (checkBox1.Checked == true)
                {
                    ProcesarI = "1";
                }
                DialogResult dialogResult = MessageBox.Show("Al establecer una configuración, solo se podrá editar cuando el programa inicie, ¿Está seguro que desea guardar la siguiente configuración?: \n\n" + ILD + "\n" + ILE + "\n" + ILF + "\n" + ILH + "\n" + ILj + "\n" + ILl + "\n" + ILm + "\n" + ILN + "\n" + ILP + "\n" + ILR + "\n" + ILT + "\n", "Atención", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    reset = true;
                    if (tabControl1.SelectedIndex == 1)
                    {
                        if (Config[19].Equals("1"))
                        {
                            GuardaCar();
                        }
                        else
                        {
                            GuardarCamp();
                        }
                    }
                    else if (tabControl1.SelectedIndex == 2)
                    {
                        if (Config[20].Equals("1"))
                        {
                            GuardaCar();
                        }
                        else
                        {
                            GuardarCamp();
                        }
                    }
                    else if (tabControl1.SelectedIndex == 3)
                    {
                        if (Config[21].Equals("1"))
                        {
                            GuardaCar();
                        }
                        else
                        {
                            GuardarCamp();
                        }
                    }
                    ResetStrings();

                    if (Config[12].Equals("0"))
                    {
                        MessageBox.Show("No se han detectado más puertos activados. La configuración se ha establecido exitosamente");
                        Salir = true;
                        this.Close();
                    }

                    else if (Config[12].Equals("1"))
                    {
                        if (tabControl1.SelectedIndex == 1)
                        {
                            MessageBox.Show("La configuración se ha establecido exitosamente");
                            MessageBox.Show("Se ha detectado otro puerto activado. Continúe con la configuración");
                            tabControl1.SelectTab(2);
                            tabPage6.Controls.Add(tabControl2);
                            tabControl2.SelectTab(0);
                        }
                        else if (tabControl1.SelectedIndex == 2)
                        {
                            MessageBox.Show("No se han detectado más puertos activados. La configuración se ha establecido exitosamente");
                            Salir = true;
                            this.Close();
                        }

                    }

                    else if (Config[12].Equals("2"))
                    {
                        if (tabControl1.SelectedIndex == 1)
                        {
                            MessageBox.Show("La configuración se ha establecido exitosamente");
                            MessageBox.Show("Se ha detectado otro puerto activado. Continúe con la configuración");
                            tabControl1.SelectTab(2);
                            tabPage6.Controls.Add(tabControl2);
                            tabControl2.SelectTab(0);
                        }
                        else if (tabControl1.SelectedIndex == 2)
                        {
                            MessageBox.Show("La configuración se ha establecido exitosamente");
                            MessageBox.Show("Se ha detectado otro puerto activado. Continúe con la configuración");
                            tabControl1.SelectTab(3);
                            tabPage7.Controls.Add(tabControl2);
                            tabControl2.SelectTab(0);
                        }
                        else if (tabControl1.SelectedIndex == 3)
                        {
                            MessageBox.Show("La configuración se ha establecido exitosamente");
                            Salir = true;
                            this.Close();
                        }
                    }
                }
                else
                {
                    reset = false;
                }
            }
            else
            {
                MessageBox.Show("No se ha definido ninguna configuración!");
            }
        }

        public void GuardaCar()
        {

            try
            {
                using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(ArchivoSQL, true))
                {
                    string[] SL;
                    if (tabControl1.SelectedIndex == 1)
                    {
                        escritor.WriteLine("Puerto 1:");
                    }
                    else if (tabControl1.SelectedIndex == 2)
                    {
                        escritor.WriteLine("Puerto 2:");
                    }
                    else if (tabControl1.SelectedIndex == 3)
                    {
                        escritor.WriteLine("Puerto 3:");
                    }
                    escritor.WriteLine("Llamadas entrantes:");
                    escritor.WriteLine(ProcesarE);
                    SL = ELD.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ELE.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ELF.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ELH.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ELj.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ELl.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ELm.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ELN.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ELP.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ELR.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ELT.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    escritor.WriteLine("Llamadas salientes:");
                    escritor.WriteLine(ProcesarS);
                    SL = SLD.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = SLE.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = SLF.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = SLH.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = SLj.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = SLl.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = SLm.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = SLN.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = SLP.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = SLR.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = SLT.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    escritor.WriteLine("Llamadas internas:");
                    escritor.WriteLine(ProcesarI);
                    SL = ILD.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ILE.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ILF.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ILH.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ILj.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ILl.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ILm.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2] + "-" + SL[4]); }
                    SL = ILN.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ILP.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ILR.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    SL = ILT.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + SL[2]); }
                    escritor.Close();
                }
            }

            catch (Exception e)
            {
                System.IO.File.Delete(ArchivoSQL);
                MessageBox.Show("Ocurrió un problema al escribir en el archivo ConfiguracionSQL.txt\n\n" + e.ToString());
            }

        }

        public void GuardarCamp()
        {
            try
            {
                using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(ArchivoSQL, true))
                {
                    string[] SL;
                    if (tabControl1.SelectedIndex == 1)
                    {
                        escritor.WriteLine("Puerto 1:");
                    }
                    else if (tabControl1.SelectedIndex == 2)
                    {
                        escritor.WriteLine("Puerto 2:");
                    }
                    else if (tabControl1.SelectedIndex == 3)
                    {
                        escritor.WriteLine("Puerto 3:");
                    }
                    escritor.WriteLine("Llamadas entrantes:");
                    escritor.WriteLine(ProcesarE);
                    SL = ELD.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2]) + "-" + SL[4]); }
                    SL = ELE.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELF.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2]) + "-" + SL[4]); }
                    SL = ELH.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELj.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELl.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELm.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2]) + "-" + SL[4]); }
                    SL = ELN.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELP.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELR.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    SL = ELT.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampEnt(SL[2])); }
                    escritor.WriteLine("Llamadas salientes:");
                    escritor.WriteLine(ProcesarS);
                    SL = SLD.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2]) + "-" + SL[4]); }
                    SL = SLE.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLF.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2]) + "-" + SL[4]); }
                    SL = SLH.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLj.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLl.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLm.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2]) + "-" + SL[4]); }
                    SL = SLN.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLP.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLR.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    SL = SLT.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampSal(SL[2])); }
                    escritor.WriteLine("Llamadas internas:");
                    escritor.WriteLine(ProcesarI);
                    SL = ILD.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2]) + "-" + SL[4]); }
                    SL = ILE.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILF.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2]) + "-" + SL[4]); }
                    SL = ILH.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILj.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILl.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILm.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2]) + "-" + SL[4]); }
                    SL = ILN.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILP.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILR.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    SL = ILT.Split('-'); if (!SL[2].Equals("No asignado")) { escritor.WriteLine(SL[0] + "-" + GuardarCampInt(SL[2])); }
                    escritor.Close();
                }
            }
            catch (Exception e)
            {
                System.IO.File.Delete(ArchivoSQL);
                MessageBox.Show("Ocurrió un problema al escribir en el archivo ConfiguracionSQL.txt\n\n" + e.ToString());
            }
        }

        public string GuardarCampEnt(string pos)
        {
            string[] Pos = pos.Split(':');
            string Parte = LlamEn.Substring(Convert.ToInt32(Pos[0]), Convert.ToInt32(Pos[1]));
            string[] Partes = LlamEn.Split(Caracter);
            int NumPos = 0;
            int Contador = 0;
            foreach(string s in Partes)
            {
                if (s.Equals(Parte))
                {
                    NumPos = Contador;
                }
                Contador++;
            }
            return (NumPos.ToString());
        }

        public string GuardarCampSal(string pos)
        {
            string[] Pos = pos.Split(':');
            string Parte = LlamEn.Substring(Convert.ToInt32(Pos[0]), Convert.ToInt32(Pos[1]));
            string[] Partes = LlamSal.Split(Caracter);
            int NumPos = 0;
            int Contador = 0;
            foreach (string s in Partes)
            {
                if (s.Equals(Parte))
                {
                    NumPos = Contador;
                }
                Contador++;
            }
            return (NumPos.ToString());
        }

        public string GuardarCampInt(string pos)
        {
            string[] Pos = pos.Split(':');
            string Parte = LlamEn.Substring(Convert.ToInt32(Pos[0]), Convert.ToInt32(Pos[1]));
            string[] Partes = LlamInt.Split(Caracter);
            int NumPos = 0;
            int Contador = 0;
            foreach (string s in Partes)
            {
                if (s.Equals(Parte))
                {
                    NumPos = Contador;
                }
                Contador++;
            }
            return (NumPos.ToString());
        }

        public bool VaciosE()
        {
            if(ELD.Equals("D->" + "Duración" + "-" + "No asignado") && ELE.Equals("E->" + "Extensión" + "-" + "No asignado")
                && ELF.Equals("F->" + "Fecha final de llamada" + "-" + "No asignado") && ELH.Equals("H->" + "Hora final de llamada" + "-" + "No asignado")
                && ELj.Equals("j->" + "Hora inicial de llamada" + "-" + "No asignado") && ELl.Equals("l->" + "Tipo de llamada" + "-" + "No asignado")
                && ELm.Equals("m->" + "Fecha inicial de llamada" + "-" + "No asignado") && ELN.Equals("N->" + "Número marcado" + "-" + "No asignado")
                && ELP.Equals("P->" + "Código proyecto" + "-" + "No asignado") && ELR.Equals("R->" + "Tipo de tráfico" + "-" + "No asignado")
                && ELT.Equals("T->" + "Troncal" + "-" + "No asignado"))
            {
                return (false);
            }
            else
            {
                if (tabControl2.SelectedIndex == 0 && textBox8.Visible == true && string.IsNullOrEmpty(textBox8.Text))
                {
                    return (false);
                }
                else
                {
                    return (true);
                }
                
            }
        }
        public bool VaciosS()
        {
            if (SLD.Equals("D->" + "Duración" + "-" + "No asignado") && SLE.Equals("E->" + "Extensión" + "-" + "No asignado")
                && SLF.Equals("F->" + "Fecha final de llamada" + "-" + "No asignado") && SLH.Equals("H->" + "Hora final de llamada" + "-" + "No asignado")
                && SLj.Equals("j->" + "Hora inicial de llamada" + "-" + "No asignado") && SLl.Equals("l->" + "Tipo de llamada" + "-" + "No asignado")
                && SLm.Equals("m->" + "Fecha inicial de llamada" + "-" + "No asignado") && SLN.Equals("N->" + "Número marcado" + "-" + "No asignado")
                && SLP.Equals("P->" + "Código proyecto" + "-" + "No asignado") && SLR.Equals("R->" + "Tipo de tráfico" + "-" + "No asignado")
                && SLT.Equals("T->" + "Troncal" + "-" + "No asignado"))
            {
                return (false);
            }
            else
            {
                return (true);
            }
        }
        public bool VaciosI()
        {
            if (ILD.Equals("D->" + "Duración" + "-" + "No asignado") && ILE.Equals("E->" + "Extensión" + "-" + "No asignado")
                && ILF.Equals("F->" + "Fecha final de llamada" + "-" + "No asignado") && ILH.Equals("H->" + "Hora final de llamada" + "-" + "No asignado")
                && ILj.Equals("j->" + "Hora inicial de llamada" + "-" + "No asignado") && ILl.Equals("l->" + "Tipo de llamada" + "-" + "No asignado")
                && ILm.Equals("m->" + "Fecha inicial de llamada" + "-" + "No asignado") && ILN.Equals("N->" + "Número marcado" + "-" + "No asignado")
                && ILP.Equals("P->" + "Código proyecto" + "-" + "No asignado") && ILR.Equals("R->" + "Tipo de tráfico" + "-" + "No asignado")
                && ILT.Equals("T->" + "Troncal" + "-" + "No asignado"))
            {
                return (false);
            }
            else
            {
                return (true);
            }
        }
        #endregion

        #region Reset

        private void tabControl2_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (reset == true)
            {
                Reset();
                reset = false;
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Al cambiar de TabPage la configuración establecida se perderá, si desea guardarla, use el botón continuar, ¿Desea cambiar de TabPage de todos modos?", "Atención", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    Reset();
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        public void Reset()
        {
            textBox6.Text = null;
            label11.TextAlign = ContentAlignment.MiddleCenter;
            label11.Text = "Cadena no seleccionada";
            button4.Enabled = false;
            button5.Enabled = false;
            SizeCad = 8.25F;
            label11.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style);
            LD.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LD.Text = null;
            LE.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LE.Text = null;
            LF.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LF.Text = null;
            LH.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LH.Text = null;
            Lj.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); Lj.Text = null;
            Ll.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); Ll.Text = null;
            Lm.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); Lm.Text = null;
            LN.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LN.Text = null;
            LP.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LP.Text = null;
            LR.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LR.Text = null;
            LT.Font = new Font(label11.Font.Name, SizeCad, label11.Font.Style); LT.Text = null;
            listBox1.Visible = false;
            listBox2.Visible = false;
            label12.Visible = false;
            label13.Visible = false;
            textBox7.Text = null;
            groupBox1.Enabled = false;
            button11.Visible = false;
            button13.Visible = false;
            button14.Visible = false;
            radioButtonLD.Enabled = false; radioButtonLD.Text = "No seleccionado"; radioButtonLD.Checked = false;
            radioButtonLE.Enabled = false; radioButtonLE.Text = "No seleccionado"; radioButtonLE.Checked = false;
            radioButtonLF.Enabled = false; radioButtonLF.Text = "No seleccionado"; radioButtonLF.Checked = false;
            radioButtonLH.Enabled = false; radioButtonLH.Text = "No seleccionado"; radioButtonLH.Checked = false;
            radioButtonLj.Enabled = false; radioButtonLj.Text = "No seleccionado"; radioButtonLj.Checked = false;
            radioButtonLl.Enabled = false; radioButtonLl.Text = "No seleccionado"; radioButtonLl.Checked = false;
            radioButtonLm.Enabled = false; radioButtonLm.Text = "No seleccionado"; radioButtonLm.Checked = false;
            radioButtonLN.Enabled = false; radioButtonLN.Text = "No seleccionado"; radioButtonLN.Checked = false;
            radioButtonLP.Enabled = false; radioButtonLP.Text = "No seleccionado"; radioButtonLP.Checked = false;
            radioButtonLR.Enabled = false; radioButtonLR.Text = "No seleccionado"; radioButtonLR.Checked = false;
            radioButtonLT.Enabled = false; radioButtonLT.Text = "No seleccionado"; radioButtonLT.Checked = false;
            groupBox3.Enabled = false;
            label10.Visible = false;
            label15.Visible = false;
            textBox8.Visible = false;
            textBox8.Text = null;
            groupBox4.Enabled = false;
            checkBox1.Checked = false;

            if (tabControl2.SelectedIndex == 0)
            {
                tabPage3.Controls.Add(textBox6);
                tabPage3.Controls.Add(button3);
                tabPage3.Controls.Add(label9);
                tabPage3.Controls.Add(groupBox2);
                tabPage3.Controls.Add(label12);
                tabPage3.Controls.Add(label13);
                tabPage3.Controls.Add(listBox1);
                tabPage3.Controls.Add(listBox2);
                tabPage3.Controls.Add(button6);
                tabPage3.Controls.Add(button12);
                tabPage3.Controls.Add(groupBox1);
                tabPage3.Controls.Add(groupBox4);
                button11.Visible = true;
                button11.Enabled = true;
                if(reset == false)
                {
                    MessageBox.Show("La configuración establecida para este campo es: \n\n" + ELD + "\n" + ELE + "\n" + ELF + "\n" + ELH + "\n" + ELj + "\n" + ELl + "\n" + ELm + "\n" + ELN + "\n" + ELP + "\n" + ELR + "\n" + ELT + "\n");
                }
            }
            else if (tabControl2.SelectedIndex == 1)
            {
                tabPage4.Controls.Add(textBox6);
                tabPage4.Controls.Add(button3);
                label9.Text = "Seleccione una cadena de muestra de llamadas salientes: ";
                tabPage4.Controls.Add(label9);
                tabPage4.Controls.Add(groupBox2);
                tabPage4.Controls.Add(label12);
                tabPage4.Controls.Add(label13);
                tabPage4.Controls.Add(listBox1);
                tabPage4.Controls.Add(listBox2);
                tabPage4.Controls.Add(button6);
                tabPage4.Controls.Add(button12);
                tabPage4.Controls.Add(groupBox1);
                tabPage4.Controls.Add(groupBox4);
                button13.Visible = true;
                button13.Enabled = true;
                if (reset == false)
                {
                    MessageBox.Show("La configuración establecida para este campo es: \n\n" + SLD + "\n" + SLE + "\n" + SLF + "\n" + SLH + "\n" + SLj + "\n" + SLl + "\n" + SLm + "\n" + SLN + "\n" + SLP + "\n" + SLR + "\n" + SLT + "\n");
                }
            }
            else if (tabControl2.SelectedIndex == 2)
            {
                tabPage5.Controls.Add(textBox6);
                tabPage5.Controls.Add(button3);
                label9.Text = "Seleccione una cadena de muestra de llamadas internas: ";
                tabPage5.Controls.Add(label9);
                tabPage5.Controls.Add(groupBox2);
                tabPage5.Controls.Add(label12);
                tabPage5.Controls.Add(label13);
                tabPage5.Controls.Add(listBox1);
                tabPage5.Controls.Add(listBox2);
                tabPage5.Controls.Add(button6);
                tabPage5.Controls.Add(button12);
                tabPage5.Controls.Add(groupBox1);
                tabPage5.Controls.Add(groupBox4);
                button14.Visible = true;
                button14.Enabled = true;
                if(reset == false)
                {
                    MessageBox.Show("La configuración establecida para este campo es: \n\n" + ILD + "\n" + ILE + "\n" + ILF + "\n" + ILH + "\n" + ILj + "\n" + ILl + "\n" + ILm + "\n" + ILN + "\n" + ILP + "\n" + ILR + "\n" + ILT + "\n");
                }
            }
        }

        public void ResetStrings()
        {

            //Selecciones llamadas entrantes
            ELD = "D->" + "Duración" + "-" + "No asignado";
            ELE = "E->" + "Extensión" + "-" + "No asignado";
            ELF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
            ELH = "H->" + "Hora final de llamada" + "-" + "No asignado";
            ELj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
            ELl = "l->" + "Tipo de llamada" + "-" + "No asignado";
            ELm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
            ELN = "N->" + "Número marcado" + "-" + "No asignado";
            ELP = "P->" + "Código proyecto" + "-" + "No asignado";
            ELR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
            ELT = "T->" + "Troncal" + "-" + "No asignado";

            //Selecciones llamadas salientes

            SLD = "D->" + "Duración" + "-" + "No asignado";
            SLE = "E->" + "Extensión" + "-" + "No asignado";
            SLF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
            SLH = "H->" + "Hora final de llamada" + "-" + "No asignado";
            SLj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
            SLl = "l->" + "Tipo de llamada" + "-" + "No asignado";
            SLm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
            SLN = "N->" + "Número marcado" + "-" + "No asignado";
            SLP = "P->" + "Código proyecto" + "-" + "No asignado";
            SLR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
            SLT = "T->" + "Troncal" + "-" + "No asignado";

            //llamadas internas

            ILD = "D->" + "Duración" + "-" + "No asignado";
            ILE = "E->" + "Extensión" + "-" + "No asignado";
            ILF = "F->" + "Fecha final de llamada" + "-" + "No asignado";
            ILH = "H->" + "Hora final de llamada" + "-" + "No asignado";
            ILj = "j->" + "Hora inicial de llamada" + "-" + "No asignado";
            ILl = "l->" + "Tipo de llamada" + "-" + "No asignado";
            ILm = "m->" + "Fecha inicial de llamada" + "-" + "No asignado";
            ILN = "N->" + "Número marcado" + "-" + "No asignado";
            ILP = "P->" + "Código proyecto" + "-" + "No asignado";
            ILR = "R->" + "Tipo de tráfico" + "-" + "No asignado";
            ILT = "T->" + "Troncal" + "-" + "No asignado";

        }


        #endregion

        #endregion

    }
}
