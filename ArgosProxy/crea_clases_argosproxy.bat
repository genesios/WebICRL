
REM La URL del Servidor de desarollo es http://desarrollo.lbc.bo/Servicios/generales 

REM la herramienta que genera la clase del Proxy es wsdl.exe
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\wsdl.exe" /l:CS /n:LbcConsultaUsuarioSistema /o:LbcConsultaUsuarioSistema.cs  http://desarrollo.lbc.bo/Servicios/generales/ConsultaUsuarioSistema.asmx?WSDL