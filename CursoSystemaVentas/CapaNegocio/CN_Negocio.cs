using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaDatos;
using CapaEntidad;

namespace CapaNegocio
{
    public class CN_Negocio
    {
        private readonly CD_Negocio objCapaDato = new CD_Negocio();

        public Negocio CargarDatos()
        {
            return objCapaDato.CargarDatos();
        }
        public bool GuardarDatos(Negocio oNegocio, out string mensaje)
        {
            mensaje = string.Empty;

            if (oNegocio.Nombre == "")
                mensaje += "El Campo Nombre no puede estar Vacio";
            if (oNegocio.RUC == "")
                mensaje += "El Campo RUC no puede estar Vacio";
            if (oNegocio.Direccion == "")
                mensaje += "El Campo Nombre no puede estar Vacio";
            if (mensaje != string.Empty)
                return false;
            else
                return objCapaDato.GuardarDatos(oNegocio,out mensaje);
        }

        public byte[] ObtenerLogo(out bool obtenido)
        {
            return objCapaDato.ObtenerLogo(out obtenido);
        }

        public bool ActualizarLogo(byte[] imagen, out string mensaje)
        {
            return objCapaDato.ActualizarLogo(imagen,out mensaje);
        }
    }
}
