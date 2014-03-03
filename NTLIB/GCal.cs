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
        private String _gmailID { get; set; }

        public GCal(String GmailID, String GmailPass)
        {
            this._gCal = new CalendarService("nanariya-tool-engine");
            _gCal.setUserCredentials(GmailID, GmailPass);
            _gmailID = GmailID;

        }

        public void WriteSchedule(String title, String description, String location,  List<String> guestEmail, DateTime startTime, DateTime endTime)
        {
            EventEntry entry = new EventEntry(title, description, location);
            When eventTime = new When(startTime, endTime);
            entry.Times.Add(eventTime);
            guestEmail.ForEach((mail) =>
            {
                Who guest = new Who();
                guest.Email = mail;
                guest.Rel = description;
                entry.Participants.Add(guest);
            });
            
            Uri url = new Uri("https://www.google.com/calendar/feeds/" + this._gmailID + "/private/full");
            AtomEntry result = _gCal.Insert(url, entry);
        }
        public IEnumerable<GCalItem> ReadSchedule(DateTime startTime, DateTime endTime, Boolean allDays) 
        { 
            EventQuery query = new EventQuery();
            query.Uri = new Uri("https://www.google.com/calendar/feeds/" + this._gmailID + "/private/full");
            query.StartTime = startTime;
            query.EndTime = endTime.AddDays(1);
            query.SortOrder = CalendarSortOrder.descending;
            query.SingleEvents = true;
            EventFeed feeds = _gCal.Query(query);
            while (feeds != null && feeds.Entries.Count > 0)
            {
                foreach(EventEntry entry in feeds.Entries)
                {
                    GCalItem item = new GCalItem
                    {
                            Title = entry.Title.Text,
                            AllDay = entry.Times[0].AllDay,
                            StartTime = entry.Times[0].StartTime,
                            EndTime = entry.Times[0].EndTime,
                            Location = entry.Locations[0].ValueString
                    };
                    List<String> guests = new List<String>();
                    foreach(Who w in entry.Participants)
                    {
                        guests.Add(w.Email);
                    }
                    item.GuestsEmail = guests;
                    yield return item;
                }
                if(feeds.NextChunk != null)
                {
                    query.Uri = new Uri(feeds.NextChunk);
                    feeds = _gCal.Query(query);
                }
                else
                {
                    feeds = null;
                }
            }
        }
    }
}
