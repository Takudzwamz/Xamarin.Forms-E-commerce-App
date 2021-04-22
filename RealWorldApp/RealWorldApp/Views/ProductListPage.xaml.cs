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
    public partial class ProductListPage : ContentPage
    {
        private ProductListPageVM viewModel;
        public ProductListPage(int categoryId, string categoryName)
        {
            InitializeComponent();
            viewModel = new ProductListPageVM(categoryId, categoryName);
            this.BindingContext = viewModel;
        }
     
        private void TapBack_Tapped(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void CvProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ProductData currectSelection = e.CurrentSelection.FirstOrDefault() as ProductData;
            if (currectSelection == null) return;
            Navigation.PushModalAsync(new ProductDetailPage(currectSelection.id));
            ((CollectionView)sender).SelectedItem = null;
        }
    }
}