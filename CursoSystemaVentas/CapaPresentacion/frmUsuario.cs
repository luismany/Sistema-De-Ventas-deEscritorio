using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmUsuario : Form
    {
        public frmUsuario()
        {
            InitializeComponent();
        }

        private void frmUsuario_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombobox() { Valor=1,Texto="Activo"});
            cboEstado.Items.Add(new OpcionCombobox() { Valor=0,Texto="Inactivo"});
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Cargar combobox Rol
            List<Rol> listaRol = new CN_Rol().ListaRol();

            foreach (Rol item in listaRol)
            {
                cboRol.Items.Add(new OpcionCombobox() {Valor=item.IdRol,Texto=item.Descripcion });
            }
            cboRol.DisplayMember = "Texto";
            cboRol.ValueMember = "Valor";
            cboRol.SelectedIndex = 0;

            //cargar elcomboox sin usar la clase OpcionCombobox
            //cboRol.DataSource = listaRol;
            //cboRol.DisplayMember = "Descripcion";
            //cboRol.ValueMember = "IdRol";
            //cboRol.SelectedIndex = 0;

            //Cargar combobox Buscarpor
            foreach (DataGridViewColumn columna in dgvData.Columns )
            {
                if (columna.Visible && columna.Name != "btnSeleccionar") cboBuscar.Items.Add(new OpcionCombobox() { Valor = columna.Name, Texto = columna.HeaderText });
                
            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;

            //Cargar Datagridview con la data
            List<Usuario> listaUsuario = new CN_Usuario().ListarUsuario();

            foreach (Usuario item in listaUsuario)
            {

                dgvData.Rows.Add(new object[] {"",item.IdUsuario,item.Documento,item.NombreCompleto,item.Correo,item.Clave,
                item.oRol.IdRol,item.oRol.Descripcion,
                item.Estado==true ? 1:0,
                item.Estado==true ? "Activo":"Inactivo"
                });
            }
        }

        private void txtGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            
            Usuario oUsuario = new Usuario()
            {
                IdUsuario = Convert.ToInt32(txtId.Text),
                Documento = txtDocumento.Text,
                NombreCompleto = txtNombre.Text,
                Correo = txtCorreo.Text,
                Clave = txtContraseña.Text,
                oRol = new Rol() { IdRol = Convert.ToInt32(((OpcionCombobox)cboRol.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((OpcionCombobox)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (oUsuario.IdUsuario == 0)
            {
                int idGenerado = new CN_Usuario().AgregarUsuario(oUsuario, out mensaje);

                if (idGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] { "",idGenerado,txtDocumento.Text,txtNombre.Text,txtCorreo.Text,txtContraseña.Text,
            ((OpcionCombobox)cboRol.SelectedItem).Valor.ToString(),
            ((OpcionCombobox)cboRol.SelectedItem).Texto.ToString(),
            ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString(),
            ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString()
            });

                    Limpiar();
                }
                else MessageBox.Show(mensaje);

            }
            else
            {
                bool resultado = new CN_Usuario().ModificarUsuario(oUsuario, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["IdUsuario"].Value = txtId.Text;
                    row.Cells["Documento"].Value = txtDocumento.Text;
                    row.Cells["NombreCompleto"].Value = txtNombre.Text;
                    row.Cells["Correo"].Value = txtCorreo.Text;
                    row.Cells["Clave"].Value = txtContraseña.Text;
                    row.Cells["IdRol"].Value = ((OpcionCombobox)cboRol.SelectedItem).Valor.ToString();
                    row.Cells["Rol"].Value = ((OpcionCombobox)cboRol.SelectedItem).Texto.ToString();
                    row.Cells["EstadoValor"].Value = ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString();

                    Limpiar();

                }
                else MessageBox.Show(mensaje);

            }

 
        }

        private void Limpiar()
        {
            txtIndice.Text = "";
            txtId.Text = "0";
            txtDocumento.Text = "";
            txtNombre.Text = "";
            txtCorreo.Text = "";
            txtContraseña.Text = "";
            txtConfirmarContraseña.Text = "";
            cboRol.SelectedIndex = 0;
            cboEstado.SelectedIndex = 0;
        }


        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvData.Columns[e.ColumnIndex].Name=="btnSeleccionar")
            {
                int indice = e.RowIndex;

                if(indice>=0)
                {
                    txtIndice.Text = indice.ToString();
                    txtId.Text = dgvData.CurrentRow.Cells["IdUsuario"].Value.ToString();
                    txtDocumento.Text = dgvData.CurrentRow.Cells["Documento"].Value.ToString();
                    txtNombre.Text = dgvData.CurrentRow.Cells["NombreCompleto"].Value.ToString();
                    txtCorreo.Text = dgvData.CurrentRow.Cells["Correo"].Value.ToString();
                    txtContraseña.Text = dgvData.CurrentRow.Cells["Clave"].Value.ToString();
                    txtConfirmarContraseña.Text = dgvData.CurrentRow.Cells["Clave"].Value.ToString();

                    foreach (OpcionCombobox oc in cboRol.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdRol"].Value))
                        {
                            int indiceCombo = cboRol.Items.IndexOf(oc);
                            cboRol.SelectedIndex = indiceCombo;
                            break;
                        }

                    }

                    foreach (OpcionCombobox oc in cboEstado.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["EstadoValor"].Value))
                        {
                            int indiceCombo = cboEstado.Items.IndexOf(oc);
                            cboEstado.SelectedIndex = indiceCombo;
                            break;
                        }

                    }

                }
                
            }
  
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            
            if (Convert.ToInt32(txtId.Text)!=0)
            {
                
                if ( MessageBox.Show("¿Seguro que desea eliminar este usuario?","Mensaje",MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                    string mensaje = string.Empty;
                    int IdUsuario = Convert.ToInt32(txtId.Text);
                
                bool resultado = new CN_Usuario().EliminarUsuario(IdUsuario,out mensaje);

                    if (resultado) {
                        dgvData.Rows.RemoveAt(Convert.ToInt32(txtIndice.Text));
                        Limpiar();
                    } 
                else MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
             }
                
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {

            string columnaFiltro = ((OpcionCombobox)cboBuscar.SelectedItem).Valor.ToString();

            if (dgvData.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    //si el valor de la columnaFiltro contiene el valor  de txtbusqueda
                    if (row.Cells[columnaFiltro].Value.ToString().Trim().ToUpper().Contains(txtbusqueda.Text.Trim().ToUpper()))
                        row.Visible = true;
                    else
                        row.Visible = false;
                }
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
          
        }
    }
}
