using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomShop.Data.Models;

namespace TomShop.Data
{
    public class OrderService
    {
        private readonly IDbContextFactory<TomShopContext> _contextFactory;
        public OrderService(IDbContextFactory<TomShopContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        //public async Task<List<TProductEntityDto>> Submit(CartSubmitDto model)
        //{
        //    using var db = _contextFactory.CreateDbContext();
        //    var cart = new TCart
        //    {
        //        Status = (int)CartStatus.Active,
        //    };
        //}
    }

    public class CartSubmitDto
    {
        public int Amount2 { get; set; }
        public List<CartItemSubmitDto> Items { get; set; }
    }

    public class CartItemSubmitDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public enum CartStatus
    {
        Active
    }
}
