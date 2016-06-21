<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="uploadFile.aspx.cs" Inherits="Testing.uploadFile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        .background {
            background-color: gray;
            opacity: 0.8;
            filter: alpha(opacity=80);
            z-index: 1000;
        }

        .popup {
            background-color: white;
            opacity: 1;
            width: 300px;
            height: 150px;
        }
        </style>
    <h2>Upload a file</h2>
    <p>&nbsp;
        <asp:Label ID="usertext" runat="server" Text="Username: "></asp:Label>
&nbsp;<asp:TextBox ID="Username" runat="server"></asp:TextBox>
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

     <p>
        <asp:Button ID="Button2" runat="server" OnClick="retrieve" Text="Retrieve" />
    </p>
    <p>
     <asp:Button ID="Button3" runat="server" OnClick="ViewMyFiles" Text="My Files" CssClass="Initial" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Button ID="Button4" runat="server" OnClick="ViewSharedFiles" Text="Files shared with me" CssClass="Initial" />
    </p>

     <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Text" HeaderText="File Name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DeleteFile" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Share" Text="Share" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="showpopup" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>

            </asp:GridView>
        </asp:View>
        <asp:View ID="view2" runat="server">
             <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Text" HeaderText="File Name" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" Text="Download" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DownloadFile"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" Text="Delete" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick="DeleteFile" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Info" Text="More" CommandArgument='<%# Eval("Value") %>' runat="server" OnClick=getinfo ToolTip="Shared with you by: " />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
    </asp:MultiView>

    <asp:Button ID="show" Text="Popup" runat="server" onclick="showpopup" style="display:none"/>
    <asp:Panel ID="panel1" runat="server" style="display:none" CssClass="popup">
        <br />
        <asp:Label runat="server" ID="fileName"></asp:Label>
        <br />
        <asp:label runat="server">Username:</asp:label>
        <asp:TextBox runat="server" ID="sharedUser"></asp:TextBox>
        <br />
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="ok" Text="Confirm" runat="server" onclick="fileshare"/>
        &nbsp;&nbsp; &nbsp;&nbsp;
        <asp:Button ID="Button5" runat="server" Text="Cancel" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="panel1" TargetControlID="show" BackgroundCssClass="background"></ajaxToolkit:ModalPopupExtender>

</asp:Content>