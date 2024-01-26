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
    public class CD_Compra
    {
        public int ObtenerCorrelativo()
        {
            int correlativo = 0;

            string consulta = "select count(*) + 1 from Compra";

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(consulta,con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            correlativo = Convert.ToInt32(cmd.ExecuteScalar());

            return correlativo;

        }

        public bool RegistarCompra(Compra obj, DataTable detalleCompra, out string mensaje )
        {
            mensaje = string.Empty;
            bool Respuesta = false;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_RegistrarCompra",con);
            cmd.Parameters.AddWithValue("IdUsuario",obj.oUsuario.IdUsuario);
            cmd.Parameters.AddWithValue("IdProveedor",obj.oProveedor.IdProveedor);
            cmd.Parameters.AddWithValue("TipoDocumento",obj.TipoDocumento);
            cmd.Parameters.AddWithValue("NumeroDocumento", obj.NumeroDocumento);
            cmd.Parameters.AddWithValue("MontoTotal", obj.MontoTotal);
            cmd.Parameters.AddWithValue("EDetalle_Compra", detalleCompra);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Respuesta;
        }

        public Compra ObtenerCompra(string numeroDocumento)
        {
            Compra oCompra = new Compra();

            StringBuilder query = new StringBuilder();
            query.AppendLine("select c.IdCompra,");
            query.AppendLine("u.NombreCompleto,");
            query.AppendLine("p.Documento,p.RazonSocial,");
            query.AppendLine("c.TipoDocumento,c.NumeroDocumento,c.MontoTotal,CONVERT(char(10), c.FechaCreacion, 103)[FechaRegistro]");
            query.AppendLine("from Compra c");
            query.AppendLine("join Proveedor p on p.IdProveedor = c.IdProveedor");
            query.AppendLine("join Usuario u on u.IdUsuario = c.IdUsuario");
            query.AppendLine("where c.NumeroDocumento = @numeroDocumento");

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(),con);
            cmd.Parameters.AddWithValue("@numeroDocumento",numeroDocumento);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                oCompra = new Compra()
                {
                    IdCompra = Convert.ToInt32(dr["IdCompra"]),
                    oUsuario=new Usuario() { NombreCompleto=dr["NombreCompleto"].ToString() },
                    oProveedor=new Proveedor() { Documento=dr["Documento"].ToString(),RazonSocial=dr["RazonSocial"].ToString() },
                    TipoDocumento=dr["TipoDocumento"].ToString(),
                    NumeroDocumento=dr["NumeroDocumento"].ToString(),
                    MontoTotal=Convert.ToDecimal(dr["MontoTotal"]),
                    FechaCreacion=dr["FechaRegistro"].ToString()
                };
            }

            return oCompra;

        }

        public List<DetalleCompra> ObtenerDetalleCompra(int idCompra)
        {
            List<DetalleCompra> listaDC = new List<DetalleCompra>();

            StringBuilder query = new StringBuilder();
            query.AppendLine("select p.Nombre,");
            query.AppendLine("dc.PrecioCompra,dc.Cantidad,dc.Total");
            query.AppendLine("from DetalleCompra dc");
            query.AppendLine("join Compra c on c.IdCompra=dc.IdCompra");
            query.AppendLine("join Producto p on p.IdProducto=dc.IdProducto");
            query.AppendLine("where dc.IdCompra=@idCompra");

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(), con);
            cmd.Parameters.AddWithValue("@idCompra", idCompra);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listaDC.Add(new DetalleCompra()
                {

                    oProducto = new Producto() { Nombre = dr["Nombre"].ToString() },
                    PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                    Cantidad = Convert.ToInt32(dr["Cantidad"]),
                    Total = Convert.ToDecimal(dr["Total"])

                });
            }

            return listaDC;
        }
    }
}
