<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Retrieval.aspx.cs" Inherits="FileTransfer.Retrieval" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <h2>Retrieval</h2>
    <p>&nbsp;<asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        :&nbsp;&nbsp;
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Retrieve" />
    </p>
    <p>
        <asp:ListBox ID="ListBox1" runat="server" Height="150px" Width="450px"></asp:ListBox>
    </p>
    <p>&nbsp;</p>
</asp:Content>
