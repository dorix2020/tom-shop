using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TomShop.Data.Models;

namespace TomShop.Data
{
    public class CartService
    {
        private readonly IDbContextFactory<TomShopContext> _contextFactory;
        public CartService(IDbContextFactory<TomShopContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }


    }
}
