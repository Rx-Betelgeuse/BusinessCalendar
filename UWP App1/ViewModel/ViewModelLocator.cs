﻿using Windows.Storage;

using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;

using Microsoft.Practices.ServiceLocation;

using BusinessCalendar.Models;
using BusinessCalendar.Service;

namespace BusinessCalendar.ViewModel
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // setup app services
            SimpleIoc.Default.Register<INavigationService2>(() =>
            {
                var nav = new BusinessCalendar.Service.NavigationService();
                nav.Configure("Home", typeof(HomePage));
                nav.Configure("People", typeof(PersonsPage));
                nav.Configure("Settings", typeof(SettingsPage));
                nav.Configure("Reminding", typeof(RemindingPage));
                return nav;
            });
            SimpleIoc.Default.Register<IDialogService, DialogService>();

            // setup models
            SimpleIoc.Default.Register<IAbout, About>();
            SimpleIoc.Default.Register<ISettings>(() => new Settings(ApplicationData.Current.LocalSettings));


            // setup view models
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public MainViewModel Main => ServiceLocator.Current.GetInstance<MainViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public AboutViewModel About => ServiceLocator.Current.GetInstance<AboutViewModel>();

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This non-static member is needed for data binding purposes.")]
        public SettingsViewModel Settings => ServiceLocator.Current.GetInstance<SettingsViewModel>();
    }
}
