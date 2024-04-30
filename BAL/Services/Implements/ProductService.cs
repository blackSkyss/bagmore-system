using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class ProductService : IProductService
    {

        Regex regexLetter = new Regex(@"[a-zA-Z]");
        Regex regexCharacter = new Regex(@"[\\~!@#$%^&*()_+={}\[\]|\\;:'""<>,.?/-]");
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }
        #region Enum
        enum SortName
        {
            NAMEASC,
            NAMEDESC,
        }
        enum SortDiscount
        {
            DISCOUNTASC,
            DISCOUNTDESC,
        }

        enum SortTTProduct
        {
            TTPRODUCTASC,
            TTPRODUCTDESC,
        }
        enum SortStatus
        {
            STATUSASC,
            STATUSDESC,
        }

        enum SortOriginalPrice
        {
            ORIGINALPRICEASC,
            ORIGINALPRICEDESC,
        }


        #endregion

        public async Task<ProductDetailViewModel> GetProductInformation(int productId)
        {
            if (regexCharacter.IsMatch(productId.ToString()))
            {
                throw new Exception("You need input number, not characters");
            }
            if (regexLetter.IsMatch(productId.ToString()))
            {
                throw new Exception("You need input number, not letters");
            }
            Product checkProductId = await ((ProductRepository)_unitOfWork.ProductRepository).GetByID(productId);
            if (checkProductId == null)
            {
                throw new Exception("product id does not exists");
            }
            Product pro = await _unitOfWork.ProductRepository.GetProductDetail(productId);
            ProductDetailViewModel viewModel = new ProductDetailViewModel();
            _mapper.Map(pro, viewModel);
            return viewModel;
        }

        public async Task<ProductViewModelAdmin> GetProductDetailAsyncAdmin(int productId)
        {
            if (regexCharacter.IsMatch(productId.ToString()))
            {
                throw new Exception("You need input number, not characters");
            }
            if (regexLetter.IsMatch(productId.ToString()))
            {
                throw new Exception("You need input number, not letters");
            }

            Product checkProductId = await ((ProductRepository)_unitOfWork.ProductRepository).GetByID(productId);
            if (checkProductId == null)
            {
                throw new Exception("product id does not exists");
            }

            Product pro = await _unitOfWork.ProductRepository.GetProductDetailAsyncAdmin(productId);
            ProductViewModelAdmin viewModel = new ProductViewModelAdmin();
            _mapper.Map(pro, viewModel);
            return viewModel;
        }
        public async Task<bool> CreateProductAsync(CreatedProductViewModel productViewModel)
        {
            string message = "";
            Product product = new Product();
            try
            {
                _mapper.Map(productViewModel, product);
                Category checkCategoryId = await ((CategoryRepository)_unitOfWork.CategoryRepository).GetByID(product.CategoryId);
                Brand checkBrandId = await ((BrandRepository)_unitOfWork.BrandRepository).GetByID(product.BrandId);

                #region check exist id
                foreach (var DescribeProduct in product.DescribeProducts)
                {
                    Size checkSizeId = await ((SizeRepository)_unitOfWork.SizeRepository).GetByID(DescribeProduct.SizeId);
                    if (checkSizeId == null)
                    {
                        message += $"|| Size id = {DescribeProduct.SizeId} doesn't exist";
                        continue;
                    }
                    DescribeProduct.SizeId = checkSizeId.Id;

                }

                foreach (var DescribeProduct in product.DescribeProducts)
                {
                    Color checkColorId = await ((ColorRepository)_unitOfWork.ColorRepository).GetByID(DescribeProduct.ColorId);
                    if (checkColorId == null)
                    {
                        message += $"|| Color id = {DescribeProduct.ColorId} doesn't exist";
                        continue;
                    }
                    DescribeProduct.ColorId = checkColorId.Id;
                }
                

                foreach (var suppliedProduct in product.SuppliedProducts)
                {
                    Supplier checkSupplierProductId = await ((SupplierRepository)_unitOfWork.SupplierRepository).GetByID(suppliedProduct.SupplierId);
                    if (checkSupplierProductId == null)
                    {
                        message += $"|| Supplier id = {suppliedProduct.SupplierId} doesn't exist";
                        continue;
                    }

                    suppliedProduct.Supplier = checkSupplierProductId;

                }


                if (checkBrandId == null)
                {
                    message += $"|| Brand id = {product.BrandId} doesn't exist";
                }
                else if (checkCategoryId == null)
                {
                    message += $"|| Category id = {product.CategoryId} doesn't exist";
                }

                if (message != "")
                {
                    throw new Exception(message);
                }

                #endregion

                #region count amount
                product.TotalProduct = product.DescribeProducts.Sum(d => d.Amount);
                product.SuppliedProducts.Select(s => s.Amount == product.DescribeProducts.Sum(d => d.Amount));
                product.Status = (int)ProductEnum.Status.STOCKING;
                //foreach(var suppliedProduct in product.SuppliedProducts)
                //{
                //    suppliedProduct.Amount = product.DescribeProducts.Sum(d => d.Amount);

                //}
                #endregion

                

                await ((ProductRepository)_unitOfWork.ProductRepository).Insert(product);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<bool> UpdateProductAsync(int productId, CreatedProductViewModel productViewModel)
        {
            string message = "";

            try
            {

                Product checkProductId = await ((ProductRepository)_unitOfWork.ProductRepository).GetByID(productId);
                if (checkProductId == null)
                {
                    throw new Exception("productId does not exist");
                }

                _mapper.Map(productViewModel, checkProductId);

                Category checkCategoryId = await ((CategoryRepository)_unitOfWork.CategoryRepository).GetByID(checkProductId.CategoryId);
                Brand checkBrandId = await ((BrandRepository)_unitOfWork.BrandRepository).GetByID(checkProductId.BrandId);

                #region check exist id
                foreach (var DescribeProduct in checkProductId.DescribeProducts)
                {
                    Size checkSizeId = await ((SizeRepository)_unitOfWork.SizeRepository).GetByID(DescribeProduct.SizeId);
                    if (checkSizeId == null)
                    {
                        message += $"|| Size id = {DescribeProduct.SizeId} doesn't exist";
                        continue;
                    }
                    DescribeProduct.SizeId = checkSizeId.Id;

                }

                foreach (var DescribeProduct in checkProductId.DescribeProducts)
                {
                    Color checkColorId = await ((ColorRepository)_unitOfWork.ColorRepository).GetByID(DescribeProduct.ColorId);
                    if (checkColorId == null)
                    {
                        message += $"|| Color id = {DescribeProduct.ColorId} doesn't exist";
                        continue;
                    }
                    DescribeProduct.ColorId = checkColorId.Id;
                }

                foreach (var suppliedProduct in checkProductId.SuppliedProducts)
                {
                    Supplier checkSupplierProductId = await ((SupplierRepository)_unitOfWork.SupplierRepository).GetByID(suppliedProduct.Supplier.Id);
                    if (checkSupplierProductId == null)
                    {
                        message += $"|| Supplier id = {suppliedProduct.Supplier.Id} doesn't exist";
                        continue;
                    }

                    suppliedProduct.Supplier = checkSupplierProductId;

                }


                if (checkBrandId == null)
                {
                    message += $"|| Brand id = {checkProductId.BrandId} doesn't exist";
                }
                else if (checkCategoryId == null)
                {
                    message += $"|| Category id = {checkProductId.CategoryId} doesn't exist";
                }

                if (message != "")
                {
                    throw new Exception(message);
                }

                #endregion



                #region count amount
                checkProductId.TotalProduct = checkProductId.DescribeProducts.Sum(d => d.Amount);
                checkProductId.SuppliedProducts.Select(s => s.Amount == checkProductId.DescribeProducts.Sum(d => d.Amount));
                //foreach(var suppliedProduct in product.SuppliedProducts)
                //{
                //    suppliedProduct.Amount = product.DescribeProducts.Sum(d => d.Amount);

                //}
                #endregion

                ((ProductRepository)_unitOfWork.ProductRepository).Update(checkProductId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<ProductViewModelAdmin>> GetAllProductsAsyncAdmin()
        {
            List<ProductViewModelAdmin> productsAdmin = new List<ProductViewModelAdmin>();
            List<Product> products = await ((ProductRepository)_unitOfWork.ProductRepository).GetAllProductsAsyncAdmin();
            if (products.Count() == 0)
            {
                throw new Exception("No products exist");
            }
            _mapper.Map(products, productsAdmin);
            return productsAdmin;
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {

                Product product = await ((ProductRepository)_unitOfWork.ProductRepository).GetByID(productId);
                if (product == null)
                {
                    throw new Exception("Id does not exist");
                }
                List<CartProduct> cartProducts = (List<CartProduct>)await ((CartProductRepository)_unitOfWork.CartProductRepository).Get();
                cartProducts = cartProducts.Where(c => c.ProductId == productId).ToList();
                product.Status = 0;
                ((ProductRepository)_unitOfWork.ProductRepository).Update(product);
                await _unitOfWork.SaveChangesAsync();
                foreach (CartProduct cartProduct in cartProducts)
                {
                    cartProduct.Status = 0;
                    ((CartProductRepository)_unitOfWork.CartProductRepository).Update(cartProduct);
                    await _unitOfWork.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Get list products
        public async Task<Tuple<List<ProductViewModelAdmin>, int, int?, int?>> GetProductsAsync(string? keySearchName,
                                                                string? keySortStatus, string? keySortOriginalPrice, string? keySortName,
                                                                string? keySortDiscount, string? keySortTTProduct,
                                                                int? PAGE_NUMBER, int? PAGE_SIZE)
        {
            try
            {
                List<ProductViewModelAdmin> productViewModelAdmins = new List<ProductViewModelAdmin>();
                IEnumerable<Product> products = new List<Product>();
                products = await ((ProductRepository)_unitOfWork.ProductRepository).GetProductSearchAsyncAdmin();

                foreach (var pro in products)
                {
                    ProductViewModelAdmin productViewModelAdmin = new ProductViewModelAdmin();
                    foreach (var product in pro.DescribeProducts!)
                    {
                        productViewModelAdmin.OriginalPrice = product.OriginalPrice.Value;
                        // _mapper.Map(product, productViewModelAdmin);
                        productViewModelAdmins.Add(productViewModelAdmin);
                    }
                    
                }
                _mapper.Map(products, productViewModelAdmins);


                #region Validation page
                string errormessage = "";

                if (PAGE_SIZE == null)
                {
                    PAGE_SIZE = 10;
                }

                if (PAGE_NUMBER == null)
                {
                    PAGE_NUMBER = 1;
                }

                if (errormessage != "")
                {
                    throw new Exception(errormessage);
                }
                #endregion

                #region Search
                if ( keySearchName != "" && keySearchName != null)
                {
                    productViewModelAdmins = productViewModelAdmins.Where(c => c.Name.ToLower().Contains(keySearchName.Trim().ToLower())).ToList();
                }
                #endregion

                if (string.IsNullOrWhiteSpace(keySortOriginalPrice) == false)
                {
                    switch (keySortOriginalPrice)
                    {
                        case nameof(SortOriginalPrice.ORIGINALPRICEASC):
                            productViewModelAdmins = productViewModelAdmins.OrderBy(x => x.OriginalPrice).ToList();
                            break;
                        case nameof(SortOriginalPrice.ORIGINALPRICEDESC):
                            productViewModelAdmins = productViewModelAdmins.OrderByDescending(x => x.OriginalPrice).ToList();
                            break;
                    }
                }

                #region SortName
                if (keySortName != "" && keySortName != null)
                {
                    if (Enum.IsDefined(typeof(SortName), keySortName))
                    {
                        switch (keySortName)
                        {
                            case nameof(SortName.NAMEASC):
                                productViewModelAdmins = productViewModelAdmins.OrderBy(c => c.Name).ToList();
                                break;
                            case nameof(SortName.NAMEDESC):
                                productViewModelAdmins = productViewModelAdmins.OrderByDescending(c => c.Name).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }
                }
                #endregion

                #region Sort Discount
                if (keySortDiscount != "" && keySortDiscount != null )
                {
                    if (Enum.IsDefined(typeof(SortDiscount), keySortDiscount))
                    {
                        switch (keySortDiscount)
                        {
                            case nameof(SortDiscount.DISCOUNTASC):
                                productViewModelAdmins = productViewModelAdmins.OrderBy(c => c.Discount).ToList();
                                break;
                            case nameof(SortDiscount.DISCOUNTDESC):
                                productViewModelAdmins = productViewModelAdmins.OrderByDescending(c => c.Discount).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }
                }
                #endregion

                #region SortStatus

                if (keySortStatus != "" && keySortStatus != null )
                {
                    if (Enum.IsDefined(typeof(SortStatus), keySortStatus))
                    {
                        switch (keySortStatus)
                        {
                            case nameof(SortStatus.STATUSASC):
                                productViewModelAdmins = productViewModelAdmins.OrderBy(c => c.Status).ToList();
                                break;
                            case nameof(SortStatus.STATUSDESC):
                                productViewModelAdmins = productViewModelAdmins.OrderByDescending(c => c.Status).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }
                }
                #endregion

                #region
                if (keySortTTProduct != "" && keySortTTProduct != null)
                {
                    if (Enum.IsDefined(typeof(SortTTProduct), keySortTTProduct))
                    {
                        switch (keySortTTProduct)
                        {
                            case nameof(SortTTProduct.TTPRODUCTASC):
                                productViewModelAdmins = productViewModelAdmins.OrderBy(c => c.TotalProduct).ToList();
                                break;
                            case nameof(SortTTProduct.TTPRODUCTDESC):
                                productViewModelAdmins = productViewModelAdmins.OrderByDescending(c => c.TotalProduct).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }
                }
                #endregion

            #region Count total page
            int totalRecords = productViewModelAdmins.Count();
            int totalPages = (int)Math.Ceiling((double)((double)totalRecords / PAGE_SIZE));
                #endregion

                #region Paging
                productViewModelAdmins = productViewModelAdmins.Skip((int)((PAGE_NUMBER - 1) * PAGE_SIZE)).Take((int)PAGE_SIZE).ToList();
            #endregion

            return Tuple.Create(productViewModelAdmins, totalPages, PAGE_NUMBER, PAGE_SIZE);

        }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
    }

}
#endregion

#region ImageToBase64
public static async Task<byte[]> ImageToBase64(IFormFile image)
{
    using (var ms = new MemoryStream())
    {
        await image.CopyToAsync(ms);
        var fileBytes = ms.ToArray();
        return fileBytes;
    }
}
        #endregion
        enum Sort
        {
            NAMEASC,
            NAMEDESC,
            PRICEASC,
            PRICEDESC
        }

        #region Get Products
        public async Task<List<ProductViewModel>> GetProductsAsync(string? sortby,
                                                       List<int>? filterbycategory,
                                                       List<int>? filterbycolor,
                                                       List<int>? filterbysize)
        {
            try
            {
                List<ProductViewModel> productviews = new List<ProductViewModel>();
                IEnumerable<Product> products = new List<Product>();
                products = await ((ProductRepository)_unitOfWork.ProductRepository).Get(includeProperties: "DescribeProducts,ProductImages");

                #region Default
                if (sortby == null && filterbycategory == null && filterbycolor == null && filterbysize == null)
                {
                    foreach (var product in products)
                    {
                        if (product.Status == (int)ProductEnum.Status.SALE || product.Status == (int)ProductEnum.Status.STOCKING)
                        {
                            var productView = _mapper.Map<ProductViewModel>(product);
                            productView.MinPrice = product.DescribeProducts.Min(d => d.Price);
                            productView.MaxPrice = product.DescribeProducts.Max(d => d.Price);
                            productView.source = product.ProductImages.FirstOrDefault(i => i.ProductId == product.Id)!.Source;
                            productviews.Add(productView);
                        }
                    }
                    return productviews;
                }
                #endregion

                #region Sort
                if (sortby != null)
                {
                    if (Enum.IsDefined(typeof(Sort), sortby))
                    {
                        switch (sortby)
                        {
                            case nameof(Sort.NAMEASC):
                                products = products.OrderBy(p => p.Name).ToList();
                                break;
                            case nameof(Sort.NAMEDESC):
                                products = products.OrderByDescending(p => p.Name).ToList();
                                break;
                            case nameof(Sort.PRICEASC):
                                products = products.OrderBy(p => p.DescribeProducts.Min(d => d.Price));
                                break;
                            case nameof(Sort.PRICEDESC):
                                products = products.OrderByDescending(p => p.DescribeProducts.Max(d => d.Price));
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }

                }
                #endregion

                #region Filter by category
                if (filterbycategory != null && filterbycolor == null && filterbysize == null)
                {
                    products = products.Where(p => filterbycategory
                                                   .Contains(p.CategoryId.Value));
                }
                #endregion

                #region Filter by color
                if (filterbycategory == null && filterbycolor != null && filterbysize == null)
                {
                    products = products.Where(p => p.DescribeProducts
                                                   .Any(d => filterbycolor
                                                   .Contains(d.ColorId.Value)));
                }
                #endregion

                #region Filter by size
                if (filterbycategory == null && filterbycolor == null && filterbysize != null)
                {
                    products = products.Where(p => p.DescribeProducts
                                                   .Any(d => filterbysize
                                                   .Contains(d.SizeId.Value)));
                }
                #endregion

                #region Filter by category and color
                if (filterbycategory != null && filterbycolor != null && filterbysize == null)
                {
                    products = products.Where(p => filterbycategory
                                                   .Contains(p.CategoryId.Value)
                                                   || p.DescribeProducts
                                                   .Any(d => filterbycolor
                                                   .Contains(d.ColorId.Value)));
                }
                #endregion

                #region Filter by category and size
                if (filterbycategory != null && filterbycolor == null && filterbysize != null)
                {
                    products = products.Where(p => filterbycategory
                                                   .Contains(p.CategoryId.Value)
                                                   || p.DescribeProducts
                                                   .Any(d => filterbysize
                                                   .Contains(d.SizeId.Value)));
                }
                #endregion

                #region Filter by color and size
                if (filterbycategory == null && filterbycolor != null && filterbysize != null)
                {
                    products = products.Where(p => p.DescribeProducts
                                                   .Any(d => filterbycolor
                                                   .Contains(d.ColorId.Value))
                                                   || p.DescribeProducts
                                                   .Any(d => filterbysize
                                                   .Contains(d.SizeId.Value)));
                }
                #endregion

                #region Filter by category, color and size
                if (filterbycategory != null && filterbycolor != null && filterbysize != null)
                {
                    products = products.Where(p => filterbycategory
                                                   .Contains(p.CategoryId.Value)
                                                   || p.DescribeProducts
                                                   .Any(d => filterbycolor
                                                   .Contains(d.ColorId.Value)
                                                   || p.DescribeProducts
                                                   .Any(d => filterbysize
                                                   .Contains(d.SizeId.Value))));
                }
                #endregion

                foreach (var product in products)
                {
                    if (product.Status == (int)ProductEnum.Status.SALE || product.Status == (int)ProductEnum.Status.STOCKING)
                    {
                        var productView = _mapper.Map<ProductViewModel>(product);
                        productView.MinPrice = product.DescribeProducts.Min(d => d.Price);
                        productView.MaxPrice = product.DescribeProducts.Max(d => d.Price);
                        productView.source = product.ProductImages.FirstOrDefault(i => i.ProductId == product.Id)!.Source;
                        productviews.Add(productView);
                    }
                }
                return productviews;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
