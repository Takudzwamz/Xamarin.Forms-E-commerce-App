using RealWorldApp.Models;
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
            ProductId = productId;
            AddCartCommand = new Command(AddToCartNow);

            Task.Run(() =>
            {
                GetProductDetails(productId);

            });
        }

        #endregion


        #region methods
        private async void AddToCartNow(object obj)
        {
            Task.Run(async () =>
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

                bool response = await DataStore.AddItemsToCart(cart);
                if (response)
                {
                    await Application.Current.MainPage.DisplayAlert("", "Your items has been added to the cart", "Alright");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Oops", "Something went wrong", "Cancel");
                }
            });
        }
        private async void GetProductDetails(int productId)
        {
            CurrentProduct = await DataStore.GetProductById(productId);
        }
        #endregion
    }
}
