using System.Collections.ObjectModel;
using Industrial.Service.Services;
using Industrial.Service.ViewModel.Master;
using WpfTest.Infrastructures;

namespace WpfTest.ViewModels
{
    public class ItemProductViewModel : BindableBase
    {
        public IItemProductService _Service;
        private ObservableCollection<ItemProductModel> _items;


        public RelayCommand NewItem { get; set; }
        public RelayCommand EditItem { get; set; }
        public RelayCommand DeleteItem { get; set; }
        public RelayCommand TestAlert { get; set; }



        public ItemProductViewModel(IItemProductService service)
        {
            _Service = service;
           Items = new ObservableCollection<ItemProductModel>( _Service.GetAll());
        }



        public ObservableCollection<ItemProductModel> Items
        {
            get { return _items; }
            set { SetProperty(ref _items,value); }
        }


        public async void LoadItems()
        {
            Items = new ObservableCollection<ItemProductModel>(await _Service.GetAllAsync());
        }
        //public ItemProductViewModel(ObservableCollection<ItemProductModel> items,
        //    RelayCommand newItem,
        //    RelayCommand editItem, ItemProductService service)
        //{
        //    NewItem = newItem;
        //    EditItem = editItem;
        //    _Service = service;
        //    DeleteItem = new RelayCommand(DeleteSelectedItem, o => o != null);
        //    Items = items;
        //}

        //public ItemProductModel selectedItem;

        //public ItemProductModel SelectedItem
        //{
        //    get { return selectedItem; }
        //    set
        //    {
        //        if (Equals(value, selectedItem)) return;
        //        selectedItem = value;
        //        OnPropertyChanged();
        //    }
        //}

        //private void DeleteSelectedItem(object obj)
        //{
        //    if (MessageBox.Show("Are you sure you want to delete this Match record?",
        //        "Delete Match",
        //            MessageBoxButton.YesNo) == MessageBoxResult.Yes)
        //    {
        //        Items.Remove(selectedItem);
        //        SelectedItem = null;
        //    }
        //}
    }
}
