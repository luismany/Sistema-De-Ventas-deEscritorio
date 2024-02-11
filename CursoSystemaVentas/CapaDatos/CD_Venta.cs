using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Venta
    {
        public int ObtenerCorrelativo()
        {
            int correlativo = 0;

            string consulta = "select count(*) + 1 from Venta";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(consulta, con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            correlativo = Convert.ToInt32(cmd.ExecuteScalar());

            return correlativo;

        }

        public bool RestarStock(int idProducto, int cantidad)
        {
            bool respueta = true;

            string query = "update Producto set Stock= Stock - @cantidad where IdProducto=@idProducto";
            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query,con);
            cmd.Parameters.AddWithValue("@idProducto",idProducto);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.CommandType = CommandType.Text;
            con.Open();

            respueta = cmd.ExecuteNonQuery() > 0 ? true : false;

            return respueta;

        }

        public bool SumarStock(int idProducto, int cantidad)
        {
            bool respueta = true;

            string query = "update Producto set Stock= Stock + @cantidad where IdProducto=@idProducto";
            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@idProducto", idProducto);
            cmd.Parameters.AddWithValue("@cantidad", cantidad);
            cmd.CommandType = CommandType.Text;
            con.Open();

            respueta = cmd.ExecuteNonQuery() > 0 ? true : false;

            return respueta;

        }

        public bool RegistarVenta(Venta obj, DataTable detalleVenta, out string mensaje)
        {
            mensaje = string.Empty;
            bool Respuesta = false;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", con);
            cmd.Parameters.AddWithValue("IdUsuario", obj.oUsuario.IdUsuario);
            cmd.Parameters.AddWithValue("TipoDocumento", obj.TipoDocumento);
            cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
            cmd.Parameters.AddWithValue("DocumentoCliente", obj.DocumentoCliente);
            cmd.Parameters.AddWithValue("NombreCliente", obj.NombreCliente);
            cmd.Parameters.AddWithValue("MontoPago", obj.MontoPago);
            cmd.Parameters.AddWithValue("MontoCambio", obj.MontoCambio);
            cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
            cmd.Parameters.AddWithValue("EDetalle_Venta", detalleVenta);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Respuesta;
        }
    }
}
