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
using TimeEntryRia.Web;
using TimeEntryRia.Services;

namespace TimeEntryRia.Views
{
    public partial class TimeEntryPage : Page
    {
        TimesheetSummaryServiceClient _service;

        public TimeEntryPage()
        {

            InitializeComponent();
            _service = new TimesheetSummaryServiceClient();
            _service.GetWeekSummaryCompleted += new EventHandler<GetWeekSummaryCompletedEventArgs>(service_GetWeekSummaryCompleted);
        }

        void service_GetWeekSummaryCompleted(object sender, GetWeekSummaryCompletedEventArgs e)
        {
            if (!e.Cancelled && e.Error == null)
            {
                WeekSummaryDataGrid.ItemsSource = e.Result;
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
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
                    WeekOfLabel.Text = string.Format("Week {0} of {1}", 
                        TimeEntry.GetIso8601WeekOfYear(selectedDate),
                        selectedDate.Year);
                    _service.GetWeekSummaryAsync(53, selectedDate);

                    return;
                }

                var dayOffset = DayOfWeek.Monday - selectedDate.DayOfWeek;
                var newDate = selectedDate + TimeSpan.FromDays(dayOffset);
                weekStartingDatePicker.SelectedDate = newDate;
                WeekOfLabel.Text = string.Format("Week {0} of {1}",
                    TimeEntry.GetIso8601WeekOfYear(newDate),
                    selectedDate.Year);

                _service.GetWeekSummaryAsync(53, newDate);
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
