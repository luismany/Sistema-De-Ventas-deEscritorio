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
                bool respuesta = new CN_Venta().RestarStock(Convert.ToInt32(txtIdProducto.Text),Convert.ToInt32( numericUpDownCantidad.Value.ToString()));

                if (respuesta)
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
                    bool respuesta = new CN_Venta().SumarStock(
                        Convert.ToInt32(dataGridView1.Rows[indice].Cells["IdProducto"].Value.ToString()),
                        Convert.ToInt32(dataGridView1.Rows[indice].Cells["Cantidad"].Value.ToString()));

                    if(respuesta)
                    {
                        dataGridView1.Rows.RemoveAt(indice);
                        CalcularTotal();
                        CalcularCambio();
                    }
                    
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

            if (txtPagarCon.Text.Trim() == "")
                txtPagarCon.Text = "0";

            if (Convert.ToDecimal(txtPagarCon.Text) < Convert.ToDecimal(txtTotalaPagar.Text))
            {
                MessageBox.Show("la cantidad con la que paga es menor al monto de la factura", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCambio.Text = "0";
            }
                
            else
            {
                Cambio = Convert.ToDecimal(txtPagarCon.Text) - Convert.ToDecimal(txtTotalaPagar.Text);
                txtCambio.Text = Cambio.ToString("0.00");
            }
        }
            

        private void txtPagarCon_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                CalcularCambio();
        }

        private void btnCrearVenta_Click(object sender, EventArgs e)
        {
            if (txtDocumentoCliennte.Text == "")
            {
                MessageBox.Show("Debe seleccionar el documento de un cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtNombreCompleto.Text == "")
            {
                MessageBox.Show("Debe ingresar el nommbre de un cliente", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if(dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("Debe agregar almenos un producto para la venta", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (txtPagarCon.Text == "")
            {
              MessageBox.Show("Debe agregar la cantidad con la que va a pagar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (Convert.ToDecimal(txtPagarCon.Text) < Convert.ToDecimal(txtTotalaPagar.Text))
            {
                MessageBox.Show("la cantidad con la que paga es menor al monto de la factura", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalle_Venta = new DataTable();

            detalle_Venta.Columns.Add("IdProducto", typeof(int));
            detalle_Venta.Columns.Add("PrecioVenta", typeof(decimal));
            detalle_Venta.Columns.Add("Cantidad", typeof(int));
            detalle_Venta.Columns.Add("SubTotal", typeof(decimal));

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                detalle_Venta.Rows.Add(new object[] { 
                
                    row.Cells["IdProducto"].Value.ToString(),
                    row.Cells["Precio"].Value.ToString(),
                    row.Cells["Cantidad"].Value.ToString(),
                    row.Cells["SubTotal"].Value.ToString()
                });
            }

            int idCorrelativo = new CN_Venta().ObtenerCorrelativo();
            string numeroDocumento = string.Format("{0:00000}", idCorrelativo);
            CalcularCambio();

            Venta oVenta = new Venta()
            {
                oUsuario = new Usuario() { IdUsuario = _Usuario.IdUsuario },
                TipoDocumento=((OpcionCombobox)cboTipoDocumento.SelectedItem).Texto,
                NumeroDocumento=numeroDocumento,
                DocumentoCliente=txtDocumentoCliennte.Text,
                NombreCliente=txtNombreCompleto.Text,
                MontoPago=Convert.ToDecimal(txtPagarCon.Text),
                MontoCambio=Convert.ToDecimal(txtCambio.Text),
                MontoTotal=Convert.ToDecimal(txtTotalaPagar.Text)
            };

            string mensaje = string.Empty;

            bool respuesta = new CN_Venta().RegistarVenta(oVenta,detalle_Venta,out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Numero de venta generada:\n" + numeroDocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                
                if (result == DialogResult.Yes)
                    Clipboard.SetText(numeroDocumento);

                txtDocumentoCliennte.Text = "";
                txtNombreCompleto.Text = "";
                dataGridView1.Rows.Clear();
                CalcularTotal();
                txtPagarCon.Text = "";
                txtCambio.Text = "";
            }
            else
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

        }
    }
}

