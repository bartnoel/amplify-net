<%@ Control Language="C#" AutoEventWireup="true" CodeFile="EventFormView.ascx.cs" Inherits="Views_Events_Controls_EventFormView" %>
<%@ Register Assembly="Csla" Namespace="Csla.Web" TagPrefix="cc1" %>


<def:FormView ID="FormView" runat="server" EnableModelValidation="true"  DataSourceID="DataSource" DefaultMode="Edit">
	<EditItemTemplate>
		<ul>
			<li><def:FormViewItem ID="Event_Name1" DataField="Name" TypeName="Event"  runat="server" /></li>
		<li><def:FormViewItem ID="Event_Price2" DataField="Price" TypeName="Event"  runat="server" /></li>
		<li><def:FormViewItem ID="Event_TicketSalesStartAt3" TypeName="Event"  DataField="TicketSalesStartAt" runat="server" /></li>
		<li><def:FormViewItem ID="Event_StartAt4" DataField="StartAt" TypeName="Event"  runat="server" /></li>
		</ul>
		<asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" 
			CommandName="Update" Text="Update" />
		&nbsp;<asp:LinkButton ID="UpdateCancelButton" runat="server" 
			CausesValidation="False" CommandName="Cancel" Text="Cancel" />
	</EditItemTemplate>
	<InsertItemTemplate>
		<def:FormViewItem ID="FormViewItem1" DataField="Name" runat="server" />
		<def:FormViewItem ID="FormViewItem2" DataField="Price" runat="server" />
		<def:FormViewItem ID="FormViewItem3" DataField="TicketSalesStartAt" runat="server" />
		<def:FormViewItem ID="FormViewItem4" DataField="StartAt" runat="server" />
		<asp:TextBox ID="test" runat="server" Text='<%# Bind("Price") %>' />
		<asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" 
			CommandName="Insert" Text="Insert" />
		&nbsp;<asp:LinkButton ID="InsertCancelButton" runat="server" 
			CausesValidation="False" CommandName="Cancel" Text="Cancel" />
	</InsertItemTemplate>
	<ItemTemplate>
		Id:
		<asp:Label ID="IdLabel" runat="server" Text='<%# Bind("Id") %>' />
		<br />
		Name:
		<asp:Label ID="NameLabel" runat="server" Text='<%# Bind("Name") %>' />
		<br />
		Price:
		<asp:Label ID="PriceLabel" runat="server" Text='<%# Bind("Price") %>' />
		<br />
		TicketSalesStartAt:
		<asp:Label ID="TicketSalesStartAtLabel" runat="server" 
			Text='<%# Bind("TicketSalesStartAt") %>' />
		<br />
		StartsAt:
		<asp:Label ID="StartsAtLabel" runat="server" Text='<%# Bind("StartsAt") %>' />
		<br />
		VenueId:
		<asp:Label ID="VenueIdLabel" runat="server" Text='<%# Bind("VenueId") %>' />
		<br />
		OrganizationId:
		<asp:Label ID="OrganizationIdLabel" runat="server" 
			Text='<%# Bind("OrganizationId") %>' />
		<br />
	</ItemTemplate>
	
</def:FormView>




<cc1:CslaDataSource TypeName="DefyTheGrind.Models.Event" ID="DataSource" runat="server">
</cc1:CslaDataSource>
