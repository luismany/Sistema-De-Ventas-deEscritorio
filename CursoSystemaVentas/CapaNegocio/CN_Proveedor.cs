using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Proveedor
    {
        private readonly CD_Proveedor objCapaDato = new CD_Proveedor();

        public List<Proveedor> ListaProveedor()
        {
            return objCapaDato.ListaProveedor();
        }

        public int AgregarProveedor(Proveedor proveedor, out string mensaje)
        {
            mensaje = string.Empty;

            if (proveedor.Documento == "")
                mensaje = "El campo Documento es obligatorio";
            if (proveedor.RazonSocial == "")
                mensaje = "El campo Razon Social es obligatorio";
            if (proveedor.Correo == "")
                mensaje = "El campo Correo es obligatorio";
            if (proveedor.Telefono == "")
                mensaje = "El campo Telefono es obligatorio";
            if (mensaje != string.Empty)
                return 0;
            else
                return objCapaDato.AgregarProveedor(proveedor,out mensaje);
        }

        public bool ModificarProveedor(Proveedor proveedor, out string mensaje)
        {
            mensaje = string.Empty;

            if (proveedor.Documento == "")
                mensaje = "El campo Documento es obligatorio";
            if (proveedor.RazonSocial == "")
                mensaje = "El campo Razon Social es obligatorio";
            if (proveedor.Correo == "")
                mensaje = "El campo Correo es obligatorio";
            if (proveedor.Telefono == "")
                mensaje = "El campo Telefono es obligatorio";
            if (mensaje != string.Empty)
                return false;
            else
                return objCapaDato.ModificarProveedor(proveedor, out mensaje);
        }

        public bool EliminarProveedor(int id, out string mensaje)
        {
            return objCapaDato.EliminarProveedor(id,out mensaje);
        }
    }
}
