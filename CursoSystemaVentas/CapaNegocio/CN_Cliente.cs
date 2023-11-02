using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public class CN_Cliente
    {
        private readonly CD_Cliente objCapaDato = new CD_Cliente();

        public List<Cliente> ListaCliente()
        {
            return objCapaDato.ListaCliente();
        }

        public int AgregarCliente(Cliente cliente, out string mensaje)
        {
            mensaje = string.Empty;

            if (cliente.Documento == "")
                mensaje = "El campo Documento no puede estar vacio";
            if (cliente.NombreCompleto == "")
                mensaje = "El campo Nombre Completo no puede estar vacio";
            if (cliente.Correo == "")
                mensaje = "El campo Correo no puede estar vacio";
            if (cliente.Telefono == "")
                mensaje = "El campo Telefono no puede estar vacio";
            if (mensaje == string.Empty)
                return objCapaDato.AgregarCliente(cliente, out mensaje);
            else
                return 0;
        }

        public bool ModificarCliente(Cliente cliente, out string mensaje)
        {
            mensaje = string.Empty;

            if (cliente.Documento == "")
                mensaje = "El campo Documento no puede estar vacio";
            if (cliente.NombreCompleto == "")
                mensaje = "El campo Nombre Completo no puede estar vacio";
            if (cliente.Correo == "")
                mensaje = "El campo Correo no puede estar vacio";
            if (cliente.Telefono == "")
                mensaje = "El campo Telefono no puede estar vacio";
            if (mensaje == string.Empty)
                return objCapaDato.ModificarCliente(cliente, out mensaje);
            else
                return false;
        }

        public bool EliminarrCliente(int id, out string mensaje)
        {
            return objCapaDato.EliminarrCliente(id,out mensaje);
        }
    }
}
