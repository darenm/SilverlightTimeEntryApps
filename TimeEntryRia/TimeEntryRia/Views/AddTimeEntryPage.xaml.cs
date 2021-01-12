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
using TimeEntryRia.ViewModels;

namespace TimeEntryRia.Views
{
    public partial class AddTimeEntryPage : Page
    {
        public AddTimeEntryPage()
        {
            InitializeComponent();
            this.Title = ApplicationStrings.AddTimeEntryPageTitle;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var vm = new AddNewTimeEntryViewModel();
            vm.NewTimeEntry.Date = DateParam;
            this.DataContext = vm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private DateTime DateParam
        {
            get
            {
                var date = DateTime.Now;

                if (this.NavigationContext.QueryString.ContainsKey("date"))
                {
                    date = DateTime.Parse(this.NavigationContext.QueryString["date"] as string);
                }
                return date;
            }
        }
    }
}
