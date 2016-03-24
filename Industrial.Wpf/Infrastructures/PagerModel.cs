using System.Collections.Generic;
using System.Collections.ObjectModel;
using Core.Common.UI;

namespace Industrial.Wpf.Infrastructures
{
    public class PagerModel<T> : BindableBase
        where T : BindableBase
    {
        private ObservableCollection<T> _data;
        private int _pageSize;
        private int _pageNumber;
        private int _total;

        public ObservableCollection<T> Data
        {
            get { return _data; }
            set { SetProperty(ref _data, value); }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { SetProperty(ref _pageSize, value); }
        }

        public int PageNumber
        {
            get { return _pageNumber; }
            set { SetProperty(ref _pageNumber, value); }
        }

        public int Total
        {
            get { return _total; }
            set { SetProperty(ref _total, value); }
        }
    }
}