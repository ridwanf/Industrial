using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Core.Common.UI;

namespace Industrial.Wpf.Infrastructures
{
    public class PageableCollection<T> : BindableBase
    {
        #region Public Properties

        // Default Entries per page Number
        private int _pageSize = 5;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                if (_pageSize != value)
                {
                    _pageSize = value;
                    SendPropertyChanged(() => PageSize);
                    Reset();
                }
            }
        }

        public int TotalData
        {
            get { return _totalData; }
            set { SetProperty(ref _totalData, value); }
        }

        public int TotalPagesNumber
        {
            get { return _totalPagesNumber; }
            set { SetProperty(ref _totalPagesNumber, value); }
        }


        //public int TotalPagesNumber
        //{
        //    get { return _totalPagesNumber; }
        //    set { SetProperty(ref _totalPagesNumber,value); }
        //}

        private int _currentPageNumber;
        public int CurrentPageNumber
        {
            get
            {
                return _currentPageNumber;
            }

            protected set
            {
                SetProperty(ref _currentPageNumber, value);
            }
        }

        private ObservableCollection<T> _currentPageItems;
        private int _totalData;
        private int _totalPagesNumber;

        public ObservableCollection<T> CurrentPageItems
        {
            get
            {
                return _currentPageItems;
            }
            private set
            {
                SetProperty(ref _currentPageItems, value);
            }
        }

        #endregion

        #region Protected Properties

        protected ObservableCollection<T> AllObjects { get; set; }

        #endregion

        #region ctor

        protected PageableCollection()
        {
        }

        public PageableCollection(IEnumerable<T> allObjects, int count,int currentPageNumber, int? entriesPerPage = null)
        {
            CurrentPageNumber = currentPageNumber;
            TotalData = count;
            CurrentPageItems = new ObservableCollection<T>(allObjects);
            if (entriesPerPage != null)
                PageSize = (int)entriesPerPage;
            TotalPagesNumber = (count - 1) / PageSize + 1;
            // Calculate(CurrentPageNumber);
        }

        #endregion

        #region Public Methods

        public void GoToNextPage()
        {
            if (CurrentPageNumber != TotalPagesNumber)
            {
                CurrentPageNumber++;
                // Calculate(CurrentPageNumber);
            }
        }

        public void GoToPreviousPage()
        {
            if (CurrentPageNumber > 1)
            {
                CurrentPageNumber--;
                // Calculate(CurrentPageNumber);
            }
        }

        public virtual void Remove(T item)
        {
            CurrentPageItems.Remove(item);
            TotalData--;
            // Update the total number of pages
            TotalPagesNumber = (TotalData - 1) / PageSize + 1;

            // if the last item on the last page is removed
            if (CurrentPageNumber > TotalPagesNumber)
                CurrentPageNumber--;

        }

        //public virtual void Add(T item)
        //{
        //    // Go back to the first page
        //    CurrentPageNumber = 1;
        //    Calculate(CurrentPageNumber);

        //    // Keep the same size and put it on top
        //    CurrentPageItems.RemoveAt(CurrentPageItems.Count - 1);
        //    CurrentPageItems.Insert(0, item);

        //    // Add it to the original collection
        //    AllObjects.Add(item);
        //    SendPropertyChanged(() => TotalPagesNumber);
        //}

        #endregion

        #region INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;
        public void SendPropertyChanged<T>(Expression<Func<T>> expression)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(((MemberExpression)expression.Body).Member.Name));
            }
        }

        #endregion

        #region Private Methods

        //protected void Calculate(int pageNumber)
        //{
        //    int upperLimit = pageNumber * PageSize;

        //    CurrentPageItems =
        //        new ObservableCollection<T>(
        //            AllObjects.Where(x => AllObjects.IndexOf(x) > upperLimit - (PageSize + 1) && AllObjects.IndexOf(x) < upperLimit));
        //}

        protected void SetCurrentPageItems()
        {
            int upperLimit = CurrentPageNumber * PageSize;

            //CurrentPageItems =
            //    new ObservableCollection<T>(
            //        AllObjects.Where(x => AllObjects.IndexOf(x) > upperLimit - (PageSize + 1) && AllObjects.IndexOf(x) < upperLimit));
        }

        private void Reset()
        {
            CurrentPageNumber = 1;
            // Calculate(CurrentPageNumber);
            SendPropertyChanged(() => TotalPagesNumber);
        }

        #endregion
    }
}