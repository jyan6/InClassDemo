<Query Kind="Program">
  <Connection>
    <ID>2ef66f16-80ea-4dce-9c40-615e5d365192</ID>
    <Server>.</Server>
    <Database>WorkSchedule</Database>
    <ShowServer>true</ShowServer>
  </Connection>
</Query>

//Q2
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

//Q3
void Main()
{
	var result = from skill in Skills
				where skill.SkillID 
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

//Q4
void Main()
{
	var result = from need in Shifts
					select new
					{
						Day = DayOfWeek,
						EmploysNeeded = EmploysNeeded
					};
}
public class Shifts
{
	public string DescripDayOfWeektion {get;set;}
	public string EmploysNeeded {get;set;}
}