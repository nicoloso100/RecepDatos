using System;
using System.IO;
using System.Windows.Forms;

namespace Servidor
{
    public partial class SerialConfig : Form
    {
        static string Direccion = Directory.GetCurrentDirectory();
        static string Carpeta = @Direccion;
        static string SubCarpeta = System.IO.Path.Combine(Carpeta, "Configuracion");
        string SerialPath = System.IO.Path.Combine(SubCarpeta, "SerialConf.txt");

        public SerialConfig()
        {
            InitializeComponent();
            foreach (string COM in System.IO.Ports.SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(COM);
            }
            if (File.Exists(SerialPath))
            {   
                using (StreamReader lector = new StreamReader(SerialPath))
                {
                    string[] Lineas = new string[7];
                    for(int i = 0; i < 7; i++)
                    {
                        Lineas[i] = lector.ReadLine();
                    }
                    comboBox1.Text = Lineas[0];
                    comboBox6.Text = Lineas[1];
                    comboBox2.Text = Lineas[2];
                    comboBox3.Text = Lineas[3];
                    comboBox7.Text = Lineas[4];
                    comboBox4.Text = Lineas[5];
                    comboBox5.Text = Lineas[6];
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("La conexión serial(si existe) se reanudará automáticamente");
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.Open();
                MessageBox.Show("Se ha conectado correctamenteal puerto: " + comboBox1.Text);
                serialPort1.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("El puerto especificado no está disponible");
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            serialPort1.PortName = comboBox1.Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox1.Text)){
                MessageBox.Show("Seleccione el puerto serial!");
            }
            else
            {
                using (StreamWriter escritor = new StreamWriter(SerialPath))
                {
                    escritor.WriteLine(comboBox1.Text);
                    escritor.WriteLine(comboBox6.Text);
                    escritor.WriteLine(comboBox2.Text);
                    escritor.WriteLine(comboBox3.Text);
                    escritor.WriteLine(comboBox7.Text);
                    escritor.WriteLine(comboBox4.Text);
                    escritor.WriteLine(comboBox5.Text);
                    escritor.Close();
                }
                MessageBox.Show("Los datos se han guardado exitosamente!");
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Está seguro que desea borrar la configuración", "Advertencia!", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                File.Delete(SerialPath);
            }
        }
    }
}
