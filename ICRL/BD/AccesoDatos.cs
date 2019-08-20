using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ICRL.ModeloDB;
using LbcOnBaseWS;
using LbcConsultaUsuarioSistema;

namespace ICRL.BD
{
  public class AccesoDatos
  {
    public string vSistema = "EXT101";
    public string vMensajeClase = string.Empty;
    public enum TipoInspeccion
    {
      DaniosPropios = 1,
      RCObjetos = 2,
      RCPersonas = 3,
      RoboParcial = 4,
      PerdidaTotalDaniosPropios = 5,
      PerdidaTotalRobo = 6,
      RCVEhicular = 7
    };
    public FlujoICRL vFlujoICRL { get; set; }
    public UsuarioICRL vUsuarioICRL { get; set; }
    public InspeccionICRL vInspeccionICRL { get; set; }

    #region Acceso Argos
    public int FValidaExisteUsuarioArgos(string pCodUsuario, string pContrasenia)
    {
      int vResultado = 0;

      UsuarioSistemaEntity vClienteResultado = new UsuarioSistemaEntity();
      ConsultaUsuarioSistema vClienteServicio = new ConsultaUsuarioSistema();

      try
      {
        // Llamada al WS Argos para validar la información de Usuario y Contrasenia
        vClienteResultado = vClienteServicio.ConsultarUsuarioSistema(pCodUsuario, vSistema, pContrasenia);
        if (1 == vClienteResultado.CodigoEstado)
        {

          vResultado = 1;
        }
      }
      catch (Exception ex)
      {
        vResultado = -1;
      }

      return vResultado;
    }

    public UsuarioICRL FTraerDatosUsuarioArgos(string pCodUsuario, string pContrasenia)
    {
      vUsuarioICRL = null;


      UsuarioSistemaEntity vClienteResultado = new UsuarioSistemaEntity();
      ConsultaUsuarioSistema vClienteServicio = new ConsultaUsuarioSistema();

      try
      {
        // Llamada al WS Argos para validar la información de Usuario y Contrasenia
        vClienteResultado = vClienteServicio.ConsultarUsuarioSistema(pCodUsuario, vSistema, pContrasenia);
        if (1 == vClienteResultado.CodigoEstado)
        {
          vUsuarioICRL = new UsuarioICRL();
          vUsuarioICRL.codUsuario = vClienteResultado.Usuario;
          vUsuarioICRL.apellidos = vClienteResultado.Apellidos;
          vUsuarioICRL.nombres = vClienteResultado.Nombres;
          vUsuarioICRL.nombreVisible = vClienteResultado.NombreLargo;
          vUsuarioICRL.codSucursal = vClienteResultado.Sucursal;
          vUsuarioICRL.correoElectronico = vClienteResultado.Correo;
          vUsuarioICRL.estado = 0;
        }
      }
      catch (Exception ex)
      {
        vMensajeClase = ex.Message;
      }

      return vUsuarioICRL;
    }
    #endregion

    #region Acceso ICRL 
    public int FValidaExisteUsuarioICRL(string pCodUsuario)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaUsuario = from d in db.Usuario
                            where d.codUsuario == pCodUsuario
                            select d;

        var vFilaTablaUsuario = vTablaUsuario.FirstOrDefault<Usuario>();

        if (null != vFilaTablaUsuario)
        {
          vResultado = vFilaTablaUsuario.idUsuario;
        }
      }

