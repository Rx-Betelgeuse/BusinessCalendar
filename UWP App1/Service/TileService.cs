using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsExtensions.Tiles;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using BusinessCalendar.Models;
using ModelsLibrary.Models;

namespace BusinessCalendar.Service
{
    class TileService
    {
        public static void TileBuilder()
        {
            XmlDocument xml = new XmlDocument();
            Event next = GetNext();
            if (next!=null)
            {
                xml.LoadXml("");
                var updater = TileUpdateManager.CreateTileUpdaterForApplication();
                TileNotification tile = new TileNotification(xml);
                updater.Update(tile);
            }
        }

        public static Event GetNext()
        {
            using (var events = new BusinessCalendarContext())
            {
                foreach (var item in events.Events.OrderBy(x => x.StartDate))
                {
                    if (item.StartDate>DateTime.Now.TimeOfDay)
                    {
                        return item;
                    }
                }
            }
            return null;
        }
    }
}
