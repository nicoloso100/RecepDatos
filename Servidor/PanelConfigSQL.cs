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
    public partial class PanelConfigSQL : Form
    {
        public PanelConfigSQL()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Parametros par = new Parametros();
            par.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea cambiar la configuración de los tipos de llamada?\n El servidor se reiniciará si realiza el cambio", "Atención", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                System.IO.File.Delete(RecepDatos.ArchivoSQL);
                Contraseña.Sal = 1;
                Application.Restart();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea cambiar la conexion a la base de datos?\n El servidor se reiniciará si realiza el cambio", "Atención", MessageBoxButtons.OKCancel);
            if (dialogResult == DialogResult.OK)
            {
                RecepDatos.Config[25] = "";
                using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(RecepDatos.Archivo))
                {
                    foreach(string s in RecepDatos.Config)
                    {
                        escritor.WriteLine(s);
                    }
                }
                Contraseña.Sal = 1;
                Application.Restart();
            }
        }
    }
}
