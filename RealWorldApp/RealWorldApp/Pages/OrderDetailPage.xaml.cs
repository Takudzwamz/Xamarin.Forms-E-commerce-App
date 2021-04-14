using RealWorldApp.Models;
using RealWorldApp.Services;
using RealWorldApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderDetailPage : ContentPage
    {
        private OrderDetailPageVM viewModel;
        public OrderDetailPage(int orderId)
        {
            InitializeComponent();
            viewModel = new OrderDetailPageVM(orderId);
            this.BindingContext = viewModel;
        }
       

        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}