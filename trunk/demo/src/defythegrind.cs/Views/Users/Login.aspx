<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layouts/Layout.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Views_Users_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

	<asp:Login ID="login" CreateUserUrl="~/Views/User/Register.aspx" CreateUserText="register" runat="server">
	
	</asp:Login>

</asp:Content>

