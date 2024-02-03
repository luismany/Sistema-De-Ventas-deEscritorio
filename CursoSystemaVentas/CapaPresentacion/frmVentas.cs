using CapaEntidad;
using CapaNegocio;
using CapaPresentacion.Modales;
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
    public partial class frmVentas : Form
    {
        private Usuario _Usuario;
        public frmVentas(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            InitializeComponent();
        }

        private void frmVentas_Load(object sender, EventArgs e)
        {
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            cboTipoDocumento.Items.Add(new OpcionCombobox() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombobox() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            txtIdProducto.Text = "0";
            txtPagarCon.Text = "";
            txtCambio.Text = "";
            txtTotalaPagar.Text = "0";


        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var modal = new md_Cliente();
            var result = modal.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtDocumentoCliennte.Text = modal._cliente.Documento;
                txtNombreCompleto.Text = modal._cliente.NombreCompleto;
            }
            else
                txtDocumentoCliennte.Select();  
        }

        private void btnBuscarProductos_Click(object sender, EventArgs e)
        {
            var modal = new md_Producto();
            var result = modal.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtIdProducto.Text = modal._producto.IdProducto.ToString();
                txtCodProducto.Text = modal._producto.Codigo;
                txtProducto.Text = modal._producto.Nombre;
                txtPrecio.Text = modal._producto.PrecioVenta.ToString();
                txtStock.Text = modal._producto.Stock.ToString();
                numericUpDownCantidad.Select();
            }
            else
                txtCodProducto.Select();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            bool existeProducto = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un Producto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Convert.ToInt32(txtStock.Text) < Convert.ToInt32(numericUpDownCantidad.Value.ToString()))
            {
                MessageBox.Show("La cantidad seleccionada es mayor que el Stock", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            foreach (DataGridViewRow fila in dataGridView1.Rows)
            {
                if (fila.Cells["IdProducto"].Value != null && fila.Cells["IdProducto"].Value.ToString() == txtIdProducto.Text)
                {
                    existeProducto = true;
                    break;
                }
            }

            if (!existeProducto)
            {
                //agrega una fila al datagridview
                dataGridView1.Rows.Add(new object[] {

                txtIdProducto.Text,
                txtProducto.Text,
                txtPrecio.Text,
                numericUpDownCantidad.Value.ToString(),
                (Convert.ToDecimal(txtPrecio.Text) * numericUpDownCantidad.Value).ToString()

                });
                CalcularTotal();
                Limpiar();
                txtCodProducto.Select();

            }

        }

        private void Limpiar()
        {
            txtIdProducto.Text = "0";
            txtCodProducto.Text = "";
            txtCodProducto.BackColor = Color.White;
            txtProducto.Text = "";
            txtPrecio.Text = "";
            txtStock.Text = "";
            numericUpDownCantidad.Value = 1;
        }

        private void CalcularTotal()
        {
            decimal total = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value);
            }
            txtTotalaPagar.Text = total.ToString("0.00");
        }

        private void txtCodProducto_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                Producto oProducto = new CN_Producto().ListarProducto().Where(p => p.Codigo == txtCodProducto.Text && p.Estado == true).FirstOrDefault();

                if (oProducto != null)
                {
                    txtCodProducto.BackColor = Color.Honeydew;
                    txtIdProducto.Text = oProducto.IdProducto.ToString();
                    txtProducto.Text = oProducto.Nombre;
                    txtPrecio.Text = oProducto.PrecioVenta.ToString();
                    txtStock.Text = oProducto.Stock.ToString();
                    numericUpDownCantidad.Select();
                }
                else
                {
                    txtCodProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                    txtPrecio.Text = "";
                    txtStock.Text = "";

                }
            }
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 5)
            {

                e.Paint(e.CellBounds, DataGridViewPaintParts.All);

                var w = Properties.Resources.trash.Width;
                var h = Properties.Resources.trash.Height;
                var x = e.CellBounds.Left + (e.CellBounds.Width - w) / 2;
                var y = e.CellBounds.Top + (e.CellBounds.Height - h) / 2;

                e.Graphics.DrawImage(Properties.Resources.trash, new Rectangle(x, y, w, h));
                e.Handled = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "btnEliminar")
            {
                int indice = e.RowIndex;

                if (indice >= 0)
                {
                    dataGridView1.Rows.RemoveAt(indice);
                    CalcularTotal();
                    CalcularCambio();
                }
            }
        }

        private void txtPagarCon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {       //esta condicion permite no poder empezar a dijitar un punto
                if (txtPagarCon.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
                {
                    e.Handled = true;
                }
                else
                {       //esta condicion permite habilitar la tecla de borar
                    if (Char.IsControl(e.KeyChar) || e.KeyChar.ToString() == ".")
                    {
                        e.Handled = false;
                    }
                    else
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void CalcularCambio()
        {
            decimal Cambio = 0;

            Cambio = Convert.ToDecimal(txtPagarCon.Text) - Convert.ToDecimal(txtTotalaPagar.Text);
            txtCambio.Text = Cambio.ToString("0.00");
        }

        private void txtPagarCon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                CalcularCambio();
        }
    }
}
