using DevExpress.Mvvm;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DotNetProjects.MjpegProcessor;

namespace Macroscope.ViewModels;

public class CameraPageViewModel : ViewModelBase
{

    private BitmapFrame _videoImage;

    public BitmapFrame VideoImage
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
        get
        {
            return new DelegateCommand(() =>
            {
                UpdateVideoStream();
            });
        }
    }


    private async void UpdateVideoStream()
    {
        try
        {
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync("http://demo.macroscop.com:8080/mobile?login=root&channelid=2016897c-8be5-4a80-b1a3-7f79a9ec729c&resolutionX=640&resolutionY=480&fps=25"))
                {
                    using (var stream = await response.Content.ReadAsStreamAsync())
                    {
                        // Разбираем поток и выделяем кадры
                        var decoder = new JpegBitmapDecoder(stream, BitmapCreateOptions.None, BitmapCacheOption.Default);

                        foreach (var frame in decoder.Frames)
                        {
                            // Отображение текущего кадра в элементе VideoImage
                            VideoImage = frame;
                        }
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