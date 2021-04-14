using RealWorldApp.Helpers;
using RealWorldApp.Models;
using RealWorldApp.Pages;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class PlaceOrderPageVM : BaseViewModel
    {
        #region Properties
        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                SetProperty(ref _totalPrice, value);
            }
        }

        private Address _address;
        public Address CurrentAddress
        {
            get => _address;
            set
            {
                SetProperty(ref _address, value);
            }
        }
        #endregion

        #region Commands
        public Command OrderCommand { get; }
        #endregion

        #region Constructor
        public PlaceOrderPageVM(double totalPrice)
        {
            TotalPrice = totalPrice;
            OrderCommand = new Command(PlaceOrderNow);
        }

        #endregion

        #region methods
        private async void PlaceOrderNow(object obj)
        {
            var order = new OrderDto()
            {
                ShipToAddress = CurrentAddress,
                BasketId = Preferences.Get(Constants.BasketID, string.Empty),
                DeliveryMethodId = (await DataStore.GetCustomerBasket()).DeliveryMethodId.Value
            };
            Order response = await DataStore.PlaceOrder(order);
            if (response != null)
            {
                await Application.Current.MainPage.DisplayAlert("", "Your Order Id is " + response.Id, "Alright");
                Application.Current.MainPage = new NavigationPage(new HomePage());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong", "Cancel");
            }
        }

        #endregion
    }
}
