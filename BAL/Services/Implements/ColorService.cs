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
    public class ColorService : IColorService
    {
        private UnitOfWork _unitOfWork;
        private IMapper _mapper;
        public ColorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = (UnitOfWork)unitOfWork;
            this._mapper = mapper;
        }

        #region Get colors
        public async Task<List<ColorViewModel>> GetColorsAsync()
        {
            try
            {
                IEnumerable<Color> colors = new List<Color>();
                colors = await ((ColorRepository)_unitOfWork.ColorRepository).Get();
                return _mapper.Map<List<ColorViewModel>>(colors.Where(c => c.Status == 1));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        #endregion
    }
}
