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
    public class ItemProductViewModel : BindableBase
    {
        public IItemProductService _Service = new ItemProductService();
        private IDialogService _dialogService;
        private ObservableCollection<ItemProductModel> _items;
        private AddEditItemViewModel _currentItem;
        private List<ItemProductModel> _allItems;

        public RelayCommand NewItem { get; private set; }
        public RelayCommand<ItemProductModel> EditItemCommand { get; private set; }
        public RelayCommand<ItemProductModel> DeleteItemCommand { get; private set; }
        public RelayCommand AddItemCommand { get; private set; }
        public RelayCommand TestAlert { get; private set; }

        public event Action<ItemProductModel> AddItemRequested = delegate { };
        public event Action<ItemProductModel> EditItemRequested = delegate { };
        public event Action<ItemProductModel> DeleteItemRequested = delegate { };
        public event CancelEventHandler ConfirmDelete;


        public ItemProductViewModel(IItemProductService service)
        {
            _Service = service;
            EditItemCommand = new RelayCommand<ItemProductModel>(OnEditCommand);
            DeleteItemCommand = new RelayCommand<ItemProductModel>(OnDeleteCommand);
            AddItemCommand = new RelayCommand(OnAddCommand);
        }

        private void OnAddCommand()
        {
            AddItemRequested(new ItemProductModel() { Id = 0, IsActive = true, CreatedDate = DateTime.Now });
        }

        private async void OnDeleteCommand(ItemProductModel obj)
        {
            
            var result = await DialogService.AskQuestionAsync("Delete Match",
                "Are you sure you want to delete this Match record?");
            if (result == MessageDialogResult.Affirmative)
            {
                _Service.DeleteAsync(obj.Id);
                _items.Remove(obj);
            }
        }

        private void OnEditCommand(ItemProductModel obj)
        {
            EditItemRequested(obj);
        }


        public ObservableCollection<ItemProductModel> Items
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
            _allItems = await _Service.GetAllAsync();
            Items = new ObservableCollection<ItemProductModel>(_allItems);
        }

        public AddEditItemViewModel CurrentItem
        {
            get { return _currentItem; }
            set { SetProperty(ref _currentItem, value); }
        }


        public override string ViewTitle
        {
            get {return "Item Product List"; }
        }


        
    }
}
