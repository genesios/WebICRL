<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GestionLiquidacion.aspx.cs" Inherits="IRCL.Presentacion.GestionLiquidacion" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="height: 178px">
            <div>
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
            </div>
            <div>
                <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
            </div>

        </div>
    </form>
</body>
</html>
