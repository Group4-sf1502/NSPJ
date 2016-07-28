<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Log.aspx.cs" Inherits="FileTransfer.Log" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <p>
    <br />
</p>
<p>
    <asp:GridView ID="GridView5" runat="server" AutoGenerateColumns="False" DataKeyNames="Username" DataSourceID="SqlDataSource1" EmptyDataText="There are no data records to display.">
        <Columns>
            <asp:BoundField DataField="Username" HeaderText="Username" ReadOnly="True" SortExpression="Username" />
            <asp:BoundField DataField="keys" HeaderText="keys" SortExpression="keys" />
            <asp:BoundField DataField="StorageSize" HeaderText="StorageSize" SortExpression="StorageSize" />
            <asp:BoundField DataField="StorageUsed" HeaderText="StorageUsed" SortExpression="StorageUsed" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [User] WHERE [Username] = @Username" InsertCommand="INSERT INTO [User] ([Username], [keys], [StorageSize], [StorageUsed]) VALUES (@Username, @keys, @StorageSize, @StorageUsed)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [Username], [keys], [StorageSize], [StorageUsed] FROM [User]" UpdateCommand="UPDATE [User] SET [keys] = @keys, [StorageSize] = @StorageSize, [StorageUsed] = @StorageUsed WHERE [Username] = @Username">
        <DeleteParameters>
            <asp:Parameter Name="Username" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="keys" Type="String" />
            <asp:Parameter Name="StorageSize" Type="Int32" />
            <asp:Parameter Name="StorageUsed" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="keys" Type="String" />
            <asp:Parameter Name="StorageSize" Type="Int32" />
            <asp:Parameter Name="StorageUsed" Type="Int32" />
            <asp:Parameter Name="Username" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
</p>
<p>
    &nbsp;</p>
<p>
    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" DataKeyNames="fileID" DataSourceID="SqlDataSource2" EmptyDataText="There are no data records to display.">
        <Columns>
            <asp:BoundField DataField="fileID" HeaderText="fileID" ReadOnly="True" SortExpression="fileID" />
            <asp:BoundField DataField="fileName" HeaderText="fileName" SortExpression="fileName" />
            <asp:BoundField DataField="fileSize" HeaderText="fileSize" SortExpression="fileSize" />
            <asp:BoundField DataField="filePath" HeaderText="filePath" SortExpression="filePath" />
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
            <asp:BoundField DataField="uploadTime" HeaderText="uploadTime" SortExpression="uploadTime" />
            <asp:BoundField DataField="IV" HeaderText="IV" SortExpression="IV" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [UserFiles] WHERE [fileID] = @fileID" InsertCommand="INSERT INTO [UserFiles] ([fileName], [fileSize], [filePath], [Username], [uploadTime], [IV]) VALUES (@fileName, @fileSize, @filePath, @Username, @uploadTime, @IV)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [fileID], [fileName], [fileSize], [filePath], [Username], [uploadTime], [IV] FROM [UserFiles]" UpdateCommand="UPDATE [UserFiles] SET [fileName] = @fileName, [fileSize] = @fileSize, [filePath] = @filePath, [Username] = @Username, [uploadTime] = @uploadTime, [IV] = @IV WHERE [fileID] = @fileID">
        <DeleteParameters>
            <asp:Parameter Name="fileID" Type="Int32" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="fileName" Type="String" />
            <asp:Parameter Name="fileSize" Type="Int32" />
            <asp:Parameter Name="filePath" Type="String" />
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="uploadTime" Type="DateTime" />
            <asp:Parameter Name="IV" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="fileName" Type="String" />
            <asp:Parameter Name="fileSize" Type="Int32" />
            <asp:Parameter Name="filePath" Type="String" />
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="uploadTime" Type="DateTime" />
            <asp:Parameter Name="IV" Type="String" />
            <asp:Parameter Name="fileID" Type="Int32" />
        </UpdateParameters>
    </asp:SqlDataSource>
</p>
<p>
    &nbsp;</p>
<p>
    <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" DataKeyNames="fileID,shareduser" DataSourceID="SqlDataSource3" EmptyDataText="There are no data records to display.">
        <Columns>
            <asp:BoundField DataField="fileID" HeaderText="fileID" ReadOnly="True" SortExpression="fileID" />
            <asp:BoundField DataField="shareduser" HeaderText="shareduser" ReadOnly="True" SortExpression="shareduser" />
            <asp:BoundField DataField="Username" HeaderText="Username" SortExpression="Username" />
        </Columns>
    </asp:GridView>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [sharedFiles] WHERE [fileID] = @fileID AND [shareduser] = @shareduser" InsertCommand="INSERT INTO [sharedFiles] ([fileID], [shareduser], [Username]) VALUES (@fileID, @shareduser, @Username)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [fileID], [shareduser], [Username] FROM [sharedFiles]" UpdateCommand="UPDATE [sharedFiles] SET [Username] = @Username WHERE [fileID] = @fileID AND [shareduser] = @shareduser">
        <DeleteParameters>
            <asp:Parameter Name="fileID" Type="Int32" />
            <asp:Parameter Name="shareduser" Type="String" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="fileID" Type="Int32" />
            <asp:Parameter Name="shareduser" Type="String" />
            <asp:Parameter Name="Username" Type="String" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="Username" Type="String" />
            <asp:Parameter Name="fileID" Type="Int32" />
            <asp:Parameter Name="shareduser" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
</p>
    <p>
        &nbsp;</p>
    <p>
        <asp:GridView ID="GridView4" runat="server" AutoGenerateColumns="False" DataKeyNames="folderPath,sharedWith" DataSourceID="SqlDataSource4" EmptyDataText="There are no data records to display.">
            <Columns>
                <asp:BoundField DataField="folderPath" HeaderText="folderPath" ReadOnly="True" SortExpression="folderPath" />
                <asp:BoundField DataField="sharedWith" HeaderText="sharedWith" ReadOnly="True" SortExpression="sharedWith" />
                <asp:BoundField DataField="User" HeaderText="User" SortExpression="User" />
                <asp:BoundField DataField="folderName" HeaderText="folderName" SortExpression="folderName" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:FileDatabaseConnectionString2 %>" DeleteCommand="DELETE FROM [sharedFolder] WHERE [folderPath] = @folderPath AND [sharedWith] = @sharedWith" InsertCommand="INSERT INTO [sharedFolder] ([folderPath], [sharedWith], [User], [folderName]) VALUES (@folderPath, @sharedWith, @User, @folderName)" ProviderName="<%$ ConnectionStrings:FileDatabaseConnectionString2.ProviderName %>" SelectCommand="SELECT [folderPath], [sharedWith], [User], [folderName] FROM [sharedFolder]" UpdateCommand="UPDATE [sharedFolder] SET [User] = @User, [folderName] = @folderName WHERE [folderPath] = @folderPath AND [sharedWith] = @sharedWith">
            <DeleteParameters>
                <asp:Parameter Name="folderPath" Type="String" />
                <asp:Parameter Name="sharedWith" Type="String" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="folderPath" Type="String" />
                <asp:Parameter Name="sharedWith" Type="String" />
                <asp:Parameter Name="User" Type="String" />
                <asp:Parameter Name="folderName" Type="String" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="User" Type="String" />
                <asp:Parameter Name="folderName" Type="String" />
                <asp:Parameter Name="folderPath" Type="String" />
                <asp:Parameter Name="sharedWith" Type="String" />
            </UpdateParameters>
        </asp:SqlDataSource>
</p>
<p>
    &nbsp;</p>
</asp:Content>
