using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;

namespace Industrial.Wpf.ViewModels
{
    class AddEditBranchViewModel : ValidatableBindableBase
    {
        private readonly IBranchService _service;
        private bool _editMode;
        private BranchModel _branch;
        private string _title;
        private ObservableCollection<BranchModel> _branchModels;
        private BranchModel _selectedBranch;

        public BranchModel SelectedBranch
        {
            get { return _selectedBranch; }
            set { SetProperty(ref _selectedBranch, value); }
        }



        public AddEditBranchViewModel(IBranchService service)
        {
            _service = service;
            CancelCommand = new RelayCommand(OnCancel);
            SaveCommand = new RelayCommand(OnSave, CanSave);

        }


        public async void LoadBranchs()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }

            BranchModels = new ObservableCollection<BranchModel>(await _service.GetDropDownAsync(Branch.Id));
            if (Branch.ParentBranchId != null)
                SelectedBranch = BranchModels.FirstOrDefault(a => a.Id == Branch.ParentBranchId);

        }


        public RelayCommand CancelCommand { get; private set; }
        public RelayCommand SaveCommand { get; private set; }


        public BranchModel Branch
        {
            get { return _branch; }
            set { SetProperty(ref _branch, value); }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set { SetProperty(ref _editMode, value); }
        }

        public ObservableCollection<BranchModel> BranchModels
        {
            get { return _branchModels; }
            set { SetProperty(ref _branchModels, value); }
        }

        public event Action Done = delegate { };


        private bool CanSave()
        {
            return !Branch.HasErrors;
        }

        private async void OnSave()
        {

            Branch.ParentBranchId = (SelectedBranch != null) ? SelectedBranch.Id : (int?)null;
            // Branch.ParentBranchId = null;
            UpdateBranch(Branch, _branch);
            if (!_branch.HasErrors)
            {
                if (EditMode)
                {
                    await _service.EditAsync(_branch);
                }
                else
                {
                    _branch.CreatedDate = DateTime.Now;
                    _branch.IsActive = true;
                    await _service.CreateAsync(_branch);
                }
                Done();

            }
            else
            {
                _branch.ErrorsChanged += RaiseCanExecuteChanged;

            }

        }

        private void UpdateBranch(BranchModel source, BranchModel target)
        {
            target.CreatedDate = source.CreatedDate;
            target.Name = source.Name;
            //  target.Children = source.Children;
            target.ParentBranchName = source.ParentBranchName;
            target.ParentBranchId = source.ParentBranchId;
            target.IsActive = source.IsActive;
            target.Id = source.Id;
        }

        private void CopyBranch(BranchModel source, BranchModel target)
        {
            target.Id = source.Id;
            if (EditMode)
            {
                target.CreatedDate = source.CreatedDate;
                target.Name = source.Name;
                //  target.Children = source.Children;
                target.ParentBranchName = source.ParentBranchName;
                target.ParentBranchId = source.ParentBranchId;
                target.IsActive = source.IsActive;
            }
        }

        public void SetBranch(BranchModel Branch)
        {
            _branch = Branch;
            if (Branch != null) Branch.ErrorsChanged -= RaiseCanExecuteChanged;
            Branch = new BranchModel();
            Branch.ErrorsChanged += RaiseCanExecuteChanged;
            CopyBranch(Branch, Branch);
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
            set { SetProperty(ref _title, value); }
        }

        public override string ViewTitle
        {
            get { return Title; }
        }
    }
}