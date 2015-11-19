using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using eRestaurantSystem.BLL;
using eRestaurantSystem.DAL.Entities;
using eRestaurantSystem.DAL.DTOs;
using eRestaurantSystem.DAL.POCOs;
#endregion

public partial class UXPages_FrontDesk : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    
    protected void SeatingGridView_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
    {
        MessageUserControl.TryRun(() =>
            {
                GridViewRow agvrow = SeatingGridView.Rows[e.NewSelectedIndex];
                //accessing a web control on the girdview row usings .FindControl("xxx") as datatype
                string tablenumber = (agvrow.FindControl("TableNumber") as Label).Text;
                string numberinparty = (agvrow.FindControl("NumberInParty") as TextBox).Text;
                string waiterid = (agvrow.FindControl("WaiterList") as DropDownList).SelectedValue;
                var when = Mocker.MockerDate.Add(Mocker.MockerTime);

                //standard call to insert a record into the database
                AdminController sysmgr = new AdminController();
                sysmgr.SeatCustomer(when, byte.Parse(tablenumber), int.Parse(numberinparty), int.Parse(waiterid));
                SeatingGridView.DataBind();
            }, "Customer Seated", "New walk-in customer has been saved");
    }
}