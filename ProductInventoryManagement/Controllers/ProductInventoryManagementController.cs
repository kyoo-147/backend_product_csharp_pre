using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductInventoryManagement.Entities;
using ProductInventoryManagement.Exceptions;
using ProductInventoryManagement.Model;
using ProductInventoryManagement.Repositories;

namespace ProductInventoryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductInventoryManagementController : ControllerBase
    {
        private readonly IProductInventoryManagementRepository _productInventoryManagementRepository;
        private readonly ILogger<ProductInventoryManagementController> _logger;
        string NotFoundError = "Lỗi không tìm thấy: Không tìm thấy sản phẩm có Id đã cho trong kho.";
        string NullObjecError = "Lỗi đối tượng rỗng: Sản phẩm không thể rỗng";
        string InvalidIdError = "Lỗi Id không hợp lệ: Id được cung cấp không khớp với Id của sản phẩm";
        string DuplicationError = "Lỗi trùng lặp: Sản phẩm có cùng tên và nhãn hiệu đã tồn tại trong kho.";
        public ProductInventoryManagementController(IProductInventoryManagementRepository productInventoryManagementRepository, ILogger<ProductInventoryManagementController> logger)
        {
            _productInventoryManagementRepository = productInventoryManagementRepository;
            _logger = logger;
        }

        /// <summary>
        /// Lấy tất cả các sản phẩm từ kho.
        /// </summary>
        /// <returns></returns>
        [Route("GetAllProductsFromInventory")]
        [HttpGet]
        public IActionResult GetAllProductsFromInventory()
        {
            var products = _productInventoryManagementRepository.GetAllProductsFromInventory();
            return Ok(products);
        }

        /// <summary>
        /// Nhận sản phẩm cho Id sản phẩm nhất định.
        /// </summary>
        /// <param name="id">Id cho sản phẩm</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Lỗi Not Found: Không tìm thấy sản phẩm trong kho.</exception>
        [Route("GetProductFromInventoryById")]
        [HttpGet]
        public IActionResult GetProductFromInventoryById([FromQuery]int id)
        {
            var product = _productInventoryManagementRepository.GetProductFromInventoryById(id);
            if (product == null)
            {
                _logger.LogError(NotFoundError);
                throw new NotFoundException(NotFoundError);
            }
            return Ok(product);
        }

        /// <summary>
        /// Lấy sản phẩm từ kho bằng cách phân trang
        /// </summary>
        /// <param name="productInventoryManagementPagingParameters">Lỗi tham số rỗng: Tham số bộ lọc hoặc phân trang không thể rỗng</param>
        /// <returns></returns>
        [Route("GetProductsFromInventoryUsingPaging")]
        [HttpGet]
        public IActionResult GetProductsFromInventoryUsingPaging([FromQuery] ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            if (productInventoryManagementPagingParameters == null)
            {
                _logger.LogError(NullParameterError);
                throw new ValidationException(NullParameterError);
            }
            var products = _productInventoryManagementRepository.GetProductsFromInventoryUsingPaging(productInventoryManagementPagingParameters);
            return Ok(products);
        }

       
        /// <summary>
        /// Nhận danh sách các sản phẩm dựa trên các thông số trang và bộ lọc nhất định
        /// </summary>
        /// <param name="productInventoryMangementFilter"></param>
        /// <param name="productInventoryManagementPagingParameters"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException">Lỗi tham số Null: Tham số bộ lọc hoặc phân trang không thể rỗng</exception>
        [Route("GetProductsFromInventoryUsingFilter")]
        [HttpGet]
        public IActionResult GetProductsFromInventoryUsingFilter([FromQuery] ProductInventoryMangementFilter productInventoryMangementFilter, [FromQuery] ProductInventoryManagementPagingParameters productInventoryManagementPagingParameters)
        {
            if (productInventoryMangementFilter == null || productInventoryManagementPagingParameters == null)
            {
                _logger.LogError(NullParameterError);
                throw new ValidationException(NullParameterError);
            }
            var products = _productInventoryManagementRepository.GetProductsFromInventoryUsingFilter(productInventoryMangementFilter, productInventoryManagementPagingParameters);
            return Ok(products);
        }

        /// <summary>
        /// Thêm sản phẩm vào kho. Tên và nhãn hiệu sản phẩm phải là duy nhất.
        /// </summary>
        /// <param name="product">Đối tượng có Id, Tên, Thương hiệu và Giá</param>
        /// <returns></returns>
        /// <exception cref="ValidationException">1) Lỗi trùng lặp: Sản phẩm có tên và nhãn hiệu đã tồn tại trong kho.
        /// 2) Lỗi đối tượng rỗng: Sản phẩm không thể rỗng </exception>
        [Route("AddProductToInventory")]
        [HttpPost]
        public IActionResult AddProductToInventory([FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError(NullObjecError);
                throw new ValidationException(NullObjecError);
            }
            var existingProducts = _productInventoryManagementRepository.GetAllProductsFromInventory();
            foreach(var existingProduct in existingProducts)
            {
                if(string.Equals(existingProduct.Name,product.Name,StringComparison.OrdinalIgnoreCase) && 
                    string.Equals(existingProduct.Brand,product.Brand,StringComparison.OrdinalIgnoreCase)) {
                    _logger.LogError(DuplicationError);
                    throw new ValidationException(DuplicationError);
                }
            }
            _productInventoryManagementRepository.AddProductToInventory(product);
            return CreatedAtAction(nameof(GetProductFromInventoryById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Cập nhật/Sửa đổi thuộc tính sản phẩm
        /// </summary>
        /// <param name="id">Id cho sản phẩm</param>
        /// <param name="product">Đối tượng có Id, Tên, Thương hiệu và Giá</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"> 1) Lỗi đối tượng rỗng: Sản phẩm không thể rỗng
        /// 2) Lỗi Id không hợp lệ: Id sản phẩm không khớp.</exception>
        /// <exception cref="NotFoundException">Lỗi Not Found: Không tìm thấy sản phẩm trong kho.</exception>
        [Route("UpdateProductInsideInventory")]
        [HttpPut]
        public IActionResult UpdateProductInsideInventory([FromQuery]int id, [FromBody] Product product)
        {
            if (product == null)
            {
                _logger.LogError(NullObjecError);
                throw new ValidationException(NullObjecError);
            }
            if(id!=product.Id)
            {
                _logger.LogError(InvalidIdError);
                throw new ValidationException(InvalidIdError);
            }
            var existingProduct = _productInventoryManagementRepository.GetProductFromInventoryById(id);
            if (existingProduct == null)
            {
                _logger.LogError(NotFoundError);
                throw new NotFoundException(NotFoundError);
            }
            _productInventoryManagementRepository.UpdateProductInsideInventory(product);
            return NoContent();
        }

        /// <summary>
        /// Xóa sản phẩm khỏi kho.
        /// </summary>
        /// <param name="id">Id cho sản phẩm</param>
        /// <returns></returns>
        /// <exception cref="NotFoundException">Lỗi Not Found: Không tìm thấy sản phẩm trong kho.</exception>
        [Route("DeleteProductFromInventory")]
        [HttpDelete]
        public IActionResult DeleteProductFromInventory([FromQuery] int id)
        {
            var existingProduct = _productInventoryManagementRepository.GetProductFromInventoryById(id);
            if (existingProduct == null)
            {
                _logger.LogError(NotFoundError);
                throw new NotFoundException(NotFoundError);
            }
            _productInventoryManagementRepository.DeleteProductFromInventory(id);
            return NoContent();
        }
    }
}
