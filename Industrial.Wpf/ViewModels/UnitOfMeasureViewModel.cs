using System;
using System.ComponentModel;
using Core.Common.UI;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;
using Industrial.Wpf.Infrastructures;
using MahApps.Metro.Controls.Dialogs;

namespace Industrial.Wpf.ViewModels
{
    public class UnitOfMeasureViewModel :PageableBase{
       private readonly IUnitOfMeasureService _service;
        private int _totalUnitOfMeasure = 0;

        private string _searchWord;
        private SortablePageableCollection<UnitOfMeasureModel> _UnitOfMeasures;

        public RelayCommand NewUnitOfMeasure { get; private set; }
        public RelayCommand<UnitOfMeasureModel> EditUnitOfMeasureCommand { get; private set; }
        public RelayCommand<UnitOfMeasureModel> DeleteUnitOfMeasureCommand { get; private set; }
        public RelayCommand AddUnitOfMeasureCommand { get; private set; }
        public RelayCommand GoToNextPageCommand { get; private set; }
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand ChangePageSizeCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }

        public event Action<UnitOfMeasureModel> AddUnitOfMeasureRequested = delegate { };
        public event Action<UnitOfMeasureModel> EditUnitOfMeasureRequested = delegate { };
        public event Action<UnitOfMeasureModel> DeleteUnitOfMeasureRequested = delegate { };

        public UnitOfMeasureViewModel(IUnitOfMeasureService service)
        {
            _service = service;

            EditUnitOfMeasureCommand = new RelayCommand<UnitOfMeasureModel>(OnEditCommand);
            DeleteUnitOfMeasureCommand = new RelayCommand<UnitOfMeasureModel>(OnDeleteCommand);
            AddUnitOfMeasureCommand = new RelayCommand(OnAddCommand);
            SearchCommand = new RelayCommand(OnSearchCommand);
            // Create Commands 
            GoToNextPageCommand = new RelayCommand(GoToNextPage);
            GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage);
            ChangePageSizeCommand = new RelayCommand(ChangePageSize);
        }

        public async void LoadUnitOfMeasures()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            _totalUnitOfMeasure = _service.GetAll().Count;
            UnitOfMeasures = new SortablePageableCollection<UnitOfMeasureModel>(await _service.GetAllAsync(1, 5), _totalUnitOfMeasure, 1, 5);
        }

        private void ChangePageSize()
        {
            UnitOfMeasures = new SortablePageableCollection<UnitOfMeasureModel>(_service.GetAll(UnitOfMeasures.CurrentPageNumber, UnitOfMeasures.PageSize), _totalUnitOfMeasure, 1, UnitOfMeasures.PageSize);
        }

        private void GoToPreviousPage()
        {
            if (_UnitOfMeasures.CurrentPageNumber > 1)
            {
                var currentPage = _UnitOfMeasures.CurrentPageNumber;
                var data = _service.GetAll(UnitOfMeasures.CurrentPageNumber - 1, UnitOfMeasures.PageSize);
                UnitOfMeasures = new SortablePageableCollection<UnitOfMeasureModel>(data, _totalUnitOfMeasure, currentPage, UnitOfMeasures.PageSize);
                _UnitOfMeasures.GoToPreviousPage();
            }
        }

        private void GoToNextPage()
        {
            if (UnitOfMeasures.CurrentPageNumber != UnitOfMeasures.TotalPagesNumber)
            {
                UnitOfMeasures = new SortablePageableCollection<UnitOfMeasureModel>(_service.GetAll(UnitOfMeasures.CurrentPageNumber + 1, UnitOfMeasures.PageSize), _totalUnitOfMeasure, UnitOfMeasures.CurrentPageNumber, UnitOfMeasures.PageSize);
                UnitOfMeasures.GoToNextPage();
            }
        }

        private void OnSearchCommand()
        {
            if (!string.IsNullOrWhiteSpace(_searchWord))
            {
                int total;
                var data = _service.Search(_searchWord, 1, UnitOfMeasures.PageSize, out total);
                _totalUnitOfMeasure = total;
                UnitOfMeasures = new SortablePageableCollection<UnitOfMeasureModel>(data, _totalUnitOfMeasure, 1, UnitOfMeasures.PageSize);
            }
            else
            {
                LoadUnitOfMeasures();
            }
        }

        private void OnAddCommand()
        {
            AddUnitOfMeasureRequested(new UnitOfMeasureModel
            {
                Id = 0,
                IsActive = true,
                CreatedDate = DateTime.Now,

            });
        }

        private async void OnDeleteCommand(UnitOfMeasureModel obj)
        {
            var result = await DialogService.AskQuestionAsync("Delete Data",
                "Are you sure you want to delete this record?");
            if (result == MessageDialogResult.Affirmative)
            {
                await _service.DeleteAsync(obj.Id);
                _UnitOfMeasures.Remove(obj);
                _totalUnitOfMeasure--;
            }
            LoadUnitOfMeasures();
        }

        private void OnEditCommand(UnitOfMeasureModel obj)
        {
            EditUnitOfMeasureRequested(obj);
        }

        public SortablePageableCollection<UnitOfMeasureModel> UnitOfMeasures
        {
            get { return _UnitOfMeasures; }
            set { SetProperty(ref _UnitOfMeasures, value); }
        }

        public override string ViewTitle
        {
            get { return "UnitOfMeasure List"; }
        }

        public string SearchWord
        {
            get { return _searchWord; }
            set { SetProperty(ref _searchWord, value); }
        }
    }
}