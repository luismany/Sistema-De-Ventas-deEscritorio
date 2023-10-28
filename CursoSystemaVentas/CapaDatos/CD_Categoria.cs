using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Categoria
    {
        public List<Categoria> ListaCategoria()
        {
            List<Categoria> lista = new List<Categoria>();

            string query = "select IdCategoria,Descripcion,Estado from Categoria";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Categoria()
                {
                    IdCategoria=Convert.ToInt32(dr["IdCategoria"]),
                    Descripcion = dr["Descripcion"].ToString(),
                    Estado=Convert.ToBoolean(dr["Estado"])
                });
            }

            return lista;
        }

        public int AgregarCategoria(Categoria categoria, out string mensaje)
        {
            int IdGeneradoResultado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarCategoria", con);
            cmd.Parameters.AddWithValue("Descripcion", categoria.Descripcion);
            cmd.Parameters.AddWithValue("Estado", categoria.Estado);
            cmd.Parameters.Add("IdGeneradoResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            IdGeneradoResultado = Convert.ToInt32(cmd.Parameters["IdGeneradoResultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return IdGeneradoResultado;
        }

        public bool ModificarCategoria(Categoria categoria, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ModificarCategoria", con);
            cmd.Parameters.AddWithValue("IdCategoria", categoria.IdCategoria);
            cmd.Parameters.AddWithValue("Descripcion", categoria.Descripcion);
            cmd.Parameters.AddWithValue("Estado", categoria.Estado);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return resultado;
        }
        public bool EliminarCategoria(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_EliminarCategoria", con);
            cmd.Parameters.AddWithValue("IdCategoria", id);
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
