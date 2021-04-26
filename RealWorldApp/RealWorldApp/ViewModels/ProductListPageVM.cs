using RealWorldApp.Helpers;
using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class ProductListPageVM : BaseViewModel
    {
        #region Properties
        private ObservableCollection<ProductData> _productByCategoryCollection;
        public ObservableCollection<ProductData> ProductByCategoryCollection
        {
            get => _productByCategoryCollection;
            set
            {
                SetProperty(ref _productByCategoryCollection, value);
            }
        }
        private string _categoryName;
        public string CategoryName
        {
            get => _categoryName;
            set
            {
                SetProperty(ref _categoryName, value);
            }
        }

        public int PageIndex { get; set; }

        #endregion

        #region Commands
        #endregion

        #region Constructor
        public ProductListPageVM(int typeId, string categoryName)
        {
            ProductByCategoryCollection = new ObservableCollection<ProductData>();
            CategoryName = categoryName;


            Task.Run(() =>
            {
                GetProducts(typeId);
            });
        }
        #endregion

        #region methods
        private async void GetProducts(int typeId)
        {
            try
            {
                var parameters = new
                {
                    TypeId = typeId
                };
                IsBusy = true;
                await Task.Delay(100);
                List<ProductData> products = await DataStore.GetProducts(parameters);
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var product in products)
                    {
                        ProductByCategoryCollection.Add(product);
                    }

                });
            }
            catch (Exception ex)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        internal void AddToCart(int id)
        {
            var CurrentProduct = ProductByCategoryCollection.FirstOrDefault(d => d.id == id);

            Task.Run(async () =>
            {

                try
                {
                    CartItem cart = new CartItem()
                    {
                        Brand = CurrentProduct.productBrand,
                        PictureUrl = CurrentProduct.pictureUrl,
                        price = CurrentProduct.price,
                        ProductName = CurrentProduct.name,
                        Quantity = 1,
                        Type = CurrentProduct.productType,
                        Id = CurrentProduct.id
                    };
                    IsBusy = true;
                    await Task.Delay(100);

                    bool response = await DataStore.AddItemsToCart(cart);
                    Device.BeginInvokeOnMainThread(async () =>
                    {

                        if (response)
                        {
                            MessagingCenter.Send<object>(this, Constants.Messaging.UpdateCartCount);
                           // await Application.Current.MainPage.DisplayAlert("", "Your items has been added to the cart", "Alright");
                            bool answer = await Application.Current.MainPage.DisplayAlert("Cart Updated?", "Would you like to visit cart?", "Yes", "Continue Shopping");
                            if (answer)
                            {
                                await Application.Current.MainPage.Navigation.PushModalAsync(new Pages.CartPage());
                            }
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong", "Cancel");
                        }
                    });
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
