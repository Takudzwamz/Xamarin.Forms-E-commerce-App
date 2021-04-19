using Newtonsoft.Json;
using RealWorldApp.Helpers;
using RealWorldApp.Models;
using RealWorldApp.Pages;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Linq;
using System.Reflection;

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

        private int _deliveryMethod;
        public int DeliveryMethod
        {
            get => _deliveryMethod;
            set
            {
              
                SetProperty(ref _deliveryMethod, value);
            }
        }

       
        private ObservableCollection<DeliveryMethod> _deliveryMethods;
        public ObservableCollection<DeliveryMethod> DeliveryMethods
        {
            get => _deliveryMethods;
            set
            {
                SetProperty(ref _deliveryMethods, value);
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

            Task.Run(() =>
            {
                GetDeliveryMethods();
            });
        }

        #endregion

        #region methods

        private async Task GetDeliveryMethods()
        {
            var data = await DataStore.GetDeliveryMethods();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                DeliveryMethods = new ObservableCollection<DeliveryMethod>(data);
                DeliveryMethods.Add(new DeliveryMethod() { id = 77, description = "Hello King", shortName = "Shortest Name", deliveryTime = "1 Hour", price = 700 });
            });
        }
        private async void PlaceOrderNow(object obj)
        {

            //Check the Address for information

            if(DeliveryMethod == 0)
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Input", "Please choose a Delivery Method", "Alright");
                return;
            }

            if (CurrentAddress.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(CurrentAddress, null)).Any(d=> d.Value == string.Empty))
            {
                await Application.Current.MainPage.DisplayAlert("Invalid Input", "Please fill address", "Alright");
                return;
            }

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
                        DeliveryMethodId = DeliveryMethod
                    };

                    IsBusy = true;
                    await Task.Delay(100);
                    //prepare payment
                    PaymentModel paymentData = await DataStore.ProcessPayFastPayment(order);

                    await Browser.OpenAsync(paymentData.PaymentLink, BrowserLaunchMode.SystemPreferred);

                    //Here is the partial code for WebAuthencator. 
                    // var authenticationResult = await WebAuthenticator.AuthenticateAsync(new Uri(paymentData.PaymentLink), new Uri(paymentData.CallbackLink));
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
