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
    public class CD_Rol
    {
       public List<Rol> ListaRol()
        {
            List<Rol> lista = new List<Rol>();

            string query = "select IdRol,Descripcion from Rol";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Rol
                {

                    IdRol = Convert.ToInt32(dr["IdRol"]),
                    Descripcion = dr["Descripcion"].ToString()

                }); ;
            }

            return lista;

        }
    }
}
