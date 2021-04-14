using RealWorldApp.Helpers;
using RealWorldApp.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RealWorldApp.ViewModels
{
    public class HomePageVM : BaseViewModel
    {
        #region Properties
        private ObservableCollection<PopularProduct> _productsCollection;
        public ObservableCollection<PopularProduct> ProductsCollection
        {
            get => _productsCollection;
            set
            {
                SetProperty(ref _productsCollection, value);
            }
        }
        private ObservableCollection<Category> _categoriesCollection;
        public ObservableCollection<Category> CategoriesCollection
        {
            get => _categoriesCollection;
            set
            {
                SetProperty(ref _categoriesCollection, value);
            }
        }

        private string _userName;
        public string UserName
        {
            get => _userName; set
            {
                SetProperty(ref _userName, value);
            }
        }

        private string _totalItems;
        public string TotalItems
        {
            get => _totalItems; set
            {
                SetProperty(ref _totalItems, value);
            }
        }
        #endregion

        #region Commands

        #endregion

        #region Constructor
        public HomePageVM()
        {
            //Initialize values
            ProductsCollection = new ObservableCollection<PopularProduct>();
            CategoriesCollection = new ObservableCollection<Category>();

            GetPopularProducts();
            GetCategories();

            UserName = Preferences.Get(Constants.UserName, string.Empty);

            Task.Run(() =>
            {
                LoadData();
            });
        }

        #endregion


        #region Methods

        public async void LoadData()
        {
            TotalCartItem response = await DataStore.GetTotalCartItems();
            TotalItems = response.totalItems.ToString();
        }
        private async void GetCategories()
        {
            List<Category> categories = await DataStore.GetCategories();
            foreach (var category in categories)
            {
                CategoriesCollection.Add(category);
            }
        }

        private async void GetPopularProducts()
        {
            List<PopularProduct> products = await DataStore.GetPopularProducts();
            foreach (var product in products)
            {
                ProductsCollection.Add(product);
            }
        }
        #endregion
    }
}
