using Microsoft.EntityFrameworkCore;
using ProductInventoryManagement.Data;
using ProductInventoryManagement.Entities;
using ProductInventoryManagement.Model;
using System;
using System.Linq.Expressions;

namespace ProductInventoryManagement.Repositories
{
    public class ProductInventoryManagementRepository:IProductInventoryManagementRepository
    {
        private readonly ProductInventoryManagementDbContext _context;
        public ProductInventoryManagementRepository(ProductInventoryManagementDbContext context)
        {
            _context = context;
        }
        public List<Product> GetAllProductsFromInventory()
        {
            return _context.Products.ToList();
        }
        public Product GetProductFromInventoryById(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }
        public void AddProductToInventory(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        public void UpdateProductInsideInventory(Product product)
        {
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Entry(product).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void DeleteProductFromInventory(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }
            _context.Products.Remove(product);
            _context.SaveChanges();
        }

        public IQueryable<Product> FindAll()
        {
            return this._context.Set<Product>();
        }

        public IQueryable<Product> FindByCondition(Expression<Func<Product, bool>> expression)
        {
            return this._context.Set<Product>()
                .Where(expression)
                .AsNoTracking();
        }

        public IEnumerable<Product> GetProductsFromInventoryUsingPaging(ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            return ProductInventoryManagementPagedList<Product>.ToPagedList(FindAll().OrderBy(product => product.Id),
                  productInventoryManagementPagingParameters.PageNumber,
                  productInventoryManagementPagingParameters.PageSize);
        }

        public IEnumerable<Product> GetProductsFromInventoryUsingFilterBrand(string brand, ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            var products = FindByCondition(product => product.Brand == brand).OrderBy(on => on.Id);

            return ProductInventoryManagementPagedList<Product>.ToPagedList(products,
                  productInventoryManagementPagingParameters.PageNumber,
                  productInventoryManagementPagingParameters.PageSize);
        }

        public IEnumerable<Product> GetProductsFromInventoryUsingFilter(ProductInventoryMangementFilter filter, ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            var query = _context.Products.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(p => p.Name.Contains(filter.Name));
            }

            if (!string.IsNullOrEmpty(filter.Brand))
            {
                query = query.Where(p => p.Brand == filter.Brand);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(p => p.Price >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= filter.MaxPrice.Value);
            }

            return ProductInventoryManagementPagedList<Product>.ToPagedList(query,
                  productInventoryManagementPagingParameters.PageNumber,
                  productInventoryManagementPagingParameters.PageSize);
        }
    }
}
