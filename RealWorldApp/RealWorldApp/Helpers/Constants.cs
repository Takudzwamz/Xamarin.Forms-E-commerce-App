using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Helpers
{
    public class Constants
    {
        public const string AccessToken = "accessToken";
        public const string UserEmail = "useremail";
        public const string UserPassword = "password";
        public const string UserName = "userName";
        public const string TokenExpirationTime = "tokenExpirationTime";
       // public const string BasketID = "basket_id";
        public const string AddressStore = "addressStore";
        public const string CALLBACK_SCHEME = "myapp";



        public class Messaging
        {
            public const string UpdateCartCount = "UpdateCartCount";
        }

        public class APIEndpoints
        {
            public class APIAccounts
            {
                public const string emailexists = "Account/emailexists";
                public const string register = "Account/register";
                public const string login = "Account/login";

            }

            public class APICart
            {
                public const string Basket = "Basket";
            }

            public class Products
            {
                public const string Types = "Products/types";
                public const string Products_ = "products";
            }

            public class Orders
            {
                public const string Orders_ = "orders";
                public const string DeliveryMethod = "orders/deliveryMethods";
            }
            public class Payments
            {

                public const string PayFast = "payments/pay";
            }
        }
    }
}
