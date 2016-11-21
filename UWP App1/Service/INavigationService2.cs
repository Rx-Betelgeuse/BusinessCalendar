using System;

using Windows.UI.Xaml.Controls;

using GalaSoft.MvvmLight.Views;

namespace UWP_App1.Service
{
    public interface INavigationService2 : INavigationService
    {
        event EventHandler Navigated;

        Frame Root { get; set; }
    }
}
