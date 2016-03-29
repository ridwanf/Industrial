using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;
using Industrial.Wpf.Infrastructures;

namespace Industrial.Wpf.ViewModels
{
    public class TestPagingGridViewModel : PageableBase
    {
        private readonly IItemProductService _service;
        private SortablePageableCollection<ItemProductModel> _items;
        private int _totalItem = 0;
        public SortablePageableCollection<ItemProductModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items, value); }
        }

        public RelayCommand GoToNextPageCommand { get; private set; }
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand ChangePageSizeCommand { get; private set; }
       

        public TestPagingGridViewModel(IItemProductService service)
        {
            _service = service;
            _totalItem = _service.GetAll().Count;
           // Items = new SortablePageableCollection<ItemProductModel>(_service.GetAll(1, 5), _totalItem,1, 5);

            // Create Commands 
            GoToNextPageCommand = new RelayCommand(GoToNextPage);
            GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage);
            ChangePageSizeCommand = new RelayCommand(ChangePageSize);

        }

        public async void LoadItems()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            Items = new SortablePageableCollection<ItemProductModel>(await _service.GetAllAsync(1, 5), _totalItem, 1, 5);
        }

        private void ChangePageSize()
        {
           
            Items = new SortablePageableCollection<ItemProductModel>(_service.GetAll(Items.CurrentPageNumber, Items.PageSize), _totalItem, 1,Items.PageSize);
        }

        private void GoToPreviousPage()
        {
            if (Items.CurrentPageNumber>1)
            {
                var currentPage = Items.CurrentPageNumber;
                var data = _service.GetAll(Items.CurrentPageNumber - 1, Items.PageSize);
                Items = new SortablePageableCollection<ItemProductModel>(data, _totalItem,currentPage, Items.PageSize);
                Items.GoToPreviousPage();
            }
           
        }

        private void GoToNextPage()
        {
            if (Items.CurrentPageNumber!=Items.TotalPagesNumber)
            {
                var currentPage = Items.CurrentPageNumber;
                var data = _service.GetAll(currentPage + 1, Items.PageSize);
                Items = new SortablePageableCollection<ItemProductModel>(data, _totalItem,currentPage, Items.PageSize);
                Items.GoToNextPage();
            }
       
        }
    }
}