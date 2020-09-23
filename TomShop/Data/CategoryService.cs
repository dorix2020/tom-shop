using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using TomShop.Data.Dtos;
using TomShop.Data.Models;

namespace TomShop.Data
{
    public class CategoryService
    {
        private readonly IDbContextFactory<TomShopContext> _contextFactory;
        public CategoryService(IDbContextFactory<TomShopContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<List<TCategoryEntityDto>> GetCategory()
        {
            using var db = _contextFactory.CreateDbContext();
            return await db.TCategory.AsNoTracking().Select(x => new TCategoryEntityDto
            {
                Id = x.Id,
                Description = x.Description,
                ExtraDetails = x.ExtraDetails,
                HasChildren = x.HasChildren,
                ImagePath = x.ImagePath,
                Name = x.Name,
                ParentId = x.ParentId
            }).ToListAsync();
        }

        public async Task<List<SelectItem<int>>> GetCategorySelectList()
        {
            var data = (await GetCategory()).Select(x => new SelectItem<int>
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId
            }).ToList();

            var childsHash = data.ToLookup(cat => cat.ParentId);
            foreach (var cat in data)
            {
                cat.Items = childsHash[cat.Id].ToList();
            }
            return data.Where(x => !x.ParentId.HasValue).ToList();
        }
    }

    public class SelectItem<T> where T : struct
    {
        public T Id { get; set; }
        public Nullable<T> ParentId { get; set; }
        public string Name { get; set; }
        public List<SelectItem<T>> Items { get; set; } = new List<SelectItem<T>>();
    }

}
