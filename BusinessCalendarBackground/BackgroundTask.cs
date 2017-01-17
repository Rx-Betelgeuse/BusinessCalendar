using ModelsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace BusinessCalendarBackground
{
    public class BackgroundTask: IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            var next = GetNext();

            UpdateTile(next);

            _deferral.Complete();
        }

        public static void UpdateTile(Event next)
        {
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            XmlDocument xml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150BlockAndText02);
            if (next != null)
            {
                xml.GetElementsByTagName("text")[0].InnerText = next.Description;
                xml.GetElementsByTagName("title")[0].InnerText = next.Title;
                xml.GetElementById("5").InnerText = next.StartDate.ToString();
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
                    if (item.StartDate > DateTime.Now.TimeOfDay)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

    }
}
