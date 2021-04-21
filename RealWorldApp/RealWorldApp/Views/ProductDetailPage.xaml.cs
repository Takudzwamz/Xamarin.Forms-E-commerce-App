using RealWorldApp.Models;
using RealWorldApp.Services;
using RealWorldApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RealWorldApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductDetailPage : ContentPage
    {
        private ProductDetailPageVM viewModel;
        public ProductDetailPage(int productId)
        {
            InitializeComponent();
            viewModel = new ProductDetailPageVM(productId);
            this.BindingContext = viewModel;
        }
        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
      
    }
}