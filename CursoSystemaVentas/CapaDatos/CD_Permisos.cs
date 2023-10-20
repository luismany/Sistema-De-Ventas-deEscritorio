using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace CapaDatos
{
   public class CD_Permisos
    {
        public List<Permiso> ListaPermisos(int idUsuario)
        {
            List<Permiso> lista = new List<Permiso>();

            StringBuilder query = new StringBuilder();
            query.AppendLine("select r.IdRol,p.NombreMenu from Permiso p");
            query.AppendLine("join Rol r on r.IdRol = p.IdRol");
            query.AppendLine("join Usuario u on u.IdRol = r.IdRol");
            query.AppendLine("where IdUsuario = @IdUsuario");

            //            select r.IdRol,p.NombreMenu from Permiso p
            //join Rol r on r.IdRol = p.IdRol
            //join Usuario u on u.IdRol = r.IdRol
            //where IdUsuario = 2

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(),con);
            cmd.Parameters.AddWithValue("@IdUsuario",idUsuario);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Permiso() { 
                
                oRol=new Rol() { IdRol=Convert.ToInt32(dr["IdRol"])},
                NombreMenu=dr["NombreMenu"].ToString()
                
                });
            }

            return lista;


        }
    }
}
