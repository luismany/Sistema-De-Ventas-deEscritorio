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
    public class CD_Producto
    {
        public List<Producto> ListarProducto()
        {
            List<Producto> lista = new List<Producto>();

            StringBuilder query = new StringBuilder();
            query.AppendLine("select p.IdProducto,p.Codigo,p.Nombre,p.Descripcion,c.IdCategoria,c.Descripcion[DescripcionCategoria],");
            query.AppendLine("p.Stock,p.PrecioCompra,p.PrecioVenta,p.Estado from Producto p");
            query.AppendLine("join Categoria c on p.IdCategoria = c.IdCategoria");

            //select p.IdProducto, p.Codigo,p.Nombre,p.Descripcion,c.IdCategoria,c.Descripcion[DescripcionCategoria],
            //p.Stock,p.PrecioCompra,p.PrecioVenta,p.Estado from Producto p
            //join Categoria c on p.IdCategoria = c.IdCategoria

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand(query.ToString(), con);
            cmd.CommandType = CommandType.Text;
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                lista.Add(new Producto()
                {
                    IdProducto = Convert.ToInt32(dr["IdProducto"]),
                    Codigo = dr["Codigo"].ToString(),
                    Nombre = dr["Nombre"].ToString(),
                    Descripcion = dr["Descripcion"].ToString(),
                    oCategoria = new Categoria {IdCategoria=Convert.ToInt32(dr["IdCategoria"]),Descripcion=dr["DescripcionCategoria"].ToString() } ,
                    Stock = Convert.ToInt32(dr["Stock"]),
                    PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                    PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                    Estado=Convert.ToBoolean(dr["Estado"]),
                });
            }

            return lista;
        }

        public int AgregarProducto(Producto producto, out string mensaje)
        {
            mensaje = string.Empty;
            int idGenedado = 0;
            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_AgregarProducto", con);
            cmd.Parameters.AddWithValue("Codigo", producto.Codigo);
            cmd.Parameters.AddWithValue("Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("Descripcion", producto.Descripcion);
            cmd.Parameters.AddWithValue("IdCategoria", producto.oCategoria.IdCategoria);
            cmd.Parameters.AddWithValue("Estado",producto.Estado);
            cmd.Parameters.Add("IdGeneradoResultado", SqlDbType.Int).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            idGenedado = Convert.ToInt32(cmd.Parameters["IdGeneradoResultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return idGenedado;
        }

        public bool ModificarProducto(Producto producto, out string mensaje)
        {
            mensaje = string.Empty;
            bool Resultado = false;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_ModificarProducto", con);
            cmd.Parameters.AddWithValue("IdProducto", producto.IdProducto);
            cmd.Parameters.AddWithValue("Codigo", producto.Codigo);
            cmd.Parameters.AddWithValue("Nombre", producto.Nombre);
            cmd.Parameters.AddWithValue("Descripcion", producto.Descripcion);
            cmd.Parameters.AddWithValue("IdCategoria", producto.oCategoria.IdCategoria);
            cmd.Parameters.AddWithValue("Estado", producto.Estado);
            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
            cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
            cmd.CommandType = CommandType.StoredProcedure;
            con.Open();

            cmd.ExecuteNonQuery();

            Resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
            mensaje = cmd.Parameters["Mensaje"].Value.ToString();

            return Resultado;
        }

        public bool EliminarProducto(int id, out string mensaje)
        {
            bool resultado = false;
            mensaje = string.Empty;

            SqlConnection con = new SqlConnection(Conexion.Cadena);
            SqlCommand cmd = new SqlCommand("sp_EliminarProducto", con);
            cmd.Parameters.AddWithValue("IdProducto", id);
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
