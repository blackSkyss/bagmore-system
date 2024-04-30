using AutoMapper;
using BAL.Helpers;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Enums;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class CategoryService : ICategoryService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public CategoryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        #region Enum
        enum SortName
        {
            NAMEASC,
            NAMEDESC
        }

        enum SortStatus
        {
            STATUSASC,
            STATUSDESC
        }

        enum SortTTProduct
        {
            TTPRODUCTASC,
            TTPRODUCTDESC,
        }
        #endregion

        #region Get list category
        public async Task<Tuple<List<CategoryViewModel>, int, int?, int?>> GetCategoriesAsync(string? keysearch,
                                                                string? keysortname,
                                                                string? keysortstatus,
                                                                string? keySortTTProduct,
                                                                int? PAGE_SIZE,
                                                                int? PAGE_NUMBER)
        {
            try
            {
                List<CategoryViewModel> categoryviews = new List<CategoryViewModel>();
                IEnumerable<Category> categories = new List<Category>();
                categories = await ((CategoryRepository)_unitOfWork.CategoryRepository).Get(includeProperties: "Products");

                foreach (var category in categories)
                {
                    CategoryViewModel categoryViewModel = new CategoryViewModel();
                    int? totalProduct = 0;
                    foreach (var product in category.Products)
                    {
                        totalProduct += product.TotalProduct;
                    }
                    categoryViewModel = _mapper.Map<CategoryViewModel>(category);
                    categoryViewModel.TotalProducts = totalProduct;
                    categoryviews.Add(categoryViewModel);
                }


                #region Search
                if (string.IsNullOrWhiteSpace(keysearch) == false)
                {
                    categoryviews = categoryviews.Where(c => c.Name.ToLower().Contains(StringHelper.ConvertToUnSign(keysearch).Trim().ToLower())).ToList();
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

                if (PAGE_SIZE <= 0)
                {
                    errormessage += "Invalid PAGE_SIZE";
                }
                if (PAGE_NUMBER <= 0)
                {
                    errormessage += "Invalid PAGE_NUMBER";
                }

                if (errormessage != "")
                {
                    throw new Exception(errormessage);
                }
                #endregion

                

                #region Sort name
                if (keysortname != null && keysortname != "")
                {
                    if (Enum.IsDefined(typeof(SortName), keysortname))
                    {
                        switch (keysortname)
                        {
                            case nameof(SortName.NAMEASC):
                                categoryviews = categoryviews.OrderBy(c => c.Name).ToList();
                                break;
                            case nameof(SortName.NAMEDESC):
                                categoryviews = categoryviews.OrderByDescending(c => c.Name).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NAME NOT VALID");
                    }

                }
                #endregion

                #region Sort status
                if (keysortstatus != null && keysortstatus != "")
                {
                    if (Enum.IsDefined(typeof(SortStatus), keysortstatus))
                    {
                        switch (keysortstatus)
                        {
                            case nameof(SortStatus.STATUSASC):
                                categoryviews = categoryviews.OrderBy(c => c.Status).ToList();
                                break;
                            case nameof(SortStatus.STATUSDESC):
                                categoryviews = categoryviews.OrderByDescending(c => c.Status).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT STATUS NOT VALID");
                    }

                }
                #endregion

                #region Count total page
                int totalRecords = categoryviews.Count();
                int totalPages = (int)Math.Ceiling((double)((double)totalRecords / PAGE_SIZE));
                #endregion

                #region SortTTProduct
                if (keySortTTProduct != "" && keySortTTProduct != null)
                {
                    if (Enum.IsDefined(typeof(SortTTProduct), keySortTTProduct))
                    {
                        switch (keySortTTProduct)
                        {
                            case nameof(SortTTProduct.TTPRODUCTASC):
                                categoryviews = categoryviews.OrderBy(c => c.TotalProducts).ToList();
                                break;
                            case nameof(SortTTProduct.TTPRODUCTDESC):
                                categoryviews = categoryviews.OrderByDescending(c => c.TotalProducts).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
                    }
                }
                #endregion

                #region Paging
                categoryviews = categoryviews.Skip((int)((PAGE_NUMBER - 1) * PAGE_SIZE)).Take((int)PAGE_SIZE).ToList();
                #endregion

                return Tuple.Create(categoryviews, totalPages, PAGE_SIZE, PAGE_NUMBER);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Get categories
        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            try
            {
                IEnumerable<Category> categories = new List<Category>();
                categories = await ((CategoryRepository)_unitOfWork.CategoryRepository).Get();
                return _mapper.Map<List<CategoryViewModel>>(categories.Where(c => c.Status == 1));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region Get categories by keyword
        public async Task<List<SearchCategoryViewModel>> GetCategoryByKeywordAsync(string keyword)
        {
            try
            {
                List<SearchCategoryViewModel> categoryViewModels = new List<SearchCategoryViewModel>();
                var categories = await _unitOfWork.CategoryRepository.GetCategoriesByKeyWord(keyword);


                foreach (var category in categories)
                {
                    SearchCategoryViewModel categoryViewModel = new SearchCategoryViewModel();
                    List<ProductViewModel> productViewModels = new List<ProductViewModel>();
                    categoryViewModel.Id = category.Id;
                    categoryViewModel.Name = category.Name;
                    categoryViewModel.Status = category.Status;

                    foreach (var product in category.Products)
                    {
                        if (product.Status == 1 && product.Name.Contains(keyword.Trim().ToLower()))
                        {
                            ProductViewModel productViewModel = new ProductViewModel();
                            productViewModel.Id = product.Id;
                            productViewModel.Name = product.Name;
                            productViewModel.MinPrice = product.DescribeProducts.Min(d => d.Price);
                            productViewModel.MaxPrice = product.DescribeProducts.Max(d => d.Price);
                            productViewModel.Description = product.Description;
                            productViewModel.source = null;
                            //productViewModel.source = product.ProductImages.FirstOrDefault(i => i.Id == product.Id)!.Source;
                            productViewModels.Add(productViewModel);
                        }
                    }

                    if (productViewModels.Count() == 0)
                    {
                        continue;
                    }

                    categoryViewModel.Products = productViewModels;
                    categoryViewModels.Add(categoryViewModel);
                }

                return categoryViewModels;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region Get category detail
        public async Task<CategoryViewModel> GetCategoryDetailAsync(int id, int? currentPage, int? itemPerPage)
        {
            try
            {
                Category category = new Category();
                /*category = (await ((CategoryRepository)_unitOfWork.CategoryRepository).Get(x => x.Id == id, includeProperties: "Products")).FirstOrDefault();*/
                category = await this._unitOfWork.CategoryRepository.GetCategoryById(id);
                List<Product> products = new List<Product>();
                List<ProductViewModel> productviews = new List<ProductViewModel>();
                foreach (var product in category.Products)
                {
                    products.Add(product);
                }
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
                CategoryViewModel categoryViewModel = _mapper.Map<CategoryViewModel>(category);
                categoryViewModel.Products = productviews;
                categoryViewModel.TotalProducts = categoryViewModel.Products.Count();
                categoryViewModel.Products.Skip((currentPage.Value - 1) * itemPerPage.Value).Take(itemPerPage.Value);
                return categoryViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Insert category
        public async Task<bool> InsertCategoryAsync(EditCategoryViewModel categoryView)
        {
            try
            {
                #region Check name exist
                string errormessage = "";
                IEnumerable<Category> categories = await ((CategoryRepository)_unitOfWork.CategoryRepository).Get();
                categories = categories.Where(c => c.Name.Trim().ToLower().Equals(categoryView.Name!.Trim().ToLower()));

                if (categories.Count() > 0)
                {
                    errormessage += "Name is already exist!";
                }

                if (errormessage != "")
                {
                    throw new Exception(errormessage);
                }
                #endregion

                Category category = new Category();
                category = _mapper.Map<Category>(categoryView);
                category.Status = (int)SupplierEnum.Status.ACTIVE;

                await ((CategoryRepository)_unitOfWork.CategoryRepository).Insert(category);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Update category
        public async Task<bool> UpdateCategoryAsync(int id, EditCategoryViewModel categoryView)
        {
            try
            {
                var category = await ((CategoryRepository)_unitOfWork.CategoryRepository).GetByID(id);
                if (category != null)
                {
                    #region Check name exist
                    string errormessage = "";

                    IEnumerable<Category> categories = await ((CategoryRepository)_unitOfWork.CategoryRepository).Get();
                    Category categoryNameExist = new Category();
                    categoryNameExist = categories.Where(c => c.Name.Trim().ToLower().Equals(categoryView.Name!.Trim().ToLower())).SingleOrDefault()!;

                    if (categoryNameExist != null)
                    {
                        if (categoryNameExist.Id != id)
                        {
                            errormessage += "Name is already exist!";
                        }
                    }


                    if (errormessage != "")
                    {
                        throw new Exception(errormessage);
                    }
                    #endregion

                    category.Name = categoryView.Name!;
                    category.Status = (int)categoryView.Status!;

                    ((CategoryRepository)_unitOfWork.CategoryRepository).Update(category);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Category doesn't not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
        #endregion

        #region Delete category
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                Category category = await ((CategoryRepository)_unitOfWork.CategoryRepository).GetByID(id);
                if (category != null)
                {
                    if (category.Status != 0)
                    {
                        category.Status = 0;
                        ((CategoryRepository)_unitOfWork.CategoryRepository).Update(category);
                        await _unitOfWork.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Category is already deleted!");
                    }
                }
                else
                {
                    throw new Exception("Category doesn't exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

    }
}
