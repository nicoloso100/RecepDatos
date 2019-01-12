using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows.Forms;

namespace Servidor
{
    public partial class Parametros : Form
    {
        public DataSet dtset;
        public MySqlDataAdapter adapter;
        public MySqlConnection Conexion;
        MySqlCommand agregar;

        public Parametros()
        {
            InitializeComponent();
            CargaDatos();
        }

        public void CargaDatos()
        {
            try
            {
                using (Conexion = new MySqlConnection(RecepDatos.Config[25]))
                {
                    Conexion.Open();
                    adapter = new MySqlDataAdapter("Select * From parametros", Conexion);
                    dtset = new DataSet();
                    adapter.Fill(dtset, "parametros");
                    dataGridView1.DataSource = dtset;
                    dataGridView1.DataMember = "parametros";
                    dataGridView1.Columns[0].Width = 200;
                    dataGridView1.Columns[1].Width = 200;
                    dataGridView1.Columns[2].Width = 392;
                    Conexion.Close();
                }
            }
            catch
            {
                MessageBox.Show("No se ha podido leer la tabla parametros de la base de datos");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Conexion = new MySqlConnection(RecepDatos.Config[25]))
            {
                Conexion.Open();
                agregar = new MySqlCommand("delete from parametros", Conexion);
                agregar.ExecuteNonQuery();
                
                try
                {
                    agregar = new MySqlCommand("insert into parametros values (?parametro, ?seleccion, ?detalle)", Conexion);
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        agregar.Parameters.Clear();
                        agregar.Parameters.AddWithValue("?parametro", Convert.ToString(row.Cells["parametro"].Value));
                        agregar.Parameters.AddWithValue("?seleccion", Convert.ToString(row.Cells["seleccion"].Value));
                        agregar.Parameters.AddWithValue("?detalle", Convert.ToString(row.Cells["Detalle"].Value));
                        agregar.ExecuteNonQuery();
                    }
                    CargaDatos();
                    MessageBox.Show("La configuración se ha guardado");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ha ocurrido un error al guardar la configuración" + ex.ToString());
                }

                Conexion.Close();
            }
        }
    }

}
