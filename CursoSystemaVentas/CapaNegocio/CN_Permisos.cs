using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Permisos
    {
        private readonly CD_Permisos objCapaDato = new CD_Permisos();

        public List<Permiso> ListaPermisos(int idUsuario)
        {
            return objCapaDato.ListaPermisos(idUsuario);
        }
        }
}
