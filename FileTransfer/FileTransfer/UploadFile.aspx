<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="uploadFile.aspx.cs" Inherits="Testing.uploadFile" EnableEventValidation="true" %>

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
            width: 300px;
            height: 150px;
            padding:5px;
            border-radius:5px;
            border: 1px solid #d0ee1b;
        }

        .grid-view tr.header {
            background-color: white;
            border-bottom: grey 1px solid;
        }

        .grid-view tr.normal {
            background-color: white;
            padding: 5px;
        }

            .grid-view tr.normal:hover {
                background-color: #bbb4b4;
                color: white;
            }

                .grid-view tr.normal:hover .button {
                    display: block;
                }

        .button {
            display: none;
        }
        .hidden {
            display:none;
        }
        .upload {
            border: 1px solid #333;
            background: #fff;
            color: black;
            width:70px;
            height:20px;
            cursor: pointer;
            text-align:center;
        }

        .feedback {
            background: rgb(232,247,237);
            border:1px solid rgb(48,182,97);
            border-radius: 3px;
            margin:9px;
            padding:8px 18px;
            }
    </style>

    <script type="text/javascript">

        function UploadFile(fileUpload) {
            if (fileUpload.value != '')
                document.getElementById("<%=Button1.ClientID %>").click();
        }

        function browse() {
            document.getElementById("<%=FileUpload1.ClientID %>").click();
        }
        
    </script>

    <h2>Upload a file</h2>
    <p>
        &nbsp;
        <asp:Label ID="usertext" runat="server" Text="Username: "></asp:Label>
        &nbsp;<asp:TextBox ID="Username" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="True" CssClass="hidden"/>
    </p>
    <p>
        <asp:Button ID="Button1" runat="server" OnClick="upload" Text="Upload" CssClass="hidden"/>

        <asp:Button ID="Button2" runat="server" OnClick="retrieve" Text="Retrieve" />

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

    </p>
    <div class="upload" id="uploadtrigger" onclick="browse()">
        Upload
    </div>
    <p>
        <asp:LinkButton ID="LinkButton3" runat="server" OnClick="ViewMyFiles" Text="My Files" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:LinkButton ID="LinkButton4" runat="server" OnClick="ViewSharedFiles" Text="Files shared with me" />
    </p>
    <div>
        <asp:Image runat="server" ImageUrl="images/addfolder.png" Width="32" Height="32" />
        <asp:LinkButton runat="server" ID="makefolder" Width="80px" Text="Add Folder" OnClick="addFolder_Click"></asp:LinkButton>
    </div>
    <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label5" runat="server"></asp:Label>
    </p>
    <span class="feedback" visible="false"></span>
    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" GridLines="None" CellSpacing="7" Width="1250px" OnRowDataBound="rowdatabind" CssClass="grid-view" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="64px" />
                        <ItemTemplate>
                            <asp:Image ID="image" runat="server" Width="64" Height="64" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Size" HeaderText="Size" />
                    <asp:BoundField DataField="Last Modified" HeaderText="Last Modified" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" Text="Download" CssClass="button" runat="server" OnClick="DownloadFile"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" Text="Delete" CssClass="button" runat="server" OnClick="DeleteFile" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Share" Text="Share" CssClass="button" runat="server" OnClick="showpopup" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="Move" Text="Move" CssClass="button" runat="server" OnClick="fillTree" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
        <asp:View ID="view2" runat="server">
            <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" GridLines="None" CellSpacing="7" Width="700px" OnRowDataBound="rowdatabind" CssClass="grid-view" OnSelectedIndexChanged="GridView2_SelectedIndexChanged">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="64px" />
                        <ItemTemplate>
                            <asp:Image ID="image" runat="server" Width="50" Height="40" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Name" />
                    <asp:BoundField DataField="Username" HeaderText="Shared with you by" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDownload" Text="Download" CssClass="button" runat="server" OnClick="DownloadSharedFile"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRemove" Text="Remove" CssClass="button" runat="server" OnClick="RemoveFile" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </asp:View>
    </asp:MultiView>

    <!-- Popup for sharing -->
    <asp:Button ID="show" Text="Popup" runat="server" OnClick="showpopup" CssClass="hidden" />
    <asp:Panel ID="panel1" runat="server" CssClass="popup" Height="300px">
        <div class="testing">
        <br />
            &nbsp;&nbsp;
        <asp:Label runat="server" ID="fileName"></asp:Label>
        <br />
            &nbsp;&nbsp;
            <asp:Label runat="server">Username:</asp:Label>
            &nbsp;
            <asp:TextBox ID="sharedUser" runat="server"></asp:TextBox>
            <br />
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <asp:GridView ID="sharedList" runat="server" AutoGenerateColumns="false" CssClass="grid-view" EmptyDataText="This file is not shared with anyone" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Username" HeaderText="Shared with" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRemove" runat="server" OnClick="RemoveShare" Text="Remove" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
            <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="ok" Text="Confirm" runat="server" OnClick="fileshare" Width="70px"/>
            &nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button5" runat="server" Text="Cancel"/>
            </div>
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="panel1" TargetControlID="show" BackgroundCssClass="background" CancelControlID="Button5"></ajaxToolkit:ModalPopupExtender>

    <!-- Popup for moving -->
    <asp:Panel ID="panel2" runat="server" CssClass="popup" Height="300px">
        <asp:Label ID="Label2" runat="server" Text="Move"></asp:Label>
        <asp:Label ID="Label3" runat="server"></asp:Label>
        <br />
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:TreeView ID="dirlist" runat="server"></asp:TreeView>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="move" runat="server" Text="Move" OnClick="move_Click" />
        &nbsp;&nbsp;
        <asp:Button ID="Button7" runat="server" Text="Cancel" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender3" runat="server" PopupControlID="panel2" TargetControlID="show" BackgroundCssClass="background" CancelControlID="Button7"></ajaxToolkit:ModalPopupExtender>

    <!-- Popup for creating folder -->
    <asp:Panel ID="panel3" runat="server" CssClass="popup" Height="150px">
        <asp:Label ID="Label4" runat="server" Text="Create Folder"></asp:Label>
        <br />
        <asp:Label ID="folder" runat="server" Text="Folder name: "></asp:Label>
        <asp:TextBox ID="foldername" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="cr8folder" runat="server" OnClick="createfolder" Text="Create" />
        <asp:Button ID="cancel2" runat="server" Text="Cancel" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" PopupControlID="panel3" TargetControlID="show" BackgroundCssClass="background" CancelControlID="cancel2"></ajaxToolkit:ModalPopupExtender>
</asp:Content>
