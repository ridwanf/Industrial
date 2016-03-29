using Core.Common.UI;
using Industrial.Service.ViewModel.Master;
using Industrial.Wpf.Infrastructures;
using Microsoft.Practices.Unity;

namespace Industrial.Wpf.ViewModels
{
    public class MainWindowViewModel:BindableBase
    {
        private ItemProductViewModel _itemProductViewModel;
        private AddEditItemViewModel _addEditItemViewModel;
        private TestPagingGridViewModel _testPagingGridViewModel;
        private SupplierViewModel _supplierViewModel;
        private AddEditSupplierViewModel _addEditSupplierViewModel;
        private MainWindowViewModel _mainWindowViewModel;
        private BankViewModel _bankViewModel;
        private AddEditBankViewModel _addEditBankViewModel;
        private UnitOfMeasureViewModel _unitOfMeasureViewModel;
        private AddEditUnitOfMeasureViewModel _addEditUnitOfMeasureViewModel;
        public RelayCommand<string> NavCommand { get; private set; }

        private BindableBase _currentViewModel;


        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            _itemProductViewModel = ContainerHelper.Container.Resolve<ItemProductViewModel>();
            _addEditItemViewModel = ContainerHelper.Container.Resolve<AddEditItemViewModel>();
            _testPagingGridViewModel = ContainerHelper.Container.Resolve<TestPagingGridViewModel>();

            _addEditSupplierViewModel = ContainerHelper.Container.Resolve<AddEditSupplierViewModel>();
            _supplierViewModel = ContainerHelper.Container.Resolve<SupplierViewModel>();

            _itemProductViewModel.EditItemRequested += NavToEditItem;
            _itemProductViewModel.AddItemRequested += NavToAddItem;

            _addEditItemViewModel.Done += NavToItemList;


            _supplierViewModel.EditSupplierRequested += NavToEditSupplier;
            _supplierViewModel.AddSupplierRequested +=  NavToAddSupplier;

            _addEditSupplierViewModel.Done += NavToSupplierist;


            _bankViewModel = ContainerHelper.Container.Resolve<BankViewModel>();
            _addEditBankViewModel = ContainerHelper.Container.Resolve<AddEditBankViewModel>();
            _bankViewModel.EditBankRequested += NavToEditBank;
            _bankViewModel.AddBankRequested += NavToAddBank;
            _addEditBankViewModel.Done += NavToBankList;


            _unitOfMeasureViewModel = ContainerHelper.Container.Resolve<UnitOfMeasureViewModel>();
            _addEditUnitOfMeasureViewModel = ContainerHelper.Container.Resolve<AddEditUnitOfMeasureViewModel>();
            _unitOfMeasureViewModel.EditUnitOfMeasureRequested += NavToEditUOM;
            _unitOfMeasureViewModel.AddUnitOfMeasureRequested += NavToAddUOM;
            _addEditUnitOfMeasureViewModel.Done += NavToUOMList;

        }

        private void NavToUOMList()
        {
             CurrentViewModel = _unitOfMeasureViewModel;

        }

        private void NavToEditUOM(UnitOfMeasureModel obj)
        {
            _addEditUnitOfMeasureViewModel.EditMode = true;
            _addEditUnitOfMeasureViewModel.Title = "Edit Unit Of Measure";
            _addEditUnitOfMeasureViewModel.SetUnitOfMeasure(obj);
            CurrentViewModel = _addEditUnitOfMeasureViewModel;
        }

        private void NavToAddUOM(UnitOfMeasureModel obj)
        {
            _addEditUnitOfMeasureViewModel.EditMode = false;
            _addEditUnitOfMeasureViewModel.Title = "Add Unit Of Measure";
            _addEditUnitOfMeasureViewModel.SetUnitOfMeasure(obj);
            CurrentViewModel = _addEditUnitOfMeasureViewModel;
        }

        private void NavToBankList()
        {
            CurrentViewModel = _bankViewModel;
        }

        private void NavToAddBank(BankModel obj)
        {
            _addEditBankViewModel.EditMode = false;
            _addEditBankViewModel.Title = "Add Bank";
            _addEditBankViewModel.SetBank(obj);
            CurrentViewModel = _addEditBankViewModel;
        }

        private void NavToEditBank(BankModel obj)
        {
            _addEditBankViewModel.EditMode = true;
            _addEditBankViewModel.Title = "Edit Bank";
            _addEditBankViewModel.SetBank(obj);
            CurrentViewModel = _addEditBankViewModel;
        }

        private void NavToItemList()
        {
            CurrentViewModel = _itemProductViewModel;
        }

        private void NavToAddItem(ItemProductModel obj)
        {
            _addEditItemViewModel.EditMode = false;
            _addEditItemViewModel.Title = "Add Item";
            _addEditItemViewModel.SetItem(obj);
            CurrentViewModel = _addEditItemViewModel;
        }

        private void NavToEditItem(ItemProductModel obj)
        {
            _addEditItemViewModel.EditMode = true;
            _addEditItemViewModel.Title = "Edit Item";
            _addEditItemViewModel.SetItem(obj);
            CurrentViewModel = _addEditItemViewModel;
        }


        #region supplier
        private void NavToSupplierist()
        {
            CurrentViewModel = _supplierViewModel;
        }

        private void NavToAddSupplier(SupplierModel obj)
        {
            _addEditSupplierViewModel.EditMode = false;
            _addEditSupplierViewModel.Title = "Add Supplier";
            _addEditSupplierViewModel.SetSupplier(obj);
            CurrentViewModel = _addEditSupplierViewModel;
        }

        private void NavToEditSupplier(SupplierModel obj)
        {
            _addEditSupplierViewModel.EditMode = true;
            _addEditSupplierViewModel.Title = "Edit Supplier";
            _addEditSupplierViewModel.SetSupplier(obj);
            CurrentViewModel = _addEditSupplierViewModel;
        }
        #endregion


        public BindableBase CurrentViewModel
        {
            get { return _currentViewModel; }
            set
            {
                SetProperty(ref _currentViewModel, value);
            }
        }

        private void OnNav(string destinataion)
        {
            switch (destinataion)
            {
                case "itemProduct":
                    CurrentViewModel = _itemProductViewModel;
                    break;
                case "supplier":
                    CurrentViewModel = _supplierViewModel;
                    break;
                case "bank":
                    CurrentViewModel = _bankViewModel;
                    break;
                case "uom":
                    CurrentViewModel = _unitOfMeasureViewModel;
                    break;
                case "testPaging":
                    CurrentViewModel = _testPagingGridViewModel;
                    break;
                default:
                    CurrentViewModel = _mainWindowViewModel;
                    break;

            }
        }

        public override string ViewTitle
        {
            get { return "Your Desktop"; }
        }
    }
}
