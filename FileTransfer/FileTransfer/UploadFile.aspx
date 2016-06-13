<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="uploadFile.aspx.cs" Inherits="Testing.uploadFile" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Upload a file</h2>
<p>
    <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="Button" />
</p>
    <p>&nbsp;
        <asp:Label ID="Label2" runat="server" Text="Username: "></asp:Label>
&nbsp;:&nbsp;
        <asp:TextBox ID="Username" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:FileUpload ID="FileUpload1" runat="server" />
    </p>
    <p>

        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Upload" />

    </p>
    <p>

        <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>

    </p>
</asp:Content>