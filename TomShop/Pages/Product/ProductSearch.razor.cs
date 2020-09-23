using DynamicData;
using DynamicData.Binding;
using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using TomShop.Data;
using TomShop.Data.EntityDtos;

namespace TomShop.Pages.Product
{
    public partial class ProductSearch
    {
        [Inject]
        protected ProductService ProductService { get; set; }

        protected override Task OnInitializedAsync()
        {
            ViewModel = new ProductSearchViewModel(ProductService);
            _ = ViewModel.LoadKeywords.Execute().ToTask();
            return Task.CompletedTask;
        }

        private object SelectedSearchValue { get; set; }
    }

    public class ProductSearchViewModel : ReactiveObject
    {
        public ReactiveCommand<ProductSearchFilter, List<TProductEntityDto>> Load { get; }
        public List<TProductEntityDto> SearchResults { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, List<TProductEntityDto>> LoadKeywords { get; }
        public List<TProductEntityDto> LoadKeywordsResults { [ObservableAsProperty]get; }

        [Reactive] public string SearchKeyword { get; set; }

        public ReactiveCommand<TProductEntityDto, Unit> AddToCart { get; }
        [Reactive] public CartDto Cart { get; set; } = new CartDto();

        public ProductSearchViewModel(ProductService productService)
        {
            Console.WriteLine($"ProductSearchViewModel");

            Load = ReactiveCommand.CreateFromTask<ProductSearchFilter, List<TProductEntityDto>>(productService.SearchProduct);
            Load.ThrownExceptions.Subscribe(error => { });
            Load.ToPropertyEx(this, x => x.SearchResults, scheduler: RxApp.MainThreadScheduler);

            LoadKeywords = ReactiveCommand.CreateFromTask(productService.GetProductSearchWords);
            LoadKeywords.ThrownExceptions.Subscribe(error => { });
            LoadKeywords.ToPropertyEx(this, x => x.LoadKeywordsResults, scheduler: RxApp.MainThreadScheduler);

            this.WhenAnyValue(x => x.SearchKeyword).Throttle(TimeSpan.FromSeconds(0.5)).DistinctUntilChanged().Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new ProductSearchFilter { Keyword = x }).InvokeCommand(Load);

            AddToCart = ReactiveCommand.Create<TProductEntityDto>(x =>
            {
                Cart.Products.Add(new CartDetail(x.Id, x.NameFull, x.PriceSell, 1));
            });
            AddToCart.ThrownExceptions.Subscribe(error => { });

        }
    }

    public class CartDto : ReactiveObject
    {
        public ObservableCollection<CartDetail> Products { get; set; } = new ObservableCollection<CartDetail>();
        public long TotalAmount { [ObservableAsProperty]get; }
        [Reactive] public long DiscountAmount { get; set; }
        public long FinalAmount { [ObservableAsProperty]get; }

        public CartDto()
        {
            Products.ToObservableChangeSet().AutoRefresh(x => x.Total).ToCollection().Select(x => x.Sum(y => y.Total)).ToPropertyEx(this, x => x.TotalAmount, scheduler: RxApp.MainThreadScheduler);
            this.WhenAnyValue(x => x.TotalAmount, x => x.DiscountAmount).Publish().RefCount().Where(x => x.Item1 > 0 && x.Item2 >= 0 && x.Item1 >= x.Item2).Select(x => x.Item1 - x.Item2).ToPropertyEx(this, x => x.FinalAmount, scheduler: RxApp.MainThreadScheduler);
        }
    }

    public class CartDetail : ReactiveObject
    {
        public int ProductId { get; set; }
        public string NameFull { get; set; }
        public long PriceSell { get; set; }
        [Reactive] public int Quantity { get; set; }

        public long Total { [ObservableAsProperty]get; }

        public CartDetail(int productId, string nameFull, long priceSell, int quantity)
        {
            ProductId = productId;
            NameFull = nameFull;
            PriceSell = priceSell;
            Quantity = quantity;

            this.WhenAnyValue(x => x.Quantity).Publish().RefCount().Select(x => x * PriceSell).ToPropertyEx(this, x => x.Total, scheduler: RxApp.MainThreadScheduler);
        }
    }
}
