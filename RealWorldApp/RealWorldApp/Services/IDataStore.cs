using RealWorldApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace RealWorldApp.Services
{
    public interface IDataStore
    {
        Task<bool> CheckEmailExist(string email);
        Task<UserDto> SignUp(Register registerData);
        Task<UserDto> Login(Login loginData);
        Task<TotalCartItem> GetTotalCartItems();
        Task<CustomerBasket> GetCustomerBasket();
        Task<List<Category>> GetCategories();
        Task<List<ProductData>> GetPopularProducts();
        Task<List<CartItem>> GetShoppingCartItems();
        Task<CartSubTotal> GetCartSubTotal();
        Task<bool> ClearShoppingCart();
        Task<List<ProductData>> GetProducts(object parameters);
        Task<ProductData> GetProductById(int productId);
        Task<bool> AddItemsToCart(CartItem cart);
        Task<bool> UpdateCartBasket(CustomerBasket basket);

        Task<List<OrderByUser>> GetOrdersByUser();
        Task<Order> GetOrderById(int orderId);
        Task<MakeOrderResponse> PlaceOrder(OrderDto order);
        Task<PaymentModel> ProcessPayFastPayment(OrderDto order);
        Task<List<DeliveryMethod>> GetDeliveryMethods();
    }
}
