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
    public partial class frmCompras : Form
    {
        private Usuario _Usuario;
        public frmCompras(Usuario oUsuario = null)
        {
            _Usuario = oUsuario;
            
            InitializeComponent();
        }

        private void frmCompras_Load(object sender, EventArgs e)
        {

            //para que aparezca la fecha actual en el textbox
            txtFecha.Text = DateTime.Now.ToString("dd/MM/yyyy");

            cboTipoDocumento.Items.Add(new OpcionCombobox() { Valor = "Boleta", Texto = "Boleta" });
            cboTipoDocumento.Items.Add(new OpcionCombobox() { Valor = "Factura", Texto = "Factura" });
            cboTipoDocumento.DisplayMember = "Texto";
            cboTipoDocumento.ValueMember = "Valor";
            cboTipoDocumento.SelectedIndex = 0;

            txtIdProducto.Text = "0";
            txtIdProveedor.Text = "0";
        }
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            var modal = new md_Proveedor();
            var result = modal.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtIdProveedor.Text = modal._proveedor.IdProveedor.ToString();
                txtDocumentoProveedor.Text = modal._proveedor.Documento;
                txtRazonSocial.Text = modal._proveedor.RazonSocial;
            }
            else
                txtDocumentoProveedor.Select();
        }
        private void btnBuscarProducto_Click(object sender, EventArgs e)
        {
            var modal = new md_Producto();
            var result = modal.ShowDialog();

            if (result == DialogResult.OK)
            {
                txtIdProducto.Text = modal._producto.IdProducto.ToString();
                txtCodProducto.Text = modal._producto.Codigo;
                txtProducto.Text = modal._producto.Nombre;
                txtPrecioCompra.Select();
            }
            else
                txtCodProducto.Select();
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
                    txtPrecioCompra.Select();
                }
                else
                {
                    txtCodProducto.BackColor = Color.MistyRose;
                    txtIdProducto.Text = "0";
                    txtProducto.Text = "";
                }
            }
        }
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            decimal precioCompra = 0;
            decimal precioVenta = 0;
            bool existeProducto = false;

            if (int.Parse(txtIdProducto.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un Producto", "Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            if (!decimal.TryParse(txtPrecioCompra.Text,out precioCompra))
            {
                MessageBox.Show("Precio Compra - Formato de moneda incorrecto","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                txtPrecioCompra.Select();
                return;
            }

            if (!decimal.TryParse(txtPrecioVenta.Text, out precioVenta))
            {
                MessageBox.Show("Precio Venta - Formato de moneda incorrecto", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPrecioCompra.Select();
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
                precioCompra.ToString("0.00"),
                precioVenta.ToString("0.00"),
                numericUpDownCantidad.Value.ToString(),
                (precioCompra * numericUpDownCantidad.Value).ToString()

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
            txtPrecioCompra.Text = "";
            txtPrecioVenta.Text = "";
            numericUpDownCantidad.Value = 1;
        }
        private void CalcularTotal()
        {
            decimal total = 0;
            if (dataGridView1.Rows.Count > 0 )
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                    total += Convert.ToDecimal(row.Cells["SubTotal"].Value);
            }
            txtTotal.Text = total.ToString("0.00");
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if (e.ColumnIndex == 6)
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
                }
            }
        }
        /// <summary>
        /// metodo para que la caja de texto no permita letras.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtPrecioCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {       //esta condicion permite no poder empezar a dijitar un punto
                if (txtPrecioCompra.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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
        private void txtPrecioVenta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {
                e.Handled = false;
            }
            else
            {       //esta condicion permite no poder empezar a dijitar un punto
                if (txtPrecioVenta.Text.Trim().Length == 0 && e.KeyChar.ToString() == ".")
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
        private void txtRegistrar_Click(object sender, EventArgs e)
        {
            if(Convert.ToInt32(txtIdProveedor.Text) == 0)
            {
                MessageBox.Show("Debe seleccionar un Proveedor","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            if (dataGridView1.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar productos a la compra ", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            DataTable detalleCompra = new DataTable();

            detalleCompra.Columns.Add("IdProducto", typeof(int));
            detalleCompra.Columns.Add("PrecioCompra", typeof(decimal));
            detalleCompra.Columns.Add("PrecioVenta", typeof(decimal));
            detalleCompra.Columns.Add("Cantidad", typeof(int));
            detalleCompra.Columns.Add("Total", typeof(decimal));

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                detalleCompra.Rows.Add(new object[] { 
                
                  Convert.ToInt32(row.Cells["IdProducto"].Value.ToString()),
                   Convert.ToDecimal(row.Cells["PrecioCompra"].Value.ToString()),
                   Convert.ToDecimal(row.Cells["PrecioVenta"].Value.ToString()),
                  Convert.ToInt32(row.Cells["Cantidad"].Value.ToString()),
                  Convert.ToDecimal( row.Cells["SubTotal"].Value.ToString())
                
                });
            }

            int correlativo = new CN_Compra().ObtenerCorrelativo();
            string numerodocumento = string.Format("{0:00000}", correlativo);

            Compra oCompra = new Compra()
            {
                oUsuario = new Usuario { IdUsuario = _Usuario.IdUsuario },
                oProveedor = new Proveedor { IdProveedor = Convert.ToInt32(txtIdProveedor.Text) },
                TipoDocumento= ((OpcionCombobox)(cboTipoDocumento.SelectedItem)).Texto,
                NumeroDocumento=numerodocumento,
                MontoTotal=Convert.ToDecimal(txtTotal.Text)

            };

            string mensaje = string.Empty;

            bool respuesta = new CN_Compra().RegistarCompra(oCompra,detalleCompra,out mensaje);

            if (respuesta)
            {
                var result = MessageBox.Show("Numero de compra generada:\n" + numerodocumento + "\n\n¿Desea copiar al portapapeles?", "Mensaje", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                if (result == DialogResult.Yes)
                    Clipboard.SetText(numerodocumento);

                txtIdProveedor.Text = "0";
                txtDocumentoProveedor.Text = "";
                txtRazonSocial.Text = "";
                dataGridView1.Rows.Clear();
                CalcularTotal();
            }
        }
    }
}
