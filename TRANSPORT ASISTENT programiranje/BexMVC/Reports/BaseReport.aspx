﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BaseReport.aspx.cs" Inherits="BexMVC.Reports.BaseReport" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Razduzenje Report</title>
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server"
            Width="100%"
            Height="700px">
        </rsweb:ReportViewer>
    </form>
</body>
</html>

