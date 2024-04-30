using AutoMapper;
using BAL.Models;
using BAL.Services.Interfaces;
using DAL.Entities;
using DAL.Infrastructure;
using DAL.Repositories.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Services.Implements
{
    public class SizeService : ISizeService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public SizeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        #region Get sizes
        public async Task<List<SizeViewModel>> GetSizesAsync()
        {
            try
            {
                IEnumerable<Size> sizes = new List<Size>();
                sizes = await ((SizeRepository)_unitOfWork.SizeRepository).Get();
                return _mapper.Map<List<SizeViewModel>>(sizes.Where(s => s.Status == 1));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}
