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
    public class CD_Reportes
    {
        public List<ReporteCompra> ReporteCompra(string fechaInicio, string fechaFin, int idProveedor)
        {
            List<ReporteCompra> listaRC = new List<ReporteCompra>();

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ReporteCompra",con);
            cmd.Parameters.AddWithValue("@FechaInicio",fechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin",fechaFin);
            cmd.Parameters.AddWithValue("@IdProveedor",idProveedor);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listaRC.Add(new ReporteCompra()
                {

                    FechaRegistro = dr["FechaRegistro"].ToString(),
                    TipoDocumento = dr["TipoDocumento"].ToString(),
                    NumeroDocumento = dr["NumeroDocumento"].ToString(),
                    MontoTotal=dr["MontoTotal"].ToString(),
                    UsuarioRegistro=dr["UsuarioRegistro"].ToString(),
                    DocumentoProveedor=dr["DocumentoProveedor"].ToString(),
                    RazonSocial=dr["RazonSocial"].ToString(),
                    CodigoProducto=dr["CodigoProducto"].ToString(),
                    NombreProducto=dr["NombreProducto"].ToString(),
                    categoria =dr["categoria"].ToString(),
                    PrecioCompra=dr["PrecioCompra"].ToString(),
                    PrecioVenta=dr["PrecioVenta"].ToString(),
                    Cantidad=dr["Cantidad"].ToString(),
                    SubTotal=dr["SubTotal"].ToString(),
                });
            }

            return listaRC;

        }

        public List<ReporteVenta> ReporteVenta(string fechaInicio, string fechaFin)
        {
            List<ReporteVenta> listaRV = new List<ReporteVenta>();

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ReporteVenta", con);
            cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);
            cmd.Parameters.AddWithValue("@FechaFin", fechaFin);
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                listaRV.Add(new ReporteVenta()
                {

                    FechaRegistro = dr["FechaRegistro"].ToString(),
                    TipoDocumento = dr["TipoDocumento"].ToString(),
                    NumeroDocumento = dr["NumeroDocumento"].ToString(),
                    MontoTotal = dr["MontoTotal"].ToString(),
                    UsuarioRegistro = dr["UsuarioRegistro"].ToString(),
                    DocumentoCliente = dr["DocumentoCliente"].ToString(),
                    NombreCliente = dr["NombreCliente"].ToString(),
                    CodigoProducto = dr["CodigoProducto"].ToString(),
                    NombreProducto = dr["NombreProducto"].ToString(),
                    categoria = dr["categoria"].ToString(),
                    PrecioVenta = dr["PrecioVenta"].ToString(),
                    Cantidad = dr["Cantidad"].ToString(),
                    SubTotal = dr["SubTotal"].ToString(),
                });
            }

            return listaRV;

        }
    }
}
