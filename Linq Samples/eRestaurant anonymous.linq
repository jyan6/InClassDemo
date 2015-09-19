<Query Kind="Program">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

void Main()
{
//anonymous data type queries
//from food in Items
//where food.MenuCategory.Description.Equals("Entree")
//		&& food.Active
//orderby food.CurrentPrice descending
//select new
//	{
//		Description = food.Description,
//		Price = food.CurrentPrice,
//		Cost = food.CurrentCost,
//		Profit = food.CurrentPrice - food.CurrentCost
//	}
//	
//from food in Items
//where food.MenuCategory.Description.Equals("Entree")
//		&& food.Active
//orderby food.CurrentPrice descending
//select new
//	{
//		food.Description,
//		food.CurrentPrice,
//		food.CurrentCost
//		//Profit = food.CurrentPrice - food.CurrentCost
//	}
	var results = from food in Items
				where food.MenuCategory.Description.Equals("Entree")
					&& food.Active
				orderby food.CurrentPrice descending
				select new FoodMargins()
					{
						Description = food.Description,
						Price = food.CurrentPrice,
						Cost = food.CurrentCost,
						Profit = food.CurrentPrice - food.CurrentCost
					};
	results.Dump();
	
	//get all the bill items for waiters in sep of 2014
	//get only those bills which were paid,
	var result2 = from orders in Bills
					where orders.PaidStatus && 
					(orders.BillDate.Month == 9 && orders.BillDate.Year == 2014)
					orderby orders.Waiter.LastName, orders.Waiter.FirstName
					select new
					{
						BillID = orders.BillID,
						WaiterName = orders.Waiter.LastName + "," + orders.Waiter.FirstName,
						Orders = orders.BillItems
					};
	result2.Dump();
	
}//eop

//define other methods and classes here

public class FoodMargins
{
	public string Description {get; set;}
	public decimal Price {get; set;}
	public decimal Cost {get; set;}
	public decimal Profit {get; set;}
}

//this is a DTO class
public class BillOrders
{
	public int BillID {get;set;}
	public string WaiterName {get;set;}
	//public BillItems Orders {get;set;}
	public IEnumerable Orders {get;set;}
}