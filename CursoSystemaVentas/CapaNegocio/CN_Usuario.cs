using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Usuario
    {
        private CD_Usuario objCapaDato = new CD_Usuario();

        public List<Usuario> ListarUsuario()
        {
            return objCapaDato.ListarUsuario();
        }
        public int AgregarUsuario(Usuario usuario,out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(usuario.Documento) || string.IsNullOrWhiteSpace(usuario.Documento))
                mensaje = "El Campo Documento no puede estar Vacio";
            if (string.IsNullOrEmpty(usuario.NombreCompleto))
                mensaje = "El Campo Documento no puede estar Vacio";
            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Correo))
                mensaje = "El Campo Documento no puede estar Vacio";
            if (mensaje == string.Empty)
                return objCapaDato.AgregarUsuario(usuario, out mensaje);
            else
                return 0;
        }

        public bool ModificarUsuario(Usuario usuario, out string mensaje)
        {
            mensaje = string.Empty;

            if (string.IsNullOrEmpty(usuario.Documento) || string.IsNullOrWhiteSpace(usuario.Documento))
                mensaje = "El Campo Documento no puede estar Vacio";
            if (string.IsNullOrEmpty(usuario.NombreCompleto))
                mensaje = "El Campo Documento no puede estar Vacio";
            if (string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrWhiteSpace(usuario.Correo))
                mensaje = "El Campo Documento no puede estar Vacio";
            if (mensaje == string.Empty)
                return objCapaDato.ModificarUsuario(usuario, out mensaje);
            else
                return false;
        }

        public bool EliminarUsuario(int id, out string mensaje)
        {
            return objCapaDato.EliminarUsuario(id,out mensaje);
        }
    }
}
