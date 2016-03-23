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
        
        public RelayCommand<string> NavCommand { get; private set; }

        private BindableBase _currentViewModel;


        public MainWindowViewModel()
        {
            NavCommand = new RelayCommand<string>(OnNav);
            _itemProductViewModel = ContainerHelper.Container.Resolve<ItemProductViewModel>();
            _addEditItemViewModel = ContainerHelper.Container.Resolve<AddEditItemViewModel>();

            _itemProductViewModel.EditItemRequested += NavToEditItem;
            _itemProductViewModel.AddItemRequested += NavToAddItem;
            _addEditItemViewModel.Done += NavToItemList;
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
                default:
                    CurrentViewModel = _itemProductViewModel;
                    break;

            }
        }

        public override string ViewTitle
        {
            get { return "Your Desktop"; }
        }
    }
}