      return vResultado;
    }

    public int FGrabaUsuarioICRL(UsuarioICRL pUsuario, string pCodUsuario)
    {
      int vResultado = 0;

      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          Usuario vTablaUsuario = new Usuario();

          vTablaUsuario.codUsuario = pUsuario.codUsuario;
          vTablaUsuario.apellidos = pUsuario.apellidos;
          vTablaUsuario.nombres = pUsuario.nombres;
          vTablaUsuario.nombreVisible = pUsuario.nombreVisible;
          vTablaUsuario.codSucursal = pUsuario.codSucursal;
          vTablaUsuario.correoElectronico = pUsuario.correoElectronico;
          vTablaUsuario.estado = 0;
          vTablaUsuario.usuarioCreacion = pCodUsuario;
          vTablaUsuario.fechaCreacion = DateTime.Now;

          db.Usuario.Add(vTablaUsuario);

          db.SaveChanges();
          vResultado = 1;

        }
      }
      catch (Exception ex)
      {
        vResultado = 0;
      }
      return vResultado;
    }

    public int FActualizaUsuarioICRL(UsuarioICRL pUsuario, int pIdUsuario)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        Usuario vTablaUsuario = new Usuario();

        vTablaUsuario = db.Usuario.Find(pIdUsuario);

        vTablaUsuario.apellidos = pUsuario.apellidos;
        vTablaUsuario.nombres = pUsuario.nombres;
        vTablaUsuario.nombreVisible = pUsuario.nombreVisible;
        vTablaUsuario.codSucursal = pUsuario.codSucursal;
        vTablaUsuario.correoElectronico = pUsuario.correoElectronico;

        db.Entry(vTablaUsuario).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;

      }

      return vResultado;
    }
    #endregion

    #region Flujo OnBase
    public int FValidaExisteFlujoOnBase(string pNumFlujo)
    {
      int vRespuesta = 0;

      /*** CONECTAR A WS ***/
      OnBaseWS vServicioOnBase = new OnBaseWS();
      /*** ESTABLECER LA APLICACIÓN ORIGEN POR DEFECTO PARA GESPRO ***/
      SistemaOrigen vOrigen = SistemaOrigen.ICRL;
      /*** INSTANCIAR EL RESULTADO COMO ResultadoEntity ***/
      ResultadoEntity vResultadoEntity = new ResultadoEntity();
      /*** LLAMAR A LA FUNCIÓN DEL WS ***/
      vResultadoEntity = vServicioOnBase.ObtenerInformacionSolicitudOnBase(pNumFlujo, vOrigen);

      /*** SI EL RESULTADO ES CORRECTO, SE EXTRAE LA INFORMACIÓN DEL FLUJO ***/
      if (vResultadoEntity.EsValido)
      {
        vRespuesta = 1;
      }

      return vRespuesta;
    }

    public int FEnviaArchivoOnBase(string pFlujoOnBase, string pTipoDocumental, string pNomUsuario, byte[] pArrayBytesArchivo)
    {
      int vResultado = 0;
      //solo se enviara un archivo a la vez
      string[] vArchivoBase64 = new string[1];

      vArchivoBase64[0] = Convert.ToBase64String(pArrayBytesArchivo);

      /*** CONECTAR A WS ***/
      OnBaseWS vServicioOnBase = new OnBaseWS();
      /*** ESTABLECER LA APLICACIÓN ORIGEN POR DEFECTO ICRL ***/
      SistemaOrigen vOrigen = SistemaOrigen.ICRL;
      /*** INSTANCIAR EL RESULTADO COMO ResultadoEntity ***/
      ResultadoEntity vResultadoEntity = new ResultadoEntity();
      /*** INSTANCIAR EL OBJETO DE ENVÍO COMO DocumentoOnBaseEntity ***/
      var vDocumento = new DocumentoOnBaseEntity();
      /*** LLENAR LAS PROPIEDADES SEGÚN EL TIPO DE DOCUMENTO A IMPORTAR ***/
      vDocumento.nroSolicitud = pFlujoOnBase;  //número de solicitud
      vDocumento.tipoDocumento = pTipoDocumental;   //nombre del tipo de documento
      vDocumento.extensionArchivo = "pdf"; //Extensión del archivo. Ej:jpg - Ej2:pdf
      /*** CREAR OBJETO KeywordOnBaseEntity DE NOMBRE DE USUARIO PARA AÑADIR AL DOCUMENTO ***/
      var keyUsuarioWS = new KeywordOnBaseEntity() { nombre = "LBC UserName WS", valor = pNomUsuario };
      /*** CREAR OBJETO KeywordOnBaseEntity DE 'No. de RC' SOLO PARA AÑADIR AL DOCUMENTO DE TIPO RE - RC Atencion ***/
      //No aplica a los tipos documentales enviamos siempre 1
      //RE - Orden de Trabajo  ;
      //RE - Orden de Compra  ;
      //RE - Orden de Indemnizacion  ;
      //var keyRC = new KeywordOnBaseEntity() { nombre = "No. de RC", valor = "1" };
      /*** ASIGNAR LOS keywords ANTERIORES AL DOCUMENTO ***/
      vDocumento.keywords = new KeywordOnBaseEntity[] { keyUsuarioWS };

      /*** LLAMAR A LA FUNCIÓN DE IMPORTACIÓN ***/
      vResultadoEntity = vServicioOnBase.ImportarDocumento(vDocumento, vArchivoBase64, vOrigen);
      string vTextoResultado = string.Empty;

      vTextoResultado = vResultadoEntity.Mensaje;
      try
      {
        long vTemporal = 0;
        vTemporal = long.Parse(vTextoResultado);
        vResultado = 1;
      }
      catch (Exception)
      {
        vResultado = 0;
      }

      return vResultado;
    }

    public FlujoICRL FTraeDatosFlujoOnBase(string pNumFlujo)
    {
      vFlujoICRL = null;

      /*** CONECTAR A WS ***/
      OnBaseWS vServicioOnBase = new OnBaseWS();
      /*** ESTABLECER LA APLICACIÓN ORIGEN POR DEFECTO ICRL ***/
      SistemaOrigen vOrigen = SistemaOrigen.ICRL;
      /*** INSTANCIAR EL RESULTADO COMO ResultadoEntity ***/
      ResultadoEntity vResultadoEntity = new ResultadoEntity();
      /*** LLAMAR A LA FUNCIÓN DEL WS ***/
      vResultadoEntity = vServicioOnBase.ObtenerInformacionSolicitudOnBase(pNumFlujo, vOrigen);

      /*** SI EL RESULTADO ES CORRECTO, SE EXTRAE LA INFORMACIÓN DEL FLUJO ***/
      if (vResultadoEntity.EsValido)
      {
        vFlujoICRL = new FlujoICRL();
        // OBTENER EL OBJETO CON LA INFORMACIÓN QUE SE NECESITA
        DocumentoOnBaseEntity vDocumento = (DocumentoOnBaseEntity)vResultadoEntity.DatosAdicionales;

        // DATOS PRINCIPALES DEL DOCUMENTO (FLUJO)
        vFlujoICRL.flujoOnBase = vDocumento.nroSolicitud;

        // OBTENER LA LISTA DE LOS DATOS (KEYWORDS) DEL DOCUMENTO
        KeywordOnBaseEntity[] vListaKeywords = (KeywordOnBaseEntity[])vDocumento.keywords;

        foreach (var vKeyword in vListaKeywords)
        {
          switch (vKeyword.nombre)
          {
            case "Nombres/Razon Social":
              vFlujoICRL.nombreAsegurado = vKeyword.valor;
              break;
            case "No. de Identificacion":
              vFlujoICRL.docIdAsegurado = vKeyword.valor;
              break;
            case "Asegurado Telefono Celular":
              vFlujoICRL.telefonoCelAsegurado = vKeyword.valor;
              break;
            case "No. de Poliza":
              vFlujoICRL.numeroPoliza = vKeyword.valor;
              break;
            case "No. de Reclamo":
              try
              {
                vFlujoICRL.numeroReclamo = int.Parse(vKeyword.valor);
              }
              catch (Exception)
              {
                vFlujoICRL.numeroReclamo = 0;
              }
              break;
            case "Causa Siniestro":
              vFlujoICRL.causaSiniestro = vKeyword.valor;
              break;
            case "Marca":
              vFlujoICRL.marcaVehiculo = vKeyword.valor;
              break;
            case "Modelo":
              vFlujoICRL.modeloVehiculo = vKeyword.valor;
              break;
            case "Color":
              vFlujoICRL.colorVehiculo = vKeyword.valor;
              break;
            case "No. de Placa":
              vFlujoICRL.placaVehiculo = vKeyword.valor;
              break;
            case "No. de Chasis":
              vFlujoICRL.chasisVehiculo = vKeyword.valor;
              break;
            case "Anio":
              try
              {
                vFlujoICRL.anioVehiculo = int.Parse(vKeyword.valor);
              }
              catch (Exception)
              {
                vFlujoICRL.anioVehiculo = 0;
              }
              break;
            case "Monto Asegurado":
              try
              {
                vFlujoICRL.valorAsegurado = decimal.Parse(vKeyword.valor);
              }
              catch (Exception)
              {
                vFlujoICRL.valorAsegurado = 0;
              }
              break;
            case "Descripcion1":
              vFlujoICRL.descripcionSiniestro = vKeyword.valor;
              break;
            case "Direccion Inspeccion":
              vFlujoICRL.direccionInspeccion = vKeyword.valor;
              break;
            case "Agencia Atencion":
              vFlujoICRL.agenciaAtencion = vKeyword.valor;
              break;
            default:
              break;
          }

        }

      }

      return vFlujoICRL;
    }
    #endregion

    #region Flujo ICRL
    public int FValidaExisteFlujoICRL(string pNumFlujo)
    {
      int vRespuesta = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaFlujo = from d in db.Flujo
                          where d.flujoOnBase == pNumFlujo
                          select d;

        var vFilaTablaFlujo = vTablaFlujo.FirstOrDefault<Flujo>();

        if (null != vFilaTablaFlujo)
        {
          vRespuesta = vFilaTablaFlujo.idFlujo;
        }
      }

      return vRespuesta;
    }

    public int FGrabaFlujoICRL(FlujoICRL pFlujo)
    {
      int vRespuesta = 0;

      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          Flujo vTablaFlujo = new Flujo();

          vTablaFlujo.flujoOnBase = pFlujo.flujoOnBase;
          vTablaFlujo.estado = 0;
          vTablaFlujo.numeroReclamo = pFlujo.numeroReclamo;
          vTablaFlujo.numeroPoliza = pFlujo.numeroPoliza;
          vTablaFlujo.causaSiniestro = pFlujo.causaSiniestro;
          vTablaFlujo.marcaVehiculo = pFlujo.marcaVehiculo;
          vTablaFlujo.modeloVehiculo = pFlujo.modeloVehiculo;
          vTablaFlujo.anioVehiculo = pFlujo.anioVehiculo;
          vTablaFlujo.colorVehiculo = pFlujo.colorVehiculo;
          vTablaFlujo.placaVehiculo = pFlujo.placaVehiculo;
          vTablaFlujo.chasisVehiculo = pFlujo.chasisVehiculo;
          vTablaFlujo.valorAsegurado = pFlujo.valorAsegurado;
          vTablaFlujo.importacionDirecta = false;
          vTablaFlujo.nombreAsegurado = pFlujo.nombreAsegurado;
          vTablaFlujo.docIdAsegurado = pFlujo.docIdAsegurado;
          vTablaFlujo.telefonocelAsegurado = pFlujo.telefonoCelAsegurado;
          vTablaFlujo.descripcionSiniestro = pFlujo.descripcionSiniestro;
          vTablaFlujo.direccionInspeccion = pFlujo.direccionInspeccion;
          vTablaFlujo.agenciaAtencion = pFlujo.agenciaAtencion;
          vTablaFlujo.contador = 0;

          db.Flujo.Add(vTablaFlujo);
          db.SaveChanges();
          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaFlujoICRL(FlujoICRL pFlujo)
    {
      int vRespuesta = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        Flujo vTablaFlujo = new Flujo();

        vTablaFlujo = db.Flujo.Find(pFlujo.idFlujo);

        vTablaFlujo.flujoOnBase = pFlujo.flujoOnBase;
        vTablaFlujo.estado = 0;
        vTablaFlujo.numeroReclamo = pFlujo.numeroReclamo;
        vTablaFlujo.numeroPoliza = pFlujo.numeroPoliza;
        vTablaFlujo.causaSiniestro = pFlujo.causaSiniestro;
        vTablaFlujo.marcaVehiculo = pFlujo.marcaVehiculo;
        vTablaFlujo.modeloVehiculo = pFlujo.modeloVehiculo;
        vTablaFlujo.anioVehiculo = pFlujo.anioVehiculo;
        vTablaFlujo.colorVehiculo = pFlujo.colorVehiculo;
        vTablaFlujo.placaVehiculo = pFlujo.placaVehiculo;
        vTablaFlujo.chasisVehiculo = pFlujo.chasisVehiculo;
        vTablaFlujo.valorAsegurado = pFlujo.valorAsegurado;
        vTablaFlujo.importacionDirecta = false;
        vTablaFlujo.nombreAsegurado = pFlujo.nombreAsegurado;
        vTablaFlujo.docIdAsegurado = pFlujo.docIdAsegurado;
        vTablaFlujo.telefonocelAsegurado = pFlujo.telefonoCelAsegurado;
        vTablaFlujo.descripcionSiniestro = pFlujo.descripcionSiniestro;
        vTablaFlujo.direccionInspeccion = pFlujo.direccionInspeccion;
        vTablaFlujo.agenciaAtencion = pFlujo.agenciaAtencion;

        db.Entry(vTablaFlujo).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vRespuesta = 1;
      }

      return vRespuesta;
    }

    public int FGrabaFlujoTempICRL()
    {
      int vRespuesta = 0;
      string vFlujoTemporal = fObtieneFlujoTemp();
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          Flujo vTablaFlujo = new Flujo();

          vTablaFlujo.flujoOnBase = vFlujoTemporal;
          vTablaFlujo.estado = 0;
          vTablaFlujo.numeroReclamo = 0;
          vTablaFlujo.numeroPoliza = string.Empty;
          vTablaFlujo.causaSiniestro = string.Empty;
          vTablaFlujo.marcaVehiculo = string.Empty;
          vTablaFlujo.modeloVehiculo = string.Empty;
          vTablaFlujo.anioVehiculo = 0;
          vTablaFlujo.colorVehiculo = string.Empty;
          vTablaFlujo.placaVehiculo = string.Empty;
          vTablaFlujo.chasisVehiculo = string.Empty;
          vTablaFlujo.valorAsegurado = 0;
          vTablaFlujo.importacionDirecta = false;
          vTablaFlujo.nombreAsegurado = string.Empty;
          vTablaFlujo.docIdAsegurado = string.Empty;
          vTablaFlujo.telefonocelAsegurado = string.Empty;
          vTablaFlujo.contador = 0;
          vTablaFlujo.descripcionSiniestro = string.Empty;
          vTablaFlujo.direccionInspeccion = string.Empty;
          vTablaFlujo.agenciaAtencion = string.Empty;

          db.Flujo.Add(vTablaFlujo);
          db.SaveChanges();
          vRespuesta = vTablaFlujo.idFlujo;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }

      return vRespuesta;
    }

    public int fObtieneContadorInspeccionFlujo(int pIdFlujo)
    {
      int vIdInspeccion = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {

        System.Data.Entity.Core.Objects.ObjectParameter vIdContador = new System.Data.Entity.Core.Objects.ObjectParameter("iValorContador", typeof(int));
        db.paIncFlujoContadorInsp(pIdFlujo, vIdContador);
        vIdInspeccion = (int)vIdContador.Value;
      }

      return vIdInspeccion;
    }

    public int fActualizaLiquidacionDP(int pIdFlujo, int pIdCotizacion, string pProveedor, int pIdTipoItem)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_DP(pIdFlujo, pIdCotizacion, pProveedor, pIdTipoItem);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaLiquidacionRP(int pIdFlujo, int pIdCotizacion, string pProveedor, int pIdTipoItem)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_RP(pIdFlujo, pIdCotizacion, pProveedor, pIdTipoItem);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaLiquidacionVE(int pIdFlujo, int pIdCotizacion, string pProveedor, int pIdTipoItem)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_VE(pIdFlujo, pIdCotizacion, pProveedor, pIdTipoItem);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaLiquidacionPE(int pIdFlujo, int pIdCotizacion, string pNumeroOrden)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_PE(pIdFlujo, pIdCotizacion, pNumeroOrden);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaLiquidacionOB(int pIdFlujo, int pIdCotizacion, string pNumeroOrden)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_OB(pIdFlujo, pIdCotizacion, pNumeroOrden);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaLiquidacionTP(int pIdFlujo, int pIdCotizacion, string pNumeroOrden, double pTipoCambio)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_TP(pIdFlujo, pIdCotizacion, pNumeroOrden, pTipoCambio);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaLiquidacionTR(int pIdFlujo, int pIdCotizacion, string pNumeroOrden, double pTipoCambio)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaLiquidacion001_TR(pIdFlujo, pIdCotizacion, pNumeroOrden, pTipoCambio);
        vResultado = 1;
      }

      return vResultado;
    }

    public int fActualizaOrdenesCotiDP(int pIdFlujo, int pIdCotizacion, string pProveedor, int pIdTipoItem)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaOrdenesCotizacion_DP(pIdFlujo, pIdCotizacion, pProveedor, pIdTipoItem);
      }

      return vResultado;
    }

    public int fActualizaOrdenesCotiRP(int pIdFlujo, int pIdCotizacion, string pProveedor, int pIdTipoItem)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaOrdenesCotizacion_RP(pIdFlujo, pIdCotizacion, pProveedor, pIdTipoItem);
      }

      return vResultado;
    }

    public int fActualizaOrdenesCotiVE(int pIdFlujo, int pIdCotizacion, string pProveedor, int pIdTipoItem)
    {
      int vResultado = 0;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        db.paActualizaOrdenesCotizacion_VE(pIdFlujo, pIdCotizacion, pProveedor, pIdTipoItem);
      }

      return vResultado;
    }

    public string fObtieneFlujoTemp()
    {
      string vFlujoTemp = string.Empty;
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        long vValorContador = 0;
        System.Data.Entity.Core.Objects.ObjectParameter vContador = new System.Data.Entity.Core.Objects.ObjectParameter("biValorContador", typeof(long));
        db.paIncrementaContador("TEMP", vContador);
        vValorContador = (long)vContador.Value;
        vFlujoTemp = vValorContador.ToString() + "T";
      }

      return vFlujoTemp;
    }

    #endregion

    #region Inspeccion ICRL
    public int FGrabaInspeccionICRL(InspeccionICRL pInspeccion)
    {
      int vRespuesta = 0;

      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          Inspeccion vTablaInspeccion = new Inspeccion();

          vTablaInspeccion.idFlujo = pInspeccion.idFlujo;
          vTablaInspeccion.idUsuario = pInspeccion.idUsuario;
          vTablaInspeccion.fechaCreacion = DateTime.Now;
          vTablaInspeccion.sucursalAtencion = pInspeccion.sucursalAtencion;
          vTablaInspeccion.direccion = pInspeccion.direccion;
          vTablaInspeccion.zona = pInspeccion.zona;
          vTablaInspeccion.causaSiniestro = pInspeccion.causaSiniestro;
          vTablaInspeccion.descripcionSiniestro = pInspeccion.descripcionSiniestro;
          vTablaInspeccion.observacionesSiniestro = pInspeccion.observacionesInspec;
          vTablaInspeccion.idInspector = pInspeccion.idInspector;
          vTablaInspeccion.nombreContacto = pInspeccion.nombreContacto;
          vTablaInspeccion.telefonoContacto = pInspeccion.telefonoContacto;
          vTablaInspeccion.correosDeEnvio = pInspeccion.correosDeEnvio;
          vTablaInspeccion.recomendacionPerdidaTotal = pInspeccion.recomendacionPerdidaTotal;
          vTablaInspeccion.estado = pInspeccion.estado;
          vTablaInspeccion.tipoCobertura = pInspeccion.tipoInspeccion;
          vTablaInspeccion.correlativo = pInspeccion.correlativo;
          vTablaInspeccion.tipoTaller = pInspeccion.tipoTaller;

          db.Inspeccion.Add(vTablaInspeccion);
          db.SaveChanges();

          vRespuesta = vTablaInspeccion.idInspeccion;

        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspeccionICRL(InspeccionICRL pInspeccion)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        Inspeccion vTablaInspeccion = new Inspeccion();

        vTablaInspeccion = db.Inspeccion.Find(pInspeccion.idInspeccion);

        vTablaInspeccion.sucursalAtencion = pInspeccion.sucursalAtencion;
        vTablaInspeccion.direccion = pInspeccion.direccion;
        vTablaInspeccion.zona = pInspeccion.zona;
        vTablaInspeccion.causaSiniestro = pInspeccion.causaSiniestro;
        vTablaInspeccion.descripcionSiniestro = pInspeccion.descripcionSiniestro;
        vTablaInspeccion.observacionesSiniestro = pInspeccion.observacionesInspec;
        vTablaInspeccion.idInspector = pInspeccion.idInspector;
        vTablaInspeccion.nombreContacto = pInspeccion.nombreContacto;
        vTablaInspeccion.telefonoContacto = pInspeccion.telefonoContacto;
        vTablaInspeccion.correosDeEnvio = pInspeccion.correosDeEnvio;
        vTablaInspeccion.recomendacionPerdidaTotal = pInspeccion.recomendacionPerdidaTotal;
        vTablaInspeccion.estado = pInspeccion.estado;
        vTablaInspeccion.tipoTaller = pInspeccion.tipoTaller;

        db.Entry(vTablaInspeccion).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;

      }

      return vResultado;
    }
    #endregion

    #region Inspeccion Daños Propios Padre

    public InspeccionDaniosPropiosPadre FTraeInspDPPadreICRL(int pSecuencial, int pIdInspeccion)
    {
      InspeccionDaniosPropiosPadre vInspeccionDPPadre = new InspeccionDaniosPropiosPadre();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspDaniosPropiosPadre vTablaInspDPadre = new InspDaniosPropiosPadre();

        vTablaInspDPadre = db.InspDaniosPropiosPadre.Find(pSecuencial, pIdInspeccion);
        if (null != vTablaInspDPadre)
        {
          vInspeccionDPPadre.secuencial = vTablaInspDPadre.secuencial;
          vInspeccionDPPadre.idInspeccion = vTablaInspDPadre.idInspeccion;
          vInspeccionDPPadre.tipoTaller = vTablaInspDPadre.tipoTaller;
          vInspeccionDPPadre.cambioAPerdidaTotal = vTablaInspDPadre.cambioAPerdidaTotal;
        }
        else
          vInspeccionDPPadre = null;
      }
      return vInspeccionDPPadre;
    }

    public int FGrabaInspDaniosPropiosPadreICRL(InspeccionDaniosPropiosPadre pInspeccionDaniosPropiosPadre)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspDaniosPropiosPadre vInspDaniosPropiosPadre = new InspDaniosPropiosPadre();

          vInspDaniosPropiosPadre.idInspeccion = pInspeccionDaniosPropiosPadre.idInspeccion;
          vInspDaniosPropiosPadre.tipoTaller = pInspeccionDaniosPropiosPadre.tipoTaller;
          vInspDaniosPropiosPadre.cambioAPerdidaTotal = pInspeccionDaniosPropiosPadre.cambioAPerdidaTotal;

          db.InspDaniosPropiosPadre.Add(vInspDaniosPropiosPadre);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspDaniosPropiosPadreICRL(InspeccionDaniosPropiosPadre pInspeccionDaniosPropiosPadre)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspDaniosPropiosPadre vTablaInspDaniosPropiosPadre = new InspDaniosPropiosPadre();

        vTablaInspDaniosPropiosPadre = db.InspDaniosPropiosPadre.Find(pInspeccionDaniosPropiosPadre.secuencial, pInspeccionDaniosPropiosPadre.idInspeccion);

        vTablaInspDaniosPropiosPadre.tipoTaller = pInspeccionDaniosPropiosPadre.tipoTaller;
        vTablaInspDaniosPropiosPadre.cambioAPerdidaTotal = pInspeccionDaniosPropiosPadre.cambioAPerdidaTotal;

        db.Entry(vTablaInspDaniosPropiosPadre).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspDaniosPropiosPadreICRL(InspeccionDaniosPropiosPadre pInspeccionDaniosPropiosPadre)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspDaniosPropiosPadre vTablaInspDaniosPropiosPadre = new InspDaniosPropiosPadre();

        vTablaInspDaniosPropiosPadre = db.InspDaniosPropiosPadre.Find(pInspeccionDaniosPropiosPadre.secuencial, pInspeccionDaniosPropiosPadre.idInspeccion);

        db.InspDaniosPropiosPadre.Remove(vTablaInspDaniosPropiosPadre);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    #endregion

    #region Inspeccion Daños Propios

    public InspeccionDaniosPropios FTraeInspDPICRL(string pIdItem, int pSecuencial)
    {
      InspeccionDaniosPropios vInspeccionDP = new InspeccionDaniosPropios();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspDaniosPropios vTablaInspDP = new InspDaniosPropios();

        vTablaInspDP = db.InspDaniosPropios.Find(pIdItem, pSecuencial);
        if (null != vTablaInspDP)
        {
          vInspeccionDP.idItem = vTablaInspDP.idItem;
          vInspeccionDP.secuencial = vTablaInspDP.secuencial;
          vInspeccionDP.compra = vTablaInspDP.compra;
          vInspeccionDP.instalacion = (bool)vTablaInspDP.instalacion;
          vInspeccionDP.pintura = (bool)vTablaInspDP.pintura;
          vInspeccionDP.mecanico = (bool)vTablaInspDP.mecanico;
          vInspeccionDP.chaperio = vTablaInspDP.chaperio;
          vInspeccionDP.reparacionPrevia = vTablaInspDP.reparacionPrevia;
          vInspeccionDP.observaciones = vTablaInspDP.observaciones;

        }
        else
          vInspeccionDP = null;
      }
      return vInspeccionDP;
    }

    public int FGrabaInspDaniosPropiosICRL(InspeccionDaniosPropios pInspeccionDaniosPropios)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspDaniosPropios vInspDaniosPropios = new InspDaniosPropios();

          vInspDaniosPropios.secuencial = pInspeccionDaniosPropios.secuencial;
          vInspDaniosPropios.idItem = pInspeccionDaniosPropios.idItem;
          vInspDaniosPropios.compra = pInspeccionDaniosPropios.compra;
          vInspDaniosPropios.instalacion = pInspeccionDaniosPropios.instalacion;
          vInspDaniosPropios.pintura = pInspeccionDaniosPropios.pintura;
          vInspDaniosPropios.mecanico = pInspeccionDaniosPropios.mecanico;
          vInspDaniosPropios.chaperio = pInspeccionDaniosPropios.chaperio;
          vInspDaniosPropios.reparacionPrevia = pInspeccionDaniosPropios.reparacionPrevia;
          vInspDaniosPropios.observaciones = pInspeccionDaniosPropios.observaciones;

          db.InspDaniosPropios.Add(vInspDaniosPropios);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspDaniosPropiosICRL(InspeccionDaniosPropios pInspeccionDaniosPropios)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspDaniosPropios vTablaInspDaniosPropios = new InspDaniosPropios();

        vTablaInspDaniosPropios = db.InspDaniosPropios.Find(pInspeccionDaniosPropios.idItem, pInspeccionDaniosPropios.secuencial);

        vTablaInspDaniosPropios.compra = pInspeccionDaniosPropios.compra;
        vTablaInspDaniosPropios.instalacion = pInspeccionDaniosPropios.instalacion;
        vTablaInspDaniosPropios.pintura = pInspeccionDaniosPropios.pintura;
        vTablaInspDaniosPropios.mecanico = pInspeccionDaniosPropios.mecanico;
        vTablaInspDaniosPropios.chaperio = pInspeccionDaniosPropios.chaperio;
        vTablaInspDaniosPropios.reparacionPrevia = pInspeccionDaniosPropios.reparacionPrevia;
        vTablaInspDaniosPropios.observaciones = pInspeccionDaniosPropios.observaciones;

        db.Entry(vTablaInspDaniosPropios).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspDaniosPropiosICRL(InspeccionDaniosPropios pInspeccionDaniosPropios)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspDaniosPropios vTablaInspDaniosPropios = new InspDaniosPropios();

        vTablaInspDaniosPropios = db.InspDaniosPropios.Find(pInspeccionDaniosPropios.idItem, pInspeccionDaniosPropios.secuencial);

        db.InspDaniosPropios.Remove(vTablaInspDaniosPropios);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public string FTraeNroFlujoInspeccion(int pIdInspeccion)
    {
      int vIdFlujo = 0;
      string vNroFlujo = string.Empty;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        Inspeccion vTablaInspeccion = new Inspeccion();
        vTablaInspeccion = db.Inspeccion.Find(pIdInspeccion);
        vIdFlujo = (int)vTablaInspeccion.idFlujo;

        Flujo vTablaFlujo = new Flujo();
        vTablaFlujo = db.Flujo.Find(vIdFlujo);
        vNroFlujo = vTablaFlujo.flujoOnBase;
      }
      return vNroFlujo;
    }

    public int FTraeIdFlujoInspeccion(int pIdInspeccion)
    {
      int vIdFlujo = 0;
      string vNroFlujo = string.Empty;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        Inspeccion vTablaInspeccion = new Inspeccion();
        vTablaInspeccion = db.Inspeccion.Find(pIdInspeccion);
        vIdFlujo = (int)vTablaInspeccion.idFlujo;
      }
      return vIdFlujo;
    }

    public int FFlujoTieneDaniosPropios(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.DaniosPropios)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public int FFlujoTieneRCObjetos(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.RCObjetos)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public int FFlujoTieneRCPersonas(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.RCPersonas)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public int FFlujoTieneRCVehicular(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.RCVEhicular)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public int FFlujoTieneRoboParcial(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.RoboParcial)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public int FFlujoTienePerdidaTotDaniosPropios(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.PerdidaTotalDaniosPropios)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public int FFlujoTienePerdidaTotRobo(int pIdFlujo)
    {
      int vIdInspeccion = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspecciones = from insp in db.Inspeccion
                                 where (insp.idFlujo == pIdFlujo)
                                    && (insp.tipoCobertura == (int)TipoInspeccion.PerdidaTotalRobo)
                                 select insp;

        var vFilaTablaInspeccion = vTablaInspecciones.FirstOrDefault<Inspeccion>();

        if (null != vFilaTablaInspeccion)
        {
          vIdInspeccion = vFilaTablaInspeccion.idInspeccion;
        }
      }

      return vIdInspeccion;
    }

    public bool FInspeccionTieneDPICRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspDaniosPropios = from idpp in db.InspDaniosPropiosPadre
                                      where (idpp.idInspeccion == pIdInspeccion)
                                      select idpp;

        var vFilaTablaInspDP = vTablaInspDaniosPropios.FirstOrDefault<InspDaniosPropiosPadre>();

        if (null != vFilaTablaInspDP)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    #endregion

    #region Inspeccion RC Objetos
    public int FGrabaInspRCObjetosICRL(InspeccionRCObjeto pInspRCObjetos)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRCObjeto vInspRCObjetos = new InspRCObjeto();

          vInspRCObjetos.idInspeccion = pInspRCObjetos.idInspeccion;
          vInspRCObjetos.nombreObjeto = pInspRCObjetos.nombreObjeto;
          vInspRCObjetos.docIdentidadObjeto = pInspRCObjetos.docIdentidadObjeto;
          vInspRCObjetos.observacionesObjeto = pInspRCObjetos.observacionesObjeto;
          vInspRCObjetos.telefonoObjeto = pInspRCObjetos.telefonoObjeto;

          db.InspRCObjeto.Add(vInspRCObjetos);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRCObjetosICRL(InspeccionRCObjeto pInspRCObjetos)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCObjeto vTablaInspRCobjetos = new InspRCObjeto();

        vTablaInspRCobjetos = db.InspRCObjeto.Find(pInspRCObjetos.secuencial, pInspRCObjetos.idInspeccion);

        vTablaInspRCobjetos.nombreObjeto = pInspRCObjetos.nombreObjeto;
        vTablaInspRCobjetos.docIdentidadObjeto = pInspRCObjetos.docIdentidadObjeto;
        vTablaInspRCobjetos.observacionesObjeto = pInspRCObjetos.observacionesObjeto;
        vTablaInspRCobjetos.telefonoObjeto = pInspRCObjetos.telefonoObjeto;

        db.Entry(vTablaInspRCobjetos).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRCObjetosICRL(InspeccionRCObjeto pInspRCObjetos)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCObjeto vTablaInspRCObjetos = new InspRCObjeto();

        vTablaInspRCObjetos = db.InspRCObjeto.Find(pInspRCObjetos.secuencial, pInspRCObjetos.idInspeccion);

        db.InspRCObjeto.Remove(vTablaInspRCObjetos);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public bool FInspeccionTieneObjCRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspObjetos = from ob in db.InspRCObjeto
                                where ob.idInspeccion == pIdInspeccion
                                select ob;

        var vFilaTablaInspObj = vTablaInspObjetos.FirstOrDefault<InspRCObjeto>();

        if (null != vFilaTablaInspObj)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    public InspeccionRCObjeto FTraeInspRCObjetoICRL(int pSecuencial, int pIdInspeccion)
    {
      InspeccionRCObjeto vInspeccionRCObjeto = new InspeccionRCObjeto();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCObjeto vTablaInspRCObjeto = new InspRCObjeto();

        vTablaInspRCObjeto = db.InspRCObjeto.Find(pSecuencial, pIdInspeccion);
        if (null != vTablaInspRCObjeto)
        {
          vInspeccionRCObjeto.secuencial = vTablaInspRCObjeto.secuencial;
          vInspeccionRCObjeto.idInspeccion = vTablaInspRCObjeto.idInspeccion;
          vInspeccionRCObjeto.nombreObjeto = vTablaInspRCObjeto.nombreObjeto;
          vInspeccionRCObjeto.docIdentidadObjeto = vTablaInspRCObjeto.docIdentidadObjeto;
          vInspeccionRCObjeto.telefonoObjeto = vTablaInspRCObjeto.telefonoObjeto;
          vInspeccionRCObjeto.observacionesObjeto = vTablaInspRCObjeto.observacionesObjeto;
        }
        else
          vInspeccionRCObjeto = null;
      }
      return vInspeccionRCObjeto;
    }

    #endregion

    #region Inspeccion RC Objetos Detalle
    public int FGrabaInspRCObjetoDetICRL(InspeccionRCObjetoDet pInspRCObjetoDetalle)
    {
      int vRespuesta = 0;

      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRCObjetoDetalle vInspRCObjetoDet = new InspRCObjetoDetalle();

          vInspRCObjetoDet.secuencial = pInspRCObjetoDetalle.secuencial;
          vInspRCObjetoDet.idItem = pInspRCObjetoDetalle.itemObjDet;
          vInspRCObjetoDet.costoReferencial = pInspRCObjetoDetalle.costoRefObjDet;
          vInspRCObjetoDet.descripcion = pInspRCObjetoDetalle.descripObjDet;

          db.InspRCObjetoDetalle.Add(vInspRCObjetoDet);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRCObjetoDetICRL(InspeccionRCObjetoDet pInspRCObjetoDetalle)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCObjetoDetalle vInspRCObjetoDet = new InspRCObjetoDetalle();

        vInspRCObjetoDet = db.InspRCObjetoDetalle.Find(pInspRCObjetoDetalle.itemObjDet, pInspRCObjetoDetalle.secuencial);

        vInspRCObjetoDet.costoReferencial = pInspRCObjetoDetalle.costoRefObjDet;
        vInspRCObjetoDet.descripcion = pInspRCObjetoDetalle.descripObjDet;

        db.Entry(vInspRCObjetoDet).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRCObjetoDetICRL(InspeccionRCObjetoDet pInspRCObjetoDetalle)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCObjetoDetalle vInspRCObjetoDet = new InspRCObjetoDetalle();

        vInspRCObjetoDet = db.InspRCObjetoDetalle.Find(pInspRCObjetoDetalle.itemObjDet, pInspRCObjetoDetalle.secuencial);

        db.InspRCObjetoDetalle.Remove(vInspRCObjetoDet);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public bool FInspeccionTieneObjDetCRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspObjetos = from ob in db.InspRCObjeto
                                where ob.idInspeccion == pIdInspeccion
                                select ob;

        var vFilaTablaInspObj = vTablaInspObjetos.FirstOrDefault<InspRCObjeto>();

        if (null != vFilaTablaInspObj)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }
    #endregion

    #region Inspeccion RC Persona
    public int FGrabaInspRCPersonasICRL(InspeccionRCPersona pInspRCPersonas)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRCPersona vInspRCPersonas = new InspRCPersona();

          vInspRCPersonas.idInspeccion = pInspRCPersonas.idInspeccion;
          vInspRCPersonas.nombrePersona = pInspRCPersonas.nombrePersona;
          vInspRCPersonas.docIdentidadPersona = pInspRCPersonas.docIdentidadPersona;
          vInspRCPersonas.observacionesPersona = pInspRCPersonas.observacionesPersona;
          vInspRCPersonas.telefonoPersona = pInspRCPersonas.telefonoPersona;

          db.InspRCPersona.Add(vInspRCPersonas);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRCPersonasICRL(InspeccionRCPersona pInspRCPersonas)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCPersona vTablaInspRCPersonas = new InspRCPersona();

        vTablaInspRCPersonas = db.InspRCPersona.Find(pInspRCPersonas.secuencial, pInspRCPersonas.idInspeccion);

        vTablaInspRCPersonas.nombrePersona = pInspRCPersonas.nombrePersona;
        vTablaInspRCPersonas.docIdentidadPersona = pInspRCPersonas.docIdentidadPersona;
        vTablaInspRCPersonas.observacionesPersona = pInspRCPersonas.observacionesPersona;
        vTablaInspRCPersonas.telefonoPersona = pInspRCPersonas.telefonoPersona;

        db.Entry(vTablaInspRCPersonas).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRCPersonasICRL(InspeccionRCPersona pInspRCPersonas)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCPersona vTablaInspRCPersonas = new InspRCPersona();

        vTablaInspRCPersonas = db.InspRCPersona.Find(pInspRCPersonas.secuencial, pInspRCPersonas.idInspeccion);

        db.InspRCPersona.Remove(vTablaInspRCPersonas);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public bool FInspeccionTienePerCRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspPersonas = from per in db.InspRCPersona
                                 where per.idInspeccion == pIdInspeccion
                                 select per;

        var vFilaTablaInspPer = vTablaInspPersonas.FirstOrDefault<InspRCPersona>();

        if (null != vFilaTablaInspPer)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    public InspeccionRCPersona FTraeInspRCPersonaICRL(int pSecuencial, int pIdInspeccion)
    {
      InspeccionRCPersona vInspeccionRCPersona = new InspeccionRCPersona();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCPersona vTablaInspRCPersona = new InspRCPersona();

        vTablaInspRCPersona = db.InspRCPersona.Find(pSecuencial, pIdInspeccion);
        if (null != vTablaInspRCPersona)
        {
          vInspeccionRCPersona.secuencial = vTablaInspRCPersona.secuencial;
          vInspeccionRCPersona.idInspeccion = vTablaInspRCPersona.idInspeccion;
          vInspeccionRCPersona.nombrePersona = vTablaInspRCPersona.nombrePersona;
          vInspeccionRCPersona.docIdentidadPersona = vTablaInspRCPersona.docIdentidadPersona;
          vInspeccionRCPersona.telefonoPersona = vTablaInspRCPersona.telefonoPersona;
          vInspeccionRCPersona.observacionesPersona = vTablaInspRCPersona.observacionesPersona;
        }
        else
          vInspeccionRCPersona = null;
      }
      return vInspeccionRCPersona;
    }

    #endregion

    #region Inspeccion RC Persona Detalle

    public int FGrabaInspRCPersonaDetICRL(InspeccionRCPersonaDet pInspRCPersonaDetalle)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRCPersonaDetalle vInspRCPersonaDet = new InspRCPersonaDetalle();

          vInspRCPersonaDet.secuencial = pInspRCPersonaDetalle.secuencial;
          vInspRCPersonaDet.tipo = pInspRCPersonaDetalle.tipoPerDet;
          vInspRCPersonaDet.montoGasto = pInspRCPersonaDetalle.montoGastoPerDet;
          vInspRCPersonaDet.descripcion = pInspRCPersonaDetalle.descripPerDet;

          db.InspRCPersonaDetalle.Add(vInspRCPersonaDet);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRCPersonaDetICRL(InspeccionRCPersonaDet pInspRCPersonaDetalle)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCPersonaDetalle vInspRCPersonaDet = new InspRCPersonaDetalle();

        vInspRCPersonaDet = db.InspRCPersonaDetalle.Find(pInspRCPersonaDetalle.tipoPerDet, pInspRCPersonaDetalle.secuencial);

        vInspRCPersonaDet.montoGasto = pInspRCPersonaDetalle.montoGastoPerDet;
        vInspRCPersonaDet.descripcion = pInspRCPersonaDetalle.descripPerDet;

        db.Entry(vInspRCPersonaDet).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRCPersonaDetICRL(InspeccionRCPersonaDet pInspRCPersonaDetalle)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCPersonaDetalle vInspRCPersonaDet = new InspRCPersonaDetalle();

        vInspRCPersonaDet = db.InspRCPersonaDetalle.Find(pInspRCPersonaDetalle.tipoPerDet, pInspRCPersonaDetalle.secuencial);

        db.InspRCPersonaDetalle.Remove(vInspRCPersonaDet);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public bool FInspeccionTienePerDetCRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspPersonas = from per in db.InspRCPersona
                                 where per.idInspeccion == pIdInspeccion
                                 select per;

        var vFilaTablaInspPer = vTablaInspPersonas.FirstOrDefault<InspRCPersona>();

        if (null != vFilaTablaInspPer)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    public int FGrabaInspRoboParcialICRL(InspeccionRoboParcial pInspRoboParcial)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRoboParcial vInspRoboParcial = new InspRoboParcial();

          vInspRoboParcial.idInspeccion = pInspRoboParcial.idInspeccion;
          vInspRoboParcial.idItem = pInspRoboParcial.item;
          vInspRoboParcial.compra = pInspRoboParcial.compra;
          vInspRoboParcial.instalacion = pInspRoboParcial.instalacion;
          vInspRoboParcial.pintura = pInspRoboParcial.pintura;
          vInspRoboParcial.mecanico = pInspRoboParcial.mecanico;
          vInspRoboParcial.chaperio = pInspRoboParcial.chaperio;
          vInspRoboParcial.reparacionPrevia = pInspRoboParcial.reparacionPrevia;
          vInspRoboParcial.observaciones = pInspRoboParcial.observaciones;

          db.InspRoboParcial.Add(vInspRoboParcial);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRoboParcialICRL(InspeccionRoboParcial pInspRoboParcial)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRoboParcial vTablaInspRoboParcial = new InspRoboParcial();

        vTablaInspRoboParcial = db.InspRoboParcial.Find(pInspRoboParcial.item, pInspRoboParcial.idInspeccion);

        vTablaInspRoboParcial.compra = pInspRoboParcial.compra;
        vTablaInspRoboParcial.instalacion = pInspRoboParcial.instalacion;
        vTablaInspRoboParcial.pintura = pInspRoboParcial.pintura;
        vTablaInspRoboParcial.mecanico = pInspRoboParcial.mecanico;
        vTablaInspRoboParcial.chaperio = pInspRoboParcial.chaperio;
        vTablaInspRoboParcial.reparacionPrevia = pInspRoboParcial.reparacionPrevia;
        vTablaInspRoboParcial.observaciones = pInspRoboParcial.observaciones;

        db.Entry(vTablaInspRoboParcial).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRoboParcialICRL(InspeccionRoboParcial pInspRoboParcial)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRoboParcial vTablaInspRoboParcial = new InspRoboParcial();

        vTablaInspRoboParcial = db.InspRoboParcial.Find(pInspRoboParcial.item, pInspRoboParcial.idInspeccion);

        db.InspRoboParcial.Remove(vTablaInspRoboParcial);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public bool FInspeccionTieneRPICRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspRoboParcial = from rp in db.InspRoboParcial
                                    where rp.idInspeccion == pIdInspeccion
                                    select rp;

        var vFilaTablaInspRP = vTablaInspRoboParcial.FirstOrDefault<InspRoboParcial>();

        if (null != vFilaTablaInspRP)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    #endregion

    #region Inspeccion Perdida Total Danios Propios

    public bool FInspeccionTienePTDPICRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspPerdidatotalDP = from ptDP in db.InspPerdidaTotalDanios
                                       where ptDP.idInspeccion == pIdInspeccion
                                       select ptDP;

        var vFilaTablaInspPerdidaTotDP = vTablaInspPerdidatotalDP.FirstOrDefault<InspPerdidaTotalDanios>();

        if (null != vFilaTablaInspPerdidaTotDP)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    public int FGrabaInspPerdidaTotalPTDPICRL(InspeccionPTDaniosPropios pInspPerdidaTotalPTDP)
    {
      int vRespuesta = 0;
      try
      {

        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspPerdidaTotalDanios vInspPerdidaTotalPTDP = new InspPerdidaTotalDanios();

          vInspPerdidaTotalPTDP.idInspeccion = pInspPerdidaTotalPTDP.idInspeccion;
          vInspPerdidaTotalPTDP.version = pInspPerdidaTotalPTDP.version;
          vInspPerdidaTotalPTDP.serie = pInspPerdidaTotalPTDP.serie;
          vInspPerdidaTotalPTDP.caja = pInspPerdidaTotalPTDP.caja;
          vInspPerdidaTotalPTDP.combustible = pInspPerdidaTotalPTDP.combustible;
          vInspPerdidaTotalPTDP.cilindrada = pInspPerdidaTotalPTDP.cilindrada;
          vInspPerdidaTotalPTDP.techoSolar = pInspPerdidaTotalPTDP.techoSolar;
          vInspPerdidaTotalPTDP.asientosCuero = pInspPerdidaTotalPTDP.asientosCuero;
          vInspPerdidaTotalPTDP.arosMagnesio = pInspPerdidaTotalPTDP.arosMagnesio;
          vInspPerdidaTotalPTDP.convertidoGNV = pInspPerdidaTotalPTDP.convertidoGNV;
          vInspPerdidaTotalPTDP.observaciones = pInspPerdidaTotalPTDP.observaciones;

          db.InspPerdidaTotalDanios.Add(vInspPerdidaTotalPTDP);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspPerdidaTotalPTDPICRL(InspeccionPTDaniosPropios pInspPerdidaTotalPTDP)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspPerdidaTotalDanios vInspPerdidaTotalPTDP = new InspPerdidaTotalDanios();

        vInspPerdidaTotalPTDP = db.InspPerdidaTotalDanios.Find(pInspPerdidaTotalPTDP.idInspeccion);

        vInspPerdidaTotalPTDP.version = pInspPerdidaTotalPTDP.version;
        vInspPerdidaTotalPTDP.serie = pInspPerdidaTotalPTDP.serie;
        vInspPerdidaTotalPTDP.caja = pInspPerdidaTotalPTDP.caja;
        vInspPerdidaTotalPTDP.combustible = pInspPerdidaTotalPTDP.combustible;
        vInspPerdidaTotalPTDP.cilindrada = pInspPerdidaTotalPTDP.cilindrada;
        vInspPerdidaTotalPTDP.techoSolar = pInspPerdidaTotalPTDP.techoSolar;
        vInspPerdidaTotalPTDP.asientosCuero = pInspPerdidaTotalPTDP.asientosCuero;
        vInspPerdidaTotalPTDP.arosMagnesio = pInspPerdidaTotalPTDP.arosMagnesio;
        vInspPerdidaTotalPTDP.convertidoGNV = pInspPerdidaTotalPTDP.convertidoGNV;
        vInspPerdidaTotalPTDP.observaciones = pInspPerdidaTotalPTDP.observaciones;

        db.Entry(vInspPerdidaTotalPTDP).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspPerdidaTotalPTDPICRL(InspeccionPTDaniosPropios pInspPerdidaTotalPTDP)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspPerdidaTotalDanios vTablaInspPerdidaTotalPTDP = new InspPerdidaTotalDanios();

        vTablaInspPerdidaTotalPTDP = db.InspPerdidaTotalDanios.Find(pInspPerdidaTotalPTDP.idInspeccion);

        db.InspPerdidaTotalDanios.Remove(vTablaInspPerdidaTotalPTDP);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    #endregion

    #region Inspeccion Perdida Total por Robo

    public bool FInspeccionTienePTROICRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspPerdidatotalRO = from ptRO in db.InspPerdidaTotalRobo
                                       where ptRO.idInspeccion == pIdInspeccion
                                       select ptRO;

        var vFilaTablaInspPerdidaTotRO = vTablaInspPerdidatotalRO.FirstOrDefault<InspPerdidaTotalRobo>();

        if (null != vFilaTablaInspPerdidaTotRO)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    public int FGrabaInspPerdidaTotalPTROICRL(InspeccionPTRobo pInspPerdidaTotalPTRO)
    {
      int vRespuesta = 0;

      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspPerdidaTotalRobo vInspPerdidaTotalPTRO = new InspPerdidaTotalRobo();

          vInspPerdidaTotalPTRO.idInspeccion = pInspPerdidaTotalPTRO.idInspeccion;
          vInspPerdidaTotalPTRO.version = pInspPerdidaTotalPTRO.version;
          vInspPerdidaTotalPTRO.serie = pInspPerdidaTotalPTRO.serie;
          vInspPerdidaTotalPTRO.caja = pInspPerdidaTotalPTRO.caja;
          vInspPerdidaTotalPTRO.combustible = pInspPerdidaTotalPTRO.combustible;
          vInspPerdidaTotalPTRO.cilindrada = pInspPerdidaTotalPTRO.cilindrada;
          vInspPerdidaTotalPTRO.techoSolar = pInspPerdidaTotalPTRO.techoSolar;
          vInspPerdidaTotalPTRO.asientosCuero = pInspPerdidaTotalPTRO.asientosCuero;
          vInspPerdidaTotalPTRO.arosMagnesio = pInspPerdidaTotalPTRO.arosMagnesio;
          vInspPerdidaTotalPTRO.convertidoGNV = pInspPerdidaTotalPTRO.convertidoGNV;
          vInspPerdidaTotalPTRO.observaciones = pInspPerdidaTotalPTRO.observaciones;

          db.InspPerdidaTotalRobo.Add(vInspPerdidaTotalPTRO);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspPerdidaTotalPTROICRL(InspeccionPTRobo pInspPerdidaTotalPTRO)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspPerdidaTotalRobo vInspPerdidaTotalPTRO = new InspPerdidaTotalRobo();

        vInspPerdidaTotalPTRO = db.InspPerdidaTotalRobo.Find(pInspPerdidaTotalPTRO.idInspeccion);

        vInspPerdidaTotalPTRO.version = pInspPerdidaTotalPTRO.version;
        vInspPerdidaTotalPTRO.serie = pInspPerdidaTotalPTRO.serie;
        vInspPerdidaTotalPTRO.caja = pInspPerdidaTotalPTRO.caja;
        vInspPerdidaTotalPTRO.combustible = pInspPerdidaTotalPTRO.combustible;
        vInspPerdidaTotalPTRO.cilindrada = pInspPerdidaTotalPTRO.cilindrada;
        vInspPerdidaTotalPTRO.techoSolar = pInspPerdidaTotalPTRO.techoSolar;
        vInspPerdidaTotalPTRO.asientosCuero = pInspPerdidaTotalPTRO.asientosCuero;
        vInspPerdidaTotalPTRO.arosMagnesio = pInspPerdidaTotalPTRO.arosMagnesio;
        vInspPerdidaTotalPTRO.convertidoGNV = pInspPerdidaTotalPTRO.convertidoGNV;
        vInspPerdidaTotalPTRO.observaciones = pInspPerdidaTotalPTRO.observaciones;

        db.Entry(vInspPerdidaTotalPTRO).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspPerdidaTotalPTROICRL(InspeccionPTRobo pInspPerdidaTotalPTRO)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspPerdidaTotalRobo vTablaInspPerdidaTotalPTRO = new InspPerdidaTotalRobo();

        vTablaInspPerdidaTotalPTRO = db.InspPerdidaTotalRobo.Find(pInspPerdidaTotalPTRO.idInspeccion);

        db.InspPerdidaTotalRobo.Remove(vTablaInspPerdidaTotalPTRO);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    #endregion

    #region RC Vehicular01

    public int FGrabaInspRCVehicularICRL(InspeccionRCVehicular pInspeccionRCVehicular)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRCVehicular vInspRCVehicular = new InspRCVehicular();

          vInspRCVehicular.idInspeccion = pInspeccionRCVehicular.idInspeccion;
          vInspRCVehicular.nombreTercero = pInspeccionRCVehicular.nombreTercero;
          vInspRCVehicular.docIdentidadTercero = pInspeccionRCVehicular.docIdentidadTercero;
          vInspRCVehicular.telefonoTercero = pInspeccionRCVehicular.telefonoTercero;
          vInspRCVehicular.marca = pInspeccionRCVehicular.marca;
          vInspRCVehicular.modelo = pInspeccionRCVehicular.modelo;
          vInspRCVehicular.anio = pInspeccionRCVehicular.anio;
          vInspRCVehicular.placa = pInspeccionRCVehicular.placa;
          vInspRCVehicular.color = pInspeccionRCVehicular.color;
          vInspRCVehicular.chasis = pInspeccionRCVehicular.chasis;
          vInspRCVehicular.kilometraje = pInspeccionRCVehicular.kilometraje;
          vInspRCVehicular.importacionDirecta = pInspeccionRCVehicular.importacionDirecta;
          vInspRCVehicular.tipoTaller = pInspeccionRCVehicular.tipoTaller;

          db.InspRCVehicular.Add(vInspRCVehicular);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRCVehicularICRL(InspeccionRCVehicular pInspeccionRCVehicular)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCVehicular vTablaInspRCVehicular01 = new InspRCVehicular();

        vTablaInspRCVehicular01 = db.InspRCVehicular.Find(pInspeccionRCVehicular.secuencial, pInspeccionRCVehicular.idInspeccion);

        vTablaInspRCVehicular01.nombreTercero = pInspeccionRCVehicular.nombreTercero;
        vTablaInspRCVehicular01.docIdentidadTercero = pInspeccionRCVehicular.docIdentidadTercero;
        vTablaInspRCVehicular01.telefonoTercero = pInspeccionRCVehicular.telefonoTercero;
        vTablaInspRCVehicular01.marca = pInspeccionRCVehicular.marca;
        vTablaInspRCVehicular01.modelo = pInspeccionRCVehicular.modelo;
        vTablaInspRCVehicular01.anio = pInspeccionRCVehicular.anio;
        vTablaInspRCVehicular01.placa = pInspeccionRCVehicular.placa;
        vTablaInspRCVehicular01.color = pInspeccionRCVehicular.color;
        vTablaInspRCVehicular01.chasis = pInspeccionRCVehicular.chasis;
        vTablaInspRCVehicular01.kilometraje = pInspeccionRCVehicular.kilometraje;
        vTablaInspRCVehicular01.importacionDirecta = pInspeccionRCVehicular.importacionDirecta;
        vTablaInspRCVehicular01.tipoTaller = pInspeccionRCVehicular.tipoTaller;

        db.Entry(vTablaInspRCVehicular01).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRCVehicularICRL(InspeccionRCVehicular pInspeccionRCVehicular)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCVehicular vTablaInspRCVehicular = new InspRCVehicular();

        vTablaInspRCVehicular = db.InspRCVehicular.Find(pInspeccionRCVehicular.secuencial, pInspeccionRCVehicular.idInspeccion);

        db.InspRCVehicular.Remove(vTablaInspRCVehicular);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public InspeccionRCVehicular FTraeInspRCVehicularICRL(int pSecuencial, int pIdInspeccion)
    {
      InspeccionRCVehicular vInspeccionRCVehicular = new InspeccionRCVehicular();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCVehicular vTablaInspRCVehicular01 = new InspRCVehicular();

        vTablaInspRCVehicular01 = db.InspRCVehicular.Find(pSecuencial, pIdInspeccion);
        if (null != vTablaInspRCVehicular01)
        {
          vInspeccionRCVehicular.secuencial = vTablaInspRCVehicular01.secuencial;
          vInspeccionRCVehicular.idInspeccion = vTablaInspRCVehicular01.idInspeccion;
          vInspeccionRCVehicular.nombreTercero = vTablaInspRCVehicular01.nombreTercero;
          vInspeccionRCVehicular.docIdentidadTercero = vTablaInspRCVehicular01.docIdentidadTercero;
          vInspeccionRCVehicular.telefonoTercero = vTablaInspRCVehicular01.telefonoTercero;
          vInspeccionRCVehicular.marca = vTablaInspRCVehicular01.marca;
          vInspeccionRCVehicular.modelo = vTablaInspRCVehicular01.modelo;
          vInspeccionRCVehicular.anio = (int)vTablaInspRCVehicular01.anio;
          vInspeccionRCVehicular.placa = vTablaInspRCVehicular01.placa;
          vInspeccionRCVehicular.color = vTablaInspRCVehicular01.color;
          vInspeccionRCVehicular.chasis = vTablaInspRCVehicular01.chasis;
          vInspeccionRCVehicular.kilometraje = (int)vTablaInspRCVehicular01.kilometraje;
          vInspeccionRCVehicular.importacionDirecta = (bool)vTablaInspRCVehicular01.importacionDirecta;
          vInspeccionRCVehicular.tipoTaller = vTablaInspRCVehicular01.tipoTaller;
        }
        else
          vInspeccionRCVehicular = null;
      }
      return vInspeccionRCVehicular;
    }

    public bool FInspeccionTieneRCVehicularICRL(int pIdInspeccion)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspRCVehicular = from rcv in db.InspRCVehicular
                                    where rcv.idInspeccion == pIdInspeccion
                                    select rcv;

        var vFilaTablaInspPer = vTablaInspRCVehicular.FirstOrDefault<InspRCVehicular>();

        if (null != vFilaTablaInspPer)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    #endregion

    #region Inspeccion RC Vehicular Detalle

    public int FGrabaInspRCV01DetICRL(InspeccionRCVehicularDet pInspeccionRCVDet)
    {
      int vRespuesta = 0;
      try
      {
        using (LBCDesaEntities db = new LBCDesaEntities())
        {
          InspRCVehicularDetalle vInspRCVDet = new InspRCVehicularDetalle();

          vInspRCVDet.idItem = pInspeccionRCVDet.idItem;
          vInspRCVDet.secuencial = pInspeccionRCVDet.secuencial;
          vInspRCVDet.compra = pInspeccionRCVDet.compra;
          vInspRCVDet.instalacion = pInspeccionRCVDet.instalacion;
          vInspRCVDet.pintura = pInspeccionRCVDet.pintura;
          vInspRCVDet.mecanico = pInspeccionRCVDet.mecanico;
          vInspRCVDet.chaperio = pInspeccionRCVDet.chaperio;
          vInspRCVDet.reparacionPrevia = pInspeccionRCVDet.reparacionPrevia;
          vInspRCVDet.observaciones = pInspeccionRCVDet.observaciones;

          db.InspRCVehicularDetalle.Add(vInspRCVDet);
          db.SaveChanges();

          vRespuesta = 1;
        }
      }
      catch (Exception ex)
      {
        vRespuesta = 0;
      }
      return vRespuesta;
    }

    public int FActualizaInspRCV01DetICRL(InspeccionRCVehicularDet pInspeccionRCVDet)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCVehicularDetalle vTablaInspRCVDet = new InspRCVehicularDetalle();

        vTablaInspRCVDet = db.InspRCVehicularDetalle.Find(pInspeccionRCVDet.idItem, pInspeccionRCVDet.secuencial);

        vTablaInspRCVDet.compra = pInspeccionRCVDet.compra;
        vTablaInspRCVDet.instalacion = pInspeccionRCVDet.instalacion;
        vTablaInspRCVDet.pintura = pInspeccionRCVDet.pintura;
        vTablaInspRCVDet.mecanico = pInspeccionRCVDet.mecanico;
        vTablaInspRCVDet.chaperio = pInspeccionRCVDet.chaperio;
        vTablaInspRCVDet.reparacionPrevia = pInspeccionRCVDet.reparacionPrevia;
        vTablaInspRCVDet.observaciones = pInspeccionRCVDet.observaciones;

        db.Entry(vTablaInspRCVDet).State = System.Data.Entity.EntityState.Modified;
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public int FBorrarInspRCV01DetICRL(InspeccionRCVehicularDet pInspeccionRCVDet)
    {
      int vResultado = 0;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        InspRCVehicularDetalle vTablaInspRCVDet = new InspRCVehicularDetalle();

        vTablaInspRCVDet = db.InspRCVehicularDetalle.Find(pInspeccionRCVDet.idItem, pInspeccionRCVDet.secuencial);

        db.InspRCVehicularDetalle.Remove(vTablaInspRCVDet);
        db.SaveChanges();

        vResultado = 1;
      }
      return vResultado;
    }

    public bool FInspeccionTieneRCVDetICRL(int pSecuencia)
    {
      bool vRespuesta = false;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vTablaInspRCVDet = from rcvdet in db.InspRCVehicularDetalle
                               where rcvdet.secuencial == pSecuencia
                               select rcvdet;

        var vFilaTablaInspRCVDet = vTablaInspRCVDet.FirstOrDefault<InspRCVehicularDetalle>();

        if (null != vFilaTablaInspRCVDet)
        {
          vRespuesta = true;
        }
      }

      return vRespuesta;
    }

    #endregion

    #region Auxiliar

    public List<Inspeccion> TraeInspeccionesPorFlujo(int pFlujo)
    {
      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        var vListaInspeccion = db.Inspeccion
            .Where(x => x.idFlujo == pFlujo)
            .ToList();

        return vListaInspeccion;
      }

    }

    public List<ListaNomenclador> FlTraeNomenGenerico(string pCategoria, int pOrdenCodDescripcion)
    {
      List<ListaNomenclador> vListaNomenclador = new List<ListaNomenclador>();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        if (1 == pOrdenCodDescripcion)
        {
          vListaNomenclador = (from n in db.Nomenclador
                               where n.categoriaNomenclador == pCategoria
                               orderby n.codigo
                               select new ListaNomenclador
                               {
                                 codigo = n.codigo,
                                 descripcion = n.descripcion
                               }).ToList();
        }
        else
        {
          vListaNomenclador = (from n in db.Nomenclador
                               where n.categoriaNomenclador == pCategoria
                               orderby n.descripcion
                               select new ListaNomenclador
                               {
                                 codigo = n.codigo,
                                 descripcion = n.descripcion
                               }).ToList();
        }


      }

      return vListaNomenclador;
    }

    public List<ListaNomenclador> FlTraeNomenGenericoDesc(string pCategoria, int pOrdenCodDescripcion)
    {
      List<ListaNomenclador> vListaNomenclador = new List<ListaNomenclador>();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        if (1 == pOrdenCodDescripcion)
        {
          vListaNomenclador = (from n in db.Nomenclador
                               where n.categoriaNomenclador == pCategoria
                               orderby n.codigo descending
                               select new ListaNomenclador
                               {
                                 codigo = n.codigo,
                                 descripcion = n.descripcion
                               }).ToList();
        }
        else
        {
          vListaNomenclador = (from n in db.Nomenclador
                               where n.categoriaNomenclador == pCategoria
                               orderby n.descripcion descending
                               select new ListaNomenclador
                               {
                                 codigo = n.codigo,
                                 descripcion = n.descripcion
                               }).ToList();
        }


      }

      return vListaNomenclador;
    }

    public Inspeccion FTraeDatosBasicosInspeccion(int pIdInspeccion)
    {
      Inspeccion vInspeccion = new Inspeccion();

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        vInspeccion = db.Inspeccion.Find(pIdInspeccion);

      }
      return vInspeccion;
    }
    #endregion


    #region Cotizacion
    public int FTraeIdFlujoCotizacion(int pIdCotizacion)
    {
      int vIdFlujo = 0;
      string vNroFlujo = string.Empty;

      using (LBCDesaEntities db = new LBCDesaEntities())
      {
        Cotizacion vTablaCotizacion = new Cotizacion();
        vTablaCotizacion = db.Cotizacion.Find(pIdCotizacion);
        vIdFlujo = (int)vTablaCotizacion.idFlujo;
      }
      return vIdFlujo;
    }
    #endregion

  }
}