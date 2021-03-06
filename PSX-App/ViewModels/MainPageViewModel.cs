﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using PlayStation.Entities.User;
using PlayStation_App.Commands.Events;
using PlayStation_App.Commands.Friends;
using PlayStation_App.Commands.Live;
using PlayStation_App.Commands.Messages;
using PlayStation_App.Commands.Navigation;
using PlayStation_App.Commands.Trophies;
using PlayStation_App.Common;
using PlayStation_App.Models;
using PlayStation_App.Models.Authentication;
using PlayStation_App.Tools;

namespace PlayStation_App.ViewModels
{
    public class MainPageViewModel : NotifierBase
    {
        public MainPageViewModel()
        {
            IsLoggedIn = false;
            if (DesignMode.DesignModeEnabled)
            {
                PopulateDesignMenu();
            }
            else
            {
                PopulateLoginMenu();
            }
        }

        private void PopulateDesignMenu()
        {
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Icon = "/Assets/Icons/Home.png",
                    Name = "ホーム",
                    Command = new NavigateToWhatsNewPage()
                }
            };
        }

        private void PopulateLoginMenu()
        {
            MenuItems = new List<MenuItem>();
        }

        private bool _isSplitViewPaneOpen;

        public bool IsSplitViewOpen
        {
            get { return _isSplitViewPaneOpen; }
            set
            {
                SetProperty(ref _isSplitViewPaneOpen, value);
                OnPropertyChanged();
            }
        }

        public void PopulateMenu()
        {
            var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
            MenuItems = new List<MenuItem>()
            {
                new MenuItem()
                {
                    Icon = "/Assets/Icons/Home.png",
                    Name = loader.GetString("Home/Text"),
                    Command = new NavigateToWhatsNewPage()
                },
                //new MenuItem()
                //{
                //    Icon = "/Assets/Icons/Event.png",
                //    Name = loader.GetString("Events/Text"),
                //    Command = new NavigateToEventsPageCommand()
                //},
                new MenuItem()
                {
                    Icon = "/Assets/Icons/Friends.png",
                    Name = loader.GetString("Friends/Text"),
                    Command = new NavigateToFriendsPage()
                },
                new MenuItem()
                {
                    Icon = "/Assets/Icons/Trophy.png",
                    Name = loader.GetString("Trophy/Text"),
                    Command = new NavigateToTrophiesPage()
                },
                new MenuItem()
                {
                    Icon = "/Assets/Icons/Messenger.png",
                    Name = loader.GetString("Messenger/Text"),
                    Command = new NavigateToMessagesViewCommand()
                },
                new MenuItem()
                {
                    Icon = "/Assets/Icons/Live.png",
                    Name = loader.GetString("LiveFromPlayStation/Text"),
                    Command = new NavigateToLiveFromPlaystationPage()
                },
                new MenuItem()
                {
                    Icon = "/Assets/Icons/AddFriendLink.png",
                    Name = loader.GetString("InviteFriendsToPsn/Text"),
                    Command = new NavigateToFriendLinkPageCommand()
                },
                new MenuItem()
                {
                    Icon = "/Assets/Icons/SignOut.png",
                    Name = loader.GetString("Signout/Text"),
                    Command = new NavigateToSelectAccountCommand()
                }
            };
            SwipeableSplitView?.UpdateLayout();
            SwipeableSplitView?.UpdateMenuList();
        }
        private AccountUser _currentUser;

        public AccountUser CurrentUser
        {
            get { return _currentUser; }
            set
            {
                SetProperty(ref _currentUser, value);
                OnPropertyChanged();
            }
        }

        public SwipeableSplitView SwipeableSplitView { get; set; }

        public LoadFriendsPageWithDetail LoadFriendsPageWithDetail { get; set; } = new LoadFriendsPageWithDetail();

        public UserAuthenticationEntity CurrentTokens => new UserAuthenticationEntity(CurrentUser.AccessToken, CurrentUser.RefreshToken, CurrentUser.RefreshDate);

        private bool _isLoggedIn;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set
            {
                SetProperty(ref _isLoggedIn, value);
                OnPropertyChanged();
            }
        }
        private List<MenuItem> _menuItems;
        public List<MenuItem> MenuItems
        {
            get { return _menuItems; }
            set
            {
                SetProperty(ref _menuItems, value);
                OnPropertyChanged();
            }
        }
    }
}
