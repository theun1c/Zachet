using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Zachet.Models;

namespace Zachet.ViewModels;

public class MainWindowViewModel
{
    private const string ConnectionString = "Host=edu.ngknn.ru;Port=5442;Database=41P_products;Username=21P;Password=123";

    public IReadOnlyList<ProductCardItem> Products { get; }

    public MainWindowViewModel()
    {
        Products = LoadProducts();
    }

    private static List<ProductCardItem> LoadProducts()
    {
        var options = new DbContextOptionsBuilder<_41pProductsContext>()
            .UseNpgsql(ConnectionString)
            .Options;

        using var context = new _41pProductsContext(options);

        return context.Products
            .Include(p => p.IdProductTypeNavigation)
            .Include(p => p.IdMaterialTypeNavigation)
            .Include(p => p.ProductWorkshops)
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Select(p => new ProductCardItem
            {
                ProductType = p.IdProductTypeNavigation.ProductType1,
                ProductName = p.Name,
                Article = $"Артикул: {p.Article}",
                MinPartnerPrice = $"Минимальная стоимость для партнера: {p.MinCostPartner}",
                MainMaterial = $"Основной материал: {p.IdMaterialTypeNavigation.MaterialType1}",
                ProductionTime = $"{Math.Round(p.ProductWorkshops.Sum(w => w.Time), 2):0.##} ч."
            })
            .ToList();
    }
}
