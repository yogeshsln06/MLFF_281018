<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="VaaaN.MLFF.WebApplication.Reports.ReportViewer" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="sm" runat="server" />
            <rsweb:reportviewer id="ReportViewer1" runat="server" asyncrendering="false" width="945" Height="950" ></rsweb:reportviewer>
        </div>
    </form>
</body>
</html>

