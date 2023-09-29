﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Restaurant.Services.OrderAPI.Messages
{
    public class CartDetailsDto
    {
        public int CartDetailsId { get; set; }

        public int CartHeaderId { get; set; }
        
        public int ProductId { get; set; }
        public virtual ProductDto Product { get; set; }

        public int Count { get; set; }
    }
}