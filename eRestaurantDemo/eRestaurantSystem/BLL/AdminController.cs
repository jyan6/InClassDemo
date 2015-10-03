using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using eRestaurantSystem.DAL;
using eRestaurantSystem.DAL.Entities;
using System.ComponentModel;//Object Data Source
using eRestaurantSystem.DAL.DTOs;
using eRestaurantSystem.DAL.POCOs;
#endregion

namespace eRestaurantSystem.BLL
{
    [DataObject]
    public class AdminController
    {
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
        public List<ReservationsByDate> GetReservationByData(string reservationdate)
        {
          using (var context = new eRestaurantContext())
          {
              //Linq is not very playful or cooperative with DateTime
              //extract the year, month and day ourselves out of the passed parameter value
              int theYear = (DateTime.Parse(reservationdate)).Year;
              int theMonth = (DateTime.Parse(reservationdate)).Month;
              int theDay = (DateTime.Parse(reservationdate)).Day;

              var result = from eventitem in context.SpecialEvents
                           orderby eventitem.Description
                           select new ReservationsByDate() // a new instance for each specialevent row  on the table
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
              return result.ToList();
          }
        }
    }
}
