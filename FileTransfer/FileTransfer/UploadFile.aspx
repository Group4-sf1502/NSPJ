﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="uploadFile.aspx.cs" Inherits="Testing.uploadFile" EnableEventValidation="true" %>

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
    </style>

    <h2>Upload a file</h2>
    <p>
        &nbsp;
        <asp:Label ID="usertext" runat="server" Text="Username: "></asp:Label>
        &nbsp;<asp:TextBox ID="Username" runat="server"></asp:TextBox>
    </p>
    <p>
        <asp:FileUpload ID="FileUpload1" runat="server" AllowMultiple="True" />
    </p>
    <p>
        <asp:Button ID="Button1" runat="server" OnClick="upload" Text="Upload" />

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="Button2" runat="server" OnClick="retrieve" Text="Retrieve" />

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;

    </p>


    <p>
        <asp:Button ID="Button3" runat="server" OnClick="ViewMyFiles" Text="My Files" />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
     <asp:Button ID="Button4" runat="server" OnClick="ViewSharedFiles" Text="Files shared with me" />
    </p>
    <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="addFolder" runat="server" OnClick="addFolder_Click" Text="Add Folder" Width="85px" />

        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label5" runat="server"></asp:Label>
    </p>
    <!-- <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Image id="fileimages" ImageUrl="C:\Users\daryl\Desktop\folder.png" runat="server"/>
                        </ItemTemplate>
                    </asp:TemplateField> -->
    <!-- <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox runat="server" id="checkboxes"/>
                        </HeaderTemplate>
                    </asp:TemplateField> -->

    <asp:MultiView ID="MultiView" runat="server">
        <asp:View ID="View1" runat="server">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EmptyDataText="No files uploaded" GridLines="None" CellSpacing="7" Width="1250px" OnRowDataBound="rowdatabind" CssClass="grid-view" OnSelectedIndexChanged="GridView1_SelectedIndexChanged" AllowSorting="True">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle width="50px"/>
                        <ItemTemplate>
                            <asp:Image id="image" runat="server" width="50" height="40"/>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="File Name" />
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
                    <asp:BoundField DataField="Name" HeaderText="File Name" />
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

    <asp:Button ID="show" Text="Popup" runat="server" OnClick="showpopup" Style="display: none" />
    <asp:Panel ID="panel1" runat="server" CssClass="popup" Height="300px">
        <br />
        <asp:Label runat="server" ID="fileName"></asp:Label>
        <br />
        <asp:Label runat="server">Username:</asp:Label>
        <asp:TextBox runat="server" ID="sharedUser"></asp:TextBox>
        <br />
        <br />
        <asp:GridView ID="sharedList" runat="server" CssClass="grid-view" GridLines="None" EmptyDataText="This file is not shared with anyone" AutoGenerateColumns="false">
            <Columns>
                <asp:BoundField DataField="Username" HeaderText="Shared with" />
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkRemove" Text="Remove" runat="server" OnClick="RemoveShare" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="ok" Text="Confirm" runat="server" OnClick="fileshare" />
        &nbsp;&nbsp; &nbsp;&nbsp;
        <asp:Button ID="Button5" runat="server" Text="Cancel" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" PopupControlID="panel1" TargetControlID="show" BackgroundCssClass="background"></ajaxToolkit:ModalPopupExtender>
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
    <asp:Panel ID="panel3" runat="server" CssClass="popup" Height="150px">
        <asp:Label ID="Label4" runat="server" Text="Create Folder"></asp:Label>
        <br />
        <asp:Label ID="folder" runat="server" Text="Folder name: "></asp:Label>
        <asp:TextBox ID="foldername" runat="server"></asp:TextBox>
        <br />
        <br />
        <asp:Button ID="createfolder" runat="server" OnClick="cr8folder" Text="Create" />
        <asp:Button ID="cancel2" runat="server" Text="Cancel" />
    </asp:Panel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender4" runat="server" PopupControlID="panel3" TargetControlID="show" BackgroundCssClass="background" CancelControlID="cancel2"></ajaxToolkit:ModalPopupExtender>
</asp:Content>
