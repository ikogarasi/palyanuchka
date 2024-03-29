﻿namespace RestaurantWeb
{
    public static class SD
    {
        public static string ProductAPIBase { get; set; }
        public static string AzureBlobAPIBase { get; set; }
        public static string IdentityAPIBase { get; set; }
        public static string ShoppingCartAPIBase { get; set; }
        public static string CouponAPIBase { get; set; }
        public enum ApiType
        {
            GET, POST, PUT, DELETE
        }
    }
}
