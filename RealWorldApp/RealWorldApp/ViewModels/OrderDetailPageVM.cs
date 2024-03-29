﻿using RealWorldApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RealWorldApp.ViewModels
{
    public class OrderDetailPageVM : BaseViewModel
    {
        #region Properties
        private ObservableCollection<OrderItemDto> _orderDetailCollection;
        public ObservableCollection<OrderItemDto> OrderDetailCollection
        {
            get => _orderDetailCollection;
            set
            {
                SetProperty(ref _orderDetailCollection, value);
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
        #endregion

        #region Commands
        #endregion

        #region Constructor
        public OrderDetailPageVM(int orderId)
        {
            OrderDetailCollection = new ObservableCollection<OrderItemDto>();
            Task.Run(() =>
            {
                GetOrderDetail(orderId);
            });

        }
        #endregion

        #region methods
        private async void GetOrderDetail(int orderId)
        {
            try
            {
                IsBusy = true;
                await Task.Delay(100);
                Order order = await DataStore.GetOrderById(orderId);
                Device.BeginInvokeOnMainThread(() =>
                {
                    foreach (var item in order.OrderItems)
                    {
                        OrderDetailCollection.Add(item);
                    }

                    TotalPrice = order.Total;

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
