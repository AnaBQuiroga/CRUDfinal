using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace CRUDfinal
{
    public partial class frmUsuarios : Form
    {
        private string conexionAccess = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source= C:\Users\Admin\Documents\BDfinal1.accdb";
        private OleDbConnection cn;
        List<Usuarios> usuario = new List<Usuarios>();
        public frmUsuarios()
        {
            InitializeComponent();
            this.CenterToScreen();
            
            
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {
            actualizarGrilla();

            foreach (var u in usuario)
            {
                cmbBuscar.Items.Add(u.Nombre);
            }

        }

        void conectar()
        {
            try
            {
                cn = new OleDbConnection(conexionAccess);
                cn.Open();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al conectar la base de datos: " + ex.Message);
            }

        }

        void desconectar()
        {

            try
            {
                if (cn != null && cn.State == ConnectionState.Open)
                {
                    cn.Close();
                    cn.Dispose();
                }


            }
            catch (Exception ex)
            {
                throw new Exception("Error al desconectar la base de datos: " + ex.Message);
            }
        }

        List<Usuarios> buscarPersonas()
        {
            
            try
            {
                conectar();
                string sql = "Select * From Usuarios";
                using (OleDbCommand cmd = new OleDbCommand(sql, cn))
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuario.Add(new Usuarios
                        {
                            Id_usuario= Convert.ToInt32(reader["Id_usuario"]),
                            Apellido = reader["Apellido"].ToString() ?? string.Empty,
                            Nombre = reader["Nombre"].ToString() ?? string.Empty,
                            DNI = Convert.ToInt32(reader["DNI"]),
                            Telefono= Convert.ToInt32(reader["Telefono"]),
                            Direccion = reader["Dirección"].ToString() ?? string.Empty
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al buscar personas: " + ex.Message);
            }
            finally
            {
                desconectar();
            }
            return usuario;
        }

        void actualizarGrilla()
        {
            //usuario.Clear();
            dgvUsuarios.DataSource = null;
            buscarPersonas();
            dgvUsuarios.DataSource = usuario;
            dgvUsuarios.Columns["Id_usuario"].Visible = false;

        }

        void limpiarGrilla(Control control)
        {
            foreach (Control item in control.Controls)
            {
                if (item is TextBox txt) txt.Text = null;
                //if (item is CheckBox chk) chk.Checked = false;
                //if (item is GroupBox || item is Panel) limpiarGrilla((Control)item);
            }
        }



        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (btnAgregar.Text == "Agregar")
            {
                btnAgregar.Text = ("Guardar");
                btnEliminar.Text = ("Cancelar");
                btnModificar.Visible = false;

                limpiarGrilla(gbDatosPersonales);
                txtNombre.Focus();
            }
            else
            {
                if (string.IsNullOrEmpty(txtNombre.Text) || string.IsNullOrEmpty(txtApellido.Text) || string.IsNullOrEmpty(txtDNI.Text))
                {
                    MessageBox.Show("Debe completar todos los campos para continuar", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(txtDNI.Text, out int dni))
                {
                    MessageBox.Show("El dni debe ser numerico", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtDNI.Focus();
                    txtDNI.SelectAll();
                    return;
                }
                btnAgregar.Text = "Agregar";
                btnModificar.Visible = true;
                btnEliminar.Text = "Eliminar";

                usuario.Add(new Usuarios
                {
                    Id_usuario = (usuario.Any()) ? usuario.Max(x => x.Id_usuario) + 1 : 1,
                    Apellido = txtApellido.Text,
                    Nombre = txtNombre.Text,
                    DNI = dni,
                    Telefono = int.TryParse(txtTelefono.Text, out int telefono) ? telefono : 0,
                    Direccion = txtDireccion.Text
                });

                actualizarGrilla();
                limpiarGrilla(this);
                dgvUsuarios.Focus();
            }
        }

        private void bloquearControles(Control control)
        {
            foreach (Control item in control.Controls)
            {
                if (item is TextBox txt) txt.Enabled = false;
                if (item is ComboBox cmb) cmb.Enabled = false;
            }
        }

        private void cmbBuscar_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nombreSeleccionado = cmbBuscar.SelectedItem.ToString();

            Usuarios seleccinado = usuario.FirstOrDefault(u => u.Nombre == nombreSeleccionado);
            if (seleccinado != null)
            {
                
                txtNombre.Text = seleccinado.Nombre;
                txtApellido.Text = seleccinado.Apellido;
                txtDNI.Text = seleccinado.DNI.ToString();
                
            }
        }
    }
}
