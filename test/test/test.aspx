<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="test.aspx.cs" Inherits="test.test" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="show" Text="Popup" runat="server" onclick="showpopup"/>
    <asp:Panel ID="panel1" runat="server" style="display:none">
        <asp:label runat="server">hello world!</asp:label>
        <asp:Button ID="ok" Text="ok" runat="server"/>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" OkControlID="ok" PopupControlID="panel1" TargetControlID="show"></ajaxToolkit:ModalPopupExtender>
</asp:Content>
