using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;

namespace TimeEntryRia.Views
{
    public partial class TimeEntryPage : Page
    {
        public TimeEntryPage()
        {
            InitializeComponent();
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            weekStartingDatePicker.SelectedDate = DateTime.Now;
        }

        private void weekStartingDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && e.AddedItems[0] is DateTime)
            {
                var selectedDate = (DateTime)e.AddedItems[0];
                if (selectedDate.DayOfWeek == DayOfWeek.Monday)
                {
                    return;
                }

                var dayOffset = DayOfWeek.Monday - selectedDate.DayOfWeek;
                var newDate = selectedDate + TimeSpan.FromDays(dayOffset);
                weekStartingDatePicker.SelectedDate = newDate;
            }
        }

        private void DecreaseWeek_Click(object sender, RoutedEventArgs e)
        {
            weekStartingDatePicker.SelectedDate = weekStartingDatePicker.SelectedDate - TimeSpan.FromDays(7);
        }

        private void IncreaseWeek_Click(object sender, RoutedEventArgs e)
        {
            weekStartingDatePicker.SelectedDate = weekStartingDatePicker.SelectedDate + TimeSpan.FromDays(7);
        }

    }
}
