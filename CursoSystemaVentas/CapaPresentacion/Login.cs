using CapaEntidad;
using CapaNegocio;
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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            Usuario oUsuario = new CN_Usuario().ListarUsuario().Where(u => u.Documento == txtNoDocumento.Text && u.Clave == txtClave.Text).FirstOrDefault();

            if (oUsuario != null)
            {
                Inicio frmInicio = new Inicio(oUsuario);

                frmInicio.Show();
                this.Hide();
                frmInicio.FormClosing += frm_Closing;
            }
            else
            {
                MessageBox.Show("Usuario o Contraseña Incorrecta","Mensaje",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            
            }
        }

        private void frm_Closing(object sender, FormClosingEventArgs e)
        {
            txtNoDocumento.Clear();
            txtClave.Clear();
            this.Show();
        }

    }
}
