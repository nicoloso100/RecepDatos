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
    public partial class Contraseña : Form
    {

        #region inicio

        public int cual;
        public static int Sal = 0;
        public static int ContraSal = 0;
        public Contraseña(int Cual)
        {
            InitializeComponent();
            textBox1.PasswordChar = '*';
            cual = Cual;
        }

        #endregion

        #region contraseña

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Equals("RD0315"))
            {
                if(cual == 0)
                {
                    ContraSal = 1;
                    this.Close();                   
                }
                else if(cual == 1)
                {
                    ConfiguraIP confip = new ConfiguraIP();
                    confip.Show();
                    this.Close();
                }
                else if(cual == 2)
                {
                    Sal = 1;
                    Application.Exit();
                }
                else if (cual == 3)
                {
                    RecepDatos.PermiteInterfaces = 1;
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("La contraseña que ha ingresado es incorrecta!");
            }
        }

        #endregion
    }
}
