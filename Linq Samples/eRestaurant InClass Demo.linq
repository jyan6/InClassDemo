<Query Kind="Statements">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//simpliest form for dumping an entity
Waiters

//simple query syntax
from person in Waiters
select person

//simple method syntax
Waiters.Select(person => person)

//inside our we will be writting C# statement
var results= from person in Waiters
				select person;
//to display the contents of a variable in LinqPad
//use the .Dump() method
results.Dump();

//implemented inside a VS project's class library BLL method
//[DataObjectMethodDataObjectMethodType.Select,false)]
//public List<Waiters> SomeMethodName ()
//{
	//you will need to connect to your DAL object
	//this will be done using a new xxxxx() constructor
	//assume your connection variable is called contextvariable
	
	//do your query
//	var results= from person in contextvariable.Waiters
//				select person;
	//return your results
//	return results.ToList();
}