using System;
using System.Collections.Generic;
using System.Text;

namespace RealWorldApp.Helpers
{
    public class Constants
    {
        public const string AccessToken = "accessToken";
        public const string UserId = "userId";
        public const string UserName = "userName";
        public const string TokenExpirationTime = "tokenExpirationTime";
        public const string BasketID = "basket_id";
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
                public const string Types = "types";
                public const string Products_ = "products";
            }

            public class Orders
            {
                public const string Orders_ = "orders";
            }
            public class Payments
            {

                public const string PayFast = "payments/pay";
            }
        }
    }
}
