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
        private readonly CD_Usuario objCapaDato = new CD_Usuario();

        public List<Usuario> ListarUsuario()
        {
            return objCapaDato.ListarUsuario();
        }
    }
}
