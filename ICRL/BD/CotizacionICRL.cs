using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICRL.ModeloDB;
using System.Data.SqlClient;
using System.Configuration;

namespace ICRL.BD
{
    public class CotizacionICRL
    {
        //private static string strCadenaConexion = System.Configuration.ConfigurationManager.AppSettings["conexionbasedatos"];
        //private static string strCadenaConexion = "Server=Wilmer-HP;database=LBCDesa;Uid=usuarioLBC;pwd=SQL2019desa";
        public static string strCadenaConexion = ConfigurationManager.ConnectionStrings["LBCDesaConnectionString"].ConnectionString;
        public enum TipoItem
        {
            Reparacion = 1,
            Repuesto = 2
        };
        //En Sumatorias Generar el tipo_descuento tiene que tener valor "Fijo" no vacio

        public class TraerCotizacionesGrillaUno
        {
            public bool Correcto;
            public List<TipoGrillaUno> Flujos = new List<TipoGrillaUno>();

            public class TipoGrillaUno
            {
                public int idFlujo;
                public string flujoOnBase;
                public string nombreAsegurado;
                public string numeroPoliza;
                public string placaVehiculo;
                public DateTime? fechaSiniestro;
                public int? estado;
            }
        }

        public TraerCotizacionesGrillaUno TraerCotizacionesPendientesGrillaUno(int intEstado)
        {
            TraerCotizacionesGrillaUno objRespuesta = new TraerCotizacionesGrillaUno();
            TraerCotizacionesGrillaUno.TipoGrillaUno teFila;
            using (LBCDesaEntities db = new LBCDesaEntities())
            {
                var vlst = from f in db.Flujo
                           join cf in db.CotizacionFlujo on f.idFlujo equals cf.idFlujo
                           where f.estado == intEstado
                           select new
                           {
                               f.idFlujo,
                               f.flujoOnBase,
                               f.nombreAsegurado,
                               f.numeroPoliza,
                               f.placaVehiculo,
                               f.fechaSiniestro,
                               f.estado
                           };

                foreach (var fila in vlst)
                {
                    teFila = new TraerCotizacionesGrillaUno.TipoGrillaUno();
                    teFila.idFlujo = fila.idFlujo;
                    teFila.estado = fila.estado;
                    teFila.fechaSiniestro = fila.fechaSiniestro;
                    teFila.flujoOnBase = fila.flujoOnBase;
                    teFila.nombreAsegurado = fila.nombreAsegurado;
                    teFila.numeroPoliza = fila.numeroPoliza;
                    teFila.placaVehiculo = fila.placaVehiculo;
                    objRespuesta.Flujos.Add(teFila);
                }
            }
            objRespuesta.Correcto = true;
            return objRespuesta;
        }

