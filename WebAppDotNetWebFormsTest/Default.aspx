<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebAppDotNetWebFormsTest._Default" %>
<%@ Register src="MainUserControl.ascx" tagname="MainUserControl" tagprefix="uc1" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <p>&nbsp;</p>
    <p>
        <uc1:MainUserControl runat="server" />
    </p>
    <p>
        <asp:Label ID="Label1" runat="server" Text="Address : " Width="150px"></asp:Label>
        <asp:TextBox ID="txtAddress" ClientIDMode="Static" runat="server" Width="200px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label2" runat="server" Text="Phone number : " Width="150px"></asp:Label>
        <asp:TextBox ID="txtPhoneNumber" ClientIDMode="Static" runat="server" Width="200px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label3" runat="server" Text="Service Name : " Width="150px"></asp:Label>
        <asp:TextBox ID="txtServiceName" ClientIDMode="Static" runat="server" Width="200px"></asp:TextBox>
    </p>
    <p>
        <asp:Label ID="Label4" runat="server" Text="Proprietary service : " Width="150px"></asp:Label>
        <asp:TextBox ID="txtProprietaryService" ClientIDMode="Static" runat="server" Width="200px"></asp:TextBox>
    </p>
    <p>
        &nbsp;</p>
</asp:Content>
