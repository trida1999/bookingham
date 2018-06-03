﻿using HotelsAndUsers.Core.Helpers;
using HotelsAndUsers.Core.Interfaces;
using HotelsAndUsers.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsAndUsers.Core
{
    internal class DBRepository : IRepository
    {
        public List<Guest> Guests { get; set; }
        public List<Hotel> Hotels { get; set; }
        public Context Context { get; set; }

        public IEnumerable<Guest> _guests => Guests;
        public IEnumerable<Hotel> _hotels => Hotels;

        public DBRepository()
        {
            try
            {
                using (Context = new Context())
                {
                    Guests = Context.Guests.ToList();
                    Hotels = Context.Hotels.ToList();                   
                }
            }
            catch
            {
                Guests = new List<Guest>();
                Hotels = new List<Hotel>();
            }
        }

        public Guest Authorize(string login, string password)
        {
            var user = Guests.FirstOrDefault(u => u.Email == login && Hash.GetHash(u.Password) == Hash.GetHash(password));
            return user;
        }

        public void RegisterGuest(Guest guest)
        {
            using (var context = new Context())
            {
                if (Guests == null)
                {
                    List<Guest> Guests = new List<Guest>();
                    context.Guests.Add(guest);
                    this.Guests = Guests;
                }
                else
                {
                    context.Guests.Add(guest);
                }
                context.SaveChanges();
            }
        }

        public void SearchEngine(List<Room> Rooms,decimal MaxPrice,DateTime CheckInDate, DateTime CheckOutDate, out List<Room> SuitableRooms, out int PossibleBeds)
        {
            SuitableRooms = new List<Room>();
            PossibleBeds = 0;
            foreach (var r in Rooms)
            {
                if (r.Reservations == null)
                {
                    r.Reservations = new List<Reservation>();
                }
                for (int i = 1; i <= r.Reservations.Count + 1; i++)
                {
                    if ((CheckInDate >= r.Reservations[i - 1].CheckOutDate) && (CheckInDate < r.Reservations[i].CheckInDate) && (r.PriceForNight <= MaxPrice))
                    {
                        SuitableRooms.Add(r);
                        PossibleBeds += r.BedNumber;
                        break;
                    }
                }
            }
        }

        public int CheckGuest(string login, string password)
        {
            if (Context.Guests != null)
            {
                var user = Guests.FirstOrDefault(u => u.Email == login && Hash.GetHash(u.Password) == Hash.GetHash(password));
                if (user != null)
                {
                    return 1;
                }
                return 0;
            }
            else
            {
                return -1;
            }
        }
    }
}
