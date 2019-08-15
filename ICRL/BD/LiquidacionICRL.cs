using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ICRL.BD
{
    public class LiquidacionICRL
    {
        public class TipoLiquidacionGrilla
        {
            public int id_flujo;
            public string orden;
            public string onbase;
            public string cliente;
            public string proveedor;
            public DateTime fecha;
            public string placa;
            public double total_orden;
            public double pagado_usd;
            public double pagado_bs;
            public double saldo;
            public short estado;
            public TipoLiquidacionGrilla()
            {
                this.id_flujo = 0;
                this.orden = "";
                this.onbase = "";
                this.cliente = "";
                this.proveedor = "";
                this.fecha = new DateTime(2000, 1, 1);
                this.placa = "";
                this.total_orden = 0.0;
                this.pagado_usd = 0.0;
                this.pagado_bs = 0.0;
                this.saldo = 0.0;
                this.estado = 0;
            }
        }
        public class TipoRespuestaGrilla
        {
            public bool correcto;
            public string mensaje;
            public List<TipoLiquidacionGrilla> datos = new List<TipoLiquidacionGrilla>();
            public System.Data.DataSet dsLiquidacionGrilla = new System.Data.DataSet();
        }
        public static TipoRespuestaGrilla LiquidacionGrilla(int Flujo, short Estado)
        {
            TipoRespuestaGrilla objRespuesta = new TipoRespuestaGrilla();
            string strComando = "SELECT cotizacion_danios_propios_sumatoria.numero_orden, Flujo.flujoOnBase, Flujo.nombreAsegurado, cotizacion_danios_propios_sumatoria.proveedor, Flujo.placaVehiculo, cotizacion_danios_propios_sumatoria.monto_final, cotizacion_danios_propios_sumatoria.id_estado " +
              "FROM   Flujo INNER JOIN Cotizacion ON Flujo.idFlujo = Cotizacion.idFltdpujo INNER JOIN cotizacion_danios_propios_sumatoria ON Cotizacion.idFlujo = cotizacion_danios_propios_sumatoria.id_flujo AND Cotizacion.idCotizacion = cotizacion_danios_propios_sumatoria.id_cotizacion  " +
              "WHERE (cotizacion_danios_propios_sumatoria.id_flujo = @id_flujo) AND (cotizacion_danios_propios_sumatoria.id_estado = @id_estado) ";
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoLiquidacionGrilla tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = Estado;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                objRespuesta.correcto = true;
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoLiquidacionGrilla();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.estado = Estado;
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["flujoOnBase"] != DBNull.Value) tdpFila.onbase = Convert.ToString(sqlDatos["flujoOnBase"]);
                    if (sqlDatos["nombreAsegurado"] != DBNull.Value) tdpFila.cliente = Convert.ToString(sqlDatos["nombreAsegurado"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["placaVehiculo"] != DBNull.Value) tdpFila.placa = Convert.ToString(sqlDatos["placaVehiculo"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.total_orden = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    objRespuesta.datos.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = Estado;
                sqlAdaptador.Fill(objRespuesta.dsLiquidacionGrilla);
                sqlAdaptador.Dispose();
            }
            catch (Exception ex)
            {
                objRespuesta.correcto = false;
                objRespuesta.mensaje = "No se puo traer el detalle de las liquidaciones debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        //LIQUIDACION001
        public class TipoLiquidacion001
        {
            public int id_flujo;
            public int id_cotizacion;
            public short tipo_origen;
            public long id_item;
            public short id_tipo_item;
            public string numero_orden;
            public string proveedor;
            public string item_descripcion;
            public double preciobs;
            public double precious;
            public DateTime fecha_recepcion;
            public bool inspeccion;
            public bool liquidacion;
            public string num_factura;
            public short id_estado;
            public TipoLiquidacion001()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.tipo_origen = 0;
                this.id_item = 0;
                this.id_tipo_item = 0;
                this.numero_orden = "";
                this.proveedor = "";
                this.item_descripcion = "";
                this.preciobs = 0.0;
                this.precious = 0.0;
                this.fecha_recepcion = new DateTime(2000, 1, 1);
                this.inspeccion = false;
                this.liquidacion = false;
                this.num_factura = "";
                this.id_estado = 0;
            }
        }
        public static bool RegistrarLiquidaciones001(TipoLiquidacion001 Liquidacion)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "INSERT INTO [dbo].[liquidacion001]([id_flujo],[id_cotizacion],[tipo_origen],[id_item],[id_tipo_item],[numero_orden],[proveedor],[item_descripcion],[preciobs],[precious],[fecha_recepcion],[inspeccion],[liquidacion],[num_factura],[id_estado]) " +
              "VALUES(@id_flujo,@id_cotizacion,@tipo_origen,@id_item,@id_tipo_item,@numero_orden,@proveedor,@item_descripcion,@preciobs,@precious,@fecha_recepcion,@inspeccion,@liquidacion,@num_factura,@id_estado)";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Liquidacion.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Liquidacion.id_cotizacion;
                sqlComando.Parameters.Add("@tipo_origen", System.Data.SqlDbType.SmallInt).Value = Liquidacion.tipo_origen;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Liquidacion.id_item;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_tipo_item;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.numero_orden;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = Liquidacion.proveedor;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.item_descripcion;
                sqlComando.Parameters.Add("@preciobs", System.Data.SqlDbType.Float).Value = Liquidacion.preciobs;
                sqlComando.Parameters.Add("@precious", System.Data.SqlDbType.Float).Value = Liquidacion.precious;
                sqlComando.Parameters.Add("@fecha_recepcion", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_recepcion;
                sqlComando.Parameters.Add("@inspeccion", System.Data.SqlDbType.Bit).Value = Liquidacion.inspeccion;
                sqlComando.Parameters.Add("@liquidacion", System.Data.SqlDbType.Bit).Value = Liquidacion.liquidacion;
                sqlComando.Parameters.Add("@num_factura", System.Data.SqlDbType.VarChar, 20).Value = Liquidacion.num_factura;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_estado;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
                blnRespuesta = true;
            }
            catch (Exception)
            {
                blnRespuesta = false;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
        public class TipoTraerLiquidacion001
        {
            public bool correcto;
            public string mensaje;
            public List<TipoLiquidacion001> Liquidaciones001 = new List<TipoLiquidacion001>();
            public System.Data.DataSet dsLiquidacion001 = new System.Data.DataSet();
        }
        public static TipoTraerLiquidacion001 TraerLiquidacion001(int Flujo)
        {
            TipoTraerLiquidacion001 objRespuesta = new TipoTraerLiquidacion001();
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[tipo_origen],[id_item],[id_tipo_item],[numero_orden],[proveedor],[item_descripcion],[preciobs],[precious],[fecha_recepcion],[inspeccion],[liquidacion],[num_factura],[id_estado] FROM [dbo].[liquidacion001] WHERE id_flujo=@id_flujo";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoLiquidacion001 tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoLiquidacion001();
                    tdpFila.id_flujo = Flujo;
                    if (sqlDatos["id_cotizacion"] != DBNull.Value) tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    if (sqlDatos["tipo_origen"] != DBNull.Value) tdpFila.tipo_origen = Convert.ToInt16(sqlDatos["tipo_origen"]);
                    if (sqlDatos["id_item"] != DBNull.Value) tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["preciobs"] != DBNull.Value) tdpFila.preciobs = Convert.ToDouble(sqlDatos["preciobs"]);
                    if (sqlDatos["precious"] != DBNull.Value) tdpFila.preciobs = Convert.ToDouble(sqlDatos["precious"]);
                    if (sqlDatos["fecha_recepcion"] != DBNull.Value) tdpFila.fecha_recepcion = Convert.ToDateTime(sqlDatos["fecha_recepcion"]);
                    if (sqlDatos["inspeccion"] != DBNull.Value) tdpFila.inspeccion = Convert.ToBoolean(sqlDatos["inspeccion"]);
                    if (sqlDatos["liquidacion"] != DBNull.Value) tdpFila.liquidacion = Convert.ToBoolean(sqlDatos["liquidacion"]);
                    if (sqlDatos["num_factura"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["num_factura"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    objRespuesta.Liquidaciones001.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.Fill(objRespuesta.dsLiquidacion001);
            }
            catch (Exception ex)
            {
                objRespuesta.correcto = false;
                objRespuesta.mensaje = "No se pudieron traer los datos de la liquidacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool ModificarLiquidacion001(TipoLiquidacion001 Liquidacion)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "UPDATE [dbo].[liquidacion001] SET [numero_orden] = @numero_orden,[proveedor] = @proveedor,[item_descripcion] = @item_descripcion,[preciobs] = @preciobs,[precious] = @precious,[fecha_recepcion] = @fecha_recepcion,[inspeccion] = @inspeccion,[liquidacion] = @liquidacion,[num_factura] = @num_factura,[id_estado] = @id_estado " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [tipo_origen] = @tipo_origen AND [id_item] = @id_item AND [id_tipo_item] = @id_tipo_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Liquidacion.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Liquidacion.id_cotizacion;
                sqlComando.Parameters.Add("@tipo_origen", System.Data.SqlDbType.SmallInt).Value = Liquidacion.tipo_origen;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Liquidacion.id_item;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_tipo_item;

                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.numero_orden;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = Liquidacion.proveedor;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.item_descripcion;
                sqlComando.Parameters.Add("@preciobs", System.Data.SqlDbType.Float).Value = Liquidacion.preciobs;
                sqlComando.Parameters.Add("@precious", System.Data.SqlDbType.Float).Value = Liquidacion.precious;
                sqlComando.Parameters.Add("@fecha_recepcion", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_recepcion;
                sqlComando.Parameters.Add("@inspeccion", System.Data.SqlDbType.Bit).Value = Liquidacion.inspeccion;
                sqlComando.Parameters.Add("@liquidacion", System.Data.SqlDbType.Bit).Value = Liquidacion.liquidacion;
                sqlComando.Parameters.Add("@num_factura", System.Data.SqlDbType.VarChar, 20).Value = Liquidacion.num_factura;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_estado;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
                sqlComando.Dispose();
                blnRespuesta = true;
            }
            catch (Exception)
            {
                blnRespuesta = false;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
        //LIQUIDACION FACTURA
        public class TipoLiquidacion001Factura
        {
            public int id_flujo;
            public long id_factura;
            public long numero_factura;
            public DateTime fecha_emision;
            public DateTime fecha_entrega;
            public short id_moneda;
            public double monto;
            public TipoLiquidacion001Factura()
            {
                this.id_flujo = 0;
                this.id_factura = 0;
                this.numero_factura = 0;
                this.fecha_emision = new DateTime(2000, 1, 1);
                this.fecha_entrega = new DateTime(2000, 1, 1);
                this.id_moneda = 0;
                this.monto = 0.0;
            }
        }
        public static bool RegistrarLiquidacion001Factura(TipoLiquidacion001Factura Factura)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "INSERT INTO [dbo].[liquidacion001_factura] ([id_flujo],[numero_factura],[fecha_emision],[fecha_entrega],[id_moneda],[monto]) VALUES (@id_flujo,@numero_factura,@fecha_emision,@fecha_entrega,@id_moneda,@monto)";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Factura.id_flujo;
                sqlComando.Parameters.Add("@numero_factura", System.Data.SqlDbType.BigInt).Value = Factura.numero_factura;
                sqlComando.Parameters.Add("@fecha_emision", System.Data.SqlDbType.DateTime).Value = Factura.fecha_emision;
                sqlComando.Parameters.Add("@fecha_entrega", System.Data.SqlDbType.DateTime).Value = Factura.fecha_entrega;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = Factura.id_moneda;
                sqlComando.Parameters.Add("@id_fmontolujo", System.Data.SqlDbType.Int).Value = Factura.monto;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
                sqlComando.Dispose();
                blnRespuesta = true;
            }
            catch (Exception)
            {
                blnRespuesta = false;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
        public class TipoTraerLiquidacion001Factura
        {
            public bool correcto;
            public string mensaje;
            public List<TipoLiquidacion001Factura> Facturas = new List<TipoLiquidacion001Factura>();
            public System.Data.DataSet dsFacturas = new System.Data.DataSet();
        }
        public static TipoTraerLiquidacion001Factura TraerLiquidacion001Factura(int Flujo)
        {
            TipoTraerLiquidacion001Factura objRespuesta = new TipoTraerLiquidacion001Factura();
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_factura],[numero_factura],[fecha_emision],[fecha_entrega],[id_moneda],[monto] FROM [dbo].[liquidacion001_factura] WHERE [id_flujo] = @id_flujo";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            TipoLiquidacion001Factura tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoLiquidacion001Factura();
                    tdpFila.id_flujo = Flujo;
                    if (sqlDatos["id_factura"] != DBNull.Value) tdpFila.id_factura = Convert.ToInt64(sqlDatos["id_factura"]);
                    if (sqlDatos["numero_factura"] != DBNull.Value) tdpFila.numero_factura = Convert.ToInt64(sqlDatos["numero_factura"]);
                    if (sqlDatos["fecha_emision"] != DBNull.Value) tdpFila.fecha_emision = Convert.ToDateTime(sqlDatos["fecha_emision"]);
                    if (sqlDatos["fecha_entrega"] != DBNull.Value) tdpFila.fecha_entrega = Convert.ToDateTime(sqlDatos["fecha_entrega"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToInt16(sqlDatos["id_moneda"]);
                    if (sqlDatos["monto"] != DBNull.Value) tdpFila.monto = Convert.ToDouble(sqlDatos["monto"]);
                    objRespuesta.Facturas.Add(tdpFila);
                }
                sqlDatos.Close(); //se mueve a esta posición para levantar el error de Datareader abierto
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.Fill(objRespuesta.dsFacturas);
                sqlComando.Dispose();
                objRespuesta.correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.correcto = false;
                objRespuesta.mensaje = "No se pudo traer el detalle de las facturas debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool ActualizarLiquidacion001Factura(TipoLiquidacion001Factura Factura)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "UPDATE [dbo].[liquidacion001_factura] SET [numero_factura] = @numero_factura,[fecha_emision] = @fecha_emision,[fecha_entrega] = @fecha_entrega,[id_moneda] = @id_moneda,[monto] = @monto WHERE [id_flujo] = @id_flujo AND [id_factura] = @id_factura";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Factura.id_flujo;
                sqlComando.Parameters.Add("@id_factura", System.Data.SqlDbType.BigInt).Value = Factura.id_factura;

                sqlComando.Parameters.Add("@numero_factura", System.Data.SqlDbType.BigInt).Value = Factura.numero_factura;
                sqlComando.Parameters.Add("@fecha_emision", System.Data.SqlDbType.DateTime).Value = Factura.fecha_emision;
                sqlComando.Parameters.Add("@fecha_entrega", System.Data.SqlDbType.DateTime).Value = Factura.fecha_entrega;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = Factura.id_moneda;
                sqlComando.Parameters.Add("@id_monto", System.Data.SqlDbType.Int).Value = Factura.monto;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
                sqlComando.Dispose();
                blnRespuesta = true;
            }
            catch (Exception)
            {
                blnRespuesta = false;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
        public static bool BorrarLiquidacion001Factura(int Flujo, long IdFactura)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
            string strComando = "DELETE FROM [dbo].[liquidacion001_factura] WHERE id_flujo = @id_flujo AND id_factura = @id_factura";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_factura", System.Data.SqlDbType.BigInt).Value = IdFactura;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
                sqlComando.Dispose();
                blnRespuesta = true;
            }
            catch (Exception)
            {
                blnRespuesta = false;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
    }
}