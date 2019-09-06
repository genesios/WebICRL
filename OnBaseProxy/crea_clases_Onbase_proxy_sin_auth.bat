
REM La URL del Servidor de desarollo es http://desanilus.lbc.bo/Nilus/WsOnbase

REM la herramienta que genera la clase del Proxy es wsdl.exe
"C:\Program Files (x86)\Microsoft SDKs\Windows\v10.0A\bin\NETFX 4.6.1 Tools\wsdl.exe" /l:CS /n:LbcOnBaseWS /o:LbcOnBaseWS.cs  http://desanilus.lbc.bo/Nilus/WsOnbase/OnBaseWS.asmx?WSDL