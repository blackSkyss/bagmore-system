using AutoMapper;
using BAL.Helpers;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class BrandService : IBrandService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }
        enum SortName
        {
            NAMEASC,
            NAMEDESC,

        }
        enum SortStatus
        {
            STATUSASC,
            STATUSDESC,
        }
        enum SortTTProduct
        {
            TTPRODUCTASC,
            TTPRODUCTDESC,
        }


        public async Task<bool> CreateBrand(BrandCreatedViewModel brandViewModel)
        {
            try
            {
                Brand brand = new Brand();
                _mapper.Map(brandViewModel, brand);
                brand.Status = (int)BrandEnum.Status.ACTIVE;
                Brand existedBrand = (await((BrandRepository)this._unitOfWork.BrandRepository).Get(x => x.Name.ToLower().Equals(brand.Name.ToLower()))).FirstOrDefault();
                if (existedBrand != null)
                {
                    throw new Exception("Brand name existed!");
                }
                await ((BrandRepository)_unitOfWork.BrandRepository).Insert(brand);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<bool> DeleteBrand(int brandId)
        {
            try
            {
                Brand checkBrandId = await ((BrandRepository)_unitOfWork.BrandRepository).GetByID(brandId);
                List<Product> products = await ((ProductRepository)_unitOfWork.ProductRepository).GetProductByBrandId(brandId);
                if (checkBrandId != null)
                {
                    ((BrandRepository)_unitOfWork.BrandRepository).Update(checkBrandId);
                    checkBrandId.Status = 0;
                    foreach (Product product in products)
                    {
                        product.Status = 0;
                        ((ProductRepository)_unitOfWork.ProductRepository).Update(product);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<BrandGetInforViewModel>> GetAllBrands()
        {
            try
            {
                List<Brand> brands = (List<Brand>)await ((BrandRepository)_unitOfWork.BrandRepository).Get();
                List<BrandGetInforViewModel> result = new List<BrandGetInforViewModel>();
                _mapper.Map(brands, result);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BrandGetInforViewModel> GetBrandDetail(int brandId, int itemsPerPage, int currentPage)
        {
            try
            {
                Brand brand = await ((BrandRepository)_unitOfWork.BrandRepository).GetByID(brandId);
                if (brand == null)
                {
                    throw new Exception("Brand Id does not exist");
                }
                BrandGetInforViewModel result = new BrandGetInforViewModel();
                List<ProductViewModel> productviews = new List<ProductViewModel>();
                IEnumerable<Product> products = new List<Product>();
                products = await ((ProductRepository)_unitOfWork.ProductRepository).Get(x => x.BrandId == brandId,includeProperties: "DescribeProducts,ProductImages");
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
                _mapper.Map(brand, result);
                productviews = productviews.Skip((currentPage - 1) * itemsPerPage).Take(itemsPerPage).ToList();
                result.ProductDetails = productviews;
                return result;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }



        public async Task<bool> UpdateBrand(int brandId, BrandCreatedViewModel brandViewModel)
        {
            try
            {
                List<Product> products = await ((ProductRepository)_unitOfWork.ProductRepository).GetProductByBrandId(brandId);
                Brand checkBrandId = await ((BrandRepository)_unitOfWork.BrandRepository).GetByID(brandId);
                if (checkBrandId == null)
                {
                    throw new Exception("Brand does not exist!");
                }
                _mapper.Map(brandViewModel, checkBrandId);
                if(checkBrandId.Status == 0)
                {
                    foreach (Product product in products)
                    {
                        product.Status = 0;
                        ((ProductRepository)_unitOfWork.ProductRepository).Update(product);
                    }
                    await _unitOfWork.SaveChangesAsync();
                }
                ((BrandRepository)_unitOfWork.BrandRepository).Update(checkBrandId);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #region Get list brands
        public async Task<Tuple<List<BrandViewModel>, int, int?, int?>> GetBrandsAsync(string? keySearchName,
                                                                 string? keySortStatus, string? keySortName,
                                                                 string? keySortTTProduct,
                                                                 int? PAGE_NUMBER, int? PAGE_SIZE)
        {
            try
            {
                List<BrandViewModel> brandViewModels = new List<BrandViewModel>();
                var brands = await ((BrandRepository)_unitOfWork.BrandRepository).Get(includeProperties: "Products");

                foreach (var brand in brands)
                {
                    BrandViewModel brandViewModel = new BrandViewModel();
                    int? totalProduct = 0;
                    foreach (var product in brand.Products!)
                    {
                        totalProduct += product.TotalProduct;
                    }
                    brandViewModel = _mapper.Map<BrandViewModel>(brand);
                    brandViewModel.TotalProducts = totalProduct;
                    brandViewModels.Add(brandViewModel);
                }

                #region Search
                if (string.IsNullOrWhiteSpace(keySearchName) == false)
                {
                    brandViewModels = brandViewModels.Where(c => c.Name.ToLower().Contains(StringHelper.ConvertToUnSign(keySearchName).Trim().ToLower())).ToList();
                }
                #endregion
                

                #region Validation page
                string errormessage = "";

                if (PAGE_SIZE == null)
                {
                    PAGE_SIZE = 5;
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

                

                #region SortName
                if (keySortName != "" && keySortName != null)
                {
                    if (Enum.IsDefined(typeof(SortName), keySortName))
                    {
                        switch (keySortName)
                        {
                            case nameof(SortName.NAMEASC):
                                brandViewModels = brandViewModels.OrderBy(c => c.Name).ToList();
                                break;
                            case nameof(SortName.NAMEDESC):
                                brandViewModels = brandViewModels.OrderByDescending(c => c.Name).ToList();
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
                if (keySortStatus != "" && keySortStatus != null)
                {
                    if (Enum.IsDefined(typeof(SortStatus), keySortStatus))
                    {
                        switch (keySortStatus)
                        {
                            case nameof(SortStatus.STATUSASC):
                                brandViewModels = brandViewModels.OrderBy(c => c.Status).ToList();
                                break;
                            case nameof(SortStatus.STATUSDESC):
                                brandViewModels = brandViewModels.OrderByDescending(c => c.Status).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }
                }
                #endregion

                #region SortTTProduct
                if (keySortTTProduct != "" && keySortTTProduct != null)
                {
                    if (Enum.IsDefined(typeof(SortTTProduct), keySortTTProduct))
                    {
                        switch (keySortTTProduct)
                        {
                            case nameof(SortTTProduct.TTPRODUCTASC):
                                brandViewModels = brandViewModels.OrderBy(c => c.TotalProducts).ToList();
                                break;
                            case nameof(SortTTProduct.TTPRODUCTDESC):
                                brandViewModels = brandViewModels.OrderByDescending(c => c.TotalProducts).ToList();
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
                int totalRecords = brandViewModels.Count();
                int totalPages = (int)Math.Ceiling((double)((double)totalRecords / PAGE_SIZE));
                #endregion

                #region Paging
                brandViewModels = brandViewModels.Skip((int)((PAGE_NUMBER - 1) * PAGE_SIZE)).Take((int)PAGE_SIZE).ToList();
                #endregion
                return Tuple.Create(brandViewModels, totalPages, PAGE_NUMBER, PAGE_SIZE);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion
    }
}
