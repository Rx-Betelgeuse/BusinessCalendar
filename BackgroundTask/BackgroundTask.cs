using System;
using System.Collections.Generic;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
//using BackgroundTask.Models;
//using BusinessCalendar.Models;

namespace BackgroundTask
{
    public sealed class BusinessCalendarBackgroundTask:IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {            
            BackgroundTaskDeferral _deferral = taskInstance.GetDeferral();

            UpdateTile();

            _deferral.Complete();
        }

        public static void UpdateTile()
        {            
            var updater = TileUpdateManager.CreateTileUpdaterForApplication();
            updater.EnableNotificationQueue(true);
            updater.Clear();
            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            ShortEvent next = (ShortEvent)localSettings.Values["event"];
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

        

       

    }
}

