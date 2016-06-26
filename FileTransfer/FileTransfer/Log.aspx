<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="FileTransfer.Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <br />
</p>
<p>
    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="userID" DataSourceID="SqlDataSource1" EmptyDataText="There are no data records to display.">
        <Columns>
            <asp:BoundField DataField="userID" HeaderText="userID" ReadOnly="True" SortExpression="userID" />
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [User] WHERE [userID] = @userID" InsertCommand="INSERT INTO [User] ([Username]) VALUES (@Username)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [userID], [Username] FROM [User]" UpdateCommand="UPDATE [User] SET [Username] = @Username WHERE [userID] = @userID">
        <DeleteParameters>
            <asp:Parameter Name="userID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Username" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="userID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</p>
<p>
    &nbsp;</p>
<p>
    <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataKeyNames="fileID" DataSourceID="SqlDataSource2" EmptyDataText="There are no data records to display.">
        <Columns>
            <asp:BoundField DataField="fileID" HeaderText="fileID" ReadOnly="True" SortExpression="fileID" />
            <asp:BoundField DataField="fileName" HeaderText="fileName" SortExpression="fileName" />
            <asp:BoundField DataField="fileSize" HeaderText="fileSize" SortExpression="fileSize" />
            <asp:BoundField DataField="filePath" HeaderText="filePath" SortExpression="filePath" />
            <asp:BoundField DataField="userID" HeaderText="userID" SortExpression="userID" />
            <asp:BoundField DataField="uploadTime" HeaderText="uploadTime" SortExpression="uploadTime" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [UserFiles] WHERE [fileID] = @fileID" InsertCommand="INSERT INTO [UserFiles] ([fileName], [fileSize], [filePath], [userID], [uploadTime]) VALUES (@fileName, @fileSize, @filePath, @userID, @uploadTime)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [fileID], [fileName], [fileSize], [filePath], [userID], [uploadTime] FROM [UserFiles]" UpdateCommand="UPDATE [UserFiles] SET [fileName] = @fileName, [fileSize] = @fileSize, [filePath] = @filePath, [userID] = @userID, [uploadTime] = @uploadTime WHERE [fileID] = @fileID">
        <DeleteParameters>
            <asp:Parameter Name="fileID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="fileName" Type="String" />
            <asp:Parameter Name="fileSize" Type="Int32" />
            <asp:Parameter Name="filePath" Type="String" />
            <asp:Parameter Name="userID" Type="Int32" />
            <asp:Parameter Name="uploadTime" Type="DateTime" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="fileName" Type="String" />
            <asp:Parameter Name="fileSize" Type="Int32" />
            <asp:Parameter Name="filePath" Type="String" />
            <asp:Parameter Name="userID" Type="Int32" />
            <asp:Parameter Name="uploadTime" Type="DateTime" />
            <asp:Parameter Name="fileID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</p>
<p>
    &nbsp;</p>
<p>
    <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" DataKeyNames="fileID" DataSourceID="SqlDataSource3" EmptyDataText="There are no data records to display.">
        <Columns>
            <asp:BoundField DataField="fileID" HeaderText="fileID" ReadOnly="True" SortExpression="fileID" />
            <asp:BoundField DataField="shareduser" HeaderText="shareduser" SortExpression="shareduser" />
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [sharedFiles] WHERE [fileID] = @fileID" InsertCommand="INSERT INTO [sharedFiles] ([fileID], [shareduser], [Username]) VALUES (@fileID, @shareduser, @Username)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [fileID], [shareduser], [Username] FROM [sharedFiles]" UpdateCommand="UPDATE [sharedFiles] SET [shareduser] = @shareduser, [Username] = @Username WHERE [fileID] = @fileID">
        <DeleteParameters>
            <asp:Parameter Name="fileID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="fileID" Type="Int32" />
            <asp:Parameter Name="shareduser" Type="Int32" />
            <asp:Parameter Name="Username" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="shareduser" Type="Int32" />
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="fileID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</p>
<p>
    &nbsp;</p>
</asp:Content>
