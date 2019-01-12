using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Net;
using System.IO;

namespace Servidor
{
    public partial class GeneraIP : Form
    {

        #region Inicio
        public static string localIP = "";
        public static string Port = "";
        public static int SiNo = 0;
        public static string Direccion = Directory.GetCurrentDirectory();
        public static string Carpeta = @Direccion;
        public static string SubCarpeta = System.IO.Path.Combine(Carpeta, "Configuracion");
        public static string Archivo = System.IO.Path.Combine(SubCarpeta, "Configuracion.txt");

        //log errores

        public static string DireccionLog = Directory.GetCurrentDirectory();
        public static string CarpetaLog = @Direccion;
        public static string SubCarpetaLog = System.IO.Path.Combine(CarpetaLog, "Log de errores");
        public static string ArchivoLog = System.IO.Path.Combine(SubCarpetaLog, "Log_Errores.txt");
        public static int Borrar = 0;

        //Caracteres
        public int Caracteres = 0;
        public int Caracteres2 = 0;
        public int Caracteres3 = 0;

        //Tipo de control de cadena
        public int TipoCaracter;
        public int TipoCaracter2;
        public int TipoCaracter3;

        //Configuracion SQL
        public static string ArchivoSQL = System.IO.Path.Combine(SubCarpeta, "ConfiguracionSQL.txt");

        //Caracteres especciales
        public static string ArchivoCarEsp = System.IO.Path.Combine(RecepDatos.SubCarpeta, "Caracteres_especiales.txt");
        List<string> P1 = new List<string>();
        List<string> P2 = new List<string>();
        List<string> P3 = new List<string>();

        public GeneraIP()
        {
            InitializeComponent();
            CargaIp();
            Desactiva();
            textBox5.PasswordChar = '*';
            this.FormClosing += GeneraIP_FormClosing;
        }


        #endregion
        
