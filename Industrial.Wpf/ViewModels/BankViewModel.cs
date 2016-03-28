using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;
using Industrial.Wpf.Infrastructures;
using MahApps.Metro.Controls.Dialogs;

namespace Industrial.Wpf.ViewModels
{
    public class BankViewModel:PageableBase
    {
        private readonly IBankService _service;
        private int _totalBank = 0;

        private string _searchWord;
        private SortablePageableCollection<BankModel> _banks;

        public RelayCommand NewBank { get; private set; }
        public RelayCommand<BankModel> EditBankCommand { get; private set; }
        public RelayCommand<BankModel> DeleteBankCommand { get; private set; }
        public RelayCommand AddBankCommand { get; private set; }
        public RelayCommand GoToNextPageCommand { get; private set; }
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand ChangePageSizeCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }

        public event Action<BankModel> AddBankRequested = delegate { };
        public event Action<BankModel> EditBankRequested = delegate { };
        public event Action<BankModel> DeleteBankRequested = delegate { };

        public BankViewModel(IBankService service)
        {
            _service = service;

            EditBankCommand = new RelayCommand<BankModel>(OnEditCommand);
            DeleteBankCommand = new RelayCommand<BankModel>(OnDeleteCommand);
            AddBankCommand = new RelayCommand(OnAddCommand);
            SearchCommand = new RelayCommand(OnSearchCommand);
            // Create Commands 
            GoToNextPageCommand = new RelayCommand(GoToNextPage);
            GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage);
            ChangePageSizeCommand = new RelayCommand(ChangePageSize);
        }

        public async void LoadBanks()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            _totalBank = _service.GetAll().Count;
            Banks = new SortablePageableCollection<BankModel>(await _service.GetAllAsync(1, 5), _totalBank, 1, 5);
        }

        private void ChangePageSize()
        {
            Banks = new SortablePageableCollection<BankModel>(_service.GetAll(Banks.CurrentPageNumber, Banks.PageSize), _totalBank, 1, Banks.PageSize);
        }

        private void GoToPreviousPage()
        {
            if (_banks.CurrentPageNumber > 1)
            {
                var currentPage = _banks.CurrentPageNumber;
                var data = _service.GetAll(Banks.CurrentPageNumber - 1, Banks.PageSize);
                Banks = new SortablePageableCollection<BankModel>(data, _totalBank, currentPage, Banks.PageSize);
                _banks.GoToPreviousPage();
            }
        }

        private void GoToNextPage()
        {
            if (Banks.CurrentPageNumber != Banks.TotalPagesNumber)
            {
                Banks = new SortablePageableCollection<BankModel>(_service.GetAll(Banks.CurrentPageNumber + 1, Banks.PageSize), _totalBank, Banks.CurrentPageNumber, Banks.PageSize);
                Banks.GoToNextPage();
            }
        }

        private void OnSearchCommand()
        {
            if (!string.IsNullOrWhiteSpace(_searchWord))
            {
                int total;
                var data = _service.Search(_searchWord, 1, Banks.PageSize, out total);
                _totalBank = total;
                Banks = new SortablePageableCollection<BankModel>(data, _totalBank, 1, Banks.PageSize);
            }
            else
            {
                LoadBanks();
            }
        }

        private void OnAddCommand()
        {
            AddBankRequested(new BankModel
            {
                Id = 0,
                IsActive = true,
                CreatedDate = DateTime.Now,

            });
             _totalBank++;
        }

        private async void OnDeleteCommand(BankModel obj)
        {
            var result = await DialogService.AskQuestionAsync("Delete Data",
                "Are you sure you want to delete this record?");
            if (result == MessageDialogResult.Affirmative)
            {
                await _service.DeleteAsync(obj.Id);
                _banks.Remove(obj);
                _totalBank--;
            }
            LoadBanks();
        }

        private void OnEditCommand(BankModel obj)
        {
            EditBankRequested(obj);
        }

        public SortablePageableCollection<BankModel> Banks
        {
            get { return _banks; }
            set { SetProperty(ref _banks, value); }
        }

        public override string ViewTitle
        {
            get { return "Bank List"; }
        }

        public string SearchWord
        {
            get { return _searchWord; }
            set { SetProperty(ref _searchWord, value); }
        }
    }
}
