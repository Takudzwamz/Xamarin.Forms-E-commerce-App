using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
                var parameters = new ProductSpecParams
                {
                    TypeId = typeId,
                    PageIndex = PageIndex,
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

        #endregion
    }
}
