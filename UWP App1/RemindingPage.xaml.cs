using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using BusinessCalendar.Models;
using Windows.Storage;
using ModelsLibrary.Models;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace BusinessCalendar
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class RemindingPage : Page
    {
        public RemindingPage()
        {
            this.InitializeComponent();            
        }

        Reminding currentReminding;

        async private System.Threading.Tasks.Task SetLocalMedia()
        {
            var openPicker = new Windows.Storage.Pickers.FileOpenPicker();

            openPicker.FileTypeFilter.Add(".wmv");
            openPicker.FileTypeFilter.Add(".mp4");
            openPicker.FileTypeFilter.Add(".wma");
            openPicker.FileTypeFilter.Add(".mp3");

            var file = await openPicker.PickSingleFileAsync();
            // mediaPlayer is a MediaElement defined in XAML
            if (file != null)
            {
                var stream = await file.OpenAsync(FileAccessMode.Read);
                mediaPlayer.SetSource(stream, file.ContentType);
                Song.Text = file.Name;
                currentReminding.AudioPath = file.Path;
            }
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SetLocalMedia();
            }
            catch (Exception)
            {
            }
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Play();
            Play.Visibility = Visibility.Collapsed;
            Stop.Visibility = Visibility.Visible;
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            Play.Visibility = Visibility.Visible;
            Stop.Visibility = Visibility.Collapsed;
        }

        public void ShowReminding()
        {
            title.Text = currentReminding.Name;
            delay.Time = currentReminding.Delay;
            Song.Text = currentReminding.Composition;
            LoadFromStringAsync(currentReminding.AudioPath);
        }

        public async void LoadFromStringAsync(string path)
        {
            try
            {
                var file = await StorageFile.GetFileFromPathAsync(path);
                var stream = await file.OpenAsync(FileAccessMode.Read);
                mediaPlayer.SetSource(stream, file.ContentType);
            }
            catch (Exception)
            {
                Song.Text = "Аудіо відсутнє";
            }
            
        }

        public void ShowListOfRemindings()
        {
            using (var remindingsContext=new BusinessCalendarContext())
            {
                ListOfRemindings.ItemsSource = remindingsContext.Remindings.ToList();
            }                
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            currentReminding = new Reminding();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            currentReminding.Name = title.Text;
            currentReminding.Delay = delay.Time;
            currentReminding.Composition = Song.Text;
            using (var remindingsContext = new BusinessCalendarContext())
            {
                foreach (var item in remindingsContext.Remindings)
                {
                    if (item.Id==currentReminding.Id)
                    {
                        remindingsContext.Remindings.Remove(item);
                    }
                }
                remindingsContext.Remindings.Add(currentReminding);
                remindingsContext.SaveChanges();

            }
            ShowListOfRemindings();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            using (var remindingsContext = new BusinessCalendarContext())
            {
                foreach (var item in remindingsContext.Remindings)
                {
                    if (item.Id == currentReminding.Id)
                    {
                        remindingsContext.Remindings.Remove(item);
                    }
                }
                remindingsContext.SaveChanges();
            }
            Page_Loaded(sender,e);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            currentReminding = (Reminding)ListOfRemindings.Items.FirstOrDefault();
            if (currentReminding==null)
            {
                currentReminding = new Reminding();
            }
            ShowReminding();
            ShowListOfRemindings();
        }

        private void ListOfRemindings_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                currentReminding = (Reminding)ListOfRemindings.SelectedItem;
                ShowReminding();
            }
            catch (Exception)
            {
            }
        }
    }
}
