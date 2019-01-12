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
    public partial class ConfiguraIP : Form
    {

        #region Inicio

        public static string Direccion = Directory.GetCurrentDirectory();
        public static string Carpeta = @Direccion;
        public static string SubCarpeta = System.IO.Path.Combine(Carpeta, "Configuracion");
        public static string Archivo = System.IO.Path.Combine(SubCarpeta, "Configuracion.txt");
        public static List<string> Config = new List<string>();

        //Caracteres
        public int Caracteres = 0;
        public int Caracteres2 = 0;
        public int Caracteres3 = 0;

        //Tipo de control de cadena
        public int TipoCaracter = 0;
        public int TipoCaracter2 = 0;
        public int TipoCaracter3 = 0;

        //Configuracion SQL
        public static string ArchivoSQL = System.IO.Path.Combine(SubCarpeta, "ConfiguracionSQL.txt");
        string CadConec;
        
        public ConfiguraIP()
        {
            InitializeComponent();
            CargaInfo();
            textBox5.PasswordChar = '*';
        }


        #endregion

        #region CargaInfo
        public void CargaInfo()
        {
            try
            {
                string Line;
                System.IO.StreamReader file = new System.IO.StreamReader(Archivo);
                while ((Line = file.ReadLine()) != null)
                {
                    Config.Add(Line);
                }
                file.Close();
                EscribeInfo();
            }
            catch
            {
                MessageBox.Show("Ocurrió un problema al leer el archivo Configuración.txt");
            }
        }

        #endregion

        #region Escribe
        public void EscribeInfo()
        {
            string Contraseña = Seguridad.DesEncriptar(Config[6]);

            textBox1.Text = Config[0];
            textBox2.Text = Config[1];
            textBox3.Text = Config[2];
            textBox6.Text = Config[3];
            textBox7.Text = Config[4];
            textBox4.Text = Config[5];
            textBox5.Text = Contraseña;
            textBox8.Text = Config[7];
            textBox9.Text = Config[8];
            textBox10.Text = Config[9];
            textBox11.Text = Config[10];
            textBox12.Text = Config[11];
            if (textBox11.Text.Equals("Desactivado"))
            {
                checkBox1.Checked = false;
            }
            else
            {
                checkBox1.Checked = true;
                textBox11.Enabled = true;
            }
            if (textBox12.Text.Equals("Desactivado"))
            {
                checkBox2.Checked = false;
            }
            else
            {
                checkBox2.Checked = true;
                textBox12.Enabled = true;
            }
            try
            {
                label19.Text = Config[13];

            }
            catch(Exception e)
            {
                MessageBox.Show("Problema al leer la cantidad de carácteres del puerto 1\n\n" + e.ToString());
                label19.Text = "Error";
            }
            string[] tiempartes = Config[16].Split('-');
            textBox24.Text = tiempartes[0];
            textBox23.Text = tiempartes[1];
            tiempartes = Config[17].Split('-');
            textBox20.Text = tiempartes[0];
            textBox14.Text = tiempartes[1];
            tiempartes = Config[18].Split('-');
            textBox22.Text = tiempartes[0];
            textBox13.Text = tiempartes[1];

            if(Config[19].Equals("2"))
            {
                tabControl1.SelectTab(1);
                textBox25.Text = Config[22];
            }

            if (!Config[10].Equals("Desactivado"))
            {
                label17.Text = Config[14];
                checkBox1.Checked = true;
                textBox11.Text = Config[10];
                textBox15.Enabled = false;
                textBox15.Text = "Escriba la cantidad";
                textBox28.Enabled = false;
                textBox28.Text = "Escriba la cantidad";

                if (Config[20].Equals("2"))
                {
                    tabControl3.SelectTab(1);
                    textBox30.Text = Config[23];
                }
            }
            else
            {
                textBox17.Enabled = false;
                textBox17.Text = "Seleccione un archivo .txt para escanear";
                textBox15.Enabled = false;
                textBox15.Text = "Escriba la cantidad";
                radioButton3.Enabled = false;
                radioButton4.Enabled = false;
                button4.Enabled = false;
                textBox28.Enabled = false;
                textBox28.Text = "Escriba la cantidad";
                radioButton10.Enabled = false;
                radioButton9.Enabled = false;
                button7.Enabled = false;
                textBox30.Enabled = false;
                textBox30.Text = "Ej: ; , -";
                textBox29.Enabled = false;
                textBox29.Text = "Seleccione un archivo .txt para escanear";
            }

            if (!Config[11].Equals("Desactivado"))
            {
                textBox18.Enabled = false;
                textBox18.Text = "Escriba la cantidad";
                checkBox2.Checked = true;
                textBox12.Text = Config[11];
                label18.Text = Config[15];
                textBox31.Enabled = false;
                textBox31.Text = "Escriba la cantidad";

                if (Config[20].Equals("2"))
                {
                    tabControl4.SelectTab(1);
                    textBox33.Text = Config[24];
                }
            }
            else
            {
                button5.Enabled = false;
                radioButton5.Enabled = false;
                radioButton6.Enabled = false;
                textBox19.Enabled = false;
                textBox19.Text = "Seleccione un archivo .txt para escanear";
                textBox18.Enabled = false;
                textBox18.Text = "Escriba la cantidad";
                radioButton12.Enabled = false;
                radioButton11.Enabled = false;
                button8.Enabled = false;
                textBox33.Enabled = false;
                textBox33.Text = "Ej: ; , -";
                textBox32.Enabled = false;
                textBox32.Text = "Seleccione un archivo .txt para escanear";
            }
            CadConec = Config[25];
            textBox34.Text = Config[26];
            textBox35.Text = Config[27];
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DialogResult dialogResult = MessageBox.Show("Está seguro que desea cambiar la configuración", "Advertencia!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text) ||
                    string.IsNullOrEmpty(textBox4.Text) || string.IsNullOrEmpty(textBox5.Text) || string.IsNullOrEmpty(textBox6.Text) ||
                    string.IsNullOrEmpty(textBox7.Text) || (string.IsNullOrEmpty(textBox8.Text) && string.IsNullOrEmpty(textBox9.Text) &&
                    string.IsNullOrEmpty(textBox10.Text)) || string.IsNullOrEmpty(textBox24.Text) || string.IsNullOrEmpty(textBox23.Text) ||
                    string.IsNullOrEmpty(textBox20.Text) || string.IsNullOrEmpty(textBox22.Text) || string.IsNullOrEmpty(textBox11.Text) ||
                    string.IsNullOrEmpty(textBox14.Text) || string.IsNullOrEmpty(textBox12.Text) || string.IsNullOrEmpty(textBox13.Text) ||
                    string.IsNullOrEmpty(textBox34.Text) || string.IsNullOrEmpty(textBox35.Text) ||
                    (checkBox1.Checked == true && label17.Text.Equals("Desactivado")) || (checkBox2.Checked == true && label18.Text.Equals("Desactivado")))
                {
                    MessageBox.Show("Hay espacios vacíos!");
                }
                else 
                {
                    if (System.IO.File.Exists(Archivo))
                    {
                        try
                        {
                            System.IO.File.Delete(Archivo);
                            Reescribir();
                        }
                        catch
                        {
                            MessageBox.Show("Ocurrió un error al escribir la nueva configuración en el archivo Configuración.txt, ¿Está abierto el archivo o siendo usado por otro programa?");
                        }
                    }
                    else
                    {
                        Reescribir();
                    }
                }
            }
        }


        public void Reescribir()
        {
            string Contraseña = Seguridad.Encriptar(textBox5.Text);

            if (tabControl1.SelectedIndex == 0)
            {
                TipoCaracter = 1;
            }
            else if (tabControl1.SelectedIndex == 1)
            {
                TipoCaracter = 2;
            }
            if (tabControl3.SelectedIndex == 0)
            {
                TipoCaracter2 = 1;
            }
            else if (tabControl3.SelectedIndex == 1)
            {
                TipoCaracter2 = 2;
            }
            if (tabControl4.SelectedIndex == 0)
            {
                TipoCaracter3 = 1;
            }
            else if (tabControl4.SelectedIndex == 1)
            {
                TipoCaracter3 = 2;
            }

            if (!System.IO.File.Exists(Archivo))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(SubCarpeta);
                    using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(Archivo))
                    {
                        escritor.WriteLine(textBox1.Text);
                        escritor.WriteLine(textBox2.Text);
                        escritor.WriteLine(textBox3.Text);
                        escritor.WriteLine(textBox6.Text);
                        escritor.WriteLine(textBox7.Text);
                        escritor.WriteLine(textBox4.Text);
                        escritor.WriteLine(Contraseña);
                        escritor.WriteLine(textBox8.Text);
                        escritor.WriteLine(textBox9.Text);
                        escritor.WriteLine(textBox10.Text);
                        if (textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            escritor.WriteLine(textBox12.Text);
                            escritor.WriteLine(textBox11.Text);
                        }
                        else
                        {
                            escritor.WriteLine(textBox11.Text);
                            escritor.WriteLine(textBox12.Text);
                        }
                        escritor.WriteLine(numPuertos());

                        if (tabControl1.SelectedIndex == 0)
                        {
                            if (radioButton1.Checked)
                            {
                                if (Caracteres != 0)
                                {
                                    escritor.WriteLine(Caracteres.ToString());
                                }
                                else
                                {
                                    escritor.WriteLine(label19.Text);
                                }
                            }
                            else if (radioButton2.Checked)
                            {
                                if (!string.IsNullOrEmpty(textBox16.Text))
                                {
                                    escritor.WriteLine(textBox16.Text);
                                }
                                else
                                {
                                    escritor.WriteLine(label19.Text);
                                }
                            }
                        }
                        else if (tabControl1.SelectedIndex == 1)
                        {
                            if (radioButton7.Checked)
                            {
                                if (Caracteres != 0)
                                {
                                    escritor.WriteLine(Caracteres.ToString());
                                }
                                else
                                {
                                    escritor.WriteLine(label19.Text);
                                }
                            }
                            else if (radioButton8.Checked)
                            {
                                if (!string.IsNullOrEmpty(textBox27.Text))
                                {
                                    escritor.WriteLine(textBox27.Text);
                                }
                                else
                                {
                                    escritor.WriteLine(label19.Text);
                                }
                            }
                        }

                        if (textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            if (tabControl4.SelectedIndex == 0)
                            {
                                if (radioButton6.Checked)
                                {
                                    if (Caracteres3 != 0)
                                    {
                                        escritor.WriteLine(Caracteres3.ToString());
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                }
                                else if (radioButton5.Checked)
                                {
                                    if (!string.IsNullOrEmpty(textBox18.Text))
                                    {
                                        escritor.WriteLine(textBox18.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                }
                            }
                            else if (tabControl4.SelectedIndex == 1)
                            {
                                if (radioButton12.Checked)
                                {
                                    if (Caracteres3.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(Caracteres3.ToString());
                                    }
                                }
                                else if (radioButton11.Checked)
                                {
                                    if (textBox31.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(textBox31.Text);
                                    }
                                }
                            }
                            if (tabControl3.SelectedIndex == 0)
                            {
                                if (radioButton4.Checked)
                                {
                                    if (Caracteres2 != 0)
                                    {
                                        escritor.WriteLine(Caracteres2.ToString());
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                }
                                else if (radioButton3.Checked)
                                {
                                    if (!string.IsNullOrEmpty(textBox18.Text))
                                    {
                                        escritor.WriteLine(textBox15.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                }
                            }
                            else if (tabControl3.SelectedIndex == 1)
                            {
                                if (radioButton10.Checked)
                                {
                                    if (Caracteres2.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(Caracteres2.ToString());
                                    }
                                }
                                else if (radioButton9.Checked)
                                {
                                    if (textBox28.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(textBox28.Text);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (tabControl3.SelectedIndex == 0)
                            {
                                if (radioButton4.Checked)
                                {
                                    if (Caracteres2 != 0)
                                    {
                                        escritor.WriteLine(Caracteres2.ToString());
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                }
                                else if (radioButton3.Checked)
                                {
                                    if (!string.IsNullOrEmpty(textBox18.Text))
                                    {
                                        escritor.WriteLine(textBox15.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                }
                            }
                            else if (tabControl3.SelectedIndex == 1)
                            {
                                if (radioButton10.Checked)
                                {
                                    if (Caracteres2.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(Caracteres2.ToString());
                                    }
                                }
                                else if (radioButton9.Checked)
                                {
                                    if (textBox28.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine(label17.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(textBox28.Text);
                                    }
                                }
                            }
                            if (tabControl4.SelectedIndex == 0)
                            {
                                if (radioButton6.Checked)
                                {
                                    if (Caracteres3 != 0)
                                    {
                                        escritor.WriteLine(Caracteres3.ToString());
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                }
                                else if (radioButton5.Checked)
                                {
                                    if (!string.IsNullOrEmpty(textBox18.Text))
                                    {
                                        escritor.WriteLine(textBox18.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                }
                            }
                            else if (tabControl4.SelectedIndex == 1)
                            {
                                if (radioButton12.Checked)
                                {
                                    if (Caracteres3.ToString().Equals("0"))
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(Caracteres3.ToString());
                                    }
                                }
                                else if (radioButton11.Checked)
                                {
                                    if (textBox31.Text.Equals("Escriba la cantidad"))
                                    {
                                        escritor.WriteLine(label18.Text);
                                    }
                                    else
                                    {
                                        escritor.WriteLine(textBox31.Text);
                                    }
                                }
                            }

                        }
                        escritor.WriteLine(textBox24.Text + "-" + textBox23.Text);

                        if (textBox11.Text.Equals("Desactivado") && !textBox12.Text.Equals("Desactivado"))
                        {
                            escritor.WriteLine(textBox22.Text + "-" + textBox13.Text);
                            escritor.WriteLine(textBox20.Text + "-" + textBox14.Text);
                        }
                        else
                        {
                            escritor.WriteLine(textBox20.Text + "-" + textBox14.Text);
                            escritor.WriteLine(textBox22.Text + "-" + textBox13.Text);
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

                        if (textBox33.Text.Equals("Ej: ; , -"))
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
                        escritor.WriteLine(CadConec);
                        escritor.WriteLine(textBox34.Text);
                        escritor.WriteLine(textBox35.Text);
                        escritor.Close();
                        MessageBox.Show("La configuración se ha establecido exitosamente!");
                        Reinicio();
                    }
                }
                catch
                {
                    MessageBox.Show("Ocurrió un problema al actualizar la configuración");
                }
            }
        }

        #endregion

        #region Salir
        public void Reinicio()
        {
            DialogResult dialogResult = MessageBox.Show("Para efectuar los cambios el servidor se deberá reiniciar. ¿Desea reiniciar automáticamente?", "Atención", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                Contraseña.Sal = 1;
                DialogResult dialogResult2 = MessageBox.Show("Al hacer cambios en la configuración de los puertos, es recomendable re-configurar los tipos de llamada en configuración SQL, ya que esto podría generar errores. ¿Desea re-configurar los tipos de llamada?", "Atención", MessageBoxButtons.YesNo);
                if (dialogResult2 == DialogResult.Yes)
                {
                    System.IO.File.Delete(ArchivoSQL);
                    Application.Restart();
                }
                else if (dialogResult2 == DialogResult.No)
                {
                    Application.Restart();
                }
                
            }
            else if (dialogResult == DialogResult.Cancel)
            {
                MessageBox.Show("El servidor seguirá trabajando la misma configuración hasta que se reinicie");
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            textBox3.Text = folderBrowserDialog1.SelectedPath;            
        }

        #endregion

        #region Checkbox

        private void checkBox1_CheckedChanged_2(object sender, EventArgs e)
        {

            if (textBox11.Enabled == false)
            {
                textBox11.Enabled = true;
                textBox11.Text = "";
                label17.Text = Config[14];
                radioButton4.Enabled = true;
                radioButton3.Enabled = true;
                textBox20.Enabled = true;
                textBox20.Text = "";
                textBox14.Enabled = true;
                textBox14.Text = "";
                label17.Text = "";
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
                label17.Text = "Desactivado";
                radioButton4.Enabled = false;
                radioButton3.Enabled = false;
                textBox17.Enabled = false;
                textBox15.Enabled = false;
                button4.Enabled = false;
                textBox17.Text = "Seleccione un archivo .txt para escanear";
                textBox15.Text = "Escriba la cantidad";
                textBox20.Enabled = false;
                textBox20.Text = "No establecido";
                textBox14.Enabled = false;
                textBox14.Text = "No establecido";
                label17.Text = "Desactivado";
                radioButton9.Enabled = false;
                radioButton10.Enabled = false;
                textBox29.Text = "Seleccione un archivo .txt para escanear";
                textBox29.Enabled = false;
                textBox30.Text = "Ej: ; , -";
                textBox30.Enabled = false;
                textBox28.Text = "Escriba la cantidad";
                textBox28.Enabled = false;
                button7.Enabled = false;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox12.Enabled == false)
            {
                textBox12.Enabled = true;
                textBox12.Text = "";
                label18.Text = Config[15];
                radioButton5.Enabled = true;
                radioButton6.Enabled = true;
                textBox22.Enabled = true;
                textBox22.Text = "";
                textBox13.Enabled = true;
                textBox13.Text = "";
                label18.Text = "";
                radioButton12.Enabled = true;
                radioButton11.Enabled = true;
                textBox33.Text = "";
                textBox33.Enabled = true;

                if (radioButton6.Checked)
                {
                    textBox19.Enabled = true;
                    textBox19.Text = "";
                    button5.Enabled = true;
                }
                else
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
                label18.Text = "Desactivado";
                radioButton6.Enabled = false;
                radioButton5.Enabled = false;
                textBox19.Enabled = false;
                textBox18.Enabled = false;
                button5.Enabled = false;
                textBox19.Text = "Seleccione un archivo .txt para escanear";
                textBox18.Text = "Escriba la cantidad";
                textBox22.Enabled = false;
                textBox22.Text = "No establecido";
                textBox13.Enabled = false;
                textBox13.Text = "No establecido";
                label18.Text = "Desactivado";
                radioButton12.Enabled = false;
                radioButton11.Enabled = false;
                textBox32.Text = "Seleccione un archivo .txt para escanear";
                textBox32.Enabled = false;
                textBox33.Text = "Ej: ; , -";
                textBox33.Enabled = false;
                textBox31.Text = "Escriba la cantidad";
                textBox31.Enabled = false;
                button8.Enabled = false;
            }
        }

        #endregion

        #region Numpuertos
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

        #region Número de carácteres
        

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (textBox21.Enabled == true)
            {
                textBox21.Enabled = false;
                textBox21.Text = "Seleccione un archivo .txt para escanear";
                button3.Enabled = false;
            }
            else
            {
                textBox21.Enabled = true;
                textBox21.Text = "";
                button3.Enabled = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            
            if (textBox16.Enabled == true)
            {
                textBox16.Enabled = false;
                textBox16.Text = "Escriba la cantidad";
            }
            else
            {
                textBox16.Enabled = true;
                textBox16.Text = "";
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

        private void radioButton3_CheckedChanged_1(object sender, EventArgs e)
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

        private void radioButton6_CheckedChanged_1(object sender, EventArgs e)
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

        private void radioButton5_CheckedChanged_1(object sender, EventArgs e)
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

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.ShowDialog();
                textBox21.Text = openFileDialog1.FileName;
                System.IO.StreamReader cad = new System.IO.StreamReader(textBox21.Text);
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

        private void button4_Click_1(object sender, EventArgs e)
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

        private void button5_Click_1(object sender, EventArgs e)
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

        private void button6_Click_1(object sender, EventArgs e)
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
                    Caracteres = 0;
                    string[] Partes = cantidad.Split(textBox25.Text[0]);
                    foreach (var s in Partes)
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

        private void button7_Click_1(object sender, EventArgs e)
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

        private void button8_Click_1(object sender, EventArgs e)
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

        #region Configuracion SQL

        private void button9_Click(object sender, EventArgs e)
        {
            PanelConfigSQL pan = new PanelConfigSQL();
            pan.Show();
        }

        #endregion

    }
}
