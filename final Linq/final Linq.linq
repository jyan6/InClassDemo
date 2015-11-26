<Query Kind="Expression">
  <Connection>
    <ID>2ef66f16-80ea-4dce-9c40-615e5d365192</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eTools</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

//Browse by category
from cat in Categories
orderby cat.Description
	select new
	{
		Categoris = cat.Description,
		Number = (from count in cat.StockItems
					select count.StockItemID).Count()
	}

//Products
from 