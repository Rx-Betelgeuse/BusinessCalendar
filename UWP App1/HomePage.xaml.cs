using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BusinessCalendar.Models;
using Windows.UI.Xaml.Controls.Maps;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Windows.Devices.Geolocation;
using System.Threading.Tasks;
using Windows.Services.Maps;
using WinRTXamlToolkit;
using WinRTXamlToolkit.AwaitableUI;
using WinRTXamlToolkit.Tools;
using BusinessCalendar.Service;
using ModelsLibrary.Models;
// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BusinessCalendar
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        List<Event> events;
        Event CurrentEvent;
        Person contact;
        public HomePage()
        {
            this.InitializeComponent();
            CurrentEvent = new Event();
        }

        private async Task<Geoposition> GetLocationAsync()
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };
            return await geolocator.GetGeopositionAsync();
        }

        private async Task<string> GetAdress(Geopoint pointToReverseGeocode)
        {
            MapLocationFinderResult result =
            await MapLocationFinder.FindLocationsAtAsync(pointToReverseGeocode);
            return result.Locations[0].Address.Street + " " + result.Locations[0].Address.StreetNumber + " " + result.Locations[0].Address.BuildingName;
        }

        private void ShowEvents(DateTimeOffset selectedDay)
        {
            using (var eventsContext = new BusinessCalendarContext())
            {
                try
                {
                    events = (from item in eventsContext.Events
                              where item.Date.Date == selectedDay.Date.Date
                              select item).OrderBy(x => x.StartDate).ToList();
                }
                catch (Exception)
                {
                    try
                    {
                        events = (from item in eventsContext.Events
                                  where item.Date.Date == selectedDay.Date.Date
                                  select item).ToList();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }

                Remindings.ItemsSource = eventsContext.Remindings.ToList();

                if (events.Count != 0)
                {
                    eventsList.ItemsSource = events;
                }
                else
                {
                    var error = new List<Event>();
                    error.Add(new Event() { Title = "На цей день нічого не заплановано" });
                    eventsList.ItemsSource = events;
                }
            }
        }
        private void Сalendar_SelectedDatesChanged(CalendarView sender, CalendarViewSelectedDatesChangedEventArgs args)
        {
            try
            {
                ShowEvents(calendar.SelectedDates.Last());
            }
            catch (Exception)
            {
            }

        }


        private async void Page_LoadedAsync(object sender, RoutedEventArgs e)
        {
            ShowEvents(DateTime.Now);
            await GetLocationAsync();
            DefaultPosition();
        }

        private async void DefaultPosition()
        {
            try
            {
                Geoposition position = await GetLocationAsync();
                map.Center = position.Coordinate.Point;
                SetPoint(position.Coordinate.Point, "Ваше розташування");
            }
            catch (Exception)
            {
                map.Visibility = Visibility.Collapsed;
            }
        }

        private void SetPoint(Geopoint location, string title)
        {
            map.MapElements.Clear();
            map.Center = location;
            MapIcon currentLocation = new MapIcon();
            currentLocation.Location = location;
            currentLocation.Title = title;
            map.MapElements.Add(currentLocation);
        }

        private void Add_Click_1(object sender, RoutedEventArgs e)
        {
            pivot.SelectedIndex = 1;
            CurrentEvent = new Event();
            ShovEvent();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var eventsContext = new BusinessCalendarContext())
            {
                try
                {
                    Event item = (Event)eventsList.SelectedItem;
                    events.Remove(item);
                    eventsContext.Events.Remove(item);
                    eventsContext.SaveChanges();
                    ShowEvents(calendar.SelectedDates.Last());
                }
                catch (Exception)
                {
                }
            }
        }

        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                using (var eventsContext = new BusinessCalendarContext())
                {
                    try
                    {
                        var matchingPersons = eventsContext.GetMatchingPersons(sender.Text);
                        sender.ItemsSource = matchingPersons.ToList();
                    }
                    catch (Exception)
                    {
                        
                    }
                    
                }
            }
        }

        private void SelectPersons(Person person)
        {
            if (person != null)
            {
                NoResults.Visibility = Visibility.Collapsed;
                ContactDetails.Visibility = Visibility.Visible;
                ContactName.Text = person.Name + " " + person.Surname;
            }
            else
            {
                NoResults.Visibility = Visibility.Visible;
                ContactDetails.Visibility = Visibility.Collapsed;
            }
        }
        /*
        public static void GetNext()
        {
            using (var events = new BusinessCalendarContext())
            {
                foreach (var item in events.Events.OrderBy(x => x.StartDate))
                {
                    if (item.StartDate > DateTime.Now.TimeOfDay)
                    {
                        var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                        localSettings.Values["event"] = (ShortEvent)item;
                    }
                }
            }
        }
        */

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                SelectPersons((Person)args.ChosenSuggestion);
            }
            else
            {
                using (var eventsContext = new BusinessCalendarContext())
                {
                    var matchingContacts = eventsContext.GetMatchingPersons(sender.Text);
                    SelectPersons(matchingContacts.FirstOrDefault());
                }
            }
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            contact = (Person)args.SelectedItem;
            sender.Text = contact.Name + " " + contact.Surname;
        }

        private async void map_MapElementClickAsync(MapControl sender, MapElementClickEventArgs args)
        {
            Adress.Text = await GetAdress(args.Location);
            SetPoint(args.Location, title.Text);
            CurrentEvent.Adress.Longitude = args.Location.Position.Longitude;
            CurrentEvent.Adress.Latitude = args.Location.Position.Latitude;
        }

        private void ShovEvent()
        {
            title.Text = CurrentEvent.Title;
            descriptioin.Text = CurrentEvent.Description;
            Person.PlaceholderText = CurrentEvent.Person.DisplayMember;
            contact = CurrentEvent.Person;
            Remindings.SelectedItem = CurrentEvent.Reminding;
            try
            {
                BasicGeoposition position = new BasicGeoposition();
                position.Latitude = CurrentEvent.Adress.Latitude;
                position.Longitude = CurrentEvent.Adress.Longitude;
                Geopoint point = new Geopoint(position);
                SetPoint(point, CurrentEvent.Adress.Adress);
                map.Center = point;
                map.ZoomLevel = 15;
            }
            catch (Exception)
            {
                DefaultPosition();
                //Geoposition position = GetLocationAsync();
                //map.Center = position.Coordinate.Point;
                //SetPoint(position.Coordinate.Point,GetAdress())
            }
        }

        private void eventsList_DoubleTapped(object sender, Windows.UI.Xaml.Input.DoubleTappedRoutedEventArgs e)
        {
            try
            {
                pivot.SelectedIndex = 1;
                CurrentEvent = (Event)eventsList.SelectedItem;
                ShovEvent();
            }
            catch (Exception)
            {
            }
        }

        private async void map_MapTappedAsync(MapControl sender, MapInputEventArgs args)
        {
            Adress.Text = await GetAdress(args.Location);
            SetPoint(args.Location, title.Text);
            CurrentEvent.Adress.Longitude = args.Location.Position.Longitude;
            CurrentEvent.Adress.Latitude = args.Location.Position.Latitude;
        }


        private async void Save_ClickAsync(object sender, RoutedEventArgs e)
        {
            CurrentEvent.Title = title.Text;
            CurrentEvent.Description = descriptioin.Text;
            CurrentEvent.Date = date.Date;
            CurrentEvent.StartDate = startTime.Time;
            CurrentEvent.FinishDate = finalTime.Time;
            CurrentEvent.Adress = new Location() { Longitude = map.Center.Position.Longitude, Latitude = map.Center.Position.Latitude, Adress = Adress.Text };
            CurrentEvent.Person = contact;
            CurrentEvent.Reminding = (Reminding)Remindings.SelectedItem;
            if (title.Text == "")
            {
                var dialog = new ContentDialog();
                dialog.Title = "Будь-ласка, введіть назву події";
                dialog.PrimaryButtonText = "Добре";
                await dialog.ShowAsync();
                return;
            }
            if (!CheckCollision(CurrentEvent))
            {
                var dialog = new ContentDialog();
                dialog.Title = "На цей час у вас вже призначені справи";
                dialog.PrimaryButtonText = "Добре";
                await dialog.ShowAsync();
                return;
            }
            using (var eventsContext = new BusinessCalendarContext())
            {
                foreach (var item in eventsContext.Events)
                {
                    if (item.Id == CurrentEvent.Id)
                    {
                        eventsContext.Events.Remove(item);
                    }
                }
                foreach (var item in eventsContext.Persons)
                {
                    if (item.Id == CurrentEvent.Id)
                    {
                        eventsContext.Persons.Remove(item);
                    }
                }
                foreach (var item in eventsContext.Remindings)
                {
                    if (item.Id == CurrentEvent.Id)
                    {
                        eventsContext.Remindings.Remove(item);
                    }
                }
                eventsContext.Locations.Add(CurrentEvent.Adress);
                eventsContext.SaveChanges();
                eventsContext.Events.Add(CurrentEvent);
                eventsContext.SaveChanges();
            }
            ShowEvents(date.Date);
            pivot.SelectedIndex = 0;
            //GetNext();
        }

        private void startTime_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (e.NewTime > finalTime.Time)
            {
                finalTime.Time = e.NewTime;
            }
        }

        private void finalTime_TimeChanged(object sender, TimePickerValueChangedEventArgs e)
        {
            if (e.NewTime < startTime.Time)
            {
                startTime.Time = e.NewTime; ;
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            CurrentEvent = new Event();
            ShovEvent();
        }

        private bool CheckCollision(Event checkedEvent)
        {
            using (var eventsContext = new BusinessCalendarContext())
            {
                events = (from item in eventsContext.Events
                          where item.Date.Date == checkedEvent.Date.Date
                          select item).ToList();
            }
            events.Add(checkedEvent);
            events.OrderBy(x => x.StartDate);
            int index = events.LastIndexOf(checkedEvent);
            try
            {
                if (events[index].StartDate < events[index - 1].FinishDate)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            try
            {
                if (events[index].FinishDate > events[index + 1].StartDate)
                {
                    return false;
                }
            }
            catch (Exception)
            {

            }
            return true;
        }

        private async void Print_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (events.Count != 0)
            {

                ReportService.GenerateReportAsync(events);
            }
            else
            {
                var dialog = new ContentDialog();
                dialog.Title = "Список подій на цей день пустий";
                dialog.PrimaryButtonText = "Добре";
                await dialog.ShowAsync();
                return;
            }
        }
    }
}
