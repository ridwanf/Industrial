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
    public class BranchViewModel : PageableBase
    {
        private readonly IBranchService _service;
        private int _totalBranch = 0;
        private string _searchWord;
        private SortablePageableCollection<BranchModel> _branchs;

        public RelayCommand NewBranch { get; private set; }
        public RelayCommand<BranchModel> EditBranchCommand { get; private set; }
        public RelayCommand<BranchModel> DeleteBranchCommand { get; private set; }
        public RelayCommand AddBranchCommand { get; private set; }
        public RelayCommand GoToNextPageCommand { get; private set; }
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand ChangePageSizeCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }

        public event Action<BranchModel> AddBranchRequested = delegate { };
        public event Action<BranchModel> EditBranchRequested = delegate { };
        public event Action<BranchModel> DeleteBranchRequested = delegate { };
        public BranchViewModel(IBranchService service)
        {
            _service = service;

            EditBranchCommand = new RelayCommand<BranchModel>(OnEditCommand);
            DeleteBranchCommand = new RelayCommand<BranchModel>(OnDeleteCommand);
            AddBranchCommand = new RelayCommand(OnAddCommand);
            SearchCommand = new RelayCommand(OnSearchCommand);
            // Create Commands 
            GoToNextPageCommand = new RelayCommand(GoToNextPage);
            GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage);
            ChangePageSizeCommand = new RelayCommand(ChangePageSize);
        }

        public string SearchWord
        {
            get { return _searchWord; }
            set { SetProperty(ref _searchWord, value); }
        }

        public SortablePageableCollection<BranchModel> Branchs
        {
            get { return _branchs; }
            set { SetProperty(ref _branchs, value); }
        }

        public async void LoadBranchs()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            _totalBranch = _service.GetAll().Count;
            Branchs = new SortablePageableCollection<BranchModel>(await _service.GetAllAsync(1, 5), _totalBranch, 1, 5);
        }

        private void ChangePageSize()
        {
            Branchs = new SortablePageableCollection<BranchModel>(_service.GetAll(Branchs.CurrentPageNumber, Branchs.PageSize), _totalBranch, 1, Branchs.PageSize);
        }

        private void GoToPreviousPage()
        {
            if (_branchs.CurrentPageNumber > 1)
            {
                var currentPage = _branchs.CurrentPageNumber;
                var data = _service.GetAll(Branchs.CurrentPageNumber - 1, Branchs.PageSize);
                Branchs = new SortablePageableCollection<BranchModel>(data, _totalBranch, currentPage, Branchs.PageSize);
                _branchs.GoToPreviousPage();
            }
        }

        private void GoToNextPage()
        {
            if (Branchs.CurrentPageNumber != Branchs.TotalPagesNumber)
            {
                Branchs = new SortablePageableCollection<BranchModel>(_service.GetAll(Branchs.CurrentPageNumber + 1, Branchs.PageSize), _totalBranch, Branchs.CurrentPageNumber, Branchs.PageSize);
                Branchs.GoToNextPage();
            }
        }

        private void OnSearchCommand()
        {
            if (!string.IsNullOrWhiteSpace(_searchWord))
            {
                int total;
                var data = _service.Search(_searchWord, 1, Branchs.PageSize, out total);
                _totalBranch = total;
                Branchs = new SortablePageableCollection<BranchModel>(data, _totalBranch, 1, Branchs.PageSize);
            }
            else
            {
                LoadBranchs();
            }
        }

        private void OnAddCommand()
        {
            AddBranchRequested(new BranchModel
            {
                Id = 0,
                IsActive = true,
                CreatedDate = DateTime.Now,

            });
            _totalBranch++;
        }

        private async void OnDeleteCommand(BranchModel obj)
        {
            var result = await DialogService.AskQuestionAsync("Delete Data",
                "Are you sure you want to delete this record?");
            if (result == MessageDialogResult.Affirmative)
            {
                await _service.DeleteAsync(obj.Id);
                _branchs.Remove(obj);
                _totalBranch--;
            }
            LoadBranchs();
        }

        private void OnEditCommand(BranchModel obj)
        {
            EditBranchRequested(obj);
        }

        public override string ViewTitle
        {
            get { return "Branch List"; }
        }
    }
}
