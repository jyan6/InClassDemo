<Query Kind="Expression">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//anonymous data type queries
from food in Items
where food.MenuCategory.Description.Equals("Entree")
		&& food.Active
orderby food.CurrentPrice descending
select new
	{
		Description = food.Description,
		Price = food.CurrentPrice,
		Cost = food.CurrentCost,
		Profit = food.CurrentPrice - food.CurrentCost
	}
	