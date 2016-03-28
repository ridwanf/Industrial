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
    public class SupplierViewModel : PageableBase
    {
        private readonly ISupplierService _service;
        private int _totalSupplier = 0;

        private string _searchWord;
        private SortablePageableCollection<SupplierModel> _suppliers;

        public RelayCommand NewSupplier { get; private set; }
        public RelayCommand<SupplierModel> EditSupplierCommand { get; private set; }
        public RelayCommand<SupplierModel> DeleteSupplierCommand { get; private set; }
        public RelayCommand AddSupplierCommand { get; private set; }
        public RelayCommand GoToNextPageCommand { get; private set; }
        public RelayCommand GoToPreviousPageCommand { get; private set; }
        public RelayCommand ChangePageSizeCommand { get; private set; }
        public RelayCommand SearchCommand { get; private set; }

        public event Action<SupplierModel> AddSupplierRequested = delegate { };
        public event Action<SupplierModel> EditSupplierRequested = delegate { };
        public event Action<SupplierModel> DeleteSupplierRequested = delegate { };

        public SupplierViewModel(ISupplierService service)
        {
            _service = service;
            EditSupplierCommand = new RelayCommand<SupplierModel>(OnEditCommand);
            DeleteSupplierCommand = new RelayCommand<SupplierModel>(OnDeleteCommand);
            AddSupplierCommand = new RelayCommand(OnAddCommand);
            SearchCommand = new RelayCommand(OnSearchCommand);
            // Create Commands 
            GoToNextPageCommand = new RelayCommand(GoToNextPage);
            GoToPreviousPageCommand = new RelayCommand(GoToPreviousPage);
            ChangePageSizeCommand = new RelayCommand(ChangePageSize);
        }

        private void ChangePageSize()
        {
            Suppliers = new SortablePageableCollection<SupplierModel>(_service.GetAll(Suppliers.CurrentPageNumber, Suppliers.PageSize), _totalSupplier, 1, Suppliers.PageSize);

        }

        private void GoToPreviousPage()
        {
            if (_suppliers.CurrentPageNumber > 1)
            {
                var currentPage = _suppliers.CurrentPageNumber;
                var data = _service.GetAll(Suppliers.CurrentPageNumber - 1, Suppliers.PageSize);
                Suppliers = new SortablePageableCollection<SupplierModel>(data, _totalSupplier, currentPage, Suppliers.PageSize);
                _suppliers.GoToPreviousPage();
            }
        }

        private void GoToNextPage()
        {
            if (Suppliers.CurrentPageNumber != Suppliers.TotalPagesNumber)
            {
                Suppliers = new SortablePageableCollection<SupplierModel>(_service.GetAll(Suppliers.CurrentPageNumber + 1, Suppliers.PageSize), _totalSupplier, Suppliers.CurrentPageNumber, Suppliers.PageSize);
                Suppliers.GoToNextPage();
            }
        }

        private void OnSearchCommand()
        {
            if (!string.IsNullOrWhiteSpace(_searchWord))
            {
                int total;
                var data = _service.Search(_searchWord, 1, Suppliers.PageSize, out total);
                _totalSupplier = total;
                Suppliers = new SortablePageableCollection<SupplierModel>(data, _totalSupplier, 1, Suppliers.PageSize);
            }
            else
            {
                LoadSuppliers();
            }
        }

        private void OnAddCommand()
        {
            AddSupplierRequested(new SupplierModel
            {
                Id = 0,
                IsActive = true,
                CreatedDate = DateTime.Now,
                RegisterDate = DateTime.Now,
                ContractDate = DateTime.Now,

            });
           // _totalSupplier++;
        }

        private async void OnDeleteCommand(SupplierModel obj)
        {
            var result = await DialogService.AskQuestionAsync("Delete Data",
                "Are you sure you want to delete this record?");
            if (result == MessageDialogResult.Affirmative)
            {
                await _service.DeleteAsync(obj.Id);
                _suppliers.Remove(obj);
                _totalSupplier--;
            }
            LoadSuppliers();
        }

        private void OnEditCommand(SupplierModel obj)
        {
            EditSupplierRequested(obj);
        }

        public SortablePageableCollection<SupplierModel> Suppliers
        {
            get { return _suppliers; }
            set { SetProperty(ref _suppliers, value); }
        }

        public async void LoadSuppliers()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                return;
            }
            _totalSupplier = _service.GetAll().Count;
            Suppliers = new SortablePageableCollection<SupplierModel>(await _service.GetAllAsync(1, 5), _totalSupplier, 1, 5);

        }


   

        public override string ViewTitle
        {
            get { return "Supplier List"; }
        }

        public string SearchWord
        {
            get { return _searchWord; }
            set { SetProperty(ref _searchWord, value); }
        }
    }
}
