using System;
using System.ComponentModel;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.ViewModels
{
    class AddEditUnitOfMeasureViewModel:ValidatableBindableBase
    {
         private readonly IUnitOfMeasureService _service;
        private bool _editMode;
        private UnitOfMeasureModel _UnitOfMeasure;
        private string _title;

        public AddEditUnitOfMeasureViewModel(IUnitOfMeasureService service)
        {
            _service = service;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);
        }

        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        public UnitOfMeasureModel UnitOfMeasure
        {
            get { return _UnitOfMeasure; }
            set { SetProperty(ref _UnitOfMeasure, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public event Action Done = delegate { };


        private bool CanSave()
        {
            return !UnitOfMeasure.HasErrors;
        }

        private async void OnSave()
        {
            UpdateUnitOfMeasure(UnitOfMeasure, _UnitOfMeasure);
            if (!_UnitOfMeasure.HasErrors)
            {
                if (EditMode)
                {
                    await _service.EditAsync(_UnitOfMeasure);
                }
                else
                {
                    _UnitOfMeasure.CreatedDate = DateTime.Now;
                    _UnitOfMeasure.IsActive = true;
                    await _service.CreateAsync(_UnitOfMeasure);
                }
                Done();
                
            }
            else
            {
                _UnitOfMeasure.ErrorsChanged += RaiseCanExecuteChanged;
                
            }
     
        }

        private void UpdateUnitOfMeasure(UnitOfMeasureModel source, UnitOfMeasureModel target)
        {
            target.CreatedDate = source.CreatedDate;
            target.Name = source.Name;
            target.IsActive = source.IsActive;
            target.Id = source.Id;
            target.DisplayName = source.DisplayName;
        }

        private void CopyUnitOfMeasure(UnitOfMeasureModel source, UnitOfMeasureModel target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.CreatedDate = source.CreatedDate;
                target.Name = source.Name;
                target.DisplayName = source.DisplayName;
                target.IsActive = source.IsActive;
            }
        }

        public void SetUnitOfMeasure(UnitOfMeasureModel UnitOfMeasure)
        {
            _UnitOfMeasure = UnitOfMeasure;
            if (UnitOfMeasure != null) UnitOfMeasure.ErrorsChanged -= RaiseCanExecuteChanged;
            UnitOfMeasure = new UnitOfMeasureModel();
            UnitOfMeasure.ErrorsChanged += RaiseCanExecuteChanged;
            CopyUnitOfMeasure(UnitOfMeasure, UnitOfMeasure);
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
