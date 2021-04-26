using RealWorldApp.Helpers;
using RealWorldApp.Models;
using RealWorldApp.Pages;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class CartPageVM : BaseViewModel
    {

        #region Properties

        private bool IsItemsLoaded { get; set; } = false;
        private CustomerBasket customerBasket { get; set; }
        private ObservableCollection<CartItem> _shoppingCartCollection;

        public ObservableCollection<CartItem> ShoppingCartCollection
        {
            get => _shoppingCartCollection;
            set
            {
                SetProperty(ref _shoppingCartCollection, value);
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set { SetProperty(ref _totalPrice, value); }
        }
        #endregion

        #region Commands
        public Command ClearCartCommand { get; }
        public Command ProceedCommand { get; }
        #endregion

        #region Constructor
        public CartPageVM()
        {
            ShoppingCartCollection = new ObservableCollection<CartItem>();
            ClearCartCommand = new Command(ClearCart);
            ProceedCommand = new Command(ProceedNow);
            Task.Run(() =>
            {
                LoadData();
            });
        }


        #endregion

        #region methods
        internal async void ClearItem(int itemId)
        {

            CartItem current = ShoppingCartCollection.FirstOrDefault(d => d.Id == itemId);
            customerBasket.Items.Remove(current);

            UpdateBasketNow();
           
        }

        private void UpdateBasketNow()
        {
            //update the API.
            Task.Run(async () =>
            {
                try
                {
                    IsBusy = true;
                    await Task.Delay(100);
                    bool status = await DataStore.UpdateCartBasket(customerBasket);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    IsBusy = false;
                }
                LoadData(customerBasket);
            });
        }

        internal void UpdateStepper(double value, int itemId)
        {
            if (!IsItemsLoaded)
                return;

            CartItem current = ShoppingCartCollection.FirstOrDefault(d => d.Id == itemId);
            if(value != current.Quantity)
            {
                return;
            }
            //Update cart item to database. 
            UpdateBasketNow();
        }

        private void ProceedNow(object obj)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new PlaceOrderPage(TotalPrice));
        }

        private async void LoadData(CustomerBasket _customerBasket = null)
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);

                if (_customerBasket == null)
                    customerBasket = await DataStore.GetCustomerBasket();
                else
                    customerBasket = _customerBasket;

                Device.BeginInvokeOnMainThread(() =>
                {
                    //clear the collection. 
                    ShoppingCartCollection = new ObservableCollection<CartItem>();
                    //Items
                    foreach (var shoppingCart in customerBasket.Items)
                    {
                        ShoppingCartCollection.Add(shoppingCart);
                    }

                    //Total Price
                    TotalPrice = customerBasket.Items.Sum(d => d.price * d.Quantity);

                    MessagingCenter.Send<object>(this, Constants.Messaging.UpdateCartCount);
                    IsItemsLoaded = true;

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
        private void ClearCart(object obj)
        {
            Task.Run(async () =>
            {
                try
                {
                    IsBusy = true;
                    await Task.Delay(100);
                    bool response = await DataStore.ClearShoppingCart();
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (response)
                        {

                            await Application.Current.MainPage.DisplayAlert("", "Your cart has been cleared", "Alright");
                            ShoppingCartCollection.Clear();
                            TotalPrice = 0;
                            MessagingCenter.Send<object>(this, Constants.Messaging.UpdateCartCount);

                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("", "Something went wrong", "Cancel");
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

            });
        }


        #endregion
    }
}
