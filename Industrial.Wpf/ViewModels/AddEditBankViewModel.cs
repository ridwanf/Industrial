using System;
using System.ComponentModel;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.ViewModels
{
    class AddEditBankViewModel:ValidatableBindableBase
    {
         private readonly IBankService _service;
        private bool _editMode;
        private BankModel _bank;
        private string _title;

        public AddEditBankViewModel(IBankService service)
        {
            _service = service;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        public BankModel Bank
        {
            get { return _bank; }
            set { SetProperty(ref _bank, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public event Action Done = delegate { };


        private bool CanSave()
        {
            return !Bank.HasErrors;
        }

        private async void OnSave()
        {
            UpdateBank(Bank, _bank);
            if (!_bank.HasErrors)
            {
                if (EditMode)
                {
                    await _service.EditAsync(_bank);
                }
                else
                {
                    _bank.CreatedDate = DateTime.Now;
                    _bank.IsActive = true;
                    await _service.CreateAsync(_bank);
                }
                Done();
                
            }
            else
            {
                _bank.ErrorsChanged += RaiseCanExecuteChanged;
                
            }
     
        }

        private void UpdateBank(BankModel source, BankModel target)
        {
            target.CreatedDate = source.CreatedDate;
            target.Name = source.Name;
            target.Code = source.Code;
            target.IsActive = source.IsActive;
            target.Id = source.Id;
        }

        private void CopyBank(BankModel source, BankModel target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.CreatedDate = source.CreatedDate;
                target.Name = source.Name;
                target.Code = source.Code;
                target.IsActive = source.IsActive;
            }
        }

        public void SetBank(BankModel bank)
        {
            _bank = bank;
            if (bank != null) bank.ErrorsChanged -= RaiseCanExecuteChanged;
            bank = new BankModel();
            bank.ErrorsChanged += RaiseCanExecuteChanged;
            CopyBank(bank, bank);
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
