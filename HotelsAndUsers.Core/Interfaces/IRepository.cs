﻿using HotelsAndUsers.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelsAndUsers.Core.Interfaces
{
    public interface IRepository
    {
        IEnumerable<Guest> _guests { get; }
        IEnumerable<Hotel> _hotels { get; }
        List<Hotel> BinHotels { get; set; }

        //int CheckGuest(string login, string password);
        void Authorize(string login, string password, out Guest guest, out Hotel hotel);
        decimal TotalPrice(Room Room, DateTime InData, DateTime OutData);
        void RegisterGuest(Guest guest);
        void RemoveGuest(Guest guest);
        void AddBooking(Guest guest, Booking booking);
        void AddReservation(Room room, Reservation reservation, DateTime CheckInDate, DateTime CheckOutDate, out int k);
        void UpdateHotel(Hotel hotel);
        void SearchEngine(List<Room> Rooms, decimal MaxPrice, DateTime CheckInDate, DateTime CheckOutDate, out List<Room> SuitableRooms, out int PossibleBeds);
    }
}
