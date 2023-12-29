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
using CapaEntidad;
using CapaNegocio;

namespace CapaPresentacion
{
    public partial class frmNegocio : Form
    {
        public frmNegocio()
        {
            InitializeComponent();
        }

        /// <summary>
        /// metodo que comvierte un array de byte en imagen
        /// </summary>
        /// <param name="imageBytes"></param>
        /// <returns></returns>
        public Image ByteToImage(byte[] imageBytes)
        {
            MemoryStream ms = new MemoryStream();
            ms.Write(imageBytes,0,imageBytes.Length);
            Image image = new Bitmap(ms);
            return image;
        }


        private void frmNegocio_Load(object sender, EventArgs e)
        {
            bool obtenido = true;

            byte[] byteImagen = new CN_Negocio().ObtenerLogo(out obtenido);
            if (obtenido)
                picLogo.Image = ByteToImage(byteImagen);

            var negocio = new CN_Negocio().CargarDatos();

            txtNombre.Text = negocio.Nombre;
            txtRuc.Text = negocio.RUC;
            txtDireccion.Text = negocio.Direccion;
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;

            Negocio oNegocio = new Negocio()
            {
                Nombre = txtNombre.Text,
                RUC = txtRuc.Text,
                Direccion = txtDireccion.Text
            };

            bool respuesta = new CN_Negocio().GuardarDatos(oNegocio,out mensaje);

            if(respuesta)
                MessageBox.Show("Datos Guardados con exito","Mensaje", MessageBoxButtons.OK,MessageBoxIcon.Information);
           else
                MessageBox.Show(mensaje, "Mensaje", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnSubir_Click(object sender, EventArgs e)
        {
            string mensaje = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.FileName = "Files|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                byte[] byteImage = File.ReadAllBytes(ofd.FileName);
                bool respuesta = new CN_Negocio().ActualizarLogo(byteImage,out mensaje);

                if (respuesta)
                    picLogo.Image = ByteToImage(byteImage);
                else
                    MessageBox.Show(mensaje,"Mensaja",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                

            }
           
               
        }
    }
}
