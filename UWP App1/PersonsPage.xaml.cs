using BusinessCalendar.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Storage;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;
using static System.Diagnostics.Debug;
using ModelsLibrary.Models;
//using static BusinessCalendar.Service.ImageConverter;

namespace BusinessCalendar
{
    public sealed partial class PersonsPage : Page
    {
        Person currentPerson;
        List<Person> persons = new List<Person>();

        public PersonsPage()
        {
            this.InitializeComponent();
        }

        private async void Page_LoadedAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ShowPersons();
            currentPerson = (Person)personsList.Items.FirstOrDefault();
            if (currentPerson == null)
                currentPerson = new Person();
            await PersonsDetailsAsync();
        }

        private void setDefaultImage()
        {
            //Image.Source = new BitmapImage(new Uri("~/Assets/Images/default_member_image.png"));
        }

        private void ShowPersons()
        {
            using (var eventsContext = new BusinessCalendarContext())
            {                
                persons = eventsContext.Persons.ToList();

                if (persons.Count != 0)
                {
                    personsList.ItemsSource = persons;
                }
                else
                {
                }               
            }

        }

        private async Task PersonsDetailsAsync()
        {

            personInformation.DataContext = currentPerson;
            try
            {
                Image.Source = await ImageFromBytes(currentPerson.Photo);
            }
            catch (Exception)
            {
                setDefaultImage();
            }
        }

        public async static Task<BitmapImage> ImageFromBytes(Byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                await stream.WriteAsync(bytes.AsBuffer());
                WriteLine(stream.Position);
                stream.Seek(0);
                await image.SetSourceAsync(stream);
            }
            return image;
        }

        private void Delete_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            using (var eventsContext = new BusinessCalendarContext())
            {
                try
                {
                    Person item = (Person)personsList.SelectedItem;
                    persons.Remove(item);
                    eventsContext.Persons.Remove(item);
                    eventsContext.SaveChanges();
                    ShowPersons();
                }
                catch (Exception)
                {
                }
            }
        }

        private async void Button_ClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            StorageFile file = await picker.PickSingleFileAsync();  
            using (var inputStream = await file.OpenSequentialReadAsync())
            {
                var readStream = inputStream.AsStreamForRead();
                byte[] buffer = new byte[readStream.Length];
                await readStream.ReadAsync(buffer, 0, buffer.Length);
                currentPerson.Photo = buffer;
            }            
                await PersonsDetailsAsync();            
        }

        private async void New_ClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            currentPerson = new Person();

            await PersonsDetailsAsync();
            //personsList.Items.Add(new ListBoxItem());
        }

        private async void Save_ClickAsync(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (Name.Text == "" | Surname.Text == "")
            {
                var dialog = new ContentDialog();
                dialog.Title = "Введіть ім'я або прізвище";
                dialog.PrimaryButtonText = "ОК";
                await dialog.ShowAsync();
                return;
            }
            currentPerson.Name = Name.Text;
            currentPerson.Surname = Surname.Text;
            currentPerson.Phone = Phone.Text;
            using (var eventsContext = new BusinessCalendarContext())
            {
                foreach (Person person in eventsContext.Persons)
                {
                    if (person.Id == currentPerson.Id)
                    {
                        eventsContext.Persons.Remove(person);
                        eventsContext.SaveChanges();
                        break;
                    }
                }
                eventsContext.Persons.Add(currentPerson);
                eventsContext.SaveChanges();
            }
            ShowPersons();
        }

        private void personsList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //PersonsDetails((Person)personsList.SelectedItem);
        }

        private async void personsList_TappedAsync(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            currentPerson = (Person)personsList.SelectedItem;
            await PersonsDetailsAsync();
        }

        private void Image_ImageFailed(object sender, Windows.UI.Xaml.ExceptionRoutedEventArgs e)
        {
            
        }

        private void Image_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Image img = sender as Image;
            BitmapImage bitmapImage = new BitmapImage();
            img.Width = bitmapImage.DecodePixelWidth = 150;
            bitmapImage.UriSource = new Uri(img.BaseUri, "Assets/Images/Imagesdefault_member_image.png");

        }
    }
}
