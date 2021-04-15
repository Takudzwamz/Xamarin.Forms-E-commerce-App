using RealWorldApp.Models;
using RealWorldApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class LoginPageVM : BaseViewModel
    {
        #region Properties
        private Login _loginData;
        public Login LoginData
        {
            get => _loginData; set
            {
                SetProperty(ref _loginData, value);
            }
        }
        #endregion

        #region Commands
        public Command LoginCommand { get; }
        #endregion

        #region Constructor
        public LoginPageVM()
        {
            LoginData = new Login();
            LoginCommand = new Command(LoginNow, CanLogin);
        }


        #endregion

        #region Methods
        private bool CanLogin(object arg)
        {
            if (string.IsNullOrWhiteSpace(LoginData.Email))
                return false;
            if (string.IsNullOrWhiteSpace(LoginData.Password))
                return false;
            return true;
        }

        private async void LoginNow(object obj)
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                bool IsSucces = await DataStore.Login(LoginData);

                Preferences.Set("email", LoginData.Email);
                Preferences.Set("password", LoginData.Password);

                if (IsSucces)
                {
                    Application.Current.MainPage = new NavigationPage(new HomePage());
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong", "Cancel");
                }
            }
            catch (Exception)
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
