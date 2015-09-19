<Query Kind="Expression">
  <Connection>
    <ID>33df9dab-5df0-4c33-aa9f-012412c7fcc9</ID>
    <Persist>true</Persist>
    <Server>.</Server>
    <Database>eRestaurant</Database>
  </Connection>
</Query>

//SUM
(from theBill in BillItems
where theBill.BillID == 104
select theBill.SalePrice * theBill.Quantity).Sum()

//MAX
(from customer in Bills
where customer.PaidStatus == true
select customer.BillItems.Sum(theBill => theBill.SalePrice * theBill.Quantity)).Max()

//the average paid bill

(from customer in Bills
where customer.PaidStatus == true
select customer.BillItems.Sum(theBill => theBill.SalePrice * theBill.Quantity)).Average()

//what is the average number of items per paid bill
//we need to get a list of number of representing the items per bill
//we take an average of the list

(from customer in Bills
where customer.PaidStatus
select customer.BillItems.Count()).Average()