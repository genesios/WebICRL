<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ICRL.Acceso.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="~/Estilo/Formatos.css" rel="stylesheet" type="text/css" />
    <link href="~/Estilo/estilo.css" rel="stylesheet" type="text/css" />
    <style>
        /*.container {
            width: 60%;
            display: block;
            margin: 0 auto;
        }

        .header {
            text-align: center;
            padding: 15px;
            background-color:darkseagreen;
            color: #fff;
            font-weight: bold;
            font-size: 2em;
        }

        .body {
            width: 100%;
        }

        table {
            width: 100%;
        }

            table tr td.col1 {
                width: 30%;
            }

            table tr td.col2 {
                width: 70%;
            }

        .foot {
            text-align: center;
            margin: 10px;
        }

        .innerbody {
            display: block;
            width: 80%;
            margin: 0 auto;
        }*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="logincontainer">
            <table class="basetable">
              <tr><th colspan="2">ACCESO SISTEMA</th></tr>
              <tr><td>
                <div>
                  <asp:Label ID="Label2" runat="server" Text="Usuario" ></asp:Label><br />
                  <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </div>
                <div>
                  <asp:Label ID="Label5" runat="server" Text="Contraseña"></asp:Label><br />
                  <asp:TextBox ID="TextBox2" runat="server" TextMode="Password"></asp:TextBox>
                </div>
              </td></tr>
              <tr><td style="text-align:center"><asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Ingresar" /></td></tr>
              <tr><td style="text-align:center" class="errormessage"><asp:Label ID="Label4" runat="server"></asp:Label></td></tr>
            </table>
        </div>
    </form>
</body>
</html>