        //DANIOS PROPIOS
        public class TipoDaniosPropios
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public short id_tipo_item;
            public string item_descripcion;
            public string chaperio;
            public string reparacion_previa;
            public bool mecanico;
            public bool pintura;
            public bool instalacion;
            public string id_moneda;
            public double precio_cotizado;
            public string id_tipo_descuento;
            public double descuento;
            public double precio_final;
            public string proveedor;
            public double monto_orden;
            public string id_tipo_descuento_orden;
            public double descuento_proveedor;
            public double deducible;
            public double monto_final;
            public bool recepcion;
            public int dias_entrega;
            public DateTime fecha_envio_efectivo;
            public string numero_orden;
            public short id_estado;
            public double tipo_cambio;
            public TipoDaniosPropios()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.id_tipo_item = 0;
                this.item_descripcion = "";
                this.chaperio = "";
                this.reparacion_previa = "";
                this.mecanico = false;
                this.pintura = false;
                this.instalacion = false;
                this.id_moneda = "";
                this.precio_cotizado = 0.0;
                this.id_tipo_descuento = "";
                this.descuento = 0.0;
                this.precio_final = 0.0;
                this.proveedor = "";
                this.monto_orden = 0.0;
                this.id_tipo_descuento_orden = "";
                this.descuento_proveedor = 0.0;
                this.deducible = 0.0;
                this.monto_final = 0.0;
                this.recepcion = false;
                this.dias_entrega = 0;
                this.fecha_envio_efectivo = new DateTime(2000, 1, 1);
                this.numero_orden = "";
                this.id_estado = 0;
                this.tipo_cambio = 0.0;
            }
            public TipoDaniosPropios CrearCopia()
            {
                TipoDaniosPropios objRespuesta = new TipoDaniosPropios();
                objRespuesta.id_flujo = this.id_flujo;
                objRespuesta.id_cotizacion = this.id_cotizacion;
                objRespuesta.id_item = this.id_item;
                objRespuesta.id_tipo_item = this.id_tipo_item;
                objRespuesta.item_descripcion = this.item_descripcion;
                objRespuesta.chaperio = this.chaperio;
                objRespuesta.reparacion_previa = this.reparacion_previa;
                objRespuesta.mecanico = this.mecanico;
                objRespuesta.pintura = this.pintura;
                objRespuesta.instalacion = this.instalacion;
                objRespuesta.id_moneda = this.id_moneda;
                objRespuesta.precio_cotizado = this.precio_cotizado;
                objRespuesta.id_tipo_descuento = this.id_tipo_descuento;
                objRespuesta.descuento = this.descuento;
                objRespuesta.precio_final = this.precio_final;
                objRespuesta.proveedor = this.proveedor;
                objRespuesta.monto_orden = this.monto_orden;
                objRespuesta.id_tipo_descuento_orden = this.id_tipo_descuento_orden;
                objRespuesta.descuento_proveedor = this.descuento_proveedor;
                objRespuesta.deducible = this.deducible;
                objRespuesta.monto_final = this.monto_final;
                objRespuesta.recepcion = this.recepcion;
                objRespuesta.dias_entrega = this.dias_entrega;
                objRespuesta.fecha_envio_efectivo = this.fecha_envio_efectivo;
                objRespuesta.numero_orden = this.numero_orden;
                objRespuesta.id_estado = this.id_estado;
                objRespuesta.tipo_cambio = this.tipo_cambio;
                return objRespuesta;
            }
        }
        public static bool DaniosPropiosRegistrar(TipoDaniosPropios DaniosPropios)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_danios_propios] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio])" +
              " VALUES (@id_flujo, @id_cotizacion, @id_tipo_item, @item_descripcion, @chaperio,@reparacion_previa, @mecanico, @pintura, @instalacion, @id_moneda, @precio_cotizado, @id_tipo_descuento,@descuento,@precio_final,@proveedor,@monto_orden, @id_tipo_descuento_orden, @descuento_proveedor, @deducible, @monto_final, @recepcion, @dias_entrega, @fecha_envio_efectivo, @numero_orden, @id_estado,@tipo_cambio)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = DaniosPropios.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = DaniosPropios.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = DaniosPropios.id_tipo_item;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 150).Value = DaniosPropios.item_descripcion;
                sqlComando.Parameters.Add("@chaperio", System.Data.SqlDbType.VarChar, 50).Value = DaniosPropios.chaperio;
                sqlComando.Parameters.Add("@reparacion_previa", System.Data.SqlDbType.VarChar, 50).Value = DaniosPropios.reparacion_previa;
                sqlComando.Parameters.Add("@mecanico", System.Data.SqlDbType.Bit).Value = DaniosPropios.mecanico;
                sqlComando.Parameters.Add("@pintura", System.Data.SqlDbType.Bit).Value = DaniosPropios.pintura;
                sqlComando.Parameters.Add("@instalacion", System.Data.SqlDbType.Bit).Value = DaniosPropios.instalacion;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropios.id_moneda;
                sqlComando.Parameters.Add("@precio_cotizado", System.Data.SqlDbType.Float).Value = DaniosPropios.precio_cotizado;
                sqlComando.Parameters.Add("@id_tipo_descuento", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropios.id_tipo_descuento;
                sqlComando.Parameters.Add("@descuento", System.Data.SqlDbType.Float).Value = DaniosPropios.descuento;
                sqlComando.Parameters.Add("@precio_final", System.Data.SqlDbType.Float).Value = DaniosPropios.precio_final;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = DaniosPropios.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = DaniosPropios.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropios.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = DaniosPropios.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = DaniosPropios.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = DaniosPropios.monto_final;
                sqlComando.Parameters.Add("@recepcion", System.Data.SqlDbType.Bit).Value = DaniosPropios.recepcion;
                sqlComando.Parameters.Add("@dias_entrega", System.Data.SqlDbType.Int).Value = DaniosPropios.dias_entrega;
                sqlComando.Parameters.Add("@fecha_envio_efectivo", System.Data.SqlDbType.DateTime).Value = DaniosPropios.fecha_envio_efectivo;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = DaniosPropios.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = DaniosPropios.id_estado;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = DaniosPropios.tipo_cambio;
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
        public class TipoDaniosPropiosTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoDaniosPropios> DaniosPropios = new List<TipoDaniosPropios>();
            public System.Data.DataSet dsDaniosPropios = new System.Data.DataSet();
        }
        public static TipoDaniosPropiosTraer DaniosPropiosTraer(int Flujo, int Cotizacion)
        {
            TipoDaniosPropiosTraer objRespuesta = new TipoDaniosPropiosTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[id_item],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio] FROM [dbo].[cotizacion_danios_propios] WHERE [id_flujo]=@id_flujo and [id_cotizacion]=@id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoDaniosPropios tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoDaniosPropios();
                    tdpFila.id_flujo = Convert.ToInt32(sqlDatos["id_flujo"]);
                    tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["chaperio"] != DBNull.Value) tdpFila.chaperio = Convert.ToString(sqlDatos["chaperio"]);
                    if (sqlDatos["reparacion_previa"] != DBNull.Value) tdpFila.reparacion_previa = Convert.ToString(sqlDatos["reparacion_previa"]);
                    if (sqlDatos["mecanico"] != DBNull.Value) tdpFila.mecanico = Convert.ToBoolean(sqlDatos["mecanico"]);
                    if (sqlDatos["pintura"] != DBNull.Value) tdpFila.pintura = Convert.ToBoolean(sqlDatos["pintura"]);
                    if (sqlDatos["instalacion"] != DBNull.Value) tdpFila.instalacion = Convert.ToBoolean(sqlDatos["instalacion"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToString(sqlDatos["id_moneda"]);
                    if (sqlDatos["precio_cotizado"] != DBNull.Value) tdpFila.precio_cotizado = Convert.ToDouble(sqlDatos["precio_cotizado"]);
                    if (sqlDatos["id_tipo_descuento"] != DBNull.Value) tdpFila.id_tipo_descuento = Convert.ToString(sqlDatos["id_tipo_descuento"]);
                    if (sqlDatos["descuento"] != DBNull.Value) tdpFila.descuento = Convert.ToDouble(sqlDatos["descuento"]);
                    if (sqlDatos["precio_final"] != DBNull.Value) tdpFila.precio_final = Convert.ToDouble(sqlDatos["precio_final"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["recepcion"] != DBNull.Value) tdpFila.recepcion = Convert.ToBoolean(sqlDatos["recepcion"]);
                    if (sqlDatos["dias_entrega"] != DBNull.Value) tdpFila.dias_entrega = Convert.ToInt32(sqlDatos["dias_entrega"]);
                    if (sqlDatos["fecha_envio_efectivo"] != DBNull.Value) tdpFila.fecha_envio_efectivo = Convert.ToDateTime(sqlDatos["fecha_envio_efectivo"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    objRespuesta.DaniosPropios.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.Fill(objRespuesta.dsDaniosPropios);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los daños propios de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }

        //Se añade el método para devolver solo un registro de TipoDaniosPropiosTraer
        public static TipoDaniosPropiosTraer DaniosPropiosTraer(int Flujo, int Cotizacion, long Item)
        {
            TipoDaniosPropiosTraer objRespuesta = new TipoDaniosPropiosTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[id_item],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio] FROM [dbo].[cotizacion_danios_propios] WHERE [id_flujo]=@id_flujo and [id_cotizacion]=@id_cotizacion and [id_item]=@id_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoDaniosPropios tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoDaniosPropios();
                    tdpFila.id_flujo = Convert.ToInt32(sqlDatos["id_flujo"]);
                    tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["chaperio"] != DBNull.Value) tdpFila.chaperio = Convert.ToString(sqlDatos["chaperio"]);
                    if (sqlDatos["reparacion_previa"] != DBNull.Value) tdpFila.reparacion_previa = Convert.ToString(sqlDatos["reparacion_previa"]);
                    if (sqlDatos["mecanico"] != DBNull.Value) tdpFila.mecanico = Convert.ToBoolean(sqlDatos["mecanico"]);
                    if (sqlDatos["pintura"] != DBNull.Value) tdpFila.pintura = Convert.ToBoolean(sqlDatos["pintura"]);
                    if (sqlDatos["instalacion"] != DBNull.Value) tdpFila.instalacion = Convert.ToBoolean(sqlDatos["instalacion"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToString(sqlDatos["id_moneda"]);
                    if (sqlDatos["precio_cotizado"] != DBNull.Value) tdpFila.precio_cotizado = Convert.ToDouble(sqlDatos["precio_cotizado"]);
                    if (sqlDatos["id_tipo_descuento"] != DBNull.Value) tdpFila.id_tipo_descuento = Convert.ToString(sqlDatos["id_tipo_descuento"]);
                    if (sqlDatos["descuento"] != DBNull.Value) tdpFila.descuento = Convert.ToDouble(sqlDatos["descuento"]);
                    if (sqlDatos["precio_final"] != DBNull.Value) tdpFila.precio_final = Convert.ToDouble(sqlDatos["precio_final"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["recepcion"] != DBNull.Value) tdpFila.recepcion = Convert.ToBoolean(sqlDatos["recepcion"]);
                    if (sqlDatos["dias_entrega"] != DBNull.Value) tdpFila.dias_entrega = Convert.ToInt32(sqlDatos["dias_entrega"]);
                    if (sqlDatos["fecha_envio_efectivo"] != DBNull.Value) tdpFila.fecha_envio_efectivo = Convert.ToDateTime(sqlDatos["fecha_envio_efectivo"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    objRespuesta.DaniosPropios.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlAdaptador.Fill(objRespuesta.dsDaniosPropios);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los daños propios de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }

        public static bool DaniosPropiosModificar(TipoDaniosPropios DaniosPropios)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_danios_propios] SET [id_tipo_item] = @id_tipo_item,[item_descripcion] = @item_descripcion, [chaperio] = @chaperio,[reparacion_previa] = @reparacion_previa,[mecanico] = @mecanico,[pintura] = @pintura,[instalacion] = @instalacion,[id_moneda] = @id_moneda,[precio_cotizado] = @precio_cotizado,[id_tipo_descuento] = @id_tipo_descuento,[descuento] = @descuento,[precio_final] = @precio_final,[proveedor] = @proveedor,[monto_orden] = @monto_orden,[id_tipo_descuento_orden] = @id_tipo_descuento_orden,[descuento_proveedor] = @descuento_proveedor,[deducible] = @deducible,[monto_final] = @monto_final,[recepcion] = @recepcion,[dias_entrega] = @dias_entrega,[fecha_envio_efectivo] = @fecha_envio_efectivo,[numero_orden] = @numero_orden,[id_estado] = @id_estado,[tipo_cambio] = @tipo_cambio " +
              "WHERE [id_flujo] = @id_flujo and [id_cotizacion] = @id_cotizacion and [id_item] = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = DaniosPropios.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = DaniosPropios.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = DaniosPropios.id_item;

                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = DaniosPropios.id_tipo_item;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 150).Value = DaniosPropios.item_descripcion;
                sqlComando.Parameters.Add("@chaperio", System.Data.SqlDbType.VarChar, 50).Value = DaniosPropios.chaperio;
                sqlComando.Parameters.Add("@reparacion_previa", System.Data.SqlDbType.VarChar, 50).Value = DaniosPropios.reparacion_previa;
                sqlComando.Parameters.Add("@mecanico", System.Data.SqlDbType.Bit).Value = DaniosPropios.mecanico;
                sqlComando.Parameters.Add("@pintura", System.Data.SqlDbType.Bit).Value = DaniosPropios.pintura;
                sqlComando.Parameters.Add("@instalacion", System.Data.SqlDbType.Bit).Value = DaniosPropios.instalacion;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropios.id_moneda;
                sqlComando.Parameters.Add("@precio_cotizado", System.Data.SqlDbType.Float).Value = DaniosPropios.precio_cotizado;
                sqlComando.Parameters.Add("@id_tipo_descuento", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropios.id_tipo_descuento;
                sqlComando.Parameters.Add("@descuento", System.Data.SqlDbType.Float).Value = DaniosPropios.descuento;
                sqlComando.Parameters.Add("@precio_final", System.Data.SqlDbType.Float).Value = DaniosPropios.precio_final;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = DaniosPropios.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = DaniosPropios.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropios.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = DaniosPropios.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = DaniosPropios.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = DaniosPropios.monto_final;
                sqlComando.Parameters.Add("@recepcion", System.Data.SqlDbType.Bit).Value = DaniosPropios.recepcion;
                sqlComando.Parameters.Add("@dias_entrega", System.Data.SqlDbType.Int).Value = DaniosPropios.dias_entrega;
                sqlComando.Parameters.Add("@fecha_envio_efectivo", System.Data.SqlDbType.DateTime).Value = DaniosPropios.fecha_envio_efectivo;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = DaniosPropios.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = DaniosPropios.id_estado;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = DaniosPropios.tipo_cambio;
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
        public static bool DaniosPropiosBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_danios_propios] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
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
        //DANIOS PROPIOS SUMATORIA
        public class TipoDanioPropioSumatoria
        {
            public int id_flujo;
            public int id_cotizacion;
            public short id_tipo_item;
            public string proveedor;
            public double monto_orden;
            public string id_tipo_descuento_orden;
            public double descuento_proveedor;
            public double deducible;
            public double monto_final;
            public string numero_orden;
            public short id_estado;
            public TipoDanioPropioSumatoria()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_tipo_item = 0;
                this.proveedor = "";
                this.monto_orden = 0.0;
                this.id_tipo_descuento_orden = "";
                this.descuento_proveedor = 0;
                this.deducible = 0;
                this.monto_final = 0;
                this.numero_orden = "";
                this.id_estado = 0;
            }
        }
        public static bool DaniosPropiosSumatoriaRegistrar(TipoDanioPropioSumatoria DaniosPropiosSumatorias)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_danios_propios_sumatoria] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado])" +
              " VALUES (@id_flujo,@id_cotizacion,@id_tipo_item,@proveedor,@monto_orden,@id_tipo_descuento_orden,@descuento_proveedor,@deducible,@monto_final,@numero_orden,@id_estado)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = DaniosPropiosSumatorias.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = DaniosPropiosSumatorias.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = DaniosPropiosSumatorias.id_tipo_item;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = DaniosPropiosSumatorias.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatorias.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropiosSumatorias.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatorias.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatorias.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatorias.monto_final;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 30).Value = DaniosPropiosSumatorias.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = DaniosPropiosSumatorias.id_estado;
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
        public class TipoDaniosPropiosSumatoriaTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoDanioPropioSumatoria> DaniosPropiosSumatoria = new List<TipoDanioPropioSumatoria>();
            public System.Data.DataSet dsDaniosPropiosSumatoria = new System.Data.DataSet();
        }
        public static TipoDaniosPropiosSumatoriaTraer DaniosPropiosSumatoriaTraer(int Flujo, int Cotizacion, short Tipo_Item)
        {
            TipoDaniosPropiosSumatoriaTraer objRespuesta = new TipoDaniosPropiosSumatoriaTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado] FROM [dbo].[cotizacion_danios_propios_sumatoria] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_tipo_item = @id_tipo_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoDanioPropioSumatoria tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoDanioPropioSumatoria();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.id_cotizacion = Cotizacion;
                    tdpFila.id_tipo_item = Tipo_Item;
                    //[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final]
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    objRespuesta.DaniosPropiosSumatoria.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
                sqlAdaptador.Fill(objRespuesta.dsDaniosPropiosSumatoria);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los daños propios de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }
        public static bool DaniosPropiosSumatoriaModificar(TipoDanioPropioSumatoria DaniosPropiosSumatoria)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_danios_propios_sumatoria] SET [monto_orden] = @monto_orden,[id_tipo_descuento_orden] = @id_tipo_descuento_orden,[descuento_proveedor] = @descuento_proveedor,[deducible] = @deducible, [monto_final] = @monto_final, [numero_orden] = @numero_orden,[id_estado] = @id_estado " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_tipo_item] = @id_tipo_item AND [proveedor] = @proveedor";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = DaniosPropiosSumatoria.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = DaniosPropiosSumatoria.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = DaniosPropiosSumatoria.id_tipo_item;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = DaniosPropiosSumatoria.proveedor;

                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatoria.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = DaniosPropiosSumatoria.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatoria.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatoria.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = DaniosPropiosSumatoria.monto_final;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 30).Value = DaniosPropiosSumatoria.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = DaniosPropiosSumatoria.id_estado;
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
        public static bool DaniosPropiosSumatoriaBorrar(int Flujo, int Cotizacion, short Tipo_Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_danios_propios_sumatoria] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_tipo_item] = @id_tipo_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
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
        public static bool DaniosPropiosSumatoriaGenerar(int Flujo, int Cotizacion, short Tipo_Item)
        {
            bool blnRespuesta = false;
            blnRespuesta = DaniosPropiosSumatoriaBorrar(Flujo, Cotizacion, Tipo_Item);
            if (!blnRespuesta) return false;
            string strComando = "INSERT INTO [dbo].[cotizacion_danios_propios_sumatoria] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado]) " +
              "SELECT [id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],SUM(precio_final),'Fijo',0.0,0.0,0.0,'',0 FROM [dbo].[cotizacion_danios_propios] " +
              "WHERE [id_flujo] = @id_flujo and [id_cotizacion]= @id_cotizacion and [id_tipo_item] = @id_tipo_item GROUP BY [id_flujo],[id_cotizacion],[id_tipo_item],[proveedor]";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
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
        //RESPONSABILIDAD CIVIL PERSONA
        public class TipoRCPersonas
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public string nombre_apellido;
            public string telefono_contacto;
            public string numero_documento;
            public string tipo_gasto;
            public double monto_gasto;
            public short id_moneda;
            public string descripcion;
            public bool rembolso;
            public double tipo_cambio;
            public short id_estado;
            public TipoRCPersonas()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.nombre_apellido = "";
                this.telefono_contacto = "";
                this.numero_documento = "";
                this.tipo_gasto = "";
                this.monto_gasto = 0.0;
                this.id_moneda = 0;
                this.descripcion = "";
                this.rembolso = false;
                this.tipo_cambio = 0.0;
                this.id_estado = 0;
            }
        }
        public static bool RCPersonaRegistrar(TipoRCPersonas RCPersona)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "INSERT INTO [dbo].[cotizacion_rc_personas]([id_flujo],[id_cotizacion],[nombre_apellido],[telefono_contacto],[numero_documento],[tipo_gasto],[monto_gasto],[id_moneda],[descripcion],[rembolso],[tipo_cambio],[id_estado]) " +
              "VALUES(@id_flujo,@id_cotizacion,@nombre_apellido,@telefono_contacto,@numero_documento,@tipo_gasto,@monto_gasto,@id_moneda,@descripcion,@rembolso,@tipo_cambio,@id_estado)";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCPersona.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCPersona.id_cotizacion;
                sqlComando.Parameters.Add("@nombre_apellido", System.Data.SqlDbType.VarChar, 150).Value = RCPersona.nombre_apellido;
                sqlComando.Parameters.Add("@telefono_contacto", System.Data.SqlDbType.VarChar, 15).Value = RCPersona.telefono_contacto;
                sqlComando.Parameters.Add("@numero_documento", System.Data.SqlDbType.VarChar, 15).Value = RCPersona.numero_documento;
                sqlComando.Parameters.Add("@tipo_gasto", System.Data.SqlDbType.VarChar, 50).Value = RCPersona.tipo_gasto;
                sqlComando.Parameters.Add("@monto_gasto", System.Data.SqlDbType.Float).Value = RCPersona.monto_gasto;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = RCPersona.id_moneda;
                sqlComando.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar, 250).Value = RCPersona.descripcion;
                sqlComando.Parameters.Add("@rembolso", System.Data.SqlDbType.Bit).Value = RCPersona.rembolso;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RCPersona.tipo_cambio;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCPersona.id_estado;
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
        public class TipoRCPersonasTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoRCPersonas> RCPersonas = new List<TipoRCPersonas>();
            public System.Data.DataSet dsRCPersonas = new System.Data.DataSet();
        }
        public static TipoRCPersonasTraer RCPersonasTraer(int Flujo, int Cotizacion)
        {
            TipoRCPersonasTraer objRespuesta = new TipoRCPersonasTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            TipoRCPersonas tpFila;
            string strComando = "SELECT [id_item],[nombre_apellido],[telefono_contacto],[numero_documento],[tipo_gasto],[monto_gasto],[id_moneda],[descripcion],[rembolso],[tipo_cambio],[id_estado] FROM [dbo].[cotizacion_rc_personas] WHERE [id_flujo] = @id_flujo AND id_cotizacion = @id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tpFila = new TipoRCPersonas();
                    tpFila.id_flujo = Flujo;
                    tpFila.id_cotizacion = Cotizacion;
                    if (sqlDatos["id_item"] != DBNull.Value) tpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["nombre_apellido"] != DBNull.Value) tpFila.nombre_apellido = Convert.ToString(sqlDatos["nombre_apellido"]);
                    if (sqlDatos["telefono_contacto"] != DBNull.Value) tpFila.telefono_contacto = Convert.ToString(sqlDatos["telefono_contacto"]);
                    if (sqlDatos["numero_documento"] != DBNull.Value) tpFila.numero_documento = Convert.ToString(sqlDatos["numero_documento"]);
                    if (sqlDatos["tipo_gasto"] != DBNull.Value) tpFila.tipo_gasto = Convert.ToString(sqlDatos["tipo_gasto"]);
                    if (sqlDatos["monto_gasto"] != DBNull.Value) tpFila.monto_gasto = Convert.ToDouble(sqlDatos["monto_gasto"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tpFila.id_moneda = Convert.ToInt16(sqlDatos["id_moneda"]);
                    if (sqlDatos["descripcion"] != DBNull.Value) tpFila.descripcion = Convert.ToString(sqlDatos["descripcion"]);
                    if (sqlDatos["rembolso"] != DBNull.Value) tpFila.rembolso = Convert.ToBoolean(sqlDatos["rembolso"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    objRespuesta.RCPersonas.Add(tpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.Fill(objRespuesta.dsRCPersonas);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los datos de la responsabilidad civil de la persona para la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool RCPersonaModificar(TipoRCPersonas RCPersona)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "UPDATE [dbo].[cotizacion_rc_personas] SET [nombre_apellido] = @nombre_apellido,[telefono_contacto] = @telefono_contacto,[numero_documento] = @numero_documento,[tipo_gasto] = @tipo_gasto,[monto_gasto] = @monto_gasto,[id_moneda] = @id_moneda,[descripcion] = @descripcion,[rembolso] = @rembolso,[tipo_cambio] = @tipo_cambio,[id_estado] = @id_estado " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_item] = @id_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCPersona.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCPersona.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = RCPersona.id_item;

                sqlComando.Parameters.Add("@nombre_apellido", System.Data.SqlDbType.VarChar, 150).Value = RCPersona.nombre_apellido;
                sqlComando.Parameters.Add("@telefono_contacto", System.Data.SqlDbType.VarChar, 15).Value = RCPersona.telefono_contacto;
                sqlComando.Parameters.Add("@numero_documento", System.Data.SqlDbType.VarChar, 15).Value = RCPersona.numero_documento;
                sqlComando.Parameters.Add("@tipo_gasto", System.Data.SqlDbType.VarChar, 50).Value = RCPersona.tipo_gasto;
                sqlComando.Parameters.Add("@monto_gasto", System.Data.SqlDbType.Float).Value = RCPersona.monto_gasto;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = RCPersona.id_moneda;
                sqlComando.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar, 250).Value = RCPersona.descripcion;
                sqlComando.Parameters.Add("@rembolso", System.Data.SqlDbType.Bit).Value = RCPersona.rembolso;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RCPersona.tipo_cambio;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCPersona.id_estado;
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
        public static bool RCPersonasBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_rc_personas] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
                blnRespuesta = true;
            }
            catch (Exception ex)
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
        //RESPONSABILIDAD CIVIL OBJETOS
        public class TipoRCObjetos
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public string nombre_apellido;
            public string telefono_contacto;
            public string numero_documento;
            public string tipo_item;
            public double monto_item;
            public short id_moneda;
            public string descripcion;
            public bool rembolso;
            public double tipo_cambio;
            public short id_estado;
            public TipoRCObjetos()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.nombre_apellido = "";
                this.telefono_contacto = "";
                this.numero_documento = "";
                this.tipo_item = "";
                this.monto_item = 0.0;
                this.id_moneda = 0;
                this.descripcion = "";
                this.rembolso = false;
                this.tipo_cambio = 0.0;
                this.id_estado = 0;
            }
        }
        public static bool RCObjetosRegistrar(TipoRCObjetos RCObjetos)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "INSERT INTO [dbo].[cotizacion_rc_objetos]([id_flujo],[id_cotizacion],[nombre_apellido],[telefono_contacto],[numero_documento],[tipo_item],[monto_item],[id_moneda],[descripcion],[rembolso],[tipo_cambio],[id_estado]) " +
              "VALUES(@id_flujo,@id_cotizacion,@nombre_apellido,@telefono_contacto,@numero_documento,@tipo_item,@monto_item,@id_moneda,@descripcion,@rembolso,@tipo_cambio,@id_estado)";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCObjetos.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCObjetos.id_cotizacion;
                sqlComando.Parameters.Add("@nombre_apellido", System.Data.SqlDbType.VarChar, 150).Value = RCObjetos.nombre_apellido;
                sqlComando.Parameters.Add("@telefono_contacto", System.Data.SqlDbType.VarChar, 15).Value = RCObjetos.telefono_contacto;
                sqlComando.Parameters.Add("@numero_documento", System.Data.SqlDbType.VarChar, 15).Value = RCObjetos.numero_documento;
                sqlComando.Parameters.Add("@tipo_item", System.Data.SqlDbType.VarChar, 50).Value = RCObjetos.tipo_item;
                sqlComando.Parameters.Add("@monto_item", System.Data.SqlDbType.Float).Value = RCObjetos.monto_item;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = RCObjetos.id_moneda;
                sqlComando.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar, 250).Value = RCObjetos.descripcion;
                sqlComando.Parameters.Add("@rembolso", System.Data.SqlDbType.Bit).Value = RCObjetos.rembolso;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RCObjetos.tipo_cambio;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCObjetos.id_estado;
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
        public class TipoRCObjetosTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoRCObjetos> RCObjetos = new List<TipoRCObjetos>();
            public System.Data.DataSet dsRCObjetos = new System.Data.DataSet();
        }
        public static TipoRCObjetosTraer RCObjetosTraer(int Flujo, int Cotizacion)
        {
            TipoRCObjetosTraer objRespuesta = new TipoRCObjetosTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            TipoRCObjetos tpFila;
            string strComando = "SELECT [id_item],[nombre_apellido],[telefono_contacto],[numero_documento],[tipo_item],[monto_item],[id_moneda],[descripcion],[rembolso],[tipo_cambio],[id_estado] FROM [dbo].[cotizacion_rc_objetos] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tpFila = new TipoRCObjetos();
                    tpFila.id_flujo = Flujo;
                    tpFila.id_cotizacion = Cotizacion;
                    if (sqlDatos["id_item"] != DBNull.Value) tpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["nombre_apellido"] != DBNull.Value) tpFila.nombre_apellido = Convert.ToString(sqlDatos["nombre_apellido"]);
                    if (sqlDatos["telefono_contacto"] != DBNull.Value) tpFila.telefono_contacto = Convert.ToString(sqlDatos["telefono_contacto"]);
                    if (sqlDatos["numero_documento"] != DBNull.Value) tpFila.numero_documento = Convert.ToString(sqlDatos["numero_documento"]);
                    if (sqlDatos["tipo_item"] != DBNull.Value) tpFila.tipo_item = Convert.ToString(sqlDatos["tipo_item"]);
                    if (sqlDatos["monto_item"] != DBNull.Value) tpFila.monto_item = Convert.ToDouble(sqlDatos["monto_item"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tpFila.id_moneda = Convert.ToInt16(sqlDatos["id_moneda"]);
                    if (sqlDatos["descripcion"] != DBNull.Value) tpFila.descripcion = Convert.ToString(sqlDatos["descripcion"]);
                    if (sqlDatos["rembolso"] != DBNull.Value) tpFila.rembolso = Convert.ToBoolean(sqlDatos["rembolso"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    objRespuesta.RCObjetos.Add(tpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.Fill(objRespuesta.dsRCObjetos);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los datos de la responsabilidad civil del objeto para la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool RCObjetosModificar(TipoRCObjetos RCObjetos)
        {
            bool blnRespuesta = false;
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "UPDATE [dbo].[cotizacion_rc_objetos] SET [nombre_apellido] = @nombre_apellido,[telefono_contacto] = @telefono_contacto,[numero_documento] = @numero_documento,[tipo_item] = @tipo_item,[monto_item] = @monto_item,[id_moneda] = @id_moneda,[descripcion] = @descripcion,[rembolso] = @rembolso,[tipo_cambio] = @tipo_cambio,[id_estado] = @id_estado " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_item] = @id_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCObjetos.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCObjetos.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = RCObjetos.id_item;

                sqlComando.Parameters.Add("@nombre_apellido", System.Data.SqlDbType.VarChar, 150).Value = RCObjetos.nombre_apellido;
                sqlComando.Parameters.Add("@telefono_contacto", System.Data.SqlDbType.VarChar, 15).Value = RCObjetos.telefono_contacto;
                sqlComando.Parameters.Add("@numero_documento", System.Data.SqlDbType.VarChar, 15).Value = RCObjetos.numero_documento;
                sqlComando.Parameters.Add("@tipo_item", System.Data.SqlDbType.VarChar, 50).Value = RCObjetos.tipo_item;
                sqlComando.Parameters.Add("@monto_item", System.Data.SqlDbType.Float).Value = RCObjetos.monto_item;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = RCObjetos.id_moneda;
                sqlComando.Parameters.Add("@descripcion", System.Data.SqlDbType.VarChar, 250).Value = RCObjetos.descripcion;
                sqlComando.Parameters.Add("@rembolso", System.Data.SqlDbType.Bit).Value = RCObjetos.rembolso;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RCObjetos.tipo_cambio;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCObjetos.id_estado;
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
        public static bool RCObjetosBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_rc_objetos] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
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
        //RESPONSABILIDAD SOCIAL VEHICULAR
        public class TipoRCVehicular
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public short id_tipo_item;
            public string item_descripcion;
            public string chaperio;
            public string reparacion_previa;
            public bool mecanico;
            public bool pintura;
            public bool instalacion;
            public string id_moneda;
            public double precio_cotizado;
            public string id_tipo_descuento;
            public double descuento;
            public double precio_final;
            public string proveedor;
            public double monto_orden;
            public string id_tipo_descuento_orden;
            public double descuento_proveedor;
            public double deducible;
            public double monto_final;
            public bool recepcion;
            public int dias_entrega;
            public DateTime fecha_envio_efectivo;
            public string numero_orden;
            public short id_estado;
            public double tipo_cambio;
            public TipoRCVehicular()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.id_tipo_item = 0;
                this.item_descripcion = "";
                this.chaperio = "";
                this.reparacion_previa = "";
                this.mecanico = false;
                this.pintura = false;
                this.instalacion = false;
                this.id_moneda = "";
                this.precio_cotizado = 0.0;
                this.id_tipo_descuento = "";
                this.descuento = 0.0;
                this.precio_final = 0.0;
                this.proveedor = "";
                this.monto_orden = 0.0;
                this.id_tipo_descuento_orden = "";
                this.descuento_proveedor = 0.0;
                this.deducible = 0.0;
                this.monto_final = 0.0;
                this.recepcion = false;
                this.dias_entrega = 0;
                this.fecha_envio_efectivo = new DateTime(2000, 1, 1);
                this.numero_orden = "";
                this.id_estado = 0;
                this.tipo_cambio = 0.0;
            }
            public TipoRCVehicular CrearCopia()
            {
                TipoRCVehicular objRespuesta = new TipoRCVehicular();
                objRespuesta.id_flujo = this.id_flujo;
                objRespuesta.id_cotizacion = this.id_cotizacion;
                objRespuesta.id_item = this.id_item;
                objRespuesta.id_tipo_item = this.id_tipo_item;
                objRespuesta.item_descripcion = this.item_descripcion;
                objRespuesta.chaperio = this.chaperio;
                objRespuesta.reparacion_previa = this.reparacion_previa;
                objRespuesta.mecanico = this.mecanico;
                objRespuesta.pintura = this.pintura;
                objRespuesta.instalacion = this.instalacion;
                objRespuesta.id_moneda = this.id_moneda;
                objRespuesta.precio_cotizado = this.precio_cotizado;
                objRespuesta.id_tipo_descuento = this.id_tipo_descuento;
                objRespuesta.descuento = this.descuento;
                objRespuesta.precio_final = this.precio_final;
                objRespuesta.proveedor = this.proveedor;
                objRespuesta.monto_orden = this.monto_orden;
                objRespuesta.id_tipo_descuento_orden = this.id_tipo_descuento_orden;
                objRespuesta.descuento_proveedor = this.descuento_proveedor;
                objRespuesta.deducible = this.deducible;
                objRespuesta.monto_final = this.monto_final;
                objRespuesta.recepcion = this.recepcion;
                objRespuesta.dias_entrega = this.dias_entrega;
                objRespuesta.fecha_envio_efectivo = this.fecha_envio_efectivo;
                objRespuesta.numero_orden = this.numero_orden;
                objRespuesta.id_estado = this.id_estado;
                objRespuesta.tipo_cambio = this.tipo_cambio;
                return objRespuesta;
            }
        }
        public static bool RCVehicularRegistrar(TipoRCVehicular RCVehicular)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_rc_vehicular] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio])" +
              " VALUES (@id_flujo, @id_cotizacion, @id_tipo_item, @item_descripcion, @chaperio,@reparacion_previa, @mecanico, @pintura, @instalacion, @id_moneda, @precio_cotizado, @id_tipo_descuento,@descuento,@precio_final,@proveedor,@monto_orden, @id_tipo_descuento_orden, @descuento_proveedor, @deducible, @monto_final, @recepcion, @dias_entrega, @fecha_envio_efectivo, @numero_orden, @id_estado,@tipo_cambio)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCVehicular.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCVehicular.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RCVehicular.id_tipo_item;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 150).Value = RCVehicular.item_descripcion;
                sqlComando.Parameters.Add("@chaperio", System.Data.SqlDbType.VarChar, 50).Value = RCVehicular.chaperio;
                sqlComando.Parameters.Add("@reparacion_previa", System.Data.SqlDbType.VarChar, 50).Value = RCVehicular.reparacion_previa;
                sqlComando.Parameters.Add("@mecanico", System.Data.SqlDbType.Bit).Value = RCVehicular.mecanico;
                sqlComando.Parameters.Add("@pintura", System.Data.SqlDbType.Bit).Value = RCVehicular.pintura;
                sqlComando.Parameters.Add("@instalacion", System.Data.SqlDbType.Bit).Value = RCVehicular.instalacion;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.VarChar, 10).Value = RCVehicular.id_moneda;
                sqlComando.Parameters.Add("@precio_cotizado", System.Data.SqlDbType.Float).Value = RCVehicular.precio_cotizado;
                sqlComando.Parameters.Add("@id_tipo_descuento", System.Data.SqlDbType.VarChar, 10).Value = RCVehicular.id_tipo_descuento;
                sqlComando.Parameters.Add("@descuento", System.Data.SqlDbType.Float).Value = RCVehicular.descuento;
                sqlComando.Parameters.Add("@precio_final", System.Data.SqlDbType.Float).Value = RCVehicular.precio_final;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RCVehicular.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RCVehicular.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RCVehicular.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RCVehicular.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RCVehicular.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RCVehicular.monto_final;
                sqlComando.Parameters.Add("@recepcion", System.Data.SqlDbType.Bit).Value = RCVehicular.recepcion;
                sqlComando.Parameters.Add("@dias_entrega", System.Data.SqlDbType.Int).Value = RCVehicular.dias_entrega;
                sqlComando.Parameters.Add("@fecha_envio_efectivo", System.Data.SqlDbType.DateTime).Value = RCVehicular.fecha_envio_efectivo;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = RCVehicular.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCVehicular.id_estado;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RCVehicular.tipo_cambio;
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
        public class TipoRCVehicularTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoRCVehicular> RCVehiculares = new List<TipoRCVehicular>();
            public System.Data.DataSet dsRCVehiculares = new System.Data.DataSet();
        }
        public static TipoRCVehicularTraer RCVehicularesTraer(int Flujo, int Cotizacion)
        {
            TipoRCVehicularTraer objRespuesta = new TipoRCVehicularTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[id_item],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio] FROM [dbo].[cotizacion_rc_vehicular] WHERE [id_flujo]=@id_flujo and [id_cotizacion]=@id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRCVehicular tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRCVehicular();
                    tdpFila.id_flujo = Convert.ToInt32(sqlDatos["id_flujo"]);
                    tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["chaperio"] != DBNull.Value) tdpFila.chaperio = Convert.ToString(sqlDatos["chaperio"]);
                    if (sqlDatos["reparacion_previa"] != DBNull.Value) tdpFila.reparacion_previa = Convert.ToString(sqlDatos["reparacion_previa"]);
                    if (sqlDatos["mecanico"] != DBNull.Value) tdpFila.mecanico = Convert.ToBoolean(sqlDatos["mecanico"]);
                    if (sqlDatos["pintura"] != DBNull.Value) tdpFila.pintura = Convert.ToBoolean(sqlDatos["pintura"]);
                    if (sqlDatos["instalacion"] != DBNull.Value) tdpFila.instalacion = Convert.ToBoolean(sqlDatos["instalacion"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToString(sqlDatos["id_moneda"]);
                    if (sqlDatos["precio_cotizado"] != DBNull.Value) tdpFila.precio_cotizado = Convert.ToDouble(sqlDatos["precio_cotizado"]);
                    if (sqlDatos["id_tipo_descuento"] != DBNull.Value) tdpFila.id_tipo_descuento = Convert.ToString(sqlDatos["id_tipo_descuento"]);
                    if (sqlDatos["descuento"] != DBNull.Value) tdpFila.descuento = Convert.ToDouble(sqlDatos["descuento"]);
                    if (sqlDatos["precio_final"] != DBNull.Value) tdpFila.precio_final = Convert.ToDouble(sqlDatos["precio_final"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["recepcion"] != DBNull.Value) tdpFila.recepcion = Convert.ToBoolean(sqlDatos["recepcion"]);
                    if (sqlDatos["dias_entrega"] != DBNull.Value) tdpFila.dias_entrega = Convert.ToInt32(sqlDatos["dias_entrega"]);
                    if (sqlDatos["fecha_envio_efectivo"] != DBNull.Value) tdpFila.fecha_envio_efectivo = Convert.ToDateTime(sqlDatos["fecha_envio_efectivo"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    objRespuesta.RCVehiculares.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.Fill(objRespuesta.dsRCVehiculares);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los RC Vehiculares de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }

        //Se añade el método para devolver solo un registro de TipoRCVehicularTraer
        public static TipoRCVehicularTraer RCVehicularesTraer(int Flujo, int Cotizacion, long Item)
        {
            TipoRCVehicularTraer objRespuesta = new TipoRCVehicularTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[id_item],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio] FROM [dbo].[cotizacion_rc_vehicular] WHERE [id_flujo]=@id_flujo and [id_cotizacion]=@id_cotizacion and [id_item]=@id_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRCVehicular tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRCVehicular();
                    tdpFila.id_flujo = Convert.ToInt32(sqlDatos["id_flujo"]);
                    tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["chaperio"] != DBNull.Value) tdpFila.chaperio = Convert.ToString(sqlDatos["chaperio"]);
                    if (sqlDatos["reparacion_previa"] != DBNull.Value) tdpFila.reparacion_previa = Convert.ToString(sqlDatos["reparacion_previa"]);
                    if (sqlDatos["mecanico"] != DBNull.Value) tdpFila.mecanico = Convert.ToBoolean(sqlDatos["mecanico"]);
                    if (sqlDatos["pintura"] != DBNull.Value) tdpFila.pintura = Convert.ToBoolean(sqlDatos["pintura"]);
                    if (sqlDatos["instalacion"] != DBNull.Value) tdpFila.instalacion = Convert.ToBoolean(sqlDatos["instalacion"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToString(sqlDatos["id_moneda"]);
                    if (sqlDatos["precio_cotizado"] != DBNull.Value) tdpFila.precio_cotizado = Convert.ToDouble(sqlDatos["precio_cotizado"]);
                    if (sqlDatos["id_tipo_descuento"] != DBNull.Value) tdpFila.id_tipo_descuento = Convert.ToString(sqlDatos["id_tipo_descuento"]);
                    if (sqlDatos["descuento"] != DBNull.Value) tdpFila.descuento = Convert.ToDouble(sqlDatos["descuento"]);
                    if (sqlDatos["precio_final"] != DBNull.Value) tdpFila.precio_final = Convert.ToDouble(sqlDatos["precio_final"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["recepcion"] != DBNull.Value) tdpFila.recepcion = Convert.ToBoolean(sqlDatos["recepcion"]);
                    if (sqlDatos["dias_entrega"] != DBNull.Value) tdpFila.dias_entrega = Convert.ToInt32(sqlDatos["dias_entrega"]);
                    if (sqlDatos["fecha_envio_efectivo"] != DBNull.Value) tdpFila.fecha_envio_efectivo = Convert.ToDateTime(sqlDatos["fecha_envio_efectivo"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    objRespuesta.RCVehiculares.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlAdaptador.Fill(objRespuesta.dsRCVehiculares);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los RC Vehiculares de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }

        public static bool RCVehicularesModificar(TipoRCVehicular RCVehicular)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_rc_vehicular] SET [id_tipo_item] = @id_tipo_item,[item_descripcion] = @item_descripcion, [chaperio] = @chaperio,[reparacion_previa] = @reparacion_previa,[mecanico] = @mecanico,[pintura] = @pintura,[instalacion] = @instalacion,[id_moneda] = @id_moneda,[precio_cotizado] = @precio_cotizado,[id_tipo_descuento] = @id_tipo_descuento,[descuento] = @descuento,[precio_final] = @precio_final,[proveedor] = @proveedor,[monto_orden] = @monto_orden,[id_tipo_descuento_orden] = @id_tipo_descuento_orden,[descuento_proveedor] = @descuento_proveedor,[deducible] = @deducible,[monto_final] = @monto_final,[recepcion] = @recepcion,[dias_entrega] = @dias_entrega,[fecha_envio_efectivo] = @fecha_envio_efectivo,[numero_orden] = @numero_orden,[id_estado] = @id_estado,[tipo_cambio] = @tipo_cambio " +
              "WHERE [id_flujo] = @id_flujo and [id_cotizacion] = @id_cotizacion and [id_item] = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCVehicular.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCVehicular.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = RCVehicular.id_item;

                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RCVehicular.id_tipo_item;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 150).Value = RCVehicular.item_descripcion;
                sqlComando.Parameters.Add("@chaperio", System.Data.SqlDbType.VarChar, 50).Value = RCVehicular.chaperio;
                sqlComando.Parameters.Add("@reparacion_previa", System.Data.SqlDbType.VarChar, 50).Value = RCVehicular.reparacion_previa;
                sqlComando.Parameters.Add("@mecanico", System.Data.SqlDbType.Bit).Value = RCVehicular.mecanico;
                sqlComando.Parameters.Add("@pintura", System.Data.SqlDbType.Bit).Value = RCVehicular.pintura;
                sqlComando.Parameters.Add("@instalacion", System.Data.SqlDbType.Bit).Value = RCVehicular.instalacion;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.VarChar, 10).Value = RCVehicular.id_moneda;
                sqlComando.Parameters.Add("@precio_cotizado", System.Data.SqlDbType.Float).Value = RCVehicular.precio_cotizado;
                sqlComando.Parameters.Add("@id_tipo_descuento", System.Data.SqlDbType.VarChar, 10).Value = RCVehicular.id_tipo_descuento;
                sqlComando.Parameters.Add("@descuento", System.Data.SqlDbType.Float).Value = RCVehicular.descuento;
                sqlComando.Parameters.Add("@precio_final", System.Data.SqlDbType.Float).Value = RCVehicular.precio_final;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RCVehicular.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RCVehicular.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RCVehicular.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RCVehicular.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RCVehicular.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RCVehicular.monto_final;
                sqlComando.Parameters.Add("@recepcion", System.Data.SqlDbType.Bit).Value = RCVehicular.recepcion;
                sqlComando.Parameters.Add("@dias_entrega", System.Data.SqlDbType.Int).Value = RCVehicular.dias_entrega;
                sqlComando.Parameters.Add("@fecha_envio_efectivo", System.Data.SqlDbType.DateTime).Value = RCVehicular.fecha_envio_efectivo;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = RCVehicular.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCVehicular.id_estado;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RCVehicular.tipo_cambio;
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
        public static bool RCVehicularBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_rc_vehicular] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
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
        //RESPONSABILIDAD SOCIAL VEHICULAR SUMATORIA
        public class TipoRCVehicularSumatoria
        {
            public int id_flujo;
            public int id_cotizacion;
            public short id_tipo_item;
            public string proveedor;
            public double monto_orden;
            public string id_tipo_descuento_orden;
            public double descuento_proveedor;
            public double deducible;
            public double monto_final;
            public string numero_orden;
            public short id_estado;
            public TipoRCVehicularSumatoria()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_tipo_item = 0;
                this.proveedor = "";
                this.monto_orden = 0.0;
                this.id_tipo_descuento_orden = "";
                this.descuento_proveedor = 0;
                this.deducible = 0;
                this.monto_final = 0;
                this.numero_orden = "";
                this.id_estado = 0;
            }
        }
        public static bool RCVehicularSumatoriaRegistrar(TipoRCVehicularSumatoria RCVehicularSumatoria)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_rc_vehicular_sumatoria] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado])" +
              " VALUES (@id_flujo,@id_cotizacion,@id_tipo_item,@proveedor,@monto_orden,@id_tipo_descuento_orden,@descuento_proveedor,@deducible,@monto_final,@numero_orden,@id_estado)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCVehicularSumatoria.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCVehicularSumatoria.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RCVehicularSumatoria.id_tipo_item;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RCVehicularSumatoria.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RCVehicularSumatoria.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RCVehicularSumatoria.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RCVehicularSumatoria.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RCVehicularSumatoria.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RCVehicularSumatoria.monto_final;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 30).Value = RCVehicularSumatoria.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCVehicularSumatoria.id_estado;
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
        public class TipoRCVehicularSumatoriaTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoRCVehicularSumatoria> RCVehicularesSumatoria = new List<TipoRCVehicularSumatoria>();
            public System.Data.DataSet dsRCVehicularSumatoria = new System.Data.DataSet();
        }
        public static TipoRCVehicularSumatoriaTraer RCVehicularSumatoriaTraer(int Flujo, int Cotizacion, short Tipo_Item)
        {
            TipoRCVehicularSumatoriaTraer objRespuesta = new TipoRCVehicularSumatoriaTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado] FROM [dbo].[cotizacion_rc_vehicular_sumatoria] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_tipo_item = @id_tipo_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRCVehicularSumatoria tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRCVehicularSumatoria();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.id_cotizacion = Cotizacion;
                    tdpFila.id_tipo_item = Tipo_Item;
                    //[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final]
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    //[],[]
                    objRespuesta.RCVehicularesSumatoria.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
                sqlAdaptador.Fill(objRespuesta.dsRCVehicularSumatoria);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los RC Vehiculares sumados de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool RCVehicularSumatoriaModificar(TipoRCVehicularSumatoria RCvehicularSumatoria)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_rc_vehicular_sumatoria] SET [monto_orden] = @monto_orden,[id_tipo_descuento_orden] = @id_tipo_descuento_orden,[descuento_proveedor] = @descuento_proveedor,[deducible] = @deducible, [monto_final] = @monto_final, [numero_orden] = @numero_orden, [id_estado] = @id_estado " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_tipo_item] = @id_tipo_item AND [proveedor] = @proveedor";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCvehicularSumatoria.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCvehicularSumatoria.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RCvehicularSumatoria.id_tipo_item;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RCvehicularSumatoria.proveedor;

                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RCvehicularSumatoria.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RCvehicularSumatoria.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RCvehicularSumatoria.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RCvehicularSumatoria.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RCvehicularSumatoria.monto_final;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 30).Value = RCvehicularSumatoria.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RCvehicularSumatoria.id_estado;
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
        public static bool RCVehicularSumatoriaBorrar(int Flujo, int Cotizacion, short Tipo_Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_rc_vehicular_sumatoria] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_tipo_item] = @id_tipo_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
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
        public static bool RCVehicularSumatoriaGenerar(int Flujo, int Cotizacion, short Tipo_Item)
        {
            bool blnRespuesta = false;
            blnRespuesta = RCVehicularSumatoriaBorrar(Flujo, Cotizacion, Tipo_Item);
            if (!blnRespuesta) return false;
            string strComando = "INSERT INTO [dbo].[cotizacion_rc_vehicular_sumatoria] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado]) " +
              "SELECT [id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],SUM(precio_final),'Fijo',0.0,0.0,0.0,'',0 FROM [dbo].[cotizacion_rc_vehicular] " +
              "WHERE [id_flujo] = @id_flujo and [id_cotizacion]= @id_cotizacion and [id_tipo_item] = @id_tipo_item GROUP BY [id_flujo],[id_cotizacion],[id_tipo_item],[proveedor]";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
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
        //RESPONSABILIDAD SOCIAL VEHICULAR TERCERO
        public class TipoRCVehicularTercero
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public string nombre;
            public string telefono;
            public string color;
            public string documento;
            public string marca;
            public string modelo;
            public string placa;
            public string anio;
            public string chasis;
            public string taller;
            public TipoRCVehicularTercero()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.nombre = "";
                this.telefono = "";
                this.color = "";
                this.documento = "";
                this.marca = "";
                this.modelo = "";
                this.placa = "";
                this.anio = "";
                this.chasis = "";
                this.taller = "";
            }
        }
        public static bool RCVehicularTerceroRegistrar(TipoRCVehicularTercero RCVehicularTercero)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_rc_vehicular_tercero] " +
              "([id_flujo],[id_cotizacion],[nombre],[telefono],[color],[documento],[marca],[modelo],[placa],[anio],[chasis],[taller]) " +
              " VALUES (@id_flujo,@id_cotizacion,@nombre,@telefono,@color,@documento,@marca,@modelo,@placa,@anio,@chasis,@taller)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCVehicularTercero.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCVehicularTercero.id_cotizacion;
                sqlComando.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar, 150).Value = RCVehicularTercero.nombre;
                sqlComando.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar, 20).Value = RCVehicularTercero.telefono;
                sqlComando.Parameters.Add("@color", System.Data.SqlDbType.VarChar, 20).Value = RCVehicularTercero.color;
                sqlComando.Parameters.Add("@documento", System.Data.SqlDbType.VarChar, 30).Value = RCVehicularTercero.documento;
                sqlComando.Parameters.Add("@marca", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.marca;
                sqlComando.Parameters.Add("@modelo", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.modelo;
                sqlComando.Parameters.Add("@placa", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.placa;
                sqlComando.Parameters.Add("@anio", System.Data.SqlDbType.VarChar, 10).Value = RCVehicularTercero.anio;
                sqlComando.Parameters.Add("@chasis", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.chasis;
                sqlComando.Parameters.Add("@taller", System.Data.SqlDbType.VarChar, 30).Value = RCVehicularTercero.taller;
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
        public class TipoRCVehicularTerceroTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoRCVehicularTercero> RCVehicularesTerceros = new List<TipoRCVehicularTercero>();
            public System.Data.DataSet dsRCVehicularTerceros = new System.Data.DataSet();
        }
        public static TipoRCVehicularTerceroTraer RCVehicularTerceroTraer(int Flujo, int Cotizacion)
        {
            TipoRCVehicularTerceroTraer objRespuesta = new TipoRCVehicularTerceroTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_item],[nombre],[telefono],[color],[documento],[marca],[modelo],[placa],[anio],[chasis],[taller] FROM [dbo].[cotizacion_rc_vehicular_tercero] WHERE [id_flujo] = @id_flujo AND [id_cotizacion]=@id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRCVehicularTercero tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRCVehicularTercero();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.id_cotizacion = Cotizacion;
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["nombre"] != DBNull.Value) tdpFila.nombre = Convert.ToString(sqlDatos["nombre"]);
                    if (sqlDatos["telefono"] != DBNull.Value) tdpFila.telefono = Convert.ToString(sqlDatos["telefono"]);
                    if (sqlDatos["color"] != DBNull.Value) tdpFila.color = Convert.ToString(sqlDatos["color"]);
                    if (sqlDatos["documento"] != DBNull.Value) tdpFila.documento = Convert.ToString(sqlDatos["documento"]);
                    if (sqlDatos["marca"] != DBNull.Value) tdpFila.marca = Convert.ToString(sqlDatos["marca"]);
                    if (sqlDatos["modelo"] != DBNull.Value) tdpFila.modelo = Convert.ToString(sqlDatos["modelo"]);
                    if (sqlDatos["placa"] != DBNull.Value) tdpFila.placa = Convert.ToString(sqlDatos["placa"]);
                    if (sqlDatos["anio"] != DBNull.Value) tdpFila.anio = Convert.ToString(sqlDatos["anio"]);
                    if (sqlDatos["chasis"] != DBNull.Value) tdpFila.chasis = Convert.ToString(sqlDatos["chasis"]);
                    if (sqlDatos["taller"] != DBNull.Value) tdpFila.taller = Convert.ToString(sqlDatos["taller"]);
                    objRespuesta.RCVehicularesTerceros.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.Fill(objRespuesta.dsRCVehicularTerceros);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los RC Vehiculares terceros de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool RCVehicularTerceroModificar(TipoRCVehicularTercero RCVehicularTercero)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_rc_vehicular_tercero] SET [nombre] = @nombre,[telefono] = @telefono,[color] = @color,[documento] = @documento,[marca] = @marca,[modelo] = @modelo,[placa] = @placa,[anio] = @anio,[chasis] = @chasis,[taller] = @taller " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_item] = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RCVehicularTercero.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RCVehicularTercero.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = RCVehicularTercero.id_item;
                sqlComando.Parameters.Add("@nombre", System.Data.SqlDbType.VarChar, 150).Value = RCVehicularTercero.nombre;
                sqlComando.Parameters.Add("@telefono", System.Data.SqlDbType.VarChar, 20).Value = RCVehicularTercero.telefono;
                sqlComando.Parameters.Add("@color", System.Data.SqlDbType.VarChar, 20).Value = RCVehicularTercero.color;
                sqlComando.Parameters.Add("@documento", System.Data.SqlDbType.VarChar, 30).Value = RCVehicularTercero.documento;
                sqlComando.Parameters.Add("@marca", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.marca;
                sqlComando.Parameters.Add("@modelo", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.modelo;
                sqlComando.Parameters.Add("@placa", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.placa;
                sqlComando.Parameters.Add("@anio", System.Data.SqlDbType.VarChar, 10).Value = RCVehicularTercero.anio;
                sqlComando.Parameters.Add("@chasis", System.Data.SqlDbType.VarChar, 50).Value = RCVehicularTercero.chasis;
                sqlComando.Parameters.Add("@taller", System.Data.SqlDbType.VarChar, 30).Value = RCVehicularTercero.taller;
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
        public static bool RCVehicularTerceroBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_rc_vehicular_tercero] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_item] = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
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
        //ROBO PARCIAL
        public class TipoRoboParcial
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public short id_tipo_item;
            public string item_descripcion;
            public string chaperio;
            public string reparacion_previa;
            public bool mecanico;
            public bool pintura;
            public bool instalacion;
            public string id_moneda;
            public double precio_cotizado;
            public string id_tipo_descuento;
            public double descuento;
            public double precio_final;
            public string proveedor;
            public double monto_orden;
            public string id_tipo_descuento_orden;
            public double descuento_proveedor;
            public double deducible;
            public double monto_final;
            public bool recepcion;
            public int dias_entrega;
            public DateTime fecha_envio_efectivo;
            public string numero_orden;
            public short id_estado;
            public double tipo_cambio;
            public TipoRoboParcial()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.id_tipo_item = 0;
                this.item_descripcion = "";
                this.chaperio = "";
                this.reparacion_previa = "";
                this.mecanico = false;
                this.pintura = false;
                this.instalacion = false;
                this.id_moneda = "";
                this.precio_cotizado = 0.0;
                this.id_tipo_descuento = "";
                this.descuento = 0.0;
                this.precio_final = 0.0;
                this.proveedor = "";
                this.monto_orden = 0.0;
                this.id_tipo_descuento_orden = "";
                this.descuento_proveedor = 0.0;
                this.deducible = 0.0;
                this.monto_final = 0.0;
                this.recepcion = false;
                this.dias_entrega = 0;
                this.fecha_envio_efectivo = new DateTime(2000, 1, 1);
                this.numero_orden = "";
                this.id_estado = 0;
                this.tipo_cambio = 0.0;
            }
            public TipoRoboParcial CrearCopia()
            {
                TipoRoboParcial objRespuesta = new TipoRoboParcial();
                objRespuesta.id_flujo = this.id_flujo;
                objRespuesta.id_cotizacion = this.id_cotizacion;
                objRespuesta.id_item = this.id_item;
                objRespuesta.id_tipo_item = this.id_tipo_item;
                objRespuesta.item_descripcion = this.item_descripcion;
                objRespuesta.chaperio = this.chaperio;
                objRespuesta.reparacion_previa = this.reparacion_previa;
                objRespuesta.mecanico = this.mecanico;
                objRespuesta.pintura = this.pintura;
                objRespuesta.instalacion = this.instalacion;
                objRespuesta.id_moneda = this.id_moneda;
                objRespuesta.precio_cotizado = this.precio_cotizado;
                objRespuesta.id_tipo_descuento = this.id_tipo_descuento;
                objRespuesta.descuento = this.descuento;
                objRespuesta.precio_final = this.precio_final;
                objRespuesta.proveedor = this.proveedor;
                objRespuesta.monto_orden = this.monto_orden;
                objRespuesta.id_tipo_descuento_orden = this.id_tipo_descuento_orden;
                objRespuesta.descuento_proveedor = this.descuento_proveedor;
                objRespuesta.deducible = this.deducible;
                objRespuesta.monto_final = this.monto_final;
                objRespuesta.recepcion = this.recepcion;
                objRespuesta.dias_entrega = this.dias_entrega;
                objRespuesta.fecha_envio_efectivo = this.fecha_envio_efectivo;
                objRespuesta.numero_orden = this.numero_orden;
                objRespuesta.id_estado = this.id_estado;
                objRespuesta.tipo_cambio = this.tipo_cambio;
                return objRespuesta;
            }
        }
        public static bool RoboParcialRegistrar(TipoRoboParcial RoboParcial)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_robo_parcial] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio])" +
              " VALUES (@id_flujo, @id_cotizacion, @id_tipo_item, @item_descripcion, @chaperio,@reparacion_previa, @mecanico, @pintura, @instalacion, @id_moneda, @precio_cotizado, @id_tipo_descuento,@descuento,@precio_final,@proveedor,@monto_orden, @id_tipo_descuento_orden, @descuento_proveedor, @deducible, @monto_final, @recepcion, @dias_entrega, @fecha_envio_efectivo, @numero_orden, @id_estado,@tipo_cambio)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RoboParcial.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RoboParcial.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RoboParcial.id_tipo_item;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 150).Value = RoboParcial.item_descripcion;
                sqlComando.Parameters.Add("@chaperio", System.Data.SqlDbType.VarChar, 50).Value = RoboParcial.chaperio;
                sqlComando.Parameters.Add("@reparacion_previa", System.Data.SqlDbType.VarChar, 50).Value = RoboParcial.reparacion_previa;
                sqlComando.Parameters.Add("@mecanico", System.Data.SqlDbType.Bit).Value = RoboParcial.mecanico;
                sqlComando.Parameters.Add("@pintura", System.Data.SqlDbType.Bit).Value = RoboParcial.pintura;
                sqlComando.Parameters.Add("@instalacion", System.Data.SqlDbType.Bit).Value = RoboParcial.instalacion;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.VarChar, 10).Value = RoboParcial.id_moneda;
                sqlComando.Parameters.Add("@precio_cotizado", System.Data.SqlDbType.Float).Value = RoboParcial.precio_cotizado;
                sqlComando.Parameters.Add("@id_tipo_descuento", System.Data.SqlDbType.VarChar, 10).Value = RoboParcial.id_tipo_descuento;
                sqlComando.Parameters.Add("@descuento", System.Data.SqlDbType.Float).Value = RoboParcial.descuento;
                sqlComando.Parameters.Add("@precio_final", System.Data.SqlDbType.Float).Value = RoboParcial.precio_final;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RoboParcial.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RoboParcial.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RoboParcial.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RoboParcial.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RoboParcial.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RoboParcial.monto_final;
                sqlComando.Parameters.Add("@recepcion", System.Data.SqlDbType.Bit).Value = RoboParcial.recepcion;
                sqlComando.Parameters.Add("@dias_entrega", System.Data.SqlDbType.Int).Value = RoboParcial.dias_entrega;
                sqlComando.Parameters.Add("@fecha_envio_efectivo", System.Data.SqlDbType.DateTime).Value = RoboParcial.fecha_envio_efectivo;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = RoboParcial.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RoboParcial.id_estado;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RoboParcial.tipo_cambio;
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
        public class TipoRoboParcialTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoRoboParcial> RobosParciales = new List<TipoRoboParcial>();
            public System.Data.DataSet dsRobosParciales = new System.Data.DataSet();
        }
        public static TipoRoboParcialTraer RoboParcialTraer(int Flujo, int Cotizacion)
        {
            TipoRoboParcialTraer objRespuesta = new TipoRoboParcialTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[id_item],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio] FROM [dbo].[cotizacion_robo_parcial] WHERE [id_flujo]=@id_flujo and [id_cotizacion]=@id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRoboParcial tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRoboParcial();
                    tdpFila.id_flujo = Convert.ToInt32(sqlDatos["id_flujo"]);
                    tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["chaperio"] != DBNull.Value) tdpFila.chaperio = Convert.ToString(sqlDatos["chaperio"]);
                    if (sqlDatos["reparacion_previa"] != DBNull.Value) tdpFila.reparacion_previa = Convert.ToString(sqlDatos["reparacion_previa"]);
                    if (sqlDatos["mecanico"] != DBNull.Value) tdpFila.mecanico = Convert.ToBoolean(sqlDatos["mecanico"]);
                    if (sqlDatos["pintura"] != DBNull.Value) tdpFila.pintura = Convert.ToBoolean(sqlDatos["pintura"]);
                    if (sqlDatos["instalacion"] != DBNull.Value) tdpFila.instalacion = Convert.ToBoolean(sqlDatos["instalacion"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToString(sqlDatos["id_moneda"]);
                    if (sqlDatos["precio_cotizado"] != DBNull.Value) tdpFila.precio_cotizado = Convert.ToDouble(sqlDatos["precio_cotizado"]);
                    if (sqlDatos["id_tipo_descuento"] != DBNull.Value) tdpFila.id_tipo_descuento = Convert.ToString(sqlDatos["id_tipo_descuento"]);
                    if (sqlDatos["descuento"] != DBNull.Value) tdpFila.descuento = Convert.ToDouble(sqlDatos["descuento"]);
                    if (sqlDatos["precio_final"] != DBNull.Value) tdpFila.precio_final = Convert.ToDouble(sqlDatos["precio_final"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["recepcion"] != DBNull.Value) tdpFila.recepcion = Convert.ToBoolean(sqlDatos["recepcion"]);
                    if (sqlDatos["dias_entrega"] != DBNull.Value) tdpFila.dias_entrega = Convert.ToInt32(sqlDatos["dias_entrega"]);
                    if (sqlDatos["fecha_envio_efectivo"] != DBNull.Value) tdpFila.fecha_envio_efectivo = Convert.ToDateTime(sqlDatos["fecha_envio_efectivo"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    objRespuesta.RobosParciales.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.Fill(objRespuesta.dsRobosParciales);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los Robos parciales de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }

        //Se añade el método para devolver solo un registro de TipoRoboParcialTraer
        public static TipoRoboParcialTraer RoboParcialTraer(int Flujo, int Cotizacion, long Item)
        {
            TipoRoboParcialTraer objRespuesta = new TipoRoboParcialTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_flujo],[id_cotizacion],[id_item],[id_tipo_item],[item_descripcion],[chaperio],[reparacion_previa],[mecanico],[pintura],[instalacion],[id_moneda],[precio_cotizado],[id_tipo_descuento],[descuento],[precio_final],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[recepcion],[dias_entrega],[fecha_envio_efectivo],[numero_orden],[id_estado],[tipo_cambio] FROM [dbo].[cotizacion_robo_parcial] WHERE [id_flujo]=@id_flujo and [id_cotizacion]=@id_cotizacion and [id_item]=@id_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRoboParcial tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRoboParcial();
                    tdpFila.id_flujo = Convert.ToInt32(sqlDatos["id_flujo"]);
                    tdpFila.id_cotizacion = Convert.ToInt32(sqlDatos["id_cotizacion"]);
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["id_tipo_item"] != DBNull.Value) tdpFila.id_tipo_item = Convert.ToInt16(sqlDatos["id_tipo_item"]);
                    if (sqlDatos["item_descripcion"] != DBNull.Value) tdpFila.item_descripcion = Convert.ToString(sqlDatos["item_descripcion"]);
                    if (sqlDatos["chaperio"] != DBNull.Value) tdpFila.chaperio = Convert.ToString(sqlDatos["chaperio"]);
                    if (sqlDatos["reparacion_previa"] != DBNull.Value) tdpFila.reparacion_previa = Convert.ToString(sqlDatos["reparacion_previa"]);
                    if (sqlDatos["mecanico"] != DBNull.Value) tdpFila.mecanico = Convert.ToBoolean(sqlDatos["mecanico"]);
                    if (sqlDatos["pintura"] != DBNull.Value) tdpFila.pintura = Convert.ToBoolean(sqlDatos["pintura"]);
                    if (sqlDatos["instalacion"] != DBNull.Value) tdpFila.instalacion = Convert.ToBoolean(sqlDatos["instalacion"]);
                    if (sqlDatos["id_moneda"] != DBNull.Value) tdpFila.id_moneda = Convert.ToString(sqlDatos["id_moneda"]);
                    if (sqlDatos["precio_cotizado"] != DBNull.Value) tdpFila.precio_cotizado = Convert.ToDouble(sqlDatos["precio_cotizado"]);
                    if (sqlDatos["id_tipo_descuento"] != DBNull.Value) tdpFila.id_tipo_descuento = Convert.ToString(sqlDatos["id_tipo_descuento"]);
                    if (sqlDatos["descuento"] != DBNull.Value) tdpFila.descuento = Convert.ToDouble(sqlDatos["descuento"]);
                    if (sqlDatos["precio_final"] != DBNull.Value) tdpFila.precio_final = Convert.ToDouble(sqlDatos["precio_final"]);
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["recepcion"] != DBNull.Value) tdpFila.recepcion = Convert.ToBoolean(sqlDatos["recepcion"]);
                    if (sqlDatos["dias_entrega"] != DBNull.Value) tdpFila.dias_entrega = Convert.ToInt32(sqlDatos["dias_entrega"]);
                    if (sqlDatos["fecha_envio_efectivo"] != DBNull.Value) tdpFila.fecha_envio_efectivo = Convert.ToDateTime(sqlDatos["fecha_envio_efectivo"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
                    objRespuesta.RobosParciales.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlAdaptador.Fill(objRespuesta.dsRobosParciales);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los Robos parciales de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }

        public static bool RoboParcialModificar(TipoRoboParcial RoboParcial)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_robo_parcial] SET [id_tipo_item] = @id_tipo_item,[item_descripcion] = @item_descripcion, [chaperio] = @chaperio,[reparacion_previa] = @reparacion_previa,[mecanico] = @mecanico,[pintura] = @pintura,[instalacion] = @instalacion,[id_moneda] = @id_moneda,[precio_cotizado] = @precio_cotizado,[id_tipo_descuento] = @id_tipo_descuento,[descuento] = @descuento,[precio_final] = @precio_final,[proveedor] = @proveedor,[monto_orden] = @monto_orden,[id_tipo_descuento_orden] = @id_tipo_descuento_orden,[descuento_proveedor] = @descuento_proveedor,[deducible] = @deducible,[monto_final] = @monto_final,[recepcion] = @recepcion,[dias_entrega] = @dias_entrega,[fecha_envio_efectivo] = @fecha_envio_efectivo,[numero_orden] = @numero_orden,[id_estado] = @id_estado,[tipo_cambio] = @tipo_cambio " +
              "WHERE [id_flujo] = @id_flujo and [id_cotizacion] = @id_cotizacion and [id_item] = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RoboParcial.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RoboParcial.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = RoboParcial.id_item;

                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RoboParcial.id_tipo_item;
                sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 150).Value = RoboParcial.item_descripcion;
                sqlComando.Parameters.Add("@chaperio", System.Data.SqlDbType.VarChar, 50).Value = RoboParcial.chaperio;
                sqlComando.Parameters.Add("@reparacion_previa", System.Data.SqlDbType.VarChar, 50).Value = RoboParcial.reparacion_previa;
                sqlComando.Parameters.Add("@mecanico", System.Data.SqlDbType.Bit).Value = RoboParcial.mecanico;
                sqlComando.Parameters.Add("@pintura", System.Data.SqlDbType.Bit).Value = RoboParcial.pintura;
                sqlComando.Parameters.Add("@instalacion", System.Data.SqlDbType.Bit).Value = RoboParcial.instalacion;
                sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.VarChar, 10).Value = RoboParcial.id_moneda;
                sqlComando.Parameters.Add("@precio_cotizado", System.Data.SqlDbType.Float).Value = RoboParcial.precio_cotizado;
                sqlComando.Parameters.Add("@id_tipo_descuento", System.Data.SqlDbType.VarChar, 10).Value = RoboParcial.id_tipo_descuento;
                sqlComando.Parameters.Add("@descuento", System.Data.SqlDbType.Float).Value = RoboParcial.descuento;
                sqlComando.Parameters.Add("@precio_final", System.Data.SqlDbType.Float).Value = RoboParcial.precio_final;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RoboParcial.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RoboParcial.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RoboParcial.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RoboParcial.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RoboParcial.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RoboParcial.monto_final;
                sqlComando.Parameters.Add("@recepcion", System.Data.SqlDbType.Bit).Value = RoboParcial.recepcion;
                sqlComando.Parameters.Add("@dias_entrega", System.Data.SqlDbType.Int).Value = RoboParcial.dias_entrega;
                sqlComando.Parameters.Add("@fecha_envio_efectivo", System.Data.SqlDbType.DateTime).Value = RoboParcial.fecha_envio_efectivo;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = RoboParcial.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RoboParcial.id_estado;
                sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = RoboParcial.tipo_cambio;
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
        public static bool RoboParcialBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_robo_parcial] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
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
        //ROBO PARCIAL SUMATORIA
        public class TipoRoboParcialSumatoria
        {
            public int id_flujo;
            public int id_cotizacion;
            public short id_tipo_item;
            public string proveedor;
            public double monto_orden;
            public string id_tipo_descuento_orden;
            public double descuento_proveedor;
            public double deducible;
            public double monto_final;
            public string numero_orden;
            public short id_estado;
            public TipoRoboParcialSumatoria()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_tipo_item = 0;
                this.proveedor = "";
                this.monto_orden = 0.0;
                this.id_tipo_descuento_orden = "";
                this.descuento_proveedor = 0;
                this.deducible = 0;
                this.monto_final = 0;
                this.numero_orden = "";
                this.id_estado = 0;
            }
        }
        public static bool RoboParcialSumatoriaRegistrar(TipoRoboParcialSumatoria RoboParcialSumatoria)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_robo_parcial_sumatoria] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado])" +
              " VALUES (@id_flujo,@id_cotizacion,@id_tipo_item,@proveedor,@monto_orden,@id_tipo_descuento_orden,@descuento_proveedor,@deducible,@monto_final,@numero_orden,@id_estado)";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RoboParcialSumatoria.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RoboParcialSumatoria.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RoboParcialSumatoria.id_tipo_item;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RoboParcialSumatoria.proveedor;
                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RoboParcialSumatoria.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.monto_final;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 30).Value = RoboParcialSumatoria.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RoboParcialSumatoria.id_estado;
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
        public class TipoRoboParcialSumatoriaTraer
        {
            public bool Correcto;
            public string Mensaje;
            //ajuste del nombre del objeto a devolver de RCVehicularSumatoria a RoboParcialSumatoria
            public List<TipoRoboParcialSumatoria> RoboParcialSumatoria = new List<TipoRoboParcialSumatoria>();
            public System.Data.DataSet dsRoboParcialSumatoria = new System.Data.DataSet();
        }
        public static TipoRoboParcialSumatoriaTraer RoboParcialSumatoriaTraer(int Flujo, int Cotizacion, short Tipo_Item)
        {
            TipoRoboParcialSumatoriaTraer objRespuesta = new TipoRoboParcialSumatoriaTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado] FROM [dbo].[cotizacion_robo_parcial_sumatoria] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_tipo_item = @id_tipo_item";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoRoboParcialSumatoria tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoRoboParcialSumatoria();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.id_cotizacion = Cotizacion;
                    tdpFila.id_tipo_item = Tipo_Item;
                    //[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final]
                    if (sqlDatos["proveedor"] != DBNull.Value) tdpFila.proveedor = Convert.ToString(sqlDatos["proveedor"]);
                    if (sqlDatos["monto_orden"] != DBNull.Value) tdpFila.monto_orden = Convert.ToDouble(sqlDatos["monto_orden"]);
                    if (sqlDatos["id_tipo_descuento_orden"] != DBNull.Value) tdpFila.id_tipo_descuento_orden = Convert.ToString(sqlDatos["id_tipo_descuento_orden"]);
                    if (sqlDatos["descuento_proveedor"] != DBNull.Value) tdpFila.descuento_proveedor = Convert.ToDouble(sqlDatos["descuento_proveedor"]);
                    if (sqlDatos["deducible"] != DBNull.Value) tdpFila.deducible = Convert.ToDouble(sqlDatos["deducible"]);
                    if (sqlDatos["monto_final"] != DBNull.Value) tdpFila.monto_final = Convert.ToDouble(sqlDatos["monto_final"]);
                    if (sqlDatos["numero_orden"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["numero_orden"]);
                    if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
                    objRespuesta.RoboParcialSumatoria.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
                sqlAdaptador.Fill(objRespuesta.dsRoboParcialSumatoria);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer los Robos Parciales sumados de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return objRespuesta;
        }
        public static bool RoboParcialSumatoriaModificar(TipoRoboParcialSumatoria RoboParcialSumatoria)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_robo_parcial_sumatoria] SET [monto_orden] = @monto_orden,[id_tipo_descuento_orden] = @id_tipo_descuento_orden,[descuento_proveedor] = @descuento_proveedor,[deducible] = @deducible, [monto_final] = @monto_final,[numero_orden] = @numero_orden,[id_estado]=@id_estado " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_tipo_item] = @id_tipo_item AND [proveedor] = @proveedor";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = RoboParcialSumatoria.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = RoboParcialSumatoria.id_cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = RoboParcialSumatoria.id_tipo_item;
                sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = RoboParcialSumatoria.proveedor;

                sqlComando.Parameters.Add("@monto_orden", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.monto_orden;
                sqlComando.Parameters.Add("@id_tipo_descuento_orden", System.Data.SqlDbType.VarChar, 10).Value = RoboParcialSumatoria.id_tipo_descuento_orden;
                sqlComando.Parameters.Add("@descuento_proveedor", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.descuento_proveedor;
                sqlComando.Parameters.Add("@deducible", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.deducible;
                sqlComando.Parameters.Add("@monto_final", System.Data.SqlDbType.Float).Value = RoboParcialSumatoria.monto_final;
                sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 30).Value = RoboParcialSumatoria.numero_orden;
                sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = RoboParcialSumatoria.id_estado;
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
        public static bool RoboParcialSumatoriaBorrar(int Flujo, int Cotizacion, short Tipo_Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_robo_parcial_sumatoria] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_tipo_item] = @id_tipo_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
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
        public static bool RoboParcialSumatoriaGenerar(int Flujo, int Cotizacion, short Tipo_Item)
        {
            bool blnRespuesta = false;
            blnRespuesta = RoboParcialSumatoriaBorrar(Flujo, Cotizacion, Tipo_Item);
            if (!blnRespuesta) return false;
            string strComando = "INSERT INTO [dbo].[cotizacion_robo_parcial_sumatoria] " +
              "([id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],[monto_orden],[id_tipo_descuento_orden],[descuento_proveedor],[deducible],[monto_final],[numero_orden],[id_estado]) " +
              "SELECT [id_flujo],[id_cotizacion],[id_tipo_item],[proveedor],SUM(precio_final),'Fijo',0.0,0.0,0.0,'',0 FROM [dbo].[cotizacion_robo_parcial] " +
              "WHERE [id_flujo] = @id_flujo and [id_cotizacion]= @id_cotizacion and [id_tipo_item] = @id_tipo_item GROUP BY [id_flujo],[id_cotizacion],[id_tipo_item],[proveedor]";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Tipo_Item;
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
        //PERDIDA TOTAL DAÑOS PROPIOS
        public class TipoPerdidaTotalPropio
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public bool condiciones_especiales;
            public string version;
            public string serie;
            public string caja;
            public string combustible;
            public string cilindrada;
            public bool techo_solar;
            public bool asientos_cuero;
            public bool aros_magnesio;
            public string observaciones_vehiculo;
            public string duenio_nombres_1;
            public string duenio_documento_1;
            public double duenio_monto_1;
            public string duenio_descripcion_1;
            public string duenio_nombres_2;
            public string duenio_documento_2;
            public double duenio_monto_2;
            public string duenio_descripcion_2;
            public string duenio_nombres_3;
            public string duenio_documento_3;
            public double duenio_monto_3;
            public string duenio_descripcion_3;
            public string duenio_nombres_4;
            public string duenio_documento_4;
            public double duenio_monto_4;
            public string duenio_descripcion_4;
            public string duenio_nombres_5;
            public string duenio_documento_5;
            public double duenio_monto_5;
            public string duenio_descripcion_5;
            public bool referencia_usada_1;
            public string referencia_medios_1;
            public string referencia_descripcion_1;
            public double referencia_monto_1;
            public bool referencia_usada_2;
            public string referencia_medios_2;
            public string referencia_descripcion_2;
            public double referencia_monto_2;
            public bool referencia_usada_3;
            public string referencia_medios_3;
            public string referencia_descripcion_3;
            public double referencia_monto_3;
            public bool referencia_usada_4;
            public string referencia_medios_4;
            public string referencia_descripcion_4;
            public double referencia_monto_4;
            public bool referencia_usada_5;
            public string referencia_medios_5;
            public string referencia_descripcion_5;
            public double referencia_monto_5;
            public bool referencia_usada_6;
            public string referencia_medios_6;
            public string referencia_descripcion_6;
            public double referencia_monto_6;
            public TipoPerdidaTotalPropio()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.condiciones_especiales = false;
                this.version = "";
                this.serie = "";
                this.caja = "";
                this.combustible = "";
                this.cilindrada = "";
                this.techo_solar = false;
                this.asientos_cuero = false;
                this.aros_magnesio = false;
                this.observaciones_vehiculo = "";
                this.duenio_nombres_1 = "";
                this.duenio_documento_1 = "";
                this.duenio_monto_1 = 0.0;
                this.duenio_descripcion_1 = "";
                this.duenio_nombres_2 = "";
                this.duenio_documento_2 = "";
                this.duenio_monto_2 = 0.0;
                this.duenio_descripcion_2 = "";
                this.duenio_nombres_3 = "";
                this.duenio_documento_3 = "";
                this.duenio_monto_3 = 0.0;
                this.duenio_descripcion_3 = "";
                this.duenio_nombres_4 = "";
                this.duenio_documento_4 = "";
                this.duenio_monto_4 = 0.0;
                this.duenio_descripcion_4 = "";
                this.duenio_nombres_5 = "";
                this.duenio_documento_5 = "";
                this.duenio_monto_5 = 0.0;
                this.duenio_descripcion_5 = "";
                this.referencia_usada_1 = false;
                this.referencia_medios_1 = "";
                this.referencia_descripcion_1 = "";
                this.referencia_monto_1 = 0.0;
                this.referencia_usada_2 = false;
                this.referencia_medios_2 = "";
                this.referencia_descripcion_2 = "";
                this.referencia_monto_2 = 0.0;
                this.referencia_usada_3 = false;
                this.referencia_medios_3 = "";
                this.referencia_descripcion_3 = "";
                this.referencia_monto_3 = 0.0;
                this.referencia_usada_4 = false;
                this.referencia_medios_4 = "";
                this.referencia_descripcion_4 = "";
                this.referencia_monto_4 = 0.0;
                this.referencia_usada_5 = false;
                this.referencia_medios_5 = "";
                this.referencia_descripcion_5 = "";
                this.referencia_monto_5 = 0.0;
                this.referencia_usada_6 = false;
                this.referencia_medios_6 = "";
                this.referencia_descripcion_6 = "";
                this.referencia_monto_6 = 0.0;
            }
        }
        public static bool PerdidaTotalPropioRegistrar(TipoPerdidaTotalPropio PerdidaTotalPropio)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_perdida_total_propio] " +
              "[id_flujo],[id_cotizacion],[condiciones_especiales],[version],[serie],[caja],[combustible],[cilindrada],[techo_solar],[asientos_cuero],[aros_magnesio],[observaciones_vehiculo],[duenio_nombres_1],[duenio_documento_1],[duenio_monto_1],[duenio_descripcion_1],[duenio_nombres_2],[duenio_documento_2],[duenio_monto_2],[duenio_descripcion_2],[duenio_nombres_3],[duenio_documento_3],[duenio_monto_3],[duenio_descripcion_3],[duenio_nombres_4],[duenio_documento_4],[duenio_monto_4],[duenio_descripcion_4],[duenio_nombres_5]," +
              "[duenio_documento_5],[duenio_monto_5],[duenio_descripcion_5],[referencia_usada_1],[referencia_medios_1],[referencia_descripcion_1],[referencia_monto_1],[referencia_usada_2],[referencia_medios_2],[referencia_descripcion_2],[referencia_monto_2],[referencia_usada_3],[referencia_medios_3],[referencia_descripcion_3],[referencia_monto_3],[referencia_usada_4],[referencia_medios_4],[referencia_descripcion_4],[referencia_monto_4],[referencia_usada_5],[referencia_medios_5],[referencia_descripcion_5],[referencia_monto_5],[referencia_usada_6],[referencia_medios_6],[referencia_descripcion_6],[referencia_monto_6]) " +
              " VALUES (@id_flujo,@id_cotizacion,@condiciones_especiales,@version,@serie,@caja,@combustible,@cilindrada,@techo_solar,@asientos_cuero,@aros_magnesio,@observaciones_vehiculo,@duenio_nombres_1,@duenio_documento_1,@duenio_monto_1,@duenio_descripcion_1,@duenio_nombres_2,@duenio_documento_2,@duenio_monto_2,@duenio_descripcion_2,@duenio_nombres_3,@duenio_documento_3,@duenio_monto_3,@duenio_descripcion_3,@duenio_nombres_4,@duenio_documento_4,@duenio_monto_4,@duenio_descripcion_4,@duenio_nombres_5," +
              "@duenio_documento_5,@duenio_monto_5,@duenio_descripcion_5,@referencia_usada_1,@referencia_medios_1,@referencia_descripcion_1,@referencia_monto_1,@referencia_usada_2,@referencia_medios_2,@referencia_descripcion_2,@referencia_monto_2,@referencia_usada_3,@referencia_medios_3,@referencia_descripcion_3,@referencia_monto_3,@referencia_usada_4,@referencia_medios_4,@referencia_descripcion_4,@referencia_monto_4,@referencia_usada_5,@referencia_medios_5,@referencia_descripcion_5,@referencia_monto_5,@referencia_usada_6,@referencia_medios_6,@referencia_descripcion_6,@referencia_monto_6)";

            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = PerdidaTotalPropio.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = PerdidaTotalPropio.id_cotizacion;
                sqlComando.Parameters.Add("@condiciones_especiales", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.condiciones_especiales;
                sqlComando.Parameters.Add("@version", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.version;
                sqlComando.Parameters.Add("@serie", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.serie;
                sqlComando.Parameters.Add("@caja", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.caja;
                sqlComando.Parameters.Add("@combustible", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.combustible;
                sqlComando.Parameters.Add("@cilindrada", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.cilindrada;
                sqlComando.Parameters.Add("@techo_solar", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.techo_solar;
                sqlComando.Parameters.Add("@asientos_cuero", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.asientos_cuero;
                sqlComando.Parameters.Add("@aros_magnesio", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.aros_magnesio;
                sqlComando.Parameters.Add("@observaciones_vehiculo", System.Data.SqlDbType.VarChar, 350).Value = PerdidaTotalPropio.observaciones_vehiculo;
                sqlComando.Parameters.Add("@duenio_nombres_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_1;
                sqlComando.Parameters.Add("@duenio_documento_1", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_1;
                sqlComando.Parameters.Add("@duenio_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_1;
                sqlComando.Parameters.Add("@duenio_descripcion_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_1;
                sqlComando.Parameters.Add("@duenio_nombres_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_2;
                sqlComando.Parameters.Add("@duenio_documento_2", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_2;
                sqlComando.Parameters.Add("@duenio_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_2;
                sqlComando.Parameters.Add("@duenio_descripcion_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_2;
                sqlComando.Parameters.Add("@duenio_nombres_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_3;
                sqlComando.Parameters.Add("@duenio_documento_3", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_3;
                sqlComando.Parameters.Add("@duenio_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_3;
                sqlComando.Parameters.Add("@duenio_descripcion_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_3;
                sqlComando.Parameters.Add("@duenio_nombres_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_4;
                sqlComando.Parameters.Add("@duenio_documento_4", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_4;
                sqlComando.Parameters.Add("@duenio_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_4;
                sqlComando.Parameters.Add("@duenio_descripcion_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_4;
                sqlComando.Parameters.Add("@duenio_nombres_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_5;
                sqlComando.Parameters.Add("@duenio_documento_5", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_5;
                sqlComando.Parameters.Add("@duenio_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_5;
                sqlComando.Parameters.Add("@duenio_descripcion_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_5;
                sqlComando.Parameters.Add("@referencia_usada_1", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_1;
                sqlComando.Parameters.Add("@referencia_medios_1", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_1;
                sqlComando.Parameters.Add("@referencia_descripcion_1", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_1;
                sqlComando.Parameters.Add("@referencia_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_1;
                sqlComando.Parameters.Add("@referencia_usada_2", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_2;
                sqlComando.Parameters.Add("@referencia_medios_2", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_2;
                sqlComando.Parameters.Add("@referencia_descripcion_2", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_2;
                sqlComando.Parameters.Add("@referencia_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_2;
                sqlComando.Parameters.Add("@referencia_usada_3", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_3;
                sqlComando.Parameters.Add("@referencia_medios_3", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_3;
                sqlComando.Parameters.Add("@referencia_descripcion_3", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_3;
                sqlComando.Parameters.Add("@referencia_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_3;
                sqlComando.Parameters.Add("@referencia_usada_4", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_4;
                sqlComando.Parameters.Add("@referencia_medios_4", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_4;
                sqlComando.Parameters.Add("@referencia_descripcion_4", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_4;
                sqlComando.Parameters.Add("@referencia_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_4;
                sqlComando.Parameters.Add("@referencia_usada_5", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_5;
                sqlComando.Parameters.Add("@referencia_medios_5", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_5;
                sqlComando.Parameters.Add("@referencia_descripcion_5", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_5;
                sqlComando.Parameters.Add("@referencia_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_5;
                sqlComando.Parameters.Add("@referencia_usada_6", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_6;
                sqlComando.Parameters.Add("@referencia_medios_6", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_6;
                sqlComando.Parameters.Add("@referencia_descripcion_6", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_6;
                sqlComando.Parameters.Add("@referencia_monto_6", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_6;
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
        public class TipoPeridaTotalPropioTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoPerdidaTotalPropio> PerdidasTotalesPropios = new List<TipoPerdidaTotalPropio>();
            public System.Data.DataSet dsPerdidasTotalesPropios = new System.Data.DataSet();
        }
        public static TipoPeridaTotalPropioTraer PerdidaTotalPropioTraer(int Flujo, int Cotizacion)
        {
            TipoPeridaTotalPropioTraer objRespuesta = new TipoPeridaTotalPropioTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_item],[condiciones_especiales],[version],[serie],[caja],[combustible],[cilindrada],[techo_solar],[asientos_cuero],[aros_magnesio],[observaciones_vehiculo],[duenio_nombres_1],[duenio_documento_1],[duenio_monto_1],[duenio_descripcion_1],[duenio_nombres_2],[duenio_documento_2],[duenio_monto_2],[duenio_descripcion_2],[duenio_nombres_3],[duenio_documento_3],[duenio_monto_3],[duenio_descripcion_3],[duenio_nombres_4],[duenio_documento_4],[duenio_monto_4],[duenio_descripcion_4],[duenio_nombres_5],[duenio_documento_5],[duenio_monto_5],[duenio_descripcion_5],[referencia_usada_1],[referencia_medios_1],[referencia_descripcion_1],[referencia_monto_1],[referencia_usada_2],[referencia_medios_2],[referencia_descripcion_2],[referencia_monto_2],[referencia_usada_3],[referencia_medios_3],[referencia_descripcion_3],[referencia_monto_3],[referencia_usada_4],[referencia_medios_4],[referencia_descripcion_4],[referencia_monto_4],[referencia_usada_5],[referencia_medios_5],[referencia_descripcion_5],[referencia_monto_5],[referencia_usada_6],[referencia_medios_6],[referencia_descripcion_6],[referencia_monto_6] " +
              "FROM [dbo].[cotizacion_perdida_total_propio] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoPerdidaTotalPropio tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoPerdidaTotalPropio();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.id_cotizacion = Cotizacion;
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["condiciones_especiales"] != DBNull.Value) tdpFila.condiciones_especiales = Convert.ToBoolean(sqlDatos["condiciones_especiales"]);
                    if (sqlDatos["version"] != DBNull.Value) tdpFila.version = Convert.ToString(sqlDatos["version"]);
                    if (sqlDatos["serie"] != DBNull.Value) tdpFila.serie = Convert.ToString(sqlDatos["serie"]);
                    if (sqlDatos["caja"] != DBNull.Value) tdpFila.caja = Convert.ToString(sqlDatos["caja"]);
                    if (sqlDatos["combustible"] != DBNull.Value) tdpFila.combustible = Convert.ToString(sqlDatos["combustible"]);
                    if (sqlDatos["cilindrada"] != DBNull.Value) tdpFila.cilindrada = Convert.ToString(sqlDatos["cilindrada"]);
                    if (sqlDatos["techo_solar"] != DBNull.Value) tdpFila.techo_solar = Convert.ToBoolean(sqlDatos["techo_solar"]);
                    if (sqlDatos["asientos_cuero"] != DBNull.Value) tdpFila.asientos_cuero = Convert.ToBoolean(sqlDatos["asientos_cuero"]);
                    if (sqlDatos["aros_magnesio"] != DBNull.Value) tdpFila.aros_magnesio = Convert.ToBoolean(sqlDatos["aros_magnesio"]);
                    if (sqlDatos["observaciones_vehiculo"] != DBNull.Value) tdpFila.observaciones_vehiculo = Convert.ToString(sqlDatos["observaciones_vehiculo"]);
                    if (sqlDatos["duenio_nombres_1"] != DBNull.Value) tdpFila.duenio_nombres_1 = Convert.ToString(sqlDatos["duenio_nombres_1"]);
                    if (sqlDatos["duenio_documento_1"] != DBNull.Value) tdpFila.duenio_documento_1 = Convert.ToString(sqlDatos["duenio_documento_1"]);
                    if (sqlDatos["duenio_monto_1"] != DBNull.Value) tdpFila.duenio_monto_1 = Convert.ToDouble(sqlDatos["duenio_monto_1"]);
                    if (sqlDatos["duenio_descripcion_1"] != DBNull.Value) tdpFila.duenio_descripcion_1 = Convert.ToString(sqlDatos["duenio_descripcion_1"]);
                    if (sqlDatos["duenio_nombres_2"] != DBNull.Value) tdpFila.duenio_nombres_2 = Convert.ToString(sqlDatos["duenio_nombres_2"]);
                    if (sqlDatos["duenio_documento_2"] != DBNull.Value) tdpFila.duenio_documento_2 = Convert.ToString(sqlDatos["duenio_documento_2"]);
                    if (sqlDatos["duenio_monto_2"] != DBNull.Value) tdpFila.duenio_monto_2 = Convert.ToDouble(sqlDatos["duenio_monto_2"]);
                    if (sqlDatos["duenio_descripcion_2"] != DBNull.Value) tdpFila.duenio_descripcion_2 = Convert.ToString(sqlDatos["duenio_descripcion_2"]);
                    if (sqlDatos["duenio_nombres_3"] != DBNull.Value) tdpFila.duenio_nombres_3 = Convert.ToString(sqlDatos["duenio_nombres_3"]);
                    if (sqlDatos["duenio_documento_3"] != DBNull.Value) tdpFila.duenio_documento_3 = Convert.ToString(sqlDatos["duenio_documento_3"]);
                    if (sqlDatos["duenio_monto_3"] != DBNull.Value) tdpFila.duenio_monto_3 = Convert.ToDouble(sqlDatos["duenio_monto_3"]);
                    if (sqlDatos["duenio_descripcion_3"] != DBNull.Value) tdpFila.duenio_descripcion_3 = Convert.ToString(sqlDatos["duenio_descripcion_3"]);
                    if (sqlDatos["duenio_nombres_4"] != DBNull.Value) tdpFila.duenio_nombres_4 = Convert.ToString(sqlDatos["duenio_nombres_4"]);
                    if (sqlDatos["duenio_documento_4"] != DBNull.Value) tdpFila.duenio_documento_4 = Convert.ToString(sqlDatos["duenio_documento_4"]);
                    if (sqlDatos["duenio_monto_4"] != DBNull.Value) tdpFila.duenio_monto_4 = Convert.ToDouble(sqlDatos["duenio_monto_4"]);
                    if (sqlDatos["duenio_descripcion_4"] != DBNull.Value) tdpFila.duenio_descripcion_4 = Convert.ToString(sqlDatos["duenio_descripcion_4"]);
                    if (sqlDatos["duenio_nombres_5"] != DBNull.Value) tdpFila.duenio_nombres_5 = Convert.ToString(sqlDatos["duenio_nombres_5"]);
                    if (sqlDatos["duenio_documento_5"] != DBNull.Value) tdpFila.duenio_documento_5 = Convert.ToString(sqlDatos["duenio_documento_5"]);
                    if (sqlDatos["duenio_monto_5"] != DBNull.Value) tdpFila.duenio_monto_5 = Convert.ToDouble(sqlDatos["duenio_monto_5"]);
                    if (sqlDatos["duenio_descripcion_5"] != DBNull.Value) tdpFila.duenio_descripcion_5 = Convert.ToString(sqlDatos["duenio_descripcion_5"]);
                    if (sqlDatos["referencia_usada_1"] != DBNull.Value) tdpFila.referencia_usada_1 = Convert.ToBoolean(sqlDatos["referencia_usada_1"]);
                    if (sqlDatos["referencia_medios_1"] != DBNull.Value) tdpFila.referencia_medios_1 = Convert.ToString(sqlDatos["referencia_medios_1"]);
                    if (sqlDatos["referencia_descripcion_1"] != DBNull.Value) tdpFila.referencia_descripcion_1 = Convert.ToString(sqlDatos["referencia_descripcion_1"]);
                    if (sqlDatos["referencia_monto_1"] != DBNull.Value) tdpFila.referencia_monto_1 = Convert.ToDouble(sqlDatos["referencia_monto_1"]);
                    if (sqlDatos["referencia_usada_2"] != DBNull.Value) tdpFila.referencia_usada_2 = Convert.ToBoolean(sqlDatos["referencia_usada_2"]);
                    if (sqlDatos["referencia_medios_2"] != DBNull.Value) tdpFila.referencia_medios_2 = Convert.ToString(sqlDatos["referencia_medios_2"]);
                    if (sqlDatos["referencia_descripcion_2"] != DBNull.Value) tdpFila.referencia_descripcion_2 = Convert.ToString(sqlDatos["referencia_descripcion_2"]);
                    if (sqlDatos["referencia_monto_2"] != DBNull.Value) tdpFila.referencia_monto_2 = Convert.ToDouble(sqlDatos["referencia_monto_2"]);
                    if (sqlDatos["referencia_usada_3"] != DBNull.Value) tdpFila.referencia_usada_3 = Convert.ToBoolean(sqlDatos["referencia_usada_3"]);
                    if (sqlDatos["referencia_medios_3"] != DBNull.Value) tdpFila.referencia_medios_3 = Convert.ToString(sqlDatos["referencia_medios_3"]);
                    if (sqlDatos["referencia_descripcion_3"] != DBNull.Value) tdpFila.referencia_descripcion_3 = Convert.ToString(sqlDatos["referencia_descripcion_3"]);
                    if (sqlDatos["referencia_monto_3"] != DBNull.Value) tdpFila.referencia_monto_3 = Convert.ToDouble(sqlDatos["referencia_monto_3"]);
                    if (sqlDatos["referencia_usada_4"] != DBNull.Value) tdpFila.referencia_usada_4 = Convert.ToBoolean(sqlDatos["referencia_usada_4"]);
                    if (sqlDatos["referencia_medios_4"] != DBNull.Value) tdpFila.referencia_medios_4 = Convert.ToString(sqlDatos["referencia_medios_4"]);
                    if (sqlDatos["referencia_descripcion_4"] != DBNull.Value) tdpFila.referencia_descripcion_4 = Convert.ToString(sqlDatos["referencia_descripcion_4"]);
                    if (sqlDatos["referencia_monto_4"] != DBNull.Value) tdpFila.referencia_monto_4 = Convert.ToDouble(sqlDatos["referencia_monto_4"]);
                    if (sqlDatos["referencia_usada_5"] != DBNull.Value) tdpFila.referencia_usada_5 = Convert.ToBoolean(sqlDatos["referencia_usada_5"]);
                    if (sqlDatos["referencia_medios_5"] != DBNull.Value) tdpFila.referencia_medios_5 = Convert.ToString(sqlDatos["referencia_medios_5"]);
                    if (sqlDatos["referencia_descripcion_5"] != DBNull.Value) tdpFila.referencia_descripcion_5 = Convert.ToString(sqlDatos["referencia_descripcion_5"]);
                    if (sqlDatos["referencia_monto_5"] != DBNull.Value) tdpFila.referencia_monto_5 = Convert.ToDouble(sqlDatos["referencia_monto_5"]);
                    if (sqlDatos["referencia_usada_6"] != DBNull.Value) tdpFila.referencia_usada_6 = Convert.ToBoolean(sqlDatos["referencia_usada_6"]);
                    if (sqlDatos["referencia_medios_6"] != DBNull.Value) tdpFila.referencia_medios_6 = Convert.ToString(sqlDatos["referencia_medios_6"]);
                    if (sqlDatos["referencia_descripcion_6"] != DBNull.Value) tdpFila.referencia_descripcion_6 = Convert.ToString(sqlDatos["referencia_descripcion_6"]);
                    if (sqlDatos["referencia_monto_6"] != DBNull.Value) tdpFila.referencia_monto_6 = Convert.ToDouble(sqlDatos["referencia_monto_6"]);
                    objRespuesta.PerdidasTotalesPropios.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.Fill(objRespuesta.dsPerdidasTotalesPropios);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer las perdidas totales por daños propios de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }
        public static bool PerdidaTotalPropioModificar(TipoPerdidaTotalPropio PerdidaTotalPropio)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_perdida_total_propio] " +
              "SET [condiciones_especiales] = @condiciones_especiales,[version] = @version,[serie] = @serie,[caja],[combustible] = @combustible, [cilindrada] = @cilindrada ,[techo_solar] = @techo_solar,[asientos_cuero] = @asientos_cuero,[aros_magnesio] = @aros_magnesio, [observaciones_vehiculo] = @observaciones_vehiculo," +
              "[duenio_nombres_1] = @duenio_nombres_1,[duenio_documento_1] = @duenio_documento_1,[duenio_monto_1] = @duenio_monto_1,[duenio_descripcion_1] = @duenio_descripcion_1," +
              "[duenio_nombres_2] = @duenio_nombres_2,[duenio_documento_2] = @duenio_documento_2,[duenio_monto_2] = @duenio_monto_2,[duenio_descripcion_2] = @duenio_descripcion_2," +
              "[duenio_nombres_3] = @duenio_nombres_3,[duenio_documento_3] = @duenio_documento_3,[duenio_monto_3] = @duenio_monto_3,[duenio_descripcion_3] = @duenio_descripcion_3," +
              "[duenio_nombres_4] = @duenio_nombres_4,[duenio_documento_4] = @duenio_documento_4,[duenio_monto_4] = @duenio_monto_4,[duenio_descripcion_4] = @duenio_descripcion_4," +
              "[duenio_nombres_5] = @duenio_nombres_5,[duenio_documento_5] = @duenio_documento_5,[duenio_monto_5] = @duenio_monto_5,[duenio_descripcion_5] = @duenio_descripcion_5," +
              "[referencia_usada_1] = @referencia_usada_1,[referencia_medios_1] = @referencia_medios_1,[referencia_descripcion_1] = @referencia_descripcion_1,[referencia_monto_1] = @referencia_monto_1," +
              "[referencia_usada_2] = @referencia_usada_2,[referencia_medios_2] = @referencia_medios_2,[referencia_descripcion_2] = @referencia_descripcion_2,[referencia_monto_2] = @referencia_monto_2," +
              "[referencia_usada_3] = @referencia_usada_3,[referencia_medios_3] = @referencia_medios_3,[referencia_descripcion_3] = @referencia_descripcion_3,[referencia_monto_3] = @referencia_monto_3," +
              "[referencia_usada_4] = @referencia_usada_4,[referencia_medios_4] = @referencia_medios_4,[referencia_descripcion_4] = @referencia_descripcion_4,[referencia_monto_4] = @referencia_monto_4," +
              "[referencia_usada_5] = @referencia_usada_5,[referencia_medios_5] = @referencia_medios_5,[referencia_descripcion_5] = @referencia_descripcion_5,[referencia_monto_5] = @referencia_monto_5," +
              "[referencia_usada_6] = @referencia_usada_6,[referencia_medios_6] = @referencia_medios_6,[referencia_descripcion_6] = @referencia_descripcion_6,[referencia_monto_6] = @referencia_monto_6 " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_item] = @id_item";

            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = PerdidaTotalPropio.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = PerdidaTotalPropio.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = PerdidaTotalPropio.id_item;

                sqlComando.Parameters.Add("@condiciones_especiales", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.condiciones_especiales;
                sqlComando.Parameters.Add("@version", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.version;
                sqlComando.Parameters.Add("@serie", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.serie;
                sqlComando.Parameters.Add("@caja", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.caja;
                sqlComando.Parameters.Add("@combustible", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.combustible;
                sqlComando.Parameters.Add("@cilindrada", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.cilindrada;
                sqlComando.Parameters.Add("@techo_solar", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.techo_solar;
                sqlComando.Parameters.Add("@asientos_cuero", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.asientos_cuero;
                sqlComando.Parameters.Add("@aros_magnesio", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.aros_magnesio;
                sqlComando.Parameters.Add("@observaciones_vehiculo", System.Data.SqlDbType.VarChar, 350).Value = PerdidaTotalPropio.observaciones_vehiculo;
                sqlComando.Parameters.Add("@duenio_nombres_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_1;
                sqlComando.Parameters.Add("@duenio_documento_1", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_1;
                sqlComando.Parameters.Add("@duenio_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_1;
                sqlComando.Parameters.Add("@duenio_descripcion_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_1;
                sqlComando.Parameters.Add("@duenio_nombres_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_2;
                sqlComando.Parameters.Add("@duenio_documento_2", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_2;
                sqlComando.Parameters.Add("@duenio_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_2;
                sqlComando.Parameters.Add("@duenio_descripcion_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_2;
                sqlComando.Parameters.Add("@duenio_nombres_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_3;
                sqlComando.Parameters.Add("@duenio_documento_3", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_3;
                sqlComando.Parameters.Add("@duenio_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_3;
                sqlComando.Parameters.Add("@duenio_descripcion_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_3;
                sqlComando.Parameters.Add("@duenio_nombres_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_4;
                sqlComando.Parameters.Add("@duenio_documento_4", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_4;
                sqlComando.Parameters.Add("@duenio_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_4;
                sqlComando.Parameters.Add("@duenio_descripcion_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_4;
                sqlComando.Parameters.Add("@duenio_nombres_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_nombres_5;
                sqlComando.Parameters.Add("@duenio_documento_5", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalPropio.duenio_documento_5;
                sqlComando.Parameters.Add("@duenio_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.duenio_monto_5;
                sqlComando.Parameters.Add("@duenio_descripcion_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalPropio.duenio_descripcion_5;
                sqlComando.Parameters.Add("@referencia_usada_1", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_1;
                sqlComando.Parameters.Add("@referencia_medios_1", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_1;
                sqlComando.Parameters.Add("@referencia_descripcion_1", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_1;
                sqlComando.Parameters.Add("@referencia_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_1;
                sqlComando.Parameters.Add("@referencia_usada_2", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_2;
                sqlComando.Parameters.Add("@referencia_medios_2", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_2;
                sqlComando.Parameters.Add("@referencia_descripcion_2", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_2;
                sqlComando.Parameters.Add("@referencia_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_2;
                sqlComando.Parameters.Add("@referencia_usada_3", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_3;
                sqlComando.Parameters.Add("@referencia_medios_3", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_3;
                sqlComando.Parameters.Add("@referencia_descripcion_3", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_3;
                sqlComando.Parameters.Add("@referencia_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_3;
                sqlComando.Parameters.Add("@referencia_usada_4", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_4;
                sqlComando.Parameters.Add("@referencia_medios_4", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_4;
                sqlComando.Parameters.Add("@referencia_descripcion_4", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_4;
                sqlComando.Parameters.Add("@referencia_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_4;
                sqlComando.Parameters.Add("@referencia_usada_5", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_5;
                sqlComando.Parameters.Add("@referencia_medios_5", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_5;
                sqlComando.Parameters.Add("@referencia_descripcion_5", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_5;
                sqlComando.Parameters.Add("@referencia_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_5;
                sqlComando.Parameters.Add("@referencia_usada_6", System.Data.SqlDbType.Bit).Value = PerdidaTotalPropio.referencia_usada_6;
                sqlComando.Parameters.Add("@referencia_medios_6", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalPropio.referencia_medios_6;
                sqlComando.Parameters.Add("@referencia_descripcion_6", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalPropio.referencia_descripcion_6;
                sqlComando.Parameters.Add("@referencia_monto_6", System.Data.SqlDbType.Float).Value = PerdidaTotalPropio.referencia_monto_6;
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
        public static bool PerdidaTotalPropioBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_perdida_total_propio] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
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
        //PERDIDA TOTAL ROBO
        public class TipoPerdidaTotalRobo
        {
            public int id_flujo;
            public int id_cotizacion;
            public long id_item;
            public bool condiciones_especiales;
            public string version;
            public string serie;
            public string caja;
            public string combustible;
            public string cilindrada;
            public bool techo_solar;
            public bool asientos_cuero;
            public bool aros_magnesio;
            public string observaciones_vehiculo;
            public string duenio_nombres_1;
            public string duenio_documento_1;
            public double duenio_monto_1;
            public string duenio_descripcion_1;
            public string duenio_nombres_2;
            public string duenio_documento_2;
            public double duenio_monto_2;
            public string duenio_descripcion_2;
            public string duenio_nombres_3;
            public string duenio_documento_3;
            public double duenio_monto_3;
            public string duenio_descripcion_3;
            public string duenio_nombres_4;
            public string duenio_documento_4;
            public double duenio_monto_4;
            public string duenio_descripcion_4;
            public string duenio_nombres_5;
            public string duenio_documento_5;
            public double duenio_monto_5;
            public string duenio_descripcion_5;
            public bool referencia_usada_1;
            public string referencia_medios_1;
            public string referencia_descripcion_1;
            public double referencia_monto_1;
            public bool referencia_usada_2;
            public string referencia_medios_2;
            public string referencia_descripcion_2;
            public double referencia_monto_2;
            public bool referencia_usada_3;
            public string referencia_medios_3;
            public string referencia_descripcion_3;
            public double referencia_monto_3;
            public bool referencia_usada_4;
            public string referencia_medios_4;
            public string referencia_descripcion_4;
            public double referencia_monto_4;
            public bool referencia_usada_5;
            public string referencia_medios_5;
            public string referencia_descripcion_5;
            public double referencia_monto_5;
            public bool referencia_usada_6;
            public string referencia_medios_6;
            public string referencia_descripcion_6;
            public double referencia_monto_6;
            public TipoPerdidaTotalRobo()
            {
                this.id_flujo = 0;
                this.id_cotizacion = 0;
                this.id_item = 0;
                this.condiciones_especiales = false;
                this.version = "";
                this.serie = "";
                this.caja = "";
                this.combustible = "";
                this.cilindrada = "";
                this.techo_solar = false;
                this.asientos_cuero = false;
                this.aros_magnesio = false;
                this.observaciones_vehiculo = "";
                this.duenio_nombres_1 = "";
                this.duenio_documento_1 = "";
                this.duenio_monto_1 = 0.0;
                this.duenio_descripcion_1 = "";
                this.duenio_nombres_2 = "";
                this.duenio_documento_2 = "";
                this.duenio_monto_2 = 0.0;
                this.duenio_descripcion_2 = "";
                this.duenio_nombres_3 = "";
                this.duenio_documento_3 = "";
                this.duenio_monto_3 = 0.0;
                this.duenio_descripcion_3 = "";
                this.duenio_nombres_4 = "";
                this.duenio_documento_4 = "";
                this.duenio_monto_4 = 0.0;
                this.duenio_descripcion_4 = "";
                this.duenio_nombres_5 = "";
                this.duenio_documento_5 = "";
                this.duenio_monto_5 = 0.0;
                this.duenio_descripcion_5 = "";
                this.referencia_usada_1 = false;
                this.referencia_medios_1 = "";
                this.referencia_descripcion_1 = "";
                this.referencia_monto_1 = 0.0;
                this.referencia_usada_2 = false;
                this.referencia_medios_2 = "";
                this.referencia_descripcion_2 = "";
                this.referencia_monto_2 = 0.0;
                this.referencia_usada_3 = false;
                this.referencia_medios_3 = "";
                this.referencia_descripcion_3 = "";
                this.referencia_monto_3 = 0.0;
                this.referencia_usada_4 = false;
                this.referencia_medios_4 = "";
                this.referencia_descripcion_4 = "";
                this.referencia_monto_4 = 0.0;
                this.referencia_usada_5 = false;
                this.referencia_medios_5 = "";
                this.referencia_descripcion_5 = "";
                this.referencia_monto_5 = 0.0;
                this.referencia_usada_6 = false;
                this.referencia_medios_6 = "";
                this.referencia_descripcion_6 = "";
                this.referencia_monto_6 = 0.0;
            }
        }
        public static bool PerdidaTotalRoboRegistrar(TipoPerdidaTotalRobo PerdidaTotalRobo)
        {
            bool blnRespuesta = false;
            string strComando = "INSERT INTO [dbo].[cotizacion_perdida_total_robo] " +
              "[id_flujo],[id_cotizacion],[condiciones_especiales],[version],[serie],[caja],[combustible],[cilindrada],[techo_solar],[asientos_cuero],[aros_magnesio],[observaciones_vehiculo],[duenio_nombres_1],[duenio_documento_1],[duenio_monto_1],[duenio_descripcion_1],[duenio_nombres_2],[duenio_documento_2],[duenio_monto_2],[duenio_descripcion_2],[duenio_nombres_3],[duenio_documento_3],[duenio_monto_3],[duenio_descripcion_3],[duenio_nombres_4],[duenio_documento_4],[duenio_monto_4],[duenio_descripcion_4],[duenio_nombres_5]," +
              "[duenio_documento_5],[duenio_monto_5],[duenio_descripcion_5],[referencia_usada_1],[referencia_medios_1],[referencia_descripcion_1],[referencia_monto_1],[referencia_usada_2],[referencia_medios_2],[referencia_descripcion_2],[referencia_monto_2],[referencia_usada_3],[referencia_medios_3],[referencia_descripcion_3],[referencia_monto_3],[referencia_usada_4],[referencia_medios_4],[referencia_descripcion_4],[referencia_monto_4],[referencia_usada_5],[referencia_medios_5],[referencia_descripcion_5],[referencia_monto_5],[referencia_usada_6],[referencia_medios_6],[referencia_descripcion_6],[referencia_monto_6]) " +
              " VALUES (@id_flujo,@id_cotizacion,@condiciones_especiales,@version,@serie,@caja,@combustible,@cilindrada,@techo_solar,@asientos_cuero,@aros_magnesio,@observaciones_vehiculo,@duenio_nombres_1,@duenio_documento_1,@duenio_monto_1,@duenio_descripcion_1,@duenio_nombres_2,@duenio_documento_2,@duenio_monto_2,@duenio_descripcion_2,@duenio_nombres_3,@duenio_documento_3,@duenio_monto_3,@duenio_descripcion_3,@duenio_nombres_4,@duenio_documento_4,@duenio_monto_4,@duenio_descripcion_4,@duenio_nombres_5," +
              "@duenio_documento_5,@duenio_monto_5,@duenio_descripcion_5,@referencia_usada_1,@referencia_medios_1,@referencia_descripcion_1,@referencia_monto_1,@referencia_usada_2,@referencia_medios_2,@referencia_descripcion_2,@referencia_monto_2,@referencia_usada_3,@referencia_medios_3,@referencia_descripcion_3,@referencia_monto_3,@referencia_usada_4,@referencia_medios_4,@referencia_descripcion_4,@referencia_monto_4,@referencia_usada_5,@referencia_medios_5,@referencia_descripcion_5,@referencia_monto_5,@referencia_usada_6,@referencia_medios_6,@referencia_descripcion_6,@referencia_monto_6)";

            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = PerdidaTotalRobo.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = PerdidaTotalRobo.id_cotizacion;
                sqlComando.Parameters.Add("@condiciones_especiales", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.condiciones_especiales;
                sqlComando.Parameters.Add("@version", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.version;
                sqlComando.Parameters.Add("@serie", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.serie;
                sqlComando.Parameters.Add("@caja", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.caja;
                sqlComando.Parameters.Add("@combustible", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.combustible;
                sqlComando.Parameters.Add("@cilindrada", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.cilindrada;
                sqlComando.Parameters.Add("@techo_solar", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.techo_solar;
                sqlComando.Parameters.Add("@asientos_cuero", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.asientos_cuero;
                sqlComando.Parameters.Add("@aros_magnesio", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.aros_magnesio;
                sqlComando.Parameters.Add("@observaciones_vehiculo", System.Data.SqlDbType.VarChar, 350).Value = PerdidaTotalRobo.observaciones_vehiculo;
                sqlComando.Parameters.Add("@duenio_nombres_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_1;
                sqlComando.Parameters.Add("@duenio_documento_1", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_1;
                sqlComando.Parameters.Add("@duenio_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_1;
                sqlComando.Parameters.Add("@duenio_descripcion_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_1;
                sqlComando.Parameters.Add("@duenio_nombres_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_2;
                sqlComando.Parameters.Add("@duenio_documento_2", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_2;
                sqlComando.Parameters.Add("@duenio_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_2;
                sqlComando.Parameters.Add("@duenio_descripcion_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_2;
                sqlComando.Parameters.Add("@duenio_nombres_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_3;
                sqlComando.Parameters.Add("@duenio_documento_3", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_3;
                sqlComando.Parameters.Add("@duenio_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_3;
                sqlComando.Parameters.Add("@duenio_descripcion_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_3;
                sqlComando.Parameters.Add("@duenio_nombres_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_4;
                sqlComando.Parameters.Add("@duenio_documento_4", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_4;
                sqlComando.Parameters.Add("@duenio_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_4;
                sqlComando.Parameters.Add("@duenio_descripcion_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_4;
                sqlComando.Parameters.Add("@duenio_nombres_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_5;
                sqlComando.Parameters.Add("@duenio_documento_5", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_5;
                sqlComando.Parameters.Add("@duenio_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_5;
                sqlComando.Parameters.Add("@duenio_descripcion_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_5;
                sqlComando.Parameters.Add("@referencia_usada_1", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_1;
                sqlComando.Parameters.Add("@referencia_medios_1", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_1;
                sqlComando.Parameters.Add("@referencia_descripcion_1", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_1;
                sqlComando.Parameters.Add("@referencia_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_1;
                sqlComando.Parameters.Add("@referencia_usada_2", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_2;
                sqlComando.Parameters.Add("@referencia_medios_2", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_2;
                sqlComando.Parameters.Add("@referencia_descripcion_2", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_2;
                sqlComando.Parameters.Add("@referencia_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_2;
                sqlComando.Parameters.Add("@referencia_usada_3", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_3;
                sqlComando.Parameters.Add("@referencia_medios_3", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_3;
                sqlComando.Parameters.Add("@referencia_descripcion_3", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_3;
                sqlComando.Parameters.Add("@referencia_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_3;
                sqlComando.Parameters.Add("@referencia_usada_4", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_4;
                sqlComando.Parameters.Add("@referencia_medios_4", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_4;
                sqlComando.Parameters.Add("@referencia_descripcion_4", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_4;
                sqlComando.Parameters.Add("@referencia_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_4;
                sqlComando.Parameters.Add("@referencia_usada_5", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_5;
                sqlComando.Parameters.Add("@referencia_medios_5", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_5;
                sqlComando.Parameters.Add("@referencia_descripcion_5", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_5;
                sqlComando.Parameters.Add("@referencia_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_5;
                sqlComando.Parameters.Add("@referencia_usada_6", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_6;
                sqlComando.Parameters.Add("@referencia_medios_6", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_6;
                sqlComando.Parameters.Add("@referencia_descripcion_6", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_6;
                sqlComando.Parameters.Add("@referencia_monto_6", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_6;
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
                sqlComando.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
        public class TipoPeridaTotalRoboTraer
        {
            public bool Correcto;
            public string Mensaje;
            public List<TipoPerdidaTotalRobo> PerdidasTotalesRobos = new List<TipoPerdidaTotalRobo>();
            public System.Data.DataSet dsPerdidasTotalesRobos = new System.Data.DataSet();
        }
        public static TipoPeridaTotalRoboTraer PerdidaTotalRoboTraer(int Flujo, int Cotizacion)
        {
            TipoPeridaTotalRoboTraer objRespuesta = new TipoPeridaTotalRoboTraer();
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            string strComando = "SELECT [id_item],[condiciones_especiales],[version],[serie],[caja],[combustible],[cilindrada],[techo_solar],[asientos_cuero],[aros_magnesio],[observaciones_vehiculo],[duenio_nombres_1],[duenio_documento_1],[duenio_monto_1],[duenio_descripcion_1],[duenio_nombres_2],[duenio_documento_2],[duenio_monto_2],[duenio_descripcion_2],[duenio_nombres_3],[duenio_documento_3],[duenio_monto_3],[duenio_descripcion_3],[duenio_nombres_4],[duenio_documento_4],[duenio_monto_4],[duenio_descripcion_4],[duenio_nombres_5],[duenio_documento_5],[duenio_monto_5],[duenio_descripcion_5],[referencia_usada_1],[referencia_medios_1],[referencia_descripcion_1],[referencia_monto_1],[referencia_usada_2],[referencia_medios_2],[referencia_descripcion_2],[referencia_monto_2],[referencia_usada_3],[referencia_medios_3],[referencia_descripcion_3],[referencia_monto_3],[referencia_usada_4],[referencia_medios_4],[referencia_descripcion_4],[referencia_monto_4],[referencia_usada_5],[referencia_medios_5],[referencia_descripcion_5],[referencia_monto_5],[referencia_usada_6],[referencia_medios_6],[referencia_descripcion_6],[referencia_monto_6] " +
              "FROM [dbo].[cotizacion_perdida_total_robo] WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion";
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            SqlDataAdapter sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
            SqlDataReader sqlDatos;
            TipoPerdidaTotalRobo tdpFila;
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlConexion.Open();
                sqlDatos = sqlComando.ExecuteReader();
                while (sqlDatos.Read())
                {
                    tdpFila = new TipoPerdidaTotalRobo();
                    tdpFila.id_flujo = Flujo;
                    tdpFila.id_cotizacion = Cotizacion;
                    tdpFila.id_item = Convert.ToInt64(sqlDatos["id_item"]);
                    if (sqlDatos["condiciones_especiales"] != DBNull.Value) tdpFila.condiciones_especiales = Convert.ToBoolean(sqlDatos["condiciones_especiales"]);
                    if (sqlDatos["version"] != DBNull.Value) tdpFila.version = Convert.ToString(sqlDatos["version"]);
                    if (sqlDatos["serie"] != DBNull.Value) tdpFila.serie = Convert.ToString(sqlDatos["serie"]);
                    if (sqlDatos["caja"] != DBNull.Value) tdpFila.caja = Convert.ToString(sqlDatos["caja"]);
                    if (sqlDatos["combustible"] != DBNull.Value) tdpFila.combustible = Convert.ToString(sqlDatos["combustible"]);
                    if (sqlDatos["cilindrada"] != DBNull.Value) tdpFila.cilindrada = Convert.ToString(sqlDatos["cilindrada"]);
                    if (sqlDatos["techo_solar"] != DBNull.Value) tdpFila.techo_solar = Convert.ToBoolean(sqlDatos["techo_solar"]);
                    if (sqlDatos["asientos_cuero"] != DBNull.Value) tdpFila.asientos_cuero = Convert.ToBoolean(sqlDatos["asientos_cuero"]);
                    if (sqlDatos["aros_magnesio"] != DBNull.Value) tdpFila.aros_magnesio = Convert.ToBoolean(sqlDatos["aros_magnesio"]);
                    if (sqlDatos["observaciones_vehiculo"] != DBNull.Value) tdpFila.observaciones_vehiculo = Convert.ToString(sqlDatos["observaciones_vehiculo"]);
                    if (sqlDatos["duenio_nombres_1"] != DBNull.Value) tdpFila.duenio_nombres_1 = Convert.ToString(sqlDatos["duenio_nombres_1"]);
                    if (sqlDatos["duenio_documento_1"] != DBNull.Value) tdpFila.duenio_documento_1 = Convert.ToString(sqlDatos["duenio_documento_1"]);
                    if (sqlDatos["duenio_monto_1"] != DBNull.Value) tdpFila.duenio_monto_1 = Convert.ToDouble(sqlDatos["duenio_monto_1"]);
                    if (sqlDatos["duenio_descripcion_1"] != DBNull.Value) tdpFila.duenio_descripcion_1 = Convert.ToString(sqlDatos["duenio_descripcion_1"]);
                    if (sqlDatos["duenio_nombres_2"] != DBNull.Value) tdpFila.duenio_nombres_2 = Convert.ToString(sqlDatos["duenio_nombres_2"]);
                    if (sqlDatos["duenio_documento_2"] != DBNull.Value) tdpFila.duenio_documento_2 = Convert.ToString(sqlDatos["duenio_documento_2"]);
                    if (sqlDatos["duenio_monto_2"] != DBNull.Value) tdpFila.duenio_monto_2 = Convert.ToDouble(sqlDatos["duenio_monto_2"]);
                    if (sqlDatos["duenio_descripcion_2"] != DBNull.Value) tdpFila.duenio_descripcion_2 = Convert.ToString(sqlDatos["duenio_descripcion_2"]);
                    if (sqlDatos["duenio_nombres_3"] != DBNull.Value) tdpFila.duenio_nombres_3 = Convert.ToString(sqlDatos["duenio_nombres_3"]);
                    if (sqlDatos["duenio_documento_3"] != DBNull.Value) tdpFila.duenio_documento_3 = Convert.ToString(sqlDatos["duenio_documento_3"]);
                    if (sqlDatos["duenio_monto_3"] != DBNull.Value) tdpFila.duenio_monto_3 = Convert.ToDouble(sqlDatos["duenio_monto_3"]);
                    if (sqlDatos["duenio_descripcion_3"] != DBNull.Value) tdpFila.duenio_descripcion_3 = Convert.ToString(sqlDatos["duenio_descripcion_3"]);
                    if (sqlDatos["duenio_nombres_4"] != DBNull.Value) tdpFila.duenio_nombres_4 = Convert.ToString(sqlDatos["duenio_nombres_4"]);
                    if (sqlDatos["duenio_documento_4"] != DBNull.Value) tdpFila.duenio_documento_4 = Convert.ToString(sqlDatos["duenio_documento_4"]);
                    if (sqlDatos["duenio_monto_4"] != DBNull.Value) tdpFila.duenio_monto_4 = Convert.ToDouble(sqlDatos["duenio_monto_4"]);
                    if (sqlDatos["duenio_descripcion_4"] != DBNull.Value) tdpFila.duenio_descripcion_4 = Convert.ToString(sqlDatos["duenio_descripcion_4"]);
                    if (sqlDatos["duenio_nombres_5"] != DBNull.Value) tdpFila.duenio_nombres_5 = Convert.ToString(sqlDatos["duenio_nombres_5"]);
                    if (sqlDatos["duenio_documento_5"] != DBNull.Value) tdpFila.duenio_documento_5 = Convert.ToString(sqlDatos["duenio_documento_5"]);
                    if (sqlDatos["duenio_monto_5"] != DBNull.Value) tdpFila.duenio_monto_5 = Convert.ToDouble(sqlDatos["duenio_monto_5"]);
                    if (sqlDatos["duenio_descripcion_5"] != DBNull.Value) tdpFila.duenio_descripcion_5 = Convert.ToString(sqlDatos["duenio_descripcion_5"]);
                    if (sqlDatos["referencia_usada_1"] != DBNull.Value) tdpFila.referencia_usada_1 = Convert.ToBoolean(sqlDatos["referencia_usada_1"]);
                    if (sqlDatos["referencia_medios_1"] != DBNull.Value) tdpFila.referencia_medios_1 = Convert.ToString(sqlDatos["referencia_medios_1"]);
                    if (sqlDatos["referencia_descripcion_1"] != DBNull.Value) tdpFila.referencia_descripcion_1 = Convert.ToString(sqlDatos["referencia_descripcion_1"]);
                    if (sqlDatos["referencia_monto_1"] != DBNull.Value) tdpFila.referencia_monto_1 = Convert.ToDouble(sqlDatos["referencia_monto_1"]);
                    if (sqlDatos["referencia_usada_2"] != DBNull.Value) tdpFila.referencia_usada_2 = Convert.ToBoolean(sqlDatos["referencia_usada_2"]);
                    if (sqlDatos["referencia_medios_2"] != DBNull.Value) tdpFila.referencia_medios_2 = Convert.ToString(sqlDatos["referencia_medios_2"]);
                    if (sqlDatos["referencia_descripcion_2"] != DBNull.Value) tdpFila.referencia_descripcion_2 = Convert.ToString(sqlDatos["referencia_descripcion_2"]);
                    if (sqlDatos["referencia_monto_2"] != DBNull.Value) tdpFila.referencia_monto_2 = Convert.ToDouble(sqlDatos["referencia_monto_2"]);
                    if (sqlDatos["referencia_usada_3"] != DBNull.Value) tdpFila.referencia_usada_3 = Convert.ToBoolean(sqlDatos["referencia_usada_3"]);
                    if (sqlDatos["referencia_medios_3"] != DBNull.Value) tdpFila.referencia_medios_3 = Convert.ToString(sqlDatos["referencia_medios_3"]);
                    if (sqlDatos["referencia_descripcion_3"] != DBNull.Value) tdpFila.referencia_descripcion_3 = Convert.ToString(sqlDatos["referencia_descripcion_3"]);
                    if (sqlDatos["referencia_monto_3"] != DBNull.Value) tdpFila.referencia_monto_3 = Convert.ToDouble(sqlDatos["referencia_monto_3"]);
                    if (sqlDatos["referencia_usada_4"] != DBNull.Value) tdpFila.referencia_usada_4 = Convert.ToBoolean(sqlDatos["referencia_usada_4"]);
                    if (sqlDatos["referencia_medios_4"] != DBNull.Value) tdpFila.referencia_medios_4 = Convert.ToString(sqlDatos["referencia_medios_4"]);
                    if (sqlDatos["referencia_descripcion_4"] != DBNull.Value) tdpFila.referencia_descripcion_4 = Convert.ToString(sqlDatos["referencia_descripcion_4"]);
                    if (sqlDatos["referencia_monto_4"] != DBNull.Value) tdpFila.referencia_monto_4 = Convert.ToDouble(sqlDatos["referencia_monto_4"]);
                    if (sqlDatos["referencia_usada_5"] != DBNull.Value) tdpFila.referencia_usada_5 = Convert.ToBoolean(sqlDatos["referencia_usada_5"]);
                    if (sqlDatos["referencia_medios_5"] != DBNull.Value) tdpFila.referencia_medios_5 = Convert.ToString(sqlDatos["referencia_medios_5"]);
                    if (sqlDatos["referencia_descripcion_5"] != DBNull.Value) tdpFila.referencia_descripcion_5 = Convert.ToString(sqlDatos["referencia_descripcion_5"]);
                    if (sqlDatos["referencia_monto_5"] != DBNull.Value) tdpFila.referencia_monto_5 = Convert.ToDouble(sqlDatos["referencia_monto_5"]);
                    if (sqlDatos["referencia_usada_6"] != DBNull.Value) tdpFila.referencia_usada_6 = Convert.ToBoolean(sqlDatos["referencia_usada_6"]);
                    if (sqlDatos["referencia_medios_6"] != DBNull.Value) tdpFila.referencia_medios_6 = Convert.ToString(sqlDatos["referencia_medios_6"]);
                    if (sqlDatos["referencia_descripcion_6"] != DBNull.Value) tdpFila.referencia_descripcion_6 = Convert.ToString(sqlDatos["referencia_descripcion_6"]);
                    if (sqlDatos["referencia_monto_6"] != DBNull.Value) tdpFila.referencia_monto_6 = Convert.ToDouble(sqlDatos["referencia_monto_6"]);
                    objRespuesta.PerdidasTotalesRobos.Add(tdpFila);
                }
                sqlDatos.Close();
                sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlAdaptador.SelectCommand.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlAdaptador.Fill(objRespuesta.dsPerdidasTotalesRobos);
                objRespuesta.Correcto = true;
            }
            catch (Exception ex)
            {
                objRespuesta.Correcto = false;
                objRespuesta.Mensaje = "No se pudo traer las perdidas totales por daños propios de la cotizacion debido a: " + ex.Message;
            }
            finally
            {
                sqlComando.Dispose();
                sqlAdaptador.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }

            return objRespuesta;
        }
        public static bool PerdidaTotalRoboModificar(TipoPerdidaTotalRobo PerdidaTotalRobo)
        {
            bool blnRespuesta = false;
            string strComando = "UPDATE [dbo].[cotizacion_perdida_total_robo] " +
              "SET [condiciones_especiales] = @condiciones_especiales,[version] = @version,[serie] = @serie,[caja],[combustible] = @combustible, [cilindrada] = @cilindrada ,[techo_solar] = @techo_solar,[asientos_cuero] = @asientos_cuero,[aros_magnesio] = @aros_magnesio, [observaciones_vehiculo] = @observaciones_vehiculo," +
              "[duenio_nombres_1] = @duenio_nombres_1,[duenio_documento_1] = @duenio_documento_1,[duenio_monto_1] = @duenio_monto_1,[duenio_descripcion_1] = @duenio_descripcion_1," +
              "[duenio_nombres_2] = @duenio_nombres_2,[duenio_documento_2] = @duenio_documento_2,[duenio_monto_2] = @duenio_monto_2,[duenio_descripcion_2] = @duenio_descripcion_2," +
              "[duenio_nombres_3] = @duenio_nombres_3,[duenio_documento_3] = @duenio_documento_3,[duenio_monto_3] = @duenio_monto_3,[duenio_descripcion_3] = @duenio_descripcion_3," +
              "[duenio_nombres_4] = @duenio_nombres_4,[duenio_documento_4] = @duenio_documento_4,[duenio_monto_4] = @duenio_monto_4,[duenio_descripcion_4] = @duenio_descripcion_4," +
              "[duenio_nombres_5] = @duenio_nombres_5,[duenio_documento_5] = @duenio_documento_5,[duenio_monto_5] = @duenio_monto_5,[duenio_descripcion_5] = @duenio_descripcion_5," +
              "[referencia_usada_1] = @referencia_usada_1,[referencia_medios_1] = @referencia_medios_1,[referencia_descripcion_1] = @referencia_descripcion_1,[referencia_monto_1] = @referencia_monto_1," +
              "[referencia_usada_2] = @referencia_usada_2,[referencia_medios_2] = @referencia_medios_2,[referencia_descripcion_2] = @referencia_descripcion_2,[referencia_monto_2] = @referencia_monto_2," +
              "[referencia_usada_3] = @referencia_usada_3,[referencia_medios_3] = @referencia_medios_3,[referencia_descripcion_3] = @referencia_descripcion_3,[referencia_monto_3] = @referencia_monto_3," +
              "[referencia_usada_4] = @referencia_usada_4,[referencia_medios_4] = @referencia_medios_4,[referencia_descripcion_4] = @referencia_descripcion_4,[referencia_monto_4] = @referencia_monto_4," +
              "[referencia_usada_5] = @referencia_usada_5,[referencia_medios_5] = @referencia_medios_5,[referencia_descripcion_5] = @referencia_descripcion_5,[referencia_monto_5] = @referencia_monto_5," +
              "[referencia_usada_6] = @referencia_usada_6,[referencia_medios_6] = @referencia_medios_6,[referencia_descripcion_6] = @referencia_descripcion_6,[referencia_monto_6] = @referencia_monto_6 " +
              "WHERE [id_flujo] = @id_flujo AND [id_cotizacion] = @id_cotizacion AND [id_item] = @id_item";

            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = PerdidaTotalRobo.id_flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = PerdidaTotalRobo.id_cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = PerdidaTotalRobo.id_item;

                sqlComando.Parameters.Add("@condiciones_especiales", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.condiciones_especiales;
                sqlComando.Parameters.Add("@version", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.version;
                sqlComando.Parameters.Add("@serie", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.serie;
                sqlComando.Parameters.Add("@caja", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.caja;
                sqlComando.Parameters.Add("@combustible", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.combustible;
                sqlComando.Parameters.Add("@cilindrada", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.cilindrada;
                sqlComando.Parameters.Add("@techo_solar", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.techo_solar;
                sqlComando.Parameters.Add("@asientos_cuero", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.asientos_cuero;
                sqlComando.Parameters.Add("@aros_magnesio", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.aros_magnesio;
                sqlComando.Parameters.Add("@observaciones_vehiculo", System.Data.SqlDbType.VarChar, 350).Value = PerdidaTotalRobo.observaciones_vehiculo;
                sqlComando.Parameters.Add("@duenio_nombres_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_1;
                sqlComando.Parameters.Add("@duenio_documento_1", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_1;
                sqlComando.Parameters.Add("@duenio_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_1;
                sqlComando.Parameters.Add("@duenio_descripcion_1", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_1;
                sqlComando.Parameters.Add("@duenio_nombres_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_2;
                sqlComando.Parameters.Add("@duenio_documento_2", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_2;
                sqlComando.Parameters.Add("@duenio_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_2;
                sqlComando.Parameters.Add("@duenio_descripcion_2", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_2;
                sqlComando.Parameters.Add("@duenio_nombres_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_3;
                sqlComando.Parameters.Add("@duenio_documento_3", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_3;
                sqlComando.Parameters.Add("@duenio_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_3;
                sqlComando.Parameters.Add("@duenio_descripcion_3", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_3;
                sqlComando.Parameters.Add("@duenio_nombres_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_4;
                sqlComando.Parameters.Add("@duenio_documento_4", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_4;
                sqlComando.Parameters.Add("@duenio_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_4;
                sqlComando.Parameters.Add("@duenio_descripcion_4", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_4;
                sqlComando.Parameters.Add("@duenio_nombres_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_nombres_5;
                sqlComando.Parameters.Add("@duenio_documento_5", System.Data.SqlDbType.VarChar, 20).Value = PerdidaTotalRobo.duenio_documento_5;
                sqlComando.Parameters.Add("@duenio_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.duenio_monto_5;
                sqlComando.Parameters.Add("@duenio_descripcion_5", System.Data.SqlDbType.VarChar, 150).Value = PerdidaTotalRobo.duenio_descripcion_5;
                sqlComando.Parameters.Add("@referencia_usada_1", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_1;
                sqlComando.Parameters.Add("@referencia_medios_1", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_1;
                sqlComando.Parameters.Add("@referencia_descripcion_1", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_1;
                sqlComando.Parameters.Add("@referencia_monto_1", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_1;
                sqlComando.Parameters.Add("@referencia_usada_2", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_2;
                sqlComando.Parameters.Add("@referencia_medios_2", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_2;
                sqlComando.Parameters.Add("@referencia_descripcion_2", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_2;
                sqlComando.Parameters.Add("@referencia_monto_2", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_2;
                sqlComando.Parameters.Add("@referencia_usada_3", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_3;
                sqlComando.Parameters.Add("@referencia_medios_3", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_3;
                sqlComando.Parameters.Add("@referencia_descripcion_3", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_3;
                sqlComando.Parameters.Add("@referencia_monto_3", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_3;
                sqlComando.Parameters.Add("@referencia_usada_4", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_4;
                sqlComando.Parameters.Add("@referencia_medios_4", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_4;
                sqlComando.Parameters.Add("@referencia_descripcion_4", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_4;
                sqlComando.Parameters.Add("@referencia_monto_4", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_4;
                sqlComando.Parameters.Add("@referencia_usada_5", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_5;
                sqlComando.Parameters.Add("@referencia_medios_5", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_5;
                sqlComando.Parameters.Add("@referencia_descripcion_5", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_5;
                sqlComando.Parameters.Add("@referencia_monto_5", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_5;
                sqlComando.Parameters.Add("@referencia_usada_6", System.Data.SqlDbType.Bit).Value = PerdidaTotalRobo.referencia_usada_6;
                sqlComando.Parameters.Add("@referencia_medios_6", System.Data.SqlDbType.VarChar, 100).Value = PerdidaTotalRobo.referencia_medios_6;
                sqlComando.Parameters.Add("@referencia_descripcion_6", System.Data.SqlDbType.VarChar, 200).Value = PerdidaTotalRobo.referencia_descripcion_6;
                sqlComando.Parameters.Add("@referencia_monto_6", System.Data.SqlDbType.Float).Value = PerdidaTotalRobo.referencia_monto_6;
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
                sqlComando.Dispose();
                sqlConexion.Close();
                sqlConexion.Dispose();
            }
            return blnRespuesta;
        }
        public static bool PerdidaTotalRoboBorrar(int Flujo, int Cotizacion, long Item)
        {
            bool blnRespuesta = false;
            string strComando = "DELETE FROM [dbo].[cotizacion_perdida_total_robo] WHERE id_flujo = @id_flujo AND id_cotizacion = @id_cotizacion AND id_item = @id_item";
            SqlConnection sqlConexion = new SqlConnection(strCadenaConexion);
            SqlCommand sqlComando = new SqlCommand(strComando);
            try
            {
                sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
                sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Cotizacion;
                sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Item;
                sqlConexion.Open();
                sqlComando.ExecuteNonQuery();
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