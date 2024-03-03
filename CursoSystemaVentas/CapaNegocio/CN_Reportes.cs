using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Reportes
    {
        private readonly CD_Reportes objCapaDatos = new CD_Reportes();


        public List<ReporteCompra> ReporteCompra(string fechaInicio, string fechaFin, int idProveedor)
        {
            return objCapaDatos.ReporteCompra(fechaInicio,fechaFin,idProveedor);
        }
        public List<ReporteVenta> ReporteVenta(string fechaInicio, string fechaFin)
        {
            return objCapaDatos.ReporteVenta(fechaInicio, fechaFin);
        }
    }
}
