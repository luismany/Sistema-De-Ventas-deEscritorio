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
    public partial class frmCategoria : Form
    {
        public frmCategoria()
        {
            InitializeComponent();
        }

        private void frmCategoria_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombobox() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombobox() { Valor = 0, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible && columna.Name != "btnSeleccionar") cboBuscar.Items.Add(new OpcionCombobox() { Valor = columna.Name, Texto = columna.HeaderText });

            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;

            List<Categoria> listaCategorias = new CN_Categoria().ListaCategoria();

            foreach (Categoria item in listaCategorias)
            {

                dgvData.Rows.Add(new object[] {"",item.IdCategoria ,item.Descripcion,
                item.Estado==true ? 1:0,
                item.Estado==true ? "Activo":"Inactivo"
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Categoria oCategoria = new Categoria()
            {
                IdCategoria = Convert.ToInt32(txtIdCategoria.Text),
                Descripcion = txtDescripcion.Text,
                Estado = Convert.ToInt32(((OpcionCombobox)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (oCategoria.IdCategoria == 0)
            {
                int idGenerado = new CN_Categoria().AgregarCategoria(oCategoria, out mensaje);

                if (idGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] { "",idGenerado,txtDescripcion.Text,
                    ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString(),
                    ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString()
                        });

                    Limpiar();
                }
                else MessageBox.Show(mensaje);

            }
            else
            {
                bool resultado = new CN_Categoria().ModificarCategoria(oCategoria, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["IdCategoria"].Value = txtIdCategoria.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;
                    row.Cells["EstadoValor"].Value = ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString();
                    row.Cells["Estado"].Value = ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString();

                    Limpiar();

                }
                else MessageBox.Show(mensaje);

            }
        }

        public void Limpiar()
        {
            txtIdCategoria.Text = "0";
            txtIndice.Text = "";
            txtDescripcion.Text = "";
            cboEstado.SelectedIndex = 0;
        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvData.Columns[e.ColumnIndex].Name == "btnSeleccionar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    txtIndice.Text = indice.ToString();
                    txtIdCategoria.Text = dgvData.CurrentRow.Cells["IdCategoria"].Value.ToString();
                    txtDescripcion.Text = dgvData.CurrentRow.Cells["Descripcion"].Value.ToString();


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
            if (Convert.ToInt32(txtIdCategoria.Text) != 0)
            {

                if (MessageBox.Show("¿Seguro que desea eliminar esta Categoria?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    int IdCategoria = Convert.ToInt32(txtIdCategoria.Text);

                    bool resultado = new CN_Categoria().EliminarCategoria(IdCategoria, out mensaje);

                    if (resultado)
                    {
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

            foreach (DataGridViewRow row in dgvData.Rows)
            {
                row.Visible = true;
            }
        }
    }
}
