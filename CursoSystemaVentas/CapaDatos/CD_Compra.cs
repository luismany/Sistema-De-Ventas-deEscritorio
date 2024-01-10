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
    }
}
