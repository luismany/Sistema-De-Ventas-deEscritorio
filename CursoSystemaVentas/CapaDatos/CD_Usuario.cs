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

            StringBuilder query = new StringBuilder();
            query.AppendLine("select u.IdUsuario,u.Documento,u.NombreCompleto,u.Correo,u.Clave,r.IdRol,r.Descripcion,u.Estado from Usuario u");
            query.AppendLine("join Rol r on r.IdRol = u.IdRol");

            //select u.IdUsuario,u.Documento,u.NombreCompleto,u.Correo,u.Clave,r.IdRol,r.Descripcion,u.Estado from Usuario u
            //join Rol r on r.IdRol = u.IdRol

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(), con);
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
                    Estado = Convert.ToBoolean(dr["Estado"]),
                    oRol = new Rol() { IdRol = Convert.ToInt32(dr["IdRol"]), Descripcion = dr["Descripcion"].ToString() }

                });
            }

            return lista;
        }

        public int AgregarUsuario(Usuario usuario,out string mensaje)
        {
            mensaje = string.Empty;
            int idGenedado = 0;
            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarUsuario", con);
            cmd.Parameters.AddWithValue("Documento", usuario.Documento);
            cmd.Parameters.AddWithValue("NombreCompleto", usuario.NombreCompleto);
            cmd.Parameters.AddWithValue("Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("Clave", usuario.Clave);
            cmd.Parameters.AddWithValue("IdRol", usuario.oRol.IdRol);
            cmd.Parameters.AddWithValue("Estado", usuario.Estado);
            cmd.Parameters.Add("IdGeneradoResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            idGenedado = Convert.ToInt32(cmd.Parameters["IdGeneradoResultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return idGenedado;
        }

        public bool ModificarUsuario(Usuario usuario, out string mensaje)
        {
            mensaje = string.Empty;
            bool Resultado = false;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ModificarUsuario", con);
            cmd.Parameters.AddWithValue("IdUsuario", usuario.IdUsuario);
            cmd.Parameters.AddWithValue("Documento", usuario.Documento);
            cmd.Parameters.AddWithValue("NombreCompleto", usuario.NombreCompleto);
            cmd.Parameters.AddWithValue("Correo", usuario.Correo);
            cmd.Parameters.AddWithValue("Clave", usuario.Clave);
            cmd.Parameters.AddWithValue("IdRol", usuario.oRol.IdRol);
            cmd.Parameters.AddWithValue("Estado", usuario.Estado);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Resultado;
        }

        public bool EliminarUsuario(int id,out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_EliminarUsuario",con);
            cmd.Parameters.AddWithValue("IdUsuario",id);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return resultado;
        }


    }
}
