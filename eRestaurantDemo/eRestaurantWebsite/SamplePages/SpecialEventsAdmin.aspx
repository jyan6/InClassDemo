<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="SpecialEventsAdmin.aspx.cs" Inherits="SpecialEventsAdmin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">

    <table align="center" style="width: 70%">
        <tr>
            <td align ="right" style="width:50%">Select an Event:</td>
            <td>
                <asp:DropDownList ID="SpecialEventList" runat="server" AppendDataBoundItems="True" DataSourceID="ODSSpecialEvents"
                     DataTextField="Description" DataValueField="EventCode"
                    >
                    <asp:ListItem Value="z">Select event</asp:ListItem>
                </asp:DropDownList>
                <asp:LinkButton ID="FetchRegistration" runat="server">Fetch Registrations</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2" align="center">
                <asp:GridView ID="ReservationList" runat="server" AllowCustomPaging="True" AllowPaging="True" AutoGenerateColumns="False" DataSourceID="ODSReservations">
                    <AlternatingRowStyle BackColor="#CCCCCC" />
                    <Columns>
                        <asp:BoundField DataField="NumberInParty" HeaderText="Party" SortExpression="NumberInParty">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ContactPhone" HeaderText="Phone" SortExpression="ContactPhone">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ReservationStatus" HeaderText="Status" SortExpression="ReservationStatus">
                        <HeaderStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:DynamicField DataField="CustomerName" HeaderText="CustomerName" />
                        <asp:DynamicField DataField="ReservationDate" HeaderText="ReservationDate" />
                    </Columns>
                    <EmptyDataTemplate>
                        No data to display at this time
                    </EmptyDataTemplate>
                    <HeaderStyle BackColor="Gray" Font-Size="Larger" />
                    <PagerSettings FirstPageText="start" LastPageText="end" Mode="NumericFirstLast" PageButtonCount="4" Position="TopAndBottom" />
                </asp:GridView>
            </td>

        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
        </tr>
    </table>
    <asp:ObjectDataSource ID="ODSSpecialEvents" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="SpecialEvents_List" TypeName="eRestaurantSystem.BLL.AdminController"></asp:ObjectDataSource>
    <asp:ObjectDataSource ID="ODSReservations" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetReservationsByEventCode" TypeName="eRestaurantSystem.BLL.AdminController">
        <SelectParameters>
            <asp:ControlParameter ControlID="SpecialEventList" Name="eventcode" PropertyName="SelectedValue" Type="String" />
        </SelectParameters>
    </asp:ObjectDataSource>
</asp:Content>

