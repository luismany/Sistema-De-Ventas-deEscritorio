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
    public partial class frmDetalleVenta : Form
    {
        public frmDetalleVenta()
        {
            InitializeComponent();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            Venta oVenta = new CN_Venta().ObtenerVenta(txtbusqueda.Text);

            if (oVenta.IdVenta != 0)
            {
                txtFecha.Text = oVenta.FechaCreacion;
                txtTipoDocumento.Text = oVenta.TipoDocumento;
                txtUsuario.Text = oVenta.oUsuario.NombreCompleto;
                txtDocumentoCliente.Text = oVenta.DocumentoCliente;
                txtNombreCliente.Text = oVenta.NombreCliente;
                txtNumeroDocumento.Text = oVenta.NumeroDocumento;
                dataGridView1.Rows.Clear();
                foreach (DetalleVenta dv in oVenta.oDetalleVenta)
                {
                    dataGridView1.Rows.Add(new object[] { 
                    
                        dv.oProducto.Nombre,
                        dv.PrecioVenta,
                        dv.Cantidad,
                        dv.SubTotal

                    });
                }
                txtMontoTotal.Text = oVenta.MontoTotal.ToString();
                txtMontoPago.Text = oVenta.MontoPago.ToString();
                txtCambio.Text = oVenta.MontoCambio.ToString();
            }
        }

        private void btnDescargarPdf_Click(object sender, EventArgs e)
        {
            if (txtbusqueda.Text == "")
            {
                MessageBox.Show("No hay datos para exportar", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string texto_Html = Properties.Resources.PlantillaVenta.ToString();

            Negocio oDatos = new CN_Negocio().CargarDatos();

            texto_Html = texto_Html.Replace("@nombrenegocio", oDatos.Nombre.ToUpper());
            texto_Html = texto_Html.Replace("@docnegocio", oDatos.RUC);
            texto_Html = texto_Html.Replace("@direcnegocio", oDatos.Direccion);

            texto_Html = texto_Html.Replace("@tipodocumento", txtTipoDocumento.Text);
            texto_Html = texto_Html.Replace("@numerodocumento", txtNumeroDocumento.Text);

            texto_Html = texto_Html.Replace("@doccliente", txtDocumentoCliente.Text);
            texto_Html = texto_Html.Replace("@nombrecliente", txtNombreCliente.Text);
            texto_Html = texto_Html.Replace("@fecharegistro", txtFecha.Text);
            texto_Html = texto_Html.Replace("@usuarioregistro", txtUsuario.Text);

            string fila = string.Empty;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                fila += "<tr>";
                fila += "<td>" + row.Cells["Producto"].Value.ToString() + "</td>";
                fila += "<td>" + row.Cells["Precio"].Value.ToString() + "</td>";
                fila += "<td>" + row.Cells["Cantidad"].Value.ToString() + "</td>";
                fila += "<td>" + row.Cells["SubTotal"].Value.ToString() + "</td>";
                fila += "</tr>";
            }

            texto_Html = texto_Html.Replace("@filas", fila);
            texto_Html = texto_Html.Replace("@montototal", txtMontoTotal.Text);
            texto_Html = texto_Html.Replace("@pagocon", txtMontoPago.Text);
            texto_Html = texto_Html.Replace("@cambio", txtCambio.Text);

            SaveFileDialog savefile = new SaveFileDialog();
            savefile.FileName = string.Format("Venta_{0}.pdf", txtNumeroDocumento.Text);
            savefile.Filter = "Pdf Files|*.pdf";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (FileStream stream = new FileStream(savefile.FileName, FileMode.Create))
                {

                    Document pdfDoc = new Document(PageSize.A4, 25, 25, 25, 25);

                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    bool obtenido = true;
                    byte[] byteImage = new CN_Negocio().ObtenerLogo(out obtenido);

                    if (obtenido)
                    {
                        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(byteImage);
                        img.ScaleToFit(60, 60);
                        img.Alignment = iTextSharp.text.Image.UNDERLYING;
                        img.SetAbsolutePosition(pdfDoc.Left, pdfDoc.GetTop(51));
                        pdfDoc.Add(img);
                    }

                    using (StringReader sr = new StringReader(texto_Html))
                    {
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    }

                    pdfDoc.Close();
                    stream.Close();
                    MessageBox.Show("Documento Generado", "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
