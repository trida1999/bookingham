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
    /// Логика взаимодействия для BinWindow.xaml
    /// </summary>
    public partial class BinWindow : Window
    {
        HotelsAndUsers.Core.Interfaces.IRepository _repo = Factory.Instance.GetRepository();
        public Guest Guest { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

        public BinWindow(Guest guest, DateTime checkin, DateTime checkout)
        {
            InitializeComponent();
            Guest = guest;
            CheckInDate = checkin;
            CheckOutDate = checkout;
            roomsList.ItemsSource = _repo.BinRooms;
        }

        private void ButtonProceed_Click(object sender, RoutedEventArgs e)
        {
            ////зарегать booking
            if (Guest == null)
            {
                MessageBox.Show("To book these rooms you need to sign in first", "Error");               
                return;
            }

            else
            {
                var confirmationWindow = new ConfirmationWindow(Guest, CheckInDate, CheckOutDate);
                confirmationWindow.Show();
                this.Close();                             
            }
        }

        private void RoomsListButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
