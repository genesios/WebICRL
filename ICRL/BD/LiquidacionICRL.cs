using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ICRL.BD
{
  public class LiquidacionICRL
  {
    public class TipoFlujo
    {
      public int idFlujo;
      public string flujoOnBase;
      public string estado;
      public string numeroReclamo;
      public string numeroPoliza;
      public string marcaVehiculo;
      public string modeloVehiculo;
      public string anioVehiculo;
      public string colorVehiculo;
      public string placaVehiculo;
      public string chasisVehiculo;
      public string valorAsegurado;
      public string importacionDirecta;
      public string nombreAsegurado;
      public string docIdAsegurado;
      public string telefonocelAsegurado;
      public string causaSiniestro;
      public string contador;
      public string descripcionSiniestro;
      public string direccionInspeccion;
      public string agenciaAtencion;
      public string fechaSiniestro;
      public TipoFlujo()
      {
        this.idFlujo = 0;
        this.flujoOnBase = "";
        this.estado = "";
        this.numeroReclamo = "";
        this.numeroPoliza = "";
        this.marcaVehiculo = "";
        this.modeloVehiculo = "";
        this.anioVehiculo = "";
        this.colorVehiculo = "";
        this.placaVehiculo = "";
        this.chasisVehiculo = "";
        this.valorAsegurado = "";
        this.importacionDirecta = "";
        this.nombreAsegurado = "";
        this.docIdAsegurado = "";
        this.telefonocelAsegurado = "";
        this.causaSiniestro = "";
        this.contador = "";
        this.descripcionSiniestro = "";
        this.direccionInspeccion = "";
        this.agenciaAtencion = "";
        this.fechaSiniestro = "";
      }

    }
    public static TipoFlujo TipoFlujoTraer(int Flujo)
    {
      TipoFlujo objRespuesta = new TipoFlujo();
      SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
      string strComando = "SELECT [idFlujo],[flujoOnBase],[estado],[numeroReclamo],[numeroPoliza],[marcaVehiculo],[modeloVehiculo],[anioVehiculo],[colorVehiculo],[placaVehiculo],[chasisVehiculo],[valorAsegurado],[importacionDirecta],[nombreAsegurado],[docIdAsegurado],[telefonocelAsegurado],[causaSiniestro],[contador],[descripcionSiniestro],[direccionInspeccion],[agenciaAtencion],[fechaSiniestro] FROM [LBCDesa].[dbo].[Flujo] WHERE idFlujo=@idFlujo";
      SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
      SqlDataReader sqlDatos;
      try
      {
        sqlComando.Parameters.Add("@idFlujo", System.Data.SqlDbType.Int).Value = Flujo;
        sqlConexion.Open();
        sqlDatos = sqlComando.ExecuteReader();
        while (sqlDatos.Read())
        {
          objRespuesta.idFlujo = Flujo;
          if (sqlDatos["flujoOnBase"] != DBNull.Value) objRespuesta.flujoOnBase = Convert.ToString(sqlDatos["flujoOnBase"]);
          if (sqlDatos["estado"] != DBNull.Value) objRespuesta.estado = Convert.ToString(sqlDatos["estado"]);
          if (sqlDatos["numeroReclamo"] != DBNull.Value) objRespuesta.numeroReclamo = Convert.ToString(sqlDatos["numeroReclamo"]);
          if (sqlDatos["numeroPoliza"] != DBNull.Value) objRespuesta.numeroPoliza = Convert.ToString(sqlDatos["numeroPoliza"]);
          if (sqlDatos["marcaVehiculo"] != DBNull.Value) objRespuesta.marcaVehiculo = Convert.ToString(sqlDatos["marcaVehiculo"]);
          if (sqlDatos["modeloVehiculo"] != DBNull.Value) objRespuesta.modeloVehiculo = Convert.ToString(sqlDatos["modeloVehiculo"]);
          if (sqlDatos["anioVehiculo"] != DBNull.Value) objRespuesta.anioVehiculo = Convert.ToString(sqlDatos["anioVehiculo"]);
          if (sqlDatos["colorVehiculo"] != DBNull.Value) objRespuesta.colorVehiculo = Convert.ToString(sqlDatos["colorVehiculo"]);
          if (sqlDatos["placaVehiculo"] != DBNull.Value) objRespuesta.placaVehiculo = Convert.ToString(sqlDatos["placaVehiculo"]);
          if (sqlDatos["chasisVehiculo"] != DBNull.Value) objRespuesta.chasisVehiculo = Convert.ToString(sqlDatos["chasisVehiculo"]);
          if (sqlDatos["valorAsegurado"] != DBNull.Value) objRespuesta.valorAsegurado = Convert.ToString(sqlDatos["valorAsegurado"]);
          if (sqlDatos["importacionDirecta"] != DBNull.Value) objRespuesta.importacionDirecta = Convert.ToString(sqlDatos["importacionDirecta"]);
          if (sqlDatos["nombreAsegurado"] != DBNull.Value) objRespuesta.nombreAsegurado = Convert.ToString(sqlDatos["nombreAsegurado"]);
          if (sqlDatos["docIdAsegurado"] != DBNull.Value) objRespuesta.docIdAsegurado = Convert.ToString(sqlDatos["docIdAsegurado"]);
          if (sqlDatos["telefonocelAsegurado"] != DBNull.Value) objRespuesta.telefonocelAsegurado = Convert.ToString(sqlDatos["telefonocelAsegurado"]);
          if (sqlDatos["causaSiniestro"] != DBNull.Value) objRespuesta.causaSiniestro = Convert.ToString(sqlDatos["causaSiniestro"]);
          if (sqlDatos["contador"] != DBNull.Value) objRespuesta.contador = Convert.ToString(sqlDatos["contador"]);
          if (sqlDatos["descripcionSiniestro"] != DBNull.Value) objRespuesta.descripcionSiniestro = Convert.ToString(sqlDatos["descripcionSiniestro"]);
          if (sqlDatos["direccionInspeccion"] != DBNull.Value) objRespuesta.direccionInspeccion = Convert.ToString(sqlDatos["direccionInspeccion"]);
          if (sqlDatos["agenciaAtencion"] != DBNull.Value) objRespuesta.agenciaAtencion = Convert.ToString(sqlDatos["agenciaAtencion"]);
          if (sqlDatos["fechaSiniestro"] != DBNull.Value) objRespuesta.fechaSiniestro = Convert.ToString(sqlDatos["fechaSiniestro"]);
        }
        sqlDatos.Close();
      }
      catch (Exception)
      {
        objRespuesta = new TipoFlujo();
      }
      finally
      {
        sqlConexion.Close();
        sqlConexion.Dispose();
      }
      return objRespuesta;
    }

    public class TipoRespuestaGrilla
    {
      public bool correcto;
      public string mensaje;
      public System.Data.DataSet dsLiquidacionGrilla = new System.Data.DataSet();
    }
    public static TipoRespuestaGrilla LiquidacionGrilla(int Estado, string Proveedor, string Sucursal, string FlujoONBase, string Placa, DateTime FechaIncio, DateTime FechaFin)
    {
      TipoRespuestaGrilla objRespuesta = new TipoRespuestaGrilla();
      string strSelect, strWhere, strGroupby;
      strSelect = "SELECT l.[numero_orden],l.[fecha_orden],l.[proveedor],l.id_estado,sum(l.[precious]) as Total, SUM(CASE liquidacion WHEN 'True' THEN preciobs WHEN 'False' THEN 0 END) as sumabspagado, SUM(CASE liquidacion WHEN 'True' THEN 0 WHEN 'False' THEN preciobs END) as sumabsnopagado,SUM(CASE liquidacion WHEN 'True' THEN precious WHEN 'False' THEN 0 END) as sumauspagado, SUM(CASE liquidacion WHEN 'True' THEN 0 WHEN 'False' THEN precious END) as sumausnopagado,f.flujoOnBase,f.nombreAsegurado,f.placaVehiculo,f.idFlujo FROM liquidacion001 as l inner join Flujo as f ON (l.id_flujo=f.idFlujo) ";
      if (Estado > 0)
        strWhere = "WHERE f.estado=" + Estado.ToString() + " and l.fecha_orden>= @fecha_orden_emp and l.fecha_orden <= @fecha_orden_ter ";
      else
        strWhere = "WHERE l.fecha_orden>= @fecha_orden_emp and l.fecha_orden <= @fecha_orden_ter ";
      strGroupby = "GROUP BY l.[numero_orden],l.[fecha_orden],l.[proveedor],l.id_estado,f.flujoOnBase,f.nombreAsegurado,f.placaVehiculo,f.idFlujo";

      string strComando;
      SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
      SqlDataAdapter sqlAdaptador;
      try
      {
        if (Sucursal.Trim().Length > 0) strWhere = strWhere + "AND f.agenciaAtencion = '" + Sucursal.Trim() + "' ";
        if (FlujoONBase.Trim().Length > 0) strWhere = strWhere + "AND f.flujoOnBase = '" + FlujoONBase.Trim() + "' ";
        if (Placa.Trim().Length > 0) strWhere = strWhere + "AND f.placaVehiculo = '" + Placa.Trim() + "' ";
        if (Proveedor.Trim().Length > 0) strWhere = strWhere + "AND l.proveedor = '" + Proveedor.Trim() + "' ";
        strComando = strSelect + strWhere + strGroupby;
        sqlAdaptador = new SqlDataAdapter(strComando, sqlConexion);
        sqlAdaptador.SelectCommand.Parameters.Add("@fecha_orden_emp", System.Data.SqlDbType.DateTime).Value = FechaIncio;
        sqlAdaptador.SelectCommand.Parameters.Add("@fecha_orden_ter", System.Data.SqlDbType.DateTime).Value = FechaFin;
        sqlConexion.Open();
        sqlAdaptador.Fill(objRespuesta.dsLiquidacionGrilla);
        sqlAdaptador.Dispose();
        objRespuesta.correcto = true;
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
      public DateTime fecha_orden;
      public string proveedor;
      public string item_descripcion;
      public double preciobs;
      public double precious;
      public DateTime fecha_recepcion;
      public bool inspeccion;
      public bool liquidacion;
      public string num_factura;
      public short id_estado;
      public DateTime fecha_liquidacion;
      public TipoLiquidacion001()
      {
        this.id_flujo = 0;
        this.id_cotizacion = 0;
        this.tipo_origen = 0;
        this.id_item = 0;
        this.id_tipo_item = 0;
        this.numero_orden = "";
        this.fecha_orden = new DateTime(2000, 1, 1);
        this.proveedor = "";
        this.item_descripcion = "";
        this.preciobs = 0.0;
        this.precious = 0.0;
        this.fecha_recepcion = new DateTime(2000, 1, 1);
        this.inspeccion = false;
        this.liquidacion = false;
        this.num_factura = "";
        this.id_estado = 0;
        this.fecha_liquidacion = new DateTime(2000, 1, 1);
      }
    }
    public static bool RegistrarLiquidaciones001(TipoLiquidacion001 Liquidacion)
    {
      bool blnRespuesta = false;
      SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
      string strComando = "INSERT INTO [dbo].[liquidacion001]([id_flujo],[id_cotizacion],[tipo_origen],[id_item],[id_tipo_item],[numero_orden],[fecha_orden],[proveedor],[item_descripcion],[preciobs],[precious],[fecha_recepcion],[inspeccion],[liquidacion],[num_factura],[id_estado],[fecha_liquidacion]) " +
        "VALUES(@id_flujo,@id_cotizacion,@tipo_origen,@id_item,@id_tipo_item,@numero_orden,@fecha_orden,@proveedor,@item_descripcion,@preciobs,@precious,@fecha_recepcion,@inspeccion,@liquidacion,@num_factura,@id_estado,@fecha_liquidacion)";
      SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
      try
      {
        sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Liquidacion.id_flujo;
        sqlComando.Parameters.Add("@id_cotizacion", System.Data.SqlDbType.Int).Value = Liquidacion.id_cotizacion;
        sqlComando.Parameters.Add("@tipo_origen", System.Data.SqlDbType.SmallInt).Value = Liquidacion.tipo_origen;
        sqlComando.Parameters.Add("@id_item", System.Data.SqlDbType.BigInt).Value = Liquidacion.id_item;
        sqlComando.Parameters.Add("@id_tipo_item", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_tipo_item;
        sqlComando.Parameters.Add("@numero_orden", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.numero_orden;
        sqlComando.Parameters.Add("@fecha_orden", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_orden;
        sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = Liquidacion.proveedor;
        sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.item_descripcion;
        sqlComando.Parameters.Add("@preciobs", System.Data.SqlDbType.Float).Value = Liquidacion.preciobs;
        sqlComando.Parameters.Add("@precious", System.Data.SqlDbType.Float).Value = Liquidacion.precious;
        sqlComando.Parameters.Add("@fecha_recepcion", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_recepcion;
        sqlComando.Parameters.Add("@inspeccion", System.Data.SqlDbType.Bit).Value = Liquidacion.inspeccion;
        sqlComando.Parameters.Add("@liquidacion", System.Data.SqlDbType.Bit).Value = Liquidacion.liquidacion;
        sqlComando.Parameters.Add("@num_factura", System.Data.SqlDbType.VarChar, 20).Value = Liquidacion.num_factura;
        sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_estado;
        sqlComando.Parameters.Add("@fecha_liquidacion", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_liquidacion;
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
      string strComando = "SELECT [id_flujo],[id_cotizacion],[tipo_origen],[id_item],[id_tipo_item],[numero_orden],[proveedor],[item_descripcion],[preciobs],[precious],[fecha_recepcion],[inspeccion],[liquidacion],[num_factura],[id_estado],[fecha_liquidacion],[fecha_orden] FROM [dbo].[liquidacion001] WHERE id_flujo=@id_flujo ORDER BY numero_orden";
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
          if (sqlDatos["precious"] != DBNull.Value) tdpFila.precious = Convert.ToDouble(sqlDatos["precious"]);
          if (sqlDatos["fecha_recepcion"] != DBNull.Value) tdpFila.fecha_recepcion = Convert.ToDateTime(sqlDatos["fecha_recepcion"]);
          if (sqlDatos["inspeccion"] != DBNull.Value) tdpFila.inspeccion = Convert.ToBoolean(sqlDatos["inspeccion"]);
          if (sqlDatos["liquidacion"] != DBNull.Value) tdpFila.liquidacion = Convert.ToBoolean(sqlDatos["liquidacion"]);
          if (sqlDatos["num_factura"] != DBNull.Value) tdpFila.numero_orden = Convert.ToString(sqlDatos["num_factura"]);
          if (sqlDatos["id_estado"] != DBNull.Value) tdpFila.id_estado = Convert.ToInt16(sqlDatos["id_estado"]);
          if (sqlDatos["fecha_liquidacion"] != DBNull.Value) tdpFila.fecha_liquidacion = Convert.ToDateTime(sqlDatos["fecha_liquidacion"]);
          if (sqlDatos["fecha_orden"] != DBNull.Value) tdpFila.fecha_liquidacion = Convert.ToDateTime(sqlDatos["fecha_orden"]);
          objRespuesta.Liquidaciones001.Add(tdpFila);
        }
        sqlDatos.Close();
        sqlAdaptador.Fill(objRespuesta.dsLiquidacion001);
        for (int i = 0; i <= objRespuesta.dsLiquidacion001.Tables[0].Rows.Count - 1; i++)
        {
          if (!objRespuesta.dsLiquidacion001.Tables[0].Rows[i].IsNull("fecha_recepcion") && Convert.ToDateTime(objRespuesta.dsLiquidacion001.Tables[0].Rows[i].ItemArray[10]).Year == 2000)
          {
            objRespuesta.dsLiquidacion001.Tables[0].Rows[i][10] = System.DBNull.Value;
          }
          if (!objRespuesta.dsLiquidacion001.Tables[0].Rows[i].IsNull("fecha_liquidacion") && Convert.ToDateTime(objRespuesta.dsLiquidacion001.Tables[0].Rows[i].ItemArray[15]).Year == 2000)
          {
            objRespuesta.dsLiquidacion001.Tables[0].Rows[i][15] = System.DBNull.Value;
          }
        }
        sqlComando.Dispose();
        objRespuesta.correcto = true;
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
      string strComando = "UPDATE [dbo].[liquidacion001] SET [numero_orden] = @numero_orden, [fecha_orden] = @fecha_orden, [proveedor] = @proveedor,[item_descripcion] = @item_descripcion,[preciobs] = @preciobs,[precious] = @precious,[fecha_recepcion] = @fecha_recepcion,[inspeccion] = @inspeccion,[liquidacion] = @liquidacion,[num_factura] = @num_factura,[id_estado] = @id_estado, [fecha_liquidacion] = @fecha_liquidacion " +
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
        sqlComando.Parameters.Add("@fecha_orden", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_orden;
        sqlComando.Parameters.Add("@proveedor", System.Data.SqlDbType.VarChar, 150).Value = Liquidacion.proveedor;
        sqlComando.Parameters.Add("@item_descripcion", System.Data.SqlDbType.VarChar, 50).Value = Liquidacion.item_descripcion;
        sqlComando.Parameters.Add("@preciobs", System.Data.SqlDbType.Float).Value = Liquidacion.preciobs;
        sqlComando.Parameters.Add("@precious", System.Data.SqlDbType.Float).Value = Liquidacion.precious;
        sqlComando.Parameters.Add("@fecha_recepcion", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_recepcion;
        sqlComando.Parameters.Add("@inspeccion", System.Data.SqlDbType.Bit).Value = Liquidacion.inspeccion;
        sqlComando.Parameters.Add("@liquidacion", System.Data.SqlDbType.Bit).Value = Liquidacion.liquidacion;
        sqlComando.Parameters.Add("@num_factura", System.Data.SqlDbType.VarChar, 20).Value = Liquidacion.num_factura;
        sqlComando.Parameters.Add("@id_estado", System.Data.SqlDbType.SmallInt).Value = Liquidacion.id_estado;
        sqlComando.Parameters.Add("@fecha_liquidacion", System.Data.SqlDbType.DateTime).Value = Liquidacion.fecha_liquidacion;
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
      public string observaciones;
      public bool asociada;
      public double tipo_cambio;
      public TipoLiquidacion001Factura()
      {
        this.id_flujo = 0;
        this.id_factura = 0;
        this.numero_factura = 0;
        this.fecha_emision = new DateTime(2000, 1, 1);
        this.fecha_entrega = new DateTime(2000, 1, 1);
        this.id_moneda = 0;
        this.monto = 0.0;
        this.observaciones = "";
        this.asociada = false;
        this.tipo_cambio = 1.0;
      }
    }
    public static bool RegistrarLiquidacion001Factura(TipoLiquidacion001Factura Factura)
    {
      bool blnRespuesta = false;
      SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
      string strComando = "INSERT INTO [dbo].[liquidacion001_factura] ([id_flujo],[numero_factura],[fecha_emision],[fecha_entrega],[id_moneda],[monto],[observaciones],[asociada],[tipo_cambio]) VALUES (@id_flujo,@numero_factura,@fecha_emision,@fecha_entrega,@id_moneda,@monto,@observaciones,@asociada,@tipo_cambio)";
      SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
      try
      {
        sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Factura.id_flujo;
        sqlComando.Parameters.Add("@numero_factura", System.Data.SqlDbType.BigInt).Value = Factura.numero_factura;
        sqlComando.Parameters.Add("@fecha_emision", System.Data.SqlDbType.DateTime).Value = Factura.fecha_emision;
        sqlComando.Parameters.Add("@fecha_entrega", System.Data.SqlDbType.DateTime).Value = Factura.fecha_entrega;
        sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = Factura.id_moneda;
        sqlComando.Parameters.Add("@monto", System.Data.SqlDbType.Float).Value = Factura.monto;
        sqlComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar, 200).Value = Factura.observaciones;
        sqlComando.Parameters.Add("@asociada", System.Data.SqlDbType.Bit).Value = Factura.asociada;
        sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = Factura.tipo_cambio;
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
      string strComando = "SELECT [id_flujo],[id_factura],[numero_factura],[fecha_emision],[fecha_entrega],[id_moneda],[monto],[observaciones],[asociada],[tipo_cambio] FROM [dbo].[liquidacion001_factura] WHERE [id_flujo] = @id_flujo";
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
          if (sqlDatos["observaciones"] != DBNull.Value) tdpFila.observaciones = Convert.ToString(sqlDatos["observaciones"]);
          if (sqlDatos["asociada"] != DBNull.Value) tdpFila.asociada = Convert.ToBoolean(sqlDatos["asociada"]);
          if (sqlDatos["tipo_cambio"] != DBNull.Value) tdpFila.tipo_cambio = Convert.ToDouble(sqlDatos["tipo_cambio"]);
          objRespuesta.Facturas.Add(tdpFila);
        }
        sqlDatos.Close();
        sqlComando.Dispose();
        sqlAdaptador.SelectCommand.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
        sqlAdaptador.Fill(objRespuesta.dsFacturas);
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
      string strComando = "UPDATE [dbo].[liquidacion001_factura] SET [numero_factura] = @numero_factura,[fecha_emision] = @fecha_emision,[fecha_entrega] = @fecha_entrega,[id_moneda] = @id_moneda,[monto] = @monto,[observaciones] = @observaciones, [asociada] = @asociada, [tipo_cambio] = @tipo_cambio WHERE [id_flujo] = @id_flujo AND [id_factura] = @id_factura";
      SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
      try
      {
        sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Factura.id_flujo;
        sqlComando.Parameters.Add("@id_factura", System.Data.SqlDbType.BigInt).Value = Factura.id_factura;

        sqlComando.Parameters.Add("@numero_factura", System.Data.SqlDbType.BigInt).Value = Factura.numero_factura;
        sqlComando.Parameters.Add("@fecha_emision", System.Data.SqlDbType.DateTime).Value = Factura.fecha_emision;
        sqlComando.Parameters.Add("@fecha_entrega", System.Data.SqlDbType.DateTime).Value = Factura.fecha_entrega;
        sqlComando.Parameters.Add("@id_moneda", System.Data.SqlDbType.SmallInt).Value = Factura.id_moneda;
        sqlComando.Parameters.Add("@monto", System.Data.SqlDbType.Float).Value = Factura.monto;
        sqlComando.Parameters.Add("@observaciones", System.Data.SqlDbType.VarChar, 200).Value = Factura.observaciones;
        sqlComando.Parameters.Add("@asociada", System.Data.SqlDbType.Bit).Value = Factura.asociada;
        sqlComando.Parameters.Add("@tipo_cambio", System.Data.SqlDbType.Float).Value = Factura.tipo_cambio;
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
    public static bool BorrarLiquidacion001Factura(int Flujo, long ID_Factura)
    {
      bool blnRespuesta = false;
      SqlConnection sqlConexion = new SqlConnection(CotizacionICRL.strCadenaConexion);
      string strComando = "DELETE FROM [dbo].[liquidacion001_factura] WHERE id_flujo = @id_flujo AND id_factura = @id_factura";
      SqlCommand sqlComando = new SqlCommand(strComando, sqlConexion);
      try
      {
        sqlComando.Parameters.Add("@id_flujo", System.Data.SqlDbType.Int).Value = Flujo;
        sqlComando.Parameters.Add("@id_factura", System.Data.SqlDbType.BigInt).Value = ID_Factura;
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