using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TomShop.Data.EntityDtos;
using TomShop.Data.Models;
using TomShop.Models;

namespace TomShop.Data
{
    public class ProductService
    {
        private readonly IDbContextFactory<TomShopContext> _contextFactory;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductService(IDbContextFactory<TomShopContext> contextFactory, IWebHostEnvironment hostingEnvironment)
        {
            _contextFactory = contextFactory;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<List<TProductEntityDto>> GetProductSearchWords()
        {
            using var db = _contextFactory.CreateDbContext();
            return await db.TProduct.AsNoTracking().Select(x => new TProductEntityDto
            {
                Id = x.Id,
                NameFull = x.NameFull,
                ImagePath = x.ImagePath
            }).ToListAsync();
        }

        public async Task<List<TProductEntityDto>> SearchProduct(ProductSearchFilter filter)
        {
            try
            {
                Console.WriteLine($"Debug {filter.Keyword}");
                using var db = _contextFactory.CreateDbContext();
                var query = db.TProduct.FromSqlInterpolated($"select a.* from TProduct as a INNER JOIN FREETEXTTABLE(TProduct, NameFull, {filter.Keyword}) as b ON a.Id = b.[KEY] ORDER BY b.RANK DESC OFFSET 0 ROWS ").AsNoTracking();
                return await query.Select(x => new TProductEntityDto
                {
                    Id = x.Id,
                    NameFull = x.NameFull,
                    ImagePath = x.ImagePath,
                    PriceSell = x.PriceSell,
                    Quantity = x.Quantity,
                }).ToListAsync();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return new List<TProductEntityDto>();
            }
        }

        public static string RemoveAccents(string input)
        {
            return new string(input
                .Normalize(System.Text.NormalizationForm.FormKD)
                .ToCharArray()
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());
        }

        public async Task Save(NewProductViewModel model)
        {
            Console.WriteLine(model.Description);

            var path = await SaveImageToDisk(model.ImageContent);
            using var db = _contextFactory.CreateDbContext();
            db.TProduct.Add(new TProduct
            {
                NameFull = model.NameFull,
                CategoryId = model.CategoryId,
                Description = model.Description,
                CreatedTime = DateTimeOffset.Now,
                Location = model.Location,
                PriceBuy = model.PriceBuy,
                PriceSell = model.PriceSell,
                Quantity = model.Quantity,
                ImagePath = path
            });
            await db.SaveChangesAsync();
        }

        public async Task<string> SaveImageToDisk(string base64)
        {
            var folderPath = Path.Combine(_hostingEnvironment.ContentRootPath, @"..\Images");
            Directory.CreateDirectory(folderPath);

            var fileName = $"{Guid.NewGuid()}.jpg";
            var filePath = Path.Combine(folderPath, fileName);
            using MemoryStream ms = new MemoryStream(Convert.FromBase64String(base64));
            using Image pic = Image.FromStream(ms);
            pic.Save(filePath, ImageFormat.Png);

            return filePath;
        }
    }

    public class ProductSearchFilter 
    {
        public string Keyword { get; set; }
        public int? Id { get; set; }
    }
}
