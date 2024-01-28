using CapaEntidad;
using CapaNegocio;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class frmDetalleCompra : Form
    {
        public frmDetalleCompra()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Compra oCompra = new CN_Compra().ObtenerCompra(txtbusqueda.Text);

            if(oCompra.IdCompra != 0)
            {
                txtNumeroDocumento.Text = oCompra.NumeroDocumento;
                txtFecha.Text = oCompra.FechaCreacion;
                txtTipoDocumento.Text = oCompra.TipoDocumento;
                txtUsuario.Text = oCompra.oUsuario.NombreCompleto;
                txtDocumentoProveedor.Text = oCompra.oProveedor.Documento;
                txtRazonSocial.Text = oCompra.oProveedor.RazonSocial;
                dataGridView1.Rows.Clear();
                foreach (DetalleCompra dc in oCompra.oDetalleCompra)
                {
                    dataGridView1.Rows.Add(new object[] {
                        dc.oProducto.Nombre,
                        dc.PrecioCompra,
                        dc.Cantidad,
                        dc.Total
                    
                    });
                }

                txtMontoTotal.Text = oCompra.MontoTotal.ToString();
            }
        }

        private void btnLimpiarBuscador_Click(object sender, EventArgs e)
        {
            txtbusqueda.Text = "";
            txtNumeroDocumento.Text = "";
            txtFecha.Text = "";
            txtTipoDocumento.Text = "";
            txtUsuario.Text = "";
            txtDocumentoProveedor.Text = "";
            txtRazonSocial.Text = "";
            dataGridView1.Rows.Clear();
            txtMontoTotal.Text = "0.00";

        }

        private void btnDescargarPdf_Click(object sender, EventArgs e)
        {
            if (txtbusqueda.Text == "")
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string texto_Html = Properties.Resources.PlantillaCompra.ToString();

            Negocio oDatos = new CN_Negocio().CargarDatos();

            texto_Html = texto_Html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            texto_Html = texto_Html.Replace("@docnegocio", oDatos.RUC);
            texto_Html = texto_Html.Replace("@direcnegocio", oDatos.Direccion);

            texto_Html = texto_Html.Replace("@tipodocumento", txtTipoDocumento.Text);
            texto_Html = texto_Html.Replace("@numerodocumento", txtNumeroDocumento.Text);

            texto_Html = texto_Html.Replace("@docproveedor", txtDocumentoProveedor.Text);
            texto_Html = texto_Html.Replace("@nombreproveedor", txtRazonSocial.Text);
            texto_Html = texto_Html.Replace("@fecharegistro", txtFecha.Text);
            texto_Html = texto_Html.Replace("@usuarioregistro", txtUsuario.Text);

            string fila = string.Empty;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                fila += "<tr>";
                fila += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                fila += "<td>" + row.Cells["PrecioCompra"].Value.ToString() + "</td>";
                fila += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                fila += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                fila += "</tr>";
            }

            texto_Html = texto_Html.Replace("@filas", fila);
            texto_Html = texto_Html.Replace("@montototal", txtMontoTotal.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Compra_{0}.pdf", txtNumeroDocumento.Text);
            savefile.Filter = "Pdf Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                FileStream stream = new FileStream(savefile.FileName, FileMode.Create);//archivo en memoria

                Document pdfdoc = new Document(PageSize.A4, 25, 25, 25, 25);

                PdfWriter writer = PdfWriter.GetInstance(pdfdoc,stream);
                pdfdoc.Open();

                bool obtenido = true;
                byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);

                if (obtenido)
                {
                    iTextSharp.text.Image img= iTextSharp.text.Image.GetInstance(byteImage);
                    img.ScaleToFit(60,60);
                    img.Alignment = iTextSharp.text.Image.UNDERLYING;//imagen sobre el texto
                    img.SetAbsolutePosition(pdfdoc.Left,pdfdoc.GetTop(51));
                    pdfdoc.Add(img);
                }

                StringReader sr = new StringReader(texto_Html);
                XMLWorkerHelper.GetInstance().ParseXHtml(writer,pdfdoc,sr);
                pdfdoc.Close();
                writer.Close();
                MessageBox.Show("Documento generado con exito","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }

        }
    }
}
