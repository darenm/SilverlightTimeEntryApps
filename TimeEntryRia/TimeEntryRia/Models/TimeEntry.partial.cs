using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TimeEntryRia.Web
{
    public partial class TimeEntry
    {
        // A fix for a bug in RIA Services that marks TimeStamp fields as required
        partial void OnCreated()
        {
            _lastUpdated = new byte[] { 0 };
        }
    }
}
