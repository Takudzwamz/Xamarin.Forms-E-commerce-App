using RealWorldApp.Helpers;
using RealWorldApp.Models;
using RealWorldApp.Pages;
using System;
using System.Threading.Tasks;
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
        private void PlaceOrderNow(object obj)
        {
            Task.Run(async () =>
            {

                try
                {

                    var order = new OrderDto()
                    {
                        ShipToAddress = CurrentAddress,
                        BasketId = Preferences.Get(Constants.BasketID, string.Empty),
                        //DeliveryMethodId = (await DataStore.GetCustomerBasket()).DeliveryMethodId.Value
                    };

                    IsBusy = true;
                    await Task.Delay(100);
                    //prepare payment
                    PaymentModel paymentData = await DataStore.ProcessPayFastPayment(order);
                    var authenticationResult = await WebAuthenticator.AuthenticateAsync(new Uri(paymentData.PaymentLink), new Uri(paymentData.CallbackLink));
                    //debug here
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
