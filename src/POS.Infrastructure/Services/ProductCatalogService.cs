using Microsoft.EntityFrameworkCore;
using POS.Application.Abstractions;
using POS.Application.Models;
using POS.Core.Entities;
using POS.Infrastructure.Data;

namespace POS.Infrastructure.Services;

internal sealed class ProductCatalogService : IProductCatalogService
{
    private readonly IDbContextFactory<PosDbContext> _dbFactory;
    private readonly ICurrentSession _session;

    public ProductCatalogService(IDbContextFactory<PosDbContext> dbFactory, ICurrentSession session)
    {
        _dbFactory = dbFactory;
        _session = session;
    }

    public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync(CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var list = await db.Categories
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name)
            .Select(c => new CategoryDto(c.Id, c.Name))
            .ToListAsync(cancellationToken);
        return list;
    }

    public async Task<CategoryDto> CreateCategoryAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));

        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var now = DateTime.UtcNow;
        var entity = new Category
        {
            Id = Guid.NewGuid(),
            Name = name.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Categories.Add(entity);
        await db.SaveChangesAsync(cancellationToken);
        return new CategoryDto(entity.Id, entity.Name);
    }

    public async Task<IReadOnlyList<ProductListItemDto>> SearchProductsAsync(string? query, Guid? categoryId = null, CancellationToken cancellationToken = default)
    {
        var storeId = _session.StoreId;
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var q = db.Products
            .AsNoTracking()
            .Where(p => !p.IsDeleted && p.IsActive);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var term = query.Trim();
            q = q.Where(p => p.Name.Contains(term) || (p.Barcode != null && p.Barcode == term));
        }

        if (categoryId.HasValue)
            q = q.Where(p => p.CategoryId == categoryId.Value);

        var products = await q
            .OrderBy(p => p.Name)
            .Take(200)
            .ToListAsync(cancellationToken);

        if (products.Count == 0)
            return Array.Empty<ProductListItemDto>();

        var ids = products.Select(p => p.Id).ToList();
        var qtyByProduct = await db.Inventories
            .AsNoTracking()
            .Where(i => ids.Contains(i.ProductId) && i.StoreId == storeId && !i.IsDeleted)
            .ToDictionaryAsync(i => i.ProductId, i => i.Quantity, cancellationToken);

        return products
            .Select(p => new ProductListItemDto(
                p.Id,
                p.Name,
                p.Barcode,
                p.Price,
                qtyByProduct.GetValueOrDefault(p.Id),
                p.ImagePath))
            .ToList();
    }

    public async Task<ProductEditDto?> GetProductForEditAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var storeId = _session.StoreId;
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var p = await db.Products.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken);
        if (p is null)
            return null;

        var qty = await db.Inventories
            .AsNoTracking()
            .Where(i => i.ProductId == id && i.StoreId == storeId && !i.IsDeleted)
            .Select(i => (decimal?)i.Quantity)
            .FirstOrDefaultAsync(cancellationToken) ?? 0m;

        return new ProductEditDto
        {
            Id = p.Id,
            Name = p.Name,
            Barcode = p.Barcode,
            Price = p.Price,
            Cost = p.Cost,
            CategoryId = p.CategoryId,
            InitialStock = qty,
            ImagePath = p.ImagePath,
            IsActive = p.IsActive
        };
    }

    public async Task<ProductEditDto> CreateProductAsync(ProductEditDto input, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(input.Name))
            throw new ArgumentException("Product name is required.", nameof(input));

        var storeId = _session.StoreId;
        var now = DateTime.UtcNow;
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = input.Name.Trim(),
            Barcode = string.IsNullOrWhiteSpace(input.Barcode) ? null : input.Barcode.Trim(),
            Price = input.Price,
            Cost = input.Cost,
            CategoryId = input.CategoryId,
            IsWeighted = false,
            IsActive = input.IsActive,
            ImagePath = string.IsNullOrWhiteSpace(input.ImagePath) ? null : input.ImagePath.Trim(),
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Products.Add(product);

        var inv = new Inventory
        {
            Id = Guid.NewGuid(),
            ProductId = product.Id,
            StoreId = storeId,
            Quantity = input.InitialStock < 0 ? 0 : input.InitialStock,
            CreatedAt = now,
            UpdatedAt = now
        };
        db.Inventories.Add(inv);

        await db.SaveChangesAsync(cancellationToken);

        input.Id = product.Id;
        return input;
    }

    public async Task<ProductEditDto> UpdateProductAsync(ProductEditDto input, CancellationToken cancellationToken = default)
    {
        var storeId = _session.StoreId;
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == input.Id && !p.IsDeleted, cancellationToken)
            ?? throw new InvalidOperationException("Product not found.");

        product.Name = input.Name.Trim();
        product.Barcode = string.IsNullOrWhiteSpace(input.Barcode) ? null : input.Barcode.Trim();
        product.Price = input.Price;
        product.Cost = input.Cost;
        product.CategoryId = input.CategoryId;
        product.IsActive = input.IsActive;
        product.ImagePath = string.IsNullOrWhiteSpace(input.ImagePath) ? null : input.ImagePath.Trim();
        product.UpdatedAt = DateTime.UtcNow;

        var inv = await db.Inventories.FirstOrDefaultAsync(
            i => i.ProductId == product.Id && i.StoreId == storeId && !i.IsDeleted,
            cancellationToken);

        var now = DateTime.UtcNow;
        if (inv is null)
        {
            inv = new Inventory
            {
                Id = Guid.NewGuid(),
                ProductId = product.Id,
                StoreId = storeId,
                Quantity = input.InitialStock < 0 ? 0 : input.InitialStock,
                CreatedAt = now,
                UpdatedAt = now
            };
            db.Inventories.Add(inv);
        }
        else
        {
            inv.Quantity = input.InitialStock < 0 ? 0 : input.InitialStock;
            inv.UpdatedAt = now;
        }

        await db.SaveChangesAsync(cancellationToken);
        return input;
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var db = await _dbFactory.CreateDbContextAsync(cancellationToken);
        var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken)
            ?? throw new InvalidOperationException("Product not found.");

        product.IsDeleted = true;
        product.IsActive = false;
        product.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync(cancellationToken);
    }
}
