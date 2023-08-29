using DevExpress.Mvvm;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;




namespace Macroscope.ViewModels;

public class CameraPageViewModel : ViewModelBase
{
    private BitmapImage _videoImage;

    public BitmapImage VideoImage
    {
        get { return _videoImage; }
        set
        {
            _videoImage = value;
            RaisePropertyChanged(nameof(VideoImage));
        }
    }

    public ICommand ShowVideo
    {
        get { return new DelegateCommand(() => { UpdateVideoStream(); }); }
    }

    private async void UpdateVideoStream()
    {
        try
        {

            // Создаем объект-запрос
            WebRequest request =
                WebRequest.Create(
                    "http://demo.macroscop.com:8080/mobile?login=root&channelid=2016897c-8be5-4a80-b1a3-7f79a9ec729c&resolutionX=640&resolutionY=480&fps=25");

            // Получаем ответ от сервера
            using (WebResponse response = await request.GetResponseAsync())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    byte[] buffer = new byte[4096];
                    int bytesRead;
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        // Создаем объект
                        MemoryStream memoryStream = new MemoryStream(buffer, 0, bytesRead);
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = memoryStream;
                        bitmapImage.EndInit();

                        // // Устанавливаем изображение в элемент Image
                        VideoImage = bitmapImage;
                    }

                    // Через временный файл
                    /*// Создаем временный файл для сохранения видео
                    string tempFilePath = Path.GetTempFileName();

                    // Сохраняем видео-поток во временном файле
                    using (FileStream fileStream = File.Create(tempFilePath))
                    {
                        await stream.CopyToAsync(fileStream);
                    }*/
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error occurred: {ex.Message}");
        }
    }
}


