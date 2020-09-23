using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using TomShop.Data;

namespace TomShop.Shared
{
    public partial class NavMenu
    {
        [Inject]
        public CategoryService CategoryService { get; set; }

         private List<CategoryMenu> categoryList;
        protected override async Task OnInitializedAsync()
        {
            var data = (await CategoryService.GetCategory()).Select(x => new CategoryMenu
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId,
                Details = JsonConvert.DeserializeObject<CategoryExtraDetails>(x.ExtraDetails ?? string.Empty),
                Description = x.Description
            }).ToList();

            var childsHash = data.ToLookup(cat => cat.ParentId);
            foreach (var cat in data)
            {
                cat.Children = childsHash[cat.Id].ToList();
            }

            categoryList = data.Where(x=> !x.ParentId.HasValue).ToList();
        }
    }

    public class CategoryMenu
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public CategoryExtraDetails Details { get; set; }
        public string Description { get; set; }
        public List<CategoryMenu> Children { get; set; }
    }

    public class CategoryExtraDetails
    {
        public string Icon { get; set; }
        public string LinkTo { get; set; }
    }
}
