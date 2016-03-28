using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Media;
using System.Security.AccessControl;
using System.Windows;
using Core;
using Core.Common.UI;
using Industrial.Data.Repositories;
using Industrial.Repository.Repositories;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;
using Industrial.Wpf.Infrastructures;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace Industrial.Wpf.ViewModels
{
    public class ItemProductViewModel : PageableBase
    {
        public IItemProductService _Service;
        private SortablePageableCollection<ItemProductModel> _items;
        private int _totalItem = 0;

        private string _searchWord;

        public RelayCommand NewItem { get; private set; }
        public RelayCommand<ItemProductModel> EditItemCommand { get; private set; }
        public RelayCommand<ItemProductModel> DeleteItemCommand { get; private set; }
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand GoToNextPageCommand { get; private set; }
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand ChangePageSizeCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }

        public event Action<ItemProductModel> AddItemRequested = delegate { };
        public event Action<ItemProductModel> EditItemRequested = delegate { };
        public event Action<ItemProductModel> DeleteItemRequested = delegate { };
      

        public ItemProductViewModel(IItemProductService service)
        {
            _Service = service;
            EditItemCommand = new RelayCommand<ItemProductModel>(OnEditCommand);
            DeleteItemCommand = new RelayCommand<ItemProductModel>(OnDeleteCommand);
            AddItemCommand = new RelayCommand(OnAddCommand);
            SearchCommand = new RelayCommand(OnSearchCommand);
            // Create Commands 
            GoToNextPageCommand = new RelayCommand(GoToNextPage);
            GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage);
            ChangePageSizeCommand = new RelayCommand(ChangePageSize);

        }

        private void OnSearchCommand()
        {
            if (!string.IsNullOrWhiteSpace(_searchWord))
            {
                int total;
                var data = _Service.Search(_searchWord,1, Items.PageSize, out total);
                _totalItem = total;
                Items = new SortablePageableCollection<ItemProductModel>(data, _totalItem, 1, Items.PageSize);
            }
            else
            {
                LoadItems();
            }
        }

        private void OnAddCommand()
        {
            AddItemRequested(new ItemProductModel() { Id = 0, IsActive = true, CreatedDate = DateTime.Now });
           // _totalItem++;
        }

        private async void OnDeleteCommand(ItemProductModel obj)
        {
            
            var result = await DialogService.AskQuestionAsync("Delete Data",
                "Are you sure you want to delete this  record?");
            if (result == MessageDialogResult.Affirmative)
            {
               await _Service.DeleteAsync(obj.Id);
                _items.Remove(obj);
                _totalItem--;
            }
            LoadItems();
        }

        private void OnEditCommand(ItemProductModel obj)
        {
            EditItemRequested(obj);
        }


        public SortablePageableCollection<ItemProductModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }


        public async void LoadItems()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            _totalItem = _Service.GetAll().Count;
            Items = new SortablePageableCollection<ItemProductModel>(await _Service.GetAllAsync(1, 5), _totalItem, 1, 5);

        }

      

        public override string ViewTitle
        {
            get {return "Item Product List"; }
        }

        private void ChangePageSize()
        {

            Items = new SortablePageableCollection<ItemProductModel>(_Service.GetAll(Items.CurrentPageNumber, Items.PageSize), _totalItem, 1, Items.PageSize);
        }

        private void GoToPreviousPage()
        {
            if (Items.CurrentPageNumber > 1)
            {
                var currentPage = Items.CurrentPageNumber;
                var data = _Service.GetAll(Items.CurrentPageNumber - 1, Items.PageSize);
                Items = new SortablePageableCollection<ItemProductModel>(data, _totalItem, currentPage, Items.PageSize);
                Items.GoToPreviousPage();
            }

        }

        private void GoToNextPage()
        {
            if (Items.CurrentPageNumber != Items.TotalPagesNumber)
            {
                var currentPage = Items.CurrentPageNumber;
                var data = _Service.GetAll(currentPage + 1, Items.PageSize);
                Items = new SortablePageableCollection<ItemProductModel>(data, _totalItem, currentPage, Items.PageSize);
                Items.GoToNextPage();
            }

        }

        public string SearchWord
        {
            get { return _searchWord; }
            set { SetProperty(ref _searchWord,value); }
        }
    }
}
