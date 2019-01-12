using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Servidor
{
    public partial class Lista_de_errores : Form
    {
        #region inicio

        List<string> ErroresPuerto1 = new List<string>();
        List<string> ErroresEntradaDatosPuerto1 = new List<string>();
        List<string> ErroresSaliDatosPuerto1 = new List<string>();
        List<string> ErroresPuerto2 = new List<string>();
        List<string> ErroresEntradaDatosPuerto2 = new List<string>();
        List<string> ErroresSaliDatosPuerto2 = new List<string>();
        List<string> ErroresPuerto3 = new List<string>();
        List<string> ErroresEntradaDatosPuerto3 = new List<string>();
        List<string> ErroresSaliDatosPuerto3 = new List<string>();
        public Lista_de_errores()
        {
            InitializeComponent();
            CargaDatos();
        }

        #endregion

        #region Listas

        public void CargaDatos()
        {
            ErroresPuerto1 = RecepDatos.ErroresPuerto1;
            ErroresEntradaDatosPuerto1 = RecepDatos.ErroresEntradaDatosPuerto1;
            ErroresSaliDatosPuerto1 = RecepDatos.ErroresSaliDatosPuerto1;
            ErroresPuerto2 = RecepDatos.ErroresPuerto2;
            ErroresEntradaDatosPuerto2 = RecepDatos.ErroresEntradaDatosPuerto2;
            ErroresSaliDatosPuerto2 = RecepDatos.ErroresSaliDatosPuerto2;
            ErroresPuerto3 = RecepDatos.ErroresPuerto3;
            ErroresEntradaDatosPuerto3 = RecepDatos.ErroresEntradaDatosPuerto3;
            ErroresSaliDatosPuerto3 = RecepDatos.ErroresSaliDatosPuerto3;
            int NumPuertos = Convert.ToInt32(RecepDatos.Config[12]);
            if(NumPuertos == 0)
            {
                listBox4.Items.Add("Desactivado");
                listBox5.Items.Add("Desactivado");
                listBox6.Items.Add("Desactivado");
                listBox7.Items.Add("Desactivado");
                listBox8.Items.Add("Desactivado");
                listBox9.Items.Add("Desactivado");
                InsertaDatosP1();
            }
            else if(NumPuertos == 1)
            {
                listBox7.Items.Add("Desactivado");
                listBox8.Items.Add("Desactivado");
                listBox9.Items.Add("Desactivado");
                InsertaDatosP1();
                InsertaDatosP2();
            }
            else
            {
                InsertaDatosP1();
                InsertaDatosP2();
                InsertaDatosP3();
            }
            

        }

        #region Datos p1

        public void InsertaDatosP1()
        {
            foreach (string item in ErroresPuerto1)
            {

                string[] partes = item.Split(';');
                
                foreach(string e in partes)
                {
                    listBox1.Items.Add(e);
                }

            }
            foreach (string item in ErroresEntradaDatosPuerto1)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox2.Items.Add(e);
                }

            }
            foreach (string item in ErroresSaliDatosPuerto1)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox3.Items.Add(e);
                }

            }
        }

        #endregion

        #region Datos p2

        public void InsertaDatosP2()
        {
            foreach (string item in ErroresPuerto2)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox4.Items.Add(e);
                }

            }
            foreach (string item in ErroresEntradaDatosPuerto2)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox6.Items.Add(e);
                }

            }
            foreach (string item in ErroresSaliDatosPuerto2)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox5.Items.Add(e);
                }

            }
        }

        #endregion

        #region Datos p3

        public void InsertaDatosP3()
        {
            foreach (string item in ErroresPuerto3)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox7.Items.Add(e);
                }

            }
            foreach (string item in ErroresEntradaDatosPuerto3)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox9.Items.Add(e);
                }

            }
            foreach (string item in ErroresSaliDatosPuerto3)
            {

                string[] partes = item.Split(';');

                foreach (string e in partes)
                {
                    listBox8.Items.Add(e);
                }

            }
        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
        
    }
}
