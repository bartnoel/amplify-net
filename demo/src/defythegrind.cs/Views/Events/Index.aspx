<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Layouts/Layout.master" AutoEventWireup="true" CodeFile="Index.aspx.cs" Inherits="Views_Events_Index" %>

<%@ Register src="Controls/EventFormView.ascx" tagname="EventFormView" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="Head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content" Runat="Server">

	<uc1:EventFormView ID="EventFormView1" runat="server" />

</asp:Content>

