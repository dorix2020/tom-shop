using Microsoft.AspNetCore.Components;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Threading.Tasks;
using System.Threading.Tasks;
using TomShop.Data;
using TomShop.Data.Dtos;
using TomShop.Models;

namespace TomShop.Pages.Product
{
    public partial class ProductImport
    {
        [Inject] protected CategoryService CategoryService { get; set; }
        [Inject] protected ProductService ProductService { get; set; }

        protected override Task OnInitializedAsync()
        {
            ViewModel = new ProductImportViewModel(CategoryService, ProductService);
            _ = ViewModel.LoadCategory.Execute().ToTask();
            return Task.CompletedTask;
        }
    }

    public class ProductImportViewModel : ReactiveObject
    {
        public NewProductViewModel NewProduct { get; set; } = new NewProductViewModel();

        public ReactiveCommand<Unit, List<SelectItem<int>>> LoadCategory { get; }
        public List<SelectItem<int>> LoadCategoryResults { [ObservableAsProperty]get; }

        public ReactiveCommand<Unit, Unit> Save { get; }
        public ProductImportViewModel(CategoryService categoryService, ProductService productService)
        {
            LoadCategory = ReactiveCommand.CreateFromTask(categoryService.GetCategorySelectList);
            LoadCategory.ThrownExceptions.Subscribe(error => { });
            LoadCategory.ToPropertyEx(this, x => x.LoadCategoryResults, scheduler: RxApp.MainThreadScheduler);


            Save = ReactiveCommand.CreateFromTask(x => productService.Save(NewProduct));
            Save.ThrownExceptions.Subscribe(error => { });
            Save.Subscribe(x => { NewProduct = new NewProductViewModel(); });
        }
    }
}
