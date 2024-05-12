using ProductInventoryManagement.Entities;
using ProductInventoryManagement.Model;

namespace ProductInventoryManagement.Repositories
{
    public interface IProductInventoryManagementRepository
    {
        void AddProductToInventory(Product product);
        void DeleteProductFromInventory(int id);
        List<Product> GetAllProductsFromInventory();
        Product GetProductFromInventoryById(int id);
        void UpdateProductInsideInventory(Product product);
        IEnumerable<Product> GetProductsFromInventoryUsingPaging(ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters);
        IEnumerable<Product> GetProductsFromInventoryUsingFilterBrand(string brand, ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters);
        IEnumerable<Product> GetProductsFromInventoryUsingFilter(ProductInventoryMangementFilter filter, ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters);
    }
}
