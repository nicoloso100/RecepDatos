using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Servidor
{
    public partial class CaracteresEspeciales : Form
    {
        public int Cod = 0;
        public static string ArchivoCarEsp = System.IO.Path.Combine(RecepDatos.SubCarpeta, "Caracteres_especiales.txt");
        public bool Salir = false;

        List<string> P1 = new List<string>();
        List<string> P2 = new List<string>();
        List<string> P3 = new List<string>();

        public CaracteresEspeciales(int cod)
        {
            InitializeComponent();
            Cod = cod;
            CargaCar();
            this.FormClosing += CaracteresEspeciales_FormClosing;
        }

        private void CaracteresEspeciales_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(Salir == true)
            {
                MessageBox.Show("La configuración de carácteres especiales se ha establecido con éxito");
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Al salir, no se guardará ninguna configuración, ¿Desea continuar?", "Atención", MessageBoxButtons.OKCancel);
                if (dialogResult == DialogResult.OK)
                {
                    MessageBox.Show("La configuración no se guardará");
                }
                else if (dialogResult == DialogResult.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }

        public void CargaCar()
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
                if (Cod == 1)
                {
                    if (P1.Count != 0)
                    {
                        foreach (string s in P1)
                        {
                            listBox1.Items.Add(s);
                        }
                    }
                }
                else if (Cod == 2)
                {
                    if (P2.Count != 0)
                    {
                        foreach (string s in P2)
                        {
                            listBox1.Items.Add(s);
                        }
                    }
                }
                else if (Cod == 3)
                {
                    if (P3.Count != 0)
                    {
                        foreach (string s in P3)
                        {
                            listBox1.Items.Add(s);
                        }
                    }
                }
            }
        }

        public void GuardaCar ()
        {
            if(Cod == 1)
            {
                P1 = new List<string>();
                foreach (var s in listBox1.Items)
                {
                    P1.Add(s.ToString());
                }
            }
            else if(Cod == 2)
            {
                P2 = new List<string>();
                foreach (var s in listBox1.Items)
                {
                    P2.Add(s.ToString());
                }
            }
            else if(Cod == 3)
            {
                P3 = new List<string>();
                foreach (var s in listBox1.Items)
                {
                    P3.Add(s.ToString());
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add("'" + textBox1.Text[0] + "'");
            textBox1.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count == 0)
            {
                MessageBox.Show("No se ha establecido ningún caracter especial");
                if (Cod == 1)
                {
                    P1 = new List<string>();
                    escribe();
                }
                else if(Cod == 2)
                {
                    P2 = new List<string>();
                    escribe();
                }
                else if (Cod == 3)
                {
                    P3= new List<string>();
                    escribe();
                }
            }
            else
            {
                escribe();
            }
        }

        public void escribe()
        {
            try
            {
                System.IO.File.Delete(ArchivoCarEsp);
                GuardaCar();
                using (StreamWriter escritor = new StreamWriter(ArchivoCarEsp))
                {
                    if (P1.Count != 0)
                    {
                        escritor.WriteLine("Puerto 1:");
                        foreach (string s in P1)
                        {
                            escritor.WriteLine(s);
                        }
                    }
                    if (P2.Count != 0)
                    {
                        escritor.WriteLine("Puerto 2:");
                        foreach (string s in P2)
                        {
                            escritor.WriteLine(s);
                        }
                    }
                    if (P3.Count != 0)
                    {
                        escritor.WriteLine("Puerto 3:");
                        foreach (string s in P3)
                        {
                            escritor.WriteLine(s);
                        }
                    }
                    escritor.Close();
                }
                Salir = true;
                this.Close();
            }
            catch (Exception s)
            {
                MessageBox.Show(s.ToString());
            }
        }
    }
}
