using Practica2.Models;
using Practica2.Repositories.Contracts;
using Practica2.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;

namespace Practica2.Repositories.Implementations
{
    public class AplicacionRepository : IAplicacion
    {
       

        public bool Add(Factura factura)
        {
            bool result = true;
            SqlConnection cnn = DataHelper.GetInstance().GetConnection();
            SqlTransaction t = null;
            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();

            var cmd = new SqlCommand("Crear_Factura", cnn, t);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@nro_factura", factura.NroFactura);
            cmd.Parameters.AddWithValue("@fecha", factura.Fecha);
            cmd.Parameters.AddWithValue("@id_forma_pago", factura.FormaPago.Id);
            cmd.Parameters.AddWithValue("@cliente", factura.Cliente);

            SqlParameter param = new SqlParameter("@nro_factura", SqlDbType.Int);
            param.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(param);

            cmd.ExecuteNonQuery();

            int nro_factura = (int)param.Value;

            int detalleId = 1;

            foreach (var detalle in factura.Detalles)

                {
                    var cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLES", cnn, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmdDetalle.Parameters.AddWithValue("@ID", detalleId);
                    cmdDetalle.Parameters.AddWithValue("@ARTICULO", detalle.Articulo.id);
                    cmdDetalle.Parameters.AddWithValue("@CANTIDAD", detalle.cantidad);
                    cmdDetalle.Parameters.AddWithValue("@nro_factura", nro_factura);

                    cmdDetalle.ExecuteNonQuery();

                    detalleId++;
                }

                t.Commit();
            }
            catch (SqlException)
            {
                if (t != null)
                    t.Rollback();

                result = false;
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open)
                {
                    cnn.Close();
                }
            }

        
            return result;
        }

        public bool Delete(int id)
        {
            var parameters = new List<ParameterSQL>();
            parameters.Add(new ParameterSQL("@nro_factura", id));
            int filasAfectadas = DataHelper.GetInstance().ExecuteSPDML("Eliminar_Factura", parameters);

            return filasAfectadas > 0;
        }

        public bool Edit(Factura factura)
        {
            var parameters = new List<ParameterSQL>();
            parameters.Add(new ParameterSQL("@nro_fcatura", factura.NroFactura));
            parameters.Add(new ParameterSQL("@fecha", factura.Fecha));
            parameters.Add(new ParameterSQL("@id_forma_pago", factura.FormaPago.Id));
            parameters.Add(new ParameterSQL("@Ciente", factura.Cliente));


            int filasAfectadas = DataHelper.GetInstance().ExecuteSPDML("Actualizar_Factura",parameters);

            return filasAfectadas > 0;

        }

        public List<Factura> GetAll()
        {

            var facturas = new List<Factura>();
            DataTable table = DataHelper.GetInstance().ExecuteSPQuery("Obtener_facturas", null);

            if (table != null)
            {
                foreach (DataRow row in table.Rows)
                {
                    var factura = new Factura
                    {
                        NroFactura = Convert.ToInt32(row["nro_factura"]),
                        Fecha = Convert.ToDateTime(row["fecha"]),
                        FormaPago = new FormaDePago
                        {
                            Id = Convert.ToInt32(row["id_forma_pago"]),
                            Nombre = row["nombre"].ToString(),
                        },
                        Cliente = row["cliente"].ToString()


                    };
                    facturas.Add(factura);
                }
            }
            return facturas;
        }
    }
}
