﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="FrontDesk.aspx.cs" Inherits="UXPages_FrontDesk" %>

<%@ Register Src="~/UserControls/MessageUserControl.ascx" TagPrefix="uc1" TagName="MessageUserControl" %>
<%@ Register Src="~/UserControls/DateTimeMocker.ascx" TagPrefix="uc1" TagName="DateTimeMocker" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" Runat="Server">
    <uc1:DateTimeMocker runat="server" ID="Mocker" />

    <!-- this is the presentation markup code for the
        seating summary display-->
    <uc1:MessageUserControl runat="server" ID="MessageUserControl" />

    <div class="col-md-7">
        <details open>
            <summary>Tables</summary>
            <p class="well">This GridView uses a &lt;asp:TemplateField …&gt; for the 
                table number and the controls to handle walk-in seating. 
                Additionally, the walk-in seating form is in a panel that 
                only shows if the seat is <em>not</em> taken. Handling of 
                the action to <b>seat customers</b> is done in the code-behind, 
                on the GridView's <code>OnSelectedIndexChanging</code> event.</p>
            <style type="text/css">
                .inline-div {
                    display: inline;
                }
            </style>
            <asp:GridView ID="SeatingGridView" runat="server" AutoGenerateColumns="False"
                    CssClass="table table-hover table-striped table-condensed"
                    DataSourceID="SeatingObjectDataSource" 
                    ItemType="eRestaurantSystem.DAL.POCOs.SeatingSummary" OnSelectedIndexChanging="SeatingGridView_SelectedIndexChanging">
                <Columns>
                    <asp:CheckBoxField DataField="Taken" HeaderText="Taken" 
                        SortExpression="Taken" ItemStyle-HorizontalAlign="Center">
                    </asp:CheckBoxField>
                    <asp:TemplateField HeaderText="Table" SortExpression="Table" 
                        ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="TableNumber" runat="server" 
                                Text='<%# Item.Table %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Seating" HeaderText="Seating" 
                        SortExpression="Seating" ItemStyle-HorizontalAlign="Center"></asp:BoundField>
                    <asp:TemplateField HeaderText="Reservation Notes / Seat Walk-In">
                        <ItemTemplate>
                            <asp:Panel ID="WalkInSeatingPanel" runat="server" 
                                CssClass="input-group input-group-sm"
                                    Visible='<%# !Item.Taken %>'>
                                <asp:TextBox ID="NumberInParty" runat="server" 
                                    CssClass="form-control col-md-1"
                                        TextMode="Number" placeholder="# people">
                                </asp:TextBox>
                                <span class="input-group-addon">
                                    <asp:DropDownList ID="WaiterList" runat="server"
                                            CssClass="selectpicker"
                                            AppendDataBoundItems="true" DataSourceID="WaitersDataSource"
                                            DataTextField="FullName" DataValueField="WaiterId">
                                        <asp:ListItem Value="0">[select a waiter]</asp:ListItem>
                                    </asp:DropDownList>
                                </span>
                                <span class="input-group-addon" style="width:5px;padding:0;
                                    border:0;background-color:white;"></span>
                                <asp:LinkButton ID="LinkButton1" runat="server" Text="Seat Customers"
                                    CssClass="input-group-btn" CommandName="Select" 
                                    CausesValidation="False" />
                            </asp:Panel>
                            <asp:Panel ID="OccupiedTablePanel" runat="server"
                                    Visible='<%# Item.Taken  %>'>
                                <asp:HyperLink ID="ServerTableLink" runat="server"
                                    NavigateUrl='<%# string.Format("~/UXPages/ServingTables.aspx?waiter={0}&bill={1}&md={2}&mt={3}&mds={4}&mts={5}",
                                                                        Item.Waiter,Item.BillID,Mocker.MockerDate.Ticks,Mocker.MockerTime.Ticks,
                                                                        Mocker.MockerDate.ToShortDateString(), Mocker.MockerTime.ToString()) %>'>
                                <%# Item.Waiter %> </asp:HyperLink>
                                <asp:Label ID="ReservationNameLabel" runat="server" 
                                        Text='<%# "&mdash;" + Item.ReservationName %>'
                                        Visible='<%# !string.IsNullOrEmpty(Item.ReservationName) %>' />
                                <asp:Panel ID="BillInfo" runat="server" CssClass="inline-div"
                                        Visible='<%# Item.BillTotal.HasValue && Item.BillTotal.Value > 0 %>'>
                                    <asp:Label ID="Label1" runat="server" Text='<%# 
                                    string.Format(" &ndash; {0:C}", Item.BillTotal) %>' />
                                </asp:Panel>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </details>
    </div>
    <asp:ObjectDataSource runat="server" ID="SeatingObjectDataSource" OldValuesParameterFormatString="original_{0}"
        SelectMethod="SeatingByDateTime" 
        TypeName="eRestaurantSystem.BLL.AdminController">
        <SelectParameters>
            <asp:ControlParameter ControlID="Mocker" PropertyName="MockerDate" Name="date" Type="DateTime"></asp:ControlParameter>
            <asp:ControlParameter ControlID="Mocker" PropertyName="MockerTime" DbType="Time" Name="time"></asp:ControlParameter>
        </SelectParameters>
    </asp:ObjectDataSource>
    <asp:ObjectDataSource runat="server" ID="WaitersDataSource" OldValuesParameterFormatString="original_{0}"
    SelectMethod="ListWaiters" 
        TypeName="eRestaurantSystem.BLL.AdminController"></asp:ObjectDataSource>

    <!--this is the presentation markup code for the 
        reservation display -->
    <div class="pull-right col-md-5">
        <!-- details is a bootstrap collapsable area-->
        <details open>
            <!-- the text displayed beside the details icon -->
            <summary>Reservations by Date/Time</summary>
            <h4>Today's Reservations</h4>
            <!-- ItemType= parameters must be directed to your
                current application locations -->
            <asp:Repeater ID="ReservationsRepeater" runat="server"
                ItemType="eRestaurantSystem.DAL.DTOs.ReservationCollection" 
                DataSourceID="ReservationsDataSource">
                <ItemTemplate>
                    <div>
                        <h4><%# Item.SeatingTime %></h4>
                        <asp:ListView ID="ReservationSummaryListView" runat="server"
                                ItemType="eRestaurantSystem.DAL.POCOs.ReservationSummary"
                                DataSource="<%# Item.Reservations %>">
                            <LayoutTemplate>
                                <div class="seating">
                                    <span runat="server" id="itemPlaceholder" />
                                </div>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <div>
                                    <%# Item.Name %> —
                                    <%# Item.NumberInParty %> —
                                    <%# Item.Status %> —
                                    PH:
                                    <%# Item.Contact %>
                                </div>
                            </ItemTemplate>
                        </asp:ListView>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <!-- TypeName= parameters must be directed to your
                current application locations -->
            <asp:ObjectDataSource runat="server" ID="ReservationsDataSource" 
                OldValuesParameterFormatString="original_{0}" 
                SelectMethod="ReservationsByTime" 
                TypeName="eRestaurantSystem.BLL.AdminController">
                <SelectParameters>
                    <asp:ControlParameter ControlID="Mocker" 
                        PropertyName="MockerDate" Name="date" 
                        Type="DateTime"></asp:ControlParameter>
                </SelectParameters>
            </asp:ObjectDataSource>
        </details>
    </div>
</asp:Content>

