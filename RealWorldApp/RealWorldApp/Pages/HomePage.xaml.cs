using RealWorldApp.Helpers;
using RealWorldApp.Models;
using System;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {

        public HomePage()
        {
            InitializeComponent();
        }

        private async void ImgMenu_Tapped(object sender, EventArgs e)
        {
            GridOverlay.IsVisible = true;
            await SlMenu.TranslateTo(0, 0, 400, Easing.Linear);
        }

        private void TapCloseMenu_Tapped(object sender, EventArgs e)
        {
            CloseHamBurgerMenu();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            CloseHamBurgerMenu();
        }

        private async void CloseHamBurgerMenu()
        {
            await SlMenu.TranslateTo(-250, 0, 400, Easing.Linear);
            GridOverlay.IsVisible = false;
        }


        private void CvCategories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currentSelection = e.CurrentSelection.FirstOrDefault() as Category;
            if (currentSelection == null) return;
            Navigation.PushModalAsync(new ProductListPage(currentSelection.id, currentSelection.name));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var currectSelection = e.CurrentSelection.FirstOrDefault() as PopularProduct;
            if (currectSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currectSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }

        private void TapCartIcon_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }

        private void TapOrders_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new OrdersPage());
        }

        private void TapContact_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new ContactPage());
        }

        private void TapCart_Tapped(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new CartPage());
        }

        private void TapLogout_Tapped(object sender, EventArgs e)
        {
            Preferences.Set(Constants.AccessToken, string.Empty);
            Preferences.Set(Constants.TokenExpirationTime, 0);
            Application.Current.MainPage = new NavigationPage(new SignupPage());
        }
    }
}