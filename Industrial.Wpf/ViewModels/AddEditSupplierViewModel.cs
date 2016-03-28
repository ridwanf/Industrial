using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI;
using Industrial.Data.Domain;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;
using StructureMap.Diagnostics;

namespace Industrial.Wpf.ViewModels
{
    public class AddEditSupplierViewModel : ValidatableBindableBase
    {
        private readonly ISupplierService _service;
        private bool _editMode;
        private SupplierModel _supplier;
        private string _title;


        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        public event Action Done = delegate { };
        public AddEditSupplierViewModel(ISupplierService service)
        {
            _service = service;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        private bool CanSave()
        {
            return !Supplier.HasErrors;
        }

        private async void OnSave()
        {
            UpdateSupplier(Supplier, _supplier);
            if (!_supplier.HasErrors)
            {
                if (EditMode)
                {
                    await _service.EditAsync(_supplier);
                }
                else
                {
                    _supplier.CreatedDate = DateTime.Now;
                    _supplier.IsActive = true;
                    await _service.CreateAsync(_supplier);
                }
                Done();

            }
            else
            {
                _supplier.ErrorsChanged += RaiseCanExecuteChanged;

            }
        }

        private void OnCancel()
        {
            Done();
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public SupplierModel Supplier
        {
            get { return _supplier; }
            set { SetProperty(ref _supplier,value); }
        }

        private void UpdateSupplier(SupplierModel source, SupplierModel target)
        {
            target.Address = source.Address;
            target.ContractDate = source.ContractDate;
            target.CreatedDate = source.CreatedDate;
            target.RegisterDate = source.RegisterDate;
            target.Email = source.Email;
            target.Email2 = source.Email2;
            target.FaxNumber = source.FaxNumber;
            target.FaxNumber2 = source.FaxNumber2;
            target.Id = source.Id;
            target.IsActive = source.IsActive;
            target.Name = source.Name;
            target.PhoneNumber = source.PhoneNumber;
            target.PhoneNumber2 = source.PhoneNumber2;
            target.PicName = source.PicName;
        }

        private void CopyItem(SupplierModel source, SupplierModel target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.Address = source.Address;
                target.ContractDate = source.ContractDate;
                target.CreatedDate = source.CreatedDate;
                target.Email = source.Email;
                target.RegisterDate = source.RegisterDate;
                target.Email2 = source.Email2;
                target.FaxNumber = source.FaxNumber;
                target.FaxNumber2 = source.FaxNumber2;
                target.IsActive = source.IsActive;
                target.Name = source.Name;
                target.PhoneNumber = source.PhoneNumber;
                target.PhoneNumber2 = source.PhoneNumber2;
                target.PicName = source.PicName;

            }
        }

        public void SetSupplier(SupplierModel supplier)
        {
            _supplier = supplier;
            if (Supplier != null) Supplier.ErrorsChanged -= RaiseCanExecuteChanged;
            Supplier = new SupplierModel(){RegisterDate = DateTime.Now,ContractDate = DateTime.Now};
            Supplier.ErrorsChanged += RaiseCanExecuteChanged;
            CopyItem(supplier, Supplier);
        }

        private void RaiseCanExecuteChanged(object sender, DataErrorsChangedEventArgs e)
        {
            SaveCommand.RaiseCanExecuteChanged();
        }

        public override string ViewTitle
        {
            get { return Title; }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
    }
}
