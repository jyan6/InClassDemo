using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.DAL;
using eRestaurantSystem.DAL.Entities;
using eRestaurantSystem.DAL.DTOs;
using eRestaurantSystem.DAL.POCOs;
using System.ComponentModel; //Object Data Source
using System.Data.Entity;
#endregion

namespace eRestaurantSystem.BLL
{
    [DataObject]
    public class AdminController
    {

        #region Queries

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SpecialEvent> SpecialEvents_List()
        {
            //connect to our DbContext class in the DAL
            //create an instance of the class
            //we will use a transaction to hold our query
            using (var context = new eRestaurantContext())
            {
                //method syntax
                //return context.SpecialEvents.OrderBy(x => x.Description).ToList();

                //query syntax
                var results = from item in context.SpecialEvents
                              orderby item.Description
                              select item;
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Waiter> Waiters_List()
        {
            using (var context = new eRestaurantContext())
            {
                var results = from item in context.Waiters
                              orderby item.LastName, item.FirstName
                              select item;
                return results.ToList();  //none, 1 or more rows
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Waiter GetWaiterByID(int waiterid)
        {
            using (var context = new eRestaurantContext())
            {
                var results = from item in context.Waiters
                              where item.WaiterID == waiterid
                              select item;
                return results.FirstOrDefault();  //one row at most
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Reservation> GetReservationsByEventCode(string eventcode)
        {
            using (var context = new eRestaurantContext())
            {
                //query syntax
                var results = from item in context.Reservations
                              where item.EventCode.Equals(eventcode)
                              orderby item.CustomerName, item.ReservationDate
                              select item;
                return results.ToList();



            }
        }

    [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<ReservationsByDate> GetReservationsByDate(string reservationdate)
        {
            using (var context = new eRestaurantContext())
            {
                //Linq is not very playful or cooperative with
                //DateTime

                //extract the year, month and day ourselves out
                //of the passed parameter value
                int theYear = (DateTime.Parse(reservationdate)).Year;
                int theMonth = (DateTime.Parse(reservationdate)).Month;
                int theDay = (DateTime.Parse(reservationdate)).Day;

                var results = from eventitem in context.SpecialEvents
                              orderby eventitem.Description
                              select new ReservationsByDate() //a new instance for each specialevent row on the table
                              {
                                  Description = eventitem.Description,
                                  Reservations = from row in eventitem.Reservations
                                                 where row.ReservationDate.Year == theYear
                                                   && row.ReservationDate.Month == theMonth
                                                   && row.ReservationDate.Day == theDay
                                                 select new ReservationDetail() // a new for each reservation of a particular specialevent code
                                                 {
                                                     CustomerName = row.CustomerName,
                                                     ReservationDate = row.ReservationDate,
                                                     NumberInParty = row.NumberInParty,
                                                     ContactPhone = row.ContactPhone,
                                                     ReservationStatus = row.ReservationStatus
                                                 }

                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
    public List<eRestaurantSystem.DAL.DTOs.MenuCategoryItems> MenuCategoryItems_List()
        {
            using (var context = new eRestaurantContext())
            {
                var results = from menuitem in context.MenuCategories
                              orderby menuitem.Description
                              select new MenuCategoryItems() 
                              {
                                  Description = menuitem.Description,
                                  MenuItems = from row in menuitem.MenuItems
                                              select new MenuItem() 
                                                 {
                                                     Description = row.Description,
                                                     Price = row.CurrentPrice,
                                                     Calories = row.Calories,
                                                     Comment = row.Comment
                                                 }
                              };
                return results.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<eRestaurantSystem.DAL.POCOs.CategoryMenuItems> GetReportCategoryMenuItems()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from cat in context.Items
                              orderby cat.Category.Description, cat.Description
                              select new eRestaurantSystem.DAL.POCOs.CategoryMenuItems
                              {
                                  CategoryDescription = cat.Category.Description,
                                  ItemDescription = cat.Description,
                                  Price = cat.CurrentPrice,
                                  Calories = cat.Calories,
                                  Comment = cat.Comment
                              };

                return results.ToList(); // this was .Dump() in Linqpad
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterBilling> GetWaiterBillingReport()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var results = from abillrow in context.Bills
                              where abillrow.BillDate.Month == 5
                              orderby abillrow.BillDate, abillrow.Waiter.LastName, abillrow.Waiter.FirstName
                              select new WaiterBilling
                              {
                                  BillDate = abillrow.BillDate.Year + "/" +
                                             abillrow.BillDate.Month + "/" +
                                             abillrow.BillDate.Day,
                                  Name = abillrow.Waiter.LastName + ", " + abillrow.Waiter.FirstName,
                                  BillID = abillrow.BillID,
                                  BillTotal = abillrow.Items.Sum(bitem => bitem.Quantity * bitem.SalePrice),
                                  PartySize = abillrow.NumberInParty,
                                  Contact = abillrow.Reservation.CustomerName
                              };
                return results.ToList();
            }
        }
        #endregion

        #region Add, Update, Delete of CRUD for CQRS
        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public void SpecialEvents_Add(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //these methods are execute using an instance level item
                //set up a instance pointer and initialize to null
                SpecialEvent added = null;
                //setup the command to execute the add
                added = context.SpecialEvents.Add(item);
                //command is not executed until it is actually saved.
                context.SaveChanges();
            }
        }
         [DataObjectMethod(DataObjectMethodType.Update, false)]
        public void SpecialEvents_Update(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                //indicate the updating item instance
                //alter the Modified Status flag for this instanc
                context.Entry<SpecialEvent>(context.SpecialEvents.Attach(item)).State =
                    System.Data.Entity.EntityState.Modified;
                //command is not executed until it is actually saved.
                context.SaveChanges();
            }
        }
         [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public void SpecialEvents_Delete(SpecialEvent item)
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                
                //lookup the instance and record if found (set pointer to instance)
                SpecialEvent existing = context.SpecialEvents.Find(item.EventCode);

                //setup the command to execute the delete
                context.SpecialEvents.Remove(existing);
                //command is not executed until it is actually saved.
                context.SaveChanges();
            }
        }

         [DataObjectMethod(DataObjectMethodType.Insert, false)]
         public int Waiters_Add(Waiter item)
         {
             using (eRestaurantContext context = new eRestaurantContext())
             {
                 //these methods are execute using an instance level item
                 //set up a instance pointer and initialize to null
                 Waiter added = null;
                 //setup the command to execute the add
                 added = context.Waiters.Add(item);
                 //command is not executed until it is actually saved.
                 context.SaveChanges();
                 //added contains the data of the newly added waiter 
                 //including the pkey value.
                 return added.WaiterID;
             }
         }
         [DataObjectMethod(DataObjectMethodType.Update, false)]
         public void Waiters_Update(Waiter item)
         {
             using (eRestaurantContext context = new eRestaurantContext())
             {
                 //indicate the updating item instance
                 //alter the Modified Status flag for this instanc
                 context.Entry<Waiter>(context.Waiters.Attach(item)).State =
                     System.Data.Entity.EntityState.Modified;
                 //command is not executed until it is actually saved.
                 context.SaveChanges();
             }
         }
         [DataObjectMethod(DataObjectMethodType.Delete, false)]
         public void Waiters_Delete(Waiter item)
         {
             using (eRestaurantContext context = new eRestaurantContext())
             {

                 //lookup the instance and record if found (set pointer to instance)
                 Waiter existing = context.Waiters.Find(item.WaiterID);

                 //setup the command to execute the delete
                 context.Waiters.Remove(existing);
                 //command is not executed until it is actually saved.
                 context.SaveChanges();
             }
         }
        #endregion

        #region Front Desk
         [DataObjectMethod(DataObjectMethodType.Select)]
         public DateTime GetLastBillDateTime()
         {
             using (eRestaurantContext context = new eRestaurantContext())
             {
                 var result = context.Bills.Max(x => x.BillDate);
                 return result;
             }
         }

         [DataObjectMethod(DataObjectMethodType.Select)]
         public List<ReservationCollection> ReservationsByTime(DateTime date)
         {
             using (var context = new eRestaurantContext())
             {
                 var result = (from data in context.Reservations
                               where data.ReservationDate.Year == date.Year
                               && data.ReservationDate.Month == date.Month
                               && data.ReservationDate.Day == date.Day
                                   // && data.ReservationDate.Hour == timeSlot.Hours
                               && data.ReservationStatus == Reservation.Booked
                               select new ReservationSummary()
                               {
                                   ID = data.ReservationID,
                                   Name = data.CustomerName,
                                   Date = data.ReservationDate,
                                   NumberInParty = data.NumberInParty,
                                   Status = data.ReservationStatus,
                                   Event = data.Event.Description,
                                   Contact = data.ContactPhone
                               }).ToList();
                 //the second part of this method uses the results of the
                 //first linq query.
                 //Linq to Entity will only execute the query when it deems
                 //necessary for having the results in memory.
                 //
                 //to get your query to execute and have the resulting data
                 //inside memory for further use, you can attach the .ToList()
                 //to the previous query

                 //note: the second query is NOT using an Entity
                 //it is using the results from a previous query

                 //itemGroup is a temporary in memory data collection
                 //this collection can be used in selecting your final
                 //data collection.
                 var finalResult = from item in result
                                   orderby item.NumberInParty
                                   group item by item.Date.Hour into itemGroup
                                   select new ReservationCollection()
                                   {
                                       Hour = itemGroup.Key,
                                       Reservations = itemGroup.ToList()
                                   };
                 return finalResult.OrderBy(x => x.Hour).ToList();
             }
         }

        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<SeatingSummary> SeatingByDateTime (DateTime date, TimeSpan time)
         {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                // Step 1 - Get the table info along with any walk-in bills and reservation bills for the specific time slot
                var step1 = from data in context.Tables
                            select new
                            {
                                Table = data.TableNumber,
                                Seating = data.Capacity,
                                // This sub-query gets the bills for walk-in customers
                                WalkIns = from walkIn in data.Bills
                                          where
                                                 walkIn.BillDate.Year == date.Year
                                              && walkIn.BillDate.Month == date.Month
                                              && walkIn.BillDate.Day == date.Day
                                              //&& walkIn.BillDate.TimeOfDay <= time
                                              && DbFunctions.CreateTime(walkIn.BillDate.Hour,
                                                                        walkIn.BillDate.Minute,
                                                                        walkIn.BillDate.Second) <= time
                                              && (!walkIn.OrderPaid.HasValue ||
                                              walkIn.OrderPaid.Value >= time)
                                                      //DbFunctions.CreateTime(walkIn.OrderPaid.Value.Hour,
                                                      //                  walkIn.OrderPaid.Value.Minute,
                                                      //                  walkIn.OrderPaid.Value.Second) >= time)
                                                                    //&& (!walkIn.PaidStatus || walkIn.OrderPaid >= time)
                                          select walkIn,
                                // This sub-query gets the bills for reservations
                                Reservations = from booking in data.Reservations
                                               from reservationParty in booking.Bills
                                               where
                                                   reservationParty.BillDate.Year == date.Year
                                                   && reservationParty.BillDate.Month == date.Month
                                                   && reservationParty.BillDate.Day == date.Day
                                                   //&& reservationParty.BillDate.TimeOfDay <= time
                                                    && DbFunctions.CreateTime(reservationParty.BillDate.Hour,
                                                                        reservationParty.BillDate.Minute,
                                                                        reservationParty.BillDate.Second) <= time
                                                   && (!reservationParty.OrderPaid.HasValue ||
                                                    //DbFunctions.CreateTime(reservationParty.OrderPaid.Value.Hour,
                                                    //                    reservationParty.OrderPaid.Value.Minute,
                                                    //                    reservationParty.OrderPaid.Value.Second) >= time)
                                                   reservationParty.OrderPaid.Value >= time)
                                               //                          && (!reservationParty.PaidStatus || reservationParty.OrderPaid >= time)
                                               select reservationParty
                            };
              

                // Step 2 - Union the walk-in bills and the reservation bills while extracting the relevant bill info
                // .ToList() helps resolve the "Types in Union or Concat are constructed incompatibly" error
                var step2 = from data in step1.ToList() // .ToList() forces the first result set to be in memory
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                CommonBilling = from info in data.WalkIns.Union(data.Reservations)
                                                select new // info
                                                {
                                                    BillID = info.BillID,
                                                    BillTotal = info.Items.Sum(bi => bi.Quantity * bi.SalePrice),
                                                    Waiter = info.Waiter.FirstName,
                                                    Reservation = info.Reservation
                                                }
                            };
                //step2.Dump();
                // Step 3 - Get just the first CommonBilling item
                //         (presumes no overlaps can occur - i.e., two groups at the same table at the same time)
                var step3 = from data in step2.ToList()
                            select new
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.CommonBilling.Count() > 0,
                                // .FirstOrDefault() is effectively "flattening" my collection of 1 item into a 
                                // single object whose properties I can get in step 4 using the dot (.) operator
                                CommonBilling = data.CommonBilling.FirstOrDefault()
                            };
                //step3.Dump();
                // Step 4 - Build our intended seating summary info
                var step4 = from data in step3
                            select new SeatingSummary() // the POCO class to use in my BLL
                            {
                                Table = data.Table,
                                Seating = data.Seating,
                                Taken = data.Taken,
                                // use a ternary expression to conditionally get the bill id (if it exists)
                                BillID = data.Taken ?               // if(data.Taken)
                                         data.CommonBilling.BillID  // value to use if true
                                       : (int?)null,               // value to use if false
                                BillTotal = data.Taken ?
                                            data.CommonBilling.BillTotal : (decimal?)null,
                                Waiter = data.Taken ? data.CommonBilling.Waiter : (string)null,
                                ReservationName = data.Taken ?
                                                  (data.CommonBilling.Reservation != null ?
                                                   data.CommonBilling.Reservation.CustomerName : (string)null)
                                                : (string)null
                            };
                return step4.ToList();
            }
         }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<WaiterOnDuty> ListWaiters()
        {
            using (eRestaurantContext context = new eRestaurantContext())
            {
                var result = from person in context.Waiters
                             where person.ReleaseDate == null
                             select new WaiterOnDuty()
                             {
                                 WaiterId = person.WaiterID,
                                 FullName = person.FirstName + " " + person.LastName
                             };
                return result.ToList();
            }
        }

        #endregion
    }//eof class
}//eof namespace