        #region Escribir
        public void CargaIp()
        {
            try {
                IPHostEntry host;
                host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily.ToString() == "InterNetwork")
                    {
                        localIP = ip.ToString();
                        textBox1.Text = localIP;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Problema al cargar la Ip local!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) || 
               string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) || 
               string.IsNullOrEmpty(textBox7.Text) || (string.IsNullOrEmpty(textBox8.Text) && string.IsNullOrEmpty(textBox9.Text) && 
               string.IsNullOrEmpty(textBox10.Text)) || string.IsNullOrEmpty(textBox11.Text) || string.IsNullOrEmpty(textBox12.Text)
               || string.IsNullOrEmpty(textBox24.Text) || string.IsNullOrEmpty(textBox23.Text) || string.IsNullOrEmpty(textBox22.Text)
               || string.IsNullOrEmpty(textBox21.Text) || string.IsNullOrEmpty(textBox16.Text) || string.IsNullOrEmpty(textBox20.Text)
               || string.IsNullOrEmpty(textBox34.Text) || string.IsNullOrEmpty(textBox35.Text))
            {
                MessageBox.Show("Hay un campo vacío!!");
            }
            else
            {
                if(tabControl1.SelectedIndex == 0 && (string.IsNullOrEmpty(textBox13.Text) || string.IsNullOrEmpty(textBox14.Text))
                  || tabControl1.SelectedIndex == 1 && (string.IsNullOrEmpty(textBox26.Text) || string.IsNullOrEmpty(textBox27.Text) || string.IsNullOrEmpty(textBox25.Text))
                  || tabControl2.SelectedIndex == 0 && (string.IsNullOrEmpty(textBox15.Text) || string.IsNullOrEmpty(textBox17.Text))
                  || tabControl2.SelectedIndex == 1 && (string.IsNullOrEmpty(textBox28.Text) || string.IsNullOrEmpty(textBox29.Text) || string.IsNullOrEmpty(textBox30.Text))
                  || tabControl3.SelectedIndex == 0 && (string.IsNullOrEmpty(textBox18.Text) || string.IsNullOrEmpty(textBox19.Text))
                  || tabControl3.SelectedIndex == 1 && (string.IsNullOrEmpty(textBox31.Text) || string.IsNullOrEmpty(textBox32.Text) || string.IsNullOrEmpty(textBox33.Text)))
                {
                    MessageBox.Show("Hay un campo vacío!!");
                }
                else
                {
                    Port = textBox2.Text;
                    localIP = textBox1.Text;
                    Contraseña contras = new Contraseña(0);
                    contras.ShowDialog();
                    if (Contraseña.ContraSal == 1)
                    {
                        CreaConfig();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
        }
        public void CreaConfig()
        {
            string Contraseña = Seguridad.Encriptar(textBox5.Text);

            if(tabControl1.SelectedIndex == 0)
            {
                TipoCaracter = 1;
            }
            else if(tabControl1.SelectedIndex == 1)
            {
                TipoCaracter = 2;
            }
            if (tabControl2.SelectedIndex == 0)
            {
                TipoCaracter2 = 1;
            }
            else if (tabControl2.SelectedIndex == 1)
            {
                TipoCaracter2 = 2;
            }
            if (tabControl3.SelectedIndex == 0)
            {
                TipoCaracter3 = 1;
            }
            else if (tabControl3.SelectedIndex == 1)
            {
                TipoCaracter3 = 2;
            }

            if (!System.IO.File.Exists(Archivo))
            {
                try {
                    System.IO.Directory.CreateDirectory(SubCarpeta);
                    using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(Archivo))
                    {
                        escritor.WriteLine(localIP);
                        escritor.WriteLine(Port);
                        escritor.WriteLine(textBox3.Text);
                        escritor.WriteLine(textBox6.Text);
                        escritor.WriteLine(textBox7.Text);
                        escritor.WriteLine(textBox4.Text);
                        escritor.WriteLine(Contraseña);
                        escritor.WriteLine(textBox8.Text);
                        escritor.WriteLine(textBox9.Text);
                        escritor.WriteLine(textBox10.Text);
                        if(textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            escritor.WriteLine(textBox12.Text);
                            escritor.WriteLine(textBox11.Text);

                        }
                        else
                        {
                            escritor.WriteLine(textBox11.Text);
                            escritor.WriteLine(textBox12.Text);
                        }
                        escritor.WriteLine(numPuertos().ToString());

                        CarEsp();

                        if (tabControl1.SelectedIndex == 0)
                        {
                            if (radioButton1.Checked)
                            {
                                escritor.WriteLine(GuarCarEspP1(Caracteres));
                            }
                            else if (radioButton2.Checked)
                            {
                                escritor.WriteLine(GuarCarEspP1(Convert.ToInt32(textBox14.Text)));
                            }
                        }
                        else if(tabControl1.SelectedIndex == 1)
                        {
                            if (radioButton7.Checked)
                            {
                                escritor.WriteLine(GuarCarEspP1(Caracteres));
                            }
                            else if (radioButton8.Checked)
                            {
                                escritor.WriteLine(GuarCarEspP1(Convert.ToInt32(textBox27.Text)));
                            }
                        }

                        if(textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            if (tabControl3.SelectedIndex == 0)
                            {
                                if (radioButton6.Checked)
                                {
                                    if (Caracteres3.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Caracteres3));
                                    }
                                }
                                else if (radioButton5.Checked)
                                {
                                    if (textBox18.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Convert.ToInt32(textBox18.Text)));
                                    }
                                }
                            }

                            else if (tabControl3.SelectedIndex == 1)
                            {

                                if (radioButton12.Checked)
                                {
                                    if (Caracteres3.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Caracteres3));
                                    }
                                }
                                else if (radioButton11.Checked)
                                {
                                    if (textBox31.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Convert.ToInt32(textBox31.Text)));
                                    }
                                }
                            }
                            if (tabControl2.SelectedIndex == 0)
                            {
                                if (radioButton4.Checked)
                                {
                                    if (Caracteres2.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Caracteres2));
                                    }
                                }
                                else if (radioButton3.Checked)
                                {
                                    if (textBox15.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Convert.ToInt32(textBox15.Text)));
                                    }
                                }
                            }
                            else if (tabControl2.SelectedIndex == 1)
                            {
                                if (radioButton10.Checked)
                                {
                                    if (Caracteres2.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Caracteres2));
                                    }
                                }
                                else if (radioButton9.Checked)
                                {
                                    if (textBox28.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Convert.ToInt32(textBox28.Text)));
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tabControl2.SelectedIndex == 0)
                            {
                                if (radioButton4.Checked)
                                {
                                    if (Caracteres2.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Caracteres2));
                                    }
                                }
                                else if (radioButton3.Checked)
                                {
                                    if (textBox15.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Convert.ToInt32(textBox15.Text)));
                                    }
                                }
                            }
                            else if(tabControl2.SelectedIndex == 1)
                            {
                                if (radioButton10.Checked)
                                {
                                    if (Caracteres2.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Caracteres2));
                                    }
                                }
                                else if (radioButton9.Checked)
                                {
                                    if (textBox28.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP2(Convert.ToInt32(textBox28.Text)));
                                    }
                                }
                            }
                            if (tabControl3.SelectedIndex == 0)
                            {
                                if (radioButton6.Checked)
                                {
                                    if (Caracteres3.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Caracteres3));
                                    }
                                }
                                else if (radioButton5.Checked)
                                {
                                    if (textBox18.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Convert.ToInt32(textBox18.Text)));
                                    }
                                }
                            }

                            else if (tabControl3.SelectedIndex == 1)
                            {

                                if (radioButton12.Checked)
                                {
                                    if (Caracteres3.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Caracteres3));
                                    }
                                }
                                else if (radioButton11.Checked)
                                {
                                    if (textBox31.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine("Desactivado");
                                    }
                                    else
                                    {
                                        escritor.WriteLine(GuarCarEspP3(Convert.ToInt32(textBox31.Text)));
                                    }
                                }
                            }
                        }

                        escritor.WriteLine(textBox16.Text + "-" + textBox20.Text);

                        if (textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            escritor.WriteLine(textBox24.Text + "-" + textBox23.Text);
                            escritor.WriteLine(textBox22.Text + "-" + textBox21.Text);
                        }
                        else
                        {
                            escritor.WriteLine(textBox22.Text + "-" + textBox21.Text);
                            escritor.WriteLine(textBox24.Text + "-" + textBox23.Text);
                        }

                        escritor.WriteLine(TipoCaracter);

                        if (textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            escritor.WriteLine(TipoCaracter3);
                            escritor.WriteLine(TipoCaracter2);
                        }
                        else
                        {
                            escritor.WriteLine(TipoCaracter2);
                            escritor.WriteLine(TipoCaracter3);
                        }

                        escritor.WriteLine(textBox25.Text);
                        if(textBox33.Text.Equals("Ej: ; , -"))
                        {
                            textBox33.Text = "";
                        }
                        if (textBox30.Text.Equals("Ej: ; , -"))
                        {
                            textBox30.Text = "";
                        }
                        if (textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            
                            escritor.WriteLine(textBox33.Text);
                            escritor.WriteLine(textBox30.Text);
                        }
                        else
                        {
                            escritor.WriteLine(textBox30.Text);
                            escritor.WriteLine(textBox33.Text);
                        }
                        escritor.WriteLine("Conexion a SQL no establecida");
                        escritor.WriteLine(textBox34.Text);
                        escritor.WriteLine(textBox35.Text);

                        escritor.Close();                        
                        SiNo = 1;
                        this.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Ocurrió un problema al establecer la configuración");
                    Application.Exit();
                }
            }

        }

        #endregion

        #region Cerrar

        private void GeneraIP_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cerrar();
        }

        private void Cerrar()
        {
            if (SiNo == 1)
            {
                MessageBox.Show("La configuración se ha establecido exitosamente!");
                Borrar = 0;
                if (File.Exists(ArchivoSQL))
                {
                    DialogResult dialogResult = MessageBox.Show("Se ha detectado una configuración de SQL, si desea continuar con esa configuración podría generar problemas en el programa, ¿Desea iniciar una nueva configuración de SQL?", "Atención", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Directory.Delete(ArchivoSQL);
                    }
                }
                SQL();
            }
            else
            {
                Borrar = 1;
                Application.Exit();
            }
        }

        public void SQL()
        {
            if (!System.IO.File.Exists(ArchivoSQL))
            {
                ConfiguraSQL sql = new ConfiguraSQL();
                sql.ShowDialog();
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;
        }

        #endregion

        #region Puertos
        

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

            if(textBox11.Enabled == false)
            {
                textBox11.Enabled = true;
                textBox11.Text = "";
                radioButton4.Enabled = true;
                radioButton3.Enabled = true;
                textBox22.Enabled = true;
                textBox21.Enabled = true;
                textBox22.Text = "";
                textBox21.Text = "";
                radioButton9.Enabled = true;
                radioButton10.Enabled = true;
                textBox30.Text = "";
                textBox30.Enabled = true;

                if (radioButton4.Checked)
                {
                    textBox17.Enabled = true;
                    textBox17.Text = "";
                    button4.Enabled = true;
                }
                else if(radioButton3.Checked)
                {
                    textBox15.Enabled = true;
                    textBox15.Text = "";
                }
                if (radioButton10.Checked)
                {
                    textBox29.Enabled = true;
                    textBox29.Text = "";
                    button7.Enabled = true;
                }
                else if (radioButton9.Checked)
                {
                    textBox28.Enabled = true;
                    textBox28.Text = "";
                }
            }
            else
            {
                textBox11.Enabled = false;
                textBox11.Text = "Desactivado";
                radioButton4.Enabled = false;
                radioButton3.Enabled = false;
                textBox17.Enabled = false;
                textBox15.Enabled = false;
                button4.Enabled = false;
                textBox17.Text = "Seleccione un archivo .txt para escanear";
                textBox15.Text = "Escriba la cantidad";
                textBox22.Enabled = false;
                textBox21.Enabled = false;
                textBox22.Text = "No establecido";
                textBox21.Text = "No establecido";
                radioButton9.Enabled = false;
                radioButton10.Enabled = false;
                textBox29.Text = "Seleccione un archivo .txt para escanear";
                textBox29.Enabled = false;
                textBox30.Text = "Ej: ; , -";
                textBox30.Enabled = false;
                textBox28.Text = "Escriba la cantidad";
                textBox28.Enabled = false;
                button7.Enabled = false;
                button10.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox12.Enabled == false)
            {
                textBox12.Enabled = true;
                textBox12.Text = "";
                radioButton6.Enabled = true;
                radioButton5.Enabled = true;
                textBox24.Enabled = true;
                textBox23.Enabled = true;
                textBox24.Text = "";
                textBox23.Text = "";
                radioButton11.Enabled = true;
                radioButton12.Enabled = true;
                textBox33.Text = "";
                textBox33.Enabled = true;
                button8.Enabled = true;
                if (radioButton6.Checked)
                {
                    textBox19.Enabled = true;
                    textBox19.Text = "";
                    button5.Enabled = true;
                }
                else if (radioButton5.Checked)
                {
                    textBox18.Enabled = true;
                    textBox18.Text = "";

                }
                if (radioButton12.Checked)
                {
                    textBox32.Enabled = true;
                    textBox32.Text = "";
                    button8.Enabled = true;
                }
                else if (radioButton11.Checked)
                {
                    textBox31.Enabled = true;
                    textBox31.Text = "";
                }
            }
            else
            {
                textBox12.Enabled = false;
                textBox12.Text = "Desactivado";
                radioButton6.Enabled = false;
                radioButton5.Enabled = false;
                textBox19.Enabled = false;
                textBox18.Enabled = false;
                button5.Enabled = false;
                textBox19.Text = "Seleccione un archivo .txt para escanear";
                textBox18.Text = "Escriba la cantidad";
                textBox24.Enabled = false;
                textBox23.Enabled = false;
                textBox24.Text = "No establecido";
                textBox23.Text = "No establecido";
                textBox31.Text = "Escriba la cantidad";
                textBox31.Enabled = false;
                textBox32.Text = "Seleccione un archivo .txt para escanear";
                textBox32.Enabled = false;
                textBox33.Text = "Ej: ; , -";
                textBox33.Enabled = false;
                radioButton11.Enabled = false;
                radioButton12.Enabled = false;
                button8.Enabled = false;
                button11.Enabled = false;
            }
        }

        public int numPuertos()
        {
            int NumPuertos = 0;

            if (!textBox11.Text.Equals("Desactivado"))
            {
                NumPuertos++;
            }
            if (!textBox12.Text.Equals("Desactivado"))
            {
                NumPuertos++;
            }
            return NumPuertos;
        }

        #endregion

        #region Caracteres Especiales
        
        private void button9_Click(object sender, EventArgs e)
        {
            CaracteresEspeciales Car = new CaracteresEspeciales(1);
            Car.ShowDialog();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            CaracteresEspeciales Car = new CaracteresEspeciales(2);
            Car.ShowDialog();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            CaracteresEspeciales Car = new CaracteresEspeciales(3);
            Car.ShowDialog();

        }

        public void CarEsp()
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
            }
        }

        public int GuarCarEspP1(int I)
        {
            if (P1.Count != 0)
            {
                string Cad;
                try
                {
                    using (StreamReader lector = new StreamReader(textBox26.Text))
                    {
                        Cad = lector.ReadLine();
                    }
                    foreach (string s in P1)
                    {
                        Cad = Cad.Replace(s[1], textBox25.Text[0]);
                    }

                    string[] Partes = Cad.Split(textBox25.Text[0]);
                    I = 0;

                    foreach(object s in Partes)
                    {
                        I++;
                    }

                }
                catch
                {
                    MessageBox.Show("Ocurrió un problema al leer el archivo especificado en el textBox 26");
                }
            }
            return (I);
        }

        public int GuarCarEspP2(int I)
        {
            if (P2.Count != 0)
            {
                string Cad;
                try
                {
                    using (StreamReader lector = new StreamReader(textBox29.Text))
                    {
                        Cad = lector.ReadLine();
                    }
                    foreach (string s in P2)
                    {
                        Cad = Cad.Replace(s[1], textBox30.Text[0]);
                    }

                    string[] Partes = Cad.Split(textBox30.Text[0]);
                    I = 0;

                    foreach (object s in Partes)
                    {
                        I++;
                    }

                }
                catch
                {
                    MessageBox.Show("Ocurrió un problema al leer el archivo especificado en el textBox 26");
                }
            }
            return (I);
        }

        public int GuarCarEspP3(int I)
        {
            if (P3.Count != 0)
            {
                string Cad;
                try
                {
                    using (StreamReader lector = new StreamReader(textBox32.Text))
                    {
                        Cad = lector.ReadLine();
                    }
                    foreach (string s in P3)
                    {
                        Cad = Cad.Replace(s[1], textBox33.Text[0]);
                    }

                    string[] Partes = Cad.Split(textBox33.Text[0]);
                    I = 0;

                    foreach (object s in Partes)
                    {
                        I++;
                    }

                }
                catch
                {
                    MessageBox.Show("Ocurrió un problema al leer el archivo especificado en el textBox 26");
                }
            }
            return (I);
        }

        #endregion

        #region Cantidad de carácteres

        public void Desactiva()
        {
            textBox17.Enabled = false;
            textBox17.Text = "Seleccione un archivo .txt para escanear";
            textBox15.Enabled = false;
            textBox15.Text = "Escriba la cantidad";
            textBox19.Enabled = false;
            textBox19.Text = "Seleccione un archivo .txt para escanear";
            textBox18.Enabled = false;
            textBox18.Text = "Escriba la cantidad";
            radioButton3.Enabled = false;
            radioButton4.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            textBox28.Text = "Escriba la cantidad";
            textBox28.Enabled = false;
            textBox29.Text = "Seleccione un archivo .txt para escanear";
            textBox29.Enabled = false;
            textBox30.Text = "Ej: ; , -";
            textBox30.Enabled = false;
            radioButton9.Enabled = false;
            radioButton10.Enabled = false;
            textBox31.Text = "Escriba la cantidad";
            textBox31.Enabled = false;
            textBox32.Text = "Seleccione un archivo .txt para escanear";
            textBox32.Enabled = false;
            textBox33.Text = "Ej: ; , -";
            textBox33.Enabled = false;
            radioButton11.Enabled = false;
            radioButton12.Enabled = false;
            button7.Enabled = false;
            button8.Enabled = false;

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if(textBox13.Enabled == true)
            {
                textBox13.Enabled = false;
                textBox13.Text = "Seleccione un archivo .txt para escanear";
                button3.Enabled = false;
            }
            else
            {
                textBox13.Enabled = true;
                textBox13.Text = "";
                button3.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox14.Enabled == true)
            {
                textBox14.Enabled = false;
                textBox14.Text = "Escriba la cantidad";
            }
            else
            {
                textBox14.Enabled = true;
                textBox14.Text = "";
            }
        }

        private void radioButton4_CheckedChanged_1(object sender, EventArgs e)
        {
            if (textBox17.Enabled == true)
            {
                textBox17.Enabled = false;
                textBox17.Text = "Seleccione un archivo .txt para escanear";
                button4.Enabled = false;
            }
            else
            {
                textBox17.Enabled = true;
                textBox17.Text = "";
                button4.Enabled = true;
            }
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox15.Enabled == true)
            {
                textBox15.Enabled = false;
                textBox15.Text = "Escriba la cantidad";
            }
            else
            {
                textBox15.Enabled = true;
                textBox15.Text = "";
            }
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox19.Enabled == true)
            {
                textBox19.Enabled = false;
                textBox19.Text = "Seleccione un archivo .txt para escanear";
                button5.Enabled = false;
            }
            else
            {
                textBox19.Enabled = true;
                textBox19.Text = "";
                button5.Enabled = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox18.Enabled == true)
            {
                textBox18.Enabled = false;
                textBox18.Text = "Escriba la cantidad";
            }
            else
            {
                textBox18.Enabled = true;
                textBox18.Text = "";
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox27.Enabled == true)
            {
                textBox27.Enabled = false;
                textBox27.Text = "Escriba la cantidad";
            }
            else
            {
                textBox27.Enabled = true;
                textBox27.Text = "";
            }
        }


        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox26.Enabled == true)
            {
                textBox26.Enabled = false;
                textBox26.Text = "Seleccione un archivo .txt para escanear";
                button6.Enabled = false;
            }
            else
            {
                textBox26.Enabled = true;
                textBox26.Text = "";
                button6.Enabled = true;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox28.Enabled == true)
            {
                textBox28.Enabled = false;
                textBox28.Text = "Escriba la cantidad";
            }
            else
            {
                textBox28.Enabled = true;
                textBox28.Text = "";
            }
        }

        private void radioButton10_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox29.Enabled == true)
            {
                textBox29.Enabled = false;
                textBox29.Text = "Seleccione un archivo .txt para escanear";
                button7.Enabled = false;
            }
            else
            {
                textBox29.Enabled = true;
                textBox29.Text = "";
                button7.Enabled = true;
            }
        }

        private void radioButton11_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox31.Enabled == true)
            {
                textBox31.Enabled = false;
                textBox31.Text = "Escriba la cantidad";
            }
            else
            {
                textBox31.Enabled = true;
                textBox31.Text = "";
            }
        }

        private void radioButton12_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox32.Enabled == true)
            {
                textBox32.Enabled = false;
                textBox32.Text = "Seleccione un archivo .txt para escanear";
                button8.Enabled = false;
            }
            else
            {
                textBox32.Enabled = true;
                textBox32.Text = "";
                button8.Enabled = true;
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox13.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox13.Text);
                string cantidad = cad.ReadLine();
                cad.Close();
                Caracteres = cantidad.Length;
                MessageBox.Show("La cadena seleccionada es: " + "(inicio)" + cantidad + "(fin)" + "\n\n la cantidad de carácteres leídos fueron: " + Caracteres);
            }
            catch
            {
                MessageBox.Show("Error al cargar el archivo de muestra");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox17.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox17.Text);
                string cantidad = cad.ReadLine();
                cad.Close();
                Caracteres2 = cantidad.Length;
                MessageBox.Show("La cadena seleccionada es: " + "(inicio)" + cantidad + "(fin)" + "\n\n la cantidad de carácteres leídos fueron: " + Caracteres2);
            }
            catch
            {
               MessageBox.Show("Error al cargar el archivo de muestra");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox19.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox19.Text);
                string cantidad = cad.ReadLine();
                cad.Close();
                Caracteres3 = cantidad.Length;
                MessageBox.Show("La cadena seleccionada es: " + "(inicio)" + cantidad + "(fin)" + "\n\n la cantidad de carácteres leídos fueron: " + Caracteres3);
            }
            catch
            {
                MessageBox.Show("Error al cargar el archivo de muestra");
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox26.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox26.Text);
                string cantidad = cad.ReadLine();
                cad.Close();
                if (string.IsNullOrEmpty(textBox25.Text))
                {
                    MessageBox.Show("No se ha seleccionao un carácter válido");
                }
                else
                {
                    button9.Enabled = true;
                    Caracteres = 0;
                    string[] Partes = cantidad.Split(textBox25.Text[0]);
                    foreach(var s in Partes)
                    {
                        Caracteres++;
                    }
                    MessageBox.Show("La cadena seleccionada es: " + "(inicio)" + cantidad + "(fin)" + "\n\n la cantidad de partes separadas por el carácter " + "'" + textBox25.Text[0] + "'" + " leídos fueron: " + Caracteres);
                }
            }
            catch
            {
                MessageBox.Show("Error al cargar el archivo de muestra");
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox29.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox29.Text);
                string cantidad = cad.ReadLine();
                cad.Close();
                if (string.IsNullOrEmpty(textBox30.Text))
                {
                    MessageBox.Show("No se ha seleccionao un carácter válido");
                }
                else
                {
                    button10.Enabled = true;
                    Caracteres2 = 0;
                    string[] Partes = cantidad.Split(textBox30.Text[0]);
                    foreach (var s in Partes)
                    {
                        Caracteres2++;
                    }
                    MessageBox.Show("La cadena seleccionada es: " + "(inicio)" + cantidad + "(fin)" + "\n\n la cantidad de partes separadas por el carácter " + "'" + textBox30.Text[0] + "'" + " leídos fueron: " + Caracteres2);
                }
            }
            catch
            {
                MessageBox.Show("Error al cargar el archivo de muestra");
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox32.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox32.Text);
                string cantidad = cad.ReadLine();
                cad.Close();
                if (string.IsNullOrEmpty(textBox33.Text))
                {
                    MessageBox.Show("No se ha seleccionao un carácter válido");
                }
                else
                {
                    button11.Enabled = true;
                    Caracteres3 = 0;
                    string[] Partes = cantidad.Split(textBox33.Text[0]);
                    foreach (var s in Partes)
                    {
                        Caracteres3++;
                    }
                    MessageBox.Show("La cadena seleccionada es: " + "(inicio)" + cantidad + "(fin)" + "\n\n la cantidad de partes separadas por el carácter " + "'" + textBox33.Text[0] + "'" + " leídos fueron: " + Caracteres3);
                }
            }
            catch
            {
                MessageBox.Show("Error al cargar el archivo de muestra");
            }
        }

        #endregion
        
    }

    #region Encriptación
    public static class Seguridad
    {

        /// Encripta una cadena
        public static string Encriptar(this string _cadenaAencriptar)
        {
            string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;
        }

        /// Esta función desencripta la cadena que le envíamos en el parámentro de entrada.
        public static string DesEncriptar(this string _cadenaAdesencriptar)
        {
            string result = string.Empty;
            byte[] decryted = Convert.FromBase64String(_cadenaAdesencriptar);
            //result = System.Text.Encoding.Unicode.GetString(decryted, 0, decryted.ToArray().Length);
            result = System.Text.Encoding.Unicode.GetString(decryted);
            return result;
        }
    }

    #endregion
}
