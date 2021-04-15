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

        private Product _currentProduct;
        public Product CurrentProduct
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
            CurrentProduct = new Product();
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
                        Brand = CurrentProduct.ProductBrand,
                        PictureUrl = CurrentProduct.PictureUrl,
                        price = CurrentProduct.Price,
                        ProductName = CurrentProduct.Name,
                        Quantity = int.Parse(CurrentProduct.Quantity.ToString()),
                        totalAmount = CurrentProduct.TotalPrice,
                        Type = CurrentProduct.ProductType,
                        Id = CurrentProduct.Id
                    };
                    IsBusy = true;
                    await Task.Delay(100);

                    bool response = await DataStore.AddItemsToCart(cart);
                    if (response)
                    {
                        await Application.Current.MainPage.DisplayAlert("", "Your items has been added to the cart", "Alright");
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
            });
        }
        private async void GetProductDetails(int productId)
        {

            try
            {
                IsBusy = true;
                await Task.Delay(100);
                CurrentProduct = await DataStore.GetProductById(productId);

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
