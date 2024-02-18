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

        public Venta ObtenerVenta(string numeroDocumento)
        {
            Venta oVenta = new Venta();

            StringBuilder query = new StringBuilder();
            query.AppendLine("select v.IdVenta,");
            query.AppendLine("u.NombreCompleto,");
            query.AppendLine("v.DocumentoCliente, v.NombreCliente,");
            query.AppendLine("v.TipoDocumento, v.NumeroDocumento,v.MontoPago,v.MontoCambio,v.MontoTotal,CONVERT(char(10), v.FechaCreacion, 103)[FechaRegistro]");
            query.AppendLine("from Venta v");
            query.AppendLine("join Usuario u on u.IdUsuario = v.IdUsuario");
            query.AppendLine("join Cliente c on c.Documento = v.DocumentoCliente");
            query.AppendLine("where v.NumeroDocumento = @numeroDocumento");

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(),con);
            cmd.Parameters.AddWithValue("@numeroDocumento",numeroDocumento);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                oVenta = new Venta()
                {
                    IdVenta = Convert.ToInt32(dr["IdVenta"]),
                    oUsuario = new Usuario(){ NombreCompleto = dr["NombreCompleto"].ToString() },
                    DocumentoCliente = dr["DocumentoCliente"].ToString(),
                    NombreCliente = dr["NombreCliente"].ToString(),
                    TipoDocumento=dr["TipoDocumento"].ToString(),
                    NumeroDocumento=dr["NumeroDocumento"].ToString(),
                    MontoPago=Convert.ToDecimal(dr["MontoPago"]),
                    MontoCambio=Convert.ToDecimal(dr["MontoCambio"]),
                    MontoTotal=Convert.ToDecimal(dr["MontoTotal"]),
                    FechaCreacion=dr["FechaRegistro"].ToString()
                };
            }

            return oVenta;
        }

        public List<DetalleVenta> ObtenerDetalleVenta(int idVenta)
        {
            List<DetalleVenta> listaDV = new List<DetalleVenta>();

            StringBuilder query = new StringBuilder();
            query.AppendLine("select p.Nombre,");
            query.AppendLine("dv.PrecioVenta,dv.Cantidad,dv.SubTotal");
            query.AppendLine("from DetalleVenta dv");
            query.AppendLine("join Venta v on v.IdVenta = dv.IdVenta");
            query.AppendLine("join Producto p on p.IdProducto = dv.IdProducto");
            query.AppendLine("where dv.IdVenta = @idVenta");

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(), con);
            cmd.Parameters.AddWithValue("@idVenta", idVenta);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listaDV.Add(new DetalleVenta()
                {

                    oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                    PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                    SubTotal = Convert.ToDecimal(dr["SubTotal"])

                });
            }

            return listaDV;

        }
    }
}
