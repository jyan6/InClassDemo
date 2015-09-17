<Query Kind="Expression">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//orderby

//default is ascending
from food in Items
orderby food.Description
select food

//also available descending
from food in Items
orderby food.CurrentPrice descending
select food

//can use both ascending and descending
from food in Items
orderby food.CurrentPrice descending, food.Calories ascending
select food

//you can use where and order by together
from food in Items
where food.MenuCategory.Description.Equals("Entree")
orderby food.CurrentPrice descending, food.Calories ascending 
select food