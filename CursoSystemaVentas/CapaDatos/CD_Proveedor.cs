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
    public class CD_Proveedor
    {
        public List<Proveedor> ListaProveedor()
        {
            List<Proveedor> lista = new List<Proveedor>();

            string query = "select IdProveedor,Documento,RazonSocial,Correo,Telefono,Estado from Proveedor";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                lista.Add(new Proveedor()
                {

                    IdProveedor = Convert.ToInt32(dr["IdProveedor"]),
                    Documento=dr["Documento"].ToString(),
                    RazonSocial=dr["RazonSocial"].ToString(),
                    Correo=dr["Correo"].ToString(),
                    Telefono=dr["Telefono"].ToString(),
                    Estado=Convert.ToBoolean(dr["Estado"]),

                });
            }

            return lista;
        }

        public int AgregarProveedor(Proveedor proveedor, out string mensaje)
        {
            mensaje = string.Empty;
            int idGeneradoResultado = 0;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarProveedor", con);
            cmd.Parameters.AddWithValue("Documento",proveedor.Documento);
            cmd.Parameters.AddWithValue("RazonSocial", proveedor.RazonSocial);
            cmd.Parameters.AddWithValue("Correo", proveedor.Correo);
            cmd.Parameters.AddWithValue("Telefono", proveedor.Telefono);
            cmd.Parameters.AddWithValue("Estado", proveedor.Estado);
            cmd.Parameters.Add("IdGeneradoResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            idGeneradoResultado = Convert.ToInt32(cmd.Parameters["IdGeneradoResultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return idGeneradoResultado;
        }

        public bool ModificarProveedor(Proveedor proveedor, out string mensaje)
        {
            mensaje = string.Empty;
            bool Resultado = false;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ModificarProveedor", con);
            cmd.Parameters.AddWithValue("IdProveedor", proveedor.IdProveedor);
            cmd.Parameters.AddWithValue("Documento", proveedor.Documento);
            cmd.Parameters.AddWithValue("RazonSocial", proveedor.RazonSocial);
            cmd.Parameters.AddWithValue("Correo", proveedor.Correo);
            cmd.Parameters.AddWithValue("Telefono", proveedor.Telefono);
            cmd.Parameters.AddWithValue("Estado", proveedor.Estado);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Resultado;
        }

        public bool EliminarProveedor(int id, out string mensaje)
        {
            mensaje = string.Empty;
            bool Resultado = false;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_EliminarProveedor", con);
            cmd.Parameters.AddWithValue("IdProveedor",id);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Resultado;
        }
    }
}
