using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Producto
    {
        private readonly CD_Producto capaDatoProducto = new CD_Producto();

        public List<Producto> ListarProducto()
        {
            return capaDatoProducto.ListarProducto();
        }

        public int AgregarProducto(Producto producto, out string mensaje)
        {
            return capaDatoProducto.AgregarProducto(producto,out mensaje);
        }

        public bool ModificarProducto(Producto producto, out string mensaje)
        {
            return capaDatoProducto.ModificarProducto(producto,out mensaje);
        }

        public bool EliminarProducto(int id, out string mensaje)
        {
            return capaDatoProducto.EliminarProducto(id,out mensaje);
        }
    }
}
