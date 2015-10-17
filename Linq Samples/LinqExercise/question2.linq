<Query Kind="Program">
  <Connection>
    <ID>2ef66f16-80ea-4dce-9c40-615e5d365192</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>WorkSchedule</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

void Main()
{
	var result = from skill in Skills
				select new String()
				{
					Description = skill.Description
				};
	result.Dump();
	// Define other methods and classes here
	
}
public class String
{
	public string Description {get;set;}
}
// Define other methods and classes here
