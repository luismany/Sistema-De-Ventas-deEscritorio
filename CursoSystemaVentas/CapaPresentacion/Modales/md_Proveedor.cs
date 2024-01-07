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

namespace CapaPresentacion.Modales
{
    public partial class md_Proveedor : Form
    {
        public Proveedor _proveedor { get; set; }
        public md_Proveedor()
        {
            InitializeComponent();
        }

        private void md_Proveedor_Load(object sender, EventArgs e)
        {
            //cargar combobox
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible ) 
                    cboBuscar.Items.Add(new OpcionCombobox() { Valor = columna.Name, Texto = columna.HeaderText });

            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;

            //Cargar Datagridview con la data
            List<Proveedor> listaProveedor = new CN_Proveedor().ListaProveedor();

            foreach (Proveedor item in listaProveedor)
            {

                dgvData.Rows.Add(new object[] {item.IdProveedor,item.Documento,item.RazonSocial });
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

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int iroW = e.RowIndex;
            int icolumn = e.ColumnIndex;

            if (iroW >= 0 && icolumn > 0)
            {
                _proveedor = new Proveedor()
                {
                    IdProveedor = Convert.ToInt32(dgvData.Rows[iroW].Cells["IdProveedor"].Value.ToString()),
                    Documento = dgvData.Rows[iroW].Cells["Documento"].Value.ToString(),
                    RazonSocial = dgvData.Rows[iroW].Cells["RazonSocial"].Value.ToString()

                };

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
