using RealWorldApp.Helpers;
using RealWorldApp.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class ProductDetailPageVM : BaseViewModel
    {
        #region Properties
        private int _productId;
        public int ProductId
        {
            get => _productId;
            set
            {
                SetProperty(ref _productId, value);
            }
        }

        private double _quantity = 1;
        public double Quantity
        {
            get => _quantity;
            set
            {
                SetProperty(ref _quantity, value);
                if (CurrentProduct != null)
                    TotalPrice = value * CurrentProduct.price;
            }
        }

        private double _totalPrice;
        public double TotalPrice
        {
            get => _totalPrice;
            set
            {
                SetProperty(ref _totalPrice, value);
            }
        }

        private ProductData _currentProduct;
        public ProductData CurrentProduct
        {
            get => _currentProduct;
            set
            {
                SetProperty(ref _currentProduct, value);
            }
        }

        #endregion

        #region Commands
        public Command AddCartCommand { get; }
        #endregion

        #region Constructor
        public ProductDetailPageVM(int productId)
        {
            CurrentProduct = new ProductData();
            ProductId = productId;
            AddCartCommand = new Command(AddToCartNow);

            Task.Run(() =>
            {
                GetProductDetails(productId);

            });
        }

        #endregion


        #region methods
        private void AddToCartNow(object obj)
        {
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
                        Quantity = Convert.ToInt32(Quantity),
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
                            await Application.Current.MainPage.DisplayAlert("", "Your items has been added to the cart", "Alright");
                            MessagingCenter.Send<object>(this, Constants.Messaging.UpdateCartCount);
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
        private async void GetProductDetails(int productId)
        {

            try
            {
                IsBusy = true;
                await Task.Delay(100);
                var data = await DataStore.GetProductById(productId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    CurrentProduct = data;
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
        #endregion
    }
}
