using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.IO;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
using ModelsLibrary.Models;
using System.Globalization;

namespace BusinessCalendar.Service
{
    class ReportService
    {
        static async void Save(MemoryStream stream, string filename)
        {
            stream.Position = 0;
            StorageFile stFile;
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.DefaultFileExtension = ".docx";
            savePicker.SuggestedFileName = filename;
            savePicker.FileTypeChoices.Add("Word Documents", new List<string>() { ".docx" });
            stFile = await savePicker.PickSaveFileAsync();
            if (stFile != null)
            {
                using (IRandomAccessStream zipStream = await stFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    using (Stream outstream = zipStream.AsStreamForWrite())
                    {
                        byte[] buffer = stream.ToArray();
                        outstream.Write(buffer, 0, buffer.Length);
                        outstream.Flush();
                    }
                }
            }
        }


        public static async void GenerateReportAsync(List<Event> events)
        {
            WordDocument document = new WordDocument();
            IWSection section = document.AddSection();
            IWParagraph Header = section.AddParagraph();
            CultureInfo ukUA = new CultureInfo("ukr-uk");
            IWTextRange HeaderStyle = Header.AppendText("Розпорядок на " + events.FirstOrDefault().Date.ToString("d", ukUA) + "\n\n\n");// DayOfWeek.ToString()+"\n\n\n");
            
            HeaderStyle.CharacterFormat.FontSize = 30;
            HeaderStyle.CharacterFormat.Bold = true;
            foreach (var item in events)
            {
                IWParagraph title = section.AddParagraph();
                IWTextRange TitleStyle = title.AppendText(item.Duration+"   "+item.Title+"\n");
                TitleStyle.CharacterFormat.FontSize = 24;
                TitleStyle.CharacterFormat.Bold = true;             
                IWParagraph description = section.AddParagraph();
                IWTextRange MainStyle = description.AppendText(item.Title+"\n"+item.Adress.Adress+"\n"+item.Person.DisplayMember+"\n\n\n\n");
                TitleStyle.CharacterFormat.FontSize = 18;
            }
                MemoryStream stream = new MemoryStream();
            //Save the document into memory stream
            await document.SaveAsync(stream, FormatType.Docx);
            //Close the documents
            document.Close();
            //Save the stream into Word document file
            Save(stream, "План на " + events.FirstOrDefault().Date.ToString("d", ukUA));                       
        }
    }
}
