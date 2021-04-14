using RealWorldApp.Models;
using RealWorldApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrdersPage : ContentPage
    {
        public OrdersPage()
        {
            InitializeComponent();
           
        }

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void LvOrders_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedOrder = e.SelectedItem as OrderByUser;
            if (selectedOrder != null)
            {
                Navigation.PushModalAsync(new OrderDetailPage(selectedOrder.Id));
            }
            ((ListView)sender).SelectedItem = null;
        }
    }
}