using System;
using System.ComponentModel;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.ViewModels
{
    public class AddEditItemViewModel : BindableBase
    {
        private readonly IItemProductService _service;
        private bool _editMode;
        private ItemProductModel _itemProduct;
        private string _title;

        public AddEditItemViewModel(IItemProductService service)
        {
            _service = service;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        public ItemProductModel ItemProduct
        {
            get { return _itemProduct; }
            set { SetProperty(ref _itemProduct, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public event Action Done = delegate { };


        private bool CanSave()
        {
            return !ItemProduct.HasErrors;
        }

        private async void OnSave()
        {
            UpdateItem(ItemProduct, _itemProduct);
            if (!_itemProduct.HasErrors)
            {
                if (EditMode)
                {
                    await _service.EditAsync(_itemProduct);
                }
                else
                {
                    _itemProduct.CreatedDate = DateTime.Now;
                    _itemProduct.IsActive = true;
                    await _service.CreateAsync(_itemProduct);
                }
                Done();
                
            }
            else
            {
                _itemProduct.ErrorsChanged += RaiseCanExecuteChanged;
                
            }
     
        }

        private void UpdateItem(ItemProductModel source, ItemProductModel target)
        {
            target.Quantity = source.Quantity;
            target.CreatedDate = source.CreatedDate;
            target.Price = source.Price;
            target.Name = source.Name;
            target.Description = source.Description;
            target.IsActive = source.IsActive;
            target.Id = source.Id;
        }

        private void CopyItem(ItemProductModel source, ItemProductModel target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.Quantity = source.Quantity;
                target.CreatedDate = source.CreatedDate;
                target.Price = source.Price;
                target.Name = source.Name;
                target.Description = source.Description;
                target.IsActive = source.IsActive;
            }
        }

        public void SetItem(ItemProductModel item)
        {
            _itemProduct = item;
            if (ItemProduct != null) ItemProduct.ErrorsChanged -= RaiseCanExecuteChanged;
            ItemProduct = new ItemProductModel();
            ItemProduct.ErrorsChanged += RaiseCanExecuteChanged;
            CopyItem(item, ItemProduct);
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        private void OnCancel()
        {
            Done();
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title,value); }
        }

        public override string ViewTitle
        {
            get { return Title; }
        }
    }
}

