using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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


        private void frmNegocio_Load(object sender, EventArgs e)
        {
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
    }
}
