using RealWorldApp.Models;
using RealWorldApp.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class SignupPageVM : BaseViewModel
    {
        #region properties

        private Register _register;
        public Register RegisterData
        {
            get => _register;
            set
            {
                SetProperty(ref _register, value);
            }
        }

        private string _tempPassword;
        public string TempPassword
        {
            get => _tempPassword; set
            {
                SetProperty(ref _tempPassword, value);
            }
        }


        #endregion


        #region Commands
        public Command SignUpCommand { get; }
        public Command LoginCommand { get; }

        #endregion


        #region Constructor
        public SignupPageVM()
        {
            SignUpCommand = new Command(SignUp, CanSignUp);
            LoginCommand = new Command(GotoLogin);
        }
        #endregion

        #region Methods
        private async void GotoLogin(object obj)
        {
            await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
        }

        private bool CanSignUp(object arg)
        {
            //if (TempPassword != RegisterData.Password)
            //    return false;
            if (string.IsNullOrWhiteSpace(RegisterData.Email))
                return false;
            if (string.IsNullOrWhiteSpace(RegisterData.DisplayName))
                return false;

            return true;
        }

        private async void SignUp(object obj)
        {

            if (TempPassword != RegisterData.Password)
            {
                await Application.Current.MainPage.DisplayAlert("Password mismatch", "Please check your confirm password", "Cancel");
                return;
            }

            try
            {
                IsBusy = true;
                await Task.Delay(100);
                bool IsEmailExists = await DataStore.CheckEmailExist(RegisterData.Email.Trim()); //emailexists
                if (!IsEmailExists)
                {
                    await Application.Current.MainPage.DisplayAlert("Email Exists", "Please enter another email or proceed to Login", "Cancel");
                    return;
                }

                bool IsRegSuccess = await DataStore.SignUp(RegisterData);

                if (IsRegSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert("Hi", "Your account has been created", "Alright");
                    await Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage());
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

        }

        #endregion
    }
}
