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
    public class CD_Cliente
    {
        public List<Cliente> ListaCliente()
        {
            List<Cliente> lista = new List<Cliente>();

            string query = "select IdCliente,Documento,NombreCompleto,Correo,Telefono,Estado from Cliente";
            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Cliente()
                {

                    IdCliente = Convert.ToInt32(dr["IdCliente"]),
                    Documento=dr["Documento"].ToString(),
                    NombreCompleto=dr["NombreCompleto"].ToString(),
                    Correo=dr["Correo"].ToString(),
                    Telefono=dr["Telefono"].ToString(),
                    Estado=Convert.ToBoolean(dr["Estado"])

                });
            }

            return lista;
        }

        public int AgregarCliente(Cliente cliente,out string mensaje)
        {
            int idGeneradoResultado = 0;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarCliente", con);
            cmd.Parameters.AddWithValue("Documento",cliente.Documento);
            cmd.Parameters.AddWithValue("NombreCompleto", cliente.NombreCompleto);
            cmd.Parameters.AddWithValue("Correo", cliente.Correo);
            cmd.Parameters.AddWithValue("Telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("Estado", cliente.Estado);
            cmd.Parameters.Add("IdGeneradoResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            idGeneradoResultado = Convert.ToInt32(cmd.Parameters["IdGeneradoResultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return idGeneradoResultado;
        }

        public bool ModificarCliente(Cliente cliente, out string mensaje)
        {
            bool Resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ModificarCliente", con);
            cmd.Parameters.AddWithValue("IdCliente", cliente.IdCliente);
            cmd.Parameters.AddWithValue("Documento", cliente.Documento);
            cmd.Parameters.AddWithValue("NombreCompleto", cliente.NombreCompleto);
            cmd.Parameters.AddWithValue("Correo", cliente.Correo);
            cmd.Parameters.AddWithValue("Telefono", cliente.Telefono);
            cmd.Parameters.AddWithValue("Estado", cliente.Estado);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Resultado;
        }

        public bool EliminarrCliente(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            string query = "delete from Cliente where IdCliente=@IdCliente";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("IdCliente", id);
            cmd.CommandType = CommandType.Text;
            con.Open();

            //cmd.ExecuteNonQuery() devuelve la cantidad de filas afectadas
            resultado = cmd.ExecuteNonQuery() > 0 ? true : false; 
            return resultado;

        }

    }
}
