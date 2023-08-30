using DevExpress.Mvvm;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace Macroscope.ViewModels;

public class CameraPageViewModel : ViewModelBase
{
    private CancellationTokenSource cancellationTokenSource;

    private static readonly object memoryStreamLock = new object();

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
        cancellationTokenSource = new CancellationTokenSource();

        string url =
            "http://demo.macroscop.com:8080/mobile?login=root&channelid=2016897c-8be5-4a80-b1a3-7f79a9ec729c&resolutionX=640&resolutionY=480&fps=25";

        using (var httpClient = new HttpClient())
        {
            try
            {
                using (var stream = await httpClient.GetStreamAsync(url).ConfigureAwait(false))
                using (var memoryStream = new MemoryStream())
                {
                    var buffer = new byte[1024];
                    int bytesRead;

                    while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token).ConfigureAwait(false)) > 0)
                    {
                        memoryStream.Write(buffer, 0, bytesRead);

                        byte[] frameBytes = await memoryStream.TryGetMjpegFrameAsync();
                        if (frameBytes != null)
                        {
                            Application.Current.Dispatcher.Invoke(() => { DisplayFrame(frameBytes); });

                            // Сброс memoryStream для следующего кадра
                            memoryStream.SetLength(0);
                            memoryStream.Seek(0, SeekOrigin.Begin);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred: {ex.Message}");
            }
        }
    }

    private void DisplayFrame(byte[] frameBytes)
    {
        try
        {
            using (MemoryStream memoryStream = new MemoryStream(frameBytes))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = memoryStream;
                bitmapImage.EndInit();

                VideoImage = bitmapImage;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error displaying frame: {ex.Message}");
        }
    }
}


