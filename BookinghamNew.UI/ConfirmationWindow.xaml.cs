﻿using HotelsAndUsers.Core;
using HotelsAndUsers.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookinghamNew.UI
{
    /// <summary>
    /// Логика взаимодействия для ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        HotelsAndUsers.Core.Interfaces.IRepository _repo = Factory.Instance.GetRepository();
        public Guest Guest { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public ConfirmationWindow(Guest guest, DateTime checkin, DateTime checkout)
        {
            InitializeComponent();
            Guest = guest;
            CheckInDate = checkin;
            CheckOutDate = checkout;
            textBoxFirstName.Text = Guest.Name;
            textBoxLasNname.Text = Guest.Surname;
            textBoxEmail.Text = Guest.Email;
            
        }

        private void ExitToReservationButton(object sender, RoutedEventArgs e)
        {           
            this.Close();
        }

        private void ConfirmationList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxFirstName.Text))
            {
                MessageBox.Show("Name cannot be empty", "Error");
                textBoxFirstName.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(textBoxLasNname.Text))
            {
                MessageBox.Show("Surname cannot be empty", "Error");
                textBoxLasNname.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(textBoxEmail.Text))
            {
                MessageBox.Show("Email cannot be empty", "Error");
                textBoxEmail.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(textBoxPassportSeries.Text))
            {
                MessageBox.Show("PassportId cannot be empty", "Error");
                textBoxPassportSeries.Focus();
                return;
            }

            else if (string.IsNullOrWhiteSpace(textBoxPassportNumber.Text))
            {
                MessageBox.Show("PassportNumber cannot be empty", "Error");
                textBoxPassportNumber.Focus();
                return;
            }

            else
            {
                if ((textBoxFirstName.Text == Guest.Name) && (textBoxLasNname.Text == Guest.Surname))
                {
                    Guest.PassportId = textBoxPassportSeries.Text;
                    Guest.PassportNumber = textBoxPassportNumber.Text;
                    Guest.Country = textBoxCountry.Text;
                }

                else
                {
                    Guest guest = new Guest()
                    {
                        Name = textBoxFirstName.Text,
                        Surname = textBoxLasNname.Text,
                        Email = textBoxEmail.Text,
                        PassportId = textBoxPassportSeries.Text,
                        PassportNumber = textBoxPassportNumber.Text,
                        Country = textBoxCountry.Text,
                    };
                    _repo.RegisterGuest(guest);
                    Guest = guest;
                }
                foreach (var hotel in _repo._hotels)
                {
                    decimal totalPrice = 0;
                    List<Room> BookedRooms = new List<Room>();
                    foreach (var binhotel in _repo.BinHotels)
                    {
                        if (binhotel.HotelId == hotel.HotelId)
                        {
                            foreach (var room in binhotel.BinRooms)
                            {
                                BookedRooms.Add(room);
                                Reservation newReservation = new Reservation()
                                {
                                    GuestId = Guest.GuestId,
                                    RoomId = room.RoomId,
                                    CheckInDate = CheckInDate,
                                    CheckOutDate = CheckOutDate
                                };
                                _repo.AddReservation(room, newReservation, CheckInDate, CheckInDate, out int k);
                                totalPrice += _repo.TotalPrice(room, CheckInDate, CheckOutDate);
                            }                                                        
                        }
                    }

                    if (BookedRooms.Count != 0)
                    {
                        Booking NewBooking = new Booking
                        {
                            HotelId = hotel.HotelId,
                            GuestId = Guest.GuestId,
                            BookingTime = DateTime.Now,
                            CheckIn = CheckInDate,
                            CheckOut = CheckOutDate,
                            TotalPrice = totalPrice
                        };
                        _repo.AddBooking(Guest, NewBooking);
                    }
                }
                MessageBox.Show("Success", "Success");
                return;
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ButtonConfirm_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}
