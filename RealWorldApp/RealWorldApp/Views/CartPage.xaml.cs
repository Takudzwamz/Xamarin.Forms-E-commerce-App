using RealWorldApp.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        private CartPageVM viewModel => (CartPageVM)this.BindingContext;
        public CartPage()
        {
            InitializeComponent();

        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }


        private void BtnProceed_Clicked(object sender, EventArgs e)
        {
        }


        private void Stepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            try
            {
                double value = e.NewValue;

                if (e.OldValue > 0)
                {
                    if (Math.Abs(value - e.OldValue) > 1)
                        return;
                    viewModel.UpdateStepper(value, int.Parse(((Stepper)sender).ClassId));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ClearItem_Tapped(object sender, EventArgs e)
        {
            viewModel.ClearItem(int.Parse(((Label)sender).ClassId));
        }
    }
}