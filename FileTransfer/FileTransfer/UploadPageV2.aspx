<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UploadPageV2.aspx.cs" Inherits="FileTransfer.UploadPageV2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Ajax file upload</h2>
<p>
    <asp:Label ID="Label1" runat="server" Text="Username:"></asp:Label>
    <asp:TextBox ID="Username" runat="server"></asp:TextBox>
</p>
    <ajaxToolkit:AjaxFileUpload ID="AjaxFileUpload1" runat="server" MaximumNumberOfFiles="5" OnUploadComplete="AjaxFileUploadEvent"/>
</asp:Content>
