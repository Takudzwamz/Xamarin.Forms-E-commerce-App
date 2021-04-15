using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class OrdersPageVM : BaseViewModel
    {
        #region Properties
        private ObservableCollection<OrderByUser> _ordersCollection;
        public ObservableCollection<OrderByUser> OrdersCollection
        {
            get => _ordersCollection;
            set
            {
                SetProperty(ref _ordersCollection, value);
            }
        }

        #endregion

        #region Commands
        #endregion

        #region Constructor

        public OrdersPageVM()
        {
            OrdersCollection = new ObservableCollection<OrderByUser>();
            Task.Run(() =>
            {
                GetOrderItems();
            });
        }
        #endregion

        #region methods
        private async Task GetOrderItems()
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                List<OrderByUser> orders = await DataStore.GetOrdersByUser();
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var order in orders)
                    {
                        OrdersCollection.Add(order);
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
