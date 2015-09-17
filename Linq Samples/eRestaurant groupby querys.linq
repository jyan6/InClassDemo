<Query Kind="Expression">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//groupby

from food in Items
group food by food.MenuCategory.Description

//this create a key with a value and the row collection for that key value

//more than one field
from food in Items
group food by new {food.MenuCategory.Description, food.CurrentPrice}