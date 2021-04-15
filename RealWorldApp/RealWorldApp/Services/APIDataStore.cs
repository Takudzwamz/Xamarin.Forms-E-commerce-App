using Newtonsoft.Json;
using RealWorldApp.Helpers;
using RealWorldApp.Models;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RealWorldApp.Services
{
    public class APIDataStore : IDataStore
    {
        #region API Calls

        //public async Task<string> MakeCall(string Url, object payload = null, IDictionary<string, string> parameters = null, Method method = Method.POST)
        public async Task<string> MakeCall(string Url, object payload = null, object parameters = null, Method method = Method.POST)
        {
            //await TokenValidator.CheckTokenValidity();
            RestClient client = new RestClient(AppSettings.ApiUrl + Url)
            {
                Authenticator = new RestSharp.Authenticators.JwtAuthenticator(Preferences.Get(Constants.AccessToken, string.Empty))
            };
            RestRequest request = new RestRequest(method);
            if (payload != null)
                request.AddParameter("application/json", JsonConvert.SerializeObject(payload), ParameterType.RequestBody);
            if (parameters != null)
            {
                //foreach (var param in parameters)
                //request.AddQueryParameter(param.Key, param.Value);
                request.AddObject(parameters);

            }

            var response = client.Execute(request);
            return response.Content;
        }
        public async Task<bool> DeleteAPI(string Url)
        {
            //await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = await httpClient.DeleteAsync(AppSettings.ApiUrl + Url);
            return response.IsSuccessStatusCode;
        }

        public async Task<string> GetAPI(string Url)
        {
            //await TokenValidator.CheckTokenValidity();
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get(Constants.AccessToken, string.Empty));
            var response = await httpClient.GetStringAsync(AppSettings.ApiUrl + Url);
            return response;
        }

        public static Task<string> PostAPI(string Url, object data, out bool IsSuccessful)
        {
            //TokenValidator.CheckTokenValidity().ConfigureAwait(true);
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("accessToken", string.Empty));
            var response = httpClient.PostAsync(AppSettings.ApiUrl + Url, content).Result;
            IsSuccessful = response.IsSuccessStatusCode;
            var jsonResult = response.Content.ReadAsStringAsync().Result;
            return Task.FromResult(jsonResult);

        }

        #endregion
        public async Task<bool> CheckEmailExist(string email)
        {
            var result = await GetAPI($"{Constants.APIEndpoints.APIAccounts.emailexists}?email={email}");

            return bool.Parse(result);
        }

        public async Task<UserDto> SignUp(Register registerData)
        {
            var result = await PostAPI(Constants.APIEndpoints.APIAccounts.register, registerData, out bool IsSucessful);
            UserDto user = JsonConvert.DeserializeObject<UserDto>(result);
            if (IsSucessful)
                Preferences.Set(Constants.AccessToken, user.Token);
            user.IsSuccess = IsSucessful;
            return user;
        }

        public async Task<UserDto> Login(Login loginData)
        {
            var result = await PostAPI(Constants.APIEndpoints.APIAccounts.login, loginData, out bool IsSucessful);
            UserDto user = JsonConvert.DeserializeObject<UserDto>(result);
            if (IsSucessful)
                Preferences.Set(Constants.AccessToken, user.Token);
            user.IsSuccess = IsSucessful;
            return user;
        }

        public async Task<TotalCartItem> GetTotalCartItems()
        {
            var basket = await GetCustomerBasket();
            return new TotalCartItem() { totalItems = basket.Items.Sum(d => d.Quantity) };
        }

        public async Task<CustomerBasket> GetCustomerBasket()
        {
            if (Preferences.ContainsKey(Constants.BasketID))
            {
                var result = await GetAPI($"{Constants.APIEndpoints.APICart.Basket}?id={Preferences.Get(Constants.BasketID, string.Empty)}");
                return JsonConvert.DeserializeObject<CustomerBasket>(result);
            }
            else
            {
                return await CreateNewBasket();
            }
        }

        private async Task<CustomerBasket> CreateNewBasket()
        {
            CustomerBasket basket = new CustomerBasket()
            {
                Id = System.Guid.NewGuid().ToString(),

            };
            Preferences.Set(Constants.BasketID, basket.Id);
            await UpdateCartBasket(basket);
            return basket;
        }

        public async Task<List<Category>> GetCategories()
        {
            var result = await GetAPI(Constants.APIEndpoints.Products.Types);
            List<Category> basket = JsonConvert.DeserializeObject<List<Category>>(result);
            return basket;
        }

        public async Task<List<ProductData>> GetPopularProducts()
        {
            ProductSpecParams parameters = new ProductSpecParams() {   };
            var result = await MakeCall(Url: Constants.APIEndpoints.Products.Products_, parameters: parameters, method: Method.GET);
            var data =  JsonConvert.DeserializeObject<ProductApiData>(result);
            return data.data.ToList();
        }

        public async Task<List<CartItem>> GetShoppingCartItems()
        {
            var basket = await GetCustomerBasket();
            return basket.Items;
        }

        public async Task<CartSubTotal> GetCartSubTotal()
        {
            var basket = await GetCustomerBasket();
            return new CartSubTotal() { subTotal = basket.Items.Sum(d => d.price) };
        }

        public async Task<bool> ClearShoppingCart()
        {
            var result = await DeleteAPI($"{Constants.APIEndpoints.APICart.Basket}?id={Preferences.Get(Constants.BasketID, string.Empty)}");
            return result;
        }

        public async Task<List<ProductData>> GetProducts(ProductSpecParams parameters)
        {
            var result = await MakeCall(Url: Constants.APIEndpoints.Products.Products_, parameters: parameters, method: Method.GET);
            var data = JsonConvert.DeserializeObject<ProductApiData>(result);
            return data.data.ToList();
        }

        public async Task<Product> GetProductById(int productId)
        {
            var result = await GetAPI($"{Constants.APIEndpoints.Products.Products_}/{productId}");
            Product product = JsonConvert.DeserializeObject<Product>(result);
            return product;
        }

        public async Task<bool> AddItemsToCart(CartItem cart)
        {
            //get the basket. 
            var basket = await GetCustomerBasket();

            int index = basket.Items.FindIndex(d => d.Id == cart.Id);
            if (index == -1) //Item does not exist
            {
                basket.Items.Add(cart);
            }
            else
            {
                basket.Items[index].Quantity += cart.Quantity;
            }

            return await UpdateCartBasket(basket);
        }

        private async Task<bool> UpdateCartBasket(CustomerBasket basket)
        {
            _ = PostAPI(Constants.APIEndpoints.APICart.Basket, basket, out bool IsSucessful);
            return IsSucessful;
        }

        public async Task<List<OrderByUser>> GetOrdersByUser()
        {
            var result = await GetAPI(Constants.APIEndpoints.Orders.Orders_);
            List<OrderByUser> orders = JsonConvert.DeserializeObject<List<OrderByUser>>(result);
            return orders;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            var result = await GetAPI($"{Constants.APIEndpoints.Orders.Orders_}/{orderId}");
            Order order = JsonConvert.DeserializeObject<Order>(result);
            return order;
        }

        public async Task<Order> PlaceOrder(OrderDto order)
        {
            string result = await PostAPI(Constants.APIEndpoints.Orders.Orders_, order, out bool IsSucessful);
            Order NewOrder = JsonConvert.DeserializeObject<Order>(result);
            return NewOrder;
        }

        public async Task<PaymentModel> ProcessPayFastPayment(OrderDto order)
        {
            string result = await PostAPI(Constants.APIEndpoints.Payments.PayFast, order, out bool IsSucessful);
            PaymentModel NewOrder = JsonConvert.DeserializeObject<PaymentModel>(result);
            return NewOrder;
        }
    }
}
