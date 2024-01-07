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
                if (fila.Cells["IdProducto"].Value.ToString()== txtIdProducto.Text)
                {
                    existeProducto = true;
                    break;
                }
            }

            if (!existeProducto)
            {
                dataGridView1.Rows.Add(new object[] { 
                
                txtIdProducto.Text,
                txtProducto.Text,
                precioCompra.ToString("0.00"),
                precioVenta.ToString("0.00"),
                numericUpDownCantidad.Value.ToString(),
                (precioCompra * numericUpDownCantidad.Value).ToString()

                });
            }
        }
    }
}
