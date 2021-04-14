using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class CartPageVM : BaseViewModel
    {

        #region Properties
        private ObservableCollection<CartItem> _shoppingCartCollection;

        public ObservableCollection<CartItem> ShoppingCartCollection
        {
            get => _shoppingCartCollection;
            set
            {
                SetProperty(ref _shoppingCartCollection, value);
            }
        }

        private string _totalPrice;
        public string TotalPrice
        {
            get => _totalPrice;
            set { SetProperty(ref _totalPrice, value); }
        }
        #endregion

        #region Commands
        public Command ClearCartCommand { get; }
        #endregion

        #region Constructor
        public CartPageVM()
        {
            ShoppingCartCollection = new ObservableCollection<CartItem>();
            ClearCartCommand = new Command(ClearCart);
            Task.Run(() =>
            {
                LoadData();
            });
        }

        #endregion

        #region methods

        private async void LoadData()
        {
            CustomerBasket basket = await DataStore.GetCustomerBasket();

            //Items
            foreach (var shoppingCart in basket.Items)
            {
                ShoppingCartCollection.Add(shoppingCart);
            }

            //Total Price
            TotalPrice = basket.Items.Sum(d => d.price).ToString();

        }
        private async void ClearCart(object obj)
        {
            bool response = await DataStore.ClearShoppingCart();
            if (response)
            {
                await Application.Current.MainPage.DisplayAlert("", "Your cart has been cleared", "Alright");
                ShoppingCartCollection.Clear();
                TotalPrice = "0";
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Something went wrong", "Cancel");
            }
        }
     

        #endregion
    }
}
