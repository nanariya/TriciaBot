using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Calendar;

namespace NTLIB
{
    public class GCal
    {
        private CalendarService _gCal { get; set; }

        public GCal(String GmailID, String GmailPass)
        {
            this._gCal = new CalendarService("nanariya-tool-engine");
            _gCal.setUserCredentials(GmailID, GmailPass);
        }

        public void WriteSchedule(String title, String description, String location, DateTime startTime, DateTime endTime)
        {
            EventEntry entry = new EventEntry(title, description, location);
            When eventTime = new When(startTime, endTime);
            entry.Times.Add(eventTime);
            Uri url = new Uri("https://www.google.com/calendar/feeds/tricia.disraeli@gmail.com/private/full");
            AtomEntry result = _gCal.Insert(url, entry);
        }
    }
}
