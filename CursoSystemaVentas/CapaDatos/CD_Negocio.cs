using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
    public class CD_Negocio
    {
        public Negocio CargarDatos()
        {

            Negocio oNegocio = new Negocio();
            string consulta = "select IdNegocio,Nombre,Ruc,Direccion from Negocio";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(consulta,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                oNegocio = new Negocio()
                {
                    IdNegocio = Convert.ToInt32(dr["IdNegocio"]),
                    Nombre = dr["Nombre"].ToString(),
                    RUC=dr["RUC"].ToString(),
                    Direccion=dr["Direccion"].ToString()
                };

            }

            return oNegocio;

        }

        public bool GuardarDatos(Negocio oNegocio, out string mensaje)
        {
            mensaje = string.Empty;
            bool respuesta = true;

            string consulta = "update Negocio set Nombre=@Nombre, RUC=@RUC, Direccion=@Direccion where IdNegocio=1";
            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(consulta,con);
            cmd.Parameters.AddWithValue("@Nombre",oNegocio.Nombre);
            cmd.Parameters.AddWithValue("@RUC", oNegocio.RUC);
            cmd.Parameters.AddWithValue("@Direccion", oNegocio.Direccion);
            cmd.CommandType = CommandType.Text;
            con.Open();

            if (cmd.ExecuteNonQuery() < 1)
            {
                mensaje = "No se pudo actualizar la informacion";
                respuesta = false;
            }

            return respuesta;
        }

    }
}
