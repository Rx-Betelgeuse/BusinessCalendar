﻿using System;
using System.Collections.ObjectModel;

using Windows.System;

using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;

using BusinessCalendar.Service;

namespace BusinessCalendar.ViewModel
{
    public class MainViewModel : ViewModel
    {
        public MainViewModel(INavigationService2 navigationService)
            : base(navigationService)
        {
            MainNavItems = new ObservableCollection<NavItem>()
            {
                new NavItem()
                {
                    Text = "Події",
                    ButtonText = "\uE162",
                    Command = new RelayCommand(() => NavigateTo("Home"), () => ActivePage != "Home" )
                },

                new NavItem()
                {
                    Text = "Люди",
                    ButtonText = "\uE13D",
                    Command = new RelayCommand(() => NavigateTo("People"), () => ActivePage != "People" )
                },

                 new NavItem()
                {
                    Text = "Нагадування",
                    ButtonText = "\uEA8F",
                    Command = new RelayCommand(() => NavigateTo("Reminding"), () => ActivePage != "Reminding" )
                }
            };

            SecondaryNavItems = new ObservableCollection<NavItem>()
            {
                new NavItem()
                {
                    Text = "Visit website",
                    ButtonText = "\uE909",
                    Command = new RelayCommand(() =>
                    {
                        var uri = new Uri("http://google.com/", UriKind.Absolute);
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                        Launcher.LaunchUriAsync(uri);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    })
                },
                new NavItem()
                {
                    Text = "Settings",
                    ButtonText = "\uE713",
                    Command = new RelayCommand(() => NavigateTo("Settings"), () => ActivePage != "Settings" )
                }
            };

            navigationService.Navigated += NavigationService_Navigated;

            // do this asynchronously on the dispatcher so that the UI is full instantiated
            // before we attempt to navigate (see comment in NavControl.xaml.cs)
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            DispatcherHelper.RunAsync(() => base.NavigateTo("Home"));
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private bool _isNavOpen;
        public bool IsNavOpen
        {
            get { return _isNavOpen; }
            set { Set<bool>(ref _isNavOpen, value); }
        }

        public RelayCommand ToggleNavCommand => new RelayCommand(() => IsNavOpen = !IsNavOpen);

        public ObservableCollection<NavItem> MainNavItems { get; private set; }

        public ObservableCollection<NavItem> SecondaryNavItems { get; private set; }

        private void NavigationService_Navigated(object sender, EventArgs e)
        {
            foreach (var nav in MainNavItems)
            {
                nav.Command.RaiseCanExecuteChanged();
                nav.IsSelected = ActivePage == nav.Text;
            }

            foreach (var nav in SecondaryNavItems)
            {
                nav.Command.RaiseCanExecuteChanged();
                nav.IsSelected = ActivePage == nav.Text;
            }
        }
    }
}
