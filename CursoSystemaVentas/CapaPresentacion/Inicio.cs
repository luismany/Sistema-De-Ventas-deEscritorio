using CapaEntidad;
using CapaNegocio;
using FontAwesome.Sharp;
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
    public partial class Inicio : Form
    {
        private static Usuario usuarioActual;
        private static IconMenuItem menuActivo = null;
        private static Form formularioActivo = null;
        public Inicio(Usuario usuario)
        {
            usuarioActual = usuario;

            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = usuarioActual.NombreCompleto;

            //para los permisos. mustra los menus a los que el usuario puede acceder segun su rol.
            List<Permiso> listaPermiso = new CN_Permisos().ListaPermisos(usuarioActual.IdUsuario);

            foreach (IconMenuItem iconmenu in menu.Items)
            {
                bool encontrado = listaPermiso.Any(m => m.NombreMenu== iconmenu.Name);

                if (encontrado == false) iconmenu.Visible = false;
            }

        }

        private void AbrirFormulario(IconMenuItem menu, Form formulario)
        {
            if (menuActivo != null)
            {
                menuActivo.BackColor = Color.White;
            }
            menu.BackColor = Color.Silver;
            menuActivo = menu;

            if (formularioActivo != null)
            {

                formularioActivo.Close();
            }
            formularioActivo = formulario;
            formulario.TopLevel = false; // Esto lo convierte en un formulario secundario
            formulario.FormBorderStyle = FormBorderStyle.None; // Quita los bordes del formulario
            formulario.Dock = DockStyle.Fill; // Rellena completamente el panel
            formulario.BackColor = Color.SteelBlue;

            submenuregistrarventa.Controls.Add(formulario); // Agrega el formulario al panel
            formulario.Show();  // Muestra el formulario secundario

        }
        private void menuusuario_Click_1(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmUsuario());
        }

        private void submenucategoria_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmCategoria());
        }
        private void submenuproducto_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmProducto());
        }
        private void iconMenuItem1_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuventas, new frmVentas());
        }

        private void submenuverdetalleventa_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menuventas, new frmDetalleVenta());
        }

        private void submenuregistrarcompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menucompras, new frmCompras());
        }

        private void submenuverdetallecompra_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menucompras, new frmDetalleCompra());
        }

        private void menuclientes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmClientes());
        }

        private void menuproveedores_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmProveedores());
        }

        private void menureportes_Click(object sender, EventArgs e)
        {
            AbrirFormulario((IconMenuItem)sender, new frmReportes());
        }

        private void negocioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AbrirFormulario(menumantenedor, new frmNegocio());
        }
    }

}