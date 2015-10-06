<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="ReservationByDate.aspx.cs" Inherits="SamplePages_ReservationByDate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <h1>Special Events Reservations by Date (Repeater)</h1>
    <table align="center" style="width: 70%">
        <tr>
            <td align="right">Enter Reservation Date (mm/dd/yyyy):</td>
            <td>
                <asp:TextBox ID="ReservationDateArg" runat="server"
                    ToolTip="Format mm/dd/yyyy" Text="01/01/1900"></asp:TextBox>
                &nbsp;<asp:LinkButton ID="FetchReservation" runat="server">Fetch Reservation</asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td colspan="2">&nbsp;</td>
        </tr>
        <tr>
            <td colspan="2">
                <div class="row col-md-12">
                    <asp:Repeater ID="EventReservationList" runat="server" 
                        DataSourceID="ODSReservationByDate">
                        <ItemTemplate>
                            <h3><%# Eval("Description") %></h3>
                            <asp:Repeater ID="ReservationList" runat="server"
                                DataSource='<%#Eval("Reservations")%>'>
                                <ItemTemplate>
                                    <h4>
                                        <%# Eval("CustomerName") %>
                                        <%# Eval("ContactPhone") %>
                                    </h4>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </td>
 
        </tr>
        </table>
    <asp:ObjectDataSource ID="ODSReservationByDate" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetReservationByData" TypeName="eRestaurantSystem.BLL.AdminController">
            <SelectParameters>
                <asp:ControlParameter ControlID="ReservationDateArg" Name="reservationdate" PropertyName="Text" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </h1>
</asp:Content>

