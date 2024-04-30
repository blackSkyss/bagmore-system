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
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class SupplierService : ISupplierService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public SupplierService(IUnitOfWork unitOfWork, IMapper mapper)
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
        enum SortEmail
        {
            EMAILASC,
            EMAILDESC
        }
        enum SortPhone
        {
            PHONEASC,
            PHONEDESC
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

        #region Get list supplier
        public async Task<Tuple<List<SupplierViewModel>, int, int?, int?>> GetSuppliersAsync(string? keysearch,
                                                                                            string? keysortname,
                                                                                            string? keysortemail,
                                                                                            string? keysortphone,
                                                                                            string? keySortTTProduct,
                                                                                            string? keysortstatus,
                                                                                            int? PAGE_SIZE,
                                                                                            int? PAGE_NUMBER)
        {
            try
            {
                List<SupplierViewModel> supplierViews = new List<SupplierViewModel>();
                IEnumerable<Supplier> suppliers = new List<Supplier>();
                suppliers = await ((SupplierRepository)_unitOfWork.SupplierRepository).Get(includeProperties: "SuppliedProducts");

                foreach (var supplier in suppliers)
                {
                    SupplierViewModel supplierViewModel = new SupplierViewModel();
                    int totalProduct = 0;
                    foreach (var product in supplier.SuppliedProducts)
                    {
                        totalProduct += 1; 
                    }
                    supplierViewModel = _mapper.Map<SupplierViewModel>(supplier);
                    supplierViewModel.TotalProducts = totalProduct;
                    supplierViews.Add(supplierViewModel);
                }

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

                #region Search
                if (keysearch != null && keysearch != "")
                {
                    supplierViews = supplierViews.Where(s => s.Name.ToLower().Contains(StringHelper.ConvertToUnSign(keysearch).Trim().ToLower())).ToList();
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
                                supplierViews = supplierViews.OrderBy(s => s.Name).ToList();
                                break;
                            case nameof(SortName.NAMEDESC):
                                supplierViews = supplierViews.OrderByDescending(s => s.Name).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NAME NOT VALID!");
                    }

                }
                #endregion

                #region Sort email
                if (keysortemail != null && keysortemail != "")
                {
                    if (Enum.IsDefined(typeof(SortEmail), keysortemail))
                    {
                        switch (keysortemail)
                        {
                            case nameof(SortEmail.EMAILASC):
                                supplierViews = supplierViews.OrderBy(s => s.Email).ToList();
                                break;
                            case nameof(SortEmail.EMAILDESC):
                                supplierViews = supplierViews.OrderByDescending(s => s.Email).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT EMAIL NOT VALID!");
                    }

                }
                #endregion

                #region Sort phone
                if (keysortphone != null && keysortphone != "")
                {
                    if (Enum.IsDefined(typeof(SortPhone), keysortphone))
                    {
                        switch (keysortphone)
                        {
                            case nameof(SortPhone.PHONEASC):
                                supplierViews = supplierViews.OrderBy(s => s.Phone).ToList();
                                break;
                            case nameof(SortPhone.PHONEDESC):
                                supplierViews = supplierViews.OrderByDescending(s => s.Phone).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT PHONE NOT VALID!");
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
                                supplierViews = supplierViews.OrderBy(c => c.TotalProducts).ToList();
                                break;
                            case nameof(SortTTProduct.TTPRODUCTDESC):
                                supplierViews = supplierViews.OrderByDescending(c => c.TotalProducts).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NOT VALID");
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
                                supplierViews = supplierViews.OrderBy(s => s.Status).ToList();
                                break;
                            case nameof(SortStatus.STATUSDESC):
                                supplierViews = supplierViews.OrderByDescending(s => s.Status).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT STATUS NOT VALID!");
                    }

                }
                #endregion

                #region Count toal page
                int totalRecords = supplierViews.Count();
                int totalPages = (int)Math.Ceiling((double)((double)totalRecords / PAGE_SIZE));
                #endregion

                #region Paging
                supplierViews = supplierViews.Skip((int)((PAGE_NUMBER - 1) * PAGE_SIZE)).Take((int)PAGE_SIZE).ToList();
                #endregion

                return Tuple.Create(supplierViews, totalPages, PAGE_SIZE, PAGE_NUMBER);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get supplier detail
        public async Task<SupplierViewModel> GetSupplierDetailAsync(int id)
        {
            try
            {
                Supplier supplier = new Supplier();
                /*supplier = (await ((SupplierRepository)_unitOfWork.SupplierRepository)
                    .Get(x => x.Id == id, includeProperties: "SuppliedProducts")).FirstOrDefault();*/
                supplier = await this._unitOfWork.SupplierRepository.GetSupplierById(id);
                List<Product> products = new List<Product>();
                List<ProductViewModel> productviews = new List<ProductViewModel>();
                foreach (var suppliedProduct in supplier.SuppliedProducts)
                {
                    products.Add(suppliedProduct.Product);
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
                SupplierViewModel supplierViewModel = _mapper.Map<SupplierViewModel>(supplier);
                supplierViewModel.Products = productviews;
                supplierViewModel.TotalProducts = products.Count;
                return supplierViewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Insert supplier
        public async Task<bool> InsertSupplierAsync(EditSupplierViewModel model)
        {
            try
            {
                #region Check name, email and phone exist
                string errormessage = "";
                IEnumerable<Supplier> suppliers = await ((SupplierRepository)_unitOfWork.SupplierRepository).Get();
                IEnumerable<Supplier> nameExist = suppliers.Where(c => c.Name.Trim().ToLower().Equals(model.Name!.Trim().ToLower()));
                IEnumerable<Supplier> emailExist = suppliers.Where(c => c.Email.Trim().ToLower().Equals(model.Email!.Trim().ToLower()));
                IEnumerable<Supplier> phoneExist = suppliers.Where(c => c.Phone.Trim().ToLower().Equals(model.Phone!.Trim().ToLower()));

                if (nameExist.Count() > 0)
                {
                    errormessage += "Name is already exist!";
                }

                if (emailExist.Count() > 0)
                {
                    errormessage += "Email is already exist!";
                }

                if (phoneExist.Count() > 0)
                {
                    errormessage += "Phone is already exist!";
                }

                if (errormessage != "")
                {
                    throw new Exception(errormessage);
                }
                #endregion

                Supplier supplier = new Supplier();
                supplier = _mapper.Map<Supplier>(model);
                supplier.Status = (int)SupplierEnum.Status.ACTIVE;


                await ((SupplierRepository)_unitOfWork.SupplierRepository).Insert(supplier);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Update supplier
        public async Task<bool> UpdateSupplierAsync(int id, EditSupplierViewModel model)
        {
            try
            {
                var supplier = await ((SupplierRepository)_unitOfWork.SupplierRepository).GetByID(id);
                if (supplier != null)
                {
                    #region Check name, email, phone exist
                    string errormessage = "";
                    IEnumerable<Supplier> suppliers = await ((SupplierRepository)_unitOfWork.SupplierRepository).Get();
                    Supplier SupplierNameExist = suppliers.Where(s => s.Name.Trim().ToLower().Equals(model.Name!.Trim().ToLower())).SingleOrDefault()!;
                    Supplier SupplierEmailExist = suppliers.Where(s => s.Email.Trim().ToLower().Equals(model.Email!.Trim().ToLower())).SingleOrDefault()!;
                    Supplier SupplierPhoneExist = suppliers.Where(s => s.Phone.Trim().ToLower().Equals(model.Phone!.Trim().ToLower())).SingleOrDefault()!;

                    if (SupplierNameExist != null)
                    {
                        if (SupplierNameExist.Id != id)
                        {
                            errormessage += "Name is already exist!";
                        }
                    }

                    if (SupplierEmailExist != null)
                    {
                        if (SupplierEmailExist.Id != id)
                        {
                            errormessage += "Email is already exist!";
                        }
                    }

                    if (SupplierPhoneExist != null)
                    {
                        if (SupplierPhoneExist.Id != id)
                        {
                            errormessage += "Phone is already exist!";
                        }
                    }

                    if (errormessage != "")
                    {
                        throw new Exception(errormessage);
                    }
                    #endregion

                    supplier.Name = model.Name!;
                    supplier.Email = model.Email!;
                    supplier.Phone = model.Phone!;
                    supplier.Status = (int)model.Status!;

                    ((SupplierRepository)_unitOfWork.SupplierRepository).Update(supplier);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Supplier doesn't not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Delete supplier
        public async Task<bool> DeleteSupplierAsync(int id)
        {
            try
            {
                Supplier supplier = await ((SupplierRepository)_unitOfWork.SupplierRepository).GetByID(id);
                if (supplier != null)
                {
                    if (supplier.Status != 0)
                    {
                        supplier.Status = 0;
                        ((SupplierRepository)_unitOfWork.SupplierRepository).Update(supplier);
                        await _unitOfWork.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        throw new Exception("Supplier is already deleted!");
                    }
                }
                else
                {
                    throw new Exception("Supplier doesn't exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        #endregion
        public async Task<List<SupplierViewModel>> GetSuppliers()
        {
            try
            {
                List<Supplier> suppliers = (await ((SupplierRepository)this._unitOfWork.SupplierRepository).Get()).ToList();
                return this._mapper.Map<List<SupplierViewModel>>(suppliers);
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
