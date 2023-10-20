using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using CapaEntidad;

namespace CapaDatos
{
    public class CD_Usuario
    {
        public List<Usuario> ListarUsuario()
        {
            List<Usuario> lista = new List<Usuario>();

            string query = "select IdUsuario,Documento,NombreCompleto,Correo,Clave,Estado from Usuario";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd= new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Usuario()
                {
                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                    Documento = dr["Documento"].ToString(),
                    NombreCompleto = dr["NombreCompleto"].ToString(),
                    Correo = dr["Correo"].ToString(),
                    Clave = dr["Clave"].ToString(),
                    Estado = Convert.ToBoolean(dr["Estado"])

                });
            }

            return lista;
        }
    }
}
