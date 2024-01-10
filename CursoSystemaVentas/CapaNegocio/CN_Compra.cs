using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Compra
    {
        private readonly CD_Compra objCompra = new CD_Compra();

        public int ObtenerCorrelativo()
        {
            return objCompra.ObtenerCorrelativo();
        }

        public bool RegistarCompra(Compra obj, DataTable detalleCompra, out string mensaje)
        {
            return objCompra.RegistarCompra(obj,detalleCompra,out mensaje);
        }
    }
}
 