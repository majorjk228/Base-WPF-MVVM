using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using Macroscope.Views;

namespace Macroscope.ViewModels;

public class MainViewModel : ViewModelBase
{

    private ICommand _navigateToCameraCommand;

    public ICommand NavigateToCameraCommand
    {
        get
        {
            return _navigateToCameraCommand ?? (_navigateToCameraCommand = new DelegateCommand(OpenCameraPage));
        }
    }

    private void OpenCameraPage()
    {
        CameraPage cameraPage = new CameraPage();
        Application.Current.MainWindow.Content = cameraPage;
    }
}