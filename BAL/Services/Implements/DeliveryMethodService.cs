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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class DeliveryMethodService : IDeliveryMethodService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public DeliveryMethodService(IUnitOfWork unitOfWork, IMapper mapper)
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
        enum SortPrice
        {
            PRICEASC,
            PRICEDESC
        }
        enum SortDescription
        {
            DESCRIPTIONASC,
            DESCRIPTIONDESC
        }
        enum SortStatus
        {
            STATUSASC,
            STATUSDESC
        }
        #endregion

        #region Get delivery methods
        public async Task<Tuple<List<DeliveryMethodViewModel>, int, int?, int?>> GetDeliveryMethodsAsync(string? keysearch, string? keysortname, string? keysortprice, string? keysortstatus, int? PAGE_SIZE, int? PAGE_NUMBER)
        {
            try
            {
                List<DeliveryMethodViewModel> deliveryMethodViews = new List<DeliveryMethodViewModel>();
                IEnumerable<DeliveryMethod> deliveries = new List<DeliveryMethod>();
                deliveries = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Get();
                #region Search
                if (keysearch != null && keysearch != "")
                {
                    deliveries = deliveries.Where(d => StringHelper.ConvertToUnSign(d.Name).ToLower().Contains(StringHelper.ConvertToUnSign(keysearch).Trim().ToLower())).ToList();
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
                                deliveries = deliveries.OrderBy(d => d.Name).ToList();
                                break;
                            case nameof(SortName.NAMEDESC):
                                deliveries = deliveries.OrderByDescending(d => d.Name).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT NAME NOT VALID!");
                    }

                }
                #endregion

                #region Sort price
                if (keysortprice != null && keysortprice != "")
                {
                    if (Enum.IsDefined(typeof(SortPrice), keysortprice))
                    {
                        switch (keysortprice)
                        {
                            case nameof(SortPrice.PRICEASC):
                                deliveries = deliveries.OrderBy(d => d.Price).ToList();
                                break;
                            case nameof(SortPrice.PRICEDESC):
                                deliveries = deliveries.OrderByDescending(d => d.Price).ToList();
                                break;
                        }
                    }
                    else
                    {
                        throw new Exception("KEY SORT PRICE NOT VALID!");
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
                                deliveries = deliveries.OrderBy(d => d.Status).ToList();
                                break;
                            case nameof(SortStatus.STATUSDESC):
                                deliveries = deliveries.OrderByDescending(d => d.Status).ToList();
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
                int totalRecords = deliveries.Count();
                int totalPages = (int)Math.Ceiling((double)((double)totalRecords / PAGE_SIZE));
                #endregion

                #region Paging
                deliveries = deliveries.Skip((int)((PAGE_NUMBER - 1) * PAGE_SIZE)).Take((int)PAGE_SIZE).ToList();
                #endregion

                return Tuple.Create(_mapper.Map<List<DeliveryMethodViewModel>>(deliveries), totalPages, PAGE_SIZE, PAGE_NUMBER);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region  Get delivery method detail
        public async Task<DeliveryMethodViewModel> GetDeliveryMethodDetailAsync(int id)
        {
            try
            {
                DeliveryMethod delivery = new DeliveryMethod();
                delivery = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).GetByID(id);
                return _mapper.Map<DeliveryMethodViewModel>(delivery);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Insert delivery method
        public async Task<bool> InsertDeliveryMethodAsync(EditDeliveryMethodViewModel deliveryViewModel)
        {
            try
            {
                string errormessage = "";

                IEnumerable<DeliveryMethod> deliveries = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Get();
                deliveries = deliveries.Where(d => d.Name.Trim().ToLower().Equals(deliveryViewModel.Name!.Trim().ToLower()));
                if (deliveries.Count() > 0)
                {
                    errormessage += "Name is already exist!";
                    throw new Exception(errormessage);
                }

                if (errormessage != "")
                {
                    throw new Exception(errormessage);
                }

                DeliveryMethod delivery = new DeliveryMethod();
                delivery = _mapper.Map<DeliveryMethod>(deliveryViewModel);
                delivery.Status = (int)DeliveryMethodEnum.Status.ACTIVE;

                await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Insert(delivery);
                await _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Update delivery method
        public async Task<bool> UpdateDeliveryMethodAsync(int id, EditDeliveryMethodViewModel deliveryView)
        {
            try
            {
                var delivery = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).GetByID(id);
                if (delivery != null)
                {
                    #region Check name exist
                    string errormessage = "";

                    IEnumerable<DeliveryMethod> deliveries = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Get();
                    DeliveryMethod deliveryNameExist = new DeliveryMethod();
                    deliveryNameExist = deliveries.Where(c => c.Name.Trim().ToLower().Equals(deliveryView.Name!.Trim().ToLower())).SingleOrDefault()!;

                    if (deliveryNameExist != null)
                    {
                        if (deliveryNameExist.Id != id)
                        {
                            errormessage += "Name is already exist!";
                        }
                    }


                    if (errormessage != "")
                    {
                        throw new Exception(errormessage);
                    }
                    #endregion

                    delivery.Name = deliveryView.Name!;
                    delivery.Price = (decimal)deliveryView.Price!;
                    delivery.Description = deliveryView.Description!;
                    delivery.Status = (int)deliveryView.Status!;

                    ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Update(delivery);
                    await _unitOfWork.SaveChangesAsync();
                    return true;
                }
                else
                {
                    throw new Exception("Delivery method doesn't not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Delete delivery method
        public async Task<bool> DeleteDeliveryMethodAsync(int id)
        {
            try
            {
                DeliveryMethod delivery = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).GetByID(id);
                if (delivery != null)
                {
                        delivery.Status = 0;
                        ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Update(delivery);
                        await _unitOfWork.SaveChangesAsync();
                        return true;
                }
                else
                {
                    throw new Exception("Delivery method does not exist!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion

        #region Get delivery method for user
        public async Task<List<DeliveryMethodUserViewModel>> GetDeliveryUser()
        {
            try
            {
                IEnumerable<DeliveryMethod> deliveries = new List<DeliveryMethod>();
                deliveries = await ((DeliveryMethodRepository)_unitOfWork.DeliveryMethodRepository).Get();
                return _mapper.Map<List<DeliveryMethodUserViewModel>>(deliveries);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

    }
}
