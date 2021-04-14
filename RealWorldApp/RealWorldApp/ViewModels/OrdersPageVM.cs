using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

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
            List<OrderByUser> orders = await DataStore.GetOrdersByUser();
            foreach (var order in orders)
            {
                OrdersCollection.Add(order);
            }
        }
        #endregion
    }
}
