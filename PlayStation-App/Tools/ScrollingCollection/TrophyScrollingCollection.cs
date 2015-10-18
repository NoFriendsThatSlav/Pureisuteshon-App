﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml.Data;
using Newtonsoft.Json;
using PlayStation.Managers;
using PlayStation_App.Models.Response;
using PlayStation_App.Models.TrophyDetail;
using PlayStation_App.Properties;
using PlayStation_App.Tools.Helpers;
using Xamarin;

namespace PlayStation_App.Tools.ScrollingCollection
{
    public class TrophyScrollingCollection : ObservableCollection<TrophyTitle>, ISupportIncrementalLoading,
        INotifyPropertyChanged
    {
        public int Offset;
        private bool _isEmpty;
        private bool _isLoading;

        public TrophyScrollingCollection()
        {
            HasMoreItems = true;
            IsLoading = false;
        }

        public string CompareUsername { get; set; }
        public string Username { get; set; }
        public int MaxCount { get; set; }

        public bool IsLoading
        {
            get { return _isLoading; }

            private set
            {
                _isLoading = value;
                NotifyPropertyChanged("IsLoading");
            }
        }

        public bool IsEmpty
        {
            get { return _isEmpty; }
            set
            {
                _isEmpty = value;
                OnPropertyChanged();
            }
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        public IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {
            return LoadDataAsync(count).AsAsyncOperation();
        }

        public bool HasMoreItems { get; set; }

        private async Task<LoadMoreItemsResult> LoadDataAsync(uint count)
        {
            if (!IsLoading)
            {
                using (var handle = Insights.TrackTime("PagingTrophyList"))
                {
                    await LoadTrophies(Username);
                }
            }
            var ret = new LoadMoreItemsResult {Count = count};
            return ret;
        }


        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task<bool> LoadTrophies(string username)
        {
            Offset = Offset + MaxCount;
            IsLoading = true;
            var trophyManager = new TrophyManager();
            var trophyResultList = await trophyManager.GetTrophyList(username, CompareUsername, Offset, Locator.ViewModels.MainPageVm.CurrentTokens);
            await AccountAuthHelpers.UpdateTokens(Locator.ViewModels.MainPageVm.CurrentUser, trophyResultList);
            var trophyList = JsonConvert.DeserializeObject<TrophyDetailResponse>(trophyResultList.ResultJson);
            if (trophyList == null)
            {
                //HasMoreItems = false;
                IsEmpty = true;
                return false;
            }
            foreach (var trophy in trophyList.TrophyTitles)
            {
                Add(trophy);
            }
            if (trophyList.TrophyTitles.Any())
            {
                HasMoreItems = true;
                MaxCount += 64;
            }
            else
            {
                if (Count <= 0)
                {
                    IsEmpty = true;
                }
                HasMoreItems = false;
            }
            IsLoading = false;
            return true;
        }
    }
}