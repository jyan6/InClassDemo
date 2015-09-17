<Query Kind="Expression">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//where clause

//list all tables that hold more that 3 people
//query syntax
from row in Tables
where row.Capacity > 3
select row

//method syntax
Tables.where (row => row.Capacity > 3)

//list all items with more that 500 calories
from food in Items
where food.Calories > 500
select food

//list all items with more that 500 calories and
//selling for more than $10.00
from food in Items
where food.Calories > 500 && food.CurrentPrice > 10.00m
select food

//list all items with more than 500 calories and are Entrees on the menu
//HINT: navigational properties of the database are known by LinqPad
from food in Items 
where food.Calories > 500 && food.MenuCategory.Description.Equals("Entree")
select food

