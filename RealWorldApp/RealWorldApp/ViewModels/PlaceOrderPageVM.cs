using Newtonsoft.Json;
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
            if (Preferences.ContainsKey(Constants.AddressStore))
                CurrentAddress = JsonConvert.DeserializeObject<Address>(Preferences.Get(Constants.AddressStore, string.Empty));
            else
                CurrentAddress = new Address();

            TotalPrice = totalPrice;
            OrderCommand = new Command(PlaceOrderNow);
        }

        #endregion

        #region methods
        private void PlaceOrderNow(object obj)
        {

            //Check the Address for information

            Task.Run(async () =>
            {

                try
                {
                    //Save Address to preference.
                    Preferences.Set(Constants.AddressStore, JsonConvert.SerializeObject(CurrentAddress));

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
                    await Browser.OpenAsync(paymentData.PaymentLink, BrowserLaunchMode.SystemPreferred);

                  //  var authenticationResult = await WebAuthenticator.AuthenticateAsync(new Uri(paymentData.PaymentLink), new Uri(paymentData.CallbackLink));
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
