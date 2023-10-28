using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Categoria
    {
        private CD_Categoria objCapaDato = new CD_Categoria();

        public List<Categoria> ListaCategoria()
        {
            return objCapaDato.ListaCategoria();
        }
        public int AgregarCategoria(Categoria categoria, out string mensaje)
        {
            mensaje = string.Empty;
            if (categoria.Descripcion == "")
                mensaje = "Es necesario agregar una descripcion a la categoria";

            if (mensaje != string.Empty)
                return 0;
            else
                return objCapaDato.AgregarCategoria(categoria, out mensaje);
        }
        public bool ModificarCategoria(Categoria categoria, out string mensaje)
        {
            mensaje = string.Empty;
            if (categoria.Descripcion == "")
                mensaje = "Es necesario agregar una descripcion a la categoria";

            if (mensaje != string.Empty)
                return false;
            else
                return objCapaDato.ModificarCategoria(categoria, out mensaje);
        }
        public bool EliminarCategoria(int id, out string mensaje)
        {
            return objCapaDato.EliminarCategoria(id,out mensaje);
        }


    }
}
