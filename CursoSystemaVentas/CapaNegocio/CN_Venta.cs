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
    public class CN_Venta
    {
        private readonly CD_Venta objVenta = new CD_Venta();

        public int ObtenerCorrelativo()
        {
            return objVenta.ObtenerCorrelativo();
        }
        public bool RestarStock(int idProducto, int cantidad)
        {
            return objVenta.RestarStock(idProducto,cantidad);
        }
        public bool SumarStock(int idProducto, int cantidad)
        {
            return objVenta.SumarStock(idProducto,cantidad);
        }
        public bool RegistarVenta(Venta obj, DataTable detalleVenta, out string mensaje)
        {
            return objVenta.RegistarVenta(obj,detalleVenta, out mensaje);
        }
    }
}
