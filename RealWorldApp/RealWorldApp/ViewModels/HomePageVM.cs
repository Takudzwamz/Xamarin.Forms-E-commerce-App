using RealWorldApp.Helpers;
using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class HomePageVM : BaseViewModel
    {
        #region Properties
        private ObservableCollection<ProductData> _productsCollection;
        public ObservableCollection<ProductData> ProductsCollection
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
            ProductsCollection = new ObservableCollection<ProductData>();
            CategoriesCollection = new ObservableCollection<Category>();

            Task.Run(() =>
             {
                 GetPopularProducts();
             });

            Task.Run(() =>
            {
                GetCategories();
            });

            UserName = Preferences.Get(Constants.UserName, string.Empty);

            Task.Run(() =>
            {
                LoadData();
            });

            MessagingCenter.Subscribe<object>(this, Constants.Messaging.UpdateCartCount, (sender) =>
            {
                LoadData();
            });
        }

        #endregion


        #region Methods

        public async void LoadData()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                TotalCartItem response = await DataStore.GetTotalCartItems();
                Device.BeginInvokeOnMainThread(() =>
                {
                    TotalItems = response.totalItems.ToString();
                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
        private async void GetCategories()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                List<Category> categories = await DataStore.GetCategories();
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var category in categories)
                    {
                        CategoriesCollection.Add(category);
                    }

                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void GetPopularProducts()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                List<ProductData> products = await DataStore.GetPopularProducts();
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var product in products)
                    {
                        ProductsCollection.Add(product);
                    }

                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }
        #endregion
    }
}
