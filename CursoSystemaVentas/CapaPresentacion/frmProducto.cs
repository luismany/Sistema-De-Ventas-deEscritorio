using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Utilidades;
using ClosedXML.Excel;
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
    public partial class frmProducto : Form
    {
        public frmProducto()
        {
            InitializeComponent();
        }

        private void frmProducto_Load(object sender, EventArgs e)
        {
            cboEstado.Items.Add(new OpcionCombobox() { Valor = 1, Texto = "Activo" });
            cboEstado.Items.Add(new OpcionCombobox() { Valor = 0, Texto = "Inactivo" });
            cboEstado.DisplayMember = "Texto";
            cboEstado.ValueMember = "Valor";
            cboEstado.SelectedIndex = 0;

            //Cargar combobox Caategoria
            List<Categoria> listaCategoria = new CN_Categoria().ListaCategoria();

            foreach (Categoria item in listaCategoria)
            {
                cboCategoria.Items.Add(new OpcionCombobox() { Valor = item.IdCategoria, Texto = item.Descripcion });
            }
            cboCategoria.DisplayMember = "Texto";
            cboCategoria.ValueMember = "Valor";
            cboCategoria.SelectedIndex = 0;

            //Cargar combobox Buscarpor
            foreach (DataGridViewColumn columna in dgvData.Columns)
            {
                if (columna.Visible && columna.Name != "btnSeleccionar") cboBuscar.Items.Add(new OpcionCombobox() { Valor = columna.Name, Texto = columna.HeaderText });

            }
            cboBuscar.DisplayMember = "Texto";
            cboBuscar.ValueMember = "Valor";
            cboBuscar.SelectedIndex = 0;

            //Cargar Datagridview con la data
            List<Producto> listaProducto = new CN_Producto().ListarProducto();

            foreach (Producto item in listaProducto)
            {

                dgvData.Rows.Add(new object[] {"",item.IdProducto,item.Codigo,item.Nombre,item.Descripcion,item.Stock,
                item.PrecioCompra,item.PrecioVenta,
                item.oCategoria.IdCategoria,item.oCategoria.Descripcion,
                item.Estado==true ? 1:0,
                item.Estado==true ? "Activo":"Inactivo"
                });
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Producto oProducto = new Producto()
            {
                IdProducto = Convert.ToInt32(txtId.Text),
                Codigo = txtCodigo.Text,
                Nombre = txtNombre.Text,
                Descripcion = txtDescripcion.Text,
                Stock = Convert.ToInt32(txtStock.Text),
                PrecioCompra = Convert.ToDecimal(txtPrecioCompra.Text),
                PrecioVenta = Convert.ToDecimal(txtPrecioVenta.Text),
                oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(((OpcionCombobox)cboCategoria.SelectedItem).Valor) },
                Estado = Convert.ToInt32(((OpcionCombobox)cboEstado.SelectedItem).Valor) == 1 ? true : false
            };

            if (oProducto.IdProducto == 0)
            {
                int idGenerado = new CN_Producto().AgregarProducto(oProducto, out mensaje);

                if (idGenerado != 0)
                {
                    dgvData.Rows.Add(new object[] { "",idGenerado,txtCodigo.Text,txtNombre.Text,txtDescripcion.Text,txtStock.Text,
                        txtPrecioCompra.Text,txtPrecioVenta.Text,
                    ((OpcionCombobox)cboCategoria.SelectedItem).Valor.ToString(),
                    ((OpcionCombobox)cboCategoria.SelectedItem).Texto.ToString(),
                    ((OpcionCombobox)cboEstado.SelectedItem).Valor.ToString(),
                    ((OpcionCombobox)cboEstado.SelectedItem).Texto.ToString()
                    });

                    Limpiar();
                }
                else MessageBox.Show(mensaje);

            }
            else
            {
                bool resultado = new CN_Producto().ModificarProducto(oProducto, out mensaje);

                if (resultado)
                {
                    DataGridViewRow row = dgvData.Rows[Convert.ToInt32(txtIndice.Text)];
                    row.Cells["IdProducto"].Value = txtId.Text;
                    row.Cells["Codigo"].Value = txtCodigo.Text;
                    row.Cells["Nombre"].Value = txtNombre.Text;
                    row.Cells["Descripcion"].Value = txtDescripcion.Text;
                    row.Cells["Stock"].Value = txtStock.Text;
                    row.Cells["PrecioCompra"].Value = txtPrecioCompra.Text;
                    row.Cells["PrecioVenta"].Value = txtPrecioVenta.Text;
                    row.Cells["IdCategoria"].Value = ((OpcionCombobox)cboCategoria.SelectedItem).Valor.ToString();
                    row.Cells["Categoria"].Value = ((OpcionCombobox)cboCategoria.SelectedItem).Texto.ToString();
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
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtStock.Text = "";
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            cboCategoria.SelectedIndex = 0;
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
                    txtId.Text = dgvData.CurrentRow.Cells["IdProducto"].Value.ToString();
                    txtCodigo.Text = dgvData.CurrentRow.Cells["Codigo"].Value.ToString();
                    txtNombre.Text = dgvData.CurrentRow.Cells["Nombre"].Value.ToString();
                    txtDescripcion.Text = dgvData.CurrentRow.Cells["Descripcion"].Value.ToString();
                    txtStock.Text = dgvData.CurrentRow.Cells["Stock"].Value.ToString();
                    txtPrecioCompra.Text = dgvData.CurrentRow.Cells["PrecioCompra"].Value.ToString();
                    txtPrecioVenta.Text = dgvData.CurrentRow.Cells["PrecioVenta"].Value.ToString();

                    foreach (OpcionCombobox oc in cboCategoria.Items)
                    {
                        if (Convert.ToInt32(oc.Valor) == Convert.ToInt32(dgvData.Rows[indice].Cells["IdCategoria"].Value))
                        {
                            int indiceCombo = cboCategoria.Items.IndexOf(oc);
                            cboCategoria.SelectedIndex = indiceCombo;
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
            if (Convert.ToInt32(txtId.Text) != 0)
            {

                if (MessageBox.Show("¿Seguro que desea eliminar este Producto?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string mensaje = string.Empty;
                    int IdProducto = Convert.ToInt32(txtId.Text);

                    bool resultado = new CN_Producto().EliminarProducto(IdProducto, out mensaje);

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

        private void txtGuardarExcel_Click(object sender, EventArgs e)
        {
            //exportar datos a excel
            if (dgvData.Rows.Count < 1)
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DataTable dt = new DataTable();

                foreach (DataGridViewColumn columna in dgvData.Columns)
                {
                    if (columna.HeaderText != "" && columna.Visible)
                        dt.Columns.Add(columna.HeaderText, typeof(string));
                }

                foreach (DataGridViewRow row in dgvData.Rows)
                {
                    if (row.Visible)
                        dt.Rows.Add(new object[] {
                            row.Cells[2].Value.ToString(),
                            row.Cells[3].Value.ToString(),
                            row.Cells[4].Value.ToString(),
                            row.Cells[5].Value.ToString(),
                            row.Cells[6].Value.ToString(),
                            row.Cells[7].Value.ToString(),
                            row.Cells[9].Value.ToString(),
                            row.Cells[11].Value.ToString(),

                        });
                }

                SaveFileDialog savefile = new SaveFileDialog();
                savefile.FileName = string.Format("ReporteProducto_{0}.xlsx", DateTime.Now.ToString("ddMMyyyyHHmmss"));
                savefile.Filter = "Excel Files | *.xlsx";

                if (savefile.ShowDialog() == DialogResult.OK)
                {

                    try
                    {
                        XLWorkbook wb = new XLWorkbook();
                        var hoja = wb.Worksheets.Add(dt, "Informe");
                        hoja.ColumnsUsed().AdjustToContents();
                        wb.SaveAs(savefile.FileName);
                        MessageBox.Show("Reporte Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }
                    catch
                    {
                        MessageBox.Show("Error al generar reporte", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }

                }

            }
        }

    }
}